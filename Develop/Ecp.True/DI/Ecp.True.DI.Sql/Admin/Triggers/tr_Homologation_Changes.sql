/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Sep-29-2019
-- Updated Date:	Mar-20-2020
-- <Description>:  This trigger is used to insert data into Audit table for newly inserted data into Homologation table.  </Description>
-- ==============================================================================================================================*/


CREATE TRIGGER [Admin].[tr_Homologation_Changes]
ON [Admin].[Homologation]  
AFTER INSERT, UPDATE
AS
SET NOCOUNT ON;
BEGIN  

DECLARE @utcDate DATETIME
SET @utcDate = Admin.udf_GetTrueDate();

IF NOT EXISTS(SELECT 1 FROM DELETED)
BEGIN	-- New Insert Case
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], NewValue, Field, Entity, [Identity]) SELECT @utcDate, 'Insert', inserted.CreatedBy, inserted.[SourceSystemId],		'SourceSystemId',		'Admin.Homologation', inserted.HomologationId FROM inserted;
	INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], NewValue, Field, Entity, [Identity]) SELECT @utcDate, 'Insert', inserted.CreatedBy, inserted.[DestinationSystemId],	'DestinationSystemId',	'Admin.Homologation', inserted.HomologationId FROM inserted;
	END  
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This trigger is used to insert data into Audit table for newly inserted data into Homologation table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Homologation',
    @level2type = N'TRIGGER',
    @level2name = N'tr_Homologation_Changes'