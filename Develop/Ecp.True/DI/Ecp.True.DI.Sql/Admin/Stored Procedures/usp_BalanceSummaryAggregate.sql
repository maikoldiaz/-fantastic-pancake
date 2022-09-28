
/*-- =================================================================================================================================================================================================================================================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jan-27-2020
-- Updated Date:	Mar-20-2020
-- <Description>:   This Procedure is used to get all the sum of Balance Summary for all the products with ownership for a node. This SP is an extension to the existing SP [usp_BalanceSummaryWithOwnership]. This SP should sum up all the rows of the output of SP [usp_BalanceSummaryWithOwnership], so it should return only one single row. </Description>
-- Design:			It is designed to use the existing SP [usp_BalanceSummaryWithOwnership] and dump its output into a temp memory table, 
					then further using CTE do the required aggregation to sum all the columns.
					Control is calculated by Volume/Inputs (if Inputs = 0.0, then send error as value).
-- ==================================================================================================================================================================================================================================================================================================================================================================*/


CREATE PROCEDURE [Admin].[usp_BalanceSummaryAggregate]
(
	   @OwnershipNodeId		INT
)
AS
BEGIN

DECLARE @OwnershipStatusId INT = (SELECT OwnershipStatusId
										FROM [Admin].[OwnershipNode]
										WHERE OwnershipNodeId = @OwnershipNodeId);

DECLARE @tempSPOutput AS TABLE ( 
								 ProductId			NVARCHAR(50)
								,Product			NVARCHAR(150)
								,[Owner]			NVARCHAR(150)
								,InitialInventory 	DECIMAL(18, 2) 
								,Inputs 			DECIMAL(18, 2) 
								,Outputs 			DECIMAL(18, 2) 
								,IdentifiedLosses 	DECIMAL(18, 2) 
								,Interface 			DECIMAL(18, 2) 
								,Tolerance 			DECIMAL(18, 2) 
								,UnidentifiedLosses DECIMAL(18, 2) 
								,FinalInventory 	DECIMAL(18, 2) 
								,Volume 			DECIMAL(18, 2) 
								,MeasurementUnit	NVARCHAR(150)
								,[Control]			DECIMAL(18, 2)
							);

INSERT INTO @tempSPOutput EXEC [Admin].[usp_BalanceSummaryWithOwnership] @OwnershipNodeId;

WITH Cte AS(
	SELECT 
			 SUM( [InitialInventory]		 )	 AS [InitialInventory]
			,SUM( [Inputs]					 )	 AS [Inputs]
			,SUM( [Outputs]					 )	 AS [Outputs]
			,SUM( [IdentifiedLosses]		 )	 AS [IdentifiedLosses]
			,SUM( [Interface]				 )	 AS [Interface]
			,SUM( [Tolerance]				 )	 AS [Tolerance]
			,SUM( [UnidentifiedLosses]		 )	 AS [UnidentifiedLosses]
			,SUM( [FinalInventory]			 )	 AS [FinalInventory]
			,SUM( [Volume]					 )	 AS [Volume]
			,'Bbl'								 AS [MeasurementUnit]

			FROM @tempSPOutput
)


	SELECT 
		 [InitialInventory]
		,[Inputs]
		,[Outputs]
		,[IdentifiedLosses]
		,[Interface]
		,[Tolerance]
		,[UnidentifiedLosses]
		,[FinalInventory]
		,[Volume]
		,[MeasurementUnit]
		,(CASE  WHEN [Inputs] = 0.0 THEN 'error'
				ELSE CAST(([Volume] / [Inputs]) AS NVARCHAR(50)) END) AS [Control]
		,@OwnershipStatusId  AS OwnershipStatusId

	FROM Cte

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get all the sum of Balance Summary for all the products with ownership for a node. This SP is an extension to the existing SP [usp_BalanceSummaryWithOwnership]. This SP should sum up all the rows of the output of SP [usp_BalanceSummaryWithOwnership], so it should return only one single row.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_BalanceSummaryAggregate',
    @level2type = NULL,
    @level2name = NULL