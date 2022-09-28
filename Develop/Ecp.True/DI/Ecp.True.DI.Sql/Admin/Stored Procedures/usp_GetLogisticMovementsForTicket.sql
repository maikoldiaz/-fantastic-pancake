/*-- =============================================================================================================================================================================
-- Author:          Intergrupo
-- Created Date:	Mar-25-2021
-- Updated Date:	May-05-2021 add logistic movement id column to query return.
-- Updated Date:	Jul-21-2021 change the DestinationStorage, SourceStorage, SourceProduct, DestinationProduct, CostCenter query.
-- Updated Date:	Jul-26-2021 add contract validation.
-- Updated Date:	Jul-29-2021 change gets documentNumber and Position for sales and purchases.
-- Updated Date:	Aug-02-2021 change netStandarVolumen by ownershipVolumen.
-- Updated Date:	Ago-30-2022 change MovementId so that when have ConcatMovementId change it.
-- <Description>:	This procedure shows the logistical movements of a ticket. </Description>
-- ==============================================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetLogisticMovementsForTicket] 
(
	@TicketId INT
)
AS 
  BEGIN 
		IF OBJECT_ID('tempdb..#TempStorageLocationProduct')IS NOT NULL
			DROP TABLE #TempStorageLocationProduct

		IF OBJECT_ID('tempdb..#TempStorage')IS NOT NULL
			DROP TABLE #TempStorage 


		SELECT  
			 N.LogisticCenterId
			,MIN (SL.Name) AS SapStorage
			,p.ProductId AS ProductId
			,n.NodeId
			INTO #TempStorage
			FROM Admin.Node N
			LEFT JOIN [Offchain].[LogisticMovement] AS LM 
			on LM.DestinationLogisticCenterId = N.LogisticCenterId OR LM.SourceLogisticCenterId = N.LogisticCenterId
			LEFT JOIN Admin.NodeStorageLocation NLS ON NLS.NodeId = N.NodeId
			LEFT JOIN Admin.StorageLocation SL ON SL.LogisticCenterId = N.LogisticCenterId
			LEFT JOIN admin.StorageLocationProduct SLP ON SLP.NodeStorageLocationId = NLS.NodeStorageLocationId
			LEFT JOIN admin.Product P ON P.ProductId = SLP.ProductId
			WHERE LM.TicketId = @TicketId 
			AND P.ProductId IN(LM.SourceProductId ,LM.DestinationProductId)
			AND N.NodeId  IN(LM.SourceLogisticNodeId, LM.DestinationLogisticNodeId) 
			AND N.LogisticCenterId IN (LM.SourceLogisticCenterId, LM.DestinationLogisticCenterId)
			GROUP BY  N.LogisticCenterId ,P.ProductId ,N.NodeId

			

		--Se obtiene el almacén origen, almacén destino, producto origen, prodcuto destino y centro de costo
		SELECT LM.LogisticMovementId,
		LM.LogisticMovementTypeId,
		LM.MovementTransactionId,
		SSL.SapStorage SourceStorage,
		DSL.SapStorage DestinationStorage,
		[CE].[Name]  CostCenter,
		[SP].[Name] SourceProduct, 
		[DP].[Name] DestinationProduct
		INTO #TempStorageLocationProduct
		FROM [Offchain].[LogisticMovement] AS LM
		LEFT JOIN [Offchain].[MovementSource] AS MS
		ON [LM].[MovementTransactionId] = [MS].[MovementTransactionId]
		LEFT JOIN [Offchain].[MovementDestination] AS MD
		ON [LM].[MovementTransactionId] = [MD].[MovementTransactionId]
		LEFT JOIN [Admin].[Node] AS SN
		ON [MS].[SourceNodeId] = [SN].[NodeId]
		LEFT JOIN [Admin].[Node] AS DN
		ON [MD].[DestinationNodeId] = [DN].[NodeId]
		LEFT JOIN [Admin].[CategoryElement] AS CE
		ON [LM].CostCenterId = [CE].[ElementId]
		LEFT JOIN [Admin].[Product] AS SP
		ON [LM].[SourceProductId] = [SP].[ProductId]
		LEFT JOIN [Admin].[Product] AS DP
		ON [LM].[DestinationProductId] = [DP].[ProductId]
		LEFT JOIN #TempStorage AS SSL
		on SSL.LogisticCenterId = LM.SourceLogisticCenterId AND SSL.NodeId = LM.SourceLogisticNodeId AND SSL.ProductId = LM.SourceProductId
		LEFT JOIN #TempStorage AS DSL
		on DSL.LogisticCenterId = LM.DestinationLogisticCenterId AND DSL.NodeId = LM.DestinationLogisticNodeId AND DSL.ProductId = LM.DestinationProductId
		WHERE [LM].[TicketId] = @TicketId

		--Obtiene movimientos logisticos por ticketId
		SELECT 
			[LM].[MovementTransactionId] AS 'movementTransactionId',
			[ST].[StatusType] AS 'state',
			[LM].[MessageProcess] AS 'description',
			[MT].[Name] AS 'movementType',
			[SLC].[Name] AS 'sourceCenter',
			SUBSTRING(TEMP.SourceStorage,CHARINDEX(':',TEMP.SourceStorage)+1,LEN(TEMP.SourceStorage)) AS 'sourceStorage',
			TEMP.SourceProduct AS 'sourceProduct',
			[DLC].[Name] AS 'destinationCenter',
			SUBSTRING(TEMP.DestinationStorage,CHARINDEX(':',TEMP.DestinationStorage)+1,LEN(TEMP.DestinationStorage)) AS 'destinationStorage',
			TEMP.DestinationProduct AS 'destinationProduct',
			[LM].[OwnershipVolume] AS 'ownershipVolume',
			[MU].[Name] AS 'units',
			[M].[OperationalDate] AS 'operationalDate',
			(CASE WHEN LM.ConcatMovementId IS NULL THEN M.MovementId
			WHEN LM.ConcatMovementId IS NOT NULL THEN LM.ConcatMovementId END) AS 'movementId',
			TEMP.CostCenter AS 'costCenter', 
			[LM].[SapTransactionCode] AS 'gmCode', 
			[LM].[DocumentNumber] AS 'documentNumber',
			[LM].[Position] AS 'position',
			[LM].[MovementOrder] AS 'order',
			[LM].[SapSentDate] AS 'accountingDate',
			[SEG].[Name] AS 'segment',
			[SCE].[Name] AS 'scenario',
			[OWN].[Name] AS 'owner',
			[LM].[LogisticMovementId] AS 'logisticMovementId'
		FROM [Offchain].[LogisticMovement] AS LM
		INNER JOIN [Admin].[Ticket] AS T
		ON [LM].[TicketId] = [T].[TicketId]
		LEFT JOIN [Offchain].[Movement] AS M
		ON [LM].[MovementTransactionId] = [M].[MovementTransactionId]
		LEFT JOIN [Admin].[StatusType] ST
		ON [LM].[StatusProcessId] = [ST].[StatusTypeId]
		LEFT JOIN [Admin].[CategoryElement] AS MT
		ON [LM].[LogisticMovementTypeId] = [MT].[ElementId]
		LEFT JOIN [Admin].[LogisticCenter] AS SLC
		ON [LM].[SourceLogisticCenterId] = [SLC].[LogisticCenterId]
		LEFT JOIN [Admin].[LogisticCenter] AS DLC
		ON [LM].[DestinationLogisticCenterId] = [DLC].[LogisticCenterId]
		LEFT JOIN [Admin].[CategoryElement] AS MU
		ON [LM].[MeasurementUnit] = [MU].[ElementId]
		INNER JOIN [Admin].[CategoryElement] AS SEG
		ON [M].[SegmentId] = [SEG].[ElementId]
		INNER JOIN [Admin].[ScenarioType] AS SCE
		ON [M].[ScenarioId] = [SCE].[ScenarioTypeId]
		INNER JOIN [Admin].[CategoryElement] AS OWN
		ON [T].[OwnerId] = [OWN].[ElementId]
		INNER JOIN #TempStorageLocationProduct AS TEMP
		ON TEMP.LogisticMovementId = LM.LogisticMovementId
		WHERE [LM].[TicketId] = @TicketId
  END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description',
	@value=N'This procedure shows the logistical movements of a ticket',
	@level0type=N'SCHEMA',
	@level0name=N'Admin',
	@level1type=N'PROCEDURE',
	@level1name=N'usp_GetLogisticMovementsForTicket'
GO