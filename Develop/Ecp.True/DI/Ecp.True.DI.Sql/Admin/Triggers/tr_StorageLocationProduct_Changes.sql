/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	03-Apr-2020
-- Updated Date:	
-- <Description>:  This trigger is used to insert data into Audit table only for NodeProductRuleId column changes in StorageLocationProduct table.  </Description>
-- ==============================================================================================================================*/

CREATE TRIGGER [Admin].[tr_StorageLocationProduct_Changes]  
ON [Admin].[StorageLocationProduct]  
AFTER UPDATE
AS
SET NOCOUNT ON;
BEGIN  

IF EXISTS(SELECT 1 FROM DELETED)
	BEGIN	-- Updated Case
	
		INSERT INTO [Audit].[AuditLog] (LogDate, LogType, [User], OldValue, NewValue, Field, Entity, [Identity]) 
		SELECT  Admin.udf_GETTRUEDATE()
				,'Update'
				,ins.[LastModifiedBy]
				,del.[NodeProductRuleId]
				,ins.[NodeProductRuleId]
				,'NodeProductRuleId'
				,'Admin.StorageLocationProduct'
				,ins.StorageLocationProductId 
		FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.StorageLocationProductId  =  del.StorageLocationProductId;
	
	END

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This trigger is used to insert data into Audit table only for NodeProductRuleId column changes in StorageLocationProduct table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'StorageLocationProduct',
    @level2type = N'TRIGGER',
    @level2name = N'tr_StorageLocationProduct_Changes'