/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Jul-16-2020
-- Description:     These test cases are for [usp_SaveMonthlyConsolidatedDeltaData] SP.
EXEC [Admin].[usp_SaveMonthlyConsolidatedDeltaInventoryDetailsWithoutCutOff] 137236,31855,'2020-07-03','2020-07-16','B5A8CF32-1665-467C-996F-C58FB13F86C6'
SELECT * FROM [Admin].[ConsolidatedDeltaInventory]
-- ==============================================================================================================================*/

--===================================  DATA FOR [Admin].[ConsolidatedInventoryProduct]=================================================================================================================================================================================================================================================

Insert into [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(31855,	10000002318,	'2020-06-30 00:00:00.000',	100000.00,	120000.00,	'31',	26970,	137236,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)

Insert into [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(31855,	10000002318,	'2020-07-10 00:00:00.000',	100000.00,	120000.00,	'31',	26970,	137236,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)

Insert into [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(31855,	10000002318,	'2020-07-13 00:00:00.000',	100000.00,	120000.00,	'31',	26970,	137236,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)

Insert into [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(31855,	10000002372,	'2020-07-13 00:00:00.000',	100000.00,	120000.00,	'31',	26970,	137236,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)

Insert into [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(31855,	10000002372,	'2020-07-14 00:00:00.000',	100000.00,	120000.00,	'31',	26970,	137236,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)

Insert into [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(31855,	10000002372,	'2020-07-15 00:00:00.000',	100000.00,	120000.00,	'31',	26970,	137236,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)

Insert into [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(31855,	10000002318,	'2020-06-30 00:00:00.000',	100000.00,	120000.00,	'31',	26970,	137236,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)

Insert into [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(31855,	10000002318,	'2020-07-10 00:00:00.000',	100000.00,	120000.00,	'31',	26970,	137236,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
   
--===================================  DATA FOR[Admin].[ConsolidatedNodeTagCalculationDate] =================================================================================================================================================================================================================================================

SELECT * FROM [Admin].[ConsolidatedNodeTagCalculationDate]
INSERT INTO [Admin].[ConsolidatedNodeTagCalculationDate](NodeId,ElementId,ElementName,CalculationDate,InputElement,InputNode,InputStartDate,InputEndDate,ExecutionId,CreatedBy,CreatedDate)
VALUES(31855,137236,'Automation_vbzgj','2020-07-10',137236,31855,'2020-07-03','2020-07-16','B5A8CF32-1665-467C-996F-C58FB13F86C6','System','2020-07-14')

INSERT INTO [Admin].[ConsolidatedNodeTagCalculationDate](NodeId,ElementId,ElementName,CalculationDate,InputElement,InputNode,InputStartDate,InputEndDate,ExecutionId,CreatedBy,CreatedDate)
VALUES(31855,137236,'Automation_vbzgj','2020-07-11',137236,31855,'2020-07-03','2020-07-16','B5A8CF32-1665-467C-996F-C58FB13F86C6','System','2020-07-14')

INSERT INTO [Admin].[ConsolidatedNodeTagCalculationDate](NodeId,ElementId,ElementName,CalculationDate,InputElement,InputNode,InputStartDate,InputEndDate,ExecutionId,CreatedBy,CreatedDate)
VALUES(31855,137236,'Automation_vbzgj','2020-07-12',137236,31855,'2020-07-03','2020-07-16','B5A8CF32-1665-467C-996F-C58FB13F86C6','System','2020-07-14')

INSERT INTO [Admin].[ConsolidatedNodeTagCalculationDate](NodeId,ElementId,ElementName,CalculationDate,InputElement,InputNode,InputStartDate,InputEndDate,ExecutionId,CreatedBy,CreatedDate)
VALUES(31855,137236,'Automation_vbzgj','2020-07-13',137236,31855,'2020-07-03','2020-07-16','B5A8CF32-1665-467C-996F-C58FB13F86C6','System','2020-07-14')

--=================================== DATA FOR  ADMIN.ConsolidatedOwner =================================================================================================================================================================================================================================================

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,21,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,21,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,22,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,22,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,23,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,23,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,24,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,24,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,25,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,25,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,26,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,26,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,27,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,27,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,28,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,28,'System','2020-07-07',NULL,NULL)

--######################################################################################################################################################################################################################################################################################################################################



--===================== TestCase1: To check if the movementary owner table returns data for Segments within a given date range ====================================

EXEC [Admin].[usp_SaveMonthlyConsolidatedDeltaInventoryDetailsWithoutCutOff]
	 @ElementId        =  137236
	 ,@NodeId         =  31855
	 ,@StartDate      =   '2020-07-03'   
	 ,@EndDate        =    '2020-07-16'  
	 ,@ExecutionId    =  'B5A8CF32-1665-467C-996F-C58FB13F86C6'
SELECT * FROM [Admin].[ConsolidatedDeltaInventory]

--========================================= Output Captured ====================================================================================================
/*
RNo	Date	NodeName	Product	NetQuantity	GrossQuantity	MeasurementUnit	Owner	OwnershipVolume	OwnershipPercentage	Scenario	Origin	ExecutionId	ExecutionDate	CreatedBy	CreatedDate

1	2020-07-13 00:00:00.000	Automation_7meee	CRUDO CAMPO CUSUCO	100000.00	120000.00	31	ECOPETROL	4000.00	0.40	NULL	Facilidad	2020-07-11 00:00:00.000	137236	31855	2020-07-03	2020-07-16	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 00:00:00.000
2	2020-07-14 00:00:00.000	Automation_7meee	CRUDO CAMPO CUSUCO	100000.00	120000.00	31	ECOPETROL	4000.00	0.40	NULL	Facilidad	2020-07-11 00:00:00.000	137236	31855	2020-07-03	2020-07-16	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 00:00:00.000
3	2020-07-15 00:00:00.000	Automation_7meee	CRUDO CAMPO CUSUCO	100000.00	120000.00	31	ECOPETROL	4000.00	0.40	NULL	Facilidad	2020-07-11 00:00:00.000	137236	31855	2020-07-03	2020-07-16	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 00:00:00.000
4	2020-07-13 00:00:00.000	Automation_7meee	CRUDO CAMPO CUSUCO	100000.00	120000.00	31	OTROS	4000.00	0.40	NULL	Facilidad	2020-07-11 00:00:00.000	137236	31855	2020-07-03	2020-07-16	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 00:00:00.000
5	2020-07-14 00:00:00.000	Automation_7meee	CRUDO CAMPO CUSUCO	100000.00	120000.00	31	OTROS	4000.00	0.40	NULL	Facilidad	2020-07-11 00:00:00.000	137236	31855	2020-07-03	2020-07-16	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 00:00:00.000
6	2020-07-15 00:00:00.000	Automation_7meee	CRUDO CAMPO CUSUCO	100000.00	120000.00	31	OTROS	4000.00	0.40	NULL	Facilidad	2020-07-11 00:00:00.000	137236	31855	2020-07-03	2020-07-16	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 00:00:00.000
7	2020-06-30 00:00:00.000	Automation_7meee	CRUDO CAMPO MAMBO	100000.00	120000.00	31	NULL	4000.00	0.40	NULL	Facilidad	2020-07-11 00:00:00.000	137236	31855	2020-07-03	2020-07-16	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 00:00:00.000
8	2020-07-10 00:00:00.000	Automation_7meee	CRUDO CAMPO MAMBO	100000.00	120000.00	31	ECOPETROL	4000.00	0.40	NULL	Facilidad	2020-07-11 00:00:00.000	137236	31855	2020-07-03	2020-07-16	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 00:00:00.000
9	2020-07-13 00:00:00.000	Automation_7meee	CRUDO CAMPO MAMBO	100000.00	120000.00	31	ECOPETROL	4000.00	0.40	NULL	Facilidad	2020-07-11 00:00:00.000	137236	31855	2020-07-03	2020-07-16	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 00:00:00.000
10	2020-07-10 00:00:00.000	Automation_7meee	CRUDO CAMPO MAMBO	100000.00	120000.00	31	OTROS	4000.00	0.40	NULL	Facilidad	2020-07-11 00:00:00.000	137236	31855	2020-07-03	2020-07-16	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 00:00:00.000
11	2020-07-13 00:00:00.000	Automation_7meee	CRUDO CAMPO MAMBO	100000.00	120000.00	31	OTROS	4000.00	0.40	NULL	Facilidad	2020-07-11 00:00:00.000	137236	31855	2020-07-03	2020-07-16	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 00:00:00.000