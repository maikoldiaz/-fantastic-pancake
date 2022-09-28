/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	03-Apr-2020
-- Updated Date:	
-- <Description>:  This trigger is used to insert data into Audit table for newly inserted or deleted data into StorageLocationProductVariable table.  </Description>
-- ==============================================================================================================================*/

CREATE TRIGGER [Admin].[tr_StorageLocationProductVariable_Changes]
ON [Admin].[StorageLocationProductVariable]
AFTER INSERT, UPDATE, DELETE
AS
SET NOCOUNT ON;
BEGIN  

DECLARE @utcDate DATETIME
SET @utcDate = Admin.udf_GetTrueDate();

If exists (Select * from inserted) and not exists(Select * from deleted)
BEGIN	-- New Insert Case
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], NewValue, Field, Entity, [Identity]) 
	SELECT @utcDate, 'Insert', inserted.CreatedBy, inserted.[StorageLocationProductId],	'StorageLocationProductId', 'Admin.StorageLocationProductVariable', inserted.StorageLocationProductVariableId 
	FROM   inserted

	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], NewValue, Field, Entity, [Identity]) 
	SELECT @utcDate, 'Insert', inserted.CreatedBy, inserted.[VariableTypeId],			'VariableTypeId',			'Admin.StorageLocationProductVariable', inserted.StorageLocationProductVariableId 
	FROM   inserted
END  

If exists(select * from deleted) and not exists(Select * from inserted)
	BEGIN	-- Only Delete case

	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, Field, Entity, [Identity]) 
	SELECT @utcDate, 'Delete', deleted.CreatedBy, deleted.[StorageLocationProductId],'StorageLocationProductId', 'Admin.StorageLocationProductVariable', deleted.StorageLocationProductVariableId 
	FROM deleted;
	
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, Field, Entity, [Identity]) 
	SELECT @utcDate, 'Delete', deleted.CreatedBy, deleted.[VariableTypeId],			'VariableTypeId',			'Admin.StorageLocationProductVariable', deleted.StorageLocationProductVariableId 
	FROM deleted;
	
    END

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' This trigger is used to insert data into Audit table for newly inserted or deleted data into StorageLocationProductVariable table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProductVariable',
    @level2type = N'TRIGGER',
    @level2name = N'tr_StorageLocationProductVariable_Changes'