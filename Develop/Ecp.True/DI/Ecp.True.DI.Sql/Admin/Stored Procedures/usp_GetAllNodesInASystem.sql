/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jan-04-2020
-- Updated Date:	Mar-20-2020
-- <Description>:   This Procedure is used to get all Nodes present in a System based on SystemId, StartDate, EndDate. </Description>
-- EXEC [Admin].[usp_GetAllNodesInASystem] 9870,'2019-12-30','2020-01-02'
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetAllNodesInASystem]
(
	   @SystemId		INT  ,
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

			IF OBJECT_ID('tempdb..#TempPreResultAllNodesInASystem') IS NOT NULL
			DROP TABLE #TempPreResultAllNodesInASystem

			IF OBJECT_ID('tempdb..#TempAllNodesInASystem') IS NOT NULL
			DROP TABLE #TempAllNodesInASystem

			CREATE TABLE #TempAllNodesInASystem
			(
				NodeId			INT,
				NodeName		NVARCHAR (150),
				OperationDate	DATE
			)


		   IF @StartDate <= @EndDate
		   BEGIN

				SELECT   NT.NodeId
						,Nd.Name AS NodeName
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
				INTO #TempPreResultAllNodesInASystem
				FROM Admin.NodeTag NT
				INNER JOIN Admin.CategoryElement CatEle
				ON CatEle.ElementId = NT.ElementId
				INNER JOIN Admin.Node ND
				ON Nd.NodeId = NT.NodeId
				WHERE NT.ElementId = @SystemId
				AND (	 (@StartDate BETWEEN NT.StartDate  AND Nt.EndDate )
					  OR (@EndDate   BETWEEN NT.StartDate  AND Nt.EndDate )
					)

				SET	  @Cnt  = (SELECT COUNT(1) 
							   FROM #TempPreResultAllNodesInASystem
							   )	

			WHILE @Cnt>0
			BEGIN
				SELECT  @NodeID = NodeId, 
						@ElementId = @SystemId, 
						@newStartDate = StartDate, 
						@newEndDate = EndDate
				FROM #TempPreResultAllNodesInASystem
				WHERE Rnum = @Cnt

				SELECT @Cnt = @Cnt-1

				INSERT INTO #TempAllNodesInASystem
				(
						 NodeId			
						,NodeName		
						,OperationDate	
				)
				SELECT   tempData.NodeId		AS 	SystemId
						,tempData.NodeName		AS  SystemName
						,udf.dates				AS  OperationDate
				FROM #TempPreResultAllNodesInASystem tempData
				INNER JOIN [Admin].[udf_GetAllDates]( @newStartDate, @newEndDate, @NodeID, @ElementId)udf
				ON tempData.NodeId  = udf.nodeId


			END

			SELECT 	 DISTINCT
						 NodeId			
						,NodeName		
						,OperationDate
			FROM #TempAllNodesInASystem
		   END
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get all Nodes present in a System based on SystemId, StartDate, EndDate.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetAllNodesInASystem',
    @level2type = NULL,
    @level2name = NULL