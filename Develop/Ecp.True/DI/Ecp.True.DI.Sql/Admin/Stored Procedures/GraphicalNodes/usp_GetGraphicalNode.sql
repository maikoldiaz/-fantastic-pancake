/*-- ============================================================================================================================
-- Author:				Microsoft
-- Created Date: 		Feb-17-2020
-- Updated Date:		Mar-20-2020
-- Modification Date:   Mar-31-2020		-- Added SegmentId, OperatorId, NodeTypeId, SendToSAP, LogisticCenterId, [Order], UnitId, Capacity
-- Modification Date:   Apr-01-2020		-- Added Columns LogisticCenter and Unit, Added Rowversion
-- Modification Date:   Apr-01-2020		-- Removed all newly added columns except Order
-- <Description>:   This Procedure is used to get the Graphical Node details for a given Segment (ElementId) and Node. </Description>
EXEC [Admin].[usp_GetGraphicalNode] 18887,0 -- Segment Category
EXEC [Admin].[usp_GetGraphicalNode] 18900,0 -- System Category
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetGraphicalNode]
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
		
		SELECT CE.ElementId
		      ,CE.[Name]
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
        WHERE (NT.NodeId = @NodeId OR @NodeId = 0)
        AND CAST((SELECT [Admin].[udf_GETTRUEDATE]()) AS DATE) BETWEEN CAST(NT.StartDate AS DATE) AND CAST(NT.EndDate AS DATE)

     -- GET NODE DETAILS
	    IF OBJECT_ID('tempdb..#TempGetNodeDetails') IS NOT NULL
        DROP TABLE #TempGetNodeDetails

		CREATE TABLE #TempGetNodeDetails 
        (
             NodeId                      INT
		    ,NodeName                    NVARCHAR (300)
			,AcceptableBalancePercentage DECIMAL(29,2)
			,ControlLimit                DECIMAL(29,2)
			,ElementId                   INT
			,CategoryId                  INT
        	,Segment                     NVARCHAR (300)
        	,Operator                    NVARCHAR (300)
        	,NodeType                    NVARCHAR (300)
        	,SegmentColor                NVARCHAR (300)
        	,NodeTypeIcon                NVARCHAR (MAX)
        	,IsActive                    INT
			,[Order]					 INT

       	)

        INSERT INTO #TempGetNodeDetails 
        (
            NodeId,
            NodeName,
            AcceptableBalancePercentage,
            ControlLimit,
            CategoryId,
            IsActive,
			[Order]
        )                  
        SELECT          ND.NodeId
                       ,ND.[Name]  AS NodeName
        	           ,ND.AcceptableBalancePercentage
        	           ,ND.ControlLimit
					   ,CE.CategoryId
					   ,ND.IsActive
					   ,ND.[Order]
        FROM #TempGetNodeTagCategoryElement CE
        INNER JOIN [Admin].[Node] ND
        ON ND.NodeId = CE.NodeId
        WHERE CE.ElementId = @ElementId
        AND (CE.NodeId = @NodeId OR @NodeId = 0)
        AND CAST((SELECT [Admin].[udf_GETTRUEDATE]()) AS DATE) BETWEEN CAST(CE.StartDate AS DATE) AND CAST(CE.EndDate AS DATE)
        
        -- UPDATE "OPERADOR" COLUMN 
        UPDATE MainTab
        SET Operator = A.Operator
        FROM #TempGetNodeDetails MainTab
        JOIN (SELECT NodeId
		            ,[Name] AS Operator
			        ,CategoryId
		      FROM #TempGetNodeTagCategoryElement
              WHERE NodeId IN (SELECT NodeId  FROM #TempGetNodeDetails )
		    )A
        ON MainTab.NodeId = A.NodeId
		WHERE A.CategoryId = 3 -- Operator

        -- UPDATE "Segment / Segment Color" COLUMN 
        UPDATE MainTab
        SET Segment      = A.Segement
		  , SegmentColor = A.Color
        FROM #TempGetNodeDetails MainTab
        JOIN (SELECT NodeId
		            ,CE.[Name] AS Segement
					,CE.Color
			        ,CE.CategoryId
			    FROM #TempGetNodeTagCategoryElement CE
               WHERE NodeId IN (SELECT NodeId FROM #TempGetNodeDetails )
		  )A
        ON MainTab.NodeId = A.NodeId
		WHERE A.CategoryId = 2 -- Segment
	
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
    @value = N'This Procedure is used to get the Graphical Node details for a given Segment (ElementId) and Node.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetGraphicalNode',
    @level2type = NULL,
    @level2name = NULL