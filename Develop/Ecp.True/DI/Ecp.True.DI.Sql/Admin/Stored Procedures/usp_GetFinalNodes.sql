/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jan-03-2020
-- Updated Date:	Mar-20-2020
--					Jun-29-2020 Made Changes related to Filter
-- <Description>:	This Procedure is used to get all the Final Nodes based on the input of a Segment, StartDate and EndDate. </Description>
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetFinalNodes]
(
	   @SegmentId		INT  ,
       @StartDate		DATE ,
       @EndDate			DATE 
)
AS
BEGIN
		DECLARE @ElementId			INT, 
				@NodeID				INT, 
				@newStartDate		DATE, 
				@newEndDate			DATE;		

		IF OBJECT_ID('tempdb..#TempNodeTag') IS NOT NULL
		DROP TABLE #TempNodeTag

		IF OBJECT_ID('tempdb..#TempPreResultFinalNodes') IS NOT NULL
		DROP TABLE #TempPreResultFinalNodes

		IF OBJECT_ID('tempdb..#TempInputData') IS NOT NULL
		DROP TABLE #TempInputData

		SELECT	Dates		AS  CalculationDate,
				ElementId	AS  SegmentId 
		INTO #TempInputData
		FROM [Admin].[udf_GetAllDates]( @StartDate, 
							            @EndDate, 
							            1, 
							            @SegmentId
									   )A

	   IF @StartDate <= @EndDate
	   BEGIN

			SELECT   NT.ElementId
					,NT.NodeId
					,CASE WHEN @StartDate >= NT.StartDate 
						  THEN @StartDate
						  ELSE CAST(NT.StartDate		AS DATE) 
						  END AS StartDate
					,CASE WHEN Nt.EndDate = '9999-12-31' 
						   OR  @EndDate   <= Nt.EndDate
						  THEN @EndDate
						  ELSE CAST(NT.EndDate AS DATE)
						  END AS EndDate
					,ROW_NUMBER()OVER(ORDER BY NT.NodeId)Rnum
			INTO #TempNodeTag
			FROM Admin.NodeTag NT
			INNER JOIN #TempInputData Dt
			ON NT.ElementId = Dt.SegmentId
			WHERE (Dt.CalculationDate   BETWEEN NT.StartDate  AND Nt.EndDate )

			SELECT	 DISTINCT
					 Subq.ElementId
					,Subq.NodeId
					,Subq.FinalNodeId	AS FinalNodeId	
					,Subq.OperationDate AS OperationDate
					,ROW_NUMBER()OVER(ORDER BY Subq.ElementId
											  ,Subq.NodeId )Rnum
			INTO #TempPreResultFinalNodes
			FROM(
					SELECT	 DISTINCT
							 NT.ElementId
							,NT.NodeId
							,NC.SourceNodeId	AS FinalNodeId
							,Dt.CalculationDate	AS OperationDate
							,DestSeg.ElementId AS SrcElementID
					FROM #TempNodeTag NT
					INNER JOIN Admin.[NodeConnection] NC
					ON  NT.NodeId   =  Nc.SourceNodeId	
					INNER JOIN Admin.NodeTag DestSeg	/*Joined NodeTag Again Based on NodeId to get SegementId which shouldnot be matching*/
					ON DestSeg.NodeId = Nc.DestinationNodeId	
					INNER JOIN Admin.Node DestND  /*Joined Node Table To Get Name */
					ON DestND.NodeId = Nc.DestinationNodeId	
					INNER JOIN Admin.CategoryElement DestEle
					ON DestEle.ElementId = DestSeg.ElementId
					INNER JOIN #TempInputData Dt
					ON NT.ElementId = Dt.SegmentId	
					WHERE (DestSeg.ElementId <> Dt.SegmentId OR DestND.Name LIKE '%Genérico%' ) ---Check if the NodeName Contains
					AND DestEle.CategoryId = 2--Segmento
					AND (Dt.CalculationDate BETWEEN NT.StartDate  AND Nt.EndDate )													
					AND (Dt.CalculationDate  BETWEEN DestSeg.StartDate  AND DestSeg.EndDate )
			  )Subq

			SELECT DISTINCT
						TempNodes.ElementId			AS SegmentId,
						TempNodes.NodeId			AS NodeId,
						Nd.Name						AS NodeName,
						TempNodes.OperationDate		AS OperationDate
			FROM #TempPreResultFinalNodes TempNodes
			INNER JOIN Admin.Node ND
			ON Nd.NodeId = TempNodes.NodeId
	END
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get all the Final Nodes based on the input of a Segment, StartDate and EndDate.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetFinalNodes',
    @level2type = NULL,
    @level2name = NULL