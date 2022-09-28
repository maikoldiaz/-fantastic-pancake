/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
This file contains SQL statements that will be appended to the build script.		
Use SQLCMD syntax to include a file in the post-deployment script.			
Example:      :r .\myfile.sql								
Use SQLCMD syntax to reference a variable in the post-deployment script.		
Example:      :setvar TableName MyTable							
            SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


select fr.FileRegistrationId, frt.FileRegistrationTransactionId,cast(m.InventoryId as nvarchar(50)) InventoryId
,m.InventoryDate
,SrcSystem.Name  SrcSystemName, m.NodeId,n.[Name] NodeName, p.[Name] ProductName,catunid.Name MeasurementUnitDesc
,Ptype.[Name] ProductTypeName
,m.ProductVolume,m.GrossStandardQuantity
, fr.UploadId, fr.UploadDate,m.TransactionHash,m.BlockchainInventoryProductTransactionId
,m.CreatedDate
,ROW_NUMBER() OVER(PARTITION BY m.InventoryId,m.productId ORDER BY m.CreatedDate ASC) as Row#
,m.EventType,m.ScenarioId
from admin.FileRegistration fr
left join admin.FileRegistrationTransaction frt on frt.fileregistrationid = fr.FileRegistrationId 
left join Offchain.InventoryProduct m on m.FileRegistrationTransactionId = frt.FileRegistrationTransactionId
left join admin.CategoryElement SrcSystem on SrcSystem.ElementId = m.SourceSystemId
left join admin.[Node] n on n.NodeId = m.NodeId
left join admin.Product p on p.ProductId = m.ProductId
left join admin.CategoryElement catunid on catunid.elementId = m.MeasurementUnit
left join admin.CategoryElement PType on Ptype.ElementId = m.ProductType
where cast(UploadDate as date) >= '20220317' order by InventoryId desc

