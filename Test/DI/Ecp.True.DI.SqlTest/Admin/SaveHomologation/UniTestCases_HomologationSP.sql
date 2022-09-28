-- 1. ******************************************************************************
-- Type = INSERT-Positive
-- INSERT --> SINGLE Homologation, 3 HomologationGroup, 5 HomologationObject, 3 HomologationDatamapping.
-- 			  Note: For a new Homologation, the HomologationId should always be 0.
------------------------------------------------------------------------------------

Delete from [Admin].[HomologationDataMapping];
Delete from [Admin].[HomologationObject];
Delete From [Admin].[HomologationGroup];
Delete from [Admin].[Homologation];
DBCC CHECKIDENT ('[Admin].[HomologationDataMapping]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[HomologationGroup]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[HomologationObject]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[Homologation]', RESEED, 0)


DECLARE @utcDate DATETIME
SET @utcDate = GETUTCDATE();

DECLARE @HomologationGroupType AS [Admin].[HomologationGroupType]
Insert Into @HomologationGroupType  Values (1, 0, 1, 0, 'User' , @utcDate, NULL, NULL)
Insert Into @HomologationGroupType  Values (2, 0, 2, 0, 'User' , @utcDate, NULL, NULL)
Insert Into @HomologationGroupType  Values (3, 0, 3, 0, 'User' , @utcDate, NULL, NULL)

DECLARE @HomologationObjectType AS [Admin].[HomologationObjectType]
INSERT INTO @HomologationObjectType VALUES (1, 0, 1, 1, 0, 'User' , @utcDate, NULL, NULL)
INSERT INTO @HomologationObjectType VALUES (1, 0, 2, 0, 0, 'User' , @utcDate, NULL, NULL)
INSERT INTO @HomologationObjectType VALUES (2, 0, 3, 1, 0, 'User' , @utcDate, NULL, NULL)
INSERT INTO @HomologationObjectType VALUES (3, 0, 4, 0, 0, 'User' , @utcDate, NULL, NULL)
INSERT INTO @HomologationObjectType VALUES (3, 0, 5, 0, 0, 'User' , @utcDate, NULL, NULL)

DECLARE @HomologationDataMappingType AS [Admin].[HomologationDataMappingType]
INSERT INTO @HomologationDataMappingType VALUES (1, 0, 	'1',			'Node1', 0, 'User' , @utcDate, NULL, NULL)
INSERT INTO @HomologationDataMappingType VALUES (2, 0, 	'10000002093',  'Node2', 0, 'User' , @utcDate, NULL, NULL)
INSERT INTO @HomologationDataMappingType VALUES (3, 0, 	'3',			'Node3', 0, 'User' , @utcDate, NULL, NULL)


EXECUTE [Admin].[usp_SaveHomologation]	@HomologationId = 0, 
										@SourceSystemId = 1, 
										@DestinationSystemId = 2, 
										@CreatedBy = 'System' , 
										@CreatedDate = @utcDate, 
										@LastModifiedBy = NULL, 
										@LastModifiedDate = NULL,  
										@HomologationGroup = @HomologationGroupType, 
										@HomologationDataMapping = @HomologationDataMappingType, 
										@HomologationObject = @HomologationObjectType;

Select * from [Admin].[Homologation];
Select * from [Admin].[HomologationGroup];
Select * from [Admin].[HomologationObject];
Select * from [Admin].[HomologationDataMapping];











-- 2. ******************************************************************************
-- Type = INSERT-Negative	-> When @SourceSystemId = @DestinationSystemId
-- INSERT --> SINGLE Homologation, 3 HomologationGroup, 5 HomologationObject, 3 HomologationDatamapping.
-- 			  Note: For a new Homologation, the HomologationId should always be 0.
------------------------------------------------------------------------------------

Delete from [Admin].[HomologationDataMapping];
Delete from [Admin].[HomologationObject];
Delete From [Admin].[HomologationGroup];
Delete from [Admin].[Homologation];
DBCC CHECKIDENT ('[Admin].[HomologationDataMapping]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[HomologationGroup]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[HomologationObject]', RESEED, 0)
DBCC CHECKIDENT ('[Admin].[Homologation]', RESEED, 0)


DECLARE @utcDate DATETIME
SET @utcDate = GETUTCDATE();

DECLARE @HomologationGroupType AS [Admin].[HomologationGroupType]
Insert Into @HomologationGroupType  Values (1, 0, 1, 0, 'User' , @utcDate, NULL, NULL)
Insert Into @HomologationGroupType  Values (2, 0, 2, 0, 'User' , @utcDate, NULL, NULL)
Insert Into @HomologationGroupType  Values (3, 0, 3, 0, 'User' , @utcDate, NULL, NULL)

DECLARE @HomologationObjectType AS [Admin].[HomologationObjectType]
INSERT INTO @HomologationObjectType VALUES (1, 0, 1, 1, 0, 'User' , @utcDate, NULL, NULL)
INSERT INTO @HomologationObjectType VALUES (1, 0, 2, 0, 0, 'User' , @utcDate, NULL, NULL)
INSERT INTO @HomologationObjectType VALUES (2, 0, 3, 1, 0, 'User' , @utcDate, NULL, NULL)
INSERT INTO @HomologationObjectType VALUES (3, 0, 4, 0, 0, 'User' , @utcDate, NULL, NULL)
INSERT INTO @HomologationObjectType VALUES (3, 0, 5, 0, 0, 'User' , @utcDate, NULL, NULL)

DECLARE @HomologationDataMappingType AS [Admin].[HomologationDataMappingType]
INSERT INTO @HomologationDataMappingType VALUES (1, 0, 	'1',			'Node1', 0, 'User' , @utcDate, NULL, NULL)
INSERT INTO @HomologationDataMappingType VALUES (2, 0, 	'10023002093',  'Node2', 0, 'User' , @utcDate, NULL, NULL)
INSERT INTO @HomologationDataMappingType VALUES (3, 0, 	'3',			'Node3', 0, 'User' , @utcDate, NULL, NULL)


EXECUTE [Admin].[usp_SaveHomologation]	@HomologationId = 0, 
										@SourceSystemId = 1, 
										@DestinationSystemId = 2, 
										@CreatedBy = 'System' , 
										@CreatedDate = @utcDate, 
										@LastModifiedBy = NULL, 
										@LastModifiedDate = NULL,  
										@HomologationGroup = @HomologationGroupType, 
										@HomologationDataMapping = @HomologationDataMappingType, 
										@HomologationObject = @HomologationObjectType;

Select * from [Admin].[Homologation];
Select * from [Admin].[HomologationGroup];
Select * from [Admin].[HomologationObject];
Select * from [Admin].[HomologationDataMapping];





