/*-- ==================================================================================================================================
-- Author:          Microsoft
-- Created date: 	Feb-17-2020
-- Updated Date:	Mar-20-2020
-- <Description>:   This Procedure is used to get the Graphical Node Connection details for a given Segment (ElementId) and Node. </Description>
EXEC [Admin].[usp_GetGraphicalNodeConnection] 18887,0
-- ===================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetGraphicalNodeConnection]
(
	     @ElementId 	INT 
		,@NodeId	    INT
)
AS
BEGIN
	SET NOCOUNT ON

       --VARIABLES DECLARATION
        DECLARE  @SystemId	        INT
		        ,@CategoryId        INT

     -- GET CATEGORY ID BASED ON INPUT CATEGORYElement
        SELECT @CategoryId =  CategoryId 
        FROM [Admin].[CategoryElement] 
        WHERE ElementId = @ElementId
	
    -- NODETAG CATEGORY ELEMENT TEMP TABLE
	    IF OBJECT_ID('tempdb..#TempGetNodeTagCategoryElement') IS NOT NULL
        DROP TABLE #TempGetNodeTagCategoryElement
		
		SELECT   CE.ElementId
				,CE.[Name] AS [Name]
				,CE.CategoryId
				,CE.IsActive
				,CE.IconId
				,CE.Color
		        ,NT.NodeId
				,NT.StartDate
				,NT.EndDate
		INTO #TempGetNodeTagCategoryElement
		FROM [Admin].[NodeTag] NT
		JOIN [Admin].[CategoryElement] CE
	      ON NT.ElementId = CE.ElementId
	   WHERE CE.ElementId = @ElementId
         AND (NT.NodeId = @NodeId OR @NodeId = 0)
         AND CAST((SELECT [Admin].[udf_GetTrueDate]()) AS DATE) BETWEEN CAST(NT.StartDate AS DATE) 
		                                                           AND CAST(NT.EndDate AS DATE)

    -- GET ALL NODES BASED ON INPUT CATEGORY, ELEMENT AND NODE
	    IF OBJECT_ID('tempdb..#TempGetAllNodes') IS NOT NULL
        DROP TABLE #TempGetAllNodes
	    	
        SELECT DISTINCT NT.NodeId
                       ,NT.CategoryId
        			   ,NT.ElementId
        INTO #TempGetAllNodes
        FROM #TempGetNodeTagCategoryElement NT
        INNER JOIN [Admin].[Node] ND
        ON ND.NodeId = NT.NodeId
        WHERE NT.CategoryId = @CategoryId
		AND NT.ElementId = @ElementId
        AND (NT.NodeId = @NodeId OR @NodeId = 0)
        AND CAST((SELECT [Admin].[udf_GetTrueDate]()) AS DATE) BETWEEN CAST(NT.StartDate AS DATE) 
		                                                           AND CAST(NT.EndDate AS DATE)

		-- GET SOURCE AND DESTINATION NODE DETAILS WHERE INPUT NODE IS SOURCE OR DESTINATION NODE
	 	IF OBJECT_ID('tempdb..#TempGetAllNodeConnections') IS NOT NULL
        DROP TABLE #TempGetAllNodeConnections

	    SELECT  SourceNodeId
	           ,DestinationNodeId
	    	   ,IsActive
	    	   ,IsTransfer
			   ,[RowVersion]
        INTO #TempGetAllNodeConnections
	    FROM [Admin].[NodeConnection] NC
	    WHERE (   SourceNodeId      IN (SELECT NodeId FROM #TempGetAllNodes)
	           OR DestinationNodeId IN (SELECT NodeId FROM #TempGetAllNodes)
	    	  )
          AND NC.IsDeleted = 0
     
		-- GET CATEGORY ID FOR ALL NODES (SOURCE AND DESTINATION NODES)
		IF OBJECT_ID('tempdb..#TempGetNodeCategory') IS NOT NULL
        DROP TABLE #TempGetNodeCategory

        SELECT * 
		INTO #TempGetNodeCategory 
		FROM (
				SELECT DISTINCT NT.NodeId,
							    NT.ElementId,
							    NT.CategoryId 
			   FROM 
			   (
						  SELECT SourceNodeId AS NodeId 
						  FROM #TempGetAllNodeConnections
						  UNION
						  SELECT DestinationNodeId AS NodeId 
						  FROM #TempGetAllNodeConnections
			   ) TEMP
			   INNER JOIN #TempGetNodeTagCategoryElement NT
			   ON TEMP.NodeId =NT.NodeId
		     ) SubQry
 
		-- FINAL SELECT QUERY
  		SELECT DISTINCT SourceNodeId
					   ,DestinationNodeId
					   ,CASE WHEN  IsActive = 1 AND IsTransfer = 1
							 THEN  'TransferPoint'
							 WHEN  IsActive = 1 AND IsTransfer <> 1
							 THEN  'Active'
							 WHEN  IsActive = 0 
							 THEN  'Inactive'
							 END AS [State]
                       ,[RowVersion]
		 FROM (SELECT  ND.SourceNodeId
					  ,ND.DestinationNodeId
					  ,ND.IsActive
					  ,ND.IsTransfer
					  ,Cat.CategoryId AS SourceCat
					  ,Cat.CategoryId AS DestCat
					  ,ND.[RowVersion]
				  FROM #TempGetAllNodeConnections ND 
				 INNER JOIN #TempGetNodeCategory Cat
					ON ND.SourceNodeId = Cat.NodeId 
				    OR ND.DestinationNodeId = Cat.NodeId
			  )Final  
	         

		-- DROP ALL TEMP TABLES
	    IF OBJECT_ID('tempdb..#TempGetNodeTagCategoryElement') IS NOT NULL
        DROP TABLE #TempGetNodeTagCategoryElement

      	IF OBJECT_ID('tempdb..#TempGetAllNodes') IS NOT NULL
        DROP TABLE #TempGetAllNodes

		IF OBJECT_ID('tempdb..#TempGetAllNodeConnections') IS NOT NULL
        DROP TABLE #TempGetAllNodeConnections

	    IF OBJECT_ID('tempdb..#TempGetNodeCategory') IS NOT NULL
        DROP TABLE #TempGetNodeCategory
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get the Graphical Node Connection details for a given Segment (ElementId) and Node.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetGraphicalNodeConnection',
    @level2type = NULL,
    @level2name = NULL