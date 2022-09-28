SELECT TOP 1 [CE].Name from [appdb].[Admin].[CategoryElement] [CE]
INNER JOIN [appdb].[Admin].[NodeTag] [NT]
ON [NT].ElementId = [CE].ElementId
WHERE [CE].CategoryId=2
AND [NT].NodeId NOT IN (SELECT [MS].SourceNodeId FROM [appdb].[Admin].[MovementSource] [MS])
AND [NT].NodeId NOT IN (SELECT [MD].DestinationNodeId FROM [appdb].[Admin].[MovementDestination] [MD])
AND [NT].NodeId NOT IN (SELECT [Inv].NodeID FROM [appdb].[Admin].[Inventory] [Inv]);    