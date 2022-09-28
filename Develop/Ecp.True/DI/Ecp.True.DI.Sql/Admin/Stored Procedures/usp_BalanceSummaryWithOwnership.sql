
/*-- =======================================================================================================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jan-05-2020
-- Updated Date1:	Jan-07-2020; Added the actual implementation of SP
-- Updated Date2:	Jan-08-2020; Added ProductId column.
-- Updated Date3:	Jan-13-2020; Implemented Group By Aggregations per product per Owner per date.
-- Updated Date4:	Jan-14-2020; Changed logic to calculate the Volume with formula -> initialInventory + inputMovement - outputMovement - finalInventory - identifiedLoss + interfaceValue + tolerance + unidentified;
-- <Description>:	This Procedure is used to get the Balance Summary with Ownership for a given Ownership Node. </Description>
-- ========================================================================================================================================================================================================================*/


CREATE PROCEDURE [Admin].[usp_BalanceSummaryWithOwnership]
(
	   @OwnershipNodeId		INT
)
AS
BEGIN

	WITH Cte AS(
		SELECT DISTINCT
          [Pr].[ProductId]							AS [ProductId]
         ,[Pr].[Name]								AS [Product]
         ,[CE].[Name]								AS [Owner]
         ,[OWNCal].[InitialInventoryVolume]		    AS [InitialInventory]
         ,[OWNCal].[InputVolume]					AS [Inputs]
         ,[OWNCal].[OutputVolume]					AS [Outputs]
         ,[OWNCal].[IdentifiedLossesVolume]			AS [IdentifiedLosses]
         ,[OWNCal].[InterfaceVolume]				AS [Interface]
         ,[OWNCal].[ToleranceVolume]				AS [Tolerance]
         ,[OWNCal].[UnidentifiedLossesVolume]		AS [UnidentifiedLosses]
         ,[OWNCal].[FinalInventoryVolume]			AS [FinalInventory]
         ,[OWNCal].[UnbalanceVolume]				AS [Volume]
         ,'Bbl'										AS [MeasurementUnit]
         ,[OWNCal].[UnbalancePercentage]			AS [Control]
		 ,[OWNCal].[Date]					--> For Debugging Purpose
		 ,[OWNCal].OwnershipCalculationId	--> For Debugging Purpose
		 ,RnAsc = ROW_NUMBER() OVER(PARTITION BY [Pr].[ProductId], [Pr].[Name], [CE].[Name]  ORDER BY [OWNCal].[Date])
		 ,RnDsc = ROW_NUMBER() OVER(PARTITION BY [Pr].[ProductId], [Pr].[Name], [CE].[Name]  ORDER BY [OWNCal].[Date] DESC)

		FROM [Admin].[OwnershipCalculation] OWNCal
		INNER JOIN [Admin].[OwnershipNode] OWNNode
			ON OWNCal.NodeId = OWNNode.NodeId
			AND
			OWNCal.OwnershipTicketId = OWNNode.TicketId
		LEFT JOIN [Admin].[Product] Pr
			ON OWNCal.ProductId = Pr.ProductId
		LEFT JOIN [Admin].[CategoryElement] CE
			ON OWNCal.OwnerId = CE.ElementId

		WHERE OWNNode.OwnershipNodeId = @OwnershipNodeId
	),

Cte2 AS

(
SELECT 
		   [ProductId]
         , [Product]
         , [Owner]
         , (SELECT [InitialInventory] FROM CTE WHERE RnAsc = 1 AND CTE.Owner = A.Owner AND CTE.Product = A.Product GROUP BY Product, Owner,[InitialInventory]) AS [InitialInventory]
         , SUM([Inputs]) As Inputs
         , SUM([Outputs]) As [Outputs]
         , SUM([IdentifiedLosses]) as[IdentifiedLosses]
         , SUM([Interface]) As [Interface]
         , SUM([Tolerance]) As [Tolerance]
         , SUM([UnidentifiedLosses]) As [UnidentifiedLosses]
		 , (SELECT [FinalInventory] FROM CTE WHERE RnDsc = 1 AND CTE.Owner = A.Owner AND CTE.Product = A.Product GROUP BY Product, Owner,[FinalInventory]) AS [FinalInventory]
         , SUM([Volume]) As [Volume]
         , [MeasurementUnit] As [MeasurementUnit]
         , SUM([Control]) As [Control]
	FROM (
			SELECT 
			  [ProductId]
			 ,[Product]
			 ,[Owner]
			 ,[InitialInventory]
			 ,Inputs
			 ,[Outputs]
			 ,[IdentifiedLosses]
			 ,[Interface]
			 ,[Tolerance]
			 ,[UnidentifiedLosses]
			 ,[FinalInventory]
			 ,[Volume]
			 ,[MeasurementUnit]
			 ,[Control]

			 FROM Cte
		) A

	GROUP BY A.ProductId, A.Product, A.Owner,  A.[MeasurementUnit]
)

SELECT DISTINCT
			  [ProductId]
			 ,[Product]
			 ,[Owner]
			 ,[InitialInventory]
			 ,[Inputs]
			 ,[Outputs]
			 ,[IdentifiedLosses]
			 ,[Interface]
			 ,[Tolerance]
			 ,[UnidentifiedLosses]
			 ,[FinalInventory]
			 ,([InitialInventory] + [Inputs] - [Outputs] - [FinalInventory] - [IdentifiedLosses] + [Interface] + [Tolerance] + [UnidentifiedLosses]) AS [Volume]
			 ,[MeasurementUnit]
			 ,0.0 AS [Control]
		FROM Cte2

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get the Balance Summary with Ownership for a given Ownership Node.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_BalanceSummaryWithOwnership',
    @level2type = NULL,
    @level2name = NULL