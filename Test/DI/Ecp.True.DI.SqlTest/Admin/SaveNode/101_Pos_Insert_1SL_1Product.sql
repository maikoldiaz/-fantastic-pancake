-- 1. ******************************************************************************
-- Type = INSERT-Positive
-- INSERT --> SINGLE NODE, SINGLE LOCATION, SINGLE PRODUCT
-- 			  Note: For a new Node, the NodeId should always be 0.
------------------------------------------------------------------------------------


Delete from [Admin].[StorageLocationProduct]
Delete From [Admin].[NodeStorageLocation]
Delete from [Admin].[Node]
Delete from [Audit].[AuditLog]
DELETE FROM Admin.NodeTag
DBCC CHECKIDENT ('[Admin].[Node]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[NodeStorageLocation]', RESEED, 0)
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


Select * from [Admin].[Node]
Select * from [Admin].[NodeStorageLocation]
Select * from [Admin].[StorageLocationProduct]
Select * from [Audit].[AuditLog]

