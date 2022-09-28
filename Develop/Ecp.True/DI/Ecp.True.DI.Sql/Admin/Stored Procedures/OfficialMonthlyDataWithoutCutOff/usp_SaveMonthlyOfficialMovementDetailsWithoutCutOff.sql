/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jul-10-2020
-- Modified date:	Jul-14-2020 Renamed table OfficialMonthlyMovementDetails to OfficialMonthlyMovementDetails
-- Modified date:	Aug-11-2020 Removed un required variables
-- Modified date: 	Sep-18-2020 Changing Percentage Calculation
-- Modified date: 	Oct-01-2020 To make consistent with other SP's changing @TodaysDate datatype to DATE
-- Description:     This Procedure is to Get Official movement details based on Element, Node, StartDate, EndDate.
 EXEC [Admin].[usp_SaveMonthlyOfficialMovementDetailsWithoutCutOff] @ElementId=137236,@NodeId=30812,@StartDate='2020-07-03',@EndDate='2020-07-06',@ExecutionId = 'B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF'
 SELECT * FROM [Admin].[OfficialMonthlyMovementDetails] WHERE EXECUTIONID = 'B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF'
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveMonthlyOfficialMovementDetailsWithoutCutOff]
(
                    @ElementId             INT
                   ,@NodeId                INT
                   ,@StartDate             DATE                      
                   ,@EndDate               DATE                      
                   ,@ExecutionId           INT 
)
AS 
BEGIN
  SET NOCOUNT ON

		IF OBJECT_ID('tempdb..#MovTempIntialNodes')IS NOT NULL
		DROP TABLE #MovTempIntialNodes

		IF OBJECT_ID('tempdb..#MovTempFinalNodes')IS NOT NULL
		DROP TABLE #MovTempFinalNodes

		IF OBJECT_ID('tempdb..#MovTempNodeTag')IS NOT NULL
		DROP TABLE #MovTempNodeTag

		IF OBJECT_ID('tempdb..#MovTempNodeTagCalculationDate')IS NOT NULL
		DROP TABLE #MovTempNodeTagCalculationDate

		IF OBJECT_ID('tempdb..#TempDistinctNodes')IS NOT NULL
		DROP TABLE #TempDistinctNodes		

		IF OBJECT_ID('tempdb..#MovTempProducts')IS NOT NULL
		DROP TABLE #MovTempProducts

		IF OBJECT_ID('tempdb..#MovTempFinalResult')IS NOT NULL
		DROP TABLE #MovTempFinalResult

		IF OBJECT_ID('tempdb..#TempOfficialMovement')IS NOT NULL
		DROP TABLE #TempOfficialMovement

		
     -- Variables Declaration
    	DECLARE @Previousdate  DATETIME =  [Admin].[udf_GetTrueDate] ()-1,
		        @Todaysdate	   DATE     =  [Admin].[udf_GetTrueDate] (),
				@NoOfDays	   INT,
                @ElementName   NVARCHAR (150)

		SELECT @NoOfDays = DATEDIFF(DAY,@StartDate,@EndDate)

		SET @ElementName = (SELECT [Name] AS ElementName 
		                     FROM [Admin].[CategoryElement] 
		                    WHERE ElementId = @ElementId		
							)
    
			CREATE TABLE #MovTempIntialNodes
			(
				SegmentId			INT,
				InitialNodeId		INT,
				NodeName			NVARCHAR(250),
				CalculationDate		DATE
			)

			CREATE TABLE #MovTempFinalNodes
			(
				SegmentId			INT,
				FinalNodeId			INT,
				NodeName			NVARCHAR(250),
				CalculationDate		DATE
			)

						
			INSERT INTO #MovTempIntialNodes
			(
				 SegmentId	  
				,InitialNodeId			
				,NodeName		
				,CalculationDate
			)
			EXEC [Admin].[usp_GetInitialNodes] @ElementId,
											   @StartDate,
											   @EndDate

			INSERT INTO #MovTempFinalNodes
			(
				 SegmentId	  
				,FinalNodeId			
				,NodeName		
				,CalculationDate
			)
			EXEC [Admin].[usp_GetFinalNodes] @ElementId,
											 @StartDate,
											 @EndDate

			SELECT * INTO #MovTempNodeTag
			  FROM (
					SELECT DISTINCT NT.NodeId,
									NT.ElementId,
									Ce.Name AS ElementName,
									CASE WHEN NT.StartDate < @StartDate 
									     THEN @StartDate
									     ELSE NT.StartDate END
									       AS StartDate,
									CASE WHEN (   CAST(NT.EndDate AS DATE) > (CAST(GETDATE() AS DATE)) 
									           OR CAST(NT.EndDate AS DATE) > (DATEADD(DAY,@NoOfDays,@StartDate))
											  )
										 THEN @EndDate
										 ELSE NT.EndDate 
										 END AS EndDate 
					FROM Admin.NodeTag NT
					INNER JOIN Admin.CategoryElement CE
					ON CE.ElementId = NT.ElementId
					WHERE NT.ElementId = @ElementId
					AND (NT.NodeId = @NodeId)
					) A                   	
		
			SELECT  NT.NodeId
				   ,NT.ElementId 
				   ,NT.ElementName
				   ,Ca.CalculationDate 
			INTO #MovTempNodeTagCalculationDate
			FROM #MovTempNodeTag NT
			CROSS APPLY (SELECT	dates	AS  CalculationDate
						 FROM [Admin].[udf_GetAllDates]( NT.StartDate, 
												         NT.EndDate, 
												         NT.NodeId, 
												         NT.ElementId)
						) CA
			WHERE CA.CalculationDate BETWEEN @StartDate AND @EndDate
		 
		 -- Capturing Movements With Products in a way to calculate Inputs & Outputs
			SELECT			 SubQ.CalculationDate
						    ,SubQ.NodeId
							,SubQ.ProductID
							,SubQ.ProductName
							,SubQ.SegmentID
							,SubQ.MovementTypeName
							,SubQ.MeasurementUnit
							,SubQ.MovementTransactionId
							,SubQ.EventType
							,SubQ.Batchid
			INTO #MovTempProducts
			FROM
			(
				SELECT	Mov.OperationalDate				  AS CalculationDate,
						Mov.SourceProductId				  AS ProductID,
						Mov.SourceProductName			  AS ProductName,	
						Mov.SegmentId					  AS SegmentID,						
						Mov.SourceNodeId				  AS NodeId,
						Mov.MovementTypeName		      AS MovementTypeName,
						Mov.MeasurementUnit				  AS MeasurementUnit,
						Mov.MovementTransactionId         AS MovementTransactionId,
						Mov.EventType                     AS EventType,
						Mov.Batchid                       AS Batchid
				FROM  [Admin].[OfficialMovementInformation] Mov
				WHERE Mov.SourceProductId	IS NOT NULL
				  AND Mov.SourceNodeId     = @NodeId
				  AND Mov.ExecutionId      = @ExecutionId
				UNION
				SELECT   Mov.OperationalDate				AS CalculationDate,
						 Mov.DestinationProductId			AS ProductID,
						 Mov.DestinationProductName		    AS ProductName,
						 Mov.SegmentId					    AS SegmentID,						
						 Mov.DestinationNodeId			    AS NodeId,
						 Mov.MovementTypeName		        AS MovementTypeName,
						 Mov.MeasurementUnit				AS MeasurementUnit,
						 Mov.MovementTransactionId          AS MovementTransactionId,
						 Mov.EventType						AS EventType,
						 Mov.Batchid					    AS Batchid
				FROM [Admin].[OfficialMovementInformation] Mov
				WHERE Mov.DestinationProductId	IS NOT NULL
				AND Mov.DestinationNodeId = @NodeId
				AND Mov.ExecutionId       = @ExecutionId
			)SubQ

		 -- Capturing Distinct Node Id's into #TempDistinctNodes Table
			SELECT DISTINCT NodeId 
			INTO #TempDistinctNodes
			FROM #MovTempNodeTagCalculationDate

			SELECT  DISTINCT
			        Mov.MovementID						   AS MovementID
				   ,Mov.MovementTransactionId              AS MovementTransactionId
				   ,Mov.SourceProductID					   AS SourceProductID
				   ,Mov.SourceProductName				   AS SourceProductName
				   ,Mov.DestinationProductId			   AS DestinationProductId
				   ,Mov.DestinationProductName			   AS DestinationProductName
				   ,Mov.SourceNodeId					   AS SourceNodeId
				   ,Mov.SourceNodeName					   AS SourceNodeName
				   ,Mov.DestinationNodeId				   AS DestinationNodeId
				   ,Mov.DestinationNodeName				   AS DestinationNodeName
				   ,Mov.[Version]                          AS [Version]
				   ,DQuery.MovementTypeName				   AS MovementTypeName
				   ,DQuery.MeasurementUnit				   AS MeasurementUnit
				   ,[Mov].SystemName                       AS SystemName
				   ,[Mov].SourceSystem                     AS SourceSystem				   
				   ,ISNULL(Mov.NetStandardVolume,0.00)	   AS NetStandardVolume
				   ,ISNULL(Mov.GrossStandardVolume,0.00)   AS GrossStandardVolume
				   ,CASE
						WHEN Mov.MessageTypeId IN (1,3)
						AND ISNULL(Mov.DestinationNodeId,0) = @NodeId
				        AND ISNULL(Mov.SourceNodeId,0) != @NodeId
					   THEN  'Entradas'
					   WHEN Mov.MessageTypeId IN (1,3)
						 AND ISNULL(Mov.SourceNodeId,0) = @NodeId
					     AND ISNULL(Mov.DestinationNodeId,0) != @NodeId
                       THEN  'Salidas'
					   WHEN  Mov.MessageTypeId = 2 --Loss
						AND  ISNULL(Mov.SourceNodeId,0) = @NodeId
						AND  ISNULL(Mov.DestinationNodeId,0) != @NodeId
					   THEN  Mov.[Classification]
					ELSE Mov.[Classification]
					END AS [Movement]
				   ,Own.[Name] AS [Owner]
				   ,CASE WHEN CHARINDEX('%',OwnershipValueUnit) > 0
			          THEN (Mov.NetStandardVolume * O.OwnershipValue) /100
			          ELSE O.OwnershipValue
			          END AS OwnershipVolume
                   ,CASE WHEN Mov.NetStandardVolume = 0
				         THEN 0 
						 WHEN CHARINDEX('%',OwnershipValueUnit) > 0
			             THEN O.OwnershipValue
			             ELSE (O.OwnershipValue / Mov.NetStandardVolume) * 100	 
			        END AS OwnershipPercentage
				   ,DQuery.ProductID					AS ProductID
				   ,DQuery.SegmentID					AS SegmentID
				   ,DQuery.CalculationDate				AS RegistrationDate
				   ,@ElementName						AS SegmentName 
				   ,DQuery.NodeId						AS NodeId
				   ,@ExecutionId                        AS ExecutionId
				   ,'ReportUser'                        AS CreatedBy	
            INTO #MovTempFinalResult
			FROM #MovTempProducts DQuery
			INNER JOIN [Admin].[OfficialMovementInformation] Mov
			ON Mov.MovementTransactionId = DQuery.MovementTransactionId
			AND Mov.ExecutionId    = @ExecutionId
			INNER JOIN [Offchain].[Owner] O
            ON  Mov.MovementTransactionId = O.MovementTransactionId
			INNER JOIN [Admin].[CategoryElement] Own
            ON Own.ElementId = O.OwnerId
			LEFT JOIN #MovTempNodeTagCalculationDate NTDate
			ON  NTDate.ElementId = DQuery.SegmentID
			AND NTDate.CalculationDate = DQuery.CalculationDate
			

            SELECT 	 DISTINCT
			         [MovementId]
                    ,[MovementTransactionId]
					,[MovementTypeName]
					,[SourceNodeName] 
					,[DestinationNodeName]
					,[Version]
					,[SourceProductName]
					,[SourceProductID]
					,[DestinationProductName]
					,[DestinationProductId]
					,[NetStandardVolume]
					,[GrossStandardVolume]
					,[MeasurementUnit]
					,[SystemName]
					,[SourceSystem]
					,[Movement]
					,[Owner]
					,[OwnershipVolume]
					,[OwnershipPercentage]
					,[RegistrationDate]
					,@ExecutionId AS ExecutionId
					,'ReportUser' AS CreatedBy
					,@Todaysdate  AS [CreatedDate]
			INTO #TempOfficialMovement
			FROM #MovTempFinalResult DQuery	


             INSERT INTO [Admin].[OfficialMonthlyMovementDetails]
                          (
                             [RNo]                   
                            ,[System]                
                            ,[Version]               
                            ,[MovementId]            
                            ,[TypeMovement]          
                            ,[Movement]              
                            ,[SourceNode]            
                            ,[DestinationNode]       
                            ,[SourceProduct]         
                            ,[DestinationProduct]    
                            ,[NetQuantity]           
                            ,[GrossQuantity]         
                            ,[MeasurementUnit]       
                            ,[Owner]                 
                            ,[Ownershipvolume]       
                            ,[Ownershippercentage]   
                            ,[Origin]                
                            ,[RegistrationDate]      
                            ,[MovementTransactionId]           
                            ,[ExecutionId]           
                            ,[CreatedBy]             
                            ,[CreatedDate]           
						   ) 
		 SELECT ROW_NUMBER() OVER (ORDER BY [System],[Version],[DestinationNode],[SourceNode],[TypeMovement]) AS RNo
		       ,* 
		   FROM (
				 SELECT      --DISTINCT
					         [SystemName]                 AS [System]
                            ,[Version]                    AS [Version] 
							,[MovementId]                 AS [MovementId]
							,[MovementTypeName]           AS [TypeMovement]
							,[Movement]                   AS [Movement]
							,[SourceNodeName]             AS [SourceNode]
							,[DestinationNodeName]        AS [DestinationNode]
							,[SourceProductName]          AS [SourceProduct]
							,[DestinationProductName]     AS [DestinationProduct]
							,[NetStandardVolume]          AS [NetQuantity]
							,[GrossStandardVolume]        AS [GrossQuantity]
							,[MeasurementUnit]            AS [MeasurementUnit]
							,[Owner]                      AS [Owner]
							,[OwnershipVolume]            AS [Ownershipvolume]     
                            ,[OwnershipPercentage]        AS [OwnershipPercentage]
							,[SourceSystem]               AS [Origin]                
                            ,[RegistrationDate]           AS [RegistrationDate]
							,[MovementTransactionId]      AS [MovementTransactionId]  
							,[ExecutionId]				  AS [ExecutionId]   
							,Mov.[CreatedBy]			  AS [CreatedBy]     
							,@Todaysdate				  AS [CreatedDate]   
				    FROM #TempOfficialMovement Mov
				    INNER JOIN [Admin].[Product] Prd
				    ON (   Mov.SourceProductId = Prd.ProductId
				        OR Mov.DestinationProductId = Prd.ProductId
				       )
                )SubQ
END  

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to feed the OfficialMovement table',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveMonthlyOfficialMovementDetailsWithoutCutOff',
    @level2type = NULL,
    @level2name = NULL