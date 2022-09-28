/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jun-12-2020
-- Updated Date:	Aug-04-2020 Modified For Performance reasons
-- Updated date:	Aug-06-2020 Removed Casting on MoveType.ElementId = Mov.MovementTypeId And CE.ElementId  = C.MeasurementUnit 
-- <Description>:	This procedure to get movements marked as official points. </Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetTransferPoints]
(
	 @SegmentId	INT
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

		IF OBJECT_ID('tempdb..#TempTransferPointMovementDetails')IS NOT NULL
		DROP TABLE #TempTransferPointMovementDetails

		IF OBJECT_ID('tempdb..#TempSapTrackingErrorsCount')IS NOT NULL
		DROP TABLE #TempSapTrackingErrorsCount

		SELECT  Mov.MovementId
			   ,MoveType.Name AS MovementTypeName
			   ,Mov.NetStandardVolume
			   ,Mov.OperationalDate
			   ,Mov.MeasurementUnit
			   ,Mov.GlobalMovementId
			   ,Mov.MovementTransactionId
		INTO #TempMovementDetailsPreResult
		FROM Offchain.Movement Mov
		LEFT JOIN [Admin].CategoryElement MoveType
		ON MoveType.ElementId = Mov.MovementTypeId
		WHERE Mov.SegmentId = @SegmentId
		AND   Mov.OperationalDate BETWEEN @StartDate AND @EndDate
		AND   Mov.IsTransferPoint = 1 
		AND   Mov.ScenarioId = 1
		AND	  Mov.TicketId IS NULL


		SELECT   NodeSrc.[Name]						   AS SourceNodeName
				,MovSrc.SourceNodeId				   AS SourceNodeId
				,SrcPrd.[Name]						   AS SourceProductName
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
			   DestPrd.[Name] AS DestinationProductName
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
				   ,Mov.MovementTypeName
				   ,Mov.SourceNodeName
				   ,Mov.DestinationNodeName
				   ,Mov.SourceProductName
				   ,Mov.DestinationProductName
				   ,Mov.NetStandardVolume
				   ,Mov.OperationalDate
				   ,Mov.MeasurementUnit
				   ,Mov.GlobalMovementId
				   ,Mov.MovementTransactionId
				   ,ROW_NUMBER()OVER(PARTITION BY Mov.MovementId 
									 ORDER BY Mov.MovementTransactionId DESC) Cnt
			FROM #TempMovementDetails Mov
		)
		SELECT C.MovementId
			  ,C.MovementTransactionId
			  ,C.MovementTypeName
			  ,C.SourceNodeName
			  ,C.DestinationNodeName
			  ,C.SourceProductName
			  ,C.DestinationProductName
			  ,C.NetStandardVolume
			  ,CE.Name AS MeasurementUnit
			  ,C.OperationalDate
			  ,Sap.SapTrackingId
			  ,Sap.ErrorMessage
		INTO #TempTransferPointMovementDetails
		FROM CTE C
		INNER JOIN Admin.CategoryElement CE
		ON CE.ElementId  = C.MeasurementUnit 
		INNER JOIN Admin.SapTracking Sap
		ON C.MovementTransactionId = Sap.MovementTransactionId
		WHERE C.Cnt = 1
		AND C.NetStandardVolume > 0
		ORDER BY C.SourceNodeName,
		C.DestinationNodeName,
		C.OperationalDate		

		SELECT Mov.SapTrackingId
			  ,COUNT(ErrorCode)	 AS	[ErrorCount]
		INTO #TempSapTrackingErrorsCount
		FROM #TempTransferPointMovementDetails Mov
		LEFT JOIN Admin.SapTrackingError Err
		ON Mov.SapTrackingId = Err.SapTrackingId
		WHERE ISNULL(Mov.ErrorMessage, '') = ''
		GROUP BY Mov.SapTrackingId

		SELECT Mov.*
			  ,Err.ErrorCount
		FROM #TempTransferPointMovementDetails Mov
		LEFT JOIN #TempSapTrackingErrorsCount Err
		ON Mov.SapTrackingId = Err.SapTrackingId
END

GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This dummy procedure to get movements marked as official points.',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetTransferPoints',
							@level2type = NULL,
							@level2name = NULL