/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Dec-16-2019
-- Updated Date:	Mar-20-2020
					Aug-04-2020 Modified For Performance reasons
					Aug-04-2020 Corrected the Spelling
					Aug-31-2020 Validating the first time nodes
-- <Description>:	This Procedure is used to get the Inventory Data based on the input of Node List, Start Date, End Date, TicketId. </Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetInventories]
(
       @NodeList				[Admin].[NodeListType] READONLY,
	   @StartDate				DATE,
	   @EndDate					DATE,
	   @TicketId				INT,
	   @FirstTimeNodes          [Admin].[NodeListType] READONLY
)
AS
BEGIN
IF @TicketId = 0
BEGIN
		SELECT  Inv.[InventoryId]
			   ,Inv.[NodeId]
			   ,Inv.[ProductId]
			   ,Prd.Name AS ProductName
			   ,Inv.[InventoryDate]
			   ,Inv.[ProductVolume]
			   ,Inv.[UncertaintyPercentage]
			   ,CEUnits.[Name] AS MeasurementUnit
		FROM offchain.InventoryProduct Inv
		INNER JOIN Admin.Product Prd
		ON Prd.ProductID = Inv.[ProductId]
		INNER JOIN [Admin].[CategoryElement] CEUnits
		ON CEUnits.ElementId = Inv.MeasurementUnit
		LEFT JOIN @FirstTimeNodes FTN
        ON Inv.NodeId = FTN.NodeId
		WHERE CEUnits.CategoryId = 6 
		AND Inv.ScenarioId = 1
		AND Inv.[NodeId] IN (SELECT NodeId FROM @NodeList)
		AND Inv.[InventoryDate] BETWEEN @StartDate AND @EndDate
		AND ([Inv].[InventoryDate] <> @StartDate
        OR ([Inv].[InventoryDate] = @StartDate AND FTN.NodeId IS NOT NULL)
        OR ([Inv].[InventoryDate] = @StartDate AND FTN.NodeId IS NULL 
		AND Inv.TicketId IS NOT NULL))

END
ELSE
BEGIN

SELECT  Inv.[InventoryId]
			   ,Inv.[NodeId]
			   ,Inv.[ProductId]
			   ,Prd.Name AS ProductName
			   ,Inv.[InventoryDate]
			   ,Inv.[ProductVolume]
			   ,Inv.[UncertaintyPercentage]
			   ,CEUnits.[Name] AS MeasurementUnit
		FROM offchain.InventoryProduct Inv
		INNER JOIN Admin.Product Prd
		ON Prd.ProductID = Inv.[ProductId]
		INNER JOIN [Admin].[CategoryElement] CEUnits
		ON CEUnits.ElementId = Inv.MeasurementUnit
		WHERE CEUnits.CategoryId = 6 
		AND Inv.ScenarioId = 1
		AND Inv.[NodeId] IN (SELECT NodeId FROM @NodeList)
		AND Inv.[InventoryDate] BETWEEN @StartDate AND @EndDate
		AND Inv.TicketId = @TicketId 
END
		 

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get the Inventory Data based on the input of Node List, Start Date, End Date, TicketId.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetInventories',
    @level2type = NULL,
    @level2name = NULL