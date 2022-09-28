/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jul-07-2020
-- Updated Date:    Jul-09-2020		Added SegmentId Filter
					Jul-10-2020     Added Columns to Select List
					Jul-14-2020     Modified Logic for column [OwnershipValue]
					Sep-21-2020     Modified code to improve the performance
					                1. Declared a variable instead using the function directly
									2. Applied NOLOCK to avoid deadlocks
					Jul-22-2021     Modified nodes validation and to or
-- <Description>:	This Procedure is used to get the Official Delta movements Data based on the input of TicketId Of Type 5(OffcialDelta)</Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetOfficialBalanceMovements]
(
	   @TicketId				INT	   
)
AS
BEGIN

	DECLARE  @TicketStartDate		      DATE
			,@TicketEndDate			      DATE
			,@TicketSegmentId		      INT
			,@PrevLastModifiedDate	      DATE
			,@TodaysDate			      DATE = Admin.udf_GetTrueDate()
			,@GetPreviousLastModifiedDate DATETIME

	IF OBJECT_ID('tempdb..#TempStepOneOfficialDeltaMovements')IS NOT NULL
    DROP TABLE #TempStepOneOfficialDeltaMovements
	
	IF OBJECT_ID('tempdb..#TempStepThreeOfficialDeltaMovements')IS NOT NULL
    DROP TABLE #TempStepThreeOfficialDeltaMovements

	IF OBJECT_ID('tempdb..#TempNodeList')IS NOT NULL
    DROP TABLE #TempNodeList

	IF OBJECT_ID('tempdb..#TempOutputOfficialDeltaMovements')IS NOT NULL
	DROP TABLE #TempOutputOfficialDeltaMovements

	DROP TABLE IF EXISTS #Ticket
	DROP TABLE IF EXISTS #Node

	SELECT CAST([StartDate]	AS DATE) AS StartDate
	      ,CAST([EndDate]   AS DATE) AS EndDate
		  ,[CategoryElementId]
		  ,[Status]
		  ,[TicketId]
	  INTO #Ticket
	  FROM [Admin].[Ticket] WITH (NOLOCK)
	
	SELECT [NodeId]
	      ,[Order]
	  INTO #Node
	  FROM [Admin].[Node]  WITH (NOLOCK)

	SELECT   @TicketStartDate = StartDate
			,@TicketEndDate   = EndDate
			,@TicketSegmentId = CategoryElementId
	FROM #Ticket
	WHERE TicketId = @TicketId

	--;with CTE AS (In Movement table
	--WHERE ScenarioId = 2 AND MovementPeriodTable(MovementPeriodStartDate = @TicketStartDate AND MovementPeriodEndDate = @TicketEnDate onlyDatePart )
	--For EachMovementId Find the latest Record By TransationId temp table
	--Step 1:
	;WITH CteMovementsData
	AS
	(
		SELECT   Mov.[MovementTransactionID]
				,Mov.[MovementId]
				,Mov.[MovementTypeId]
				,Mov.[SourceNodeId]
				,Mov.[DestinationNodeId]
				,Mov.[SourceProductId]
				,Mov.[DestinationProductId]
				,Mov.[SystemId]
				,Mov.[NetStandardVolume]
				,Mov.[OfficialDeltaTicketId]
				,Mov.[OperationalDate]
				,Mov.[SourceProductTypeId]
				,Mov.[DestinationProductTypeId]
				,Mov.[MeasurementUnit]
				,Mov.[SegmentId]
				,Mov.SourceSystemId
				,mov.CreatedDate AS DateValue
			    ,ROW_NUMBER()OVER(PARTITION BY Mov.MovementId
								  ORDER     BY Mov.MovementTransactionId DESC)Rnum
		FROM [Admin].[view_MovementInformation] Mov  
		INNER JOIN Offchain.MovementPeriod MovPeriod  WITH (NOLOCK)
		ON Mov.[MovementTransactionId] = MovPeriod.[MovementTransactionId]
		WHERE Mov.ScenarioId = 2
		AND Mov.SegmentId = @TicketSegmentId
		AND CAST(MovPeriod.[StartTime]AS DATE) = @TicketStartDate 
		AND CAST(MovPeriod.[EndTime]  AS DATE) = @TicketEndDate  
	)
	SELECT	 Mov.[MovementTransactionID]
			,Mov.[MovementId]
			,Mov.[MovementTypeId]
			,Mov.[SourceNodeId]
			,Mov.[DestinationNodeId]
			,Mov.[SourceProductId]
			,Mov.[DestinationProductId]
			,Mov.[SystemId]
			,Mov.[NetStandardVolume]
			,Mov.[OfficialDeltaTicketId]
			,Mov.[OperationalDate]
			,Mov.[DateValue]
			,Mov.[SourceProductTypeId]
			,Mov.[DestinationProductTypeId]
			,Mov.[MeasurementUnit]
			,Mov.[SegmentId]
			,Mov.[SourceSystemId]
	INTO #TempStepOneOfficialDeltaMovements 
	FROM CteMovementsData Mov
	WHERE Rnum = 1
	AND [NetStandardVolume] > 0 


	--Step 2-->remove all the records from temp which  have Delta TicketID
	DELETE s1
	FROM #TempStepOneOfficialDeltaMovements s1
	LEFT JOIN [Admin].[Ticket] t
	ON s1.[OfficialDeltaTicketId] = t.[TicketId]
	WHERE s1.[OfficialDeltaTicketId] IS NOT NULL
	AND t.[Status] NOT IN (1, 2)


	--Step 3: System,SourceNodeId,DestinationNodeId and Source destination ProductID, MovementTypeID 
	--Pick the latest record Based on MovementTranSactionID Into Temp table
	;WITH CTEStep3
	AS
	(
		SELECT 	 Mov.[MovementTransactionID]
			    ,Mov.[MovementId]
			    ,Mov.[MovementTypeId]
			    ,Mov.[SourceNodeId]
			    ,Mov.[DestinationNodeId]
			    ,Mov.[SourceProductId]
			    ,Mov.[DestinationProductId]
			    ,Mov.[SystemId]
				,Mov.[OperationalDate]
				,Mov.[DateValue]
			    ,Mov.[SourceProductTypeId]
			    ,Mov.[DestinationProductTypeId]
			    ,Mov.[MeasurementUnit]
			    ,Mov.[SegmentId]
				,Mov.NetStandardVolume
				,Mov.[SourceSystemId]
				,ROW_NUMBER()OVER(PARTITION BY  Mov.[SystemId]
											   ,Mov.[SourceNodeId]
											   ,Mov.[DestinationNodeId]
											   ,Mov.[SourceProductId]
											   ,Mov.[DestinationProductId]
											   ,Mov.[MovementTypeId]
								  ORDER BY Mov.MovementTransactionId DESC)Rnum
		FROM #TempStepOneOfficialDeltaMovements Mov
	)
	SELECT *
	INTO #TempStepThreeOfficialDeltaMovements
	FROM CTEStep3
	WHERE Rnum = 1

	--Step 5 : If Step 4 has Data: based on on Step 4 --(SELECT [Admin].[udf_GetPreviousLastModifiedDate] (@TicketId,@TicketSegmentId,@TicketStartDate,@TicketEndDate))date 
	--remove all the movements 
	--from Step 3 CreationDate is Before or less than step 4 Date(LastModifiedDate) 
	--Else step 3 

      SELECT @GetPreviousLastModifiedDate = [Admin].[udf_GetPreviousLastModifiedDate] (@TicketId
	                                                                                  ,@TicketSegmentId
																					  ,@TicketStartDate
																					  ,@TicketEndDate)

      DELETE 
      FROM #TempStepThreeOfficialDeltaMovements
      WHERE DateValue < (@GetPreviousLastModifiedDate)

	--Step 6 : DeltaNode table JOIN with ticket on ticketID 
	--WHERE Segment = InputTicketsSegmentId Start and endDate Input tickets and Status = 0 from ticket table
	--And Status from DeltaNode table should be Delta--
	--Will give us NodeList from this Step--
	SELECT DNode.NodeId
	  INTO #TempNodeList
	  FROM [Admin].[DeltaNode] DNode  WITH (NOLOCK)
	INNER JOIN #Ticket Tic
	ON Dnode.TicketId = Tic.TicketId
	WHERE Tic.CategoryElementId = @TicketSegmentId
	AND Tic.StartDate = @TicketStartDate
	AND Tic.EndDate   = @TicketEndDate
	AND Dnode.[Status] IN (8,9)--submitted for approval,Approved,Rejected

	--Step 7 : Remove Movements from Step 5 based on if SourceNode or DestinationNode belong to Step 6 NodeList
	DELETE Mov
	FROM #TempStepThreeOfficialDeltaMovements MOV
	INNER JOIN #TempNodeList NodeList
	ON (   Mov.SourceNodeId = NodeList.NodeId
	    OR Mov.DestinationNodeId = NodeList.NodeId
		)

	--,Mov.[SourceNodeSystem]
	--Step 8: Result 
	SELECT  Mov.[MovementTransactionID]										AS [MovementTransactionID]
		   ,Mov.[MovementId]												AS [MovementId]
		   ,Mov.[MovementTypeId]											AS [MovementTypeId]
		   ,Mov.[SourceNodeId]												AS [SourceNodeId]
		   ,[Admin].[udf_GetNodeElementID](Mov.SourceNodeId,2)				AS [SourceNodeSegmentID]
		   ,SrcNd.[Order]													AS [SourceNodeOrder]
		   ,[Admin].[udf_GetNodeElementID](Mov.SourceNodeId,8)				AS [SourceNodeSystem]
		   ,Mov.[DestinationNodeId]											AS [DestinationNodeId]
		   ,[Admin].[udf_GetNodeElementID](Mov.DestinationNodeId,2)			AS [DestinationNodeSegmentID]
		   ,DestNd.[Order]													AS [DestinationNodeOrder]
		   ,[Admin].[udf_GetNodeElementID](Mov.DestinationNodeId,8)			AS [DestinationNodeSystem]
		   ,Mov.[SourceProductId]											AS [SourceProductId]
		   ,Mov.[DestinationProductId]										AS [DestinationProductId]
		   ,Mov.[SystemId]													AS [SystemId]
		   ,Mov.[OperationalDate]											AS [OperationalDate]
		   ,Mov.[SourceProductTypeId]										AS [SourceProductTypeId]
		   ,Mov.[DestinationProductTypeId]									AS [DestinationProductTypeId]
		   ,Mov.[MeasurementUnit]											AS [MeasurementUnit]
		   ,Mov.[SegmentId]													AS [SegmentId]
		   ,Mov.[SourceSystemId]											AS [SourceSystemId]
		   ,Mp.StartTime													AS [StartDate]
		   ,Mp.EndTime														AS [EndDate]
		   ,Own.[OwnerId]													AS [OwnerId]
		   ,CASE WHEN Own.OwnershipValueUnit = '%'
				 THEN (Own.[OwnershipValue]*Mov.NetStandardVolume)/100	
				 ELSE Own.[OwnershipValue]								END AS [OwnershipValue]
	INTO #TempOutputOfficialDeltaMovements
	FROM #TempStepThreeOfficialDeltaMovements MOV
	LEFT JOIN Admin.Node SrcNd
	ON Mov.SourceNodeId = SrcNd.NodeId
	LEFT JOIN Admin.Node DestNd
	ON Mov.DestinationNodeId = DestNd.NodeId
	INNER JOIN Offchain.[Owner] Own
	ON Mov.MovementTransactionId = Own.MovementTransactionId
	INNER JOIN Offchain.[MovementPeriod] Mp
	ON Mov.MovementTransactionId = Mp.MovementTransactionId

	SELECT  [MovementTransactionID]
		   ,[MovementId]
		   ,[MovementTypeId]
		   ,[SourceNodeId]
		   ,[SourceNodeSegmentID]
		   ,[SourceNodeOrder]
		   ,[SourceNodeSystem]
		   ,[DestinationNodeId]
		   ,[DestinationNodeSegmentID]
		   ,[DestinationNodeOrder]
		   ,[DestinationNodeSystem]
		   ,[SourceProductId]
		   ,[DestinationProductId]
		   ,[SystemId]
		   ,[OperationalDate]
		   ,[SourceProductTypeId]
		   ,[DestinationProductTypeId]
		   ,[MeasurementUnit]
		   ,[SegmentId]
		   ,[StartDate]
		   ,[EndDate]
		   ,[OwnerId]
		   ,[OwnershipValue]
	FROM #TempOutputOfficialDeltaMovements
	WHERE SourceNodeSegmentID IS NOT NULL OR DestinationNodeSegmentID IS NOT NULL
	AND [SourceNodeSystem] IS NOT NULL OR DestinationNodeSystem IS NOT NULL
	AND [SourceSystemId] NOT IN (189, 190)

END

GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Official Delta movements Data based on the input of TicketId Of Type 5(OffcialDelta)',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetOfficialBalanceMovements',
							@level2type = NULL,
							@level2name = NULL