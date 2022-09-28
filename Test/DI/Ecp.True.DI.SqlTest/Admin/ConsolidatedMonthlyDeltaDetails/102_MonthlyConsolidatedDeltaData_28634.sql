/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Aug-05-2020
-- Description:     This data can be used to test PBI-31881 report (Monthly Consolidate Delta Report)
-- ==============================================================================================================================*/

DECLARE @ElementName VARCHAR (50) = 'Automation_E029i'
       ,@NodeName VARCHAR (50)    = 'Automation_N029i'
	   ,@TicketId INT 
       ,@ElementId INT
       ,@NodeId INT
	   ,@OtherNodeId INT = (SELECT TOP 1 NodeId FROM [Admin].[Node])
	   ,@MTranId_1 INT
	   ,@MTranId_2 INT
	   ,@MTranId_3 INT
	   ,@MTranId_4 INT
	   ,@MTranId_5 INT
	   ,@MTranId_6 INT
	   ,@MTranId_7 INT
	   ,@MTranId_8 INT
	   ,@Con_Mov_1 INT
	   ,@Con_Mov_2 INT
	   ,@Con_Mov_3 INT
	   ,@Con_Mov_4 INT
	   ,@MeasurementUnit INT
	   ,@InvProdId_1 INT
	   ,@InvProdId_2 INT
	   ,@InvProdId_3 INT
	   ,@InvProdId_4 INT
	   ,@InvProdId_5 INT
	   ,@InvProdId_6 INT
	   ,@InvProdId_7 INT
	   ,@InvProdId_8 INT
	   ,@FileRegistrationTransactionId INT = (SELECT TOP 1 FileRegistrationTransactionId FROM [Admin].[FileRegistrationTransaction])	

 
	--SELECT DISTINCT FileRegistrationTransactionId  FROM [Offchain].[Movement]   
/*===================================  DATA FOR [Admin].[CategoryElement] ===============================================================================================*/
INSERT INTO [Admin].[CategoryElement] ([Name],[Description],CategoryId,IsActive,IsOperationalSegment,CreatedBy) VALUES (@ElementName,'CategoryElementAutomation',2,1,1,'User')
SELECT @ElementId = SCOPE_IDENTITY()

/*===================================  DATA FOR [Admin].[NodeTag] =======================================================================================================*/
INSERT INTO [Admin].[Node] ([Name],[Description],LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],NodeOwnershipRuleId,CreatedBy)
                    VALUES (@NodeName,'NodeOne',4033,1,0,'1.96','0.10',875345,1,'User')
SELECT @NodeId = SCOPE_IDENTITY()
/*===================================  DATA FOR [Admin].[NodeTag] =======================================================================================================*/
INSERT INTO [Admin].[NodeTag] 
(	NodeId,	ElementId,	StartDate,	EndDate,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES
(@NodeId,@ElementId,'2020-06-02','9999-12-31 00:00:00.000','System','2020-07-17',NULL,NULL)

/*===================================  DATA FOR [Admin].[Ticket] =======================================================================================================*/
INSERT INTO [Admin].Ticket
 (CategoryElementId,    StartDate,                    EndDate,                        Status,    TicketTypeId,    TicketGroupId,    OwnerId,    NodeId,    CreatedBy,    CreatedDate)                    
VALUES    ( @ElementId,        '2020-06-04 08:19:48.907',    '2020-06-14 08:19:48.907',        4,        5,                NULL,            NULL,    @NodeId,    'Demo',        GETDATE()    )
SELECT @TicketId = SCOPE_IDENTITY()

/*************************************************** START OF BBL EELEMENT ************************************************************/
SET @MeasurementUnit = 31

/*===================================  DATA FOR [OffChain].[Movement] ====================================================================================================*/

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,		     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			@TicketId,			@ElementId, 	'2020-05-31',   	200.00,                 3000.00,				0.22,		@MeasurementUnit	,2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,		@TicketId,		       NULL,	NULL,		  NULL,				       1,	        '6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,					NULL,		NULL,	NULL,					       12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			189,				1,				NULL,							NULL,					NULL,			NULL,					1,				1,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)

SELECT @MTranId_1 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	   IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			@TicketId,			@ElementId, 	'2020-06-01',   	200.00,                 3000.00,				0.22,			@MeasurementUnit	,2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,	@TicketId,					NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			190,		1,			NULL,			1,			190,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				3,								NULL,									NULL,					'Demo',	'2020-07-17' ,	NULL,	     NULL)
SELECT @MTranId_2 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	     OwnershipTicketId,	    ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			   @TicketId,	@ElementId, 	'2020-06-05',   	200.00,                 3000.00,				0.22,			@MeasurementUnit	,2,		'Reporte Operativo Cusiana -Fecha'	,    'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,	   @TicketId,			   NULL,	NULL,		  NULL,				    1,       	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,						NULL,		NULL,	     NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			190,		1,			NULL,			1,			190,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				3,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)
SELECT @MTranId_3 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,		EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			@TicketId,			@ElementId, 	'2020-06-10',   	200.00,                 3000.00,				0.22,					@MeasurementUnit	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,							@TicketId,					NULL,	NULL,NULL,	1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,		NULL,		NULL,	    NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			190,		1,			NULL,			1,			190,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				4,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)
SELECT @MTranId_4 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	       OwnershipTicketId,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			   @TicketId,	@ElementId, 	'2020-06-15',   	200.00,                 3000.00,				0.22,			@MeasurementUnit	,2,		'Reporte Operativo Cusiana -Fecha'	,   'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,		@TicketId,					NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			190,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				1,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)
SELECT @MTranId_5 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			@TicketId,			@ElementId, 	'2020-06-30',   	200.00,                 3000.00,				0.22,			@MeasurementUnit	,2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,		@TicketId,		NULL,	     NULL,		  NULL,				1,	        '6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,								NULL,		NULL,	 NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			189,				1,				NULL,							NULL,					      NULL,			   NULL,					NULL,				2,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)
SELECT @MTranId_6 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,		     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			    @TicketId,	@ElementId, 	'2020-06-01',   	200.00,                 3000.00,				0.22,			@MeasurementUnit	,	2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,	@TicketId,					NULL,	NULL,		  NULL,				1,             	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,							NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			190,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				3,	                          NULL,	                            NULL,                            'Demo',     '2020-07-17' ,	  NULL,	        NULL)
SELECT @MTranId_7 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			@TicketId,			@ElementId, 	'2020-06-30',   	200.00,                 3000.00,				0.22,					@MeasurementUnit	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,							@TicketId,				NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		190,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				4,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)
SELECT @MTranId_8 = SCOPE_IDENTITY()

--===================================  DATA FOR [Offchain].[MovementSource], [Offchain].[MovementDestination] =================================================================================================================================================================================================================================================

INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_1,			@NodeId,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_1,	@OtherNodeId,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)


INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_2,			@NodeId,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_2,	@OtherNodeId,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)

INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_3,			@NodeId,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_3,	@OtherNodeId,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)


INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_4,			@NodeId,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_4,	@OtherNodeId,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)



INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_5,			@NodeId,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_5,		@OtherNodeId,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)

INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_6,			@NodeId,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_6,	@OtherNodeId,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)


INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_7,			@OtherNodeId,				NULL,					10000002372,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_7,	@NodeId,                       NULL,								10000002318,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)

INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_8,			@OtherNodeId,				NULL,					10000002372,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_8,	@NodeId,                       NULL,								10000002318,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)

/*===================================  DATA FOR [Admin].[ConsolidatedMovement] ==============================================================================================*/

-- SELECT * FROM [Admin].ConsolidatedMovement
INSERT INTO [Admin].ConsolidatedMovement
(SourceNodeId,	SourceProductId,	DestinationNodeId,	DestinationProductId,	MovementTypeId,	StartDate,		EndDate,	NetStandardVolume,	GrossStandardVolume,	MeasurementUnit,	TicketId,	SegmentId,	SourceSystemId,	ExecutionDate,	IsActive,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(	@NodeId,	10000002318,		@OtherNodeId,				10000002372,				156,		'2020-06-26','2020-06-30',	20000.00,			20000.00,				@MeasurementUnit,					@TicketId,		@ElementId,		1,				'2020-07-10',	1,			'System',	'2020-07-17',	NULL,			NULL)
SELECT @Con_Mov_1 = SCOPE_IDENTITY()

INSERT INTO [Admin].ConsolidatedMovement
(SourceNodeId,	SourceProductId,	DestinationNodeId,	DestinationProductId,	MovementTypeId,	StartDate,		EndDate,	NetStandardVolume,	GrossStandardVolume,	MeasurementUnit,	TicketId,	SegmentId,	SourceSystemId,	ExecutionDate,	IsActive,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(	@NodeId,	10000002318,		@OtherNodeId,				10000002372,				156,		'2020-06-26','2020-06-30',	20000.00,			20000.00,				@MeasurementUnit,					@TicketId,		@ElementId,		1,				'2020-07-10',	1,			'System',	'2020-07-17',	NULL,			NULL)
SELECT @Con_Mov_2 = SCOPE_IDENTITY()

INSERT INTO [Admin].ConsolidatedMovement
(SourceNodeId,	SourceProductId,	DestinationNodeId,	DestinationProductId,	MovementTypeId,	StartDate,		EndDate,	NetStandardVolume,	GrossStandardVolume,	MeasurementUnit,	TicketId,	SegmentId,	SourceSystemId,	ExecutionDate,	IsActive,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(	@OtherNodeId,	10000002318,		@NodeId,				10000002372,				156,		'2020-06-26','2020-06-30',	20000.00,			20000.00,				@MeasurementUnit,					@TicketId,		@ElementId,		1,				'2020-07-10',	1,			'System',	'2020-07-17',	NULL,			NULL)
SELECT @Con_Mov_3 = SCOPE_IDENTITY()

INSERT INTO [Admin].ConsolidatedMovement
(SourceNodeId,	SourceProductId,	DestinationNodeId,	DestinationProductId,	MovementTypeId,	StartDate,		EndDate,	NetStandardVolume,	GrossStandardVolume,	MeasurementUnit,	TicketId,	SegmentId,	SourceSystemId,	ExecutionDate,	IsActive,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(	@OtherNodeId,	10000002318,		@NodeId,				10000002372,				156,		'2020-06-26','2020-06-30',	20000.00,			20000.00,				@MeasurementUnit,					@TicketId,		@ElementId,		1,				'2020-07-10',	1,			'System',	'2020-07-17',	NULL,			NULL)
SELECT @Con_Mov_4 = SCOPE_IDENTITY()

/*===================================  DATA FOR [Admin].[ConsolidatedMovement] ========================================================================================*/
-- SELECT * FROM [Admin].[ConsolidatedMovement]
-- SELECT * FROM [Offchain].[Movement] WHERE ConsolidatedMovementTransactionId IS NOT NULL
UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_1 WHERE MovementTransactionId = @MTranId_1

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_2 WHERE MovementTransactionId = @MTranId_2

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_3 WHERE MovementTransactionId = @MTranId_3

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_4 WHERE MovementTransactionId = @MTranId_4

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_1 WHERE MovementTransactionId = @MTranId_5

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_1 WHERE MovementTransactionId = @MTranId_6

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_2 WHERE MovementTransactionId = @MTranId_7

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_4 WHERE MovementTransactionId = @MTranId_8

--===================================  DATA FOR [Admin].[ConsolidatedOwner] =================================================================================================================================================================================================================================================
--Insertion into Consolidated Owner table

INSERT INTO [Admin].ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,@Con_Mov_1,NULL,'System','2020-07-07',NULL,NULL)

INSERT INTO [Admin].ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,@Con_Mov_2,NULL,'System','2020-07-07',NULL,NULL)

INSERT INTO [Admin].ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,@Con_Mov_3,NULL,'System','2020-07-07',NULL,NULL)

INSERT INTO [Admin].ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,@Con_Mov_4,NULL,'System','2020-07-07',NULL,NULL)

/*===================================  DATA FOR [Admin].[ConsolidatedInventoryProduct]===============================================================================*/

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002318,	'2020-06-30 00:00:00.000',	100000.00,	120000.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_1 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002318,	'2020-05-31 00:00:00.000',	110000.00,	120000.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_2 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002318,	'2020-06-30 00:00:00.000',	120000.00,	120000.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_3 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002372,	'2020-06-10 00:00:00.000',	130000.00,	120000.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_4 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002372,	'2020-06-14 00:00:00.000',	140000.00,	120000.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_5 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002372,	'2020-06-15 00:00:00.000',	150000.00,	120000.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_6 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002318,	'2020-06-30 00:00:00.000',	160000.00,	120000.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_7 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002372,	'2020-05-31 00:00:00.000',	170000.00,	120000.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_8 = SCOPE_IDENTITY()

/*=================================== DATA FOR  ADMIN.ConsolidatedOwner ==============================================================================================*/

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,@InvProdId_1,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,@InvProdId_1,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,@InvProdId_2,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,@InvProdId_2,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,@InvProdId_3,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,@InvProdId_3,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,@InvProdId_4,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,@InvProdId_4,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,@InvProdId_5,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,@InvProdId_5,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,@InvProdId_6,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,@InvProdId_6,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,@InvProdId_7,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,@InvProdId_7,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,4000.00,0.40,NULL,@InvProdId_8,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,@InvProdId_8,'System','2020-07-07',NULL,NULL)

/*************************************************** END OF BBL EELEMENT ************************************************************/



/*************************************************** START OF KG EELEMENT ************************************************************/
SET @MeasurementUnit = 147
/*===================================  DATA FOR [OffChain].[Movement] ====================================================================================================*/

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	       OwnershipTicketId,	    ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		  42,			@TicketId,	@ElementId, 	'2020-05-31',   	200.00,                 1500.00,				0.22,			@MeasurementUnit	,	2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	  0,				NULL,				NULL,			@FileRegistrationTransactionId,		@TicketId,			NULL,	   NULL,		  NULL,				  1,	    '6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,								NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			189,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				1,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)
SELECT @MTranId_1 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,		     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			@TicketId,			@ElementId, 	'2020-06-05',   	100.00,                 1700.00,				0.22,					@MeasurementUnit	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,							@TicketId,					NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			190,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				2,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)
SELECT @MTranId_2 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,		     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			@TicketId,			@ElementId, 	'2020-06-01',   	200.00,                 30.00,				0.22,					@MeasurementUnit	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,							@TicketId,					NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			190,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				3,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)
SELECT @MTranId_3 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			@TicketId,			@ElementId, 	'2020-06-15',   	980.00,                 5700.00,				0.22,					@MeasurementUnit	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,							@TicketId,					NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			190,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				4,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)
SELECT @MTranId_4 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			@TicketId,			@ElementId, 	'2020-06-30',   	270.00,                 6880.00,				0.22,					@MeasurementUnit	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,							@TicketId,					NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			190,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				1,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)
SELECT @MTranId_5 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			@TicketId,			@ElementId, 	'2020-06-30',   	100.00,                 9876.00,				0.22,					@MeasurementUnit	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,							@TicketId,					NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			189,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				2,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)
SELECT @MTranId_6 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	    ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			@TicketId,			@ElementId, 	'2020-06-02',   	167.00,                 5470.00,				0.22,					@MeasurementUnit	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,							@TicketId,				NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			190,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				3,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)
SELECT @MTranId_7 = SCOPE_IDENTITY()

INSERT INTO OFFCHAIN.Movement
(			MessageTypeId,	SystemTypeId,	EventType,	MovementId,	MovementTypeId,	TicketId,	SegmentId,	OperationalDate,	GrossStandardVolume,	NetStandardVolume,	UncertaintyPercentage,	MeasurementUnit,	ScenarioId,		Observations,					Classification,	IsDeleted,	IsSystemGenerated,	VariableTypeId,	FileRegistrationTransactionId,	OwnershipTicketId,	     ReasonId,	Comment,	MovementContractId,	BlockchainStatus,	BlockchainMovementTransactionId,	PreviousBlockchainMovementTransactionId,	Tolerance,	OperatorId,	BackupMovementId,	GlobalMovementId,	BalanceStatus,	SAPProcessStatus,	MovementEventId,	SourceMovementId,	TransactionHash,														BlockNumber,	RetryCount,	IsOfficial,	Version,	BatchId,	SystemId,	SourceSystemId,	IsTransferPoint,	SourceMovementTransactionId,	SourceInventoryProductId,	DeltaTicketId,	OfficialDeltaTicketId,	PendingApproval,	OfficialDeltaMessageTypeId,	ConsolidatedMovementTransactionId,	ConsolidatedInventoryProductId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(         1,	          2,			'Insert',    	1,		42,			@TicketId,			@ElementId, 	'2020-06-15',   	567.00,                 1678.00,				0.22,					@MeasurementUnit	,				2,		'Reporte Operativo Cusiana -Fecha'	,'Movimiento',	0,				NULL,				NULL,			@FileRegistrationTransactionId,							@TicketId,					NULL,	NULL,		  NULL,				1,	'6B20704E-75FA-4AB9-80F1-433EC58A4E51',					NULL,										NULL,		NULL,	NULL,					12345,			NULL,				NULL,			NULL,				NULL,				'0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24',	'0x15e650',			0,			0,		1,			NULL,			1,			1,				1,				NULL,							NULL,					NULL,			NULL,					NULL,				4,								NULL,									NULL,					'Demo',		'2020-07-17' ,	NULL,	NULL)
SELECT @MTranId_8 = SCOPE_IDENTITY()

--===================================  DATA FOR [Offchain].[MovementSource], [Offchain].[MovementDestination] =================================================================================================================================================================================================================================================

INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_1,			@NodeId,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_1,	@OtherNodeId,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)


INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_2,			@OtherNodeId,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_2,	@NodeId,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)

INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_3,			@NodeId,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_3,	@OtherNodeId,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)


INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_4,			@OtherNodeId,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_4,	@NodeId,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)



INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_5,			@NodeId,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_5,		@OtherNodeId,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)

INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_6,			@NodeId,				NULL,					10000002318,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_6,	@OtherNodeId,                       NULL,								10000002372,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)


INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_7,			@NodeId,				NULL,					10000002372,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_7,	@OtherNodeId,                       NULL,								10000002318,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)

INSERT INTO Offchain.MovementSource
(MovementTransactionId,	SourceNodeId,	SourceStorageLocationId,	SourceProductId,	SourceProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(@MTranId_8,			@OtherNodeId,				NULL,					10000002372,		NULL,                   'Demo',		'2020-07-17',	NULL,			NULL)


INSERT INTO Offchain.MovementDestination
(MovementTransactionId,	DestinationNodeId,	DestinationStorageLocationId,	DestinationProductId,	DestinationProductTypeId,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
VALUES(@MTranId_8,	@NodeId,                       NULL,								10000002318,        NULL,						'Demo',		'2020-07-17',	NULL,			NULL	)

/*===================================  DATA FOR [Admin].[ConsolidatedMovement] ==============================================================================================*/


INSERT INTO Admin.ConsolidatedMovement
(SourceNodeId,	SourceProductId,	DestinationNodeId,	DestinationProductId,	MovementTypeId,	StartDate,		EndDate,	NetStandardVolume,	GrossStandardVolume,	MeasurementUnit,	TicketId,	SegmentId,	SourceSystemId,	ExecutionDate,	IsActive,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(	@NodeId,	10000002318,   @OtherNodeId,				10000002372,	156,		'2020-06-26','2020-06-30',	   39048.00,			1600.00,				@MeasurementUnit,					@TicketId,		@ElementId,		1,				'2020-07-10',	1,			'System',	'2020-07-17',	NULL,			NULL)
SELECT @Con_Mov_1 = SCOPE_IDENTITY()

INSERT INTO Admin.ConsolidatedMovement
(SourceNodeId,	SourceProductId,	DestinationNodeId,	DestinationProductId,	MovementTypeId,	StartDate,		EndDate,	NetStandardVolume,	GrossStandardVolume,	MeasurementUnit,	TicketId,	SegmentId,	SourceSystemId,	ExecutionDate,	IsActive,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(	@NodeId,	10000002318,		@OtherNodeId,				10000002372,				156,		'2020-06-26','2020-06-30',	89283.00,			1200.00,				@MeasurementUnit,					@TicketId,		@ElementId,		1,				'2020-07-10',	1,			'System',	'2020-07-17',	NULL,			NULL)
SELECT @Con_Mov_2 = SCOPE_IDENTITY()

INSERT INTO Admin.ConsolidatedMovement
(SourceNodeId,	SourceProductId,	DestinationNodeId,	DestinationProductId,	MovementTypeId,	StartDate,		EndDate,	NetStandardVolume,	GrossStandardVolume,	MeasurementUnit,	TicketId,	SegmentId,	SourceSystemId,	ExecutionDate,	IsActive,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(	@NodeId,	10000002318,		@OtherNodeId,				10000002372,				156,		'2020-06-26','2020-06-30',	56293.00,			20000.00,				@MeasurementUnit,					@TicketId,		@ElementId,		1,				'2020-07-10',	1,			'System',	'2020-07-17',	NULL,			NULL)
SELECT @Con_Mov_3 = SCOPE_IDENTITY()

INSERT INTO Admin.ConsolidatedMovement
(SourceNodeId,	SourceProductId,	DestinationNodeId,	DestinationProductId,	MovementTypeId,	StartDate,		EndDate,	NetStandardVolume,	GrossStandardVolume,	MeasurementUnit,	TicketId,	SegmentId,	SourceSystemId,	ExecutionDate,	IsActive,	CreatedBy,	CreatedDate,	LastModifiedBy,	LastModifiedDate)
Values(	@NodeId,	10000002318,		@OtherNodeId,				10000002372,				156,		'2020-06-26','2020-06-30',	76800.00,			34500.00,				@MeasurementUnit,					@TicketId,		@ElementId,		1,				'2020-07-10',	1,			'System',	'2020-07-17',	NULL,			NULL)
SELECT @Con_Mov_4 = SCOPE_IDENTITY()

/*===================================  DATA FOR [Admin].[ConsolidatedMovement] ========================================================================================*/

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_1 WHERE MovementTransactionId = @MTranId_1

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_2 WHERE MovementTransactionId = @MTranId_2

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_3 WHERE MovementTransactionId = @MTranId_3

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_4 WHERE MovementTransactionId = @MTranId_4

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_1 WHERE MovementTransactionId = @MTranId_5

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_1 WHERE MovementTransactionId = @MTranId_6

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_2 WHERE MovementTransactionId = @MTranId_7

UPDATE Offchain.Movement SET ConsolidatedMovementTransactionId = @Con_Mov_4 WHERE MovementTransactionId = @MTranId_8

--===================================  DATA FOR [Admin].[ConsolidatedOwner] =================================================================================================================================================================================================================================================
INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,@Con_Mov_1,NULL,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,6500.00,0.60,@Con_Mov_2,NULL,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,40.00,0.20,@Con_Mov_3,NULL,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,1349.00,0.30,@Con_Mov_4,NULL,'System','2020-07-07',NULL,NULL)

/*===================================  DATA FOR [Admin].[ConsolidatedInventoryProduct]===============================================================================*/

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002318,	'2020-06-30 00:00:00.000',	200000.00,	78.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_1 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002318,	'2020-05-31 00:00:00.000',	210000.00,	45.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_2 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002318,	'2020-06-13 00:00:00.000',	220000.00,	2.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_3 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002372,	'2020-06-13 00:00:00.000',	230000.00,	567.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_4 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002372,	'2020-06-14 00:00:00.000',	240000.00,	34.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_5 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002372,	'2020-06-15 00:00:00.000',	250000.00,	6754.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_6 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002318,	'2020-06-30 00:00:00.000',	260000.00,	765.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_7 = SCOPE_IDENTITY()

INSERT INTO  [Admin].[ConsolidatedInventoryProduct](
NodeId,ProductId,InventoryDate,ProductVolume,GrossStandardQuantity,MeasurementUnit,TicketId,SegmentId,SourceSystemId,ExecutionDate,IsActive,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate) VALUES
	(@NodeId,	10000002372,	'2020-05-31 00:00:00.000',	270000.00,	2353.00,	@MeasurementUnit,	@TicketId,	@ElementId,	1,	'2020-07-11 00:00:00.000',	1	,'System'	,'2020-07-11 00:00:00.000',	NULL,	NULL)
SELECT @InvProdId_8 = SCOPE_IDENTITY()

/*=================================== DATA FOR  ADMIN.ConsolidatedOwner ==============================================================================================*/

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,6575.00,0.40,NULL,@InvProdId_1,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,23235.00,0.40,NULL,@InvProdId_1,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,34645.00,0.40,NULL,@InvProdId_2,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,34.00,0.40,NULL,@InvProdId_2,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,789.00,0.40,NULL,@InvProdId_3,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,21.00,0.40,NULL,@InvProdId_3,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,56.00,0.40,NULL,@InvProdId_4,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,@InvProdId_4,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,6786.00,0.40,NULL,@InvProdId_5,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,789.00,0.40,NULL,@InvProdId_5,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,3453.00,0.40,NULL,@InvProdId_6,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,235345.00,0.40,NULL,@InvProdId_6,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,346.00,0.40,NULL,@InvProdId_7,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,4000.00,0.40,NULL,@InvProdId_7,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(124,6786.00,0.40,NULL,@InvProdId_8,'System','2020-07-07',NULL,NULL)

INSERT INTO ADMIN.ConsolidatedOwner(OwnerId,OwnershipVolume,OwnershipPercentage,ConsolidatedMovementId,ConsolidatedInventoryProductId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES(30,35.00,0.40,NULL,@InvProdId_8,'System','2020-07-07',NULL,NULL)

/*************************************************** END OF KG EELEMENT ************************************************************/

 SELECT * FROM [Admin].[CategoryElement] WHERE [Name] = @ElementName
 SELECT * FROM [Admin].[Node] WHERE [Name] = @NodeName