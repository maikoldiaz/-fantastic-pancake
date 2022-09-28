/*-- =========================================================================================================================================
-- Author:          Microsoft
-- Created date: 	Mar-09-2020
-- Updated Date:	Mar-20-2020
-- <Description>:   This procedure is used to get the Graphical Source Node Connection details for a given Destination Node. </Description>
EXEC [Admin].[usp_GetGraphicalSourceNodeConnectionsDetails] 6128
-- ==========================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetGraphicalSourceNodeConnectionsDetails]
(
	    @NodeId	    INT
)
AS
BEGIN
	SET NOCOUNT ON

		-- GET NODE CONNECTION DETAILS WHERE INPUT NODE IS DESTINATION NODE
	 	IF OBJECT_ID('tempdb..#TempSourceNodeConnections') IS NOT NULL
        DROP TABLE #TempSourceNodeConnections

	    SELECT  SourceNodeId
	           ,DestinationNodeId
	    	   ,CASE WHEN  IsActive = 1 AND IsTransfer = 1
							 THEN  'TransferPoint'
							 WHEN  IsActive = 1 AND IsTransfer <> 1
							 THEN  'Active'
							 WHEN  IsActive = 0 
							 THEN  'Inactive'
							 END AS [State]
              ,[RowVersion]
        INTO #TempSourceNodeConnections
	    FROM [Admin].[NodeConnection] NC
	    WHERE DestinationNodeId = @NodeId
		  AND NC.IsDeleted = 0
	    	  
   -- NODETAG CATEGORY ELEMENT TEMP TABLE
	    IF OBJECT_ID('tempdb..#TempGetNodeTagCategoryElement') IS NOT NULL
        DROP TABLE #TempGetNodeTagCategoryElement
		
		SELECT   CE.ElementId
				,CE.[Name] AS [Name]
				,CE.CategoryId
				,NT.NodeId
 		INTO #TempGetNodeTagCategoryElement
		FROM [Admin].[NodeTag] NT
		JOIN [Admin].[CategoryElement] CE
		ON NT.ElementId = CE.ElementId
		JOIN #TempSourceNodeConnections SrcNodes
		  ON NT.NodeId = SrcNodes.SourceNodeId
        WHERE CAST((SELECT [Admin].[udf_GETTRUEDATE]()) AS DATE) BETWEEN CAST(NT.StartDate AS DATE) 
		                                                           AND CAST(NT.EndDate AS DATE)

		-- GET SOURCE AND DESTINATION NODE DETAILS WHERE SOURCE NODE OF GIVEN DESTINATION NODE
	 	IF OBJECT_ID('tempdb..#TempGetAllNodeConnections') IS NOT NULL
        DROP TABLE #TempGetAllNodeConnections

	    SELECT  SourceNodeId
	           ,DestinationNodeId
	    	   ,IsActive
	    	   ,IsTransfer
			   ,[RowVersion]
        INTO #TempGetAllNodeConnections
	    FROM [Admin].[NodeConnection] NC
	    WHERE (   SourceNodeId      IN (SELECT SourceNodeId FROM #TempSourceNodeConnections)
	           OR DestinationNodeId IN (SELECT SourceNodeId FROM #TempSourceNodeConnections)
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
		SELECT SourceNodeId,DestinationNodeId,[State],[RowVersion] FROM #TempSourceNodeConnections
		UNION
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
					  ,NDCat.CategoryId AS SourceCat
					  ,NDCat.CategoryId AS DestCat 
					  ,ND.[RowVersion]
				  FROM #TempGetAllNodeConnections ND 
				 INNER JOIN #TempGetNodeCategory NDCat
					ON (  ND.SourceNodeId=NDCat.NodeId 
				       OR ND.DestinationNodeId=NDCat.NodeId
					   )
			  )Final  
	         

		-- DROP ALL TEMP TABLES
	 	IF OBJECT_ID('tempdb..#TempSourceNodeConnections') IS NOT NULL
        DROP TABLE #TempSourceNodeConnections

	    IF OBJECT_ID('tempdb..#TempGetNodeTagCategoryElement') IS NOT NULL
        DROP TABLE #TempGetNodeTagCategoryElement

		IF OBJECT_ID('tempdb..#TempGetAllNodeConnections') IS NOT NULL
        DROP TABLE #TempGetAllNodeConnections

	    IF OBJECT_ID('tempdb..#TempGetNodeCategory') IS NOT NULL
        DROP TABLE #TempGetNodeCategory
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This procedure is used to get the Graphical Source Node Connection details for a given Destination Node.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetGraphicalSourceNodeConnectionsDetails',
    @level2type = NULL,
    @level2name = NULL