/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Jul-16-2020
-- Description:     These test cases are for [usp_SaveMonthlyConsolidatedDeltaData] SP.
	EXEC [Admin].[usp_SaveMonthlyConsolidatedDeltaData]
	  @ElementId        =  137236
	 ,@NodeId         =  31855
	 ,@StartDate      =   '2020-07-03'   
	 ,@EndDate        =    '2020-07-17'  
	 ,@ExecutionId    =  'B5A8CF32-1665-467C-996F-C58FB13F86C6'
   SELECT * FROM [Admin].[ConsolidatedDeltaBalance]
-- Database backup Used:	dbaeuecpdevtrue
-- ==============================================================================================================================*/

--===================================  DATA FOR [Admin].[ConsolidatedDeltaMovementInformation]=================================================================================================================================================================================================================================================

Insert into [Admin].[ConsolidatedDeltaMovementInformation](OperationalDate,	SourceProductId,	SourceProductName,	SegmentID,	SourceNodeId,	SourceNodeName,	SourceNodeNameIsGeneric,	DestinationProductId,
DestinationProductName,	DestinationNodeId,	DestinationNodeName,	DestinationNodeNameIsGeneric,	MessageTypeId,	Classification,	MovementID,	MovementTypeName,	MeasurementUnit,	MovementTransactionId,	EventType,	SystemName,	SourceSystem,	DeltaNetStandardVolume,	DeltaGrossStandardVolume,	ExecutionDate,	ConsolidatedMovementId,	ConsolidateInvDate,	ConsolidatedInvVolume,
ConsolidatedInvQuantity,	ConsolidatedMovVolume,	ConsolidatedMovQuantity,	OfficialDeltaMessageTypeId,	Version,	OwnerName,	OwnershipVolume,	OwnershipPercentage,	Scenario,	InputElement,	InputNode,	InputStartDate,	InputEndDate,	ExecutionId,	CreatedBy,	CreatedDate) 
VALUES('2020-07-16',10000002318,'CRUDO CAMPO MAMBO',137236,31855,'Automation_7meee',1,10000002372,'CRUDO CAMPO CUSUCO',30750,'Automation_06zr6',1,4,NULL,1,NULL,'31',11022,'Insert','EXCEL - OCENSA','SINOPER',3000.00,200.00,'2020-07-16',1,'2020-07-16',3000.00,
2000.00,3000.00,1000.00,4,1,'ECOPETROL',1000.00,0.50,'Oficial',137236,31855,'2020-07-03','2020-07-17','B5A8CF32-1665-467C-996F-C58FB13F86C6','ReportUser','2020-07-16')

Insert into [Admin].[ConsolidatedDeltaMovementInformation](OperationalDate,	SourceProductId,	SourceProductName,	SegmentID,	SourceNodeId,	SourceNodeName,	SourceNodeNameIsGeneric,	DestinationProductId,
DestinationProductName,	DestinationNodeId,	DestinationNodeName,	DestinationNodeNameIsGeneric,	MessageTypeId,	Classification,	MovementID,	MovementTypeName,	MeasurementUnit,	MovementTransactionId,	EventType,	SystemName,	SourceSystem,	DeltaNetStandardVolume,	DeltaGrossStandardVolume,	ExecutionDate,	ConsolidatedMovementId,	ConsolidateInvDate,	ConsolidatedInvVolume,
ConsolidatedInvQuantity,	ConsolidatedMovVolume,	ConsolidatedMovQuantity,	OfficialDeltaMessageTypeId,	Version,	OwnerName,	OwnershipVolume,	OwnershipPercentage,	Scenario,	InputElement,	InputNode,	InputStartDate,	InputEndDate,	ExecutionId,	CreatedBy,	CreatedDate) 
VALUES('2020-07-16',10000002318,'CRUDO CAMPO MAMBO',137236,31855,'Automation_7meee',1,10000002372,'CRUDO CAMPO CUSUCO',30750,'Automation_06zr6',1,3,NULL,1,NULL,'31',11022,'Insert','EXCEL - OCENSA','SINOPER',3000.00,200.00,'2020-07-16',1,'2020-07-16',3000.00,
2000.00,3000.00,1000.00,3,1,'ECOPETROL',1000.00,0.50,'Oficial',137236,31855,'2020-07-03','2020-07-17','B5A8CF32-1665-467C-996F-C58FB13F86C6','ReportUser','2020-07-16')

Insert into [Admin].[ConsolidatedDeltaMovementInformation](OperationalDate,	SourceProductId,	SourceProductName,	SegmentID,	SourceNodeId,	SourceNodeName,	SourceNodeNameIsGeneric,	DestinationProductId,
DestinationProductName,	DestinationNodeId,	DestinationNodeName,	DestinationNodeNameIsGeneric,	MessageTypeId,	Classification,	MovementID,	MovementTypeName,	MeasurementUnit,	MovementTransactionId,	EventType,	SystemName,	SourceSystem,	DeltaNetStandardVolume,	DeltaGrossStandardVolume,	ExecutionDate,	ConsolidatedMovementId,	ConsolidateInvDate,	ConsolidatedInvVolume,
ConsolidatedInvQuantity,	ConsolidatedMovVolume,	ConsolidatedMovQuantity,	OfficialDeltaMessageTypeId,	Version,	OwnerName,	OwnershipVolume,	OwnershipPercentage,	Scenario,	InputElement,	InputNode,	InputStartDate,	InputEndDate,	ExecutionId,	CreatedBy,	CreatedDate) 
VALUES('2020-07-16',10000002318,'CRUDO CAMPO MAMBO',137236,31855,'Automation_7meee',1,10000002372,'CRUDO CAMPO CUSUCO',30750,'Automation_06zr6',1,2,NULL,1,NULL,'31',11022,'Insert','EXCEL - OCENSA','SINOPER',3000.00,200.00,'2020-07-16',1,'2020-07-16',3000.00,
2000.00,3000.00,1000.00,2,1,'ECOPETROL',1000.00,0.50,'Oficial',137236,31855,'2020-07-03','2020-07-17','B5A8CF32-1665-467C-996F-C58FB13F86C6','ReportUser','2020-07-16')

Insert into [Admin].[ConsolidatedDeltaMovementInformation](OperationalDate,	SourceProductId,	SourceProductName,	SegmentID,	SourceNodeId,	SourceNodeName,	SourceNodeNameIsGeneric,	DestinationProductId,
DestinationProductName,	DestinationNodeId,	DestinationNodeName,	DestinationNodeNameIsGeneric,	MessageTypeId,	Classification,	MovementID,	MovementTypeName,	MeasurementUnit,	MovementTransactionId,	EventType,	SystemName,	SourceSystem,	DeltaNetStandardVolume,	DeltaGrossStandardVolume,	ExecutionDate,	ConsolidatedMovementId,	ConsolidateInvDate,	ConsolidatedInvVolume,
ConsolidatedInvQuantity,	ConsolidatedMovVolume,	ConsolidatedMovQuantity,	OfficialDeltaMessageTypeId,	Version,	OwnerName,	OwnershipVolume,	OwnershipPercentage,	Scenario,	InputElement,	InputNode,	InputStartDate,	InputEndDate,	ExecutionId,	CreatedBy,	CreatedDate) 
VALUES('2020-07-16',10000002318,'CRUDO CAMPO MAMBO',137236,31855,'Automation_7meee',1,10000002372,'CRUDO CAMPO CUSUCO',30750,'Automation_06zr6',1,1,NULL,1,NULL,'31',11022,'Insert','EXCEL - OCENSA','SINOPER',3000.00,200.00,'2020-07-16',1,'2020-07-16',3000.00,
2000.00,3000.00,1000.00,1,1,'ECOPETROL',1000.00,0.50,'Oficial',137236,31855,'2020-07-03','2020-07-17','B5A8CF32-1665-467C-996F-C58FB13F86C6','ReportUser','2020-07-16')

Insert into [Admin].[ConsolidatedDeltaMovementInformation](OperationalDate,	SourceProductId,	SourceProductName,	SegmentID,	SourceNodeId,	SourceNodeName,	SourceNodeNameIsGeneric,	DestinationProductId,
DestinationProductName,	DestinationNodeId,	DestinationNodeName,	DestinationNodeNameIsGeneric,	MessageTypeId,	Classification,	MovementID,	MovementTypeName,	MeasurementUnit,	MovementTransactionId,	EventType,	SystemName,	SourceSystem,	DeltaNetStandardVolume,	DeltaGrossStandardVolume,	ExecutionDate,	ConsolidatedMovementId,	ConsolidateInvDate,	ConsolidatedInvVolume,
ConsolidatedInvQuantity,	ConsolidatedMovVolume,	ConsolidatedMovQuantity,	OfficialDeltaMessageTypeId,	Version,	OwnerName,	OwnershipVolume,	OwnershipPercentage,	Scenario,	InputElement,	InputNode,	InputStartDate,	InputEndDate,	ExecutionId,	CreatedBy,	CreatedDate) 
VALUES('2020-07-16',10000002372,'CRUDO CAMPO CUSUCO',137236,30750,'Automation_06zr6',1,10000002318,'CRUDO CAMPO MAMBO',31855,'Automation_7meee',1,4,NULL,1,NULL,'31',11022,'Insert','EXCEL - OCENSA','SINOPER',3000.00,200.00,'2020-07-16',1,'2020-07-16',3000.00,
2000.00,3000.00,1000.00,4,1,'ECOPETROL',1000.00,0.50,'Oficial',137236,31855,'2020-07-03','2020-07-17','B5A8CF32-1665-467C-996F-C58FB13F86C6','ReportUser','2020-07-16')

Insert into [Admin].[ConsolidatedDeltaMovementInformation](OperationalDate,	SourceProductId,	SourceProductName,	SegmentID,	SourceNodeId,	SourceNodeName,	SourceNodeNameIsGeneric,	DestinationProductId,
DestinationProductName,	DestinationNodeId,	DestinationNodeName,	DestinationNodeNameIsGeneric,	MessageTypeId,	Classification,	MovementID,	MovementTypeName,	MeasurementUnit,	MovementTransactionId,	EventType,	SystemName,	SourceSystem,	DeltaNetStandardVolume,	DeltaGrossStandardVolume,	ExecutionDate,	ConsolidatedMovementId,	ConsolidateInvDate,	ConsolidatedInvVolume,
ConsolidatedInvQuantity,	ConsolidatedMovVolume,	ConsolidatedMovQuantity,	OfficialDeltaMessageTypeId,	Version,	OwnerName,	OwnershipVolume,	OwnershipPercentage,	Scenario,	InputElement,	InputNode,	InputStartDate,	InputEndDate,	ExecutionId,	CreatedBy,	CreatedDate) 
VALUES('2020-07-16',10000002372,'CRUDO CAMPO CUSUCO',137236,30750,'Automation_06zr6',1,10000002318,'CRUDO CAMPO MAMBO',31855,'Automation_7meee',1,3,NULL,1,NULL,'31',11022,'Insert','EXCEL - OCENSA','SINOPER',3000.00,200.00,'2020-07-16',1,'2020-07-16',3000.00,
2000.00,3000.00,1000.00,3,1,'ECOPETROL',1000.00,0.50,'Oficial',137236,31855,'2020-07-03','2020-07-17','B5A8CF32-1665-467C-996F-C58FB13F86C6','ReportUser','2020-07-16')

Insert into [Admin].[ConsolidatedDeltaMovementInformation](OperationalDate,	SourceProductId,	SourceProductName,	SegmentID,	SourceNodeId,	SourceNodeName,	SourceNodeNameIsGeneric,	DestinationProductId,
DestinationProductName,	DestinationNodeId,	DestinationNodeName,	DestinationNodeNameIsGeneric,	MessageTypeId,	Classification,	MovementID,	MovementTypeName,	MeasurementUnit,	MovementTransactionId,	EventType,	SystemName,	SourceSystem,	DeltaNetStandardVolume,	DeltaGrossStandardVolume,	ExecutionDate,	ConsolidatedMovementId,	ConsolidateInvDate,	ConsolidatedInvVolume,
ConsolidatedInvQuantity,	ConsolidatedMovVolume,	ConsolidatedMovQuantity,	OfficialDeltaMessageTypeId,	Version,	OwnerName,	OwnershipVolume,	OwnershipPercentage,	Scenario,	InputElement,	InputNode,	InputStartDate,	InputEndDate,	ExecutionId,	CreatedBy,	CreatedDate) 
VALUES('2020-07-16',10000002318,'CRUDO CAMPO MAMBO',137236,31855,'Automation_7meee',1,10000002318,'CRUDO CAMPO MAMBO',31855,'Automation_7meee',1,2,NULL,1,NULL,'31',11022,'Insert','EXCEL - OCENSA','SINOPER',3000.00,200.00,'2020-07-17',1,'2020-07-16',3000.00,
2000.00,3000.00,1000.00,2,1,'ECOPETROL',1000.00,0.50,'Oficial',137236,31855,'2020-07-03','2020-07-17','B5A8CF32-1665-467C-996F-C58FB13F86C6','ReportUser','2020-07-16')


Insert into [Admin].[ConsolidatedDeltaMovementInformation]
(OperationalDate,	SourceProductId,	SourceProductName,	SegmentID,	SourceNodeId,	SourceNodeName, 	SourceNodeNameIsGeneric,	DestinationProductId,DestinationProductName,	DestinationNodeId,	DestinationNodeName,	DestinationNodeNameIsGeneric,	MessageTypeId,	Classification,	MovementID,	MovementTypeName,	MeasurementUnit,	MovementTransactionId,	EventType,	SystemName,	    SourceSystem,	DeltaNetStandardVolume,	DeltaGrossStandardVolume,	ExecutionDate,	ConsolidatedMovementId,	ConsolidateInvDate,	ConsolidatedInvVolume,ConsolidatedInvQuantity,	ConsolidatedMovVolume,	ConsolidatedMovQuantity,	OfficialDeltaMessageTypeId,	Version,	OwnerName,	OwnershipVolume,	OwnershipPercentage,	Scenario,	InputElement,	InputNode,	InputStartDate,	InputEndDate,	ExecutionId,							CreatedBy,	CreatedDate) VALUES
('2020-07-16',      10000002318,        'CRUDO CAMPO MAMBO',137236,     31855,          'Automation_7meee',  1,                         10000002318,         'CRUDO CAMPO MAMBO',       31855,              'Automation_7meee',      1,                               1,             NULL,           1,            NULL,             '31',                  11022,              'Insert',  'EXCEL - OCENSA','SINOPER',      3000.00,                 200.00,                    '2020-07-16',      1,                   '2020-07-16',         3000.00,             2000.00,                  3000.00,                1000.00,                     1,                         1,          'ECOPETROL', 1000.00,           0.50,					'Oficial',	137236,			31855,		'2020-07-03',	'2020-07-17',	'B5A8CF32-1665-467C-996F-C58FB13F86C6','ReportUser','2020-07-16')


Insert into [Admin].[ConsolidatedDeltaMovementInformation]
(OperationalDate,	SourceProductId,	SourceProductName,	SegmentID,	SourceNodeId,	SourceNodeName, 	SourceNodeNameIsGeneric,	DestinationProductId,DestinationProductName,	DestinationNodeId,	DestinationNodeName,	DestinationNodeNameIsGeneric,	MessageTypeId,	Classification,	MovementID,	MovementTypeName,	MeasurementUnit,	MovementTransactionId,	EventType,	SystemName,	    SourceSystem,	DeltaNetStandardVolume,	DeltaGrossStandardVolume,	ExecutionDate,	ConsolidatedMovementId,	ConsolidateInvDate,	ConsolidatedInvVolume,ConsolidatedInvQuantity,	ConsolidatedMovVolume,	ConsolidatedMovQuantity,	OfficialDeltaMessageTypeId,	Version,	OwnerName,	OwnershipVolume,	OwnershipPercentage,	Scenario,	InputElement,	InputNode,	InputStartDate,	InputEndDate,	ExecutionId,							CreatedBy,	CreatedDate) VALUES
('2020-07-17',      10000002318,        'CRUDO CAMPO MAMBO',137236,     31855,          'Automation_7meee',  1,                         10000002318,         'CRUDO CAMPO MAMBO',       31855,              'Automation_7meee',      1,                               1,             NULL,           1,            NULL,             '31',                  11022,              'Insert',  'EXCEL - OCENSA','SINOPER',      3000.00,                 200.00,                    '2020-07-16',      1,                   '2020-07-16',         3000.00,             2000.00,                  3000.00,                1000.00,                     1,                         1,          'ECOPETROL', 1000.00,           0.50,					'Oficial',	137236,			31855,		'2020-07-03',	'2020-07-17',	'B5A8CF32-1665-467C-996F-C58FB13F86C6','ReportUser','2020-07-16')


Insert into [Admin].[ConsolidatedDeltaMovementInformation]
(OperationalDate,	SourceProductId,	SourceProductName,	SegmentID,	SourceNodeId,	SourceNodeName, 	SourceNodeNameIsGeneric,	DestinationProductId,DestinationProductName,	DestinationNodeId,	DestinationNodeName,	DestinationNodeNameIsGeneric,	MessageTypeId,	Classification,	MovementID,	MovementTypeName,	MeasurementUnit,	MovementTransactionId,	EventType,	SystemName,	    SourceSystem,	DeltaNetStandardVolume,	DeltaGrossStandardVolume,	ExecutionDate,	ConsolidatedMovementId,	ConsolidateInvDate,	ConsolidatedInvVolume,ConsolidatedInvQuantity,	ConsolidatedMovVolume,	ConsolidatedMovQuantity,	OfficialDeltaMessageTypeId,	Version,	OwnerName,	OwnershipVolume,	OwnershipPercentage,	Scenario,	InputElement,	InputNode,	InputStartDate,	InputEndDate,	ExecutionId,							CreatedBy,	CreatedDate) VALUES
('2020-07-02',      10000002318,        'CRUDO CAMPO MAMBO',137236,     31855,          'Automation_7meee',  1,                         10000002318,         'CRUDO CAMPO MAMBO',       31855,              'Automation_7meee',      1,                               2,             NULL,           1,            NULL,             '31',                  11022,              'Insert',  'EXCEL - OCENSA','SINOPER',      3000.00,                 200.00,                    '2020-07-02',      1,                   '2020-07-16',         3000.00,             2000.00,                  3000.00,                1000.00,                     2,                         1,          'ECOPETROL', 1000.00,           0.50,					'Oficial',	137236,			31855,		'2020-07-03',	'2020-07-17',	'B5A8CF32-1665-467C-996F-C58FB13F86C6','ReportUser','2020-07-16')

Insert into [Admin].[ConsolidatedDeltaMovementInformation]
(OperationalDate,	SourceProductId,	SourceProductName,	SegmentID,	SourceNodeId,	SourceNodeName, 	SourceNodeNameIsGeneric,	DestinationProductId,DestinationProductName,	DestinationNodeId,	DestinationNodeName,	DestinationNodeNameIsGeneric,	MessageTypeId,	Classification,	MovementID,	MovementTypeName,	MeasurementUnit,	MovementTransactionId,	EventType,	SystemName,	    SourceSystem,	DeltaNetStandardVolume,	DeltaGrossStandardVolume,	ExecutionDate,	ConsolidatedMovementId,	ConsolidateInvDate,	ConsolidatedInvVolume,ConsolidatedInvQuantity,	ConsolidatedMovVolume,	ConsolidatedMovQuantity,	OfficialDeltaMessageTypeId,	Version,	OwnerName,	OwnershipVolume,	OwnershipPercentage,	Scenario,	InputElement,	InputNode,	InputStartDate,	InputEndDate,	ExecutionId,							CreatedBy,	CreatedDate) VALUES
('2020-07-02',      10000002318,        'CRUDO CAMPO MAMBO',137236,     31855,          'Automation_7meee',  1,                         10000002318,         'CRUDO CAMPO MAMBO',       31855,              'Automation_7meee',      1,                               1,             NULL,           1,            NULL,             '31',                  11022,              'Insert',  'EXCEL - OCENSA','SINOPER',      3000.00,                 200.00,                    '2020-07-02',      1,                   '2020-07-16',         3000.00,             2000.00,                  3000.00,                1000.00,                     1,                         1,          'ECOPETROL', 1000.00,           0.50,					'Oficial',	137236,			31855,		'2020-07-03',	'2020-07-17',	'B5A8CF32-1665-467C-996F-C58FB13F86C6','ReportUser','2020-07-16')



   

--######################################################################################################################################################################################################################################################################################################################################



--===================== TestCase1: To check if the movementary owner table returns data for Segments within a given date range ====================================

EXEC [Admin].[usp_SaveMonthlyConsolidatedDeltaData]
	  @ElementId        =  137236
	 ,@NodeId         =  31855
	 ,@StartDate      =   '2020-07-03'   
	 ,@EndDate        =    '2020-07-17'  
	 ,@ExecutionId    =  'B5A8CF32-1665-467C-996F-C58FB13F86C6'
   SELECT * FROM [Admin].[ConsolidatedDeltaBalance]

--========================================= Output Captured ====================================================================================================
/*
RNo	Inventories	        Product	           Owner	MeasurementUnit	Values	InputElementId	InputNodeId	InputStartDate	InputEndDate	ExecutionId	CreatedBy	CreatedDate

2	Delta inv. inicial	CRUDO CAMPO MAMBO	ECOPETROL	31	3000.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
4	Delta entradas	    CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
6	Delta salidas	    CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
8	Delta inv. final	CRUDO CAMPO MAMBO	ECOPETROL	31	3000.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
1	Inv. inicial	    CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
3	Entradas	        CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
5	Salidas	            CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
7	Inv. final	        CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
9	Control	            CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
2	Delta inv. inicial	CRUDO CAMPO MAMBO	ECOPETROL	31	3000.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
4	Delta entradas	    CRUDO CAMPO MAMBO	ECOPETROL	31	3000.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
6	Delta salidas	    CRUDO CAMPO MAMBO	ECOPETROL	31	3000.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
8	Delta inv. final	CRUDO CAMPO MAMBO	ECOPETROL	31	3000.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
1	Inv. inicial		CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
3	Entradas			CRUDO CAMPO MAMBO	ECOPETROL	31	3000.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
5	Salidas				CRUDO CAMPO MAMBO	ECOPETROL	31	3000.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
7	Inv. final			CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
9	Control				CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
2	Delta inv. inicial	CRUDO CAMPO MAMBO	ECOPETROL	31	3000.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
4	Delta entradas		CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
6	Delta salidas		CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
8	Delta inv. final	CRUDO CAMPO MAMBO	ECOPETROL	31	3000.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
1	Inv. inicial		CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
3	Entradas			CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
5	Salidas				CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
7	Inv. final			CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
9	Control				CRUDO CAMPO MAMBO	ECOPETROL	31	0.00	137236	31855	2020-07-03	2020-07-17	B5A8CF32-1665-467C-996F-C58FB13F86C6	ReportUser	2020-07-16 04:15:30.827
