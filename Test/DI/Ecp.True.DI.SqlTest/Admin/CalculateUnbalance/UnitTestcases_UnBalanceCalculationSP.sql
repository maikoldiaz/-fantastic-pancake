-- Unit Tests for unBalance calculation SP


	-- Seeding data in all relevant tables (Assuming that all master tables are already populated) and UnBalanceCalculation SP also exists.

Delete from [Admin].[InventoryProduct]
Delete from [Admin].[Inventory]
Delete from [Admin].[MovementDestination]
Delete from [Admin].[MovementSource]
Delete from [Admin].[MovementPeriod]
Delete from [Admin].[Movement]

DBCC CHECKIDENT ('[Admin].[InventoryProduct]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[Inventory]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[MovementDestination]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[MovementSource]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[MovementPeriod]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[Movement]', RESEED, 0)


Delete from [Admin].[StorageLocationProduct]
Delete From [Admin].[NodeStorageLocation]
Delete from [Admin].[Node]
Delete from [Audit].[AuditLog]
DBCC CHECKIDENT ('[Admin].[Node]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[NodeStorageLocation]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[StorageLocationProduct]', RESEED, 0)
DBCC CHECKIDENT ('[Audit].[AuditLog]', RESEED, 0)

DECLARE @utcDate DATETIME
SET @utcDate = GETUTCDATE();

DECLARE @NodeStorageLocationTableType AS [Admin].[NodeStorageLocationType]
Insert Into @NodeStorageLocationTableType  Values (1, 0, 'SLNode1', 'description1', 4, 1, 0, 1, 'F001', 'System' , @utcDate, NULL, NULL)
Insert Into @NodeStorageLocationTableType  Values (2, 0, 'SLNode2', 'description1', 4, 1, 0, 1, 'M001', 'System' , @utcDate, NULL, NULL)

DECLARE @StorageLocationProductTableType AS [Admin].[StorageLocationProductType]
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000003012', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '30000000040', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000003008', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '30000000045', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '30000000047', 0, 'System' , @utcDate, NULL, NULL)

EXECUTE [Admin].[usp_SaveNode]	@NodeId = 0,		--1
								@Name = 'SAN FERNANDO', 
								@Description = 'SAN FERNANDO', 
								@NodeTypeId = 1, 
								@SegmentId = 2, 
								@OperatorId = 3, 
								@LogisticCenterId = '1000', 
								@IsActive = 1, 
								@SendToSAP = 1, 
								@CreatedBy = 'System' , 
								@CreatedDate = @utcDate, 
								@LastModifiedBy = NULL, 
								@LastModifiedDate = NULL, 
								@NodeStorageLocation = @NodeStorageLocationTableType, 
								@StorageLocationProduct = @StorageLocationProductTableType;



Delete from @NodeStorageLocationTableType
Insert Into @NodeStorageLocationTableType  Values (1, 0, 'SLNode1', 'description1', 4, 1, 0, 1, 'F001', 'System' , @utcDate, NULL, NULL)
Insert Into @NodeStorageLocationTableType  Values (2, 0, 'SLNode2', 'description1', 4, 1, 0, 1, 'M001', 'System' , @utcDate, NULL, NULL)

Delete from @StorageLocationProductTableType
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000003012', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '30000000040', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000003008', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '30000000045', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '30000000047', 0, 'System' , @utcDate, NULL, NULL)

EXECUTE [Admin].[usp_SaveNode]	@NodeId = 0,			--2
								@Name = 'SAN FERNANDO - APIAY', 
								@Description = 'SAN FERNANDO - APIAY', 
								@NodeTypeId = 1, 
								@SegmentId = 2, 
								@OperatorId = 3, 
								@LogisticCenterId = '1000', 
								@IsActive = 1, 
								@SendToSAP = 1, 
								@CreatedBy = 'System' , 
								@CreatedDate = @utcDate, 
								@LastModifiedBy = NULL, 
								@LastModifiedDate = NULL, 
								@NodeStorageLocation = @NodeStorageLocationTableType, 
								@StorageLocationProduct = @StorageLocationProductTableType;



Delete from @NodeStorageLocationTableType
Insert Into @NodeStorageLocationTableType  Values (1, 0, 'SLNode1', 'description1', 4, 1, 0, 1, 'F001', 'System' , @utcDate, NULL, NULL)
Insert Into @NodeStorageLocationTableType  Values (2, 0, 'SLNode2', 'description1', 4, 1, 0, 1, 'M001', 'System' , @utcDate, NULL, NULL)

Delete from @StorageLocationProductTableType
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000003012', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '30000000040', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000003008', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '30000000045', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '30000000047', 0, 'System' , @utcDate, NULL, NULL)

EXECUTE [Admin].[usp_SaveNode]	@NodeId = 0,		--3
								@Name = 'BATERIA CASTILLA', 
								@Description = 'BATERIA CASTILLA', 
								@NodeTypeId = 1, 
								@SegmentId = 2, 
								@OperatorId = 3, 
								@LogisticCenterId = '1000', 
								@IsActive = 1, 
								@SendToSAP = 1, 
								@CreatedBy = 'System' , 
								@CreatedDate = @utcDate, 
								@LastModifiedBy = NULL, 
								@LastModifiedDate = NULL, 
								@NodeStorageLocation = @NodeStorageLocationTableType, 
								@StorageLocationProduct = @StorageLocationProductTableType;




Delete from @NodeStorageLocationTableType
Insert Into @NodeStorageLocationTableType  Values (1, 0, 'SLNode1', 'description1', 4, 1, 0, 1, 'F001', 'System' , @utcDate, NULL, NULL)
Insert Into @NodeStorageLocationTableType  Values (2, 0, 'SLNode2', 'description1', 4, 1, 0, 1, 'M001', 'System' , @utcDate, NULL, NULL)

Delete from @StorageLocationProductTableType
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000003012', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '30000000040', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000003008', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '30000000045', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '30000000047', 0, 'System' , @utcDate, NULL, NULL)

EXECUTE [Admin].[usp_SaveNode]	@NodeId = 0,		--4
								@Name = 'BATERIA CHICHIMENE', 
								@Description = 'BATERIA CHICHIMENE', 
								@NodeTypeId = 1, 
								@SegmentId = 2, 
								@OperatorId = 3, 
								@LogisticCenterId = '1000', 
								@IsActive = 1, 
								@SendToSAP = 1, 
								@CreatedBy = 'System' , 
								@CreatedDate = @utcDate, 
								@LastModifiedBy = NULL, 
								@LastModifiedDate = NULL, 
								@NodeStorageLocation = @NodeStorageLocationTableType, 
								@StorageLocationProduct = @StorageLocationProductTableType;



Delete from @NodeStorageLocationTableType
Insert Into @NodeStorageLocationTableType  Values (1, 0, 'SLNode1', 'description1', 4, 1, 0, 1, 'F001', 'System' , @utcDate, NULL, NULL)
Insert Into @NodeStorageLocationTableType  Values (2, 0, 'SLNode2', 'description1', 4, 1, 0, 1, 'M001', 'System' , @utcDate, NULL, NULL)

Delete from @StorageLocationProductTableType
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000003012', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '30000000040', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000003008', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '30000000045', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '30000000047', 0, 'System' , @utcDate, NULL, NULL)

EXECUTE [Admin].[usp_SaveNode]	@NodeId = 0,		--5
								@Name = 'APIAY', 
								@Description = 'APIAY', 
								@NodeTypeId = 1, 
								@SegmentId = 2, 
								@OperatorId = 3, 
								@LogisticCenterId = '1000', 
								@IsActive = 1, 
								@SendToSAP = 1, 
								@CreatedBy = 'System' , 
								@CreatedDate = @utcDate, 
								@LastModifiedBy = NULL, 
								@LastModifiedDate = NULL, 
								@NodeStorageLocation = @NodeStorageLocationTableType, 
								@StorageLocationProduct = @StorageLocationProductTableType;



Delete from @NodeStorageLocationTableType
Insert Into @NodeStorageLocationTableType  Values (1, 0, 'SLNode1', 'description1', 4, 1, 0, 1, 'F001', 'System' , @utcDate, NULL, NULL)
Insert Into @NodeStorageLocationTableType  Values (2, 0, 'SLNode2', 'description1', 4, 1, 0, 1, 'M001', 'System' , @utcDate, NULL, NULL)

Delete from @StorageLocationProductTableType
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000003012', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '30000000040', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000003008', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '30000000045', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '30000000047', 0, 'System' , @utcDate, NULL, NULL)

EXECUTE [Admin].[usp_SaveNode]	@NodeId = 0,		--6
								@Name = 'CHICHIMENE - SAN FERNANDO', 
								@Description = 'CHICHIMENE - SAN FERNANDO', 
								@NodeTypeId = 1, 
								@SegmentId = 2, 
								@OperatorId = 3, 
								@LogisticCenterId = '1000', 
								@IsActive = 1, 
								@SendToSAP = 1, 
								@CreatedBy = 'System' , 
								@CreatedDate = @utcDate, 
								@LastModifiedBy = NULL, 
								@LastModifiedDate = NULL, 
								@NodeStorageLocation = @NodeStorageLocationTableType, 
								@StorageLocationProduct = @StorageLocationProductTableType;

	-----------------------------------------------------------------

INSERT INTO [Admin].[Movement] VALUES (1, 1, 'SourceSystem1', 'EventType1', 1234567890, 'MovementTypeId1', '20190721 10:34:09 AM', 25000.00, 242536.50, 'Bls', 'Scenario1', 'Observation1', 'Movement', 0, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[Movement] VALUES (1, 1, 'SourceSystem2', 'EventType2', 2234567890, 'MovementTypeId2', '20190721 10:34:09 AM', 27000.00, 1500.00, 'Bls', 'Scenario2', 'Observation2', 'Movement', 0, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[Movement] VALUES (1, 2, 'SourceSystem3', 'EventType3', 3234567890, 'MovementTypeId3', '20190721 10:34:09 AM', 28000.00, 5000.00, 'Bls', 'Scenario3', 'Observation3', 'Movement', 0, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[Movement] VALUES (1, 1, 'SourceSystem4', 'EventType4', 4234567890, 'MovementTypeId4', '20190721 10:34:09 AM', 29000.00, 118020.43, 'Bls', 'Scenario4', 'Observation4', 'Movement', 0, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[Movement] VALUES (1, 2, 'SourceSystem5', 'EventType5', 5234567890, 'MovementTypeId5', '20190721 10:34:09 AM', 30000.00, 117482.57, 'Bls', 'Scenario5', 'Observation5', 'Movement', 0, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[Movement] VALUES (1, 2, 'SourceSystem6', 'EventType5', 5234567890, 'MovementTypeId6', '20190721 10:34:09 AM', 30000.00, 15000.00, 'Bls', 'Scenario6', 'Observation6', 'Movement', 0, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[Movement] VALUES (1, 2, 'SourceSystem7', 'EventType5', 5234567890, 'MovementTypeId7', '20190721 10:34:09 AM', 30000.00, 8300.00, 'Bls', 'Scenario7', 'Observation7', 'Movement', 0, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[Movement] VALUES (1, 2, 'SourceSystem8', 'EventType5', 5234567890, 'MovementTypeId8', '20190721 10:34:09 AM', 30000.00, 242536.50, 'Bls', 'Scenario8', 'Observation8', 'Movement', 0, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[Movement] VALUES (2, 2, 'SourceSystem9', 'EventType5', 5234567890, 'MovementTypeId9', '20190721 10:34:09 AM', 30000.00, 100.00, 'Bls', 'Scenario9', 'Observation9', 'Movement', 0, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[Movement] VALUES (2, 2, 'SourceSystem10', 'EventType5', 5234567890, 'MovementTypeId10', '20190721 10:34:09 AM', 30000.00, 210.00, 'Bls', 'Scenario10', 'Observation10', 'Movement', 0, 'System', GETUTCDATE(), NULL, NULL)

INSERT INTO [Admin].[MovementPeriod] VALUES (1, '20190711 10:34:09 AM', '20190711 10:34:09 AM', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementPeriod] VALUES (2, '20190711 10:34:09 AM', '20190711 10:34:09 AM', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementPeriod] VALUES (3, '20190712 10:34:09 AM', '20190712 10:34:09 AM', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementPeriod] VALUES (4, '20190713 10:34:09 AM', '20190713 10:34:09 AM', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementPeriod] VALUES (5, '20190714 10:34:09 AM', '20190714 10:34:09 AM', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementPeriod] VALUES (1, '20190711 10:34:09 AM', '20190711 10:34:09 AM', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementPeriod] VALUES (2, '20190711 10:34:09 AM', '20190711 10:34:09 AM', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementPeriod] VALUES (3, '20190712 10:34:09 AM', '20190712 10:34:09 AM', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementPeriod] VALUES (4, '20190713 10:34:09 AM', '20190713 10:34:09 AM', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementPeriod] VALUES (5, '20190714 10:34:09 AM', '20190714 10:34:09 AM', 'System', GETUTCDATE(), NULL, NULL)

INSERT INTO [Admin].[MovementSource] VALUES (1, 2, 1, 1, 'SourceProductType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementSource] VALUES (2, 1, 3, 6, 'SourceProductType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementSource] VALUES (3, 1, 4, 5, 'SourceProductType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementSource] VALUES (4, 3, 2, 2, 'SourceProductType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementSource] VALUES (5, 4, 2, 3, 'SourceProductType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementSource] VALUES (6, 3, 1, 1, 'SourceProductType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementSource] VALUES (7, 4, 3, 6, 'SourceProductType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementSource] VALUES (8, 1, 4, 5, 'SourceProductType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementSource] VALUES (9, 1, 2, 2, 'SourceProductType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementSource] VALUES (10,1, 2, 3, 'SourceProductType1', 'System', GETUTCDATE(), NULL, NULL)

INSERT INTO [Admin].[MovementDestination] VALUES (1, 5, 2, 3, 'DestinationType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementDestination] VALUES (2, 6, 2, 2, 'DestinationType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementDestination] VALUES (3, 2, 4, 5, 'DestinationType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementDestination] VALUES (4, 1, 3, 6, 'DestinationType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementDestination] VALUES (5, 1, 1, 1, 'DestinationType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementDestination] VALUES (6, 1, 4, 5, 'DestinationType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementDestination] VALUES (7, 1, 3, 6, 'DestinationType1', 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[MovementDestination] VALUES (8, 2, 1, 1, 'DestinationType1', 'System', GETUTCDATE(), NULL, NULL)


INSERT INTO [Admin].[Inventory] VALUES ('SINOPER', 'TRUE', 'EventType', 1234567890, '20190720 10:34:09 AM', 1, 'Observation1', 'Scenario', 0, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[Inventory] VALUES ('SINOPER', 'TRUE', 'EventType', 1234567890, '20190720 10:34:09 AM', 2, 'Observation1', 'Scenario', 0, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[Inventory] VALUES ('SINOPER', 'TRUE', 'EventType', 2234567890, '20190721 10:34:09 AM', 1, 'Observation2', 'Scenario', 0, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[Inventory] VALUES ('SINOPER', 'TRUE', 'EventType', 2234567890, '20190721 10:34:09 AM', 2, 'Observation2', 'Scenario', 0, 'System', GETUTCDATE(), NULL, NULL)



INSERT INTO [Admin].[InventoryProduct] VALUES (4, 'ProductType1', 167442.91, 'Bls', 1, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[InventoryProduct] VALUES (1, 'ProductType1', 27317.37, 'Bls', 1, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[InventoryProduct] VALUES (1, 'ProductType1', 25704.56, 'Bls', 1, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[InventoryProduct] VALUES (1, 'ProductType1', 143317.54, 'Bls', 1, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[InventoryProduct] VALUES (1, 'ProductType1', 140885.42, 'Bls', 1, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[InventoryProduct] VALUES (1, 'ProductType1', 92284.00, 'Bls', 2, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[InventoryProduct] VALUES (4, 'ProductType1', 181626.43, 'Bls', 3, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[InventoryProduct] VALUES (1, 'ProductType1', 28442.38, 'Bls', 3, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[InventoryProduct] VALUES (1, 'ProductType1', 55147.54, 'Bls', 3, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[InventoryProduct] VALUES (1, 'ProductType1', 142204.44, 'Bls', 3, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[InventoryProduct] VALUES (1, 'ProductType1', 104539.72, 'Bls', 3, 'System', GETUTCDATE(), NULL, NULL)
INSERT INTO [Admin].[InventoryProduct] VALUES (1, 'ProductType1', 92284.00, 'Bls', 4, 'System', GETUTCDATE(), NULL, NULL)

------------------------------------------------------------------------------------------------------------------------------------------

/*
	------- Helper Select Queries -----------------------------------------

	Select * from [Admin].[Node]
	Select * from [Admin].[NodeStorageLocation]
	Select * from [Admin].[StorageLocationProduct]


	-------------------------------------------------------------------------------------------
	Select * from Admin.Product where name = 'MEZCLA CASTILLA BLEND'		--10000003012
	Select * from Admin.Product where name = 'NAFTA VIRGEN'					--30000000040
	Select * from Admin.Product where name = 'MEZCLA MAGDALENA BLEND'		--10000003008	MEZCLA CASTILLA
	Select * from Admin.Product where name = 'NAFTA IMPORTADA'				--30000000045	NAFTA VIRGEN A1
	Select * from Admin.Product where name = 'NAFTA CRACKEADA'				--30000000047	NAFTA VIRGEN A2



	Select n.NodeId, n.Name,slp.StorageLocationProductId,  slp.ProductName  From Admin.Node n
	JOIN [Admin].[NodeStorageLocation] nsl ON n.NodeId = nsl.NodeId
	JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON slp.NodeStorageLocationId = n.NodeId


	Select * from [Admin].[Movement]
	--Select * from [Admin].[MovementPeriod]
	Select * from [Admin].[MovementSource]
	Select * from [Admin].[MovementDestination]

	Select * from [Admin].[Inventory]
	Select * from [Admin].[InventoryProduct]
	Select * from [Admin].[view_InventoryProductWithProductName];


	Select x.*, y.InventoryDate from [Admin].[view_InventoryProductWithProductName] x
	LEFT JOIN [Admin].[Inventory] y ON x.InventoryProductId = y.InventoryTransactionId

	EXECUTE [Admin].[usp_CalculateUnbalance] 1, '20190721 10:34:09 AM', '20190721 10:34:09 AM';
	EXECUTE [Admin].[usp_CalculateUnbalance] 1, '20190721 10:34:09 AM', '20190721 10:34:09 AM', 1;
	EXECUTE [Admin].[usp_CalculateUnbalance] 1, '20190721 10:34:09 AM', '20190721 10:34:09 AM', 2;
	EXECUTE [Admin].[usp_CalculateUnbalance] 1, '20190721 10:34:09 AM', '20190721 10:34:09 AM', 3;
	EXECUTE [Admin].[usp_CalculateUnbalance] 1, '20190721 10:34:09 AM', '20190721 10:34:09 AM', 4;
	EXECUTE [Admin].[usp_CalculateUnbalance] 1, '20190721 10:34:09 AM', '20190721 10:34:09 AM', 5;

*/

------------------------------------------------------------------------------------------------------------------------------------------


	-- Negative Test cases
	--											Node		StartDate					EndDate
	EXECUTE [Admin].[usp_CalculateUnbalance]	null,	'20190721 10:34:09 AM',		'20190721 10:34:09 AM';		-- INVENTORY_NODEID_REQUIREDVALIDATION

	EXECUTE [Admin].[usp_CalculateUnbalance]	1,			null,					'20190721 10:34:09 AM';		-- STARTDATE_REQUIREDVALIDATION

	EXECUTE [Admin].[usp_CalculateUnbalance]	1,		'20190721 10:34:09 AM',		null;						-- ENDDATE_REQUIREDVALIDATION

	EXECUTE [Admin].[usp_CalculateUnbalance]	1,		'20190722 10:34:09 AM',		'20190721 10:34:09 AM';		-- DATES_INCONSISTENT

	EXECUTE [Admin].[usp_CalculateUnbalance]	1,		'20190910 10:34:09 AM',		'20190912 10:34:09 AM';		-- ENDDATE_BEFORENOWVALIDATION  (make sure to update the enddate)

	-- Positive test Case
	EXECUTE [Admin].[usp_CalculateUnbalance]	1,		'20190721 10:34:09 AM',		'20190721 10:34:09 AM';

---------------------------------------------------------------------------------------------------------------------------------------------


	-- Plain Queries to match the SP result.


DECLARE @NodeId INT = 1;
DECLARE @StartDate DATETIME = '20190721 10:34:09 AM';
DECLARE @EndDate DATETIME = '20190721 10:34:09 AM';


	-- Inputs
Select m.MovementId, m.MovementTypeId, m.OperationalDate, ms.SourceNode, md.DestinationNode, ms.SourceProduct, md.DestinationProduct, m.NetStandardVolume, m.Scenario, mt.Name from [Admin].[Movement] m
JOIN (Select msi.*, n.Name As SourceNode, slp.ProductName As SourceProduct
		from [Admin].[MovementSource] msi
		LEFT JOIN [Admin].[Node] n ON msi.SourceNodeId = n.NodeId
		LEFT JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON msi.SourceProductId = slp.StorageLocationProductId) ms		
		ON m.MovementTransactionId = ms.MovementTransactionId

JOIN (Select mdi.*, n.Name As DestinationNode, slp.ProductName As DestinationProduct
		from [Admin].[MovementDestination] mdi
		LEFT JOIN [Admin].[Node] n ON mdi.DestinationNodeId = n.NodeId
		LEFT JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON mdi.DestinationProductId = slp.StorageLocationProductId) md	
		ON m.MovementTransactionId = md.MovementTransactionId

LEFT JOIN [Admin].[MessageType] mt on mt.MessageTypeId = m.MessageTypeId
WHERE	md.DestinationNodeId = @NodeId
		AND mt.MessageTypeId = 1		-- Taking only 'Movement' type
		AND @StartDate <= m.OperationalDate 
		AND m.OperationalDate < (select DATEADD(dd, DATEDIFF(dd, 0, @EndDate), 1));



	-- Outputs
Select m.MovementId, m.MovementTypeId, m.OperationalDate, ms.SourceNode, md.DestinationNode, ms.SourceProduct, md.DestinationProduct, m.NetStandardVolume, m.Scenario, mt.Name from [Admin].[Movement] m
JOIN (Select msi.*, n.Name As SourceNode, slp.ProductName As SourceProduct
		from [Admin].[MovementSource] msi
		LEFT JOIN [Admin].[Node] n ON msi.SourceNodeId = n.NodeId
		LEFT JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON msi.SourceProductId = slp.StorageLocationProductId) ms		
		ON m.MovementTransactionId = ms.MovementTransactionId

JOIN (Select mdi.*, n.Name As DestinationNode, slp.ProductName As DestinationProduct
		from [Admin].[MovementDestination] mdi
		LEFT JOIN [Admin].[Node] n ON mdi.DestinationNodeId = n.NodeId
		LEFT JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON mdi.DestinationProductId = slp.StorageLocationProductId) md	
		ON m.MovementTransactionId = md.MovementTransactionId

LEFT JOIN [Admin].[MessageType] mt on mt.MessageTypeId = m.MessageTypeId
WHERE	ms.SourceNodeId = @NodeId
		AND mt.MessageTypeId = 1		-- Taking only 'Movement' type
		AND @StartDate <= m.OperationalDate 
		AND m.OperationalDate < (select DATEADD(dd, DATEDIFF(dd, 0, @EndDate), 1));
		
		
	
	-- Losses
Select m.MovementId, m.MovementTypeId, m.OperationalDate, ms.SourceNode, NULL as DestinationNode, ms.SourceProduct, NULL as DestinationProduct , m.NetStandardVolume, m.Scenario, mt.Name from [Admin].[Movement] m
JOIN (Select msi.*, n.Name As SourceNode, slp.ProductName As SourceProduct
		from [Admin].[MovementSource] msi
		LEFT JOIN [Admin].[Node] n ON msi.SourceNodeId = n.NodeId
		LEFT JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON msi.SourceProductId = slp.StorageLocationProductId) ms		
		ON m.MovementTransactionId = ms.MovementTransactionId

LEFT JOIN [Admin].[MessageType] mt on mt.MessageTypeId = m.MessageTypeId
WHERE	ms.SourceNodeId = @NodeId
		AND mt.MessageTypeId = 2		-- Taking only 'Loss' type
		AND @StartDate <= m.OperationalDate 
		AND m.OperationalDate < (select DATEADD(dd, DATEDIFF(dd, 0, @EndDate), 1));



		-- Initial Inventory (with Additional products, we need to filterout manaully the unwanted ones)
SELECT ip.ProductName, ISNULL(SUM(ip.ProductVolume),0.0) AS InitialInventory FROM (SELECT ip.*, slp.ProductName FROM [Admin].[InventoryProduct] ip
LEFT JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON slp.StorageLocationProductId = ip.ProductId) ip
LEFT JOIN [Admin].[Inventory] i ON ip.InventoryProductId = i.InventoryTransactionId
WHERE	i.NodeId = @NodeId 
		AND i.InventoryDate >= (select DATEADD(dd, DATEDIFF(dd, 0, @StartDate), -1)) AND i.InventoryDate < (select DATEADD(dd, DATEDIFF(dd, 0, @StartDate), 0))
		--AND ip.ProductName IN (SELECT ProductName FROM @tempUniqueProducts)
GROUP BY ip.ProductName;


		-- Final Inventory (with Additional products, we need to filterout manaully the unwanted ones)
SELECT ip.ProductName, ISNULL(SUM(ip.ProductVolume),0.0) AS FinalInventory FROM [Admin].[view_InventoryProductWithProductName] ip
LEFT JOIN [Admin].[Inventory] i ON ip.InventoryProductId = i.InventoryTransactionId
WHERE	i.NodeId = @NodeId 
		AND i.InventoryDate >= (select DATEADD(dd, DATEDIFF(dd, 0, @EndDate), 0)) AND i.InventoryDate < (select DATEADD(dd, DATEDIFF(dd, 0, @EndDate), 1))
		--AND ip.ProductName IN (SELECT ProductName FROM @tempUniqueProducts)
GROUP BY ip.ProductName;


 -- Note: Analyse the above generated data in Excel as per SP output

