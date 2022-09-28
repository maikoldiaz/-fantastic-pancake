-- 1. ******************************************************************************
-- Type = Node Seed script for creating 3 Nodes
--			This is required to test NodeTag Test Scenarios
------------------------------------------------------------------------------------


Delete from [Admin].[StorageLocationProduct]
Delete From [Admin].[NodeStorageLocation]
Delete From [Admin].[NodeTag]
Delete from [Admin].[Node]
--Delete from [Audit].[AuditLog]
DBCC CHECKIDENT ('[Admin].[Node]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[NodeStorageLocation]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[NodeTag]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[StorageLocationProduct]', RESEED, 0)
--DBCC CHECKIDENT ('[Audit].[AuditLog]', RESEED, 0)


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



Delete from @NodeStorageLocationTableType
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



Delete from @NodeStorageLocationTableType
Insert Into @NodeStorageLocationTableType  Values (1, 0, 'SLNode3', 'description1', 4, 1, 0, 1, '1005:M001', 'System' , @utcDate, NULL, NULL)
Insert Into @NodeStorageLocationTableType  Values (2, 0, 'SLNode4', 'description1', 4, 1, 0, 1, '1006:C001', 'System' , @utcDate, NULL, NULL)

Delete from @StorageLocationProductTableType
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000002441', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (1, 0, 1, '10000003000', 0, 'System' , @utcDate, NULL, NULL)
INSERT INTO @StorageLocationProductTableType VALUES (2, 0, 1, '10000003001', 0, 'System' , @utcDate, NULL, NULL)

EXECUTE [Admin].[usp_SaveNode]	@NodeId = 0, 
								@Name = 'Node3', 
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
--Select * from [Audit].[AuditLog]





-- *************************************** NODE-TAG Test Scenarios *********************************************************************
-- Insert - Positive
Delete from [Admin].[NodeTag];
DBCC CHECKIDENT ('[Admin].[NodeTag]', RESEED, 0)

DECLARE @StartDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
INSERT INTO @NodeTagTable VALUES (0, 1, 'System' , @utcDate, NULL, NULL)
INSERT INTO @NodeTagTable VALUES (0, 2, 'System' , @utcDate, NULL, NULL)
INSERT INTO @NodeTagTable VALUES (0, 3, 'System' , @utcDate, NULL, NULL)

EXEC [Admin].[usp_NodeTag] @OperationType = 1, @ElementId = 4, @InputDate = @StartDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];


-- Insert - Positive
DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @StartDate DATETIME = DATEADD(dd, 2, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
INSERT INTO @NodeTagTable VALUES (0, 1, 'System' , @utcDate, NULL, NULL)

EXEC [Admin].[usp_NodeTag] @OperationType = 1, @ElementId = 5, @InputDate = @StartDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];



-- Insert - Negative (StartDateLessThanCurrentDate)
DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @StartDate DATETIME = DATEADD(dd, -2, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
INSERT INTO @NodeTagTable VALUES (0, 1, 'System' , @utcDate, NULL, NULL)

EXEC [Admin].[usp_NodeTag] @OperationType = 1, @ElementId = 5, @InputDate = @StartDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];


-- Insert - Negative (UNIQUENESS_FAILED)
DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @StartDate DATETIME = DATEADD(dd, 2, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
INSERT INTO @NodeTagTable VALUES (0, 1, 'System' , @utcDate, NULL, NULL)

EXEC [Admin].[usp_NodeTag] @OperationType = 1, @ElementId = 4, @InputDate = @StartDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];



-- Insert - Negative (TIME_OVERLAP)
--	Note: Not possible to get this Exception as per current uniqueness rule which automatically takes care of time overlap.

--------------------------------------------------------------------------



-- Set Expire - Positive

DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @EndDate DATETIME = DATEADD(dd, 20, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
INSERT INTO @NodeTagTable VALUES (10, 1, 'System' , @utcDate, NULL, NULL)
INSERT INTO @NodeTagTable VALUES (11, 2, 'System' , @utcDate, NULL, NULL)

EXEC [Admin].[usp_NodeTag] @OperationType = 3, @InputDate = @EndDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];


-- Set Expire - Positive (Expanding the Expiry date)
DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @EndDate DATETIME = DATEADD(dd, 40, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
INSERT INTO @NodeTagTable VALUES (10, 1, 'System' , @utcDate, NULL, NULL)
INSERT INTO @NodeTagTable VALUES (11, 2, 'System' , @utcDate, NULL, NULL)

EXEC [Admin].[usp_NodeTag] @OperationType = 3, @InputDate = @EndDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];


-- Set Expire - Positive (Shrinking the Expiry date, but greater than today's date)

DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @EndDate DATETIME = DATEADD(dd, 10, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
INSERT INTO @NodeTagTable VALUES (10, 1, 'System' , @utcDate, NULL, NULL)
INSERT INTO @NodeTagTable VALUES (11, 2, 'System' , @utcDate, NULL, NULL)

EXEC [Admin].[usp_NodeTag] @OperationType = 3, @InputDate = @EndDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];



-- Set Expire - Negative (EndDateLessThanCurrentDate)

DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @EndDate DATETIME = DATEADD(dd, -10, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
INSERT INTO @NodeTagTable VALUES (10, 1, 'System' , @utcDate, NULL, NULL)
INSERT INTO @NodeTagTable VALUES (11, 2, 'System' , @utcDate, NULL, NULL)

EXEC [Admin].[usp_NodeTag] @OperationType = 3, @InputDate = @EndDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];


-- Set Expire - Negative (Try to set expiry of default (special) Elements) [Error = CannotExpireDefaultCategories]

DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @EndDate DATETIME = DATEADD(dd, 20, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
INSERT INTO @NodeTagTable VALUES (10, 1, 'System' , @utcDate, NULL, NULL)
INSERT INTO @NodeTagTable VALUES (11, 2, 'System' , @utcDate, NULL, NULL)

EXEC [Admin].[usp_NodeTag] @OperationType = 3, @InputDate = @EndDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];

----------------------------------------------------------------------------------


-- Update - Positive	(Updating the special default NodeTags)

DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @StartDate DATETIME = DATEADD(dd, 2, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
INSERT INTO @NodeTagTable VALUES (1, 1, 'System' , @utcDate, NULL, NULL)
INSERT INTO @NodeTagTable VALUES (7, 3, 'System' , @utcDate, NULL, NULL)


EXEC [Admin].[usp_NodeTag] @OperationType = 2, @ElementId = 7, @InputDate = @StartDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];



-- Update - Negative	(Updating the Original ElementID which does not belong to same CategoryId)

DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @StartDate DATETIME = DATEADD(dd, 2, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
INSERT INTO @NodeTagTable VALUES (1, 1, 'System' , @utcDate, NULL, NULL)
INSERT INTO @NodeTagTable VALUES (7, 3, 'System' , @utcDate, NULL, NULL)


EXEC [Admin].[usp_NodeTag] @OperationType = 2, @ElementId = 7, @InputDate = @StartDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];





----------------------------------------------------------------------------------
-- Update - Positive	(Updating the normal NodeTags)

 -- Set Expiry
DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @EndDate DATETIME = DATEADD(dd, 40, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
--INSERT INTO @NodeTagTable VALUES (10, 1, 'System' , @utcDate, NULL, NULL)
INSERT INTO @NodeTagTable VALUES (11, 2, 'System' , @utcDate, NULL, NULL)

EXEC [Admin].[usp_NodeTag] @OperationType = 3, @InputDate = @EndDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];



			-- Update - Negative:	Intermediate negative test when setting the new StartDate less than already existing EndDate. (TIME_OVERLAP)
				DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
				DECLARE @StartDate DATETIME = DATEADD(dd, 20, @todaysDate);
				DECLARE @utcDate DATETIME = GETUTCDATE();

				DECLARE @NodeTagTable AS [Admin].[NodeTagType];
				INSERT INTO @NodeTagTable VALUES (10, 1, 'System' , @utcDate, NULL, NULL)
				INSERT INTO @NodeTagTable VALUES (11, 2, 'System' , @utcDate, NULL, NULL)

				EXEC [Admin].[usp_NodeTag] @OperationType = 2, @ElementId = 4, @InputDate = @StartDate, @NodeTag = @NodeTagTable;
				Select * from [Admin].[view_NodeTagWithCategoryId];


	-- Actual Update Happening
DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @StartDate DATETIME = DATEADD(dd, 55, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
INSERT INTO @NodeTagTable VALUES (10, 1, 'System' , @utcDate, 'System', @utcDate)
INSERT INTO @NodeTagTable VALUES (11, 2, 'System' , @utcDate, 'System', @utcDate)

EXEC [Admin].[usp_NodeTag] @OperationType = 2, @ElementId = 10, @InputDate = @StartDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];


----------------------------------------------------------------------------------

-- Update - Negative	(Updating the normal NodeTags, when the new StartDate is less than one of the existing EndDate WHERE EndDate <> '1900-01-01 00:00:00.000')


 -- Set Expiry
DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @EndDate DATETIME = DATEADD(dd, 40, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
--INSERT INTO @NodeTagTable VALUES (10, 1, 'System' , @utcDate, NULL, NULL)
INSERT INTO @NodeTagTable VALUES (11, 2, 'System' , @utcDate, NULL, NULL)

EXEC [Admin].[usp_NodeTag] @OperationType = 3, @InputDate = @EndDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];


	-- Actual Update Happening
DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, GETUTCDATE()), 0));
DECLARE @StartDate DATETIME = DATEADD(dd, 20, @todaysDate);
DECLARE @utcDate DATETIME = GETUTCDATE();

DECLARE @NodeTagTable AS [Admin].[NodeTagType];
INSERT INTO @NodeTagTable VALUES (10, 1, 'System' , @utcDate, 'System', @utcDate)
INSERT INTO @NodeTagTable VALUES (11, 2, 'System' , @utcDate, 'System', @utcDate)

EXEC [Admin].[usp_NodeTag] @OperationType = 2, @ElementId = 10, @InputDate = @StartDate, @NodeTag = @NodeTagTable;
Select * from [Admin].[view_NodeTagWithCategoryId];

