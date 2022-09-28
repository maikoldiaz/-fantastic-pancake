/*-- ============================================================================================================================================= 
-- Author:			Microsoft    
-- Created Date:	Nov-22-2019    
-- Updated Date:	Mar-20-2020
--				    Apr-09-2020  -- Removed(BlockchainStatus = 1)   
--					Apr-14-2010 --> Removed OwnershipAnalytics references.
					Apr-22-2020     Replaced Ownership table with owner table
					Jun-05-2020     Filtered Records which don't have OwnerId
					Jun-25-2020		Added Filter TicketId IS NOT NULL For Bug 57068 
--					Sep-10-2020		Pushed the Result temptable and Added DISTINCT As Part of Bug Fix 78166
-- <Description>:   This Procedure is used to get the Input Movement Property details for the Excel file based on the Ticket Id.  </Description>  
-- ===============================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetInputMovementPropertyDetails] 
(	
	@TicketId INT
) 
AS 
  BEGIN 
	
	  IF Object_ID('tempdb..#TempDeterminedInputownershipOfInitialnodes') IS NOT NULL 
      DROP TABLE #TempDeterminedInputownershipOfInitialnodes 

      IF Object_ID('tempdb..#TempNodeTag') IS NOT NULL 
      DROP TABLE #TempNodeTag 

      IF Object_ID('tempdb..#TempMovementDetails') IS NOT NULL 
      DROP TABLE #TempMovementDetails 

      IF Object_ID('tempdb..#TempPreResultNoOwnerShipDefined') IS NOT NULL 
      DROP TABLE #TempPreResultNoOwnerShipDefined

      IF Object_ID('tempdb..#TempResultDetails') IS NOT NULL 
	  DROP TABLE #TempResultDetails

      DECLARE @SegmentId INT 
      DECLARE @StartDate DATETIME 
      DECLARE @EndDate DATETIME,
			  @CutOffTicketId INT

      SET @SegmentId= (SELECT CategoryElementId 
                       FROM   [Admin].[Ticket] 
                       WHERE  TicketId = @TicketId) 
      SET @StartDate= (SELECT CAST(StartDate  AS DATE)
                       FROM   [Admin].[Ticket] 
                       WHERE  TicketId = @TicketId) 
      SET @EndDate= (SELECT CAST(EndDate  AS DATE)
                     FROM   [Admin].[Ticket] 
                     WHERE  TicketId = @TicketId)

		--Get the associated CutOffTicketID
		SELECT TOP 1 @CutOffTicketId = TicketId
		FROM Admin.Ticket
		WHERE TicketTypeId = 1
		AND Status = 0 
		AND @StartDate BETWEEN StartDate AND EndDate
		AND CategoryElementId = @SegmentId
		ORDER By TicketId DESC

	    SELECT NT.ElementId
			  ,NT.NodeId
			  ,CAST(NT.StartDate	AS DATE) AS StartDate
			  ,CAST(NT.EndDate		AS DATE) AS EndDate
		INTO #TempNodeTag
		FROM Admin.NodeTag NT
		WHERE NT.ElementId =  @SegmentId
		AND StartDate <= @StartDate AND EndDate >= @EndDate

		SELECT Mov.MovementTransactionId,
			   Mov.MovementID,
			   Mov.SourceNodeId,
			   Mov.SourceNodeName,
			   Mov.DestinationNodeId,
			   Mov.SourceProductId,
			   Mov.DestinationProductId,
			   Mov.EventType,
			   Mov.[NetStandardVolume],
			   Mov.TicketId,	
			  Mov.[OperationalDate] AS [OperationalDate],
			   Mov.SegmentId
		INTO #TempMovementDetails
		FROM [Admin].[view_MovementInformation] Mov
		WHERE Mov.TicketId = @CutOffTicketId
		AND Mov.[OperationalDate] BETWEEN @StartDate AND @EndDate

		SELECT  MovementTransactionId
			   ,DestinationNodeId
			   ,EventType
			   ,NetStandardVolume
			   ,TicketId
			   ,ElementId
			   ,OperationalDate
			   ,MovementID
			   ,SourceNodeId
			   ,SourceProductId
		       ,ROW_NUMBER()OVER(PARTITION BY SubQ.[MovementId]   
		       					ORDER      BY SubQ.[MovementTransactionId] DESC)AS Rnum
			   
		INTO #TempDeterminedInputownershipOfInitialnodes
		FROM 
		(
	         SELECT Mov.MovementTransactionId
		     	  ,Mov.DestinationNodeId
				  ,Mov.SourceNodeId
				  ,Mov.SourceProductId
		     	  ,Mov.EventType
		     	  ,Mov.[NetStandardVolume]
		     	  ,Mov.TicketId	  
		     	  ,NT.ElementId
		     	  ,Mov.[OperationalDate]	
		     	  ,Mov.MovementID
		     FROM #TempNodeTag NT
		     INNER JOIN #TempMovementDetails Mov
		     ON Mov.[DestinationNodeId] = [NT].[NodeId]
		     WHERE Mov.[SourceNodeId] NOT IN (SELECT NodeId FROM #TempNodeTag)
		     AND Mov.[OperationalDate] BETWEEN NT.StartDate AND NT.EndDate
		     UNION
	         SELECT Mov.MovementTransactionId
		     	  ,Mov.DestinationNodeId
				  ,Mov.SourceNodeId
				  ,Mov.SourceProductId
		     	  ,Mov.EventType
		     	  ,Mov.[NetStandardVolume]
		     	  ,Mov.TicketId	  
		     	  ,NT.ElementId
		     	  ,Mov.[OperationalDate]	
		     	  ,Mov.MovementID
		     FROM #TempNodeTag NT
		     INNER JOIN #TempMovementDetails Mov
		     ON Mov.SourceNodeId = [NT].[NodeId]
		     INNER JOIN Admin.CategoryElement CatEle
		     ON CatEle.ElementId = NT.ElementId
		     WHERE Mov.SourceNodeName LIKE '%Genérico%' 
		     AND Mov.[OperationalDate] BETWEEN NT.StartDate AND NT.EndDate
		)SubQ



		--Find out the Movement Transaction which Doesn't have OwnerShipDefined
		SELECT   [temp].[MovementTransactionId]						AS [MovementTransactionId]
				,Temp.SourceNodeId
				,temp.DestinationNodeId
				,Temp.SourceProductId
				,temp.NetStandardVolume
				,[own].[OwnerId]									AS OwnerId
		INTO #TempPreResultNoOwnerShipDefined
		FROM #TempDeterminedInputownershipOfInitialnodes temp
		LEFT JOIN [Offchain].[Owner] own
		ON [own].[MovementTransactionId] = [temp].[MovementTransactionId]
 		WHERE [temp].[Rnum] = 1
		AND Temp.EventType IN ('Update','Insert')
		AND Temp.NetStandardVolume > 0
		AND [own].[OwnerId] IS NULL


		SELECT  MovementId
			   ,OwnerId
			   ,CAST(OwnershipVolume		AS DECIMAL(18,2)) AS OwnershipVolume
			   ,CAST(OwnershipPercentage 	AS DECIMAL(18,2)) AS OwnershipPercentage 
			   ,AppliedRule
			   ,CAST(NetStandardVolume		AS DECIMAL(18,2)) AS NetStandardVolume 
		INTO #TempResultDetails
		FROM 
		(
		SELECT   [temp].[MovementTransactionId]									AS MovementId
				,[own].[OwnerId]												AS OwnerId
                ,CASE WHEN CHARINDEX('%',Own.OwnershipValueUnit)> 0 OR Own.OwnershipValueUnit = '159'
                     THEN (Own.OwnershipValue*temp.NetStandardVolume)/100 --When %
                     ELSE Own.OwnershipValue --When No %
                     END														AS OwnershipVolume
                ,CASE WHEN CHARINDEX('%',Own.OwnershipValueUnit)> 0 OR Own.OwnershipValueUnit = '159'
                     THEN  Own.OwnershipValue
                     ELSE (Own.OwnershipValue/temp.NetStandardVolume)*100 
                     END														AS OwnershipPercentage
				,0																AS AppliedRule
				,temp.NetStandardVolume											AS NetStandardVolume
		FROM #TempDeterminedInputownershipOfInitialnodes temp
		LEFT JOIN [Offchain].[Owner] own
		ON [own].[MovementTransactionId] = [temp].[MovementTransactionId]
		INNER JOIN [Admin].[Node] DestND
		ON [DestND].[NodeId] = [temp].[DestinationNodeId]
		WHERE [temp].[Rnum] = 1
		AND Temp.EventType IN ('Update','Insert')
		AND Temp.NetStandardVolume > 0
		UNION
		SELECT	 Temp.[MovementTransactionId]									AS MovementId
				,NCPO.OwnerId													AS OwnerId
                ,(Temp.NetStandardVolume*NCPO.[OwnershipPercentage]	)/100		AS OwnershipVolume
                ,NCPO.[OwnershipPercentage]										AS OwnershipPercentage 
				,-1																AS AppliedRule
				,temp.NetStandardVolume											AS NetStandardVolume
		FROM #TempPreResultNoOwnerShipDefined Temp
		LEFT JOIN [Admin].[NodeConnection] NC
		ON  NC.SourceNodeId = temp.SourceNodeId
		AND NC.DestinationNodeId = temp.DestinationNodeId
		LEFT JOIN Admin.NodeConnectionProduct NCP
		ON  NCP.NodeConnectionId = NC.NodeConnectionId
		AND NCp.ProductId = temp.SourceProductId
		LEFT JOIN Admin.NodeConnectionProductOwner NCPO
		ON NCPO.NodeConnectionProductId = NCP.NodeConnectionProductId
		WHERE NC.IsDeleted = 0
		)A
		WHERE OwnerId IS NOT NULL


		SELECT  DISTINCT
				MovementId
			   ,OwnerId
			   ,OwnershipVolume
			   ,OwnershipPercentage 
			   ,AppliedRule
			   ,NetStandardVolume 
		FROM #TempResultDetails
  END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
							@value = N'This Procedure is used to get the Input Movement Property details for the Excel file based on the Ticket Id.',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetInputMovementPropertyDetails',
							@level2type = NULL,
							@level2name = NULL