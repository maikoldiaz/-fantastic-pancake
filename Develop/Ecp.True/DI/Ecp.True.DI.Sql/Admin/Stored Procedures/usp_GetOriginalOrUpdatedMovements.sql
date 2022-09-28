/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jun-11-2020
-- Updated Date:	Jun-16-2020 Modified the Logic To that can be consumed for PBI(6379,6381),Added delta TicketID Logic and movementType Id
--					Jun-17-2020	Added Extra Columns for 6381 PBI
--					Jun-18-2020 Modified the Code to bring Description of  CancellationType And Modified Delta Ticket Logic
								Modified CancellationType Join Type
--					Jun-19-2020	Modified Code for the ticketID Condition
--                  July-1-2020 Added UPPER function for EventType
                    Sept-22-2020 : As part of bug fix 80491, we are checking for TicketId and OwnershipTicketId for updated movements. 
					We can move this check only OwnershipTicketId later (CR)
-- <Description>:	This Procedure is used to get the Original Movements on the input of TicketId. </Description>
					@Is_Original -->0-->Updated--Actual--1-->Original 
EXEC [Admin].[usp_GetOriginalORUpdatedMovements] 65637,'2020-04-30','2020-06-30',0
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetOriginalOrUpdatedMovements]
(
	     @SegmentId				INT
		,@StartDate				DATE
		,@EndDate				DATE
	    ,@IsOriginal			BIT-->0-->Updated--Actual--1-->Original 
)
AS
BEGIN

		IF OBJECT_ID('tempdb..#TempSegmentMovements')IS NOT NULL
		DROP TABLE #TempSegmentMovements

		IF OBJECT_ID('tempdb..#TempMovementsDetails')IS NOT NULL
		DROP TABLE #TempMovementsDetails

		IF OBJECT_ID('tempdb..#TempNewMovements')IS NOT NULL
		DROP TABLE #TempNewMovements

		IF OBJECT_ID('tempdb..#TempOriginalUpdatedMovementDetails')IS NOT NULL
		DROP TABLE #TempOriginalUpdatedMovementDetails

		CREATE TABLE #TempOriginalUpdatedMovementDetails
		(
			 MovementTransactionId		INT
			,MovementId					VARCHAR(50) collate SQL_Latin1_General_CP1_CS_AS
			,MovementTypeId				NVARCHAR (150)	
			,MovementTypeName			NVARCHAR(150)
			,SourceNodeName				NVARCHAR(150)
			,DestinationNodeName		NVARCHAR(150)
			,SourceProductName			NVARCHAR(150)
			,DestinationProductName		NVARCHAR(150)
			,NetStandardVolume			DECIMAL(18,2)
			,MeasurementUnit			NVARCHAR(150)
			,OperationalDate			DATETIME
			,GlobalMovementId			NVARCHAR(50) collate SQL_Latin1_General_CP1_CS_AS
			,TicketId					INT
			,OwnershipTicketId		    INT
			,EventType					NVARCHAR(25)
			,CancellationType			NVARCHAR(150)
		)
		
		--MovementDetails based on Ticket(Segment, StartDate & EndDate)
		SELECT   Mov.MovementTransactionId
				,Mov.MovementId
				,Mov.MovementTypeId
				,Mov.MovementTypeName
				,Mov.SourceNodeName
				,Mov.DestinationNodeName
				,Mov.SourceProductName
				,Mov.DestinationProductName
				,Mov.NetStandardVolume
				,Mov.MeasurementUnit
				,Mov.OperationalDate
				,Mov.GlobalMovementId 
				,Mov.TicketId
				,Mov.OwnershipTicketId
				,Mov.EventType
				,Mov.DeltaTicketId
				,Ann.AnnulationMovementTypeId AS CancellationType
		INTO #TempSegmentMovements
		FROM Admin.view_MovementInformation Mov	
		LEFT JOIN (SELECT * FROM [Admin].[Annulation] WHERE IsActive = 1) Ann
		ON Mov.MovementTypeId = Ann.SourceMovementTypeId
		WHERE Mov.SegmentId = @SegmentId
		AND (Mov.OperationalDate BETWEEN @StartDate AND @EndDate) AND Mov.ScenarioId = 1
		AND EXISTS (SELECT 1
					FROM Admin.view_MovementInformation MovIn
					WHERE MovIn.MovementID = Mov.MovementID
					AND MovIn.DeltaTicketId IS NULL)

		SELECT M1.*
		INTO #TempMovementsDetails
		FROM #TempSegmentMovements M1
		WHERE NOT EXISTS ( SELECT 1 
						   FROM #TempSegmentMovements M2 
						   WHERE M1.MovementId = M2.MovementId 
						   AND M2.GlobalMovementId IS NOT NULL)
		AND   EXISTS     ( SELECT 1 
						   FROM #TempSegmentMovements M3 
						   WHERE M1.MovementId = M3.MovementId 
						   AND M3.TicketId IS NOT NULL)--TicketId it might
		AND   EXISTS     ( SELECT 1 
						   FROM #TempSegmentMovements M4 
						   WHERE M1.MovementId = M4.MovementId 
						   AND M4.TicketId IS NULL) --TicketId it might

		IF @IsOriginal = 1
		BEGIN
			--OriginalMovements
			;WITH CTE
			AS
			(
				SELECT	 M1.MovementTransactionId
						,M1.MovementId
						,M1.MovementTypeId
						,M1.MovementTypeName
						,M1.SourceNodeName
						,M1.DestinationNodeName
						,M1.SourceProductName
						,M1.DestinationProductName
						,M1.NetStandardVolume
						,M1.MeasurementUnit
						,M1.OperationalDate
						,M1.GlobalMovementId 
						,M1.TicketId
						,M1.EventType
						,M1.CancellationType
						,M1.OwnershipTicketId
						,M1.DeltaTicketId
					    ,ROW_NUMBER()OVER(PARTITION BY M1.MovementId 
										  ORDER BY M1.MovementTransactionId DESC) AS RNUM
				FROM #TempMovementsDetails M1
			)
			SELECT  MovementTransactionId
				   ,MovementId
				   ,MovementTypeId
				   ,MovementTypeName
				   ,SourceNodeName
				   ,DestinationNodeName
				   ,SourceProductName
				   ,DestinationProductName
				   ,NetStandardVolume
				   ,MeasurementUnit
				   ,OperationalDate
				   ,GlobalMovementId 
				   ,TicketId
				   ,EventType
				   ,CancellationType
			INTO #TempMovementsWithUpdatedDetails
			FROM CTE
			WHERE RNUM = 1 AND TicketId IS NULL AND OwnershipTicketId IS NULL AND DeltaTicketId IS NULL


			;WITH CTE
			AS
			(
				SELECT  tmpMov.MovementTransactionId
				   ,tmpMov.MovementId
				   ,tmpMov.MovementTypeId
				   ,tmpMov.MovementTypeName
				   ,tmpMov.SourceNodeName
				   ,tmpMov.DestinationNodeName
				   ,tmpMov.SourceProductName
				   ,tmpMov.DestinationProductName
				   ,tmpMov.NetStandardVolume
				   ,tmpMov.MeasurementUnit
				   ,tmpMov.OperationalDate
				   ,tmpMov.GlobalMovementId 
				   ,tmpMov.TicketId
				   ,tmpMov.EventType
				   ,tmpMov.CancellationType
				   ,ROW_NUMBER()OVER(PARTITION BY tmpMov.MovementId 
										  ORDER BY tmpMov.MovementTransactionId DESC) AS RNUM
					FROM #TempMovementsDetails tmpMov INNER JOIN  #TempMovementsWithUpdatedDetails tmpUp
					ON tmpMov.MovementId = tmpUp.MovementId
					AND tmpMov.TicketId IS NOT NULL
			)
			INSERT INTO #TempOriginalUpdatedMovementDetails
			(
				    MovementTransactionId
				   ,MovementId
				   ,MovementTypeId
				   ,MovementTypeName
				   ,SourceNodeName
				   ,DestinationNodeName
				   ,SourceProductName
				   ,DestinationProductName
				   ,NetStandardVolume
				   ,MeasurementUnit
				   ,OperationalDate
				   ,GlobalMovementId 
				   ,TicketId
				   ,EventType
				   ,CancellationType
			)
			SELECT  MovementTransactionId
				   ,MovementId
				   ,MovementTypeId
				   ,MovementTypeName
				   ,SourceNodeName
				   ,DestinationNodeName
				   ,SourceProductName
				   ,DestinationProductName
				   ,NetStandardVolume
				   ,MeasurementUnit
				   ,OperationalDate
				   ,GlobalMovementId 
				   ,TicketId
				   ,EventType
				   ,CancellationType
			FROM CTE
			WHERE RNUM = 1 AND EventType <> 'DELETE'
		END
		ELSE
		--UpdatedMovements Logic
		BEGIN

			--New MovementDetails
			;WITH CTE
			AS
			(
			SELECT * ,
			ROW_NUMBER()OVER( PARTITION BY M1.MovementId 
							  ORDER BY MovementTransactionId DESC) AS RNUM
			FROM #TempSegmentMovements M1 
			WHERE NOT EXISTS ( SELECT 1 
							   FROM #TempSegmentMovements M2 
							   WHERE m1.MovementId = M2.MovementId 
							   AND M2.GlobalMovementId IS NOT NULL)
			AND NOT EXISTS ( SELECT 1 
							 FROM #TempSegmentMovements m3 
							 WHERE m1.MovementId = m3.MovementId 
							 AND m3.TicketId IS NOT NULL) 
			AND NOT EXISTS ( SELECT 1 
							 FROM #TempSegmentMovements m4 
							 WHERE m1.MovementId = m4.MovementId 
							 AND m4.DeltaTicketId IS NOT NULL)
			AND NOT EXISTS ( SELECT 1 
							 FROM #TempSegmentMovements m5 
							 WHERE m1.MovementId = m5.MovementId 
							 AND m5.OwnershipTicketId IS NOT NULL)
			)
			SELECT   Mov.MovementTransactionId
					,Mov.MovementId
					,Mov.MovementTypeId
					,Mov.MovementTypeName
					,Mov.SourceNodeName
					,Mov.DestinationNodeName
					,Mov.SourceProductName
					,Mov.DestinationProductName
					,Mov.NetStandardVolume
					,Mov.MeasurementUnit
					,Mov.OperationalDate
					,Mov.GlobalMovementId 
					,Mov.TicketId
					,Mov.EventType	
				    ,Mov.CancellationType
			INTO #TempNewMovements
			FROM CTE Mov
			WHERE RNUM = 1 
			AND EventType <> 'DELETE'

			;WITH CTE
			AS
			(
				SELECT	 M1.MovementTransactionId
						,M1.MovementId
						,M1.MovementTypeId
						,M1.MovementTypeName
						,M1.SourceNodeName
						,M1.DestinationNodeName
						,M1.SourceProductName
						,M1.DestinationProductName
						,M1.NetStandardVolume
						,M1.MeasurementUnit
						,M1.OperationalDate
						,M1.GlobalMovementId 
						,M1.TicketId
						,M1.EventType
						,M1.CancellationType
						,M1.DeltaTicketId
						,M1.OwnershipTicketId
					    ,ROW_NUMBER()OVER(PARTITION BY m1.MovementId 
										  ORDER BY M1.MovementTransactionId DESC) AS RNUM
				FROM #TempMovementsDetails M1
			)
			INSERT INTO #TempOriginalUpdatedMovementDetails
			(
				    MovementTransactionId
				   ,MovementId
				   ,MovementTypeId
				   ,MovementTypeName
				   ,SourceNodeName
				   ,DestinationNodeName
				   ,SourceProductName
				   ,DestinationProductName
				   ,NetStandardVolume
				   ,MeasurementUnit
				   ,OperationalDate
				   ,GlobalMovementId 
				   ,TicketId
				   ,EventType
				   ,CancellationType
			)
			SELECT  MovementTransactionId
				   ,MovementId
				   ,MovementTypeId
				   ,MovementTypeName
				   ,SourceNodeName
				   ,DestinationNodeName
				   ,SourceProductName
				   ,DestinationProductName
				   ,NetStandardVolume
				   ,MeasurementUnit
				   ,OperationalDate
				   ,GlobalMovementId 
				   ,TicketId
				   ,EventType
				   ,CancellationType
			FROM CTE
			WHERE RNUM = 1 AND TicketId IS NULL AND OwnershipTicketId IS NULL AND DeltaTicketId IS NULL
			UNION
			SELECT  MovementTransactionId
				   ,MovementId
				   ,MovementTypeId
				   ,MovementTypeName
				   ,SourceNodeName
				   ,DestinationNodeName
				   ,SourceProductName
				   ,DestinationProductName
				   ,NetStandardVolume
				   ,MeasurementUnit
				   ,OperationalDate
				   ,GlobalMovementId 
				   ,TicketId
				   ,EventType
				   ,CancellationType
			FROM #TempNewMovements
		END

		SELECT Mov.MovementTransactionId			AS	MovementTransactionId	
			  ,Mov.MovementId						AS 	MovementId	
			  ,Mov.MovementTypeName					AS 	MovementType			
			  ,Mov.SourceNodeName					AS 	SourceNode			
			  ,Mov.DestinationNodeName				AS 	DestinationNode		
			  ,Mov.SourceProductName				AS 	SourceProduct		
			  ,Mov.DestinationProductName			AS 	DestinationProduct	
			  ,ABS(Mov.NetStandardVolume)			AS 	Amount				
			  ,Mov.OperationalDate					AS 	OperationalDate				  
			  ,Ce.Name								AS 	Unit		
			  ,CASE WHEN Mov.EventType = 'Insert'
					THEN 1
					WHEN Mov.EventType = 'Update'
					THEN 2
					WHEN Mov.EventType = 'Delete'
					THEN 3
					WHEN Mov.EventType = 'ReInject'
					THEN 4		END					AS Action			  
			  ,Mov.GlobalMovementId					AS GlobalMovementId			
			  ,Mov.TicketId							AS TicketId
			  ,ABS(Mov.NetStandardVolume)           AS NetStandardVolume
			  ,Mov.EventType						AS EventType			  
			  ,CeCType.Name							AS CancellationType		  
		FROM #TempOriginalUpdatedMovementDetails Mov
		INNER JOIN Admin.CategoryElement CE
		ON Mov.MeasurementUnit = CE.ElementId 
		LEFT JOIN Admin.CategoryElement CeCType
		ON Mov.CancellationType = CeCType.ElementId 
END
GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Original Movements on the input of TicketId',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetOriginalORUpdatedMovements',
							@level2type = NULL,
							@level2name = NULL