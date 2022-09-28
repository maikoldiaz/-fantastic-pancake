/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Sep-29-2019
-- Updated Date:	Mar-20-2020
-- <Description>:  This trigger is used to insert data into Audit table for newly inserted data into HomologationGroup table.  </Description>
-- ==============================================================================================================================*/

CREATE TRIGGER [Admin].[tr_HomologationGroup_Changes]
ON [Admin].[HomologationGroup]
AFTER INSERT, UPDATE, DELETE
AS
SET NOCOUNT ON;
BEGIN  

DECLARE @utcDate DATETIME
SET @utcDate = Admin.udf_GetTrueDate();

If exists (Select * from inserted) and not exists(Select * from deleted)
BEGIN	-- New Insert Case
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], NewValue, Field, Entity, [Identity]) SELECT @utcDate, 'Insert', inserted.CreatedBy, inserted.[GroupTypeId],		'GroupTypeId',		'Admin.HomologationGroup', inserted.HomologationGroupId  FROM inserted;
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], NewValue, Field, Entity, [Identity]) SELECT @utcDate, 'Insert', inserted.CreatedBy, inserted.[HomologationId],	'HomologationId',	'Admin.HomologationGroup', inserted.HomologationGroupId  FROM inserted;
END  

--If exists(select * from deleted) and not exists(Select * from inserted)
--	BEGIN	-- Only Delete case
--	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, Field, Entity) SELECT @utcDate, 'Delete', DELETED.CreatedBy, DELETED.[GroupTypeId],		'GroupTypeId',		'Admin.HomologationGroup'  FROM DELETED;
--	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, Field, Entity) SELECT @utcDate, 'Delete', DELETED.CreatedBy, DELETED.[HomologationId],	'HomologationId',	'Admin.HomologationGroup'  FROM DELETED;
--	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, Field, Entity) SELECT @utcDate, 'Delete', DELETED.CreatedBy, DELETED.[HomologationGroupId],	'HomologationGroupId',	'Admin.HomologationGroup'  FROM DELETED;
--	END

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This trigger is used to insert data into Audit table for newly inserted data into HomologationGroup table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'HomologationGroup',
    @level2type = N'TRIGGER',
    @level2name = N'tr_HomologationGroup_Changes'