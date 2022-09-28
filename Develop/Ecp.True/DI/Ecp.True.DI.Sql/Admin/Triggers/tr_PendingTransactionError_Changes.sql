/* ===========================================================================================================
-- Author:			Microsoft
-- Created Date:	Oct-15-2019
-- Updated Date:	Mar-20-2020
-- <Description>:	This trigger is to track the insert and update changes in the PendingTransactionError table and log them in AuditLog table.  </Description>
-- ===========================================================================================================*/
CREATE TRIGGER [Admin].[tr_PendingTransactionError_Changes]
ON [Admin].[PendingTransactionError] 
AFTER INSERT, UPDATE 
AS 
    SET NOCOUNT ON; 

  BEGIN 
	  DECLARE @TodaysDate DATETIME = Admin.udf_GetTrueDate()	
	  
      IF EXISTS(SELECT 1 
                FROM   DELETED) 
        BEGIN 
				  -- Updated Case 
				  INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   [Identity]
							   ) 
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[TransactionId]			AS OldValue, 
				 		  ins.[TransactionId]			AS NewValue, 
				 		  'TransactionId'				AS Field, 
				 		  'Admin.PendingTransactionError'		AS Entity, 
				 		  ins.ErrorId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[ErrorId]  =  del.[ErrorId]
				  WHERE ins.[TransactionId]  <> del.[TransactionId];
				  
				  INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   [Identity]
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[ErrorMessage]		AS OldValue, 
				 		  ins.[ErrorMessage]		AS NewValue, 
				 		  'ErrorMessage'			AS Field, 
				 		  'Admin.PendingTransactionError'		AS Entity, 
				 		  ins.ErrorId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[ErrorId]  =  del.[ErrorId]
				  WHERE ins.[ErrorMessage]  <> del.[ErrorMessage]; 
				  
				  INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   [Identity]
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[Comment]		AS OldValue, 
				 		  ins.[Comment]		AS NewValue, 
				 		  'Comment'			AS Field, 
				 		  'Admin.PendingTransactionError'		AS Entity, 
				 		  ins.ErrorId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[ErrorId]  =  del.[ErrorId]
				  WHERE ins.[Comment]  <> del.[Comment]; 
				  
				  INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   [Identity]
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[LastModifiedDate]AS OldValue, 
				 		  ins.[LastModifiedDate]AS NewValue, 
				 		  'LastModifiedDate'	AS Field, 
				 		  'Admin.PendingTransactionError'		AS Entity, 
				 		  ins.ErrorId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[ErrorId]  =  del.[ErrorId]
				  WHERE ins.[LastModifiedDate]  <> del.[LastModifiedDate];
        END 
      ELSE 
        BEGIN -- New Insert Case 
            INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         [Identity]) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[TransactionId], 
                   'TransactionId', 
                   'Admin.PendingTransactionError', 
                   inserted.ErrorId 
            FROM   inserted; 

            INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         [Identity]) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[ErrorMessage], 
                   'ErrorMessage', 
                   'Admin.PendingTransactionError', 
                   inserted.ErrorId 
            FROM   inserted; 

            INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         [Identity]) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[Comment], 
                   'Comment', 
                   'Admin.PendingTransactionError', 
                   inserted.ErrorId 
            FROM   inserted; 

            INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         [Identity]) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[CreatedDate], 
                   'CreatedDate', 
                   'Admin.PendingTransactionError', 
                   inserted.ErrorId 
            FROM   inserted; 
        END 		
		
  END 
  
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This trigger is to track the insert and update changes in the PendingTransactionError table and log them in AuditLog table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransactionError',
    @level2type = N'TRIGGER',
    @level2name = N'tr_PendingTransactionError_Changes'