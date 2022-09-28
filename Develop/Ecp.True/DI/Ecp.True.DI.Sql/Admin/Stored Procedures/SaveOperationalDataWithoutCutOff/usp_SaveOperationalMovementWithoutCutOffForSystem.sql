/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jan-28-2020
-- Updated Date:	
-- Updated date: 	Apr-08-2020  -- Added SystemName as per PBI 28962
-- Updated date: 	Apr-22-2020  -- Added EventType Column
-- Updated date: 	Apr-23-2020  -- Removed Distinct to get all the Movement Details
-- Updated date: 	May-05-2020  -- Added new Column "MovementTransactionId", changed Join Criteria and added temp table
-- Updated date:    Jun-15-2020  -- Added batchid column as part of #PBI31874
                                 -- Modified sorting order as part of #PBI31874
                                 -- Updated scenarioid = 1 as part of #PBI31874
-- Updated date:    July-02-2020 -- Separated Common Code Portion and changed logic
-- Updated date:	July-20-2020 -- Added temp table To Improve Performance
-- Updated date:    Aug-04-2020  -- Changed input paramters from STRING to INTEGER AND removed delete old data
-- Updated date:    Aug-07-2020  -- Removing unrequired columns and conditions
-- Updated date:    Aug-13-2020  -- Added ISNULL condition
-- Description:     This Procedure is to Get TimeOne Movement Details Data for System Category, Element, Node, StartDate, EndDate.
-- EXEC  [Admin].[usp_SaveOperationalMovementWithoutCutOffForSystem] 'Sistema','Automation_vepja_System','Automation_2tvc6','2020-03-30','2020-04-01','49CA1512-8ACD-4105-9271-01648C1155CC'
-- EXEC  [Admin].[usp_SaveOperationalMovementWithoutCutOffForSystem] 'Sistema','Automation_vepja_System','Todos','2020-03-30','2020-04-01','49CA1512-8ACD-4105-9271-01648C1155CC'
   SELECT * FROM [Admin].[OperationalMovement] WHERE InputCategory = 'Sistema' AND ExecutionId = '49CA1512-8ACD-4105-9271-01648C1155CC'
-- ==============================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveOperationalMovementWithoutCutOffForSystem]
(
        @CategoryId                 INT 
       ,@ElementId                  INT 
       ,@NodeId                     INT 
       ,@StartDate                  DATE                      
       ,@EndDate                    DATE                      
       ,@ExecutionId                INT
)
AS
BEGIN
  SET NOCOUNT ON


			IF OBJECT_ID('tempdb..#MovTempIntialNodesForSystem')IS NOT NULL
			DROP TABLE #MovTempIntialNodesForSystem

			IF OBJECT_ID('tempdb..#MovTempFinalNodesForSystem')IS NOT NULL
			DROP TABLE #MovTempFinalNodesForSystem

			IF OBJECT_ID('tempdb..#MovTempNodeTagSystem')IS NOT NULL
			DROP TABLE #MovTempNodeTagSystem

			IF OBJECT_ID('tempdb..#MovTempNodeTagCalculationDate')IS NOT NULL
			DROP TABLE #MovTempNodeTagCalculationDate

			IF OBJECT_ID('tempdb..#MovTempProductsForSystem')IS NOT NULL
			DROP TABLE #MovTempProductsForSystem

			IF OBJECT_ID('tempdb..#MovTempResultForAllSystemData')IS NOT NULL
			DROP TABLE #MovTempResultForAllSystemData

			IF OBJECT_ID('tempdb..#TempAllOperationalMovement')IS NOT NULL
			DROP TABLE #TempAllOperationalMovement

			IF OBJECT_ID('tempdb..#MovTempResultForOneSystemData')IS NOT NULL
			DROP TABLE #MovTempResultForOneSystemData

			IF OBJECT_ID('tempdb..#TempOneOperationalMovement')IS NOT NULL
			DROP TABLE #TempOneOperationalMovement

			 IF OBJECT_ID('tempdb..#TempDistinctNodes')IS NOT NULL
			DROP TABLE #TempDistinctNodes

			IF OBJECT_ID('tempdb..#DQueryTemp')IS NOT NULL
			DROP TABLE #DQueryTemp

			IF OBJECT_ID('tempdb..#TempOmittedMovementTypesForSystem')IS NOT NULL
			DROP TABLE #TempOmittedMovementTypesForSystem
			
	     -- Variables Declaration			
			DECLARE @Previousdate  DATETIME =  [Admin].[udf_GetTrueDate] ()-1,
					@Todaysdate	   DATETIME =  [Admin].[udf_GetTrueDate] (),
                    @ElementName   NVARCHAR (250),
                    @NoOfDays	   INT

            SELECT @ElementName = CE.[Name]
            FROM [Admin].CategoryElement CE
            WHERE CE.[ElementId] = @ElementId
            AND CE.CategoryId    = @CategoryId 


			SELECT @NoOfDays = DATEDIFF(DAY,@StartDate,@EndDate)

			CREATE TABLE #MovTempIntialNodesForSystem
			(
				SegmentId			INT,
				InitialNodeId		INT,
				NodeName			NVARCHAR(250),
				CalculationDate		DATE
			)

			CREATE TABLE #MovTempFinalNodesForSystem
			(
				SegmentId			INT,
				FinalNodeId			INT,
				NodeName			NVARCHAR(250),
				CalculationDate		DATE
			)

			INSERT INTO #MovTempIntialNodesForSystem
			(
				 SegmentId	  
				,InitialNodeId			
				,NodeName		
				,CalculationDate
			)
			EXEC [Admin].[usp_GetInitialNodes] @ElementId,
											   @StartDate,
											   @EndDate

			INSERT INTO #MovTempFinalNodesForSystem
			(
				 SegmentId	  
				,FinalNodeId			
				,NodeName		
				,CalculationDate
			)
			EXEC [Admin].[usp_GetFinalNodes] @ElementId,
											 @StartDate,
											 @EndDate


			SELECT * INTO #MovTempNodeTagSystem 
			  FROM (
					SELECT DISTINCT NT.NodeId,
									NT.ElementId,
									Ce.Name AS ElementName,
									CASE WHEN NT.StartDate < @StartDate 
									     THEN @StartDate
									     ELSE NT.StartDate END
									       AS StartDate,
									CASE WHEN (   CAST(NT.EndDate AS DATE) > (CAST(@Todaysdate AS DATE)) 
									           OR CAST(NT.EndDate AS DATE) > (DATEADD(DAY,@NoOfDays,@StartDate))
											  )
										 THEN @EndDate
										 ELSE NT.EndDate 
										 END AS EndDate 
					FROM Admin.NodeTag NT
					INNER JOIN Admin.CategoryElement CE
					ON CE.ElementId = NT.ElementId
					WHERE NT.ElementId = @ElementId
					AND (NT.NodeId = @NodeId OR @NodeId = 0)
					AND CE.CategoryId = 8 
					) A                   	
	
			SELECT  NT.NodeId
				   ,NT.ElementId 
				   ,NT.ElementName
				   ,Ca.CalculationDate 
			INTO #MovTempNodeTagCalculationDate
			FROM #MovTempNodeTagSystem NT
			CROSS APPLY (SELECT	dates	AS  CalculationDate
						 FROM [Admin].[udf_GetAllDates]( NT.StartDate, 
												         NT.EndDate, 
												         NT.NodeId, 
												         NT.ElementId)
						) CA
			WHERE CA.CalculationDate BETWEEN @StartDate AND @EndDate


			SELECT           SubQ.CalculationDate
						    ,SubQ.NodeId
							,SubQ.ProductID
							,SubQ.ProductName
							,SubQ.SegmentID
							,SubQ.MovementTypeName
							,SubQ.MeasurementUnit
							,SubQ.MovementTransactionId
							,SubQ.EventType
							,SubQ.Batchid
			INTO #MovTempProductsForSystem
			FROM
			(
				SELECT			 	Mov.OperationalDate               AS CalculationDate,
									Mov.SourceProductId				  AS ProductID,
									Mov.SourceProductName			  AS ProductName,	
									Mov.SegmentId					  AS SegmentID,						
									Mov.SourceNodeId				  AS NodeId,
									Mov.MovementTypeName		      AS MovementTypeName,
									Mov.MeasurementUnit				  AS MeasurementUnit,
									Mov.MovementTransactionId,
									Mov.EventType,
									Mov.Batchid
				FROM  [Admin].[MovementInformationMovforSystemReport] Mov
				INNER JOIN #MovTempNodeTagCalculationDate NS    
                        ON NS.NodeId = Mov.SourceNodeId 
                       AND NS.CalculationDate = Mov.OperationalDate
				WHERE Mov.SourceProductId	IS NOT NULL
				AND Mov.SegmentId IS NOT NULL -- Segment Should not be NULL
				AND (Mov.SourceNodeId    = @NodeId OR @NodeId = 0)
				AND Mov.ExecutionId      = @ExecutionId
								
				UNION
				SELECT				Mov.OperationalDate					AS CalculationDate,
									Mov.DestinationProductId			AS ProductID,
									Mov.DestinationProductName		    AS ProductName,
									Mov.SegmentId					    AS SegmentID,						
									Mov.DestinationNodeId			    AS NodeId,
									Mov.MovementTypeName		        AS MovementTypeName,
									Mov.MeasurementUnit				    AS MeasurementUnit,
									Mov.MovementTransactionId,
									Mov.EventType,
									Mov.Batchid
				FROM [Admin].[MovementInformationMovforSystemReport] Mov
				 INNER JOIN #MovTempNodeTagCalculationDate NS 
                        ON NS.NodeId = Mov.DestinationNodeId 
                       AND NS.CalculationDate = Mov.OperationalDate
				WHERE Mov.DestinationProductId	IS NOT NULL
				AND Mov.SegmentId IS NOT NULL--Segment Should not be NULL
				AND (Mov.DestinationNodeId = @NodeId OR @NodeId = 0)
				AND Mov.ExecutionId      = @ExecutionId
			)SubQ

			SELECT DISTINCT NodeId 
			INTO #TempDistinctNodes
			FROM #MovTempNodeTagCalculationDate

			SELECT  DISTINCT 
				    DQuery.ProductID		        AS ProductID
				   ,DQuery.ProductName		        AS ProductName
				   ,DQuery.SegmentID		        AS SegmentID
				   ,DQuery.NodeId			        AS NodeId
				   ,DQuery.CalculationDate          AS CalculationDate
				   ,DQuery.MovementTransactionId	AS MovementTransactionId
				   ,DQuery.MovementTypeName			AS MovementTypeName
				   ,DQuery.MeasurementUnit			AS MeasurementUnit
				   ,DQuery.EventType                AS EventType
				   ,DQuery.BatchId
			INTO #DQueryTemp		
			FROM #MovTempProductsForSystem DQuery

			SELECT * INTO #TempOmittedMovementTypesForSystem 
			FROM (
					SELECT [Name] 
					FROM [Admin].[CategoryElement] 
					WHERE [CategoryID] = 9 AND ([ElementId] = 49 OR [ElementId] = 50)
				 ) TSubQ

IF (@NodeId = 0) -- CALCULATING THE DATA FOR ALL NODES
 BEGIN

			SELECT  DISTINCT
					Mov.MovementID						AS MovementID
				   ,Mov.MovementTransactionId           AS MovementTransactionId
				   ,Mov.SourceProductID					AS SourceProductID
				   ,Mov.SourceProductName				AS SourceProductName
				   ,Mov.DestinationProductId			AS DestinationProductId
				   ,Mov.DestinationProductName			AS DestinationProductName
				   ,Mov.SourceNodeId					AS SourceNodeId
				   ,Mov.SourceNodeName					AS SourceNodeName
				   ,Mov.DestinationNodeId				AS DestinationNodeId
				   ,Mov.DestinationNodeName				AS DestinationNodeName
				   ,DQuery.MovementTypeName				AS MovementTypeName
				   ,DQuery.MeasurementUnit				AS MeasurementUnit
				   ,DQuery.EventType                    AS EventType
				   ,DQuery.BatchId
				    ,[Mov].SourceSystem AS SystemName
					,CASE
					 WHEN Mov.MessageTypeId In (1,3)
					  AND ISNULL(Mov.DestinationNodeId,0) = ISNULL(IntialNodes.InitialNodeId,0) 
					  AND ISNULL(NT.ElementId,0)         = ISNULL(IntialNodes.SegmentId,0)
					  AND (   Mov.SourceNodeId NOT IN (SELECT NodeId FROM #TempDistinctNodes)
					       OR Mov.SourceNodeNameIsGeneric = 1
					      )  
					 	  
					  THEN 'Entradas' 
					  WHEN Mov.MessageTypeId In (1,3)
                          AND ISNULL(Mov.SourceNodeId,0) = ISNULL(FinalNodes.FinalNodeId,0)
					      AND ISNULL(NT.ElementId,0)    = ISNULL(FinalNodes.SegmentId,0)
                          AND (   Mov.DestinationNodeId NOT IN (SELECT NodeId FROM #TempDistinctNodes)
                               OR Mov.DestinationNodeNameIsGeneric = 1
                              )
                              THEN  'Salidas'
                         WHEN  ISNULL(Mov.MessageTypeId,0) = 2 --Loss
                           AND  Mov.SourceNodeId NOT IN (SELECT NodeId FROM #TempDistinctNodes)

				  	  THEN Mov.[Classification]
					  ELSE Mov.[Classification]
                      END
					  AS [Movement]
				   ,ISNULL(Mov.NetStandardVolume,0.00)	AS NetStandardVolume
				   ,ISNULL(Mov.GrossStandardVolume,0.00)AS GrossStandardVolume
				   ,ISNULL(Mov.UncertaintyPercentage,0.00) AS UncertaintyPercentage
				   ,ISNULL(Mov.MessageTypeId,0)         AS MessageTypeId
                   ,ISNULL(Mov.[Classification],'')     AS [Classification]
				   ,DQuery.ProductID					AS ProductID
				   ,DQuery.SegmentID					AS SegmentID
				   ,@ElementName						AS SegmentName
				   ,DQuery.NodeId						AS NodeId
				   ,DQuery.CalculationDate				AS CalculationDate
				   ,@ExecutionId AS ExecutionId
				   ,'ReportUser' AS CreatedBy	
            INTO #MovTempResultForAllSystemData					
			FROM #MovTempProductsForSystem DQuery
			INNER JOIN [Admin].[MovementInformationMovforSystemReport] Mov
			ON Mov.MovementTransactionId = DQuery.MovementTransactionId
			AND Mov.ExecutionId      = @ExecutionId
			LEFT JOIN #MovTempIntialNodesForSystem IntialNodes
		    ON   DQuery.CalculationDate = IntialNodes.CalculationDate
			AND Mov.DestinationNodeId = IntialNodes.InitialNodeId
            LEFT JOIN #MovTempFinalNodesForSystem FinalNodes
		    ON    DQuery.CalculationDate = FinalNodes.CalculationDate
			AND Mov.SourceNodeId = FinalNodes.FinalNodeId
			LEFT JOIN (select NT.NodeId, NT.ElementId 
			from admin.NodeTag NT
			inner join admin.CategoryElement CE on CE.ElementId = NT.ElementId and CE.CategoryId = 8) NT on NT.NodeId = DQuery.NodeId
			

			SELECT 	 DISTINCT
			         [BatchId]
					,[MovementId]
					,[MovementTransactionId]
					,[CalculationDate]
					,[MovementTypeName]
					,[SourceNodeName] 
					,[DestinationNodeName]
					,[SourceProductName]
					,[SourceProductID]
					,[DestinationProductName]
					,[DestinationProductId]
					,[NetStandardVolume]
					,[GrossStandardVolume]
					,[MeasurementUnit]
					,[EventType]
					,[SystemName]
					,[Movement]
					,[UncertaintyPercentage]
					,NULL AS [Uncertainty]
					,@ExecutionId AS ExecutionId
					,'ReportUser' AS CreatedBy
					,@Todaysdate  AS [CreatedDate]
			INTO #TempAllOperationalMovement
			FROM #MovTempResultForAllSystemData DQuery	
 

    		INSERT INTO [Admin].[OperationalMovement]
					   (
							 [RNo]
							,[BatchId]
							,[MovementId]
							,[MovementTransactionId]
							,[CalculationDate]
							,[MovementTypeName]
							,[SourceNode]
							,[DestinationNode]
							,[SourceProduct]
							,[DestinationProduct]
							,[NetStandardVolume]
							,[GrossStandardVolume]
							,[MeasurementUnit]
							,[EventType]
							,[SystemName]
							,[Movement]
							,[PercentStandardUnCertainty]
							,[Uncertainty]
							,[ProductId]
							,[ExecutionId]
							,[CreatedBy]
							,[CreatedDate]
					   ) 
	    SELECT ROW_NUMBER() OVER (ORDER BY [MovementId], [CalculationDate] ASC) AS RNo
	           ,* 
	      FROM (
				 SELECT      --DISTINCT
				            [BatchId]
							,[MovementId]
							,[MovementTransactionId]
							,[CalculationDate]
							,[MovementTypeName]
							,[SourceNodeName] 
							,[DestinationNodeName]
							,[SourceProductName]
							,[DestinationProductName]
							,[NetStandardVolume]
							,[GrossStandardVolume]
							,[MeasurementUnit]
							,[EventType]
							,[SystemName]
							,[Movement]
							,[UncertaintyPercentage]
							,[Uncertainty]
							,Prd.[ProductId]
							,[ExecutionId]
							,Mov.[CreatedBy]
							,@Todaysdate AS [CreatedDate]
				       FROM #TempAllOperationalMovement Mov
				       INNER JOIN [Admin].[Product] Prd
				       ON (   Mov.SourceProductId      = Prd.ProductId
				           OR Mov.DestinationProductId = Prd.ProductId
				          ) AND Mov.MovementTypeName NOT IN (SELECT [Name] 
														  FROM #TempOmittedMovementTypesForSystem)
                 )SubQ

  END  

ELSE -- CALCULATING THE DATA FOR SINGLE NODE
 BEGIN

			SELECT  DISTINCT
			        Mov.BatchId                         AS BatchId
				   ,Mov.MovementID						AS MovementID
				   ,Mov.MovementTransactionId           AS MovementTransactionId
				   ,Mov.SourceProductID					AS SourceProductID
				   ,Mov.SourceProductName				AS SourceProductName
				   ,Mov.DestinationProductId			AS DestinationProductId
				   ,Mov.DestinationProductName			AS DestinationProductName
				   ,Mov.SourceNodeId					AS SourceNodeId
				   ,Mov.SourceNodeName					AS SourceNodeName
				   ,Mov.DestinationNodeId				AS DestinationNodeId
				   ,Mov.DestinationNodeName				AS DestinationNodeName
				   ,DQuery.MovementTypeName				AS MovementTypeName
				   ,DQuery.MeasurementUnit				AS MeasurementUnit
				   ,DQuery.EventType                    AS EventType
				   ,[Mov].SourceSystem AS SystemName
				   ,ISNULL(Mov.NetStandardVolume,0.00)	AS NetStandardVolume
				   ,ISNULL(Mov.GrossStandardVolume,0.00)AS GrossStandardVolume
				   ,ISNULL(Mov.UncertaintyPercentage,0.00) AS UncertaintyPercentage
				   ,CASE
                        WHEN Mov.MessageTypeId IN (1,3)
                        AND ISNULL(Mov.DestinationNodeId,0) = @NodeId
                        AND ISNULL(Mov.SourceNodeId,0)     != @NodeId
                       THEN  'Entradas'
                       WHEN Mov.MessageTypeId IN (1,3)
                         AND ISNULL(Mov.SourceNodeId,0)       = @NodeId
                         AND ISNULL(Mov.DestinationNodeId,0) != @NodeId
                       THEN  'Salidas'
                       WHEN  ISNULL(Mov.MessageTypeId,0) = 2 --Loss
                        AND  ISNULL(Mov.SourceNodeId,0)       = @NodeId
                        AND  ISNULL(Mov.DestinationNodeId,0) != @NodeId
                       THEN  Mov.[Classification]
                    ELSE Mov.[Classification]
                    END AS [Movement]
				   ,DQuery.ProductID					AS ProductID
				   ,DQuery.SegmentID					AS SegmentID
				   ,@ElementName						AS SegmentName
				   ,DQuery.NodeId						AS NodeId
				   ,DQuery.CalculationDate				AS CalculationDate
				   ,@ExecutionId AS ExecutionId
				   ,'ReportUser' AS CreatedBy	
            INTO #MovTempResultForOneSystemData					
			FROM #DQueryTemp DQuery
			INNER JOIN [Admin].[MovementInformationMovforSystemReport] Mov
			ON Mov.MovementTransactionId = DQuery.MovementTransactionId
			AND Mov.ExecutionId      = @ExecutionId
			LEFT JOIN #MovTempNodeTagCalculationDate NTDate
			ON  NTDate.ElementId = DQuery.SegmentID
			AND NTDate.CalculationDate = DQuery.CalculationDate



            SELECT 	 DISTINCT
			         [BatchId]
					,[MovementId]
                    ,[MovementTransactionId]
					,[CalculationDate]
					,[MovementTypeName]
					,[SourceNodeName] 
					,[DestinationNodeName]
					,[SourceProductName]
					,[SourceProductID]
					,[DestinationProductName]
					,[DestinationProductId]
					,[NetStandardVolume]
					,[GrossStandardVolume]
					,[MeasurementUnit]
					,[EventType]
					,[SystemName]
					,[Movement]
					,[UncertaintyPercentage]
					,NULL AS [Uncertainty]
					,@ExecutionId AS ExecutionId
					,'ReportUser' AS CreatedBy
					,@Todaysdate  AS [CreatedDate]
			INTO #TempOneOperationalMovement
			FROM #MovTempResultForOneSystemData DQuery	


			INSERT INTO [Admin].[OperationalMovement]
					   (
							 [RNo]
							,[BatchId]
							,[MovementId]
							,[MovementTransactionId]
							,[CalculationDate]
							,[MovementTypeName]
							,[SourceNode]
							,[DestinationNode]
							,[SourceProduct]
							,[DestinationProduct]
							,[NetStandardVolume]
							,[GrossStandardVolume]
							,[MeasurementUnit]
							,[EventType]
							,[SystemName]
							,[Movement]
							,[PercentStandardUnCertainty]
							,[Uncertainty]
							,[ProductId]
							,[ExecutionId]
							,[CreatedBy]
							 ,[CreatedDate]
					   ) 
			SELECT ROW_NUMBER() OVER (ORDER BY [MovementId], [CalculationDate] ASC) AS RNo
			      ,* 
			  FROM (
			        SELECT   --DISTINCT
					         [BatchId]
							,[MovementId]
							,[MovementTransactionId]
							,[CalculationDate]
							,[MovementTypeName]
							,[SourceNodeName] 
							,[DestinationNodeName]
							,[SourceProductName]
							,[DestinationProductName]
							,[NetStandardVolume]
							,[GrossStandardVolume]
							,[MeasurementUnit]
							,[EventType]
							,[SystemName]
							,[Movement]
							,[UncertaintyPercentage]
							,[Uncertainty]
							,Prd.[ProductId]
							,[ExecutionId]
							,Mov.[CreatedBy]
							,@Todaysdate AS [CreatedDate]
				    FROM #TempOneOperationalMovement Mov
				    INNER JOIN [Admin].[Product] Prd
				    ON (   Mov.SourceProductId = Prd.ProductId
				        OR Mov.DestinationProductId = Prd.ProductId
				       ) AND Mov.MovementTypeName NOT IN (SELECT [Name] 
														  FROM #TempOmittedMovementTypesForSystem)
               )SubQ

  END

  END
  GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne Movement Details Data for System Category, Element, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveOperationalMovementWithoutCutOffForSystem',
    @level2type = NULL,
    @level2name = NULL
