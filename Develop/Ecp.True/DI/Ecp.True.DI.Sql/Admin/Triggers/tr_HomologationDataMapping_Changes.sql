/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Sep-29-2019
-- Updated Date:	Mar-20-2020
-- <Description>:  This trigger is used to insert data into Audit table for newly inserted or deleted data of HomologationDataMapping table.  </Description>
-- ==============================================================================================================================*/

CREATE TRIGGER [Admin].[tr_HomologationDataMapping_Changes]
ON [Admin].[HomologationDataMapping]
AFTER INSERT, UPDATE, DELETE
AS
SET NOCOUNT ON;
BEGIN  

DECLARE @utcDate DATETIME
SET @utcDate = Admin.udf_GetTrueDate();

If exists (Select * from inserted) and not exists(Select * from deleted)
BEGIN	-- New Insert Case
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], NewValue, Field, Entity, [Identity]) SELECT @utcDate, 'Insert', inserted.CreatedBy, inserted.[SourceValue],			'SourceValue',			'Admin.HomologationDataMapping', inserted.HomologationDataMappingId FROM inserted;
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], NewValue, Field, Entity, [Identity]) SELECT @utcDate, 'Insert', inserted.CreatedBy, inserted.[DestinationValue],		'DestinationValue',		'Admin.HomologationDataMapping', inserted.HomologationDataMappingId FROM inserted;
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], NewValue, Field, Entity, [Identity]) SELECT @utcDate, 'Insert', inserted.CreatedBy, inserted.[HomologationGroupId],	'HomologationGroupId',	'Admin.HomologationDataMapping', inserted.HomologationDataMappingId FROM inserted;
END  

DECLARE @DeletedBy NVARCHAR(260);

If exists(select * from deleted) and not exists(Select * from inserted)
	BEGIN	-- Only Delete case

	SELECT @DeletedBy = Hg.LastModifiedBy FROM deleted d INNER JOIN [Admin].[HomologationGroup] Hg ON d.HomologationGroupId = Hg.HomologationGroupId;
	IF (@DeletedBy IS NULL)
	BEGIN
		SELECT @DeletedBy = Hg.CreatedBy FROM deleted d INNER JOIN [Admin].[HomologationGroup] Hg ON d.HomologationGroupId = Hg.HomologationGroupId;
	END

	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, Field, Entity, [Identity]) SELECT @utcDate, 'Delete', @DeletedBy, DELETED.[SourceValue],			'SourceValue',			'Admin.HomologationDataMapping', DELETED.HomologationDataMappingId FROM DELETED;
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, Field, Entity, [Identity]) SELECT @utcDate, 'Delete', @DeletedBy, DELETED.[DestinationValue],		'DestinationValue',		'Admin.HomologationDataMapping', DELETED.HomologationDataMappingId FROM DELETED;
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, Field, Entity, [Identity]) SELECT @utcDate, 'Delete', @DeletedBy, DELETED.[HomologationGroupId],	'HomologationGroupId',	'Admin.HomologationDataMapping', DELETED.HomologationDataMappingId FROM DELETED;
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, Field, Entity, [Identity]) SELECT @utcDate, 'Delete', @DeletedBy, DELETED.[HomologationDataMappingId],	'HomologationDataMappingId',	'Admin.HomologationDataMapping', DELETED.HomologationDataMappingId FROM DELETED;
    END

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This trigger is used to insert data into Audit table for newly inserted or deleted data of HomologationDataMapping table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationDataMapping',
    @level2type = N'TRIGGER',
    @level2name = N'tr_HomologationDataMapping_Changes'