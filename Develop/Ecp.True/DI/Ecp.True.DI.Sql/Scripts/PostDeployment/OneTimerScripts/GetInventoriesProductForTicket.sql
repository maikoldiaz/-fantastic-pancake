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

IF EXISTS (Select 'X' from sys.schemas Where name = 'Offchain')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='7fd20cb2-1bdc-4e15-8992-e16f6b2b6a64' AND Status = 1)
	BEGIN
		BEGIN TRY
			SELECT 
				[Tic].[TicketId]							AS Ticket,
				[Tic].[TicketTypeId],
				[Tic].[StartDate],
				Inv.[InventoryProductId]					AS InventoryProductId,
				Inv.[InventoryDate]					    AS OperationalDate,
				Inv.[NodeId]								AS NodeId,
				Inv.[ProductId]							AS ProductId,
				Inv.[TankName]							AS Tank,
				Inv.[ProductVolume]						AS NetVolume,
				Inv.[EventType],
				Inv.TicketId,
				[Tic].[EndDate],
				o.OwnershipValue,
				o.OwnerId,
				ROW_NUMBER()OVER(PARTITION BY Inv.[ProductId]
											,Inv.InventoryID
											,Inv.NodeId
											,Inv.[InventoryDate]
											,ISNULL(Inv.[TankName],'')
											,ISNULL(Inv.BatchId,'')
									ORDER BY Inv.[InventoryProductId] DESC  
									)AS Rnum
			FROM   [Admin].[Ticket] Tic
			INNER JOIN [Admin].[NodeTag] NT
			ON [Tic].[CategoryElementId] = [NT].[ElementId]		
			INNER JOIN Offchain.InventoryProduct Inv
			ON Inv.NodeId = NT.NodeId
			LEFT JOIN [Offchain].[Owner] o
			ON Inv.[InventoryProductId] = [o].[InventoryProductId]
			LEFT JOIN Admin.CategoryElement CatEle
			ON [o].[OwnerId] = CatEle.ElementId	
			WHERE [Tic].[TicketId]=25929 
			AND Inv.[InventoryDate]  Between CAST([Tic].[StartDate] AS DATE) AND CAST([Tic].[EndDate] AS DATE)
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('7fd20cb2-1bdc-4e15-8992-e16f6b2b6a64', 1, 'POST');
		END TRY
		
		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('7fd20cb2-1bdc-4e15-8992-e16f6b2b6a64', 0, 'POST');
		END CATCH
	END
END
