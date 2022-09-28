/*-- ============================================================================================================================
-- Author:          Intergrupo
-- Created Date:	Nov-10-2021
-- <Description>:   This return if the node is a Product Conversion.</Description>
-- ==============================================================================================================================*/

CREATE FUNCTION [Admin].[udf_NodeIsProductConversion](
@CategoryId INT ,
@NodeSource Nvarchar(MAX),
@NodeDestination Nvarchar(MAX),
@StartDate DATE,
@EndDate DATE,
@ExecutionId INT)
RETURNS INT
AS
BEGIN
	DECLARE @ProductConversionCount INT = 0;


	if @CategoryId = 2
	begin
		SET @ProductConversionCount = (select sum(NodeCount) from (Select count(N.NodeId) NodeCount
										 from [Admin].[Node] N
										 where N.[Name] = @NodeSource 
										   and N.NodeId In (Select Distinct NodeId 
															from [Admin].[NodeTagCalculationDateForSegmentReport] 
															where ExecutionId = @ExecutionId and CalculationDate between @StartDate and @EndDate)
										union all
										Select count(N.NodeId) + iif(count(N.NodeId)=0,0,1)
											from [Admin].[Node] N
											where N.[Name] = @NodeDestination 
											and N.NodeId In (Select Distinct NodeId 
															from [Admin].[NodeTagCalculationDateForSegmentReport] 
															where ExecutionId = @ExecutionId and CalculationDate between @StartDate and @EndDate)) a)
	end

	if @CategoryId = 8
	begin
		SET @ProductConversionCount = (select sum(NodeCount) from (Select count(N.NodeId) NodeCount
										 from [Admin].[Node] N
										 where N.[Name] = @NodeSource 
										   and N.NodeId In (Select Distinct NodeId 
															from [Admin].[NodeTagCalculationDateForSystemReport] 
															where ExecutionId = @ExecutionId and CalculationDate between @StartDate and @EndDate)
										union all
										Select count(N.NodeId) + iif(count(N.NodeId)=0,0,1)
											from [Admin].[Node] N
											where N.[Name] = @NodeDestination 
											and N.NodeId In (Select Distinct NodeId 
															from [Admin].[NodeTagCalculationDateForSystemReport] 
															where ExecutionId = @ExecutionId and CalculationDate between @StartDate and @EndDate)) a)
	end

	RETURN @ProductConversionCount;
END
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This return if the node is a Product Conversion.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'FUNCTION',
    @level1name = N'udf_NodeIsProductConversion',
    @level2type = NULL,
    @level2name = NULL