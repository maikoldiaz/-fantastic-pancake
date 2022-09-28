/*-- ===================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jan-03-2020
-- Updated Date:	Mar-20-2020
--					Jun-29-2020 Made Changes related to Filter
-- <Description>:	This Procedure is used to get all the Intial Nodes for a given Segement between a given Date range. </Description>
-- Exec [Admin].[usp_GetNodesForSegment] 9796,'2019-12-29','2019-12-29'
-- =====================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetNodesForSegment]
(
	   @SegmentId		INT  ,
       @StartDate		DATE ,
       @EndDate			DATE 
)
AS
BEGIN

		DECLARE @Cnt				INT  = 0,			 
				@ElementId			INT, 
				@NodeID				INT, 
				@newStartDate		DATE, 
				@newEndDate			DATE;

		IF OBJECT_ID('tempdb..#TempNodeTag') IS NOT NULL
		DROP TABLE #TempNodeTag

		IF OBJECT_ID('tempdb..#TempNodesForSegment') IS NOT NULL
		DROP TABLE #TempNodesForSegment

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


		CREATE TABLE #TempNodesForSegment
		(
			SegmentId		INT,
			NodeId			INT,
			OperationDate	DATE
		)

	   IF @StartDate <= @EndDate
	   BEGIN

			SELECT   NT.ElementId		AS ElementId
					,NT.NodeId			AS NodeId
					,Dt.CalculationDate AS OperationDate
			INTO #TempNodeTag
			FROM Admin.NodeTag NT
			INNER JOIN #TempInputData Dt
			ON NT.ElementId = Dt.SegmentId
			WHERE (Dt.CalculationDate   BETWEEN NT.StartDate  AND Nt.EndDate )

			SELECT DISTINCT
						TempNodes.ElementId			AS SegmentId,
						TempNodes.NodeId			AS NodeId,
						Nd.Name						AS NodeName,
						TempNodes.OperationDate		AS OperationDate
			FROM #TempNodeTag TempNodes
			INNER JOIN Admin.Node ND
			ON Nd.NodeId = TempNodes.NodeId
	END

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get all the Intial Nodes for a given Segement between a given Date range.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetNodesForSegment',
    @level2type = NULL,
    @level2name = NULL