-- 13. ******************************************************************************
-- Type = UPDATE-Negative
-- UPDATE --> Try updating an existing Node with 0 NodeStorageLocation.
--				This should throw an error - 'One or More Product does not belong to the given StorageLocation.'
-- 			  Note: For an existing node the NodeId should be passed.
------------------------------------------------------------------------------------
Delete from [Admin].[StorageLocationProduct]
Delete From [Admin].[NodeStorageLocation]
Delete From [Admin].[NodeTag]
Delete from [Admin].[Node]
Delete from [Audit].[AuditLog]
DBCC CHECKIDENT ('[Admin].[Node]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[NodeStorageLocation]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[NodeTag]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[StorageLocationProduct]', RESEED, 0)
DBCC CHECKIDENT ('[Audit].[AuditLog]', RESEED, 0)


DECLARE @utcDate DATETIME
SET @utcDate = GETUTCDATE();

DECLARE @NodeStorageLocationTableType AS [Admin].[NodeStorageLocationType]
Insert Into @NodeStorageLocationTableType  Values (1, 0, 'SLNode1', 'description1', 4, 1, 0, 1, '1000:C001', 'System' , @utcDate, NULL, NULL)

DECLARE @StorageLocationProductTableType AS [Admin].[StorageLocationProductType]
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000002049', 0, 'System' , @utcDate, NULL, NULL)

EXECUTE [Admin].[usp_SaveNode]	@NodeId = 0, 
								@Name = 'Node1', 
								@Description = 'Description1', 
								@NodeTypeId = 3, 
								@SegmentId = 1, 
								@OperatorId = 2, 
								@LogisticCenterId = '1000', 
								@IsActive = 1, 
								@SendToSAP = 1, 
								@CreatedBy = 'System' , 
								@CreatedDate = @utcDate, 
								@LastModifiedBy = NULL, 
								@LastModifiedDate = NULL, 
								@NodeStorageLocation = @NodeStorageLocationTableType, 
								@StorageLocationProduct = @StorageLocationProductTableType;


Delete from @NodeStorageLocationTableType;
Insert Into @NodeStorageLocationTableType  Values (1, 0, 'SLNode1', 'description1', 4, 1, 0, 1, '1000:C001', 'System' , @utcDate, NULL, NULL)
Insert Into @NodeStorageLocationTableType  Values (2, 0, 'SLNode2', 'description1', 4, 1, 0, 1, '1000:M001', 'System' , @utcDate, NULL, NULL)

Delete from @StorageLocationProductTableType
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000002049', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000002093', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '10000002199', 0, 'System' , @utcDate, NULL, NULL)

EXECUTE [Admin].[usp_SaveNode]	@NodeId = 0, 
								@Name = 'Node2', 
								@Description = 'Description1', 
								@NodeTypeId = 3, 
								@SegmentId = 1, 
								@OperatorId = 2, 
								@LogisticCenterId = '1000', 
								@IsActive = 1, 
								@SendToSAP = 1, 
								@CreatedBy = 'System' , 
								@CreatedDate = @utcDate, 
								@LastModifiedBy = NULL, 
								@LastModifiedDate = NULL, 
								@NodeStorageLocation = @NodeStorageLocationTableType, 
								@StorageLocationProduct = @StorageLocationProductTableType;




Delete from @NodeStorageLocationTableType;
--Insert Into @NodeStorageLocationTableType  Values (0, 2, 'SLNode1', 'description1', 4, 1, 2, 1, '1000:C001', NULL , NULL, 'User1', @utcDate)

Delete from @StorageLocationProductTableType
INSERT INTO @StorageLocationProductTableType VALUES (0, 3, 1, '10000002093', 2, NULL , NULL, 'User1', @utcDate)


EXECUTE [Admin].[usp_SaveNode]	@NodeId = 2, 
								@Name = 'Node2', 
								@Description = 'Description1', 
								@NodeTypeId = 3, 
								@SegmentId = 1, 
								@OperatorId = 2, 
								@LogisticCenterId = '1000', 
								@IsActive = 1, 
								@SendToSAP = 1, 
								@CreatedBy = NULL , 
								@CreatedDate = NULL, 
								@LastModifiedBy = 'User1', 
								@LastModifiedDate = @utcDate, 
								@NodeStorageLocation = @NodeStorageLocationTableType, 
								@StorageLocationProduct = @StorageLocationProductTableType;



Select * from [Admin].[Node]
Select * from [Admin].[NodeStorageLocation]
Select * from [Admin].[StorageLocationProduct]
Select * from [Audit].[AuditLog]

