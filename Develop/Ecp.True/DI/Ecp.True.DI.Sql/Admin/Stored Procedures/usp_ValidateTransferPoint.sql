/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jun-17-2020
-- Updated Date:	Jun-19-2020 Added SegmentID Condition
-- Updated Date:	Aug-31-2020 Reverted exception return condition
-- <Description>:	This procedure to get movements marked as official points Validing them. </Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_ValidateTransferPoint]
(
	 @SegmentId			INT
	,@SourceNodeId		INT
	,@DestinationNodeId	INT
	,@OperationalDate   DATE
)
AS
BEGIN
	DECLARE @ReturnFlag				INT = 0,
			@SourceNodeSegmentId	INT = 0,
			@DestinationSegmentId	INT = 0
		
	IF EXISTS(SELECT 1 
			  FROM Admin.CategoryElement 
			  WHERE ElementId = @SegmentId 
			  AND IsOperationalSegment = 1)
	BEGIN
		
		SELECT @SourceNodeSegmentId = NTSrc.ElementId
		FROM Admin.Nodetag NTSrc
		INNER JOIN Admin.CategoryElement ce
		ON ce.ElementId = NTSrc.ElementId
		WHERE ce.CategoryId = 2
		AND NTSrc.NodeId = @SourceNodeId
		AND @OperationalDate BETWEEN NTSrc.StartDate AND NTSrc.EndDate

		SELECT @DestinationSegmentId = NTDest.ElementId
		FROM Admin.Nodetag NTDest
		INNER JOIN Admin.CategoryElement ce
		ON ce.ElementId = NTDest.ElementId
		WHERE ce.CategoryId = 2
		AND NTDest.NodeId = @DestinationNodeId
		AND @OperationalDate BETWEEN NTDest.StartDate AND NTDest.EndDate

		--Find segement for source node id and destination node id.. if they are different return 1, else return 0
		SELECT @ReturnFlag = CASE WHEN @SourceNodeSegmentId <> @DestinationSegmentId 
								  THEN 1
								  ELSE 0 
								  END	
								  
		IF @ReturnFlag = 1
		BEGIN
			RAISERROR('It is a Transfer Point Movement',15,1)
		END
	END	
END

GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This procedure to get movements marked as official points Validing them',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_ValidateTransferPoint',
							@level2type = NULL,
							@level2name = NULL