/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Mar-09-2020
-- Updated Date:	Mar-20-2020
-- <Description>:   This procedure is used to get the Graphical Source Node details for a given Destination Node. </Description>
EXEC [Admin].[usp_GetGraphicalSourceNodesDetails] 2200
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetGraphicalSourceNodesDetails]
(
	     @NodeId	    INT
)
AS
BEGIN
	SET NOCOUNT ON
    
	-- NODETAG CATEGORY ELEMENT TEMP TABLE
	    IF OBJECT_ID('tempdb..#TempGetNodeTagCategoryElement') IS NOT NULL
        DROP TABLE #TempGetNodeTagCategoryElement
		
		SELECT DISTINCT CE.ElementId
		               ,CE.[Name]
			           ,CE.CategoryId
					   ,CE.IsActive
			           ,CE.IconId
			           ,CE.Color
					   ,NT.NodeId
        INTO #TempGetNodeTagCategoryElement
		FROM [Admin].[NodeTag] NT
		JOIN [Admin].[CategoryElement] CE
		ON NT.ElementId = CE.ElementId
	   WHERE (NT.NodeId IN (SELECT SourceNodeId 
		                       FROM [Admin].[NodeConnection] 
		                      WHERE DestinationNodeId = @NodeId
							    AND IsDeleted = 0
							)
               OR NT.NodeId = @NodeId
			   ) 
		  AND CAST((SELECT [Admin].[udf_GETTRUEDATE]()) AS DATE) BETWEEN CAST(NT.StartDate AS DATE) 
		                                                             AND CAST(NT.EndDate AS DATE)
          AND CE.CategoryId IN (1,2,3) 

     -- GET NODE DETAILS
	    IF OBJECT_ID('tempdb..#TempGetNodeDetails') IS NOT NULL
        DROP TABLE #TempGetNodeDetails

		CREATE TABLE #TempGetNodeDetails 
        (
             NodeId                      INT
		    ,NodeName                    NVARCHAR (200)
			,AcceptableBalancePercentage DECIMAL(29,2)
			,ControlLimit                DECIMAL(29,2)
			,ElementId                   INT
			,CategoryId                  INT
        	,Segment                     NVARCHAR (200)
        	,Operator                    NVARCHAR (200)
        	,NodeType                    NVARCHAR (200)
        	,SegmentColor                NVARCHAR (200)
        	,NodeTypeIcon                NVARCHAR (MAX)
        	,IsActive                    INT
			,[Order]                     INT
       	)

        INSERT INTO #TempGetNodeDetails 
        (
            NodeId,
            NodeName,
            AcceptableBalancePercentage,
            ControlLimit,
            IsActive,
			[Order]
        )                  
        SELECT          ND.NodeId
                       ,ND.[Name]  AS NodeName
        	           ,ND.AcceptableBalancePercentage
        	           ,ND.ControlLimit
					   ,ND.IsActive
					   ,ND.[Order]
           FROM [Admin].[Node] ND
		   WHERE (ND.NodeId IN (SELECT SourceNodeId 
		                       FROM [Admin].[NodeConnection] 
		                      WHERE DestinationNodeId = @NodeId
							    AND IsDeleted = 0
							)
               OR ND.NodeId = @NodeId
               )
 

        -- UPDATE "OPERADOR" COLUMN 
        UPDATE MainTab
        SET Operator = A.Operator
	   FROM #TempGetNodeDetails MainTab
        JOIN (SELECT NodeId
		            ,[Name] AS Operator
			        ,CategoryId
					,ElementId
		      FROM #TempGetNodeTagCategoryElement
              WHERE NodeId IN (SELECT NodeId  FROM #TempGetNodeDetails )
		    )A
        ON MainTab.NodeId = A.NodeId
		WHERE A.CategoryId = 3 -- Operator

        -- UPDATE "Segment, SegmentColor" COLUMN 
        UPDATE MainTab
        SET Segment      = A.NodeType
		  , SegmentColor = A.SegmentColor
        FROM #TempGetNodeDetails MainTab
        JOIN (SELECT NodeId
		            ,CE.[Name] AS NodeType
			        ,CE.CategoryId
			        ,CE.Color AS SegmentColor
		      FROM #TempGetNodeTagCategoryElement CE
              WHERE NodeId IN (SELECT NodeId FROM #TempGetNodeDetails )
		  )A
        ON MainTab.NodeId = A.NodeId
		WHERE A.CategoryId = 2 -- SegmentName
		
        -- UPDATE "NODE TYPE" COLUMN 
        UPDATE MainTab
        SET NodeType = A.NodeType
		    ,NodeTypeIcon = Content
        FROM #TempGetNodeDetails MainTab
        JOIN (SELECT NodeId
		            ,CE.[Name] AS NodeType
			        ,CE.CategoryId
			        ,Icon.Content
		      FROM #TempGetNodeTagCategoryElement CE
              LEFT JOIN [Admin].[Icon] Icon
		      ON Icon.IconId = CE.IconId
              WHERE NodeId IN (SELECT NodeId FROM #TempGetNodeDetails )
		  )A
        ON MainTab.NodeId = A.NodeId
		WHERE A.CategoryId = 1 -- Tipo de Nodo

     --FINAL SELECT QUERY 
       SELECT NodeId
             ,NodeName
             ,AcceptableBalancePercentage
    		 ,ControlLimit
    		 ,Segment
    		 ,Operator 
    		 ,NodeType
    		 ,SegmentColor
    		 ,NodeTypeIcon
    		 ,CAST(IsActive AS BIT)  AS IsActive
			 ,[Order]
    	FROM #TempGetNodeDetails

   -- DROP ALL TEMP TABLES
	    IF OBJECT_ID('tempdb..#TempGetNodeTagCategoryElement') IS NOT NULL
        DROP TABLE #TempGetNodeTagCategoryElement

	    IF OBJECT_ID('tempdb..#TempGetNodeDetails') IS NOT NULL
        DROP TABLE #TempGetNodeDetails
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This procedure is used to get the Graphical Source Node details for a given Destination Node.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetGraphicalSourceNodesDetails',
    @level2type = NULL,
    @level2name = NULL