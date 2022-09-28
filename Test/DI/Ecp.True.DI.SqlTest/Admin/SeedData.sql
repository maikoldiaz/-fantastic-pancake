-- Script to populate master and dependent tables.

-- All Select Statements
Select * from [Audit].[AuditLog]
Select * from [Admin].[Node]
Select * from [Admin].[NodeStorageLocation]
Select * from [Admin].[StorageLocationProduct]

Select * from [Admin].[Category]
Select * from [Admin].[CategoryElement] 
Select * from [Admin].[LogisticCenter]
Select * from [Admin].[Product]
Select * from [Admin].[StorageLocation]


Delete from [Admin].[StorageLocationProduct]
Delete From [Admin].[NodeStorageLocation]
Delete from [Admin].[Node]
Delete from [Admin].[CategoryElement]
Delete from [Admin].[Category]
Delete from [Admin].[LogisticCenter]
Delete from [Admin].[Product]
Delete from [Admin].[StorageLocation]
Delete from [Audit].[AuditLog]

DBCC CHECKIDENT ('[Admin].[StorageLocationProduct]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[NodeStorageLocation]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[Node]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[CategoryElement]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[Category]', RESEED, 0)
DBCC CHECKIDENT ('[Audit].[AuditLog]', RESEED, 0)

DECLARE @utcDate DATETIME
SET @utcDate = GETUTCDATE();

Insert Into [Admin].[Category] Values ('Segments', 				'Description1', 1, 0, 'System' , @utcDate, NULL, NULL)
Insert Into [Admin].[Category] Values ('Operators', 			'Description2', 1, 0, 'System' , @utcDate, NULL, NULL)
Insert Into [Admin].[Category] Values ('NodeTypes', 			'Description3', 1, 0, 'System' , @utcDate, NULL, NULL)
Insert Into [Admin].[Category] Values ('StorageLocationTypes', 	'Description4', 1, 0, 'System' , @utcDate, NULL, NULL)

Insert Into [Admin].[CategoryElement] Values ('Elemet1', 'Description1', 1, 1, 'System' , @utcDate, NULL, NULL)
Insert Into [Admin].[CategoryElement] Values ('Elemet2', 'Description2', 2, 1, 'System' , @utcDate, NULL, NULL)
Insert Into [Admin].[CategoryElement] Values ('Elemet3', 'Description3', 3, 1, 'System' , @utcDate, NULL, NULL)
Insert Into [Admin].[CategoryElement] Values ('Elemet4', 'Description4', 4, 1, 'System' , @utcDate, NULL, NULL)


Insert Into [Admin].[StorageLocation] Values ('M001', 'PR APIAY : MATERIA PRIMA', 1, 'System' , @utcDate, NULL, NULL)
Insert Into [Admin].[StorageLocation] Values ('F001', 'PR EL CENTRO-GCB : INV TRANSITO', 1, 'System' , @utcDate, NULL, NULL)
Insert Into [Admin].[StorageLocation] Values ('O001', 'PR CARROTANQUES : MATERIA PRIMA', 1, 'System' , @utcDate, NULL, NULL)
Insert Into [Admin].[StorageLocation] Values ('O002', 'PR CARROTANQUES : MATERIA PRIMAB', 1, 'System' , @utcDate, NULL, NULL)

