/*-- =================================================================================================
-- Author:		Microsoft
-- Created date: Aug-05-2020
-- Updated date: Sep-15-2020  Removing multiplication with "-1" as per BUG 78346
-- Updated date: May-20-2021  add NodeId Validation
-- -- <Description>:	This procedure is to Fetch InventoryDetailsWithOwner Data For PowerBi Report From
				From Tables(Inventory, InventoryProduct, Unbalance, Ownership, Ticket, Product, Node, 
--				CategoryElement,Category)</Description></Description>
   EXEC [Admin].[usp_SaveMovementsByProduct] @TicketId = -1(full load)
   EXEC [Admin].[usp_SaveMovementsByProduct] @TicketId = 31615(ticketid load)
   SELECT * FROM ADMIN.MovementsByProductWithoutOwner where TicketId = 31615
   UPdate [Offchain].[Unbalance] set [Inputs]=17.00 where TicketId = 31615
-- ===================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveMovementsByProduct]
(
	      @TicketId INT,
		  @NodeId INT = NULL
)
AS
BEGIN
  SET NOCOUNT ON


IF OBJECT_ID('tempdb..#Source')IS NOT NULL
			DROP TABLE #Source


DECLARE @Todaysdate	 DATETIME =  [Admin].[udf_GetTrueDate] ()

--temp table for segment data
	 SELECT 
		     [FilterType]
			,[InitialInventory]			
			,[Input]				
			,[Output]				
			,[IdentifiedLosses]			
			,[Interface]				
			,[Tolerance]			
			,[UnidentifiedLosses]	
			,[FinalInventory]		
			,[Control]			
			,[Category]
			,[Element] 
			,[NodeName]
			,[CalculationDate]
			,[Product]
			,[ProductId]
			,[TicketId]
			,[NodeId]
			,[SegmentId]
			,[SystemId]
			,[PercentageValue]
		    ,'ReportUser'   as [CreatedBy]
			,@TodaysDate    as [CreatedDate]
			,NULL			as [LastModifiedBy]
			,NULL           as [LastModifiedDate]
				INTO #Source from 
				(SELECT DISTINCT  
									'Unbalance'														AS FilterType
									,[Ubal].[InitialInventory] 										AS InitialInventory		-- Inventario Inicial
    								,[Ubal].[Inputs] 												AS Input				-- Entradas
    								,[Ubal].[Outputs] 												AS [Output]				-- Salidas
    								,[Ubal].[IdentifiedLosses]							AS IdentifiedLosses		-- Pérdidas Identificadas
    								,[Ubal].[Interface] * -1  										AS Interface			-- Interfases
    								,CASE WHEN Ubal.ToleranceUnbalance > 0 
										  THEN ABS(Ubal.[Tolerance]) * -1
										  ELSE ABS(Ubal.[Tolerance])
									 END															AS Tolerance			-- Tolerancia
    								,[Ubal].[UnidentifiedLosses] * -1 								AS UnidentifiedLosses	-- Pérdidas No Identificadas
    								,[Ubal].[FinalInvnetory]										AS FinalInventory		-- Inventario Final
    								,[Ubal].[Unbalance] 											AS [Control]			-- Control
									,[Cat].[Name]													AS Category
									,[Element].[Name]												AS Element 
    								,[ND].[Name]													AS NodeName
    								,[Ubal].[CalculationDate] 										AS CalculationDate
    								,[Prd].[Name]													AS Product
    								,[Prd].[ProductId]												AS ProductId
									,[Ubal].[TicketId]												AS TicketId
    								,[Ubal].[NodeId]												AS NodeId
									,[Segment].[ElementId]											AS SegmentId
									,[System].[ElementId]											AS SystemId
									,100                                                            AS PercentageValue

				FROM [Offchain].[Unbalance] Ubal
				INNER JOIN [Admin].[Product] Prd
				ON [Ubal].[ProductId] = [Prd].[ProductId]
				INNER JOIN [Admin].[Ticket] Tick
				ON [Tick].[TicketId] = [Ubal].[TicketId]
				INNER JOIN [Admin].[Node] ND
				ON [ND].[NodeId] = [Ubal].[NodeId]
				INNER JOIN [Admin].[NodeTag] NT
				ON [NT].[NodeId] = [ND].[NodeId]
				INNER JOIN [Admin].[CategoryElement] Element
				ON [NT].[ElementId] = [Element].[ElementId]
				INNER JOIN [Admin].[Category]  Cat
				ON [Element].[CategoryId] = [Cat].[CategoryId]
				LEFT JOIN [Admin].[CategoryElement] Segment
				ON [Segment].[ElementId] = [NT].[ElementId]
				AND [Segment].[CategoryId] = 2  -- SegmentType
				LEFT JOIN [Admin].[CategoryElement] [System]
				ON [System].[ElementId] = [NT].[ElementId]
				AND [System].[CategoryId] = 8  --SystemType
				WHERE [Tick].[Status] = 0		--> 0 = successfully processed
				AND [Tick].[ErrorMessage] IS NULL
				AND [Tick].[TicketTypeId] = 1 -- Cutoff 
				AND [Cat].[Name] IN ('Segmento','Sistema')
				AND (Tick.[TicketId]= @TicketId OR @TicketId = -1 )
				AND (@NodeId IS  NULL OR  (Ubal.NodeId = @NodeId))
				UNION

				SELECT DISTINCT  
									'SegmentUnbalance'												AS FilterType
									,[SU].[InitialInventoryVolume] 									AS InitialInventory		-- Inventario Inicial
    								,[SU].[InputVolume] 											AS Input				-- Entradas
    								,[SU].[OutputVolume] 											AS [Output]				-- Salidas
    								,[SU].[IdentifiedLossesVolume]									AS IdentifiedLosses		-- Pérdidas Identificadas
    								,[SU].[InterfaceVolume]		 									AS Interface			-- Interfases
    								,[SU].[ToleranceVolume]		 									AS Tolerance			-- Tolerancia
    								,[SU].[UnidentifiedLossesVolume]								AS UnidentifiedLosses	-- Pérdidas No Identificadas
    								,[SU].[FinalInventoryVolume] 									AS FinalInventory		-- Inventario Final
    								,[SU].[UnbalanceVolume] 										AS [Control]			-- Control
									,[Cat].[Name]													AS Category
									,[Element].[Name]												AS Element 
    								,NULL															AS NodeName
    								,[SU].[Date] 										            AS CalculationDate
    								,[Prd].[Name]													AS Product
    								,[Prd].[ProductId]												AS ProductId
									,[SU].[TicketId]												AS TicketId
    								,NULL															AS NodeId
									,[SU].[SegmentId]												AS SegmentId
									,NULL															AS SystemId
									,100                                                            AS PercentageValue

				FROM [Admin].[SegmentUnbalance] SU
				INNER JOIN [Admin].[Product] Prd
				ON [SU].[ProductId] = [Prd].[ProductId]
				INNER JOIN [Admin].[Ticket] Tick
				ON [Tick].[TicketId] = [SU].[TicketId] 
				INNER JOIN [Admin].[CategoryElement] Element
				ON [SU].[SegmentId] = [Element].[ElementId]
				AND [Element].[CategoryId] = 2 -- Segmento
				INNER JOIN [Admin].[Category]  Cat
				ON [Element].[CategoryId] = [Cat].[CategoryId]
				WHERE [Tick].[Status] = 0		--> 0 = successfully processed
				AND [Tick].[ErrorMessage] IS NULL
				AND [Tick].[TicketTypeId] = 1 -- Cutoff 
				AND (Tick.[TicketId]= @TicketId OR @TicketId = -1 )
				UNION

				SELECT DISTINCT  
									'SystemUnbalance'												AS FilterType
									,[SysUB].[InitialInventoryVolume] 								AS InitialInventory		-- Inventario Inicial
    								,[SysUB].[InputVolume]											AS Input				-- Entradas
    								,[SysUB].[OutputVolume] 										AS [Output]				-- Salidas
    								,[SysUB].[IdentifiedLossesVolume] 								AS IdentifiedLosses		-- Pérdidas Identificadas
    								,[SysUB].[InterfaceVolume]										AS Interface			-- Interfases
    								,[SysUB].[ToleranceVolume]										AS Tolerance			-- Tolerancia
    								,[SysUB].[UnidentifiedLossesVolume]								AS UnidentifiedLosses	-- Pérdidas No Identificadas
    								,[SysUB].[FinalInventoryVolume] 								AS FinalInventory		-- Inventario Final
    								,[SysUB].[UnbalanceVolume]										AS [Control]			-- Control
									,[Cat].[Name]													AS Category
									,[Element].[Name]												AS Element 
    								,NULL															AS NodeName
    								,[SysUB].[Date]													AS CalculationDate
    								,[Prd].[Name]													AS Product
    								,[Prd].[ProductId]												AS ProductId
									,[SysUB].[TicketId]												AS TicketId
    								,NULL															AS NodeId
									,[SysUB].[SegmentId]											AS SegmentId
									,[SysUB].[SystemId]												AS SystemId
									,100                                                            AS PercentageValue

				FROM [Admin].[SystemUnbalance] SysUB
				INNER JOIN [Admin].[Product] Prd
				ON [SysUB].[ProductId] = [Prd].[ProductId]
				INNER JOIN [Admin].[Ticket] Tick
				ON [Tick].[TicketId] = [SysUB].[TicketId]
				INNER JOIN [Admin].[CategoryElement] Element		
				ON [SysUB].[SystemId] = [Element].[ElementId]
				AND [Element].[CategoryId] = 8  -- Sistema
				INNER JOIN [Admin].[Category]  Cat
				ON [Element].[CategoryId] = [Cat].[CategoryId]

				WHERE [Tick].[Status] = 0		--> 0 = successfully processed
				AND [Tick].[ErrorMessage] IS NULL
				AND [Tick].[TicketTypeId] = 1 -- Cutoff 
				AND (Tick.[TicketId]= @TicketId OR @TicketId = -1 )
						)P
				
				MERGE [Admin].[MovementsByProductWithoutOwner] AS TARGET 
				USING #Source AS SOURCE  
				ON  ISNULL(TARGET.[FilterType]			 ,'')	=  ISNULL(  SOURCE.[FilterType]			 ,'')
				AND ISNULL(TARGET.[Category]			 ,'')	=  ISNULL(  SOURCE.[Category]			 ,'')
				AND ISNULL(TARGET.[Element]		         ,'')	=  ISNULL(  SOURCE.[Element]			 ,'')
				AND ISNULL(TARGET.[NodeName]			 ,'')	=  ISNULL(  SOURCE.[NodeName]			 ,'')
				AND ISNULL(TARGET.[Product]			     ,'')	=  ISNULL(  SOURCE.[Product]	         ,'')
				AND ISNULL(TARGET.[ProductId]			 ,'')	=  ISNULL(  SOURCE.[ProductId]		     ,'')
				AND ISNULL(TARGET.[SegmentId]			 ,'')	=  ISNULL(  SOURCE.[SegmentId]		     ,'')
				AND ISNULL(TARGET.[SegmentId]			 ,'')	=  ISNULL(  SOURCE.[SegmentId]		     ,'')
				AND ISNULL(TARGET.[NodeId]			     ,'')	=  ISNULL(  SOURCE.[NodeId]		         ,'')
				AND ISNULL(TARGET.[TicketId]             ,'')	=  ISNULL(  SOURCE.[TicketId]            ,'')
				AND ISNULL(TARGET.[CalculationDate]      ,'')	=  ISNULL(  SOURCE.[CalculationDate]     ,'')

			

				/*Update records in Target Table using source*/
				WHEN MATCHED  AND ( 
						 TARGET.[InitialInventory]      <> SOURCE.[InitialInventory] 
					  OR TARGET.[Input]			        <> SOURCE.[Input]						
					  OR TARGET.[Output]			    <> SOURCE.[Output]				
					  OR TARGET.[IdentifiedLosses]	    <> SOURCE.[IdentifiedLosses]			
					  OR TARGET.[Interface]             <> SOURCE.[Interface]      
					  OR TARGET.[Tolerance]             <> SOURCE.[Tolerance]  
					  OR TARGET.[UnidentifiedLosses]    <> SOURCE.[UnidentifiedLosses]		
					  OR TARGET.[FinalInventory]		<> SOURCE.[FinalInventory]
					  OR TARGET.[Control]				<> SOURCE.[Control]
					  OR TARGET.[PercentageValue]		<> SOURCE.[PercentageValue]
					  
					  )

				THEN UPDATE 
					 SET 
						TARGET.[InitialInventory]        = SOURCE.[InitialInventory] 
					  , TARGET.[Input]			         = SOURCE.[Input]						
					  , TARGET.[Output]			         = SOURCE.[Output]				
					  , TARGET.[IdentifiedLosses]	     = SOURCE.[IdentifiedLosses]			
					  , TARGET.[Interface]               = SOURCE.[Interface]      
					  , TARGET.[Tolerance]               = SOURCE.[Tolerance]  
					  , TARGET.[UnidentifiedLosses]      = SOURCE.[UnidentifiedLosses]		
					  , TARGET.[FinalInventory]		     = SOURCE.[FinalInventory]
					  , TARGET.[Control]				 = SOURCE.[Control]
					  , TARGET.[PercentageValue]		 = SOURCE.[PercentageValue]
					  , TARGET.LastModifiedBy            = 'ReportUser'
					  , TARGET.[LastModifiedDate]        = @TodaysDate
					  
       
				/* 2. Performing the INSERT operation */ 
  
				/* When no records are matched with TARGET table 
				   Then insert the records in the target table */ 
				WHEN NOT MATCHED BY TARGET 
				
				THEN INSERT (
				 [FilterType]
				,[InitialInventory]		
				,[Input]				
				,[Output]			
				,[IdentifiedLosses]		
				,[Interface]			
				,[Tolerance]			
				,[UnidentifiedLosses]
				,[FinalInventory]	
				,[Control]			
				,[Category]
				,[Element] 
				,[NodeName]
				,[CalculationDate]
				,[Product]
				,[ProductId]
				,[TicketId]
				,[NodeId]
				,[SegmentId]
				,[SystemId]
				,[PercentageValue]
				,[CreatedBy]
				,[CreatedDate]
				,[LastModifiedBy]
				,[LastModifiedDate]
				
			     )

			 VALUES 
			 (
			 SOURCE.[FilterType]
			,SOURCE.[InitialInventory]		
			,SOURCE.[Input]				
			,SOURCE.[Output]				
			,SOURCE.[IdentifiedLosses]		
			,SOURCE.[Interface]			
			,SOURCE.[Tolerance]			
			,SOURCE.[UnidentifiedLosses]
			,SOURCE.[FinalInventory]		
			,SOURCE.[Control]			
			,SOURCE.[Category]
			,SOURCE.[Element] 
			,SOURCE.[NodeName]
			,SOURCE.[CalculationDate]
			,SOURCE.[Product]
			,SOURCE.[ProductId]
			,SOURCE.[TicketId]
			,SOURCE.[NodeId]
			,SOURCE.[SegmentId]
			,SOURCE.[SystemId]
			,SOURCE.[PercentageValue]
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
    @level1name = N'usp_SaveMovementsByProduct',
    @level2type = NULL,
    @level2name = NULL