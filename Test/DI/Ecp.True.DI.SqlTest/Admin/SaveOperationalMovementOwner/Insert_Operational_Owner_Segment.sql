/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Jun-17-2020

-- Description:     These test cases are for View [Admin].[OperationalMovementOwner]

-- Database backup Used:	dbaeuecpdevtrue
-- ==============================================================================================================================*/

/*
The below insert statements are required to populate Sistema related data in [Offchain].[OperationalMovementOwner] table.

*/

--=================================== SEGMENT DATA FOR [Admin].[OperationalMovement] =================================================================================================================================================================================================================================================

INSERT [Admin].[OperationalMovement] ([RNo], [BatchId], [MovementId], [MovementTransactionId], [CalculationDate], [MovementTypeName], [SourceNode], [DestinationNode], [SourceProduct], [DestinationProduct], [NetStandardVolume], [GrossStandardVolume], [MeasurementUnit], [EventType], [SystemName], [Movement], [PercentStandardUnCertainty], [Uncertainty], [ProductID], [InputCategory], [InputElementName], [InputNodeName], [InputStartDate], [InputEndDate], [ExecutionId], [CreatedBy], [CreatedDate]) 
VALUES (1, NULL, N'2367854', 11207, CAST(N'2020-06-08T00:00:00.000' AS DateTime), N'Automation_rjl9l', N'Automation_6dthw', N'Automation_40fiv', N'CRUDO CAMPO CUSUCO', N'CRUDO CAMPO CUSUCO', CAST(416871.82 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), N'Bbl', N'Insert', N'EXCEL - OCENSA', N'Salidas', CAST(0.22 AS Decimal(5, 2)), NULL, N'10000002372', N'Segmento', N'Automation_8srjq', N'Todos', CAST(N'2020-06-01' AS Date), CAST(N'2020-06-10' AS Date), N'9e67b8af-3d22-400f-bdb5-85a41a7a245a', N'ReportUser', CAST(N'2020-06-11T14:44:11.400' AS DateTime))
GO


--=================================== SEGMENT DATA FOR[Offchain].[Ownership] =================================================================================================================================================================================================================================================

INSERT [Offchain].[Ownership] ([OwnershipId], [MessageTypeId], [TicketId], [MovementTransactionId], [InventoryProductId], [OwnerId], [OwnershipPercentage], [OwnershipVolume], [AppliedRule], [RuleVersion], [ExecutionDate], [BlockchainStatus], [BlockchainMovementTransactionId], [BlockchainInventoryProductTransactionId], [IsDeleted], [EventType], [BlockchainOwnershipId], [PreviousBlockchainOwnershipId], [TransactionHash], [BlockNumber], [RetryCount], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (4797, 7, 24053, 11207, 50258, 30, CAST(100.00 AS Decimal(5, 2)), CAST(1200.00 AS Decimal(18, 2)), N'1', N'1', CAST(N'2020-04-30T06:54:34.840' AS DateTime), 1, NULL, N'343a23d9-596b-4e29-b336-9340708f0ea7', 0, N'Insert', N'30ac82e0-ca5c-47e5-b5a2-305f8deba820', NULL, N'0x46ba288d20ac33a17e6b4f61fdbcc01f5449a7ecfcc09a1c5571c3f3a6018ec7', N'0x15e746', 0, N'Segmento', CAST(N'2020-04-30T06:54:34.863' AS DateTime), N'Sistema', CAST(N'2020-04-30T06:54:46.260' AS DateTime))
GO
INSERT [Offchain].[Ownership] ([OwnershipId], [MessageTypeId], [TicketId], [MovementTransactionId], [InventoryProductId], [OwnerId], [OwnershipPercentage], [OwnershipVolume], [AppliedRule], [RuleVersion], [ExecutionDate], [BlockchainStatus], [BlockchainMovementTransactionId], [BlockchainInventoryProductTransactionId], [IsDeleted], [EventType], [BlockchainOwnershipId], [PreviousBlockchainOwnershipId], [TransactionHash], [BlockNumber], [RetryCount], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (4798, 7, 24053, 11207, 50701, 30, CAST(100.00 AS Decimal(5, 2)), CAST(1200.00 AS Decimal(18, 2)), N'1', N'1', CAST(N'2020-04-30T06:54:34.840' AS DateTime), 1, NULL, N'cd24004c-6e8a-4976-ac8d-402b1d9a9726', 0, N'Insert', N'3a00eb79-c4ad-45ad-864a-b298c901ad3f', NULL, N'0x9218a5502323429d98c0c703892adee178947543a9d013e566e0687deff62062', N'0x15e746', 0, N'Segmento', CAST(N'2020-04-30T06:54:34.863' AS DateTime), N'Sistema', CAST(N'2020-04-30T06:54:46.100' AS DateTime))
GO
INSERT [Offchain].[Ownership] ([OwnershipId], [MessageTypeId], [TicketId], [MovementTransactionId], [InventoryProductId], [OwnerId], [OwnershipPercentage], [OwnershipVolume], [AppliedRule], [RuleVersion], [ExecutionDate], [BlockchainStatus], [BlockchainMovementTransactionId], [BlockchainInventoryProductTransactionId], [IsDeleted], [EventType], [BlockchainOwnershipId], [PreviousBlockchainOwnershipId], [TransactionHash], [BlockNumber], [RetryCount], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (4799, 7, 24053, 11207, 50706, 30, CAST(100.00 AS Decimal(5, 2)), CAST(9500.00 AS Decimal(18, 2)), N'1', N'1', CAST(N'2020-04-30T06:54:34.840' AS DateTime), 1, NULL, N'e064b59b-e86e-44b1-adfa-3f14abf33ace', 0, N'Insert', N'3aedca42-a4e8-48b9-bcf0-719d20a6fd58', NULL, N'0xe957fdc56d1b58b90aafde5dc83dca8c7917215fb4ce7114ece2bb6b5037f478', N'0x15e746', 0, N'Segmento', CAST(N'2020-04-30T06:54:34.863' AS DateTime), N'Sistema', CAST(N'2020-04-30T06:54:46.427' AS DateTime))
GO
INSERT [Offchain].[Ownership] ([OwnershipId], [MessageTypeId], [TicketId], [MovementTransactionId], [InventoryProductId], [OwnerId], [OwnershipPercentage], [OwnershipVolume], [AppliedRule], [RuleVersion], [ExecutionDate], [BlockchainStatus], [BlockchainMovementTransactionId], [BlockchainInventoryProductTransactionId], [IsDeleted], [EventType], [BlockchainOwnershipId], [PreviousBlockchainOwnershipId], [TransactionHash], [BlockNumber], [RetryCount], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (4800, 7, 24053, 11207, 50707, 30, CAST(100.00 AS Decimal(5, 2)), CAST(19508.00 AS Decimal(18, 2)), N'1', N'1', CAST(N'2020-04-30T06:54:34.840' AS DateTime), 1, NULL, N'bc44d428-7974-40d8-b9f6-f53088724509', 0, N'Insert', N'be4eb123-a926-478c-93a0-3d2406a2c88c', NULL, N'0x49b55665c0d15a5cc97366c76e9ff28611c415de53e361b9e4199edc5148c253', N'0x15e746', 0, N'Segmento', CAST(N'2020-04-30T06:54:34.863' AS DateTime), N'Sistema', CAST(N'2020-04-30T06:54:46.357' AS DateTime))
GO

--=================================== SEGMENT DATA FOR [Offchain].[Owner] =================================================================================================================================================================================================================================================

INSERT [Offchain].[Owner] ([OwnerId], [OwnershipValue], [OwnershipValueUnit], [InventoryProductId], [MovementTransactionId], [BlockchainStatus], [BlockchainMovementTransactionId], [BlockchainInventoryProductTransactionId], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (N'31833', CAST(100.00 AS Decimal(18, 2)), N'%', 83886, 11207, 1, NULL, N'7717f462-dfc0-4843-8353-97ab1998ee0f', N'Sistema', CAST(N'2020-05-06T05:55:29.157' AS DateTime), N'Segmento', CAST(N'2020-05-06T05:55:41.133' AS DateTime))
GO
INSERT [Offchain].[Owner] ([OwnerId], [OwnershipValue], [OwnershipValueUnit], [InventoryProductId], [MovementTransactionId], [BlockchainStatus], [BlockchainMovementTransactionId], [BlockchainInventoryProductTransactionId], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (N'31833', CAST(100.00 AS Decimal(18, 2)), N'%', 83887, 11207, 1, NULL, N'fc5fc14a-91bc-4695-8de3-de2a27d8c914', N'Sistema', CAST(N'2020-05-06T06:19:40.647' AS DateTime), N'Segmento', CAST(N'2020-05-06T06:19:51.140' AS DateTime))
GO
INSERT [Offchain].[Owner] ([OwnerId], [OwnershipValue], [OwnershipValueUnit], [InventoryProductId], [MovementTransactionId], [BlockchainStatus], [BlockchainMovementTransactionId], [BlockchainInventoryProductTransactionId], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
VALUES (N'31833', CAST(100.00 AS Decimal(18, 2)), N'%', 83888, 11207, 1, NULL, N'd94b34dc-9d79-4b9f-9279-59d73e62ddfa', N'Sistema', CAST(N'2020-05-06T06:19:40.700' AS DateTime), N'Segmento', CAST(N'2020-05-06T06:19:51.167' AS DateTime))
GO
INSERT [Offchain].[Owner] ([OwnerId], [OwnershipValue], [OwnershipValueUnit], [InventoryProductId], [MovementTransactionId], [BlockchainStatus], [BlockchainMovementTransactionId], [BlockchainInventoryProductTransactionId], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) 
VALUES (N'31833', CAST(100.00 AS Decimal(18, 2)), N'%', 83902, 11207, 1, NULL, N'7e70aca7-24b9-4885-9062-5881a44cc045', N'Sistema', CAST(N'2020-05-06T10:50:47.170' AS DateTime), N'Segmento', CAST(N'2020-05-06T20:11:36.227' AS DateTime))
GO

--=================================== SEGMENT DATA FOR [Offchain].[Owner] =================================================================================================================================================================================================================================================
INSERT [Admin].[CategoryElement] ([ElementId], [Name], [Description], [CategoryId], [IsActive], [IconId], [Color], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (30, N'ECOPETROL', N'ECOPETROL', 7, 1, NULL, N'#00903B', N'Sistema', CAST(N'2020-02-07T04:08:45.113' AS DateTime), NULL, NULL)
GO

--######################################################################################################################################################################################################################################################################################################################################



--===================== TestCase1: To check if the movementary owner table returns data for Segments within a given date range ====================================

EXEC [Admin].[usp_SaveOperationalMovementOwnerWithoutCutOffForSegment] 'Segmento','Automation_8srjq','Todos','2020-06-01','2020-06-10','9e67b8af-3d22-400f-bdb5-85a41a7a245a'

SELECT * FROM [Admin].[OperationalMovementOwner] 
WHERE InputCategory = 'Segmento'

--========================================= Output Captured ====================================================================================================
/*
RNo	MovementId	BatchId	CalculationDate	TypeMovement	SourceNode	DestinationNode	SourceProduct	DestinationProduct	NetQuantity	GrossQuantity	MeasurementUnit	EventType	SystemName	Owner	Ownershipvolume	Ownershippercentage	ProductID	InputCategory	InputElementName	InputNodeName	InputStartDate	InputEndDate	ExecutionId	CreatedBy	CreatedDate
1	2367854	NULL	2020-06-08	Automation_rjl9l	Automation_6dthw	Automation_40fiv	CRUDO CAMPO CUSUCO	CRUDO CAMPO CUSUCO	416872	200	Bbl	Insert	EXCEL - OCENSA	ECOPETROL	416872	100	10000002372	Segmento	Automation_8srjq	Todos	2020-06-01	2020-06-10	9e67b8af-3d22-400f-bdb5-85a41a7a245a	ReportUser	2020-06-17 05:45:55.533*/

