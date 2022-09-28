/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Jul-17-2020
-- Description:     These test cases are for [usp_SaveMonthlyConsolidatedDeltaData] SP.
	EXEC [Admin].[usp_SaveMonthlyConsolidatedDeltaDataWithoutCutOffReport] @ElementId   = 137236
                                                                    ,@NodeId      = 31855
                                                                    ,@StartDate   = '2020-06-01'                    
                                                                    ,@EndDate     = '2020-06-30'                    
                                                                    ,@ExecutionId = 'B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF'

   SELECT * FROM [Admin].[ConsolidatedDeltaMovementInformation] WHERE Executionid = 'B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF1'
   SELECT * FROM [Admin].[ConsolidatedNodeTagCalculationDate] WHERE Executionid = 'B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF1'
-- Database backup Used:	dbaeuecpdevtrue
-- ==============================================================================================================================*/

--===================================  DATA FOR [OffChain].[Movement]=================================================================================================================================================================================================================================================

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	SourceSystem,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	SystemName,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'SINOPER',  	'Insert',    	1,		42,			23727,			137236, 	'2020-05-31',   	200.00,                 3000.00,				0.22,					31	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			6313,							23727,			'EXCEL - OCENSA',		NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				1,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	SourceSystem,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	SystemName,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'SINOPER',  	'Insert',    	1,		42,			23727,			137236, 	'2020-05-31',   	200.00,                 3000.00,				0.22,					31	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			6313,							23727,			'EXCEL - OCENSA',		NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				2,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)


INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	SourceSystem,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	SystemName,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'SINOPER',  	'Insert',    	1,		42,			23727,			137236, 	'2020-06-02',   	200.00,                 3000.00,				0.22,					31	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			6313,							23727,			'EXCEL - OCENSA',		NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				3,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)


INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	SourceSystem,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	SystemName,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'SINOPER',  	'Insert',    	1,		42,			23727,			137236, 	'2020-06-15',   	200.00,                 3000.00,				0.22,					31	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			6313,							23727,			'EXCEL - OCENSA',		NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				4,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)


INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	SourceSystem,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	SystemName,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'SINOPER',  	'Insert',    	1,		42,			23727,			137236, 	'2020-06-30',   	200.00,                 3000.00,				0.22,					31	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			6313,							23727,			'EXCEL - OCENSA',		NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				1,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	SourceSystem,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	SystemName,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'SINOPER',  	'Insert',    	1,		42,			23727,			137236, 	'2020-06-30',   	200.00,                 3000.00,				0.22,					31	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			6313,							23727,			'EXCEL - OCENSA',		NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				2,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)


INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	SourceSystem,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	SystemName,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'SINOPER',  	'Insert',    	1,		42,			23727,			137236, 	'2020-06-02',   	200.00,                 3000.00,				0.22,					31	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			6313,							23727,			'EXCEL - OCENSA',		NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				3,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)


INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	SourceSystem,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	SystemName,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'SINOPER',  	'Insert',    	1,		42,			23727,			137236, 	'2020-06-15',   	200.00,                 3000.00,				0.22,					31	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			6313,							23727,			'EXCEL - OCENSA',		NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				4,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)



--===================================  DATA FOR [Offchain].[MovementSource], [Offchain].[MovementDestination] =================================================================================================================================================================================================================================================

Insert into Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(24659,			31855,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


Insert into Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(24659,	30750,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)


Insert into Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(24660,			31855,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


Insert into Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(24660,	30750,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)

Insert into Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(24661,			31855,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


Insert into Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(24661,	30750,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)


Insert into Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(24662,			31855,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


Insert into Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(24662,	30750,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)



Insert into Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(24663,			31855,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


Insert into Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(24663,		30750,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)

Insert into Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(24664,			31855,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


Insert into Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(24664,	30750,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)


Insert into Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(24665,			30750,				NULL,					10000002372,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


Insert into Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(24665,	31855,                       NULL,								10000002318,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)

Insert into Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(24666,			30750,				NULL,					10000002372,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


Insert into Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(24666,	31855,                       NULL,								10000002318,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)


--===================================  DATA FOR [Admin].[ConsolidatedMovement] =================================================================================================================================================================================================================================================


Insert into Admin.ConsolidatedMovement
(SourceNodeId,	SourceProductId,	DestinationNodeId,	DestinationProductId,	MovementTypeId,	StartDate,		EndDate,	NetStandardVolume,	GrossStandardVolume,	MeasurementUnit,	TicketId,	SegmentId,	SourceSystemId,	ExecutionDate,	IsActive,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(	31855,	10000002318,		30750,				10000002372,				156,		'2020-06-26','2020-06-30',	20000.00,			20000.00,				31,					26921,		137236,		1,				'2020-07-10',	1,			'System',	'2020-07-17',	NULL,			NULL)


Insert into Admin.ConsolidatedMovement
(SourceNodeId,	SourceProductId,	DestinationNodeId,	DestinationProductId,	MovementTypeId,	StartDate,		EndDate,	NetStandardVolume,	GrossStandardVolume,	MeasurementUnit,	TicketId,	SegmentId,	SourceSystemId,	ExecutionDate,	IsActive,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(	31855,	10000002318,		30750,				10000002372,				156,		'2020-06-26','2020-06-30',	20000.00,			20000.00,				31,					26921,		137236,		1,				'2020-07-10',	1,			'System',	'2020-07-17',	NULL,			NULL)


Insert into Admin.ConsolidatedMovement
(SourceNodeId,	SourceProductId,	DestinationNodeId,	DestinationProductId,	MovementTypeId,	StartDate,		EndDate,	NetStandardVolume,	GrossStandardVolume,	MeasurementUnit,	TicketId,	SegmentId,	SourceSystemId,	ExecutionDate,	IsActive,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(	31855,	10000002318,		30750,				10000002372,				156,		'2020-06-26','2020-06-30',	20000.00,			20000.00,				31,					26921,		137236,		1,				'2020-07-10',	1,			'System',	'2020-07-17',	NULL,			NULL)

Insert into Admin.ConsolidatedMovement
(SourceNodeId,	SourceProductId,	DestinationNodeId,	DestinationProductId,	MovementTypeId,	StartDate,		EndDate,	NetStandardVolume,	GrossStandardVolume,	MeasurementUnit,	TicketId,	SegmentId,	SourceSystemId,	ExecutionDate,	IsActive,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(	31855,	10000002318,		30750,				10000002372,				156,		'2020-06-26','2020-06-30',	20000.00,			20000.00,				31,					26921,		137236,		1,				'2020-07-10',	1,			'System',	'2020-07-17',	NULL,			NULL)


--===================================  DATA FOR [Admin].[ConsolidatedMovement] =================================================================================================================================================================================================================================================

-- Updating Movement's ConsolidatedMovementTransactionId so that we can join it with ConsolidatedMOvement
Update Offchain.Movement set ConsolidatedMovementTransactionId = 29 where MovementTransactionId = 24659

Update Offchain.Movement set ConsolidatedMovementTransactionId = 30 where MovementTransactionId = 24660

Update Offchain.Movement set ConsolidatedMovementTransactionId = 31 where MovementTransactionId = 24661

Update Offchain.Movement set ConsolidatedMovementTransactionId = 32 where MovementTransactionId = 24662

Update Offchain.Movement set ConsolidatedMovementTransactionId = 29 where MovementTransactionId = 24663

Update Offchain.Movement set ConsolidatedMovementTransactionId = 29 where MovementTransactionId = 24664

Update Offchain.Movement set ConsolidatedMovementTransactionId = 29 where MovementTransactionId = 24665

Update Offchain.Movement set ConsolidatedMovementTransactionId = 29 where MovementTransactionId = 24666

--===================================  DATA FOR [Admin].[ConsolidatedOwner] =================================================================================================================================================================================================================================================
--Insertion into Consolidated Owner table

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,29,NULL,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,30,NULL,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,31,NULL,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,32,NULL,'System','2020-07-07',NULL,NULL)

--===================================  DATA FOR [Admin].[NodeTag] =================================================================================================================================================================================================================================================
Insert into Admin.NodeTag 
(	NodeId,	ElementId,	StartDate,	EndDate,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES
(31855,137236,'2020-06-02','9999-12-31 00:00:00.000','System','2020-07-17',NULL,NULL)

--######################################################################################################################################################################################################################################################################################################################################



--===================== TestCase1: To check if the movementary owner table returns data for Segments within a given date range ====================================

EXEC [Admin].[usp_SaveMonthlyConsolidatedDeltaDataWithoutCutOffReport] @ElementId   = 137236
                                                                    ,@NodeId      = 31855
                                                                    ,@StartDate   = '2020-06-01'                    
                                                                    ,@EndDate     = '2020-06-30'                    
                                                                    ,@ExecutionId = 'B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF'

   SELECT * FROM [Admin].[ConsolidatedDeltaMovementInformation] 
   SELECT * FROM [Admin].[ConsolidatedNodeTagCalculationDate]

--========================================= Output Captured ====================================================================================================
/* Output for [Admin].[ConsolidatedDeltaMovementInformation]
OperationalDate	SourceProductId	SourceProductName	SegmentID	SourceNodeId	SourceNodeName	SourceNodeNameIsGeneric	DestinationProductId	DestinationProductName	DestinationNodeId	DestinationNodeName	DestinationNodeNameIsGeneric	MessageTypeId	Classification	MovementID	MovementTypeName		MeasurementUnit	MovementTransactionId	EventType	SystemName		SourceSystem	NetStandardVolume	GrossStandardVolume	ConsolidatedMovementId	ConsolidatedMovVolume	ConsolidatedMovQuantity	OfficialDeltaMessageTypeId	Version	OwnerName	OwnershipVolume	OwnershipPercentage	Scenario	InputElement	InputNode	InputStartDate	InputEndDate	ExecutionId	CreatedBy	CreatedDate

2020-05-31		10000002318		CRUDO CAMPO MAMBO	137236			31855	Automation_7meee	0							10000002372				CRUDO CAMPO CUSUCO	30750				Automation_06zr6		0								1				Movimiento		1		Traslado de productos	Bbl				24659					Insert		EXCEL - OCENSA	Facilidad		3000.00				200.00					29						20000.00				20000.00				1							1	ECOPETROL	4000.00				0.40				Oficial		137236		31855			2020-06-01	2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
2020-05-31		10000002318		CRUDO CAMPO MAMBO	137236			31855	Automation_7meee	0							10000002372				CRUDO CAMPO CUSUCO	30750				Automation_06zr6		0								2				Movimiento		1		Traslado de productos	Bbl				24660					Insert		EXCEL - OCENSA	Facilidad		3000.00				200.00					30						20000.00				20000.00				2							1	ECOPETROL	4000.00				0.40				Oficial		137236		31855			2020-06-01	2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
2020-06-02		10000002372		CRUDO CAMPO CUSUCO	137236			30750	Automation_06zr6	0							10000002318				CRUDO CAMPO MAMBO	31855				Automation_7meee		0								3				Movimiento		1		Traslado de productos	Bbl				24665					Insert		EXCEL - OCENSA	Facilidad		3000.00				200.00					29						20000.00				20000.00				3							1	ECOPETROL	4000.00				0.40				Oficial		137236		31855			2020-06-01	2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
2020-06-15		10000002318		CRUDO CAMPO MAMBO	137236			31855	Automation_7meee	0							10000002372				CRUDO CAMPO CUSUCO	30750				Automation_06zr6		0								3				Movimiento		1		Traslado de productos	Bbl				24661					Insert		EXCEL - OCENSA	Facilidad		3000.00				200.00					31						20000.00				20000.00				3							1	ECOPETROL	4000.00				0.40				Oficial		137236		31855			2020-06-01	2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
2020-06-15		10000002318		CRUDO CAMPO MAMBO	137236			31855	Automation_7meee	0							10000002372				CRUDO CAMPO CUSUCO	30750				Automation_06zr6		0								4				Movimiento		1		Traslado de productos	Bbl				24662					Insert		EXCEL - OCENSA	Facilidad		3000.00				200.00					32						20000.00				20000.00				4							1	ECOPETROL	4000.00				0.40				Oficial		137236		31855			2020-06-01	2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
2020-06-15		10000002372		CRUDO CAMPO CUSUCO	137236			30750	Automation_06zr6	0							10000002318				CRUDO CAMPO MAMBO	31855				Automation_7meee		0								4				Movimiento		1		Traslado de productos	Bbl				24666					Insert		EXCEL - OCENSA	Facilidad		3000.00				200.00					29						20000.00				20000.00				4							1	ECOPETROL	4000.00				0.40				Oficial		137236		31855			2020-06-01	2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
2020-06-30		10000002318		CRUDO CAMPO MAMBO	137236			31855	Automation_7meee	0							10000002372				CRUDO CAMPO CUSUCO	30750				Automation_06zr6		0								1				Movimiento		1		Traslado de productos	Bbl				24663					Insert		EXCEL - OCENSA	Facilidad		3000.00				200.00					29						20000.00				20000.00				1							1	ECOPETROL	4000.00				0.40				Oficial		137236		31855			2020-06-01	2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
2020-06-30		10000002318		CRUDO CAMPO MAMBO	137236			31855	Automation_7meee	0							10000002372				CRUDO CAMPO CUSUCO	30750				Automation_06zr6		0								2				Movimiento		1		Traslado de productos	Bbl				24664					Insert		EXCEL - OCENSA	Facilidad		3000.00				200.00					29						20000.00				20000.00				2							1	ECOPETROL	4000.00				0.40				Oficial		137236		31855			2020-06-01	2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000

/* Output for [Admin].[ConsolidatedNodeTagCalculationDate]
NodeId	ElementId	ElementName			CalculationDate	InputElement	InputNode	InputStartDate	InputEndDate	ExecutionId	CreatedBy	CreatedDate
31855	137236		Automation_vbzgj	2020-06-02			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-03			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-04			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-05			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-06			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-07			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-08			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-09			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-10			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-11			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-12			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-13			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-14			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-15			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-16			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-17			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-18			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-19			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-20			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-21			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-22			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-23			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-24			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-25			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-26			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-27			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-28			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-29			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000
31855	137236		Automation_vbzgj	2020-06-30			137236			31855	2020-06-01		2020-06-30	B9E667B0-0EF0-4ACD-BAB6-C8FE92B6A7EF	System	2020-07-17 00:00:00.000