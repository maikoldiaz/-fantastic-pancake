/*-- =================================================================================================
-- Author:		Intergrupo
-- Created date: May-25-2021
-- Updated date: Ago-03-2021 remove movement exist validation
-- Updated date: Ago-04-2021 Add IsReconciled field
-- Updated date: Ago-31-2021 Add validation ownershipvolumen By Cristian Higuita
-- ===================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveMovementDetailsWithOwnerOtherSegment]
(
	@OwnershipTicketId INT,
	@NodeId INT = NULL
)
AS
BEGIN
  SET NOCOUNT ON

  --Se borran tablas temporales
	  IF OBJECT_ID('tempdb..#Source')IS NOT NULL
				DROP TABLE #Source
	  IF OBJECT_ID('tempdb..#segment')IS NOT NULL
				DROP TABLE #segment
	  IF OBJECT_ID('tempdb..#system')IS NOT NULL
				DROP TABLE #system
	

	-- se obtiene fecha actual de creacion de registro
    DECLARE @Todaysdate	 DATETIME =  [Admin].[udf_GetTrueDate] ()

	
	-- se elimina data que no se utilizara en el reporte
	delete a from Admin.MovementDetailsWithOwnerOtherSegment a
	where a.OwnershipTicketId = @OwnershipTicketId

		select  [Element].[name] OtherSegment
			,[Mov].[MovementId]
		  ,[Mov].[MovementTransactionId]
		  ,@OwnershipTicketId [OwnershipTicketId]
		  ,[Mov].operationaldate
		  ,[Mov].MovementTypeName [Operacion]
		  ,[Mov].SourceNodeId
		  ,[Mov].SourceNodeName [SourceNode]
		  ,[Mov].DestinationNodeId
		  ,[Mov].DestinationNodeName [DestinationNode]
		  ,[Mov].SourceProductName [SourceProduct]
		  ,[Mov].DestinationProductName [DestinationProduct]
		  ,[Mov].[NetStandardVolume]
		  ,[Mov].[IsReconciled]
		  ,CEUnits.[Name] [MeasurementUnit]
		  ,CASE WHEN [Ownership].OwnerId is not null THEN  [Ownership].OwnershipPercentage WHEN [Owner].OwnershipValueUnit like '%[%]%' OR UPPER([Owner].OwnershipValueUnit) = 'PORCENTAJE' 
				THEN [Owner].OwnershipValue ELSE CAST([Owner].OwnershipValue / [Mov].[NetStandardVolume] * 100 AS DECIMAL(18,2)) END AS OwnershipPercentage
		  ,[ElementOwner].Name	[OwnerName]	  
		  ,CASE WHEN [Ownership].OwnerId is not null THEN [Ownership].OwnershipVolume WHEN [Owner].OwnershipValueUnit like '%[%]%' OR UPPER([Owner].OwnershipValueUnit) = 'PORCENTAJE' 
				THEN CAST((([Mov].NetStandardVolume * [Owner].Ownershipvalue)/100) AS DECIMAL(18,2)) ELSE [Owner].OwnershipValue END AS OwnershipVolume
		  ,[Element].CategoryId
		  ,[Elementsegmentoriginal].ElementId 
		  ,[Elementsegmentoriginal].[Name] [Element]
		  ,Cat.[Name] AS [Category]
		  ,St.[Name] [ScenarioName]	
		  into #segment
		  from [Admin].[view_MovementInformation] [Mov]
INNER JOIN [Admin].[CategoryElement] [Element] 
				ON [Mov].segmentid = [Element].[ElementId]
INNER JOIN [Admin].[CategoryElement] CEUnits
				ON CEUnits.ElementId = Mov.MeasurementUnit 
				AND CEUnits.CategoryID = 6 --'Unidad de Medida' From name Column in Category
left JOIN [offchain].[Ownership] [Ownership] ON [Mov].[MovementTransactionId] = [Ownership].[MovementTransactionId] AND [Mov].[OwnershipTicketId] = [Ownership].[TicketId]
left JOIN [offchain].[Owner] [Owner] ON [Mov].[MovementTransactionId] = [Owner].[MovementTransactionId] 
	INNER JOIN [Admin].[CategoryElement] [ElementOwner] 
					ON case when [Ownership].OwnerId is null then  [Owner].OwnerId else [Ownership].OwnerId end = [ElementOwner].ElementId
					AND [ElementOwner].[CategoryID] = 7 -- 'Propietario' From name 
INNER JOIN [Admin].[Category]  [Cat]
				ON [Element].[CategoryId] = [Cat].[CategoryId]
LEFT JOIN [Admin].ScenarioType St ON St.ScenarioTypeId = Mov.scenarioId
INNER JOIN [Admin].[Ticket] T
				ON T.TicketId = @OwnershipTicketId
INNER JOIN [Admin].[CategoryElement] [Elementsegmentoriginal] 
				ON [T].[CategoryElementId] = [Elementsegmentoriginal].[ElementId]
where  OwnershipTicketConciliationId = @OwnershipTicketId 






	SELECT * INTO #System  
		FROM
	( SELECT DISTINCT 
			[MovementId]
			,[MovementTransactionId]
			,[OwnershipTicketId]
			,operationaldate
			,[Operacion]
			,[SourceNodeId]
			,[SourceNode]
			,[DestinationNodeId]
			,[DestinationNode]			
			,[SourceProduct]
			,[DestinationProduct]
			,[NetStandardVolume]
			,[MeasurementUnit]
			,[OwnershipPercentage]
			,[OwnerName]
			,[OwnershipVolume]
			,[ScenarioName]			
			,Cat1.[Name] AS [Category]
			,CatEle.[Name]	AS [Element]
			,OtherSegment
			,[IsReconciled]
	FROM #segment SQ
	INNER JOIN [Admin].NodeTag NT            
			 ON NT.ElementId = SQ.ElementId            
			 INNER JOIN [Admin].Category  Cat            
			 ON SQ.CategoryId = Cat.CategoryId            
			 INNER JOIN [Admin].NodeTag NT1            
			 ON NT.NodeId = NT1.NodeId            
			 INNER JOIN [Admin].CategoryElement CatEle            
			 ON CatEle.ElementId = NT1.ElementId AND CatEle.CategoryId = 8            
			 INNER JOIN [Admin].Category  Cat1            
			 ON CatEle.CategoryId = Cat1.CategoryId) P 



		select 
		[MovementId]
			,[MovementTransactionId]
			,[OwnershipTicketId]
			,operationaldate
			,[Operacion]
			,[SourceNode]
			,[DestinationNode]
			,[SourceProduct]
			,[DestinationProduct]
			,[NetStandardVolume]
			,[MeasurementUnit]
			,[OwnershipPercentage]
			,[OwnerName]
			,[OwnershipVolume]
			,[ScenarioName]
			,[Category]
			,[Element]
			,[NodeName]
			,[OperationalDate] AS [CalculationDate]
			,OtherSegment
			,[IsReconciled]
			,'ReportUser' AS [CreatedBy]
			,@TodaysDate  AS [CreatedDate]
			,NULL         AS [LastModifiedBy]
			,NULL         AS [LastModifiedDate]
			INTO #Source
		 FROM (
		 
			 select 
				[MovementId]
				,[MovementTransactionId]
				,[OwnershipTicketId]
				,operationaldate
				,[Operacion]
				,[SourceNode]
				,[DestinationNode]
				,[SourceProduct]
				,[DestinationProduct]
				,[NetStandardVolume]
				,[MeasurementUnit]
				,[OwnershipPercentage]
				,[OwnerName]
				,[OwnershipVolume]
				,[ScenarioName]
				,[Category]
				,[Element]
				,OtherSegment
				,[IsReconciled]
				,CONCAT('-_-', ND.[Name], '-_-')   AS NodeName
			 FROM #Segment Mov
						 INNER JOIN Admin.[Node] ND
				 		  ON (ND.NodeId = Mov.SourceNodeId 
				 		  OR ND.NodeId = Mov.DestinationNodeId)
		UNION 

			select 
				[MovementId]
				,[MovementTransactionId]
				,[OwnershipTicketId]
				,operationaldate
				,[Operacion]
				,[SourceNode]
				,[DestinationNode]
				,[SourceProduct]
				,[DestinationProduct]
				,[NetStandardVolume]
				,[MeasurementUnit]
				,[OwnershipPercentage]
				,[OwnerName]
				,[OwnershipVolume]
				,[ScenarioName]
				,[Category]
				,[Element]
				,OtherSegment
				,[IsReconciled]
				,CONCAT('-_-', 'Todos', '-_-')   AS NodeName
			 FROM #Segment Mov

		 UNION

		  select 
				[MovementId]
				,[MovementTransactionId]
				,[OwnershipTicketId]
				,operationaldate
				,[Operacion]
				,[SourceNode]
				,[DestinationNode]
				,[SourceProduct]
				,[DestinationProduct]
				,[NetStandardVolume]
				,[MeasurementUnit]
				,[OwnershipPercentage]
				,[OwnerName]
				,[OwnershipVolume]
				,[ScenarioName]
				,[Category]
				,[Element]
				,OtherSegment
				,[IsReconciled]
				,CONCAT('-_-', ND.[Name], '-_-')   AS NodeName
			 FROM #System Mov
						 INNER JOIN Admin.[Node] ND
				 		  ON (ND.NodeId = Mov.SourceNodeId 
				 		  OR ND.NodeId = Mov.DestinationNodeId)
		UNION 

			select 
				[MovementId]
				,[MovementTransactionId]
				,[OwnershipTicketId]
				,operationaldate
				,[Operacion]
				,[SourceNode]
				,[DestinationNode]
				,[SourceProduct]
				,[DestinationProduct]
				,[NetStandardVolume]
				,[MeasurementUnit]
				,[OwnershipPercentage]
				,[OwnerName]
				,[OwnershipVolume]
				,[ScenarioName]
				,[Category]
				,[Element]
				,OtherSegment
				,[IsReconciled]
				,CONCAT('-_-', 'Todos', '-_-')   AS NodeName
			 FROM #System Mov

		 )SubQ




				MERGE Admin.MovementDetailsWithOwnerOtherSegment AS TARGET 
				USING #Source AS SOURCE 
				ON  ISNULL(TARGET.[MovementTransactionId]	,'')=	ISNULL(SOURCE.[MovementTransactionId]	,'')
				AND ISNULL(TARGET.MovementId				,'')=	ISNULL(SOURCE.MovementId				,'')
				AND ISNULL(TARGET.[CalculationDate]			,'')=	ISNULL(SOURCE.[CalculationDate]			,'')
				AND ISNULL(TARGET.[SourceNode]				,'')=	ISNULL(SOURCE.[SourceNode]				,'')
				AND ISNULL(TARGET.[DestinationNode]			,'')=	ISNULL(SOURCE.[DestinationNode]			,'')
				AND ISNULL(TARGET.[SourceProduct]			,'')=	ISNULL(SOURCE.[SourceProduct]			,'')
				AND ISNULL(TARGET.[DestinationProduct]		,'')=	ISNULL(SOURCE.[DestinationProduct]		,'')
				AND ISNULL(TARGET.[MeasurementUnit]			,'')=	ISNULL(SOURCE.[MeasurementUnit]			,'')
				AND ISNULL(TARGET.[SourceProduct]			,'')=	ISNULL(SOURCE.[SourceProduct]			,'')
				AND ISNULL(TARGET.[OwnerName]				,'')=	ISNULL(SOURCE.[OwnerName]				,'')
				AND ISNULL(TARGET.[Operacion]				,'')=	ISNULL(SOURCE.[Operacion]				,'')				
				AND ISNULL(TARGET.[Category]				,'')=	ISNULL(SOURCE.[Category]				,'')
				AND ISNULL(TARGET.[Element]					,'')=	ISNULL(SOURCE.[Element]					,'')
				AND ISNULL(TARGET.[NodeName]				,'')=	ISNULL(SOURCE.[NodeName]				,'')
				AND ISNULL(TARGET.[OwnershipTicketId]       ,'')=   ISNULL(SOURCE.[OwnershipTicketId]       ,'')
				AND ISNULL(TARGET.[scenarioname]       ,'')=   ISNULL(SOURCE.[scenarioname]       ,'')

				WHEN MATCHED and (TARGET.[NetStandardVolume] <> SOURCE.[NetStandardVolume])
				THEN UPDATE 
				  SET TARGET.[NetStandardVolume]	   =  SOURCE.[NetStandardVolume]
				WHEN NOT MATCHED BY TARGET  
				THEN INSERT ([MovementId],[MovementTransactionId],[OwnershipTicketId],[Operacion],[SourceNode],[DestinationNode]
		  ,[SourceProduct],[DestinationProduct],[NetStandardVolume],[MeasurementUnit],[OwnershipPercentage]
		  ,[OwnerName],[OwnershipVolume],[ScenarioName],[Category],[Element],[NodeName],[CalculationDate],[OtherSegment],[IsReconciled],[CreatedBy],[CreatedDate])

		  values (SOURCE.[MovementId],SOURCE.[MovementTransactionId],SOURCE.[OwnershipTicketId],SOURCE.[Operacion],SOURCE.[SourceNode],SOURCE.[DestinationNode]
		  ,SOURCE.[SourceProduct],SOURCE.[DestinationProduct],SOURCE.[NetStandardVolume],SOURCE.[MeasurementUnit],SOURCE.[OwnershipPercentage]
		  ,SOURCE.[OwnerName],SOURCE.[OwnershipVolume],SOURCE.[ScenarioName],SOURCE.[Category],SOURCE.[Element],SOURCE.[NodeName],SOURCE.[CalculationDate]
		  ,Source.OtherSegment,[IsReconciled],[CreatedBy],[CreatedDate]);

end

