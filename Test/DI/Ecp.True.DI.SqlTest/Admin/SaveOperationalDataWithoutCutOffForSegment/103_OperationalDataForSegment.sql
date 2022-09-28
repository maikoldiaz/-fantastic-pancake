/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Jan-30-2020

-- Description:     These test cases are for View [Admin].[Operational]

-- Database backup Used:	appdb_dev_Jan_27_2020
-- ==============================================================================================================================*/

/*
The below insert statements are required to populate Segment data in [Offchain].[Inventory] and 
[Offchain].[InventoryProduct] tables.

*/

INSERT INTO [Offchain].[Inventory] ([SystemTypeId],[SystemName],[SourceSystem],[DestinationSystem],[EventType],[TankName],[InventoryId],[TicketId],[InventoryDate],[NodeId],[SegmentId],[Observations],[Scenario],[IsDeleted],[FileRegistrationTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (1,NULL,'SINOPER','TRUE','INSERT',NULL,9876,NULL,'2020-01-04 22:57:59.000',3411 ,7230,NULL ,'Operativo',0,2268,'System','2020-01-28 15:19:28.697',NULL,NULL )

INSERT INTO [Offchain].[Inventory] ([SystemTypeId],[SystemName],[SourceSystem],[DestinationSystem],[EventType],[TankName],[InventoryId],[TicketId],[InventoryDate],[NodeId],[SegmentId],[Observations],[Scenario],[IsDeleted],[FileRegistrationTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (1,NULL,'SINOPER','TRUE','INSERT',NULL,9877,NULL,'2020-01-05 22:57:59.000',3450 ,7230,NULL ,'Operativo',0,2268,'System','2020-01-28 15:19:28.697',NULL,NULL )

INSERT INTO [Offchain].[Inventory] ([SystemTypeId],[SystemName],[SourceSystem],[DestinationSystem],[EventType],[TankName],[InventoryId],[TicketId],[InventoryDate],[NodeId],[SegmentId],[Observations],[Scenario],[IsDeleted],[FileRegistrationTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (1,NULL,'SINOPER','TRUE','INSERT',NULL,9878,NULL,'2020-01-06 22:57:59.000',3495 ,7230,NULL ,'Operativo',0,2268,'System','2020-01-28 15:19:28.697',NULL,NULL )

INSERT INTO [Offchain].[Inventory] ([SystemTypeId],[SystemName],[SourceSystem],[DestinationSystem],[EventType],[TankName],[InventoryId],[TicketId],[InventoryDate],[NodeId],[SegmentId],[Observations],[Scenario],[IsDeleted],[FileRegistrationTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (1,NULL,'SINOPER','TRUE','INSERT',NULL,9876,NULL,'2020-01-03 22:57:59.000',3411 ,7230,NULL ,'Operativo',0,2268,'System','2020-01-28 15:19:28.697',NULL,NULL )

INSERT INTO [Offchain].[Inventory] ([SystemTypeId],[SystemName],[SourceSystem],[DestinationSystem],[EventType],[TankName],[InventoryId],[TicketId],[InventoryDate],[NodeId],[SegmentId],[Observations],[Scenario],[IsDeleted],[FileRegistrationTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (1,NULL,'SINOPER','TRUE','INSERT',NULL,9877,NULL,'2020-01-03 22:57:59.000',3450 ,7230,NULL ,'Operativo',0,2268,'System','2020-01-28 15:19:28.697',NULL,NULL )

INSERT INTO [Offchain].[Inventory] ([SystemTypeId],[SystemName],[SourceSystem],[DestinationSystem],[EventType],[TankName],[InventoryId],[TicketId],[InventoryDate],[NodeId],[SegmentId],[Observations],[Scenario],[IsDeleted],[FileRegistrationTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (1,NULL,'SINOPER','TRUE','INSERT',NULL,9878,NULL,'2020-01-05 22:57:59.000',3495 ,7230,NULL ,'Operativo',0,2268,'System','2020-01-28 15:19:28.697',NULL,NULL )

INSERT INTO [Offchain].[Inventory] ([SystemTypeId],[SystemName],[SourceSystem],[DestinationSystem],[EventType],[TankName],[InventoryId],[TicketId],[InventoryDate],[NodeId],[SegmentId],[Observations],[Scenario],[IsDeleted],[FileRegistrationTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (1,NULL,'SINOPER','TRUE','INSERT',NULL,9876,NULL,'2020-01-02 22:57:59.000',3411 ,7230,NULL ,'Operativo',0,2268,'System','2020-01-28 15:19:28.697',NULL,NULL )

--=================================== SEGMENT DATA FOR [Offchain].[InventoryProduct] =================================================================================================================================================================================================================================================

INSERT INTO [Offchain].[InventoryProduct]([ProductId],[ProductType],[ProductVolume],[MeasurementUnit],[InventoryProductId],[UncertaintyPercentage],[OwnershipTicketId],[ReasonId],[Comment],[BlockchainStatus],[BlockchainInventoryProductTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10000002318,7252,34450,31,957,0.04,NULL,NULL,NULL,NULL,NULL,'System','2020-01-28 15:19:28.697',NULL,NULL)

INSERT INTO [Offchain].[InventoryProduct]([ProductId],[ProductType],[ProductVolume],[MeasurementUnit],[InventoryProductId],[UncertaintyPercentage],[OwnershipTicketId],[ReasonId],[Comment],[BlockchainStatus],[BlockchainInventoryProductTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10000002372,7252,598793,31,960,0.04,NULL,NULL,NULL,NULL,NULL,'System','2020-01-28 15:19:28.697',NULL,NULL)

INSERT INTO [Offchain].[InventoryProduct]([ProductId],[ProductType],[ProductVolume],[MeasurementUnit],[InventoryProductId],[UncertaintyPercentage],[OwnershipTicketId],[ReasonId],[Comment],[BlockchainStatus],[BlockchainInventoryProductTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10000002318,7252,34874,31,961,0.04,NULL,NULL,NULL,NULL,NULL,'System','2020-01-28 15:19:28.697',NULL,NULL)


INSERT INTO [Offchain].[InventoryProduct]([ProductId],[ProductType],[ProductVolume],[MeasurementUnit],[InventoryProductId],[UncertaintyPercentage],[OwnershipTicketId],[ReasonId],[Comment],[BlockchainStatus],[BlockchainInventoryProductTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10000002318,7252,34450,31,962,0.04,NULL,NULL,NULL,NULL,NULL,'System','2020-01-28 15:19:28.697',NULL,NULL)

INSERT INTO [Offchain].[InventoryProduct]([ProductId],[ProductType],[ProductVolume],[MeasurementUnit],[InventoryProductId],[UncertaintyPercentage],[OwnershipTicketId],[ReasonId],[Comment],[BlockchainStatus],[BlockchainInventoryProductTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10000002372,7252,598793,31,963,0.04,NULL,NULL,NULL,NULL,NULL,'System','2020-01-28 15:19:28.697',NULL,NULL)

INSERT INTO [Offchain].[InventoryProduct]([ProductId],[ProductType],[ProductVolume],[MeasurementUnit],[InventoryProductId],[UncertaintyPercentage],[OwnershipTicketId],[ReasonId],[Comment],[BlockchainStatus],[BlockchainInventoryProductTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10000002318,7252,34874,31,964,0.04,NULL,NULL,NULL,NULL,NULL,'System','2020-01-28 15:19:28.697',NULL,NULL)

INSERT INTO [Offchain].[InventoryProduct]([ProductId],[ProductType],[ProductVolume],[MeasurementUnit],[InventoryProductId],[UncertaintyPercentage],[OwnershipTicketId],[ReasonId],[Comment],[BlockchainStatus],[BlockchainInventoryProductTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10000002318,7252,34874,31,965,0.04,NULL,NULL,NULL,NULL,NULL,'System','2020-01-28 15:19:28.697',NULL,NULL)


--CASE: Category='Segmento',Element='Automation_su4to',NodeName='ALL',StartDate='2020-01-01',EndDate='2020-01-28',ExecutionId='738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'		
EXEC  [Admin].[usp_SaveOperationalDataWithoutCutOffForSegment] 'Segmento','Automation_su4to','ALL','2020-01-01','2020-01-28','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'		
/*
OUTPUT QUERY:- SELECT ProductId,ProductName,SegmentId,SegmentName,NodeId,CalculationDate,Inputs,Outputs,IdentifiedLosses
               ,IntialInventory,FinalInventory,UnBalance FROM [Admin].[Operational] 
			   WHERE SegmentName = 'Automation_su4to'
            ORDER BY NodeId,CalculationDate

OUTPUT:
ProductId	ProductName	SegmentId	SegmentName	NodeId	CalculationDate	Inputs	Outputs	    IdentifiedLosses	IntialInventory	FinalInventory	UnBalance
10000002318	CRUDO CAMPO MAMBO	7230	Automation_su4to	3411	2020-01-02	0.00	0.00	NULL	            0.00	        34874.00	-34874.00
10000002318	CRUDO CAMPO MAMBO	7230	Automation_su4to	3411	2020-01-03	0.00	0.00	NULL	            34874.00	    34450.00	424.00
10000002318	CRUDO CAMPO MAMBO	7230	Automation_su4to	3411	2020-01-04	0.00	0.00	NULL	            34450.00	    34450.00	0.00
10000002372	CRUDO CAMPO CUSUCO	7230	Automation_su4to	3450	2020-01-03	0.00	0.00	NULL	            0.00	        598793.00	-598793.00
10000002372	CRUDO CAMPO CUSUCO	7230	Automation_su4to	3450	2020-01-05	0.00	0.00	NULL	            0.00	        598793.00	-598793.00
10000002318	CRUDO CAMPO MAMBO	7230	Automation_su4to	3495	2020-01-05	0.00	0.00	NULL	            0.00	        34874.00	-34874.00
10000002318	CRUDO CAMPO MAMBO	7230	Automation_su4to	3495	2020-01-06	0.00	0.00	NULL	            34874.00	    34874.00	0.00
/*