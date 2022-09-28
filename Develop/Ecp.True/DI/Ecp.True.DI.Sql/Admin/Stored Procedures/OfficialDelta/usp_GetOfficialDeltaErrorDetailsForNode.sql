/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jul-09-2020
					Jul-14-2020 Added Logic to Fetch the Data From [ConsolidatedMovement], [ConsolidatedInventoryProduct]
-- <Description>:	This Procedure is used to get the Delta Node Errors Data based on the input of DeltaNodeId</Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetOfficialDeltaErrorDetailsForNode]
(
	   @DeltaNodeId				INT	   
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
			 FROM [Admin].[DeltaNodeError] Delta
			 INNER JOIN Admin.view_MovementInformation Mov
			 ON Delta.MovementTransactionId = Mov.MovementTransactionId
			 INNER JOIN Admin.CategoryElement CE
			 ON CE.ElementId = Mov.MeasurementUnit
			 WHERE Delta.DeltaNodeId = @DeltaNodeId
			 UNION 
			 SELECT   InvInfo.InventoryId				AS Identifier
					 ,'Inventario'						AS Type 
					 ,InvInfo.NodeName					AS SourceNode
					 ,NULL								AS DestinationNode
					 ,InvInfo.ProductName				AS SourceProduct
					 ,NULL								AS DestinationProduct
					 ,InvInfo.ProductVolume				AS Quantity
					 ,CE.Name							AS Unit
					 ,InvInfo.InventoryDate				AS [Date]
					 ,Delta.ErrorMessage				AS Error
			 FROM [Admin].[DeltaNodeError] Delta
			 INNER JOIN Admin.view_InventoryInformation InvInfo
			 ON Delta.InventoryProductId = InvInfo.InventoryProductId
			 INNER JOIN Admin.CategoryElement CE
			 ON CE.ElementId = InvInfo.MeasurementUnit
			 WHERE Delta.DeltaNodeId = @DeltaNodeId
			 UNION 
			 SELECT   cast(ConMov.ConsolidatedMovementId AS VARCHAR(50))			AS Identifier--Srini to Confirm on this Field Source
					 ,'Movimiento'													AS Type 
					 ,[Admin].[udf_GetNodeName](ConMov.SourceNodeId)				AS SourceNode
					 ,[Admin].[udf_GetNodeName](ConMov.DestinationNodeId)			AS DestinationNode
					 ,[Admin].[udf_GetProductName](ConMov.SourceProductId)			AS SourceProduct
					 ,[Admin].[udf_GetProductName](ConMov.DestinationProductId)		AS DestinationProduct
					 ,ConMov.NetStandardVolume										AS Quantity
					 ,CE.Name														AS Unit
					 ,ConMov.ExecutionDate											AS [Date]--Srini to Confirm on this Field Source
					 ,Delta.ErrorMessage											AS Error
			 FROM [Admin].[DeltaNodeError] Delta
			 INNER JOIN Admin.[ConsolidatedMovement] ConMov
			 ON Delta.[ConsolidatedMovementId] = ConMov.[ConsolidatedMovementId]
			 INNER JOIN Admin.CategoryElement CE
			 ON CE.ElementId = ConMov.MeasurementUnit
			 WHERE Delta.DeltaNodeId = @DeltaNodeId
			 UNION 
			 SELECT   cast(ConInv.ConsolidatedInventoryProductId AS VARCHAR(50))	AS Identifier--Srini to Confirm on this Field Source
					 ,'Inventario'													AS Type 
					 ,[Admin].[udf_GetNodeName](ConInv.NodeId)						AS SourceNode
					 ,NULL															AS DestinationNode
					 ,[Admin].[udf_GetProductName](ConInv.ProductId)				AS SourceProduct
					 ,NULL															AS DestinationProduct
					 ,ConInv.ProductVolume											AS Quantity
					 ,CE.Name														AS Unit
					 ,ConInv.ExecutionDate											AS [Date]--Srini to Confirm on this Field Source
					 ,Delta.ErrorMessage											AS Error
			 FROM [Admin].[DeltaNodeError] Delta
			 INNER JOIN Admin.[ConsolidatedInventoryProduct] ConInv
			 ON Delta.[ConsolidatedInventoryProductId] = ConInv.[ConsolidatedInventoryProductId]
			 INNER JOIN Admin.CategoryElement CE
			 ON CE.ElementId = ConInv.MeasurementUnit
			 WHERE Delta.DeltaNodeId = @DeltaNodeId
END
GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Delta Node Errors Data based on the input of DeltaNodeId',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetOfficialDeltaErrorDetailsForNode',
							@level2type = NULL,
							@level2name = NULL