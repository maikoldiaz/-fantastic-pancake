/*-- ============================================================================================================================
-- Author:				Microsoft
-- Created Date: 		Jan-06-2020
-- Updated Date1:		Jan-08-2020, Added new columns.
-- Modification Date:	Mar-30-2020, Added JOIN with table Admin.Contract and fetched movement id, document number and position.
-- Modification Date:	Apr-01-2020, Added MovementTypeId.
--						Apr-09-2020  -- Removed(BlockchainStatus = 1)   
--						Jun-15-2020 Added logic for MovemetType ('Compra','Venta','ACE Entrada','ACE Salida')
--						Sep-23-2021 Add IsDeleted movement validation 
-- <Description>:		This Procedure is used to get Movement and Inventory details for given Ownership Node. </Description>
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_OwnershipNodeMovementInventoryDetails]
(
       @OwnershipNodeID	INT
)
AS
BEGIN
 DECLARE	 @OwnershipTicketId INT = 0
				,@NodeID			INT = 0
				,@StartDate			DATE
				,@EndDate			DATE

		SELECT  @OwnershipTicketId = TicketId,
				@NodeID			   = NodeId
		FROM [Admin].[OwnershipNode] OwnNode
		WHERE OwnNode.OwnershipNodeId = @OwnershipNodeID

		SELECT  @StartDate = DATEADD(DD,-1,Tic.StartDate), 
				@EndDate = Tic.EndDate
		FROM [Admin].[Ticket] Tic
		WHERE Tic.TicketId = @OwnershipTicketId

		SELECT DISTINCT 

			  CASE  WHEN TempMov.MessageTypeId NOT IN (1,2) AND TempMov.VariableTypeId = 1 THEN 1	--> 1 = Interfase
					WHEN TempMov.MessageTypeId NOT IN (1,2) AND TempMov.VariableTypeId = 2 THEN 2	--> 2 = Tolerancia
					WHEN TempMov.MessageTypeId NOT IN (1,2) AND TempMov.VariableTypeId = 3 THEN 3	--> 3 = Pérdida No Identificada

                    WHEN (TempMov.MessageTypeId  = 1  OR (TempMov.MessageTypeId  = 3 AND TempMov.VariableTypeId IS NULL)) AND TempMov.SourceNodeId =	  @NodeID THEN 6 --> 6 = Salida  
		            WHEN (TempMov.MessageTypeId  = 1  OR (TempMov.MessageTypeId  = 3 AND TempMov.VariableTypeId IS NULL)) AND TempMov.DestinationNodeId = @NodeID THEN 5 --> 5 = Entrada  

					WHEN TempMov.MessageTypeId  = 2 THEN 7	--> 7 = Pérdida Identificada
					ELSE NULL END				AS VariableTypeId
			  ,TempMov.MovementId				AS MovementId
			  ,CASE WHEN TempMov.MovementTypeName
						IN ('Compra','Venta','ACE Entrada','ACE Salida')
					THEN MovSrcConn.MovementId	END AS SourceMovementId --for all other its NUll
			  ,TempMov.MovementTypeId			AS MovementTypeId
			  ,TempMov.MovementTypeName			AS MovementType
			  ,TempMov.MovementTransactionId	AS TransactionId
			  ,TempMov.OperationalDate			AS OperationalDate
			  ,NULL								AS TankName
			  ,TempMov.SourceNodeId				AS SourceNodeId
			  ,TempMov.SourceNodeName			AS SourceNode 
			  ,TempMov.DestinationNodeId		AS DestinationNodeId
			  ,TempMov.DestinationNodeName		AS DestinationNode
			  ,TempMov.SourceProductId			AS SourceProductId
			  ,TempMov.SourceProductName		AS SourceProduct
			  ,TempMov.DestinationProductId		AS DestinationProductId
			  ,TempMov.DestinationProductName	AS DestinationProduct
			  ,TempMov.NetStandardVolume		AS NetVolume
			  ,MUnit.Name						AS Unit
			  ,MUnit.ElementId					AS UnitId
			  ,OwnShip.OwnerId					AS OwnerId
			  ,OwnName.[Name]					AS OwnerName
			  ,OwnShip.AppliedRule				AS OwnershipFunction
			  ,OwnShip.OwnershipVolume			AS OwnershipVolume
			  ,OwnShip.OwnershipPercentage		AS OwnershipPercentage
			  ,TempMov.ReasonId					AS ReasonId
			  ,CE.[Name]						AS Reason
			  ,TempMov.Comment					AS Comment
			  ,1								AS IsMovement
			  ,OwnName.Color					AS [Color]
			  ,MovCont.MovementContractId		AS MovementContractId
			  ,MovCont.ContractId				AS ContractId
			  ,MovCont.DocumentNumber			AS [DocumentNumber]
			  ,MovCont.Position					AS [Position]
		FROM [Admin].[view_MovementInformation] TempMov
		INNER JOIN Admin.CategoryElement MUnit
		ON MUnit.ElementId = TempMov.MeasurementUnit
		AND MUnit.CategoryId = 6 --Unit of measurement
		INNER JOIN [Offchain].[Ownership] OwnShip
		ON OwnShip.MovementTransactionId = TempMov.MovementTransactionId
		INNER JOIN Admin.CategoryElement OwnName
		ON OwnName.[ElementId] = OwnShip.[OwnerId]
		AND OwnName.CategoryId = 7
		LEFT JOIN Admin.CategoryElement CE
		ON CE.ElementId = TempMov.ReasonId
	    LEFT JOIN [Admin].[MovementContract] MovCont
		ON MovCont.MovementContractId = TempMov.ContractId
		LEFT JOIN Offchain.Movement MovSrcConn
		ON MovSrcConn.MovementTransactionId = TempMov.SourceMovementId
		WHERE TempMov.OwnershipTicketId = @OwnershipTicketId
		AND (TempMov.SourceNodeId  = @NodeID OR TempMov.DestinationNodeId = @NodeID)
		AND OwnShip.IsDeleted = 0 AND TempMov.IsDeleted = 0 

		UNION

		SELECT DISTINCT 

			   CASE  WHEN CAST(Inv.InventoryDate AS DATE) = @StartDate  THEN 4	--> 4 = Inventario Inicial
					 WHEN CAST(Inv.InventoryDate AS DATE) = @EndDate	THEN 8	--> 8 = Inventario Final
					 END						AS VariableTypeId
			   ,NULL							AS MovementId
			   ,NULL							AS SourceMovementId
			   ,NULL							AS MovementTypeId
			   ,NULL							AS MovementType
			   ,Inv.InventoryTransactionId		AS TransactionId
			   ,Inv.InventoryDate				AS OperationalDate
			   ,Inv.TankName					AS TankName
			   ,Inv.NodeId						AS SourceNodeId
			   ,Inv.NodeName					AS SourceNode 
			   ,NULL							AS DestinationNodeId
			   ,NULL							AS DestinationNode
			   ,Inv.ProductId					AS SourceProductId
			   ,Inv.ProductName					AS SourceProduct
			   ,NULL							AS DestinationProductId
			   ,NULL							AS DestinationProduct
			   ,Inv.ProductVolume				AS NetVolume
			   ,MUnit.Name						AS Unit
			   ,MUnit.ElementId					AS UnitId
			   ,OwnShip.OwnerId					AS OwnerId
			   ,OwnName.[Name]					AS OwnerName
			   ,OwnShip.AppliedRule				AS OwnershipFunction
			   ,OwnShip.OwnershipVolume			AS OwnershipVolume
			   ,OwnShip.OwnershipPercentage		AS OwnershipPercentage
			   ,Inv.ReasonId					AS ReasonId
			   ,CE.[Name]						AS Reason
			   ,Inv.Comment						AS Comment
			   ,0								AS IsMovement
			   ,OwnName.Color					AS [Color]
			   ,NULL							AS MovementContractId
			   ,NULL							AS ContractId
			   ,NULL							AS [DocumentNumber]
			   ,NULL							AS [Position]
		FROM [Admin].[view_InventoryInformation] Inv
		INNER JOIN [Offchain].[Ownership] OwnShip
		ON OwnShip.InventoryProductId = Inv.InventoryProductId
		INNER JOIN Admin.CategoryElement MUnit
		ON MUnit.ElementId = Inv.MeasurementUnit
		AND MUnit.CategoryId = 6 --Unit of measurement
		INNER JOIN Admin.CategoryElement OwnName
		ON OwnName.[ElementId] = OwnShip.[OwnerId]
		AND OwnName.CategoryId = 7
		LEFT JOIN Admin.CategoryElement CE
		ON CE.ElementId = Inv.ReasonId
		WHERE Inv.NodeId = @NodeID
		AND (CAST(Inv.InventoryDate AS DATE) = @StartDate OR (CAST(Inv.InventoryDate AS DATE) = @EndDate AND Inv.OwnershipTicketId = @OwnershipTicketId))
		AND OwnShip.IsDeleted = 0
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get Movement and Inventory details for given Ownership Node.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_OwnershipNodeMovementInventoryDetails',
    @level2type = NULL,
    @level2name = NULL