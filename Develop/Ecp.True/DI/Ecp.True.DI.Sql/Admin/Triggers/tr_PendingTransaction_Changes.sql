/* ===========================================================================================================
-- Author:			Microsoft
-- Created Date:	Oct-15-2019
-- Updated Date:	Mar-20-2020
-- <Description>:	This trigger is to track the insert and update changes in the PendingTransaction table and log them in AuditLog table. </Description>
-- ===========================================================================================================*/
CREATE TRIGGER [Admin].[tr_PendingTransaction_Changes]
ON [Admin].[PendingTransaction] 
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
				 		  del.[MessageTypeId]			AS OldValue, 
				 		  ins.[MessageTypeId]			AS NewValue, 
				 		  'MessageTypeId'				AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[MessageTypeId]  <> del.[MessageTypeId];
				  
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
				 		  del.[BlobName]		AS OldValue, 
				 		  ins.[BlobName]		AS NewValue, 
				 		  'BlobName'			AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[BlobName]  <> del.[BlobName];
				  
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
				 		  del.[MessageId]		AS OldValue, 
				 		  ins.[MessageId]		AS NewValue, 
				 		  'MessageId'			AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[MessageId]  <> del.[MessageId];
				  
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
				 		  del.[ErrorJson]			AS OldValue, 
				 		  ins.[ErrorJson]			AS NewValue, 
				 		  'ErrorJson'				AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[ErrorJson]  <> del.[ErrorJson];

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
				 		  del.[SourceNodeId]			AS OldValue, 
				 		  ins.[SourceNodeId]			AS NewValue, 
				 		  'SourceNodeId'				AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[SourceNodeId]  <> del.[SourceNodeId];

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
				 		  del.[DestinationNodeId]			AS OldValue, 
				 		  ins.[DestinationNodeId]			AS NewValue, 
				 		  'DestinationNodeId'				AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[DestinationNodeId]  <> del.[DestinationNodeId];

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
				 		  del.[SourceProductId]			AS OldValue, 
				 		  ins.[SourceProductId]			AS NewValue, 
				 		  'SourceProductId'				AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[SourceProductId]  <> del.[SourceProductId];

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
				 		  del.[DestinationProductId]			AS OldValue, 
				 		  ins.[DestinationProductId]			AS NewValue, 
				 		  'DestinationProductId'				AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[DestinationProductId]  <> del.[DestinationProductId];

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
				 		  del.[ActionTypeId]			AS OldValue, 
				 		  ins.[ActionTypeId]			AS NewValue, 
				 		  'ActionTypeId'				AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[ActionTypeId]  <> del.[ActionTypeId];

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
				 		  del.[Volume]			AS OldValue, 
				 		  ins.[Volume]			AS NewValue, 
				 		  'Volume'				AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[Volume]  <> del.[Volume];

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
				 		  del.[Units]			AS OldValue, 
				 		  ins.[Units]			AS NewValue, 
				 		  'Units'				AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[Units]  <> del.[Units];

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
				 		  del.[StartDate]			AS OldValue, 
				 		  ins.[StartDate]			AS NewValue, 
				 		  'StartDate'				AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[StartDate]  <> del.[StartDate];

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
				 		  del.[EndDate]			AS OldValue, 
				 		  ins.[EndDate]			AS NewValue, 
				 		  'EndDate'				AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[EndDate]  <> del.[EndDate];
				  
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
				 		  del.[TicketId]			AS OldValue, 
				 		  ins.[TicketId]			AS NewValue, 
				 		  'TicketId'				AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[TicketId]  <> del.[TicketId];

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
				 		  del.[SystemTypeId]			AS OldValue, 
				 		  ins.[SystemTypeId]			AS NewValue, 
				 		  'SystemTypeId'				AS Field, 
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
				  WHERE ins.[SystemTypeId]  <> del.[SystemTypeId];

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
				 		  'Admin.PendingTransaction'		AS Entity, 
				 		  ins.TransactionId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TransactionId]  =  del.[TransactionId]
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
                   inserted.[MessageTypeId], 
                   'MessageTypeId', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[BlobName], 
                   'BlobName', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[MessageId], 
                   'MessageId', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[ErrorJson], 
                   'ErrorJson', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[SourceNodeId], 
                   'SourceNodeId', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[DestinationNodeId], 
                   'DestinationNodeId', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[SourceProductId], 
                   'SourceProductId', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[DestinationProductId], 
                   'DestinationProductId', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[ActionTypeId], 
                   'ActionTypeId', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[Volume], 
                   'Volume', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[Units], 
                   'Units', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[StartDate], 
                   'StartDate', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[EndDate], 
                   'EndDate', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[TicketId], 
                   'TicketId', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   inserted.[SystemTypeId], 
                   'SystemTypeId', 
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
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
                   'Admin.PendingTransaction', 
                   inserted.TransactionId 
            FROM   inserted; 
        END 		
		
  END 

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This trigger is to track the insert and update changes in the PendingTransaction table and log them in AuditLog table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PendingTransaction',
    @level2type = N'TRIGGER',
    @level2name = N'tr_PendingTransaction_Changes'