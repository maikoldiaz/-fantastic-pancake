/*-- ========================================================================================================================================== 
-- Author:				Microsoft   
-- Created Date:		Nov-22-2019  
-- Updated Date:		Mar-20-2020
-- Modification Date:   Apr-01-2020  -- Added: IdentifiedLoss,UnidentifiedLoss,Interface,Tolerance,FinalInventory. Removed: OwnershipPercentage
-- <Description>:   This Procedure is used to get the Node Configuration details for the Excel file based on the Ticket Id.   </Description>
-- EXEC [Admin].[usp_GetNodeConfigurationDetails] 25252
-- ============================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetNodeConfigurationDetails] 
(
	@TicketId INT
)
AS 
  BEGIN
	  DECLARE @StartDate DATETIME 
      DECLARE @EndDate DATETIME
      SET @StartDate= (SELECT CAST(StartDate  AS DATE)
                       FROM   [Admin].[Ticket] 
                       WHERE  TicketId = @TicketId) 
      SET @EndDate= (SELECT CAST(EndDate  AS DATE)
                     FROM   [Admin].[Ticket] 
                     WHERE  TicketId = @TicketId) 

		IF OBJECT_ID('tempdb..#TempGetNodeConfigurationDetails') IS NOT NULL
        DROP TABLE #TempGetNodeConfigurationDetails

		CREATE TABLE #TempGetNodeConfigurationDetails 
        (
             NodeId                      INT
			,NodeOrder					 INT
			,ProductId					 NVARCHAR (300)
			,ProductOrder				 INT			
			,OwnerId					 INT
			,OwnershipPercentage		 DECIMAL(5,2)	
			,NodeOwnershipRuleId		 INT
			,NodeProductOwnershipRuleId  INT
			,IdentifiedLoss				 DECIMAL(5,2)
			,UnidentifiedLoss			 DECIMAL(5,2)
			,Interface					 DECIMAL(5,2)
			,Tolerance					 DECIMAL(5,2)
			,FinalInventory				 DECIMAL(5,2)
        )

       INSERT INTO #TempGetNodeConfigurationDetails 
        (
            NodeId,
            NodeOrder,				
			ProductId,	
			ProductOrder,
			OwnerId,	
			OwnershipPercentage,
			NodeOwnershipRuleId,
			NodeProductOwnershipRuleId
		)                
      SELECT DISTINCT 
			[NT].[NodeId]                             AS NodeId, 
            [ND].[Order]                              AS NodeOrder, 
            [SLP].[ProductId]                         AS ProductId,
            [SLP].[StorageLocationProductId]          AS ProductOrder, 
            [SLPO].[OwnerId]                          AS OwnerId, 
            [SLPO].[OwnershipPercentage]              AS OwnershipPercentage,
            [ND].NodeOwnershipRuleId                  AS NodeOwnershipRuleId,
            [SLP].NodeProductRuleId                   AS NodeProductOwnershipRuleId
      FROM   [Admin].[Ticket] Tic 
      INNER JOIN [Admin].[NodeTag] NT 
      ON [Tic].[CategoryElementId] = [NT].[ElementId] 
      INNER JOIN [Admin].[Node] ND 
      ON [ND].[NodeId] = [NT].[NodeId]
      LEFT JOIN [Admin].[NodeStorageLocation] NSL 
      ON [NSL].[NodeId] = [NT].[NodeId] 
      LEFT JOIN [Admin].[StorageLocationProduct] SLP 
      ON [SLP].[NodeStorageLocationId] = [NSL].[NodeStorageLocationId]
      LEFT JOIN [Admin].[StorageLocationProductOwner] SLPO 
      ON [SLPO].[StorageLocationProductId] = [SLP].[StorageLocationProductId]
      WHERE 
			[Tic].[TicketId] = @TicketId 
			AND [Tic].[TicketTypeId]=2			-- TicketTypeID 2 represents Ownership
			AND @StartDate >= NT.StartDate 
			AND @EndDate   <= Nt.EndDate


        -- UPDATE "IdentifiedLoss" COLUMN	
        UPDATE MainTab
        SET IdentifiedLoss = ISNULL(MainTab.OwnershipPercentage,0)
        FROM #TempGetNodeConfigurationDetails MainTab
        INNER JOIN [Admin].[StorageLocationProductVariable] SLPV
				ON MainTab.ProductOrder = [SLPV].[StorageLocationProductId]
		INNER JOIN [Admin].[VariableType] VT
				ON [SLPV].[VariableTypeId] = VT.[VariableTypeId]
		WHERE VT.[VariableTypeId] = 7 AND VT.IsConfigurable = 1

        -- UPDATE "UnidentifiedLoss" COLUMN
		UPDATE MainTab
        SET UnidentifiedLoss = ISNULL(MainTab.OwnershipPercentage,0)
        FROM #TempGetNodeConfigurationDetails MainTab
        INNER JOIN [Admin].[StorageLocationProductVariable] SLPV
				ON MainTab.ProductOrder = [SLPV].[StorageLocationProductId]
		INNER JOIN [Admin].[VariableType] VT
				ON [SLPV].[VariableTypeId] = VT.[VariableTypeId]
		WHERE VT.[VariableTypeId] = 3 AND VT.IsConfigurable = 1

        -- UPDATE "Interface" COLUMN
		UPDATE MainTab
        SET Interface = ISNULL(MainTab.OwnershipPercentage,0)
        FROM #TempGetNodeConfigurationDetails MainTab
        INNER JOIN [Admin].[StorageLocationProductVariable] SLPV
				ON MainTab.ProductOrder = [SLPV].[StorageLocationProductId]
		INNER JOIN [Admin].[VariableType] VT
				ON [SLPV].[VariableTypeId] = VT.[VariableTypeId]
		WHERE VT.[VariableTypeId] = 1 AND VT.IsConfigurable = 1

        -- UPDATE "Tolerance" COLUMN		
		UPDATE MainTab
        SET Tolerance = ISNULL(MainTab.OwnershipPercentage,0)
        FROM #TempGetNodeConfigurationDetails MainTab
        INNER JOIN [Admin].[StorageLocationProductVariable] SLPV
				ON MainTab.ProductOrder = [SLPV].[StorageLocationProductId]
		INNER JOIN [Admin].[VariableType] VT
				ON [SLPV].[VariableTypeId] = VT.[VariableTypeId]
		WHERE VT.[VariableTypeId] = 2 AND VT.IsConfigurable = 1

        -- UPDATE "FinalInventory" COLUMN		
		UPDATE MainTab
        SET FinalInventory = ISNULL(MainTab.OwnershipPercentage,0)
        FROM #TempGetNodeConfigurationDetails MainTab
        INNER JOIN [Admin].[StorageLocationProductVariable] SLPV
				ON MainTab.ProductOrder = [SLPV].[StorageLocationProductId]
		INNER JOIN [Admin].[VariableType] VT
				ON [SLPV].[VariableTypeId] = VT.[VariableTypeId]
		WHERE VT.[VariableTypeId] = 8 AND VT.IsConfigurable = 1

     --FINAL SELECT QUERY 
       SELECT DISTINCT
			NodeOwnershipRuleId,
			NodeProductOwnershipRuleId,
			OwnerId,
			NodeId,         				
			ProductId,
			NodeOrder,		
			ProductOrder,	
			ISNULL(IdentifiedLoss,0) AS IdentifiedLoss,
			ISNULL(UnidentifiedLoss,0) AS UnidentifiedLoss,
			ISNULL(Interface,0) AS Interface,
			ISNULL(Tolerance,0) AS Tolerance,
			ISNULL(FinalInventory,0) AS FinalInventory
    	FROM #TempGetNodeConfigurationDetails

   -- DROP ALL TEMP TABLES
	    IF OBJECT_ID('tempdb..#TempGetNodeConfigurationDetails') IS NOT NULL
        DROP TABLE #TempGetNodeConfigurationDetails

  END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get the Node Configuration details for the Excel file based on the Ticket Id.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetNodeConfigurationDetails',
    @level2type = NULL,
    @level2name = NULL