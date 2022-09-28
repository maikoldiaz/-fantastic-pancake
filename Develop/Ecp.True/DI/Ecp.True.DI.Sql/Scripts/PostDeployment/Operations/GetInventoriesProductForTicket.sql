/*
 Pre-Deployment Script Template							
 --------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
 SELECT * FROM [$(TableName)]					
 --------------------------------------------------------------------------------------
 */

SELECT
	[Tic].[TicketId] AS Ticket,
	[Tic].[TicketTypeId] AS TicTicketTypeId,
	[Tic].[StartDate] AS TicStartDate,
	Inv.[InventoryProductId] AS InvInventoryProductId,
	Inv.[InventoryDate] AS InvOperationalDate,
	Inv.[NodeId] AS InvNodeId,
	Inv.[ProductId] AS InvProductId,
	Inv.[TankName] AS InvTank,
	Inv.[ProductVolume] AS InvNetVolume,
	Inv.[EventType] AS InvEventType,
	Inv.TicketId AS InvTicketId,
	[Tic].[EndDate] AS TicEndDate,
	o.*,
	ROW_NUMBER() OVER(
		PARTITION BY Inv.[ProductId],
		Inv.InventoryID,
		Inv.NodeId,
		Inv.[InventoryDate],
		ISNULL(Inv.[TankName], ''),
		ISNULL(Inv.BatchId, '')
		ORDER BY
			Inv.[InventoryProductId] DESC
	) AS Rnum
FROM
	[Admin].[Ticket] Tic
	INNER JOIN [Admin].[NodeTag] NT ON [Tic].[CategoryElementId] = [NT].[ElementId]
	INNER JOIN Offchain.InventoryProduct Inv ON Inv.NodeId = NT.NodeId
	LEFT JOIN [Offchain].[Owner] o ON Inv.[InventoryProductId] = [o].[InventoryProductId]
	LEFT JOIN Admin.CategoryElement CatEle ON [o].[OwnerId] = CatEle.ElementId
WHERE
	[Tic].[TicketId] = 25929
	AND Inv.[InventoryDate] Between CAST([Tic].[StartDate] AS DATE)
	AND CAST([Tic].[EndDate] AS DATE)