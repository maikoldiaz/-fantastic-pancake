/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jul-10-2020
-- Description:     This Procedure is to Get Official Monthly InventoryQuality details based on Element, Node, StartDate, EndDate.
--EXEC [Admin].[usp_SaveMonthlyOfficialInventoryQualityDetailsWithoutCutOff] 137236,30812,'2020-06-26','2020-06-28','bd5b494e-ac14-466b-b507-0f93fd22ed7f'
--SELECT * FROM ADMIN.OfficialMonthlyInventoryQualityDetails
-- ==============================================================================================================================*/

--===================================  DATA FOR [Admin].[OfficialMonthlyInventoryDetails] =================================================================================================================================================================================================================================================

INSERT INTO [Admin].[OfficialMonthlyInventoryDetails]
(RNo,[System],[Version],[InventoryId],NodeName,Product,MeasurementUnit,[Owner],[Origin],[RegistrationDate],[InventoryProductId],[InputElementId],
[InputNodeId],[InputStartDate],[InputEndDate],[ExecutionId],[CreatedBy],CreatedDate)
VALUES ( 1,'Sistema1','1','DEFECT 884777','Automation_87wox','CRUDO CAMPO MAMBO','Bbl','ECOPETROL','true','07-07-1997',7844,
137236,30812,'2020-06-26','2020-06-28','bd5b494e-ac14-466b-b507-0f93fd22ed7f','ReportUser','07-07-2020')
GO
INSERT INTO [Admin].[OfficialMonthlyInventoryDetails]
(RNo,[System],[Version],[InventoryId],NodeName,Product,MeasurementUnit,[Owner],[Origin],[RegistrationDate],[InventoryProductId],[InputElementId],
[InputNodeId],[InputStartDate],[InputEndDate],[ExecutionId],[CreatedBy],CreatedDate)
VALUES ( 1,'Sistema1','1','DEFECT 884777','Automation_87wox','CRUDO CAMPO MAMBO','Bbl','ECOPETROL','true','07-07-1997',7844,
137236,30812,'2020-06-26','2020-06-28','bd5b494e-ac14-466b-b507-0f93fd22ed7f','ReportUser','07-07-2020')
GO

--===================================  DATA FOR [Admin].[Attribute] =================================================================================================================================================================================================================================================

INSERT INTO [Admin].[Attribute]([AttributeId],[AttributeValue],[ValueAttributeUnit],[AttributeDescription],[InventoryProductId],[MovementTransactionId],[AttributeType]        
,[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (167,'Contenido Azufre',137197,'Automation_xsat7',7844,NULL,NULL,'System','2020-07-09',NULL,NULL)
GO
INSERT INTO [Admin].[Attribute]([AttributeId],[AttributeValue],[ValueAttributeUnit],[AttributeDescription],[InventoryProductId],[MovementTransactionId],[AttributeType]        
,[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (168,'Contenide',137197,'Automation_xsat8',7844,NULL,NULL,'System','2020-07-09',NULL,NULL)
GO


--######################################################################################################################################################################################################################################################################################################################################



--===================== TestCase1: To check if the MonthlyInventoryQuality returns data for Segments within a given date range ====================================

EXEC [Admin].[usp_SaveMonthlyOfficialInventoryQualityDetailsWithoutCutOff] 137236,30812,'2020-06-26','2020-06-28','bd5b494e-ac14-466b-b507-0f93fd22ed7f'
SELECT * FROM ADMIN.OfficialMonthlyInventoryQualityDetails

--========================================= Output Captured ====================================================================================================
/*
--RNo	System	Version	InventoryId	NodeName	Product	NetStandardVolume	GrossStandardQuantity	MeasurementUnit	Owner	OwnershipVolume	OwnershipPercentage	Origin	RegistrationDate	Attribute	AttributeValue	ValueAttributeUnit	AttributeDescription	InventoryProductId	InputElementId	InputNodeId	InputStartDate	InputEndDate	ExecutionId	CreatedBy	CreatedDate
--1	Sistema1	1	DEFECT 884777	Automation_87wox	CRUDO CAMPO MAMBO	NULL	NULL	Bbl	ECOPETROL	NULL	NULL	true	1997-07-07 00:00:00.000	Contenido Azufre	Contenido Azufre	Automation_xsat7	Automation_xsat7	7844	137236	30812	2020-06-26	2020-06-28	bd5b494e-ac14-466b-b507-0f93fd22ed7f	ReportUser	2020-07-10 00:00:00.000
--2	Sistema1	1	DEFECT 884777	Automation_87wox	CRUDO CAMPO MAMBO	NULL	NULL	Bbl	ECOPETROL	NULL	NULL	true	1997-07-07 00:00:00.000	Contenido Azufre	Contenido Azufre	Automation_xsat7	Automation_xsat7	7844	137236	30812	2020-06-26	2020-06-28	bd5b494e-ac14-466b-b507-0f93fd22ed7f	ReportUser	2020-07-10 00:00:00.000
--3	Sistema1	1	DEFECT 884777	Automation_87wox	CRUDO CAMPO MAMBO	NULL	NULL	Bbl	ECOPETROL	NULL	NULL	true	1997-07-07 00:00:00.000	Contenido Azufre	Contenido Azufre	Automation_xsat7	Automation_xsat7	7844	137236	30812	2020-06-26	2020-06-28	bd5b494e-ac14-466b-b507-0f93fd22ed7f	ReportUser	2020-07-10 00:00:00.000
--4	Sistema1	1	DEFECT 884777	Automation_87wox	CRUDO CAMPO MAMBO	NULL	NULL	Bbl	ECOPETROL	NULL	NULL	true	1997-07-07 00:00:00.000	Contenido Azufre	Contenido Azufre	Automation_xsat7	Automation_xsat7	7844	137236	30812	2020-06-26	2020-06-28	bd5b494e-ac14-466b-b507-0f93fd22ed7f	ReportUser	2020-07-10 00:00:00.000
--5	Sistema1	1	DEFECT 884777	Automation_87wox	CRUDO CAMPO MAMBO	NULL	NULL	Bbl	ECOPETROL	NULL	NULL	true	1997-07-07 00:00:00.000	Contenido Azufre	Contenido Azufre	Automation_xsat7	Automation_xsat7	7844	137236	30812	2020-06-26	2020-06-28	bd5b494e-ac14-466b-b507-0f93fd22ed7f	ReportUser	2020-07-10 00:00:00.000
--6	Sistema1	1	DEFECT 884777	Automation_87wox	CRUDO CAMPO MAMBO	NULL	NULL	Bbl	ECOPETROL	NULL	NULL	true	1997-07-07 00:00:00.000	Contenido Azufre	Contenido Azufre	Automation_xsat7	Automation_xsat7	7844	137236	30812	2020-06-26	2020-06-28	bd5b494e-ac14-466b-b507-0f93fd22ed7f	ReportUser	2020-07-10 00:00:00.000
--7	Sistema1	1	DEFECT 884777	Automation_87wox	CRUDO CAMPO MAMBO	NULL	NULL	Bbl	ECOPETROL	NULL	NULL	true	1997-07-07 00:00:00.000	ECOPETROL	Contenide	Automation_xsat7	Automation_xsat8	7844	137236	30812	2020-06-26	2020-06-28	bd5b494e-ac14-466b-b507-0f93fd22ed7f	ReportUser	2020-07-10 00:00:00.000
--8	Sistema1	1	DEFECT 884777	Automation_87wox	CRUDO CAMPO MAMBO	NULL	NULL	Bbl	ECOPETROL	NULL	NULL	true	1997-07-07 00:00:00.000	ECOPETROL	Contenide	Automation_xsat7	Automation_xsat8	7844	137236	30812	2020-06-26	2020-06-28	bd5b494e-ac14-466b-b507-0f93fd22ed7f	ReportUser	2020-07-10 00:00:00.000
--9	Sistema1	1	DEFECT 884777	Automation_87wox	CRUDO CAMPO MAMBO	NULL	NULL	Bbl	ECOPETROL	NULL	NULL	true	1997-07-07 00:00:00.000	ECOPETROL	Contenide	Automation_xsat7	Automation_xsat8	7844	137236	30812	2020-06-26	2020-06-28	bd5b494e-ac14-466b-b507-0f93fd22ed7f	ReportUser	2020-07-10 00:00:00.000
--10	Sistema1	1	DEFECT 884777	Automation_87wox	CRUDO CAMPO MAMBO	NULL	NULL	Bbl	ECOPETROL	NULL	NULL	true	1997-07-07 00:00:00.000	ECOPETROL	Contenide	Automation_xsat7	Automation_xsat8	7844	137236	30812	2020-06-26	2020-06-28	bd5b494e-ac14-466b-b507-0f93fd22ed7f	ReportUser	2020-07-10 00:00:00.000*/
