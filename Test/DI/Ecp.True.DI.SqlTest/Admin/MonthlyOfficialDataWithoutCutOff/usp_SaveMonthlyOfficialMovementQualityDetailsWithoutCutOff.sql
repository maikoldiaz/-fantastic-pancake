/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jul-10-2020
-- Description:     This Procedure is to Get Official Monthly InventoryQuality details based on Element, Node, StartDate, EndDate.
--EXEC [Admin].[usp_SaveMonthlyOfficialMovementQualityDetailsWithoutCutOff] 137236,30812,'2020-06-26','2020-06-28','bd5b494e-ac14-466b-b507-0f93fd22ed7f'
--SELECT * FROM ADMIN.OfficialMonthlyInventoryQualityDetails
-- ==============================================================================================================================*/

--=================================== DATA FOR [Admin].[OfficialMonthlyMovmentDetails] =================================================================================================================================================================================================================================================

INSERT INTO [Admin].[OfficialMonthlyMovmentDetails]([RNo],[System],[Version],[MovementId],[TypeMovement],[Movement],[SourceNode],[DestinationNode],[SourceProduct]			
 ,[DestinationProduct],[MeasurementUnit],[Owner],[Origin],[RegistrationDate],[MovementTransactionId] ,[InputElementId],[InputNodeId]           
 ,[InputStartDate],[InputEndDate],[ExecutionId],[CreatedBy],[CreatedDate])
VALUES (1,'Sistema1','1',637290872470652947,'Pérdida no identificada','Movement1','Automation_quf8h','Automation_ew8ak','CRUDO CAMPO MAMBO',null,'Bbl',
'ECOPETROL','true','2020-07-07',18565,137236,30812,'2020-06-26','2020-06-28','bd5b494e-ac14-466b-b507-0f93fd22ed7f','ReportUser','07-07-2020')
GO


--=================================== SEGMENT DATA FOR [Admin].[Attribute] =================================================================================================================================================================================================================================================

INSERT INTO Admin.Attribute([AttributeId],[AttributeValue],[ValueAttributeUnit],[AttributeDescription],[InventoryProductId],[MovementTransactionId],[AttributeType]        
,[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (167,'Contenido Azufre',137197,'Automation_xsat7',NULL,18565,NULL,'System','2020-07-09',NULL,NULL)
GO


--######################################################################################################################################################################################################################################################################################################################################



--===================== TestCase1: To check if the movementary quality table returns data for Segments within a given date range ====================================
EXEC [Admin].[usp_SaveMonthlyOfficialMovementQualityDetailsWithoutCutOff] 137236,30812,'2020-06-26','2020-06-28','bd5b494e-ac14-466b-b507-0f93fd22ed7f'
SELECT * FROM ADMIN.[OfficialMonthlyMovmentQualityDetails]
GO

--========================================= Output Captured ====================================================================================================
--RNo	System	Version	MovementId	TypeMovement	Movement	SourceNode	DestinationNode	SourceProduct	DestinationProduct	NetQuantity	GrossQuantity	MeasurementUnit	Owner	Ownershipvolume	Ownershippercentage	Origin	RegistrationDate	Attribute	AttributeValue	ValueAttributeUnit	AttributeDescription	MovementTransactionId	InputElementId	InputNodeId	InputStartDate	InputEndDate	ExecutionId	CreatedBy	CreatedDate
--1	Sistema1	1	637290872470652947	Pérdida no identificada	Movement1	Automation_quf8h	Automation_ew8ak	CRUDO CAMPO MAMBO	NULL	NULL	NULL	Bbl	ECOPETROL	NULL	NULL	true	2020-07-07 00:00:00.000	Contenido Azufre	Contenido Azufre	Automation_xsat7	Automation_xsat7	18565	137236	30812	2020-06-26	2020-06-28	bd5b494e-ac14-466b-b507-0f93fd22ed7f	ReportUser	2020-07-10 00:00:00.000
--2	Sistema2	1	637290872470652947	Pérdida no identificada	Movement1	Automation_quf8h	Automation_ew8ak	CRUDO CAMPO MAMBO	NULL	NULL	NULL	Bbl	ECOPETROL	NULL	NULL	true	2020-07-07 00:00:00.000	Contenido Azufre	Contenido Azufre	Automation_xsat7	Automation_xsat7	18565	137236	30812	2020-06-26	2020-06-28	bd5b494e-ac14-466b-b507-0f93fd22ed7f	ReportUser	2020-07-10 00:00:00.000
--3	Sistema3	1	637290872470652947	Pérdida no identificada	Movement1	Automation_quf8h	Automation_ew8ak	CRUDO CAMPO MAMBO	NULL	NULL	NULL	Bbl	ECOPETROL	NULL	NULL	true	2020-07-07 00:00:00.000	Contenido Azufre	Contenido Azufre	Automation_xsat7	Automation_xsat7	18565	137236	30812	2020-06-26	2020-06-28	bd5b494e-ac14-466b-b507-0f93fd22ed7f	ReportUser	2020-07-10 00:00:00.000

