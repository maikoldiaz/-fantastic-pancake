/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Jan-28-2020

-- Description:     These test cases are for View [Admin].[InventoryDetailsBeforeCutoff]

-- Database backup Used:	appdb_dev_0128
-- ==============================================================================================================================*/

/*
The below insert statements are required to populate Segment and System related data in [Offchain].[Inventory] and 
[Offchain].[InventoryProduct] tables.

*/

--=================================== SEGMENT DATA FOR [Offchain].[Inventory] =================================================================================================================================================================================================================================================
INSERT INTO [Offchain].[Inventory] ([SystemTypeId],[SystemName],[SourceSystem],[DestinationSystem],[EventType],[TankName],[InventoryId],[TicketId],[InventoryDate],[NodeId],[SegmentId],[Observations],[Scenario],[IsDeleted],[FileRegistrationTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (1,NULL,'SINOPER','TRUE','INSERT',NULL,9876,NULL,'2030-10-04 22:57:59.000',3411 ,7230,NULL ,'Operativo',0,2268,'System','2020-01-28 15:19:28.697',NULL,NULL )

INSERT INTO [Offchain].[Inventory] ([SystemTypeId],[SystemName],[SourceSystem],[DestinationSystem],[EventType],[TankName],[InventoryId],[TicketId],[InventoryDate],[NodeId],[SegmentId],[Observations],[Scenario],[IsDeleted],[FileRegistrationTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (1,NULL,'SINOPER','TRUE','INSERT',NULL,9877,NULL,'2030-10-05 22:57:59.000',3450 ,7334,NULL ,'Operativo',0,2268,'System','2020-01-28 15:19:28.697',NULL,NULL )

INSERT INTO [Offchain].[Inventory] ([SystemTypeId],[SystemName],[SourceSystem],[DestinationSystem],[EventType],[TankName],[InventoryId],[TicketId],[InventoryDate],[NodeId],[SegmentId],[Observations],[Scenario],[IsDeleted],[FileRegistrationTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (1,NULL,'SINOPER','TRUE','INSERT',NULL,9878,NULL,'2030-10-06 22:57:59.000',3495 ,7461,NULL ,'Operativo',0,2268,'System','2020-01-28 15:19:28.697',NULL,NULL )

--=================================== SEGMENT DATA FOR [Offchain].[InventoryProduct] =================================================================================================================================================================================================================================================

INSERT INTO [Offchain].[InventoryProduct]([ProductId],[ProductType],[ProductVolume],[MeasurementUnit],[InventoryProductId],[UncertaintyPercentage],[OwnershipTicketId],[ReasonId],[Comment],[BlockchainStatus],[BlockchainTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10000002318,7252,34450,31,957,0.04,NULL,NULL,NULL,NULL,NULL,'System','2020-01-28 15:19:28.697',NULL,NULL)

INSERT INTO [Offchain].[InventoryProduct]([ProductId],[ProductType],[ProductVolume],[MeasurementUnit],[InventoryProductId],[UncertaintyPercentage],[OwnershipTicketId],[ReasonId],[Comment],[BlockchainStatus],[BlockchainTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10000002372,7252,34450,31,958,0.04,NULL,NULL,NULL,NULL,NULL,'System','2020-01-28 15:19:28.697',NULL,NULL)

INSERT INTO [Offchain].[InventoryProduct]([ProductId],[ProductType],[ProductVolume],[MeasurementUnit],[InventoryProductId],[UncertaintyPercentage],[OwnershipTicketId],[ReasonId],[Comment],[BlockchainStatus],[BlockchainTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10000002318,7252,34450,31,959,0.04,NULL,NULL,NULL,NULL,NULL,'System','2020-01-28 15:19:28.697',NULL,NULL)

--=================================== SYSTEM DATA FOR [Offchain].[Inventory] =================================================================================================================================================================================================================================================

INSERT INTO [Offchain].[Inventory] ([SystemTypeId],[SystemName],[SourceSystem],[DestinationSystem],[EventType],[TankName],[InventoryId],[TicketId],[InventoryDate],[NodeId],[SegmentId],[Observations],[Scenario],[IsDeleted],[FileRegistrationTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (1,NULL,'SINOPER','TRUE','INSERT',NULL,9900,NULL,'2030-10-04 22:57:59.000',4125 ,9925,NULL ,'Operativo',0,2268,'System','2020-01-28 15:19:28.697',NULL,NULL )

INSERT INTO [Offchain].[Inventory] ([SystemTypeId],[SystemName],[SourceSystem],[DestinationSystem],[EventType],[TankName],[InventoryId],[TicketId],[InventoryDate],[NodeId],[SegmentId],[Observations],[Scenario],[IsDeleted],[FileRegistrationTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (1,NULL,'SINOPER','TRUE','INSERT',NULL,9901,NULL,'2030-10-05 22:57:59.000',4331 ,10603,NULL ,'Operativo',0,2268,'System','2020-01-28 15:19:28.697',NULL,NULL )

INSERT INTO [Offchain].[Inventory] ([SystemTypeId],[SystemName],[SourceSystem],[DestinationSystem],[EventType],[TankName],[InventoryId],[TicketId],[InventoryDate],[NodeId],[SegmentId],[Observations],[Scenario],[IsDeleted],[FileRegistrationTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (1,NULL,'SINOPER','TRUE','INSERT',NULL,9902,NULL,'2030-10-06 22:57:59.000',4346 ,10853,NULL ,'Operativo',0,2268,'System','2020-01-28 15:19:28.697',NULL,NULL )

--=================================== SYSTEM DATA FOR [Offchain].[InventoryProduct] =================================================================================================================================================================================================================================================

INSERT INTO [Offchain].[InventoryProduct]([ProductId],[ProductType],[ProductVolume],[MeasurementUnit],[InventoryProductId],[UncertaintyPercentage],[OwnershipTicketId],[ReasonId],[Comment],[BlockchainStatus],[BlockchainTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10000002318,7252,34450,31,960,0.04,NULL,NULL,NULL,NULL,NULL,'System','2020-01-28 15:19:28.697',NULL,NULL)

INSERT INTO [Offchain].[InventoryProduct]([ProductId],[ProductType],[ProductVolume],[MeasurementUnit],[InventoryProductId],[UncertaintyPercentage],[OwnershipTicketId],[ReasonId],[Comment],[BlockchainStatus],[BlockchainTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10000002372,7252,28000,31,961,0.08,NULL,NULL,NULL,NULL,NULL,'System','2020-01-28 15:19:28.697',NULL,NULL)

INSERT INTO [Offchain].[InventoryProduct]([ProductId],[ProductType],[ProductVolume],[MeasurementUnit],[InventoryProductId],[UncertaintyPercentage],[OwnershipTicketId],[ReasonId],[Comment],[BlockchainStatus],[BlockchainTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10000002318,7252,37000,31,962,0.06,NULL,NULL,NULL,NULL,NULL,'System','2020-01-28 15:19:28.697',NULL,NULL)

--######################################################################################################################################################################################################################################################################################################################################



--===================== TestCase1: To check if the Inventory Details View returns data for Segments within a given date range ====================================

SELECT * FROM [Admin].[InventoryDetailsBeforeCutoff]
WHERE Category = 'Segmento'
AND CalculationDate BETWEEN '2030-10-04' AND '2030-10-06'

--========================================= Output Captured ====================================================================================================
/*

InventoryId	CalculationDate	NodeName	TankName	Product	NetVolume	MeasurementUnit	UncertaintyPercentage	ProductId	Category	Element	RNo
9876	2030-10-04	Automation_5u3uk	NULL	CRUDO CAMPO MAMBO	34450.00	Bbl	0.04	10000002318	Segmento	Automation_su4to	217
9878	2030-10-06	Automation_8ia3v	NULL	CRUDO CAMPO MAMBO	34450.00	Bbl	0.04	10000002318	Segmento	Automation_tphuf	294
9877	2030-10-05	Automation_blq8d	NULL	CRUDO CAMPO CUSUCO	34450.00	Bbl	0.04	10000002372	Segmento	Automation_l12hf	349

*/


--===================== TestCase2: To check if the Inventory Details View returns data for Systems within a given date range ====================================

SELECT * FROM [Admin].[InventoryDetailsBeforeCutoff]
WHERE Category = 'Sistema' 
AND CalculationDate BETWEEN '2030-10-04' AND '2030-10-06'

--========================================= Output Captured ====================================================================================================
/*

InventoryId	CalculationDate	NodeName	TankName	Product	NetVolume	MeasurementUnit	UncertaintyPercentage	ProductId	Category	Element	RNo
9902	2030-10-06	Automation_ct8ze	NULL	CRUDO CAMPO MAMBO	37000.00	Bbl	0.06	10000002318	Sistema	SystemForData	394
9901	2030-10-05	Automation_enxjq	NULL	CRUDO CAMPO CUSUCO	28000.00	Bbl	0.08	10000002372	Sistema	SystemManual	424
9900	2030-10-04	Automation_nm2s0	NULL	CRUDO CAMPO MAMBO	34450.00	Bbl	0.04	10000002318	Sistema	SystemElement2	571

*/

--===================== TestCase3: To check if Inventories where Source System is TRUE are excluded ============================================================

-- Updating an Inventory for Segment by setting it's SourceSystem to 'TRUE'. 
-- This Inventory should be excluded in the result set.

UPDATE [Offchain].[Inventory] SET [SourceSystem] = 'TRUE' WHERE InventoryId = '9878'

-- Updating an Inventory for System by setting it's SourceSystem to 'TRUE'. 
-- This Inventory should be excluded in the result set.

UPDATE [Offchain].[Inventory] SET [SourceSystem] = 'TRUE' WHERE InventoryId = '9900'

-- To check data returned by the View for Segment for the given date range.

SELECT * FROM [Admin].[InventoryDetailsBeforeCutoff]
WHERE Category = 'Segmento'
AND CalculationDate BETWEEN '2030-10-04' AND '2030-10-06'

-- To check data returned by the View for Segment for the given date range.

SELECT * FROM [Admin].[InventoryDetailsBeforeCutoff]
WHERE Category = 'Sistema' 
AND CalculationDate BETWEEN '2030-10-04' AND '2030-10-06'

--========================================= Outputs Captured ====================================================================================================

-- OUTPUT FOR SEGMENT

/*

InventoryId	CalculationDate	NodeName	TankName	Product	NetVolume	MeasurementUnit	UncertaintyPercentage	ProductId	Category	Element	RNo
9876	2030-10-04	Automation_5u3uk	NULL	CRUDO CAMPO MAMBO	34450.00	Bbl	0.04	10000002318	Segmento	Automation_su4to	217
9877	2030-10-05	Automation_blq8d	NULL	CRUDO CAMPO CUSUCO	34450.00	Bbl	0.04	10000002372	Segmento	Automation_l12hf	348

*/

-- OUTPUT FOR SYSTEM

/*

InventoryId	CalculationDate	NodeName	TankName	Product	NetVolume	MeasurementUnit	UncertaintyPercentage	ProductId	Category	Element	RNo
9902	2030-10-06	Automation_ct8ze	NULL	CRUDO CAMPO MAMBO	37000.00	Bbl	0.06	10000002318	Sistema	SystemForData	393
9901	2030-10-05	Automation_enxjq	NULL	CRUDO CAMPO CUSUCO	28000.00	Bbl	0.08	10000002372	Sistema	SystemManual	423

*/

--#################################################################################################################################################################