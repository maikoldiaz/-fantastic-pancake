/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jan-04-2020
-- Updated Date:	Mar-20-2020
-- <Description>:	This Procedure is used to get all the Systems for a given Segment, StartDate and EndDate. </Description>
-- EXEC [Admin].[usp_GetAllSystemInASegment] 9796,'2019-12-30','2020-01-02'
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetAllSystemInASegment]
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

			IF OBJECT_ID('tempdb..#TempPreResultSystemData') IS NOT NULL
			DROP TABLE #TempPreResultSystemData

			IF OBJECT_ID('tempdb..#TempAllSystemInASegment') IS NOT NULL
			DROP TABLE #TempAllSystemInASegment

			CREATE TABLE #TempAllSystemInASegment
			(
				SystemId		INT,
				SystemName		NVARCHAR (150),
				OperationDate	DATE
			)

	   IF @StartDate <= @EndDate
	   BEGIN

			SELECT   NT.NodeId
			INTO #TempNodeTag
			FROM Admin.NodeTag NT
			INNER JOIN Admin.CategoryElement CatEle
			ON CatEle.ElementId = NT.ElementId
			WHERE NT.ElementId = @SegmentId
			AND (	 (@StartDate BETWEEN NT.StartDate  AND Nt.EndDate )
				  OR (@EndDate   BETWEEN NT.StartDate  AND Nt.EndDate )
				)


			SELECT   Nt.ElementId AS SystemId
					,CatEle.Name  AS SystemName
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
			INTO #TempPreResultSystemData
			FROM Admin.NodeTag NT
			INNER JOIN #TempNodeTag TempNT
			ON NT.NodeId = TempNT.NodeId
			INNER JOIN Admin.CategoryElement CatEle
			ON CatEle.ElementId = NT.ElementId
			WHERE CatEle.CategoryId = 8 --Sistema (Category id of type System)
			AND (	 (@StartDate BETWEEN NT.StartDate  AND Nt.EndDate )
				 OR (@EndDate    BETWEEN NT.StartDate  AND Nt.EndDate )
				)

			SET	  @Cnt  = (SELECT COUNT(1) 
						   FROM #TempPreResultSystemData)	

			WHILE @Cnt>0
			BEGIN
				SELECT  @NodeID = SystemId, 
						@ElementId = @SegmentId, 
						@newStartDate = StartDate, 
						@newEndDate = EndDate
				FROM #TempPreResultSystemData
				WHERE Rnum = @Cnt

				SELECT @Cnt = @Cnt-1

				INSERT INTO #TempAllSystemInASegment
				(
					 SystemId		
					,SystemName		
					,OperationDate		
				)
				SELECT   tempData.SystemId		AS 	SystemId
						,tempData.SystemName	AS  SystemName
						,udf.dates				AS  OperationDate
				FROM #TempPreResultSystemData tempData
				INNER JOIN [Admin].[udf_GetAllDates]( @newStartDate, @newEndDate, @NodeID, @ElementId)udf
				ON tempData.SystemId  = udf.nodeId


			END

			SELECT 	 DISTINCT
					 SystemId		
					,SystemName		
					,OperationDate	
			FROM #TempAllSystemInASegment
	END
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get all the Systems for a given Segment, StartDate and EndDate.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetAllSystemInASegment',
    @level2type = NULL,
    @level2name = NULL