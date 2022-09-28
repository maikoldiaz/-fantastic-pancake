/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jun-12-2020
-- Modified Date:	Jun-18-2020
-- <Description>:	This Procedure is used to get the Delta Inventores Data based on the input of SegmentId, Start Date, End Date</Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetDeltaErrorDetailsForMovAndInventories]
(
	   @TicketId				INT	   
)
AS
BEGIN

			 SELECT   Mov.MovementID					AS Identifier
					 ,'Movimiento'						AS Type 
					 ,Mov.SourceNodeName				AS SourceNode
					 ,Mov.DestinationNodeName			AS DestinationNode
					 ,Mov.SourceProductName				AS SourceProduct
					 ,Mov.DestinationProductName		AS DestinationProduct
					 ,Mov.NetStandardVolume				AS Quantity
					 ,CE.Name							AS Unit
					 ,Mov.OperationalDate				AS [Date]
					 ,Delta.ErrorMessage				AS Error
			 FROM [Admin].[DeltaError] Delta
			 INNER JOIN Admin.view_MovementInformation Mov
			 ON Delta.MovementTransactionId = Mov.MovementTransactionId
			 INNER JOIN Admin.CategoryElement CE
			 ON CE.ElementId = Mov.MeasurementUnit
			 WHERE Delta.TicketId = @TicketId
			 UNION ALL
			 SELECT   InvInfo.InventoryId				AS Identifier
					 ,'Inventario'						AS Type 
					 ,InvInfo.NodeName					AS SourceNode--Inventory Node Should be that shown as Source Node or destinationNode ?
					 ,NULL								AS DestinationNode
					 ,InvInfo.ProductName				AS SourceProduct--Inventory Product Should be that shown as Source Product or Destination ?
					 ,NULL								AS DestinationProduct
					 ,InvInfo.ProductVolume				AS Quantity
					 ,CE.Name							AS Unit
					 ,InvInfo.InventoryDate				AS [Date]
					 ,Delta.ErrorMessage				AS Error
			 FROM [Admin].[DeltaError] Delta
			 INNER JOIN Admin.view_InventoryInformation InvInfo
			 ON Delta.InventoryProductId = InvInfo.InventoryProductId
			 INNER JOIN Admin.CategoryElement CE
			 ON CE.ElementId = InvInfo.MeasurementUnit
			 WHERE Delta.TicketId = @TicketId
END
GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Delta Inventores Data based on the input of SegmentId, Start Date, End Date',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetDeltaErrorDetailsForMovAndInventories',
							@level2type = NULL,
							@level2name = NULL