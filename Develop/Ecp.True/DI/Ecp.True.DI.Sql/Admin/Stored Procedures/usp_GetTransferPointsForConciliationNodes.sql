/*-- ===============================================================================================================================================
-- Author:          Intergrupo
-- Created Date: 	May-12-2021
-- Updated Date: 	Oct-4-2021 Add column SourceNodeSegmentId, DestinationNodeSegmentId
-- <Description>:	This procedure to get Transfer paint movements dot conciliation nodes. </Description>
-- ================================================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_GetTransferPointsForConciliationNodes]
(
	 @DtConciliationNodes Admin.ConciliationNodeList READONLY
	,@StartDate DATE
	,@EndDate   DATE
)
AS 
BEGIN
		
		IF OBJECT_ID('tempdb..#TempMovementDetailsPreResult')IS NOT NULL
		DROP TABLE #TempMovementDetailsPreResult

		IF OBJECT_ID('tempdb..#TempMovementPreNewNodeDetailsBefore')IS NOT NULL
		DROP TABLE 	#TempMovementPreNewNodeDetailsBefore

		IF OBJECT_ID('tempdb..#TempMovementDetails')IS NOT NULL
		DROP TABLE #TempMovementDetails

		IF OBJECT_ID('tempdb..#TempNodeTag')IS NOT NULL
		DROP TABLE #TempNodeTag

		IF OBJECT_ID('tempdb..#TempTransferPointMovementDetails')IS NOT NULL
		DROP TABLE #TempTransferPointMovementDetails

		SELECT NT.ElementId AS SegmentId, NT.NodeId INTO #TempNodeTag FROM Admin.NodeTag NT JOIN Admin.CategoryElement C on nt.ElementId = C.ElementId  WHERE CategoryId = 2

		SELECT  Mov.MovementId
		       ,Mov.SegmentId
		       ,Mov.MovementTypeId
			   ,MoveType.Name AS MovementTypeName
			   ,Mov.NetStandardVolume
			   ,Mov.OperationalDate
			   ,Mov.MeasurementUnit
			   ,Mov.GlobalMovementId
			   ,Mov.MovementTransactionId
			   ,Mov.OwnershipTicketId
			   ,Mov.ScenarioId
			   ,Mov.UncertaintyPercentage
		INTO #TempMovementDetailsPreResult
		FROM Offchain.Movement Mov
		LEFT JOIN [Admin].CategoryElement MoveType
		ON MoveType.ElementId = Mov.MovementTypeId
		WHERE Mov.OperationalDate BETWEEN @StartDate AND @EndDate
		AND   Mov.MovementTypeId NOT in (49, 50)  -- 49 Compras, 50 ventas
		AND   Mov.ScenarioId = 1


		SELECT   NodeSrc.[Name]						   AS SourceNodeName
				,MovSrc.SourceNodeId				   AS SourceNodeId
				,(SELECT TOP 1 SegmentId 
					FROM #TempNodeTag 
					WHERE 
					NodeId = MovSrc.SourceNodeId)	   AS SourceNodeSegmentId
				,SrcPrd.[Name]						   AS SourceProductName
				,SrcPrd.ProductId					   AS SourceProductId
				,Mov.*
		INTO #TempMovementPreNewNodeDetailsBefore
		FROM #TempMovementDetailsPreResult Mov
		LEFT JOIN Offchain.MovementSource MovSrc
		ON Mov.MovementTransactionID = MovSrc.MovementTransactionId		
		INNER JOIN Admin.Node NodeSrc
		ON NodeSrc.NodeId = MovSrc.SourceNodeId
		INNER JOIN Admin.Product SrcPrd
		ON Srcprd.ProductId = MovSrc.SourceProductId

		SELECT Mov.*,
			   NodeDest.Name  AS DestinationNodeName,
			   MovDest.DestinationNodeId,
			   (SELECT TOP 1 SegmentId 
					FROM #TempNodeTag 
					WHERE 
					NodeId = MovDest.DestinationNodeId) AS DestinationNodeSegmentId,
			   DestPrd.[Name] AS DestinationProductName,
			   DestPrd.ProductId AS DestinationProductId
		INTO #TempMovementDetails
		FROM #TempMovementPreNewNodeDetailsBefore Mov
		INNER JOIN Offchain.MovementDestination MovDest
		ON Mov.MovementTransactionID = MovDest.MovementTransactionId
		INNER JOIN Admin.Node NodeDest
		ON NodeDest.NodeId = MovDest.DestinationNodeId
		INNER JOIN Admin.Product DestPrd
		ON DestPrd.ProductId = MovDest.DestinationProductId

		--Deleting the MovementId which have GlobalMovementId IS Not NULL
		;WITH CteGlobalMovementIdIsNotNUll
		AS
		(
			SELECT Mov.MovementID
			FROM #TempMovementDetails Mov
			WHERE Mov.GlobalMovementId IS NOT NULL
		)
		DELETE Temp
		FROM #TempMovementDetails Temp
		INNER JOIN CteGlobalMovementIdIsNotNUll Cte
		ON Temp.MovementID = Cte.MovementID

		--Get the Latest Record for Each Movement and Pick if NetStandardVolume > 0
		;WITH CTE
		AS
		(
			SELECT  Mov.MovementId
				   ,Mov.SegmentId
				   ,Mov.MovementTypeName
				   ,Mov.MovementTypeId
				   ,Mov.SourceNodeName
				   ,Mov.SourceNodeId
				   ,Mov.SourceNodeSegmentId
				   ,Mov.DestinationNodeName
				   ,Mov.DestinationNodeId
				   ,Mov.DestinationNodeSegmentId
				   ,Mov.SourceProductName
				   ,Mov.SourceProductId
				   ,Mov.DestinationProductName
				   ,Mov.DestinationProductId
				   ,Mov.NetStandardVolume
				   ,Mov.OperationalDate
				   ,Mov.MeasurementUnit
				   ,Mov.GlobalMovementId
				   ,Mov.MovementTransactionId
				   ,Mov.OwnershipTicketId	
				   ,Mov.ScenarioId
				   ,Mov.UncertaintyPercentage
				   ,ROW_NUMBER()OVER(PARTITION BY Mov.MovementId 
									 ORDER BY Mov.MovementTransactionId DESC) Cnt
			FROM #TempMovementDetails Mov
		)

		SELECT C.MovementId
			  ,C.MovementTransactionId
			  ,C.MovementTypeId
			  ,C.SegmentId
			  ,C.MovementTypeName
			  ,C.SourceNodeName
			  ,C.SourceNodeId
			  ,C.SourceNodeSegmentId
			  ,C.DestinationNodeName
			  ,C.DestinationNodeId
			  ,C.DestinationNodeSegmentId
			  ,C.SourceProductName
			  ,C.SourceProductId
			  ,C.DestinationProductName
			  ,C.DestinationProductId
			  ,C.NetStandardVolume
			  ,C.ScenarioId
			  ,C.OwnershipTicketId
			  ,C.MeasurementUnit
			  ,C.OperationalDate
			  ,C.UncertaintyPercentage
		INTO #TempTransferPointMovementDetails
		FROM CTE C
		WHERE C.Cnt = 1
		AND C.NetStandardVolume > 0
		ORDER BY C.SourceNodeName,
		C.DestinationNodeName,
		C.OperationalDate		


		SELECT Mov.*
			  ,CASE WHEN Mov.SegmentId IN (SELECT CA.ElementId FROM CategoryElement CA WHERE CA.IsOperationalSegment = 1 AND CA.CategoryId = 2)
				THEN ow.OwnershipPercentage
				WHEN ow2.OwnershipValueUnit <> '%' THEN (100 * ow2.OwnershipValue)/Mov.NetStandardVolume ELSE ow2.OwnershipValue  END as OwnershipPercentage

			    ,CASE WHEN Mov.SegmentId IN (SELECT CA.ElementId FROM CategoryElement CA WHERE CA.IsOperationalSegment = 1 AND CA.CategoryId = 2)
				THEN ow.OwnershipVolume
				WHEN ow2.OwnershipValueUnit = '%' THEN (Mov.NetStandardVolume * ow2.OwnershipValue)/100 ELSE ow2.OwnershipValue  END as OwnershipVolume

			    ,CASE WHEN Mov.SegmentId IN (SELECT CA.ElementId FROM CategoryElement CA WHERE CA.IsOperationalSegment = 1 AND CA.CategoryId = 2)
				THEN ow.OwnerId ELSE ow2.OwnerId  END as OwnerId
				

			    ,CASE WHEN Mov.SegmentId IN (SELECT CA.ElementId FROM Admin.CategoryElement CA WHERE CA.IsOperationalSegment = 1 AND CA.CategoryId = 2)
				THEN '%' ELSE ow2.OwnershipValueUnit  END as OwnershipValueUnit
		FROM #TempTransferPointMovementDetails Mov
			INNER JOIN @DtConciliationNodes Nc
			ON Nc.SourceNodeId = Mov.SourceNodeId and Nc.DestinationNodeId = Mov.DestinationNodeId
			LEFT JOIN (SELECT D.MovementTransactionId, D.OwnershipPercentage ,D.OwnerId, D.OwnershipVolume FROM Offchain.Ownership D 
			WHERE  D.IsDeleted = 0) AS ow
			ON ow.MovementTransactionId = Mov.MovementTransactionId AND Mov.SegmentId IN (SELECT CA.ElementId FROM	Admin.CategoryElement CA WHERE CA.IsOperationalSegment = 1 AND CA.CategoryId = 2)
			LEFT JOIN (SELECT C.MovementTransactionId, C.OwnershipValue, C.OwnerId, C.OwnershipValueUnit FROM Offchain.Owner C) AS ow2
			ON ow2.MovementTransactionId = Mov.MovementTransactionId AND Mov.SegmentId not IN (SELECT CA.ElementId FROM	Admin.CategoryElement CA WHERE CA.IsOperationalSegment = 1 AND CA.CategoryId = 2)
		WHERE CASE WHEN Mov.SegmentId IN (SELECT CA.ElementId FROM Admin.CategoryElement CA WHERE CA.IsOperationalSegment = 1 AND CA.CategoryId = 2) 
		THEN ow.OwnerId ELSE ow2.OwnerId  END  IS NOT NULL 
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description',
								@value=N'This procedure to get Transfer paint movements dot conciliation nodes.',
								@level0type=N'SCHEMA',
								@level0name=N'Admin',
								@level1type=N'PROCEDURE',
								@level1name=N'usp_GetTransferPointsForConciliationNodes'
GO
