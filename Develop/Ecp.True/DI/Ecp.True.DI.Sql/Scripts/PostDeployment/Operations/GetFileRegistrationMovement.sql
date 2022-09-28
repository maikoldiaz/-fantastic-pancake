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


select fr.FileRegistrationId, frt.FileRegistrationTransactionId
,cast(m.MovementId as nvarchar(50)) MovementId
,MovSrc.SourceNodeName,MovDest.DestinationNodeName,SourceProductName,DestinationProductName,m.NetStandardVolume,m.GrossStandardVolume
,m.EventType,m.MeasurementUnit,catunid.Name MeasurementUnitDesc,m.MovementTypeId,MType.Name MovementTypeDesc
,MovSrc.SourceProductTypeId, PSrcType.name SourceProductTypeName
,m.SourceSystemId,SrcSystem.Name SourceSystemName
,m.[Classification]
,MovDest.DestinationProductTypeId, PDestType.Name DestinationProductTypeName
,fr.UploadId, fr.UploadDate,m.TransactionHash,m.BlockchainMovementTransactionId
,mp.StartTime [Fecha Inicial], mp.EndTime [Fecha Final]
,m.ScenarioId as Scenario
,m.CreatedDate as FechaCreacion
,ROW_NUMBER() OVER(PARTITION BY m.MovementId ORDER BY m.CreatedDate ASC) as Row#
from admin.FileRegistration fr
left join admin.FileRegistrationTransaction frt on frt.fileregistrationid = fr.FileRegistrationId 
left join Offchain.Movement m on m.FileRegistrationTransactionId = frt.FileRegistrationTransactionId
LEFT JOIN (SELECT  MovDest.MovementTransactionId
				  ,MovDest.DestinationNodeId
				  ,DesNd.[Name] AS DestinationNodeName
				  ,MovDest.DestinationProductId		
				  ,DestPrd.[Name] AS DestinationProductName				  
				  ,MovDest.DestinationProductTypeId
				  ,MovDest.DestinationStorageLocationId				  
		   FROM [Offchain].[MovementDestination] MovDest
		   INNER JOIN [Admin].[Node] DesNd
		   ON DesNd.NodeId = MovDest.DestinationNodeId 
		   INNER JOIN [Admin].Product DestPrd
		   ON DestPrd.ProductId = MovDest.DestinationProductId
		  )MovDest ON m.MovementTransactionId = MovDest.MovementTransactionId
LEFT JOIN (SELECT  MovSrc.MovementTransactionId
				  ,MovSrc.SourceNodeId
				  ,SrcNd.[Name] AS SourceNodeName
				  ,MovSrc.SourceProductId		
				  ,SrcPrd.[Name] AS SourceProductName				  
				  ,MovSrc.SourceProductTypeId
				  ,MovSrc.SourceStorageLocationId				  
		   FROM [Offchain].[MovementSource] MovSrc
		   INNER JOIN [Admin].[Node] SrcNd
		   ON SrcNd.NodeId = MovSrc.SourceNodeId 
		   INNER JOIN [Admin].Product SrcPrd
		   ON SrcPrd.ProductId = MovSrc.SourceProductId
		  )MovSrc
ON m.MovementTransactionId = MovSrc.MovementTransactionId
left join admin.CategoryElement catunid on catunid.elementId = m.MeasurementUnit
left join admin.CategoryElement MType on Mtype.ElementId = m.MovementTypeId
left join admin.CategoryElement PSrcType on PSrcType.ElementId = MovSrc.SourceProductTypeId
left join admin.CategoryElement PDestType on PDestType.ElementId = MovDest.DestinationProductTypeId
left join admin.CategoryElement SrcSystem on SrcSystem.ElementId = m.SourceSystemId
left join offchain.MovementPeriod mp on mp.MovementTransactionId = m.MovementTransactionId
where cast(UploadDate as date) >= '20220317' order by m.CreatedDate desc


