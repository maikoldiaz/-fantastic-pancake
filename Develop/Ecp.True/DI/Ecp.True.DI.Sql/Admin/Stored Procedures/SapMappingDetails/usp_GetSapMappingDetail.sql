/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	JUL-07-2020
-- Update date:     Jul-13-2020 -> Removed execution id
--					Sep-07-2020 -> Updated Category id as 22 for Systems - "Sistema origen", "Sistema Destino" and "Sistema oficial"  
-- Description:     This Procedure is to Get details of entities from SapMapping table.
-- EXEC [Admin].[usp_GetSapMappingDetail] '49CA1512-8ACD-4105-9271-01648C1155CC'
   SELECT * FROM [Admin].[SapMappingDetail] WHERE  ExecutionId = '49CA1512-8ACD-4105-9271-01648C1155CC'
-- ==============================================================================================================================*/
CREATE PROCEDURE admin.usp_GetSapMappingDetail
 
AS
DECLARE @Previousdate  DATETIME =  [Admin].[udf_GetTrueDate] ()-1
/* TRUNK AND RELOAD THE TABLE */

DELETE FROM [Admin].[SapMappingDetail] 


/* Inserting into SapMappingDetail table */
INSERT INTO [Admin].[SapMappingDetail]
(
 [Sistema origen]			
,[Tipo de movimiento_o]		
,[Producto_o]					
,[Nodo origen_o]			    
,[Nodo destino_o]		    
,[Sistema destino]				
,[Tipo de movimiento_d]		
,[Producto_d]				
,[Nodo origen_d]		        
,[Nodo destino_d]	        
,[Sistema oficial]			
						
--Internal Common Columns              
,[CreatedBy]                			
)


SELECT  DISTINCT
		CASE WHEN SrcSys.elementid IS NULL THEN CAST(sap.SourceSystemid AS NVARCHAR(150)) ELSE SrcSys.Name END AS SourceSystemName,
		CASE WHEN SrcMov.elementid IS NULL THEN CAST(SourceMovementTypeId AS NVARCHAR(150)) ELSE SrcMov.Name END AS SourceMovementTypeName,
		CASE WHEN SrcPrd.productid IS NULL THEN CAST(SourceProductId AS NVARCHAR(150)) ELSE SrcPrd.Name END AS  SourceProductName,
		CASE WHEN SrcSysSrcNode.Nodeid IS NULL THEN CAST(SourceSystemSourceNodeId AS NVARCHAR(150)) ELSE SrcSysSrcNode.Name END AS  SourceSystemSourceNodeName,
		CASE WHEN SrcSysDstNode.Nodeid IS NULL THEN CAST(SourceSystemDestinationNodeId AS NVARCHAR(150)) ELSE SrcSysDstNode.Name END AS SourceSystemDestinationNodeName,
		CASE WHEN DstSys.elementid IS NULL THEN CAST(DestinationSystemId AS NVARCHAR(150)) ELSE DstSys.Name END AS DestinationSystemName,
		CASE WHEN DstMov.elementid IS NULL THEN CAST(DestinationMovementTypeId AS NVARCHAR(150)) ELSE DstMov.Name END AS DestinationMovementTypeName,
		CASE WHEN DstPrd.productid IS NULL then CAST(DestinationProductId AS NVARCHAR(150)) else DstPrd.Name end as  DestinationProductName,
		CASE WHEN DstSysSrcNode.Nodeid IS NULL THEN  CAST(DestinationSystemSourceNodeId AS NVARCHAR(150)) ELSE DstSysSrcNode.Name END AS DestinationSystemSourceNodeName,
		CASE WHEN DstSysDstNode.Nodeid IS NULL THEN CAST(DestinationSystemDestinationNodeId AS NVARCHAR(150)) ELSE DstSysDstNode.Name END AS DestinationSystemDestinationNodeName,
		CASE WHEN OffSys.elementid IS NULL THEN CAST(OfficialSystem  AS NVARCHAR(150)) ELSE OffSys.Name END AS OfficialSystemName,
		'ReportUser' as [CreatedBy]

FROM admin.SapMapping Sap

--SOURCE SYSTEM 
LEFT JOIN admin.CategoryElement SrcSys on Sap.SourceSystemid=SrcSys.elementid and SrcSys.categoryid=22 -- Changed from 8 to 22 as per mentioned in bug 77181
LEFT JOIN admin.CategoryElement SrcMov on Sap.SourceMovementTypeId=SrcMov.elementid and SrcMov.categoryid=9 -- condition for MovementType
LEFT JOIN admin.product SrcPrd on sap.SourceProductId=SrcPrd.productid
LEFT JOIN admin.node SrcSysSrcNode on sap.SourceSystemSourceNodeId=SrcSysSrcNode.Nodeid
LEFT JOIN admin.node SrcSysDstNode on sap.SourceSystemDestinationNodeId=SrcSysDstNode.Nodeid
-- DESTINATION SYSTEM
LEFT JOIN admin.CategoryElement DstSys on Sap.DestinationSystemId=DstSys.elementid and DstSys.categoryid=22 -- Changed from 8 to 22 as per mentioned in bug 77181
LEFT JOIN admin.CategoryElement DstMov on Sap.DestinationMovementTypeId=DstMov.elementid and DstMov.CategoryId=9 -- condition for MovementType
LEFT JOIN  admin.product DstPrd on sap.DestinationProductId=DstPrd.productid
LEFT JOIN admin.node DstSysSrcNode on sap.DestinationSystemSourceNodeId=DstSysSrcNode.Nodeid
LEFT JOIN admin.node DstSysDstNode on sap.DestinationSystemDestinationNodeId=DstSysDstNode.Nodeid
-- OFFICIAL SYSTEM 
LEFT JOIN admin.CategoryElement OffSys on Sap.OfficialSystem=OffSys.elementid and OffSys.categoryid=22 -- Changed from 8 to 22 as per mentioned in bug 77181


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get details of entities from SapMapping table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetSapMappingDetail',
    @level2type = NULL,
    @level2name = NULL