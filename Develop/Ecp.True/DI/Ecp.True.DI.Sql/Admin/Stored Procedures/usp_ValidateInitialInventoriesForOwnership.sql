/*-- =================================================================================================================================
-- Author:          Microsoft  
-- Created Date:    May-20-2020
-- Updated Date:    May-29-2020 Removed UnNecessary Conditions(Join with extra tables)
-- Updated Date:    Jun-11-2020 Modified the Criteria for Initial Inventories and Movement Logic
--					Jun-15-2020 Added NodeTag Logic Verification in the Procedure
--					Aug-03-2020 Modified Code Logic For performance reasons
--					Sep-07-2020 Modified Partition By Logic
-- <Description>:	This Procedure is used to get operational cutoff to ensure ownership initialization of the initial inventories  </Description>
-- EXEC [Admin].[usp_ValidateInitialInventoriesForOwnership] 66791,'2020-05-31','2020-06-03'
-- ==================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_ValidateInitialInventoriesForOwnership] 
(
       @SegmentId          INT,
	   @StartDate		   DATETIME,
	   @EndDate			   DATETIME
)
AS 
  BEGIN 
		IF OBJECT_ID('tempdb..#TempNodeDetailsInventories')IS NOT NULL
		DROP TABLE #TempNodeDetailsInventories

		IF OBJECT_ID('tempdb..#TempMovementPreNewNodeDetails')IS NOT NULL
		DROP TABLE #TempMovementPreNewNodeDetails

		IF OBJECT_ID('tempdb..#TempPreResultInvDetails')IS NOT NULL
		DROP TABLE #TempPreResultInvDetails

		IF OBJECT_ID('tempdb..#TempIntialInventoryForInventoryData')IS NOT NULL
		DROP TABLE #TempIntialInventoryForInventoryData

		IF OBJECT_ID('tempdb..#TempNodeTagDetails')IS NOT NULL
		DROP TABLE #TempNodeTagDetails

		IF OBJECT_ID('tempdb..#TempMovementDetails')IS NOT NULL
		DROP TABLE #TempMovementDetails

		IF OBJECT_ID('tempdb..#TempMovementPreNewNodeDetailsBefore')IS NOT NULL
		DROP TABLE 	#TempMovementPreNewNodeDetailsBefore

		DECLARE @PreviousdayOfStartDate DATETIME = DATEADD(DAY,-1,@StartDate)


		SELECT Mov.MovementTransactionID,
			   Mov.SegmentId,
			   Mov.IsDeleted,
			   Mov.OperationalDate
		INTO #TempMovementDetails
		FROM Offchain.Movement Mov
		WHERE Mov.SegmentId = @SegmentId
		AND CAST(Mov.OperationalDate AS DATE) BETWEEN @StartDate AND @EndDate

		SELECT   NodeSrc.name						   AS SourceNodeName
				,MovSrc.SourceNodeId				   AS SourceNodeId
				,Mov.OperationalDate			       AS OperationalDate
				,Mov.MovementTransactionID
		INTO #TempMovementPreNewNodeDetailsBefore
		FROM #TempMovementDetails Mov
		INNER JOIN Offchain.MovementSource MovSrc
		ON Mov.MovementTransactionID = MovSrc.MovementTransactionId		
		INNER JOIN Admin.Node NodeSrc
		ON NodeSrc.NodeId = MovSrc.SourceNodeId

		SELECT MovSrc.SourceNodeName,
			   MovSrc.SourceNodeId,
			   NodeDest.Name AS DestinationNodeName,
			   MovDest.DestinationNodeId,
			   Mov.OperationalDate
		INTO #TempMovementPreNewNodeDetails
		FROM #TempMovementDetails Mov
		LEFT JOIN #TempMovementPreNewNodeDetailsBefore MovSrc
		ON MovSrc.MovementTransactionId = Mov.MovementTransactionId
		LEFT JOIN Offchain.MovementDestination MovDest
		ON Mov.MovementTransactionID = MovDest.MovementTransactionId
		INNER JOIN Admin.Node NodeDest
		ON NodeDest.NodeId = MovDest.DestinationNodeId

		;WITH CTE
		AS
		(
			SELECT   InvPrd.NodeId											  AS NodeId	
					,InvPrd.InventoryDate 									  AS InventoryDate
					,InvPrd.ProductVolume									  AS ProductVolume
					,InvPrd.InventoryProductId								  AS InventoryProductId
					,InvPrd.SegmentId										  AS SegmentId
					,ROW_NUMBER()OVER(PARTITION BY  InvPrd.InventoryProductUniqueId
									  ORDER BY InvPrd.InventoryProductId DESC) AS Cnt					
			FROM Offchain.InventoryProduct InvPrd			
			WHERE InvPrd.SegmentId = @SegmentId
			AND InvPrd.InventoryDate  BETWEEN @PreviousdayOfStartDate AND @EndDate
		)
		SELECT C.*
		INTO #TempIntialInventoryForInventoryData
		FROM CTE C
		WHERE Cnt = 1
		AND ProductVolume > 0

		SELECT NodeId
			  ,StartDate
			  ,EndDate
		INTO #TempNodeTagDetails
		FROM Admin.NodeTag NT
		WHERE Nt.ElementId = @SegmentId

		SELECT   ND.[Name]								AS NodeName
				,InvPrd.NodeId							AS NodeId	
				,InvPrd.InventoryDate					AS CalculationDate
				,'InventoryNode'					    AS TypeOfNode
		INTO #TempNodeDetailsInventories
		FROM Offchain.InventoryProduct InvPrd
		INNER JOIN Admin.[Node] ND
		ON ND.NodeId = InvPrd.NodeId
		AND InvPrd.SegmentId = @SegmentId
		AND InvPrd.InventoryDate  BETWEEN @StartDate AND @EndDate
		UNION
		SELECT   Src.SourceNodeName						AS NodeName
				,Src.SourceNodeId						AS NodeId	
				,Src.OperationalDate				    AS CalculationDate
				,'MovementSourceNode'					AS TypeOfNode
		FROM #TempMovementPreNewNodeDetails Src
		INNER JOIN #TempNodeTagDetails NT
		ON Src.SourceNodeId = NT.NodeId
		WHERE Src.SourceNodeId IS NOT NULL
		AND Src.OperationalDate BETWEEN NT.StartDate AND NT.EndDate
		UNION
		SELECT   Dest.DestinationNodeName				AS NodeName
				,Dest.DestinationNodeId					AS NodeId	
				,Dest.OperationalDate					AS CalculationDate
				,'MovementDestinationNode'				AS TypeOfNode
		FROM #TempMovementPreNewNodeDetails Dest
		INNER JOIN #TempNodeTagDetails NT
		ON Dest.DestinationNodeId = NT.NodeId
		WHERE Dest.DestinationNodeId IS NOT NULL
		AND Dest.OperationalDate BETWEEN NT.StartDate AND NT.EndDate

		--Suppose Use Selection (Start Date-->jan-01-2020 & End Date-->Jan-10-2020)
		--Node "N1" is Tagged for Jan-01-2020, Jan-03-2020 and Jan-05-2020 
		--then Node record Combination ("N1" is Tagged for Jan-01-2020 will kept)
		--and rest records will be deleted("N1" is Tagged for Jan-03-2020 and Jan-05-2020 )	
		--Deleting data from #TempNewNodeDetails		
		;With CTE
		AS
		(
			SELECT   NodeName
					,NodeId	
					,CalculationDate
					,TypeOfNode
					,ROW_NUMBER()OVER(PARTITION BY NodeId ORDER BY CalculationDate) Rno
			FROM #TempNodeDetailsInventories
		)
		DELETE FROM CTE 
		WHERE Rno <> 1

		
		--New Node(Check in Movement/ Inventory for the Nodes there should not be any ticket for these nodes then they are new nodes)
		--Deleting the Nodes which has Ticket ID based on InventoryProduct Table
		DELETE Nodedtls
		FROM #TempNodeDetailsInventories Nodedtls
		INNER JOIN Offchain.InventoryProduct InvPrd
		ON Nodedtls.NodeId = InvPrd.NodeId
		WHERE InvPrd.TicketId IS NOT NULL
		AND InvPrd.SegmentId = @SegmentId

		--Deleting the Nodes which has Ticket ID based on Movement Details
		DELETE Nodedtls
		FROM #TempNodeDetailsInventories Nodedtls
		INNER JOIN Admin.view_MovementInformation Mov
		ON (Nodedtls.NodeId = Mov.SourceNodeId 
			OR Nodedtls.NodeId = Mov.DestinationNodeId) 
		WHERE Mov.TicketId IS NOT NULL
		AND Mov.SegmentId = @SegmentId
		
		--Pick the Nodes which doesn't have Record in Owner Table By Referring Columns(InventoryProductId from InventoryProduct table)	
		SELECT   Lft.NodeName			AS [NodeName]
				,Lft.NodeId				AS NodeId
				,Lft.CalculationDate		AS [Date]
				,CASE WHEN InvPrd.InventoryProductId IS NULL 
					  THEN 0
					  ELSE 1
					  END			    AS InitialInventoryExists
				,CASE WHEN Own.Id IS NULL
					  THEN 0
					  ELSE 1 
					  END			    AS OwnerExists
		INTO #TempPreResultInvDetails
		FROM #TempNodeDetailsInventories Lft--New Nodes
		LEFT JOIN #TempIntialInventoryForInventoryData InvPrd
		ON Lft.NodeId = InvPrd.NodeId
		AND InvPrd.InventoryDate = DATEADD(DAY,-1,CAST(Lft.CalculationDate AS DATE))	
		AND InvPrd.SegmentId = @SegmentId
		LEFT JOIN Offchain.[Owner] Own
		ON Own.InventoryProductId = InvPrd.InventoryProductId

		--Eliminating records which have ownership and Intial Inventory(Both)
		DELETE FROM #TempPreResultInvDetails
		WHERE InitialInventoryExists = 1 
		AND OwnerExists = 1

		--Need to Validate only if there is atleast one record with Out Owner Details associated(atleast 1 record with out owner)
		IF EXISTS(SELECT 1 FROM #TempPreResultInvDetails WHERE OwnerExists = 0)
		BEGIN
			--Type Columns will have (1-->If they have inventories , 2-->If they Don't have inventories )
			SELECT  DISTINCT
					Lft.NodeName											AS [NodeName]
					,DATEADD(DAY,-1,CAST(Lft.[Date] AS DATE))				AS [InventoryDate]
					,CASE WHEN InitialInventoryExists = 1 AND OwnerExists = 0
							THEN 1 
							WHEN InitialInventoryExists = 0
							THEN 2 
							END			 AS [Type]
			FROM #TempPreResultInvDetails Lft
		END
		ELSE
		BEGIN
			--Return EmptyResult Where there are no records where OwnerExists = 0
			SELECT  DISTINCT
					Lft.NodeName			AS [NodeName]
					,Lft.[Date]				AS [InventoryDate]
					,NULL AS [Type]
			FROM #TempPreResultInvDetails Lft
			WHERE 1 = 2
		END
  END
GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get operational cutoff to ensure ownership initialization of the initial inventories',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_ValidateInitialInventoriesForOwnership',
							@level2type = NULL,
							@level2name = NULL