/*-- =================================================================================================
-- Author:		Microsoft
-- Created date: Jul-30-2020
-- Updated date: May-20-2021  add NodeId Validation
-- -- <Description>:	This procedure is to Fetch InventoryDetailsWithOwner Data For PowerBi Report From
				From Tables(Inventory, InventoryProduct, Unbalance, Ownership, Ticket, Product, Node, 
--				CategoryElement,Category)</Description></Description>
   EXEC [Admin].[usp_SaveInventoryDetailsWithOwner] @OwnershipTicketId = -1(full load)
   EXEC [Admin].[usp_SaveInventoryDetailsWithOwner] @OwnershipTicketId = 31352(ticketid load)
   SELECT * FROM Admin.InventoryDetailsWithOwner
-- ===================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveInventoryDetailsWithOwner]
(
	      @OwnershipTicketId INT,
		  @NodeId INT = NULL
)
AS
BEGIN
  SET NOCOUNT ON

IF OBJECT_ID('tempdb..#segment')IS NOT NULL
			DROP TABLE #segment
IF OBJECT_ID('tempdb..#Source')IS NOT NULL
			DROP TABLE #Source

DECLARE @Todaysdate	 DATETIME =  [Admin].[udf_GetTrueDate] ()

--temp table for segment data
SELECT 			--DISTINCT
                     Cat.[Name]							AS Category
                    ,Element.[Name]						AS Element
                    ,Inv.NodeName						AS NodeName
					,Inv.InventoryDate					AS CalculationDate
                    ,Inv.ProductName					AS Product
                    ,Inv.ProductId						AS ProductId
                    ,ElementOwner.[Name]				AS OwnerName 
					,Inv.InventoryId
					,Inv.InventoryProductId
					,Inv.InventoryDate
					,Inv.TankName
					,Inv.BatchId
					,CAST(Inv.ProductVolume  AS DECIMAL(29,2)) AS [Net Volume]					
					,CEUnits.Name AS [Volume Unit] 				--MeasurmentUnit,
					,Inv.EventType                                                              
					,Inv.SourceSystem AS SystemName
					,CAST(Inv.UncertaintyPercentage AS DECIMAL(29,2)) AS UncertainityPercentage
					,CAST(ISNULL(Inv.ProductVolume,1)*ISNULL(Inv.UncertaintyPercentage,1)AS DECIMAL(29,2)) AS Incertidumbre
					,CAST(Owner.[OwnershipVolume] AS DECIMAL(29,2)) AS [OwnershipVolume]
					,CAST(Owner.[OwnershipPercentage] AS DECIMAL(29,2)) AS [OwnershipPercentage]
					,Owner.[AppliedRule]	
					,Inv.GrossStandardQuantity
					,Element.ElementId
					,Element.CategoryId
					,[Owner].[TicketId] AS [OwnershipTicketId]

					into #Segment

	FROM [Admin].[view_InventoryInformation] Inv
    INNER JOIN [Admin].Ticket Tick
			ON Tick.TicketId = Inv.OwnershipTicketId
    INNER JOIN [offchain].[Ownership] [Owner]
			ON [Owner].[InventoryProductId] = Inv.[InventoryProductId] 
			AND [Owner].[TicketId] = Tick.TicketId
    INNER JOIN [Admin].CategoryElement ElementOwner  
			ON [Owner].OwnerId=  ElementOwner.ElementId
    INNER JOIN [Admin].[CategoryElement] CEUnits
		  ON CEUnits.ElementId = Inv.MeasurementUnit
    INNER JOIN [Admin].CategoryElement Element 
			ON Element.[ElementId] = Tick.CategoryElementId
    INNER JOIN [Admin].Category  Cat
			ON Element.CategoryId = Cat.CategoryId
     WHERE Tick.[Status] = 0        --> 0 = successfully processed
			AND Tick.ErrorMessage IS NULL
			AND Tick.[TicketTypeId] = 2 --> Here ticket type id 2 is "OWNERSHIP"
			AND ElementOwner.CategoryId = 7 --> Here categoryId 7 is "Propietario" i.e. Ownership;
	 AND [Owner].IsDeleted = 0
	 AND ([Owner].[TicketId]= @OwnershipTicketId OR @OwnershipTicketId =-1)
	 AND (@NodeId IS  NULL OR  (Inv.NodeId = @NodeId))
	 


	SELECT --DISTINCT
			 [Category]
			,[Element]
			,[NodeName]
			,[CalculationDate]
			,[Product]
			,[ProductId]
			,[OwnerName]
			,[InventoryId]
			,[InventoryProductId]
			,[InventoryDate]
			,[TankName]
			,[BatchId]
			,[Net Volume]
			,[Volume Unit]
			,[EventType]
			,[SystemName]
			,[UncertainityPercentage]
			,[Incertidumbre]
			,[OwnershipVolume]
			,[OwnershipPercentage]
			,[AppliedRule]
			,[GrossStandardQuantity]
			,[OwnershipTicketId]
			,'ReportUser'   as [CreatedBy]
			,@TodaysDate    as [CreatedDate]
			,NULL			as [LastModifiedBy]
			,NULL           as [LastModifiedDate]
					INTO #Source
			 FROM (
		     SELECT 
						 Category
						,Element
						,NodeName
						,CalculationDate
					    ,Product
						,ProductId
						,OwnerName
						,InventoryId
						,InventoryProductId
						,InventoryDate
						,TankName
						,BatchId
						,[Net Volume]
						,[Volume Unit]
						,EventType
						,SystemName
						,UncertainityPercentage
						,Incertidumbre
						,OwnershipVolume
						,OwnershipPercentage
						,AppliedRule
						,GrossStandardQuantity
						,[OwnershipTicketId]
						FROM 
						#Segment

				UNION

				SELECT DISTINCT
                     Cat1.[Name]					AS Category
                    ,CatEle.[Name]					AS Element
                    ,SG.NodeName					AS NodeName
					,SG.InventoryDate				AS CalculationDate
                    ,SG.Product
                    ,SG.ProductId					AS ProductId
                    ,SG.OwnerName 
					,SG.InventoryId
					,SG.InventoryProductId
					,SG.InventoryDate
					,SG.TankName
					,SG.BatchId
					,SG.[Net Volume]				
					,SG.[Volume Unit] 				
					,SG.EventType                                                              
					,SG.SystemName
					,SG.UncertainityPercentage
					,SG.Incertidumbre
					,SG.[OwnershipVolume]
					,SG.[OwnershipPercentage]
					,SG.[AppliedRule]	
					,SG.GrossStandardQuantity
					,SG.[OwnershipTicketId]
					FROM #Segment SG
					INNER JOIN Admin.NodeTag NT
							ON NT.ElementId = SG.ElementId
					INNER JOIN Admin.Category CA
							ON CA.CategoryId = SG.CategoryId
					INNER JOIN Admin.NodeTag NT1
							ON NT.NodeId = NT1.NodeId
					INNER JOIN Admin.CategoryElement CatEle
							ON CatEle.ElementId = NT1.ElementId AND CatEle.CategoryId = 8
					INNER JOIN Admin.Category  Cat1
							ON CatEle.CategoryId = Cat1.CategoryId   
					)SubQ
				
				MERGE [Admin].[InventoryDetailsWithOwner] AS TARGET 
				USING #Source AS SOURCE  
				ON  ISNULL(TARGET.[Category]			 ,'')=  ISNULL(  SOURCE.[Category]			 ,'')
				AND ISNULL(TARGET.[Element]			     ,'')=  ISNULL(  SOURCE.[Element]			 ,'')
				AND ISNULL(TARGET.[NodeName]			 ,'')=  ISNULL(  SOURCE.[NodeName]			 ,'')
				AND ISNULL(TARGET.[CalculationDate]		 ,'')=  ISNULL(  SOURCE.[CalculationDate]	 ,'')
				AND ISNULL(TARGET.[Product]				 ,'')=  ISNULL(  SOURCE.[Product]			 ,'')
				AND ISNULL(TARGET.[ProductId]			 ,'')=  ISNULL(  SOURCE.[ProductId]			 ,'')
				AND ISNULL(TARGET.[OwnerName]			 ,'')=  ISNULL(  SOURCE.[OwnerName]			 ,'')
				AND ISNULL(TARGET.[InventoryId]			 ,'')=  ISNULL(  SOURCE.[InventoryId]		 ,'')
				AND ISNULL(TARGET.[InventoryProductId]	 ,'')=  ISNULL(  SOURCE.[InventoryProductId] ,'')
				AND ISNULL(TARGET.[InventoryDate]		 ,'')=  ISNULL(  SOURCE.[InventoryDate]		 ,'')
				AND ISNULL(TARGET.[TankName]			 ,'')=  ISNULL(  SOURCE.[TankName]			 ,'')
				AND ISNULL(TARGET.[BatchId]			     ,'')=  ISNULL(  SOURCE.[BatchId]			 ,'')
				AND ISNULL(TARGET.[OwnershipTicketId]	 ,'')=  ISNULL(  SOURCE.[OwnershipTicketId]	 ,'')

				/*Update records in Target Table using source*/
				WHEN MATCHED  AND ( 
					     TARGET.[Net Volume]			  <> SOURCE.[Net Volume]			
					  OR TARGET.[Volume Unit]			  <> SOURCE.[Volume Unit]			
					  OR TARGET.[EventType]				  <> SOURCE.[EventType]				
					  OR TARGET.[SystemName]			  <> SOURCE.[SystemName]			
					  OR TARGET.[UncertainityPercentage]  <> SOURCE.[UncertainityPercentage]
					  OR TARGET.[Incertidumbre]		      <> SOURCE.[Incertidumbre]			
					  OR TARGET.[OwnershipVolume]		  <> SOURCE.[OwnershipVolume]		
					  OR TARGET.[OwnershipPercentage]	  <> SOURCE.[OwnershipPercentage]	
					  OR TARGET.[AppliedRule]			  <> SOURCE.[AppliedRule]			
					  OR TARGET.[GrossStandardQuantity]   <> SOURCE.[GrossStandardQuantity])

				THEN UPDATE 
					 SET 
					   TARGET.[Net Volume]			   = SOURCE.[Net Volume]			
					  ,TARGET.[Volume Unit]			   = SOURCE.[Volume Unit]			
					  ,TARGET.[EventType]			   = SOURCE.[EventType]				
					  ,TARGET.[SystemName]			   = SOURCE.[SystemName]			
					  ,TARGET.[UncertainityPercentage] = SOURCE.[UncertainityPercentage]
					  ,TARGET.[Incertidumbre]		   = SOURCE.[Incertidumbre]			
					  ,TARGET.[OwnershipVolume]		   = SOURCE.[OwnershipVolume]		
					  ,TARGET.[OwnershipPercentage]	   = SOURCE.[OwnershipPercentage]	
					  ,TARGET.[AppliedRule]			   = SOURCE.[AppliedRule]			
					  ,TARGET.[GrossStandardQuantity]  = SOURCE.[GrossStandardQuantity]
					  ,TARGET.LastModifiedBy           = 'ReportUser'
					  ,TARGET.[LastModifiedDate]       = @TodaysDate
					  
       
				/* 2. Performing the INSERT operation */ 
  
				/* When no records are matched with TARGET table 
				   Then insert the records in the target table */ 
				WHEN NOT MATCHED BY TARGET 
				
				THEN INSERT (
				 [Category]
				,[Element]
				,[NodeName]
				,[CalculationDate]
				,[Product]
				,[ProductId]
				,[OwnerName]
				,[InventoryId]
				,[InventoryProductId]
				,[InventoryDate]
				,[TankName]
				,[BatchId]
				,[Net Volume]
				,[Volume Unit]
				,[EventType]
				,[SystemName]
				,[UncertainityPercentage]
				,[Incertidumbre]
				,[OwnershipVolume]
				,[OwnershipPercentage]
				,[AppliedRule]
				,[GrossStandardQuantity]
				,[OwnershipTicketId]
				,[CreatedBy]
				,[CreatedDate]
				,[LastModifiedBy]
				,[LastModifiedDate]
			     )

			 VALUES 
			 (
			 SOURCE.[Category]
			,SOURCE.[Element]
			,SOURCE.[NodeName]
			,SOURCE.[CalculationDate]
			,SOURCE.[Product]
			,SOURCE.[ProductId]
			,SOURCE.[OwnerName]
			,SOURCE.[InventoryId]
			,SOURCE.[InventoryProductId]
			,SOURCE.[InventoryDate]
			,SOURCE.[TankName]
			,SOURCE.[BatchId]
			,SOURCE.[Net Volume]
			,SOURCE.[Volume Unit]
			,SOURCE.[EventType]
			,SOURCE.[SystemName]
			,SOURCE.[UncertainityPercentage]
			,SOURCE.[Incertidumbre]
			,SOURCE.[OwnershipVolume]
			,SOURCE.[OwnershipPercentage]
			,SOURCE.[AppliedRule]
			,SOURCE.[GrossStandardQuantity]
			,SOURCE.[OwnershipTicketId]
			,SOURCE.[CreatedBy]
			,SOURCE.[CreatedDate]
			,SOURCE.[LastModifiedBy]
			,SOURCE.[LastModifiedDate]
			);


END
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This procedure is to Fetch MovementDetailsWithOwner Data',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveInventoryDetailsWithOwner',
    @level2type = NULL,
    @level2name = NULL