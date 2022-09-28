/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Dec-18-2019
-- Updated Date:	Jan-29-2020; Added IsActive column in output.
-- Updated Date:	Feb-27-2020; Added @isInterface condition to conditionally suppress the Nodes result for ElementId 2 and 3.
-- Updated Date:	Mar-09-2020; Modified the SP to get all Nodes whose unique product count is > 1.
--					Aug-07-2020: Removed casting on operationalDate
-- Updated Date:	Sep-10-2020; Modified the SP to related to Function STRING_AGG
-- <Description>:   This Procedure is to Get all nodes which matches the business rules as mentioned below based on ElementId, StartDate, EndDate,IsInterface,IsDebug. </Description>
--	EXEC [Admin].[usp_GetAllNodesTest] 10,'2020-02-28', '2020-03-04', 1, 1
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetAllNodes]
(
	@catElementId INT,
	@startDate date,
	@endDate date,
	@isInterface bit = 0,
	@isDebug bit = 0		-- No need to pass this parameter when calling from API. This is used for debugging purpose, if 1 is passed it will emit all the interim resultsets.
)
AS

BEGIN
BEGIN TRY

	IF EXISTS(SELECT 1 FROM [Admin].[CategoryElement] WHERE ElementId = @catElementId)
	BEGIN

		IF OBJECT_ID('tempdb..#TempTableGAN1') IS NOT NULL
				DROP TABLE #TempTableGAN1
							   
		-- Process begins for resultset1	--> Creates list of nodes with overlapping each day logic for the given elementId.

			Select   NodeId
					,ElementId
					,CASE When StartDate >= SDInput Then StartDate
						  When SDInput > StartDate  Then SDInput
					 End  As NewStartDate
					,CASE When EndDate <= EDInput Then EndDate
						  When EDInput < EndDate  Then EDInput
					 End  As NewEndDate
					 ,ROW_NUMBER()OVER(ORDER BY NodeId Asc)RNUM
			INTO #TempTableGAN1
				from 
				(
				Select  NodeId
						,ElementId
						,Cast (StartDate As Date ) as StartDate
						,@startDate as SDInput
						,Cast (EndDate as Date) as EndDate
						,@endDate as EDInput 
						from Admin.NodeTag 
						Where ElementId = @catElementId 
							  And NOT (		-- This includes any overlapping dates
								(@endDate < StartDate)
										OR
								(@startDate > EndDate)
					)
				) A

		declare @resultset1 As table (nodeid int, elementid int, dates Date);

		declare	  @Cnt INT  = (SELECT COUNT(*) FROM #TempTableGAN1)
		WHILE @Cnt>0
		BEGIN
			DECLARE @ElementId INT, @NodeID INT, @newStartDate date, @newEndDate date;

			SELECT @NodeID = NodeId, @ElementId = ElementId, @newStartDate = NewStartDate, @newEndDate = NewEndDate
			FROM #TempTableGAN1
			WHERE Rnum = @Cnt
			SELECT @Cnt = @Cnt-1

			INSERT INTO @resultset1
			SELECT * FROM [Admin].[udf_GetAllDates](@newStartDate, @newEndDate, @NodeID, @ElementId) C

		END

		IF (@isDebug = 1)
		BEGIN
			Select o.dates, o.nodeid, n.Name, o.elementid 
			from @resultset1 o
			INNER JOIN Admin.Node n
				On o.nodeid = n.NodeId
		END

	

		--------------------------------------------------------------------------------------------

		-- Process begins for resultset2 (#TempTableGAN2)		--> List of nodes with their distinct products either from MovementSource or MovementDestination.

		IF (@isInterface = 1)
		BEGIN
			IF OBJECT_ID('tempdb..#TempTableGAN2') IS NOT NULL
					DROP TABLE #TempTableGAN2

			Select NodeId, ProductId, Movement
			INTO #TempTableGAN2
			FROM (
					Select   distinct
							 ms.SourceNodeId			As NodeId
							,ms.SourceProductId			As ProductId
							,'MovementSource'			As Movement
					from Offchain.MovementSource ms
					Inner Join Offchain.Movement mov
						On mov.MovementTransactionId = ms.MovementTransactionId
					Where mov.OperationalDate between @startDate And @endDate		-- operationdate between SD and ED

							UNION

					Select   distinct
							 md.DestinationNodeId		As NodeId
							,md.DestinationProductId	As ProductId
							,'MovementDestination'		As Movement
					from Offchain.MovementDestination md
					Inner Join Offchain.Movement mov
						On mov.MovementTransactionId = md.MovementTransactionId
					Where mov.OperationalDate  between @startDate And @endDate		-- operationdate between SD and ED
			) A
			WHERE	NodeId		IS NOT NULL 
		
			IF (@isDebug = 1)
			BEGIN
				Select o.NodeId, o.ProductId, o.Movement
				from #TempTableGAN2 o			
			END
		END

		--------------------------------------------------------------------------------------------

		-- Process begins for resultset3 (#TempTableGAN3)		--> Gets all the nodes whose Product count from Movement tables is > 1.

		IF (@isInterface = 1)
		BEGIN
			IF OBJECT_ID('tempdb..#TempTableGAN3') IS NOT NULL
					DROP TABLE #TempTableGAN3

			Select   NodeId
					,STRING_AGG(CAST(ProductId AS NVARCHAR(MAX)), '__') As ProductIds
					,MAX(RNum) As CountOfUniqueProduct
			INTO #TempTableGAN3
			FROM
			(
				Select   resultset2.nodeid				As NodeId
						,resultset2.ProductId			As ProductId
						,COUNT(resultset2.ProductId)	As CountOfProduct
						,Row_Number() Over (Partition By resultset2.NodeId Order By resultset2.ProductId) As RNum
				from #TempTableGAN2 resultset2
				INNER JOIN (Select Distinct NodeId from @resultset1) rs1
					ON rs1.nodeid = resultset2.NodeId
				Group By resultset2.ProductId, resultset2.NodeId
			) A
			Group By NodeId
			HAVING MAX(RNum) > 1

			IF (@isDebug = 1)
			BEGIN
				Select o.NodeId, o.ProductIds, o.CountOfUniqueProduct
				from #TempTableGAN3 o
			END
		END

		--------------------------------------------------------------------------------------------
		
		-- Process begins for resultset4		--> Final list of filtered nodes with other columns.

		declare @resultset4 As table (dates Date, nodeid int);

		IF (@isInterface = 1)
		BEGIN
			INSERT INTO @resultset4
			Select rs1.dates, rs1.nodeid from @resultset1 rs1
			INNER JOIN #TempTableGAN3 resultset3
				ON resultset3.NodeId = rs1.nodeid
		END
		ELSE
		BEGIN
			INSERT INTO @resultset4
			Select rs1.dates, rs1.nodeid from @resultset1 rs1
		END


		Select	 o.dates							AS CalculationDate
				,o.nodeid							AS NodeID
				,n.[Name]							AS [Name]
				,n.ControlLimit						AS [ControlLimit]
				,n.AcceptableBalancePercentage		AS [AcceptableBalancePercentage]
				,n.IsActive							AS [IsActive]
		from @resultset4 o
		INNER JOIN Admin.Node n
			On o.nodeid = n.NodeId
		--------------------------------------------------------------------------------------------

		-- Dropping up tempTables.

		IF OBJECT_ID('tempdb..#TempTableGAN1') IS NOT NULL
			DROP TABLE #TempTableGAN1
		
		IF OBJECT_ID('tempdb..#TempTableGAN2') IS NOT NULL
				DROP TABLE #TempTableGAN2

		IF OBJECT_ID('tempdb..#TempTableGAN3') IS NOT NULL
				DROP TABLE #TempTableGAN3
		--------------------------------------------------------------------------------------------
	END
	ELSE
	BEGIN
		RAISERROR ('Invalid CategoryElementId',1,1) 
	END
END TRY

       BEGIN CATCH
             THROW
       END CATCH
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get all nodes which matches the business rules as mentioned below based on ElementId, StartDate, EndDate,IsInterface,IsDebug.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetAllNodes',
    @level2type = NULL,
    @level2name = NULL