/*-- =============================================================================================================================
-- Author:			Microsoft
-- Created Date:	Dec-16-2019
-- Updated Date:	Mar-20-2020
					Mar-27-2020 Made changes to the code to Handle Performance(Bringing down the execution time)
					Aug-05-2020 Modified Code for Performance reasons
					Aug-05-2020 Replaced View Code with Tables(Movement, Movement Source, Movement Destination)
-- <Description>:	This Procedure is used to get the Pending Transfer Movements based on the input of a Ticket. </Description>
-- =============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetPendingTransferMovement] 
(
	@TicketId INT
)
AS 
  BEGIN 

	IF OBJECT_ID('tempdb..#TempGetPendingTransferMovement')IS NOT NULL
	DROP TABLE #TempGetPendingTransferMovement

	IF OBJECT_ID('tempdb..#TempNodeConnection')IS NOT NULL
	DROP TABLE #TempNodeConnection

	DECLARE  @TicketStartDate	DATE
			,@TicketEndDate		DATE
			,@CategoryElementId INT

	SELECT   @TicketStartDate	= StartDate
			,@TicketEndDate		= EndDate
			,@CategoryElementId = CategoryElementId
	FROM   [Admin].[Ticket] Tic
	WHERE Tic.TicketId = @TicketId 
	AND [Tic].[TicketTypeId]=2

	SELECT NC.SourceNodeId								AS SourceNodeId,
		   NC.DestinationNodeId							AS DestinationNodeId,
		   NC.AlgorithmId								AS AlgorithmId,
		   [CESN].[Name]								AS SourceNodeType,
		   [CESN].[ElementId]							AS SourceNodeTypeId,
		   [CEDN].[Name]								AS DestinationNodeType,
		   [CEDN].[ElementId]							AS DestinationNodeTypeId
	INTO #TempNodeConnection
	FROM  [Admin].[NodeTag] NT	
	INNER JOIN [Admin].[NodeConnection] NC 
	ON [NT].[NodeId] = [NC].[DestinationNodeId]
	AND [NC].[AlgorithmId] IS NOT NULL
	AND [NC].[IsTransfer]=1
	AND [NT].[ElementId]=@CategoryElementId
	INNER JOIN Admin.NodeTag NTS
	ON [NTS].[NodeId]=[NC].[SourceNodeId]
	INNER JOIN [Admin].[CategoryElement] CESN
	ON [CESN].[ElementId] = [NTS].[ElementId]
	AND [CESN].[CategoryId]=1
	INNER JOIN Admin.NodeTag NTD
	ON [NTD].[NodeId]=[NC].[DestinationNodeId]
	INNER JOIN [Admin].[CategoryElement] CEDN
	ON [CEDN].[ElementId] = [NTD].[ElementId]
	AND [CEDN].[CategoryId]=1
	WHERE NC.IsDeleted = 0



	SELECT  Mov.[MovementTransactionId]				    AS MovementTransactionId, 
			Mov.[OperationalDate]					    AS OperationalDate, 
			MovSrc.[SourceNodeId]						AS SourceNodeId, 
			SrcNd.[Name]								AS SourceNode,
			MovDest.[DestinationNodeId]					AS DestinationNodeId, 
			DesNd.[Name]								AS DestinationNode,
			MovSrc.[SourceProductId]					AS SourceProductId, 
			SrcPrd.[Name]								AS SourceProduct, 
			MovSrc.[SourceProductTypeId]				AS SourceProductTypeId, 
			[CESP].[Name]								AS SourceProductType,
			MovDest.[DestinationProductId]				AS DestinationProductId, 
			DestPrd.[Name]								AS DestinationProduct, 
			MovDest.[DestinationProductTypeId]			AS DestinationProductTypeId,
			[CEDP].[Name]								AS DestinationProductType,
			Mov.[NetStandardVolume]						AS NetVolume, 
			Mov.[MeasurementUnit]						AS MeasurementUnit, 
			Mov.[MovementId]							AS MovementId,
			Mov.[MovementTypeId]						AS MovementTypeId,  
			MoveType.[Name]								AS MovementType,
			[NC].[AlgorithmId]							AS AlgorithmId,
			NC.SourceNodeType,
			NC.SourceNodeTypeId,
			NC.DestinationNodeType,
			NC.DestinationNodeTypeId
		   ,ROW_NUMBER() OVER( PARTITION BY Mov.[MovementId] 
							   ORDER     BY Mov.[MovementTransactionId] DESC)AS Rnum 
	INTO #TempGetPendingTransferMovement
	FROM #TempNodeConnection NC	
	INNER JOIN Offchain.MovementSource MovSrc
	ON  MovSrc.[SourceNodeId]=NC.[SourceNodeId]
	INNER JOIN Offchain.MovementDestination MovDest
	ON  MovDest.[DestinationNodeId]=[NC].[DestinationNodeId] 
	INNER JOIN Offchain.Movement Mov
	ON  Mov.MovementTransactionId = MovSrc.MovementTransactionId
	AND Mov.MovementTransactionId = MovDest.MovementTransactionId
	AND Mov.[OperationalDate]  BETWEEN @TicketStartDate AND @TicketEndDate
	AND Mov.[EventType] IN ('Insert','Update')
	INNER JOIN [Admin].[Node] DesNd
	ON DesNd.NodeId = MovDest.DestinationNodeId 
	INNER JOIN [Admin].Product DestPrd
	ON DestPrd.ProductId = MovDest.DestinationProductId
	INNER JOIN [Admin].[Node] SrcNd
	ON SrcNd.NodeId = MovSrc.SourceNodeId 
	INNER JOIN [Admin].Product SrcPrd
	ON SrcPrd.ProductId = MovSrc.SourceProductId
	LEFT JOIN [Admin].[CategoryElement] CESP 
	ON [CESP].[ElementId]  = MovSrc.[SourceProductTypeId]
	LEFT JOIN [Admin].[CategoryElement] CEDP 
	ON [CEDP].[ElementId]= MovDest.[DestinationProductTypeId] 	
	LEFT JOIN [Admin].CategoryElement MoveType
	ON MoveType.ElementId = Mov.MovementTypeId
	AND MoveType.CategoryId = 9 --MovementType		

	SELECT  AlgorithmId,
			MovementType,
			SourceNode,
			SourceNodeType,
			DestinationNode,
			DestinationNodeType,
			SourceProduct,
			SourceProductType,
			@TicketId AS TicketId,
			MovementId,
			NetVolume,
			MovementTypeId,
			SourceNodeId,
			SourceNodeTypeId,
			DestinationNodeId,
			DestinationNodeTypeId,
			SourceProductId,
			SourceProductTypeId,
			OperationalDate,
			MeasurementUnit,
			MovementTransactionId,
			@TicketStartDate	AS StartDate,
			@TicketEndDate		AS EndDate
	FROM   #TempGetPendingTransferMovement 
	WHERE  Rnum = 1  
	AND NetVolume > 0 
  END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get the Pending Transfer Movements based on the input of a Ticket.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetPendingTransferMovement',
    @level2type = NULL,
    @level2name = NULL