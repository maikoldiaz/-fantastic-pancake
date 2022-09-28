/*-- =================================================================================================
-- Author:		Microsoft
-- Created date: Aug-05-2020
-- Updated date: Jun-10-2021 -- Add Field Measurement Unit 
-- <Description>:	This View is to Fetch MovementsByProductWithOwner Data For PowerBi Report From
				Tables(OwnershipCalculation, SegmentOwnershipCalculation, SystemOwnershipCalculation,
				Product, Ticket, Node, NodeTag, CategoryElement,  Category)</Description>
-- ===================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveMovementsByProductWithOwner]
(
	      @OwnershipTicketId INT,
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
			,[OwnerName]			
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
			,[ProductName]
			,[ProductId]
			,[MeasurementUnit]
			,[OwnershipTicketId]
			,[NodeId]
			,[SegmentId]
			,[SystemId]
			,[NodeTagActive]
		    ,'ReportUser'   as [CreatedBy]
			,@TodaysDate    as [CreatedDate]
			,NULL			as [LastModifiedBy]
			,NULL           as [LastModifiedDate]


INTO #Source from 
(SELECT DISTINCT			
					'Ownership'														AS FilterType
					,[OC].[InitialInventoryVolume]									AS InitialInventory		-- Inventario Inicial
					,[ElementOwner].[Name]											AS OwnerName			-- Propietario
    				,[OC].[InputVolume] 											AS Input				-- Entradas
    				,[OC].[OutputVolume] 											AS [Output]				-- Salidas
    				,[OC].[IdentifiedLossesVolume]									AS IdentifiedLosses		-- Pérdidas Identificadas
    				,[OC].[InterfaceVolume] 										AS Interface			-- Interfases
    				,[OC].[ToleranceVolume] 										AS Tolerance			-- Tolerancia
    				,[OC].[UnidentifiedLossesVolume] 								AS UnidentifiedLosses	-- Pérdidas No Identificadas
    				,[OC].[FinalInventoryVolume] 									AS FinalInventory		-- Inventario Final
    				,[OC].[UnbalanceVolume]											AS [Control]			-- Control
					,[Cat].[Name]													AS Category
				    ,[Element].[Name]												AS Element 
    				,[ND].[Name]													AS NodeName
    				,[OC].[Date] 													AS CalculationDate
    				,[Prd].[Name]													AS ProductName
    				,[Prd].[ProductId]												AS ProductId
					,Measu.[Name]													AS MeasurementUnit
                    ,[OC].[OwnershipTicketId]										AS OwnershipTicketId
    				,[OC].[NodeId]													AS NodeId
					,[Segment].[ElementId]											AS SegmentId
					,[System].[ElementId]											AS SystemId
					,CASE WHEN [OC].[Date] BETWEEN [NT].[StartDate] AND [NT].[EndDate]
						  THEN 1
						  ELSE 0 
						  END AS NodeTagActive
		FROM [Admin].[OwnershipCalculation] OC
		INNER JOIN [Admin].[Product] Prd
        ON [OC].[ProductId] = [Prd].[ProductId]
		INNER JOIN [Admin].[Ticket] Tick
        ON [Tick].[TicketId] = [OC].[OwnershipTicketId]
		INNER JOIN [Admin].[CategoryElement] ElementOwner 
        ON [OC].[OwnerId] = [ElementOwner].[ElementId]
		INNER JOIN [Admin].[Node] ND
        ON [ND].[NodeId] = [OC].[NodeId]
		INNER JOIN [Admin].[NodeTag] NT
        ON [NT].[NodeId] = [ND].[NodeId]
		INNER JOIN [Admin].[CategoryElement] Element
        ON [NT].[ElementId] = [Element].[ElementId]
		LEFT JOIN [Admin].[CategoryElement] Segment
		ON [Segment].[ElementId] = [NT].[ElementId]
		AND [Segment].[CategoryId] = 2
		LEFT JOIN [Admin].[CategoryElement] System
		ON [System].[ElementId] = [NT].[ElementId]
		AND [System].[CategoryId] = 8
		INNER JOIN [Admin].[Category]  Cat
        ON [Element].[CategoryId] = [Cat].[CategoryId]
		LEFT JOIN [Admin].[CategoryElement] Measu
		ON Measu.[ElementId] = [OC].MeasurementUnit
		AND Measu.[CategoryId] = 6
     	WHERE [Tick].[Status] = 0		--> 0 = successfully processed
		AND [Tick].[ErrorMessage] IS NULL
		AND [Tick].[TicketTypeId] = 2 --> Here ticket type id 2 is "OWNERSHIP"
		AND [ElementOwner].[CategoryId] = 7 --> Here categoryId 7 is "Propietario"
		AND [Cat].[Name] IN ('Segmento','Sistema')
		AND (Tick.[TicketId]= @OwnershipTicketId OR @OwnershipTicketId = -1 )
		AND (@NodeId IS NULL OR(OC.NodeId = @NodeId))

UNION

SELECT DISTINCT			
					'SegmentOwnership'													AS FilterType
					,[SOC].[InitialInventoryVolume] 									AS InitialInventory		-- Inventario Inicial
					,[ElementOwner].[Name]												AS OwnerName			-- Propietario
    				,[SOC].[InputVolume] 												AS Input				-- Entradas
    				,[SOC].[OutputVolume] 												AS [Output]				-- Salidas
    				,[SOC].[IdentifiedLossesVolume] 									AS IdentifiedLosses		-- Pérdidas Identificadas
    				,[SOC].[InterfaceVolume]											AS Interface			-- Interfases
    				,[SOC].[ToleranceVolume]											AS Tolerance			-- Tolerancia
    				,[SOC].[UnidentifiedLossesVolume]									AS UnidentifiedLosses	-- Pérdidas No Identificadas
    				,[SOC].[FinalInventoryVolume] 										AS FinalInventory		-- Inventario Final
    				,[SOC].[UnbalanceVolume] 											AS [Control]			-- Control
					,[Cat].[Name]														AS Category
				    ,[Element].[Name]													AS Element 
    				,NULL																AS NodeName
    				,[SOC].[Date] 											            AS CalculationDate
    				,[Prd].[Name]														AS ProductName
    				,[Prd].[ProductId]													AS ProductId
					,Measu.[name]														AS MeasurementUnit
                    ,[SOC].[OwnershipTicketId]											AS OwnershipTicketId
    				,NULL																AS NodeId
					,[SOC].[SegmentId]													AS SegmentId
					,NULL																AS SystemId
					,NULL																AS NodeTagActive
		FROM [Admin].[SegmentOwnershipCalculation] SOC
		INNER JOIN [Admin].[Product] Prd
        ON [SOC].[ProductId] = [Prd].[ProductId]
		INNER JOIN [Admin].[Ticket] Tick
        ON [Tick].[TicketId] = [SOC].[OwnershipTicketId] 
		INNER JOIN [Admin].[CategoryElement] ElementOwner 
        ON [SOC].[OwnerId] = [ElementOwner].[ElementId]
		INNER JOIN [Admin].[CategoryElement] Element
        ON [SOC].[SegmentId] = [Element].[ElementId]
		AND [Element].[CategoryId] = 2 -- Segmento
		INNER JOIN [Admin].[Category]  Cat
        ON [Element].[CategoryId] = [Cat].[CategoryId]	
		LEFT JOIN [Admin].[CategoryElement] Measu
		ON Measu.[ElementId] = SOC.MeasurementUnit
		AND Measu.[CategoryId] = 6
		WHERE [Tick].[Status] = 0		--> 0 = successfully processed
		AND [Tick].[ErrorMessage] IS NULL
		AND [Tick].[TicketTypeId] = 2 --> Here ticket type id 2 is "OWNERSHIP"
		AND [ElementOwner].[CategoryId] = 7 --> Here categoryId 7 is "Propietario"
		AND (Tick.[TicketId]= @OwnershipTicketId OR @OwnershipTicketId = -1 )

UNION

SELECT DISTINCT			
					'SystemOwnership'													AS FilterType
					,[SysOC].[InitialInventoryVolume] 									AS InitialInventory		-- Inventario Inicial
					,[ElementOwner].[Name]												AS OwnerName			-- Propietario
    				,[SysOC].[InputVolume]												AS Input				-- Entradas
    				,[SysOC].[OutputVolume]												AS [Output]				-- Salidas
    				,[SysOC].[IdentifiedLossesVolume]									AS IdentifiedLosses		-- Pérdidas Identificadas
    				,[SysOC].[InterfaceVolume]											AS Interface			-- Interfases
    				,[SysOC].[ToleranceVolume]											AS Tolerance			-- Tolerancia
    				,[SysOC].[UnidentifiedLossesVolume]									AS UnidentifiedLosses	-- Pérdidas No Identificadas
    				,[SysOC].[FinalInventoryVolume] 									AS FinalInventory		-- Inventario Final
    				,[SysOC].[UnbalanceVolume]											AS [Control]			-- Control
					,[Cat].[Name]														AS Category
				    ,[Element].[Name]													AS Element 
    				,NULL																AS NodeName
    				,[SysOC].[Date] 													AS CalculationDate
    				,[Prd].[Name]														AS ProductName
    				,[Prd].[ProductId]													AS ProductId
					,Measu.[Name]														AS MeasurementUnit
                    ,[SysOC].[OwnershipTicketId]										AS OwnershipTicketId
    				,NULL																AS NodeId
					,[SysOC].[SegmentId]												AS SegmentId
					,[SysOC].[SystemId]													AS SystemId
					,NULL																AS NodeTagActive
		FROM [Admin].[SystemOwnershipCalculation] SysOC
		INNER JOIN [Admin].[Product] Prd
        ON [SysOC].[ProductId] = [Prd].[ProductId]
		INNER JOIN [Admin].[Ticket] Tick
        ON [Tick].[TicketId] = [SysOC].[OwnershipTicketId]
		INNER JOIN [Admin].[CategoryElement] ElementOwner 
        ON [SysOC].[OwnerId] = [ElementOwner].[ElementId]
		INNER JOIN [Admin].[CategoryElement] Element		
        ON [SysOC].[SystemId] = [Element].[ElementId]
		AND [Element].[CategoryId] = 8  -- Sistema
		INNER JOIN [Admin].[Category]  Cat
        ON [Element].[CategoryId] = [Cat].[CategoryId]	
		LEFT JOIN [Admin].[CategoryElement] Measu
		ON Measu.[ElementId] = SysOC.MeasurementUnit
		AND Measu.[CategoryId] = 6
		WHERE [Tick].[Status] = 0		--> 0 = successfully processed
		AND [Tick].[ErrorMessage] IS NULL
		AND [Tick].[TicketTypeId] = 2 --> Here ticket type id 2 is "OWNERSHIP"
		AND [ElementOwner].[CategoryId] = 7 --> Here categoryId 7 is "Propietario"
		AND (Tick.[TicketId]= @OwnershipTicketId OR @OwnershipTicketId = -1 )
		)P
				
				MERGE [Admin].[MovementsByProductWithOwner] AS TARGET 
				USING #Source AS SOURCE  
				ON  ISNULL(TARGET.[FilterType]			 ,'')	=  ISNULL(  SOURCE.[FilterType]			 ,'')
				AND ISNULL(TARGET.[OwnerName]			 ,'')	=  ISNULL(  SOURCE.[OwnerName]			 ,'')
				AND ISNULL(TARGET.[Category]			 ,'')	=  ISNULL(  SOURCE.[Category]			 ,'')
				AND ISNULL(TARGET.[Element]		         ,'')	=  ISNULL(  SOURCE.[Element]			 ,'')
				AND ISNULL(TARGET.[NodeName]			 ,'')	=  ISNULL(  SOURCE.[NodeName]			 ,'')
				AND ISNULL(TARGET.[ProductName]			 ,'')	=  ISNULL(  SOURCE.[ProductName]	     ,'')
				AND ISNULL(TARGET.[ProductId]			 ,'')	=  ISNULL(  SOURCE.[ProductId]		     ,'')
				AND ISNULL(TARGET.[MeasurementUnit]		 ,'')	=  ISNULL(  SOURCE.[MeasurementUnit]	 ,'')
				AND ISNULL(TARGET.[SegmentId]			 ,'')	=  ISNULL(  SOURCE.[SegmentId]		     ,'')
				AND ISNULL(TARGET.[SystemId]			 ,'')	=  ISNULL(  SOURCE.[SystemId]		     ,'')
				AND ISNULL(TARGET.[NodeId]			     ,'')	=  ISNULL(  SOURCE.[NodeId]		         ,'')
				AND ISNULL(TARGET.[OwnershipTicketId]    ,'')	=  ISNULL(  SOURCE.[OwnershipTicketId]   ,'')
				AND ISNULL(TARGET.[NodeTagActive]		 ,'')	=  ISNULL(  SOURCE.[NodeTagActive]		 ,'')


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
					  
					  )

				THEN UPDATE 
					 SET 
						 TARGET.[InitialInventory]      = SOURCE.[InitialInventory] 
					  , TARGET.[Input]			        = SOURCE.[Input]						
					  , TARGET.[Output]			        = SOURCE.[Output]				
					  , TARGET.[IdentifiedLosses]	    = SOURCE.[IdentifiedLosses]			
					  , TARGET.[Interface]              = SOURCE.[Interface]      
					  , TARGET.[Tolerance]              = SOURCE.[Tolerance]  
					  , TARGET.[UnidentifiedLosses]     = SOURCE.[UnidentifiedLosses]		
					  , TARGET.[FinalInventory]		    = SOURCE.[FinalInventory]
					  , TARGET.[Control]				= SOURCE.[Control]
					  ,TARGET.LastModifiedBy             = 'ReportUser'
					  ,TARGET.[LastModifiedDate]         = @TodaysDate
					  
       
				/* 2. Performing the INSERT operation */ 
  
				/* When no records are matched with TARGET table 
				   Then insert the records in the target table */ 
				WHEN NOT MATCHED BY TARGET 
				
				THEN INSERT (
				 [FilterType]
				,[InitialInventory]			
				,[OwnerName]			
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
				,[ProductName]
				,[ProductId]
				,[MeasurementUnit]
				,[OwnershipTicketId]
				,[NodeId]
				,[SegmentId]
				,[SystemId]
				,[NodeTagActive]
				,[CreatedBy]
				,[CreatedDate]
				,[LastModifiedBy]
				,[LastModifiedDate]
				
			     )

			 VALUES 
			 (
			 SOURCE.[FilterType]
			,SOURCE.[InitialInventory]		
			,SOURCE.[OwnerName]			
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
			,SOURCE.[ProductName]
			,SOURCE.[ProductId]
			,SOURCE.[MeasurementUnit]
			,SOURCE.[OwnershipTicketId]
			,SOURCE.[NodeId]
			,SOURCE.[SegmentId]
			,SOURCE.[SystemId]
			,SOURCE.[NodeTagActive]
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
    @level1name = N'usp_SaveMovementsByProductWithOwner',
    @level2type = NULL,
    @level2name = NULL