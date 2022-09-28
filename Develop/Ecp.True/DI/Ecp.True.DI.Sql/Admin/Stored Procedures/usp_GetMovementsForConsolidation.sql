/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jul-06-2020
-- <Description>:	This Procedure is used to get the Consolidated Movements on SegmentId , StartDate, EndDate, Node. </Description>
DECLARE @Node [Admin].[MovementNodeType]
INSERT INTO @Node EXEC [Admin].[usp_GetMovementNodesForConsolidation] 145391,'2020-05-01', '2020-05-31'
EXEC [Admin].[usp_GetMovementsForConsolidation] 145391,'2020-05-01', '2020-05-31', @Node
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetMovementsForConsolidation]
(
	     @SegmentId				INT
		,@StartDate				DATE
		,@EndDate				DATE
		,@Node					[Admin].[MovementNodeType] READONLY
)
AS
BEGIN
		SELECT  Mov.MovementTransactionId
			   ,Mov.SourceNodeId
			   ,Mov.SourceProductId
			   ,Mov.DestinationNodeId
			   ,Mov.DestinationProductId
			   ,Mov.MovementTypeId
			   ,Mov.MeasurementUnit
			   ,Mov.NetStandardVolume
			   ,Mov.GrossStandardVolume
			   ,Mov.OriginalMovementTransactionId
			   ,Mov.OwnershipTicketId
		INTO #TempOriginalMovement
		FROM Admin.view_MovementInformation Mov
		INNER JOIN @Node N ON ISNULL(Mov.SourceNodeId,0) = ISNULL(N.SourceNodeId, 0) AND ISNULL(Mov.DestinationNodeId, 0) = ISNULL(N.DestinationNodeId, 0)
		WHERE Mov.SegmentId = @SegmentId
		AND Mov.OperationalDate BETWEEN 
			CASE
				WHEN  Mov.MovementTypeId IN(153,154) 
					THEN  DATEADD(DAY,-1,@StartDate)
					ELSE   @StartDate END
			AND @EndDate 
		AND Mov.ScenarioId = 1 AND Mov.OriginalMovementTransactionId IS NULL

		SELECT  Mov.MovementTransactionId
			   ,Mov.SourceNodeId
			   ,Mov.SourceProductId
			   ,Mov.DestinationNodeId
			   ,Mov.DestinationProductId
			   ,Mov.MovementTypeId
			   ,Mov.MeasurementUnit
			   ,Mov.NetStandardVolume
			   ,Mov.GrossStandardVolume
			   ,Mov.OriginalMovementTransactionId
			   ,Mov.OwnershipTicketId
		INTO #TempCancellationMovement
		FROM Admin.view_MovementInformation Mov
		INNER JOIN #TempOriginalMovement Org ON Mov.OriginalMovementTransactionId = Org.MovementTransactionId
		WHERE Mov.SegmentId = @SegmentId
		AND Mov.OperationalDate BETWEEN 
			CASE
				WHEN  Mov.MovementTypeId IN(153,154) 
					THEN  DATEADD(DAY,-1,@StartDate)
					ELSE   @StartDate END
			AND @EndDate 
		AND Mov.ScenarioId = 1

		SELECT * INTO #TempSegmentMovement FROM (
			SELECT * FROM #TempOriginalMovement
			UNION
			SELECT * FROM #TempCancellationMovement)
		as tmp

		DECLARE @IsSegmentSON BIT

		SELECT @IsSegmentSON = IsOperationalSegment 
		FROM Admin.CategoryElement 
		WHERE ElementId = @SegmentId
		AND CategoryID = 2

		IF @IsSegmentSON = 1
		BEGIN
			SELECT  Mov.MovementTransactionId 
				   ,Mov.SourceNodeId 
				   ,Mov.SourceProductId 
				   ,Mov.DestinationNodeId 
				   ,Mov.DestinationProductId
				   ,Mov.MovementTypeId 
				   ,Mov.MeasurementUnit
				   ,Mov.NetStandardVolume
				   ,Mov.GrossStandardVolume
				   ,Mov.OriginalMovementTransactionId
				   ,Annu.SourceMovementTypeId
				   ,Own.OwnerId
				   ,Own.OwnershipPercentage
				   ,Own.OwnershipVolume
				   ,NULL AS OwnershipValueUnit
				   ,Mov.OwnershipTicketId
			FROM #TempSegmentMovement Mov 
			INNER JOIN Offchain.[Ownership] OWN 
			ON Mov.MovementTransactionId = OWN.MovementTransactionId
			LEFT JOIN Admin.Annulation Annu 
			ON Mov.MovementTypeId = Annu.AnnulationMovementTypeId
			WHERE Mov.OwnershipTicketId IS NOT NULL
		END
		ELSE
		BEGIN
			SELECT  Mov.MovementTransactionId 
				   ,Mov.SourceNodeId 
				   ,Mov.SourceProductId 
				   ,Mov.DestinationNodeId 
				   ,Mov.DestinationProductId
				   ,Mov.MovementTypeId 
				   ,Mov.MeasurementUnit
				   ,Mov.NetStandardVolume
				   ,Mov.GrossStandardVolume
				   ,NULL AS OriginalMovementTransactionId
				   ,NULL AS SourceMovementTypeId
				   ,CAST(Own.OwnerId AS INT) AS OwnerId
				   ,NULL AS OwnershipPercentage
				   ,Own.OwnershipValue AS OwnershipVolume
				   ,Own.OwnershipValueUnit
				   ,Mov.OwnershipTicketId
			FROM #TempSegmentMovement Mov 
			INNER JOIN Offchain.[Owner] OWN 
			ON Mov.MovementTransactionId = OWN.MovementTransactionId
		END
END
GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Consolidated Movements on SegmentId , StartDate, EndDate, Node.',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetMovementsForConsolidation',
							@level2type = NULL,
							@level2name = NULL