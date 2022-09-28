/*-- ============================================================================================================================
-- Author:          Intergrupo
-- Created Date:	Apr-26-2021
-- Updated Date:	Ago-30-2021 Add validation generic node
-- Updated Date:	Sep-14-2021 Change validation Generic type node
-- Updated Date:	Sep-21-2021 Change CONCILIATION validation
-- <Description>:   This function get all connections of conciliation nodes, filter for node or segment </Description>
-- ==============================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_GetConnectionConciliationNodes](@NodeId INT = NULL, @SegmentId INT = NULL)
AS
BEGIN
	IF OBJECT_ID('tempdb..#NodeTag')IS NOT NULL
	DROP TABLE #NodeTag

	IF OBJECT_ID('tempdb..#TempCategoryElement')IS NOT NULL
	DROP TABLE #TempCategoryElement

	IF OBJECT_ID('tempdb..#TempNodes')IS NOT NULL
	DROP TABLE #TempNodes
	CREATE TABLE #TempNodes (NodeId INT)
	
	IF OBJECT_ID('tempdb..#TempNodeGeneric')IS NOT NULL
		DROP TABLE #TempNodeGeneric 

	SELECT 
		Nt.NodeId,
		Nt.StartDate,
		Nt.EndDate
		INTO #TempNodeGeneric
			FROM Admin.NodeTag Nt
				INNER Join Admin.CategoryElement c on nt.ElementId = c.ElementId
			WHERE  c.Name like'%Gen[eé]rico%' and c.CategoryId = 1

	SELECT ce.ElementId, ce.CategoryId 
	INTO #TempCategoryElement
	FROM Admin.CategoryElement ce

	SELECT NT.NodeId, NT.ElementId
	INTO #NodeTag
	FROM Admin.NodeTag NT
	Where NT.enddate= '9999-12-31 00:00:00.000'

	IF(@NodeId IS NOT NULL)
	BEGIN
		INSERT INTO #TempNodes VALUES (@NodeId)
	END
	ELSE
	BEGIN	
		INSERT INTO #TempNodes SELECT NT.NodeId FROM #NodeTag NT JOIN Admin.Node N ON NT.NodeId = N.NodeId WHERE NT.ElementId = @SegmentId
	END

	SELECT dt.NodeConnectionId, dt.Description, CONVERT(INT,dt.SourceSegmentId) AS SourceSegmentId, 
		   CONVERT(INT, dt.DestinationSegmentId) AS DestinationSegmentId, category.Name AS SourceSegmentName, 
		   category2.Name AS DestinationSegmentName, dt.SourceNodeId, dt.DestinationNodeId, Node1.Name AS SourceNodeName, 
		   Node2.Name AS DestinationNodeName

	FROM (SELECT nc.NodeConnectionId, nc.Description, nc.SourceNodeId, nc.DestinationNodeId, nc.IsActive, nc.IsDeleted, ce.ElementId, ce.Name, 
			 (SELECT ce.ElementId FROM #TempCategoryElement ce JOIN #NodeTag nta ON nta.ElementId = ce.ElementId 
			  WHERE nta.NodeId = nc.SourceNodeId AND ce.CategoryId=2) AS SourceSegmentId,
			 (SELECT ce.ElementId FROM #TempCategoryElement ce JOIN #NodeTag nta ON nta.ElementId = ce.ElementId 
			  WHERE nta.NodeId = nc.DestinationNodeId  and ce.CategoryId=2) AS DestinationSegmentId
		   FROM Admin.NodeConnection nc JOIN #NodeTag nt ON nt.NodeId in (nc.SourceNodeId, nc.DestinationNodeId) 
										 JOIN Admin.Node n ON n.NodeId = nt.NodeId 
										 JOIN Admin.CategoryElement ce ON nt.ElementId = ce.ElementId 
		   WHERE ce.CategoryId =2 AND nt.NodeId IN (SELECT dtn.NodeId FROM #TempNodes AS dtn)
		  ) AS dt 
	JOIN Admin.CategoryElement category on category.ElementId = SourceSegmentId 
	JOIN Admin.CategoryElement category2 on category2.ElementId = DestinationSegmentId 
	JOIN Admin.Node Node1 on Node1.NodeId = SourceNodeId 
	JOIN Admin.Node Node2 on Node2.NodeId = DestinationNodeId 
	WHERE dt.SourceSegmentId <> dt.DestinationSegmentId 
		    AND dt.IsActive = 1 and dt.IsDeleted = 0
			AND Lower(Node1.[Name]) NOT LIKE '%gen[eé]rico%'
			AND Lower(Node1.[Name]) <> 'CONCILIACION'
			AND Lower(Node2.[Name]) NOT LIKE '%gen[eé]rico%'
			AND Lower(Node2.[Name]) <> 'CONCILIACION'
			AND Node1.NodeId NOT IN (SELECT NodeId FROM #TempNodeGeneric) -- Tipo generico
			AND Node2.NodeId NOT IN (SELECT NodeId FROM #TempNodeGeneric) -- Tipo generico
	 GROUP BY dt.NodeConnectionId, dt.Description, dt.SourceSegmentId, dt.DestinationSegmentId, 
	 category.Name, category2.Name, dt.SourceNodeId, dt.DestinationNodeId, Node1.Name, Node2.Name
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description',
								@value=N'This function get all connections of conciliation nodes, filter for node or segment',
								@level0type=N'SCHEMA',
								@level0name=N'Admin',
								@level1type=N'PROCEDURE',
								@level1name=N'usp_GetConnectionConciliationNodes'
GO
