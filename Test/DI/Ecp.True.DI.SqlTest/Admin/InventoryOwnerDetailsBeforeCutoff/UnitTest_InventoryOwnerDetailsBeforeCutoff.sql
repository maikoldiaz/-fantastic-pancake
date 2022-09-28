/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Jun-17-2020

-- Description:     These test cases are for Table [Admin].[OperationalInventoryOwner]

-- Database backup Used:	dbaeuecpdevtrue
-- ==============================================================================================================================*/

/*
The below insert statements are required to populate Segmento and Sistema related data in [Admin].[OperationalInventoryOwner] table.

*/

--=================================== SEGMENT DATA FOR [Admin].[OperationalInventory] =================================================================================================================================================================================================================================================

INSERT INTO [Admin].[OperationalInventory] (RNo,InventoryId,InventoryProductId,CalculationDate,NodeName,TankName,BatchId,Product,ProductId,NetStandardVolume,MeasurementUnit,EventType,SystemName,PercentStandardUnCertainty,InputCategory,InputElementName,InputNodeName,InputStartDate,InputEndDate,ExecutionId,CreatedBy,CreatedDate,GrossStandardQuantity)
VALUES (3,'DEFECT 531520',3962,'2020-06-07 00:00:00.000','Automation_6dthw',NULL,NULL,'CRUDO CAMPO CUSUCO','10000002372',34450.00,'Bbl','Update','EXCEL',0.21,'Segmento','Automation_8srjq','Todos','2020-06-01','2020-06-10','9e67b8af-3d22-400f-bdb5-85a41a7a245a','ReportUser','2020-06-11 14:44:12.187',NULL)
GO
--=================================== SEGMENT DATA FOR[Offchain].[Ownership] =================================================================================================================================================================================================================================================

INSERT INTO Offchain.Ownership(OwnershipId,MessageTypeId,TicketId,MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,BlockchainMovementTransactionId,BlockchainInventoryProductTransactionId,IsDeleted,EventType,BlockchainOwnershipId,PreviousBlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) 
values(4759,7,23757,NULL,3962,30,100.00,1700.00,1,1,'2020-06-12 02:51:27.527',1,NULL,'702AD1CC-3170-4988-90BB-949781D79B46',0,'Update','7D7A1966-BE7B-4AC0-BFF8-4E5F0A1B9886','354572F8-B6A4-45F8-A6CB-3AA54C673C5D','0x990f578138db338a9c578f3d6b1e9d998cfb6f2a3a3045e87aca6ad6fe80cc33','0x1f2157',0,'trueadmin','2020-06-12 02:51:27.640','System','2020-06-12 02:52:19.330')
GO

INSERT INTO Offchain.Ownership(OwnershipId,MessageTypeId,TicketId,MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,BlockchainMovementTransactionId,BlockchainInventoryProductTransactionId,IsDeleted,EventType,BlockchainOwnershipId,PreviousBlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) 
values(4754,7,23757,NULL,3962,29,0.00,0.00,'Propiedad Manual',1,'2020-06-12 02:51:27.527',1,NULL,'702AD1CC-3170-4988-90BB-949781D79B46',0,'Insert','6E5993DB-B534-458B-9E2F-12936ACF789D',NULL,'0x0f8aa4909183535d0bfb63019cac5ce421b3ec86510cc15f0d7a6ccaa3498e6d','0x1f2157',0,'trueadmin','2020-06-12 02:51:27.640','System','2020-06-12 02:52:19.103')
GO

INSERT INTO Offchain.Ownership(OwnershipId,MessageTypeId,TicketId,MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,BlockchainMovementTransactionId,BlockchainInventoryProductTransactionId,IsDeleted,EventType,BlockchainOwnershipId,PreviousBlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) 
values(4758,7,23757,NULL,3962,30,-100.00,-1700.00,1,1,'2020-06-11 05:45:52.947',1,NULL,'702AD1CC-3170-4988-90BB-949781D79B46',1,'Update','354572F8-B6A4-45F8-A6CB-3AA54C673C5D',NULL,NULL,NULL,0,'trueadmin','2020-06-12 02:51:27.640','System','2020-06-12 02:52:19.330')
GO

INSERT INTO Offchain.Ownership(OwnershipId,MessageTypeId,TicketId,MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,BlockchainMovementTransactionId,BlockchainInventoryProductTransactionId,IsDeleted,EventType,BlockchainOwnershipId,PreviousBlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) 
values(4732,7,23757,NULL,3962,30,100.00,1700.00,1,1,'2020-06-11 05:45:52.947',1,NULL,'702AD1CC-3170-4988-90BB-949781D79B46',1,'Insert','3BD3E68A-3BC0-4381-9C5C-B5987A8D6E7B',NULL,'0x68bfe5a3064f2c97051188e85378b9759ef634ae590e03fef83f90e8223045c9','0x1ee600',0,'System','2020-06-11 05:45:52.967','trueadmin','2020-06-12 02:51:27.640')


--=================================== SEGMENT DATA FOR [Offchain].[Owner] =================================================================================================================================================================================================================================================

INSERT INTO Offchain.Owner(OwnerId,OwnershipValue,OwnershipValueUnit,InventoryProductId,MovementTransactionId,BlockchainStatus,BlockchainMovementTransactionId,BlockchainInventoryProductTransactionId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) 
VALUES (71026,100.00,'%',3962,NULL,1,NULL,'44CCCDDB-130C-4446-9FAB-0E80844B7226','System','2020-06-11 07:18:26.663','System','2020-06-11 07:18:39.120')
GO
--=================================== SEGMENT DATA FOR [Admin].[CategoryElement] =================================================================================================================================================================================================================================================



INSERT [Admin].[CategoryElement] ([ElementId], [Name], [Description], [CategoryId], [IsActive], [IconId], [Color], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (30, N'ECOPETROL', N'ECOPETROL', 7, 1, NULL, N'#00903B', N'Sistema', CAST(N'2020-02-07T04:08:45.113' AS DateTime), NULL, NULL)
GO

--######################################################################################################################################################################################################################################################################################################################################

EXEC [Admin].[usp_SaveInventoryOwnerDetailsWithoutCutOffForSegment] 'Segmento','Automation_8srjq','Todos','2020-06-01','2020-06-10','9e67b8af-3d22-400f-bdb5-85a41a7a245a'


--===================== TestCase1: To check if the Inventory owner table returns data for Segments within a given date range ====================================
SELECT * FROM [Admin].[OperationalInventoryOwner] 
WHERE InputCategory = 'Segmento'

--========================================= Output Captured ====================================================================================================

/*
RNo InventoryId     InventoryProductId   CalculationDate            NodeName           TankName BatchId Product             ProductId   NetStandardVolume MeasurementUnit	EventType  SystemName   GrossStandardQuantity       Owner      OwnershipVolume OwnershipPercentage InputCategory InputElementName InputNodeName InputStartDate InputEndDate ExecutionId CreatedBy CreatedDate
3	DEFECT 531520	3962	             2020-06-07 00:00:00.000	Automation_6dthw	NULL	NULL	CRUDO CAMPO CUSUCO	10000002372	34450.00	        Bbl	               Update	EXCEL	     NULL	                    ECOPETROL	34450.00	   100.00	Segmento	Automation_8srjq	Todos	2020-06-01	2020-06-10	9e67b8af-3d22-400f-bdb5-85a41a7a245a	ReportUser	2020-06-17 04:14:40.997
3	DEFECT 531520	3962	             2020-06-07 00:00:00.000	Automation_6dthw	NULL	NULL	CRUDO CAMPO CUSUCO	10000002372	34450.00	        Bbl	               Update	EXCEL	     NULL	                    EQUION	34450.00	      100.00	Segmento	Automation_8srjq	Todos	2020-06-01	2020-06-10	9e67b8af-3d22-400f-bdb5-85a41a7a245a	ReportUser	2020-06-17 04:14:40.997
3	DEFECT 531520	3962	             2020-06-07 00:00:00.000	Automation_6dthw	NULL	NULL	CRUDO CAMPO CUSUCO	10000002372	34450.00	        Bbl	               Update	EXCEL	     NULL	                    ECOPETROL	34450.00	  100.00	Segmento	Automation_8srjq	Todos	2020-06-01	2020-06-10	9e67b8af-3d22-400f-bdb5-85a41a7a245a	ReportUser	2020-06-17 04:14:40.997
3	DEFECT 531520	3962	             2020-06-07 00:00:00.000	Automation_6dthw	NULL	NULL	CRUDO CAMPO CUSUCO	10000002372	34450.00	        Bbl	               Update	EXCEL	     NULL	                    ECOPETROL	34450.00	  100.00	Segmento	Automation_8srjq	Todos	2020-06-01	2020-06-10	9e67b8af-3d22-400f-bdb5-85a41a7a245a	ReportUser	2020-06-17 04:14:40.997
*/
--####################################################################################################################

--=================================== SYSTEM DATA FOR [Admin].[OperationalInventory] =================================================================================================================================================================================================================================================

INSERT INTO [Admin].[OperationalInventory] (RNo,InventoryId,InventoryProductId,CalculationDate,NodeName,TankName,BatchId,Product,ProductId,NetStandardVolume,MeasurementUnit,EventType,SystemName,PercentStandardUnCertainty,InputCategory,InputElementName,InputNodeName,InputStartDate,InputEndDate,ExecutionId,CreatedBy,CreatedDate,GrossStandardQuantity)
VALUES (3,'DEFECT 531520',3962,'2020-06-07 00:00:00.000','Automation_6dthw',NULL,NULL,'CRUDO CAMPO CUSUCO','10000002372',34450.00,'Bbl','Update','EXCEL',0.21,'Sistema','Automation_8srjq','Todos','2020-06-01','2020-06-10','9e67b8af-3d22-400f-bdb5-85a41a7a245a','ReportUser','2020-06-11 14:44:12.187',NULL)
GO
--=================================== SYSTEM DATA FOR[Offchain].[Ownership] =================================================================================================================================================================================================================================================

INSERT INTO Offchain.Ownership(OwnershipId,MessageTypeId,TicketId,MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,BlockchainMovementTransactionId,BlockchainInventoryProductTransactionId,IsDeleted,EventType,BlockchainOwnershipId,PreviousBlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) 
values(4759,7,23757,NULL,3962,30,100.00,1700.00,1,1,'2020-06-12 02:51:27.527',1,NULL,'702AD1CC-3170-4988-90BB-949781D79B46',0,'Update','7D7A1966-BE7B-4AC0-BFF8-4E5F0A1B9886','354572F8-B6A4-45F8-A6CB-3AA54C673C5D','0x990f578138db338a9c578f3d6b1e9d998cfb6f2a3a3045e87aca6ad6fe80cc33','0x1f2157',0,'trueadmin','2020-06-12 02:51:27.640','System','2020-06-12 02:52:19.330')
GO

INSERT INTO Offchain.Ownership(OwnershipId,MessageTypeId,TicketId,MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,BlockchainMovementTransactionId,BlockchainInventoryProductTransactionId,IsDeleted,EventType,BlockchainOwnershipId,PreviousBlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) 
values(4754,7,23757,NULL,3962,29,0.00,0.00,'Propiedad Manual',1,'2020-06-12 02:51:27.527',1,NULL,'702AD1CC-3170-4988-90BB-949781D79B46',0,'Insert','6E5993DB-B534-458B-9E2F-12936ACF789D',NULL,'0x0f8aa4909183535d0bfb63019cac5ce421b3ec86510cc15f0d7a6ccaa3498e6d','0x1f2157',0,'trueadmin','2020-06-12 02:51:27.640','System','2020-06-12 02:52:19.103')
GO

INSERT INTO Offchain.Ownership(OwnershipId,MessageTypeId,TicketId,MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,BlockchainMovementTransactionId,BlockchainInventoryProductTransactionId,IsDeleted,EventType,BlockchainOwnershipId,PreviousBlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) 
values(4758,7,23757,NULL,3962,30,-100.00,-1700.00,1,1,'2020-06-11 05:45:52.947',1,NULL,'702AD1CC-3170-4988-90BB-949781D79B46',1,'Update','354572F8-B6A4-45F8-A6CB-3AA54C673C5D',NULL,NULL,NULL,0,'trueadmin','2020-06-12 02:51:27.640','System','2020-06-12 02:52:19.330')
GO

INSERT INTO Offchain.Ownership(OwnershipId,MessageTypeId,TicketId,MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,BlockchainMovementTransactionId,BlockchainInventoryProductTransactionId,IsDeleted,EventType,BlockchainOwnershipId,PreviousBlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) 
values(4732,7,23757,NULL,3962,30,100.00,1700.00,1,1,'2020-06-11 05:45:52.947',1,NULL,'702AD1CC-3170-4988-90BB-949781D79B46',1,'Insert','3BD3E68A-3BC0-4381-9C5C-B5987A8D6E7B',NULL,'0x68bfe5a3064f2c97051188e85378b9759ef634ae590e03fef83f90e8223045c9','0x1ee600',0,'System','2020-06-11 05:45:52.967','trueadmin','2020-06-12 02:51:27.640')


--=================================== SYSTEM DATA FOR [Offchain].[Owner] =================================================================================================================================================================================================================================================

INSERT INTO Offchain.Owner(OwnerId,OwnershipValue,OwnershipValueUnit,InventoryProductId,MovementTransactionId,BlockchainStatus,BlockchainMovementTransactionId,BlockchainInventoryProductTransactionId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) 
VALUES (71026,100.00,'%',3962,NULL,1,NULL,'44CCCDDB-130C-4446-9FAB-0E80844B7226','System','2020-06-11 07:18:26.663','System','2020-06-11 07:18:39.120')
GO
--=================================== SYSTEM DATA FOR [Admin].[CategoryElement] =================================================================================================================================================================================================================================================



INSERT [Admin].[CategoryElement] ([ElementId], [Name], [Description], [CategoryId], [IsActive], [IconId], [Color], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (30, N'ECOPETROL', N'ECOPETROL', 7, 1, NULL, N'#00903B', N'Sistema', CAST(N'2020-02-07T04:08:45.113' AS DateTime), NULL, NULL)
GO

--######################################################################################################################################################################################################################################################################################################################################

EXEC [Admin].[usp_SaveInventoryOwnerDetailsWithoutCutOffForSegment] 'Sistema','Automation_8srjq','Todos','2020-06-01','2020-06-10','9e67b8af-3d22-400f-bdb5-85a41a7a245a'


--===================== TestCase1: To check if the Inventory owner table returns data for Segments within a given date range ====================================
SELECT * FROM [Admin].[OperationalInventoryOwner] 
WHERE InputCategory = 'Sistema'

--========================================= Output Captured ====================================================================================================

/*
RNo InventoryId     InventoryProductId   CalculationDate            NodeName           TankName BatchId Product             ProductId   NetStandardVolume MeasurementUnit	EventType  SystemName   GrossStandardQuantity       Owner      OwnershipVolume OwnershipPercentage InputCategory InputElementName InputNodeName InputStartDate InputEndDate ExecutionId CreatedBy CreatedDate
3	DEFECT 531520	3962	             2020-06-07 00:00:00.000	Automation_6dthw	NULL	NULL	CRUDO CAMPO CUSUCO	10000002372	34450.00	        Bbl	               Update	EXCEL	     NULL	                    ECOPETROL	34450.00	   100.00	Sistema	Automation_8srjq	Todos	2020-06-01	2020-06-10	9e67b8af-3d22-400f-bdb5-85a41a7a245a	ReportUser	2020-06-17 04:14:40.997
3	DEFECT 531520	3962	             2020-06-07 00:00:00.000	Automation_6dthw	NULL	NULL	CRUDO CAMPO CUSUCO	10000002372	34450.00	        Bbl	               Update	EXCEL	     NULL	                    EQUION	34450.00	      100.00	Sistema	Automation_8srjq	Todos	2020-06-01	2020-06-10	9e67b8af-3d22-400f-bdb5-85a41a7a245a	ReportUser	2020-06-17 04:14:40.997
3	DEFECT 531520	3962	             2020-06-07 00:00:00.000	Automation_6dthw	NULL	NULL	CRUDO CAMPO CUSUCO	10000002372	34450.00	        Bbl	               Update	EXCEL	     NULL	                    ECOPETROL	34450.00	  100.00	Sistema	Automation_8srjq	Todos	2020-06-01	2020-06-10	9e67b8af-3d22-400f-bdb5-85a41a7a245a	ReportUser	2020-06-17 04:14:40.997
3	DEFECT 531520	3962	             2020-06-07 00:00:00.000	Automation_6dthw	NULL	NULL	CRUDO CAMPO CUSUCO	10000002372	34450.00	        Bbl	               Update	EXCEL	     NULL	                    ECOPETROL	34450.00	  100.00	Sistema	Automation_8srjq	Todos	2020-06-01	2020-06-10	9e67b8af-3d22-400f-bdb5-85a41a7a245a	ReportUser	2020-06-17 04:14:40.997
*/
--####################################################################################################################