/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Sep-29-2019
-- Updated Date:	Mar-20-2020
-- <Description>:  This trigger is used to insert data into Audit table for newly inserted or deleted data of HomologationObject table.  </Description>
-- ==============================================================================================================================*/

CREATE TRIGGER [Admin].[tr_HomologationObject_Changes]
ON [Admin].[HomologationObject]
AFTER INSERT, UPDATE, DELETE
AS
SET NOCOUNT ON;
BEGIN  

DECLARE @utcDate DATETIME
SET @utcDate = Admin.udf_GetTrueDate();

If exists (Select * from inserted) and not exists(Select * from deleted)
BEGIN	-- New Insert Case
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], NewValue, Field, Entity, [Identity]) SELECT @utcDate, 'Insert', inserted.CreatedBy, inserted.[HomologationObjectTypeId],	'HomologationObjectTypeId',	'Admin.HomologationObject', inserted.HomologationObjectTypeId  FROM inserted;
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], NewValue, Field, Entity, [Identity]) SELECT @utcDate, 'Insert', inserted.CreatedBy, inserted.[IsRequiredMapping],			'IsRequiredMapping',		'Admin.HomologationObject', inserted.HomologationObjectTypeId  FROM inserted;
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], NewValue, Field, Entity, [Identity]) SELECT @utcDate, 'Insert', inserted.CreatedBy, inserted.[HomologationGroupId],		'HomologationGroupId',		'Admin.HomologationObject', inserted.HomologationObjectTypeId  FROM inserted;
END

DECLARE @DeletedBy NVARCHAR(260);


If exists(select * from deleted) and not exists(Select * from inserted)
	BEGIN	-- Only Delete case

	SELECT @DeletedBy = Hg.LastModifiedBy FROM deleted d INNER JOIN [Admin].[HomologationGroup] Hg ON d.HomologationGroupId = Hg.HomologationGroupId;
	IF (@DeletedBy IS NULL)
	BEGIN
		SELECT @DeletedBy = Hg.CreatedBy FROM deleted d INNER JOIN [Admin].[HomologationGroup] Hg ON d.HomologationGroupId = Hg.HomologationGroupId;
	END
        INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, Field, Entity, [Identity]) 
			SELECT @utcDate, 'Delete', @DeletedBy, DELETED.[HomologationObjectTypeId],	'HomologationObjectTypeId',	'Admin.HomologationObject', DELETED.HomologationObjectTypeId  FROM DELETED;
		INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, Field, Entity, [Identity]) 
			SELECT @utcDate, 'Delete', @DeletedBy, DELETED.[IsRequiredMapping],	'IsRequiredMapping',	'Admin.HomologationObject', DELETED.HomologationObjectTypeId  FROM DELETED;
		INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, Field, Entity, [Identity]) 
			SELECT @utcDate, 'Delete', @DeletedBy, DELETED.[HomologationGroupId],	'HomologationGroupId',	'Admin.HomologationObject', DELETED.HomologationObjectTypeId  FROM DELETED;
		INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, Field, Entity, [Identity]) 
			SELECT @utcDate, 'Delete', @DeletedBy, DELETED.[HomologationObjectId],	'HomologationObjectId',	'Admin.HomologationObject', DELETED.HomologationObjectTypeId  FROM DELETED;
    END

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This trigger is used to insert data into Audit table for newly inserted or deleted data of HomologationObject table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationObject',
    @level2type = N'TRIGGER',
    @level2name = N'tr_HomologationObject_Changes'