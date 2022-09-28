-- ======================================================================================================================
-- Author: Microsoft
-- Create date:  Aug-07-2020
-- Updated date: May-06-2020; added NodeId parameter
-- <Description>: This procedure is to Fetch Data [Admin].[BalanceControl] For PowerBi Report From Tables (Unbalance, Product, Node, NodeTag,  CategoryElement,Category)</Description>
--EXEC Admin.usp_SaveBalanceControl @TicketId = -1
--SELECT * FROM [Admin].[BalanceControl] 
-- ======================================================================================================================


CREATE  PROCEDURE [Admin].[usp_SaveBalanceControl]
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

	SELECT DISTINCT  
									 [Cat].[Name]													AS Category
									,[Element].[Name]												AS Element 
    								,[ND].[Name]													AS NodeName
    								,CAST([Ubal].[CalculationDate] AS DATE)							AS CalculationDate
    								,[Prd].[Name]													AS Product
    								,[Prd].[ProductId]												AS ProductId
									,[Ubal].[TicketId]												AS TicketId
    								,[Ubal].[NodeId]												AS NodeId
									,[Ubal].[ToleranceInputs]										AS Input
									,[Ubal].[ToleranceUnbalance]									AS [Unbalance]	
    								,[Ubal].[StandardUncertainty]									AS [StandardUncertainty]
    								,[Ubal].[AverageUncertainty]									AS [AverageUncertainty]
    								,[Ubal].[AverageUncertaintyUnbalancePercentage]					AS [AverageUncertaintyUnbalance]
    								,[Ubal].[Warning]												AS [Warning]
    								,[Ubal].[Action]												AS [Action]
    								,[Ubal].[ControlTolerance]										AS [ControlTolerance]
									,[Warning] * (-1)												AS [Warning(-)]
									,[Action] * (-1)												AS [Action(-)]
									,[ControlTolerance] * (-1)										AS [ControlTolerance(-)]
									,'ReportUser'													as [CreatedBy]
									,@TodaysDate													as [CreatedDate]
									,NULL															as [LastModifiedBy]
									,NULL															as [LastModifiedDate]
				INTO #Source
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
				WHERE [Tick].[Status] = 0		--> 0 = Successfully processed
				AND [Tick].[ErrorMessage] IS NULL
				AND [Tick].[TicketTypeId] = 1 -- Cutoff 
				AND [Element].[CategoryId] = 2  -- SegmentType
				AND [Element].[IsActive] = 1  -- Active Segments
				AND (Tick.[TicketId]= @TicketId OR @TicketId = -1 )
				AND (@NodeId IS  NULL OR  (Ubal.NodeId = @NodeId))


				MERGE [Admin].[BalanceControl] AS TARGET 
				USING #Source AS SOURCE  
				ON  ISNULL(TARGET.[Category]			 ,'')=  ISNULL(  SOURCE.[Category]			 ,'')
				AND ISNULL(TARGET.[Element]			     ,'')=  ISNULL(  SOURCE.[Element]			 ,'')
				AND ISNULL(TARGET.[NodeName]			 ,'')=  ISNULL(  SOURCE.[NodeName]			 ,'')
				AND ISNULL(TARGET.[CalculationDate]		 ,'')=  ISNULL(  SOURCE.[CalculationDate]	 ,'')
				AND ISNULL(TARGET.[Product]				 ,'')=  ISNULL(  SOURCE.[Product]			 ,'')
				AND ISNULL(TARGET.[ProductId]			 ,'')=  ISNULL(  SOURCE.[ProductId]			 ,'')
				AND ISNULL(TARGET.[TicketId]			 ,'')=  ISNULL(  SOURCE.[TicketId]			 ,'')
				AND ISNULL(TARGET.[NodeId]			     ,'')=  ISNULL(  SOURCE.[NodeId]			 ,'')

				/*Update records in Target Table using source*/
				WHEN MATCHED  AND ( 
						 TARGET.Input								<> SOURCE.Input 
					  OR TARGET.[Unbalance]							<> SOURCE.[Unbalance]							
					  OR TARGET.[StandardUncertainty]				<> SOURCE.[StandardUncertainty]			
					  OR TARGET.[AverageUncertainty]				<> SOURCE.[AverageUncertainty]      
					  OR TARGET.[AverageUncertaintyUnbalance]       <> SOURCE.[AverageUncertaintyUnbalance]  
					  OR TARGET.[Warning]                           <> SOURCE.[Warning]		
					  OR TARGET.[Action]                            <> SOURCE.[Action]
					  OR TARGET.[ControlTolerance]                  <> SOURCE.[ControlTolerance]
					  OR TARGET.[Warning(-)]                        <> SOURCE.[Warning(-)]
					  OR TARGET.[Action(-)]                         <> SOURCE.[Action(-)]
					  OR TARGET.[ControlTolerance(-)]               <> SOURCE.[ControlTolerance(-)])

				THEN UPDATE 
					 SET 
					  	TARGET.Input								= SOURCE.Input 
					  , TARGET.[Unbalance]							= SOURCE.[Unbalance]											
					  , TARGET.[StandardUncertainty]				= SOURCE.[StandardUncertainty]			
					  , TARGET.[AverageUncertainty]				    = SOURCE.[AverageUncertainty]      
					  , TARGET.[AverageUncertaintyUnbalance]        = SOURCE.[AverageUncertaintyUnbalance] 
					  , TARGET.[Warning]                            = SOURCE.[Warning]			
					  , TARGET.[Action]                             = SOURCE.[Action]
					  , TARGET.[ControlTolerance]                   = SOURCE.[ControlTolerance]
					  , TARGET.[Warning(-)]                         = SOURCE.[Warning(-)]
					  , TARGET.[Action(-)]                          = SOURCE.[Action(-)]
					  , TARGET.[ControlTolerance(-)]                = SOURCE.[ControlTolerance(-)]
					  ,	TARGET.LastModifiedBy						= 'ReportUser'
					  ,	TARGET.[LastModifiedDate]					= @TodaysDate


			 /* 2. Performing the INSERT operation */ 
  
				/* When no records are matched with TARGET table 
				   Then insert the records in the target table */ 
				WHEN NOT MATCHED BY TARGET 
				
				THEN INSERT(
				 [Category]
				,[Element] 
				,[NodeName]
				,[CalculationDate]
				,[Product]
				,[ProductId]
				,[TicketId]
				,[NodeId]
				,[Input]
				,[Unbalance]	
				,[StandardUncertainty]
				,[AverageUncertainty]
				,[AverageUncertaintyUnbalance]
				,[Warning]
				,[Action]
				,[ControlTolerance]
				,[Warning(-)]
				,[Action(-)]
				,[ControlTolerance(-)] 
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
			,SOURCE.[TicketId]
			,SOURCE.[NodeId]
			,SOURCE.[Input]
			,SOURCE.[Unbalance]	
			,SOURCE.[StandardUncertainty]
			,SOURCE.[AverageUncertainty]
			,SOURCE.[AverageUncertaintyUnbalance]
			,SOURCE.[Warning]
			,SOURCE.[Action]
			,SOURCE.[ControlTolerance]
			,SOURCE.[Warning(-)]
			,SOURCE.[Action(-)]
			,SOURCE.[ControlTolerance(-)] 
			,SOURCE.[CreatedBy]
			,SOURCE.[CreatedDate]
			,SOURCE.[LastModifiedBy]
			,SOURCE.[LastModifiedDate]
			);		

END

Go
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This procedure is to Fetch Data [Admin].[BalanceControl] For PowerBi Report From Tables (Unbalance, Product, Node, NodeTag,  CategoryElement,Category)',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveBalanceControl',
    @level2type = NULL,
    @level2name = NULL