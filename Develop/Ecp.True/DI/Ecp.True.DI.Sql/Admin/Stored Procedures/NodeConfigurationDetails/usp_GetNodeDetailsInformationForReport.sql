/*-- ======================================================================================================================================================================================
-- Author:          Microsoft 
-- Created Date:  Mar-27-2020
-- Description:   This Procedure is used to show node statuses for a ticket for Power Bi report(10.10.08ConfiguracionDetalladaPorNodo08.pbix).
--                  Updates: Added Configuration parameter for number of days 
-- EXEC [Admin].[usp_GetNodeDetailsInformationForReport]   'Segmento','perfelement_1_637246506879499449','FE3BD753-B5C6-45B0-8315-ED0E857DE7C6'
-- ======================================================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetNodeDetailsInformationForReport] 
(
	@Category               NVARCHAR(256), 
	@Element				NVARCHAR(256), 
	@ExecutionId            NVARCHAR(250)
)
AS 
BEGIN 
		DECLARE @CategoryID		INT,
				@ElementId		INT,
				@TodaysDate		DATETIME =  [Admin].[udf_GetTrueDate](),
				@Previousdate	DATETIME =  [Admin].[udf_GetTrueDate]()-1
		
		SELECT @CategoryID = CategoryId
		FROM Admin.Category Ca
		WHERE Name = @Category

		SELECT @ElementId = ElementId
		FROM Admin.CategoryElement CE
		WHERE Name = @Element
		AND CategoryId = @CategoryID

	    --Deleting records from [Admin].[NodeConnectionInfo]
        DELETE FROM [Admin].[NodeConnectionInfo] 
        WHERE Category = @Category
        AND ExecutionId = @ExecutionId
	    
        DELETE FROM [Admin].[NodeConnectionInfo] 
        WHERE LastLoadedDate < (SELECT @Previousdate)

	    --Deleting records from [Admin].[NodeProductInfo]
        DELETE FROM [Admin].NodeProductInfo 
        WHERE Category = @Category
        AND ExecutionId = @ExecutionId
	    
        DELETE FROM [Admin].NodeProductInfo 
        WHERE LastLoadedDate < (SELECT @Previousdate)

	    --Deleting records from [Admin].[NodeGeneralInfo]
        DELETE FROM [Admin].NodeGeneralInfo 
        WHERE Category = @Category
        AND ExecutionId = @ExecutionId
	    
        DELETE FROM [Admin].NodeGeneralInfo 
        WHERE LastLoadedDate < (SELECT @Previousdate)

		INSERT INTO [Admin].[NodeConnectionInfo]
				   (
					[ConnectionType]
				   ,[NodeName]
				   ,[NodeConnectionName]
				   ,[ProductName]
				   ,[ProductId]
				   ,[Priority]
				   ,[TransferPoint]
				   ,[AlgorithmName]
				   ,[OwnershipStrategy]
				   ,[UncertaintyConnectionProduct]
				   ,[OwnershipPercentage]
				   ,[OwnerName]
				   ,[Category]
				   ,[Element]
				   ,[NodeId]
				   ,[RNo]
				   ,[ExecutionId]
				   ,[LastLoadedDate]
				   )	
		SELECT	
				[ConnectionType],
				[NodeName],
				[NodeConnectionName],
				[ProductName],
				[ProductId],
				[Priority],
				CASE WHEN [IsTransfer] = 1 
					 THEN 'Si' 
					 ELSE 'No' 
					 END  AS [TransferPoint],
				[AlgorithmName],
				[OwnershipStrategy],
				[UncertaintyConnectionProduct],
				[OwnershipPercentage],
				[OwnerName],
				[Category],
				[Element],
				[NodeId],
				DENSE_RANK() OVER (ORDER BY ConnectionType,NodeConnectionName,ProductName)  AS [RNo],				
				@ExecutionId AS ExecutionId,
				@TodaysDate  AS [LastLoadedDate]
		FROM (
					SELECT 
					'Entrada'														AS [ConnectionType],
					[DestND].[Name]													AS [NodeName],
					[SrcND].[Name]													AS [NodeConnectionName],  -- Should be Name of Source Node
					[Prd].[Name]													AS [ProductName],
					[Prd].[ProductId]												AS [ProductId],
					[NCP].[Priority]												AS [Priority],
					NC.IsTransfer													AS [IsTransfer],
					[Alg].[ModelName]												AS [AlgorithmName],
					[NCPRule].[RuleName]											AS [OwnershipStrategy],
					[NCP].[UncertaintyPercentage]/100								AS [UncertaintyConnectionProduct],
					[NCPO].[OwnershipPercentage]/100								AS [OwnershipPercentage],
					[CEOwnerName].[Name]											AS [OwnerName],
					[CA].[Name]														AS [Category],
					[CE].[Name]														AS [Element],
					[DestND].[NodeId]												AS [NodeId]
					FROM [Admin].[NodeConnection] NC
					INNER JOIN [Admin].[Node] DestND
					ON [DestND].[NodeId] = [NC].[DestinationNodeId]
					AND [DestND].[IsActive] = 1
					INNER JOIN [Admin].[NodeTag] NT
					ON [NT].[NodeId] = [DestND].[NodeId]
					INNER JOIN [Admin].[CategoryElement] CE
					ON [CE].[ElementId] = [NT].[ElementId]
					AND CE.ElementId IN (@ElementId)
					AND CE.CategoryId IN (@CategoryID)
					INNER JOIN [Admin].[Category] CA
					ON [CA].[CategoryId] = [CE].[CategoryId]
					AND CA.CategoryId IN (@CategoryID)
					INNER JOIN [Admin].[Node] SrcND
					ON [SrcND].[NodeId] = [NC].[SourceNodeId]
					INNER JOIN [Admin].[NodeConnectionProduct] NCP
					ON [NCP].[NodeConnectionId] = [NC].[NodeConnectionId]
					INNER JOIN [Admin].[Product] Prd
					ON [Prd].[ProductId] = [NCP].[ProductId]
					LEFT JOIN [Admin].[NodeConnectionProductOwner] NCPO
					ON [NCPO].[NodeConnectionProductId] = [NCP].[NodeConnectionProductId]
					LEFT JOIN [Admin].[CategoryElement] CEOwnerName
					ON [CEOwnerName].[ElementId] = [NCPO].[OwnerId]
					LEFT JOIN [Admin].[Algorithm] Alg
					ON [Alg].[AlgorithmId] = [NC].[AlgorithmId]
					LEFT JOIN [Admin].[NodeConnectionProductRule] NCPRule
					ON [NCPRule].[RuleId] = [NCP].[NodeConnectionProductRuleId]
					WHERE NC.IsDeleted = 0
					UNION
					SELECT 
					'Salida'														AS [ConnectionType],
					[SrcND].[Name]													AS [NodeName],
					[DestND].[Name]													AS [NodeConnectionName], -- Should be name of Destination Node
					[Prd].[Name]													AS [ProductName],
					[Prd].[ProductId]												AS [ProductId],
					[NCP].[Priority]												AS [[Priority],
					NC.IsTransfer													AS [IsTransfer],
					[Alg].[ModelName]												AS [AlgorithmName],
					[NCPRule].[RuleName]											AS [OwnershipStrategy],
					[NCP].[UncertaintyPercentage]/100								AS [UncertaintyConnectionProduct],
					[NCPO].[OwnershipPercentage]/100								AS [OwnershipPercentage],
					[CEOwnerName].[Name]											AS [OwnerName],
					[CA].[Name]														AS [Category],
					[CE].[Name]														AS [Element],
					[SrcND].[NodeId]												AS [NodeId]
					FROM [Admin].[NodeConnection] NC
					INNER JOIN [Admin].[Node] SrcND
					ON [SrcND].[NodeId] = [NC].[SourceNodeId]
					AND SrcND.[IsActive] = 1
					INNER JOIN [Admin].[NodeTag] NT
					ON [NT].[NodeId] = [SrcND].[NodeId]
					INNER JOIN [Admin].[CategoryElement] CE
					ON [CE].[ElementId] = [NT].[ElementId]
					AND CE.ElementId IN (@ElementId)
					AND CE.CategoryId IN (@CategoryID)
					INNER JOIN [Admin].[Category] CA
					ON [CA].[CategoryId] = [CE].[CategoryId]
					AND CA.CategoryId IN (@CategoryID)
					INNER JOIN [Admin].[Node] DestND
					ON [DestND].[NodeId] = [NC].[DestinationNodeId]
					INNER JOIN [Admin].[NodeConnectionProduct] NCP
					ON [NCP].[NodeConnectionId] = [NC].[NodeConnectionId]
					INNER JOIN [Admin].[Product] Prd
					ON [Prd].[ProductId] = [NCP].[ProductId]
					LEFT JOIN [Admin].[NodeConnectionProductOwner] NCPO
					ON [NCPO].[NodeConnectionProductId] = [NCP].[NodeConnectionProductId]
					LEFT JOIN [Admin].[CategoryElement] CEOwnerName
					ON [CEOwnerName].[ElementId] = [NCPO].[OwnerId]
					LEFT JOIN [Admin].[Algorithm] Alg
					ON [Alg].[AlgorithmId] = [NC].[AlgorithmId]
					LEFT JOIN [Admin].[NodeConnectionProductRule] NCPRule
					ON [NCPRule].[RuleId] = [NCP].[NodeConnectionProductRuleId]
					WHERE NC.IsDeleted = 0
		) NodeConnDetails

		INSERT INTO [Admin].[NodeProductInfo]
				   (
						[ProductName]
					   ,[ProductOrder]
					   ,[OwnershipStrategy]
					   ,[OwnerName]
					   ,[OwnershipPercentage]
					   ,[UncertaintyNodeProduct]
					   ,[Category]
					   ,[Element]
					   ,[NodeName]
					   ,[NodeId]
					   ,[PI]
					   ,[Interfase]
					   ,[PNI]
					   ,[Tolerancia]
					   ,[Inventario]
					   ,[ExecutionId]
					   ,[LastLoadedDate]
				   )
		SELECT 
				[ProductName],
				[ProductOrder],
				[OwnershipStrategy],
				[OwnerName],
				[OwnershipPercentage],
				[UncertaintyNodeProduct],
				[Category],
				[Element],
				[NodeName],
				[NodeId],
				CASE WHEN (SUM([PI])>1) THEN 1 ELSE SUM(PI) END						AS [PI],
				CASE WHEN (SUM([Interfase])>1) THEN 1 ELSE SUM([Interfase])	END		AS [Interfase],
				CASE WHEN (SUM([PNI])>1) THEN 1 ELSE SUM([PNI]) END					AS [PNI],
				CASE WHEN (SUM([Tolerancia])>1) THEN 1 ELSE SUM([Tolerancia]) END	AS [Tolerancia],
				CASE WHEN (SUM([Inventario])>1) THEN 1 ELSE SUM([Inventario]) END	AS [Inventario],
				@ExecutionId AS [ExecutionId],
				@TodaysDate  AS [LastLoadedDate]
		FROM
		(
				SELECT DISTINCT
				[Prd].[Name]																							AS [ProductName],
				[SLP].[ProductId]																						AS [ProductOrder],
				[NPR].[RuleName]																						AS [OwnershipStrategy],
				CASE WHEN [VT].[Name] = 'Pérdida Identificada' AND [VT].[IsConfigurable] = 1 THEN 1  ELSE 0 END			AS [PI],
				CASE WHEN [VT].[Name] = 'Interfase' AND [VT].[IsConfigurable] = 1 THEN 1 ELSE 0 END                     AS [Interfase],
				CASE WHEN [VT].[Name] = 'Pérdida No Identificada' AND [VT].[IsConfigurable] = 1 THEN 1 ELSE 0 END		AS [PNI],
				CASE WHEN [VT].[Name] = 'Tolerancia' AND [VT].[IsConfigurable] = 1 THEN 1 ELSE 0 END                    AS [Tolerancia],
				CASE WHEN [VT].[Name] = 'Inventario Final' AND [VT].[IsConfigurable] = 1 THEN 1 ELSE 0 END              AS [Inventario],
				[CEOwner].[Name]                                                                                        AS [OwnerName],
				[SLPO].[OwnershipPercentage]/100                                                                        AS [OwnershipPercentage],
				[SLP].[UncertaintyPercentage]/100                                                                       AS [UncertaintyNodeProduct],
				[CA].[Name]                                                                                             AS [Category],
				[CE].[Name]                                                                                             AS [Element],
				[ND].[Name]                                                                                             AS [NodeName],
				[ND].[NodeId]                                                                                           AS [NodeId]
				FROM [Admin].[Node] ND
				INNER JOIN [Admin].[NodeStorageLocation] NSL
				ON [NSL].[NodeId] = [ND].[NodeId]
				AND [ND].[IsActive] = 1
				INNER JOIN [Admin].[NodeTag] NT
				ON [NT].[NodeId] = [ND].[NodeId]
				INNER JOIN [Admin].[CategoryElement] CE
				ON [CE].[ElementId] = [NT].[ElementId]
				AND CE.ElementId IN (@ElementId)
				AND CE.CategoryId IN (@CategoryID)
				INNER JOIN [Admin].[Category] CA
				ON [CA].[CategoryId] = [CE].[CategoryId]
				AND [CE].[CategoryId] IN (@CategoryID)
				INNER JOIN [Admin].[StorageLocationProduct] SLP
				ON [SLP].[NodeStorageLocationId] = [NSL].[NodeStorageLocationId]
				INNER JOIN [Admin].[Product] Prd
				ON [Prd].[ProductId] = [SLP].[ProductId]
				LEFT JOIN [Admin].[NodeProductRule] NPR
				ON [NPR].[RuleId] = [SLP].[NodeProductRuleId]
				LEFT JOIN [Admin].[StorageLocationProductVariable] SLPV
				ON [SLPV].[StorageLocationProductId] = [SLP].[StorageLocationProductId]
				LEFT JOIN [Admin].[VariableType] VT
				ON [VT].[VariableTypeId] = [SLPV].[VariableTypeId]
				LEFT JOIN [Admin].[StorageLocationProductOwner] SLPO
				ON [SLPO].[StorageLocationProductId] = [SLP].[StorageLocationProductId]
				LEFT JOIN [Admin].[CategoryElement] CEOwner
				ON [CEOwner].[ElementId] = [SLPO].[OwnerId] 
		)A
		GROUP BY [ProductName],
				 [ProductOrder],
				 [OwnershipStrategy],
				 [OwnerName],
				 [OwnershipPercentage],
				 [UncertaintyNodeProduct],
				 [Category],
				 [Element],
				 [NodeName],
				 [NodeId]

		INSERT INTO [Admin].[NodeGeneralInfo]
           (
		    [NodeName]
           ,[NodeOrder]
           ,[NodeId]
           ,[NodeOwnershipStrategy]
           ,[NodeControlLimit]
           ,[NodeAcceptableBalancePercentage]
           ,[Category]
           ,[Element]
           ,[ExecutionId]
           ,[LastLoadedDate]
		   )
		SELECT	[NodeName]														AS [NodeName],
				ISNULL([NodeOrder],'-')											AS [NodeOrder],
				[NodeId]														AS [NodeId],
				ISNULL([NodeOwnershipStrategy],'-')                             AS [NodeOwnershipStrategy],
				REPLACE(ISNULL([NodeControlLimit],'-'),'.',',')                 AS [NodeControlLimit],
				REPLACE(ISNULL([NodeAcceptableBalancePercentage],'-'),'.',',')  AS [NodeAcceptableBalancePercentage],
				[Category]														AS [Category],
				[Element]														AS [Element],
				@ExecutionId													AS [ExecutionId],
				@TodaysDate														AS [LastLoadedDate]
		FROM
		(
				SELECT  DISTINCT
						[ND].[Name]																		AS [NodeName], 
						CAST([ND].[Order] AS VARCHAR)													AS [NodeOrder],
						[ND].[NodeId]																	AS [NodeId],
						[NOR].[RuleName]																AS [NodeOwnershipStrategy],
						'+-' + CAST([ND].[ControlLimit] AS VARCHAR)										AS [NodeControlLimit],
						CAST([ND].[AcceptableBalancePercentage] AS VARCHAR)+'%'							AS [NodeAcceptableBalancePercentage],
						[CE].[Name]																		AS [Element],
						[CA].[Name]																		AS [Category]
				FROM [Admin].[Node] ND
				INNER JOIN [Admin].[NodeTag] NT
				ON [NT].[NodeId] = [ND].[NodeId]
				AND [NT].[StartDate] <= @TodaysDate
				AND [NT].[EndDate] >= @TodaysDate
				AND [ND].[IsActive] = 1
				INNER JOIN [Admin].[CategoryElement] CE
				ON [CE].[ElementId] = [NT].[ElementId]
				AND CE.ElementId IN (@ElementId)
				AND CE.CategoryId IN (@CategoryID)
				INNER JOIN [Admin].[Category] CA
				ON [CA].[CategoryId] = [CE].[CategoryId]
				AND CA.CategoryId IN (@CategoryID)
				LEFT JOIN [Admin].[NodeOwnershipRule] NOR
				ON [NOR].[RuleId] = [ND].[NodeOwnershipRuleId]
		)NodeInfo

END 
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
							@value = N'This Procedure is used to show node statuses for a ticket for Power Bi report(10.10.08ConfiguracionDetalladaPorNodo08.pbix)',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetNodeDetailsInformationForReport',
							@level2type = NULL,
							@level2name = NULL