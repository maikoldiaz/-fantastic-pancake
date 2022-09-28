/* ===========================================================================================================
-- Author:		Microsoft
-- Create date: Oct-15-2019
-- Description:	This Trigger is to Track the Insert and Update Changes in the table([Offchain].[Inventory] ) 
--				and Log them in [Audit].[AuditLog] 
-- ===========================================================================================================*/
CREATE TRIGGER [Offchain].[tr_Inventory_Changes]
ON [Offchain].[Inventory] 
AFTER INSERT, UPDATE 
AS 
    SET NOCOUNT ON; 

  BEGIN 
	  DECLARE @TodaysDate DATETIME = Getutcdate()	
	  
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
							   NodeCode
							   ) 
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[SystemTypeId]			AS OldValue, 
				 		  ins.[SystemTypeId]			AS NewValue, 
				 		  'SystemTypeId'				AS Field, 
				 		  'Offchain.Inventory'		AS Entity, 
				 		  ins.InventoryTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[InventoryTransactionId]  =  del.[InventoryTransactionId]
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
							   NodeCode
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[SourceSystem]		AS OldValue, 
				 		  ins.[SourceSystem]		AS NewValue, 
				 		  'SourceSystem'			AS Field, 
				 		  'Offchain.Inventory'		AS Entity, 
				 		  ins.InventoryTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[InventoryTransactionId]  =  del.[InventoryTransactionId]
				  WHERE ins.[SourceSystem]  <> del.[SourceSystem]; 
				  
				  INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   NodeCode
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[DestinationSystem]		AS OldValue, 
				 		  ins.[DestinationSystem]		AS NewValue, 
				 		  'DestinationSystem'			AS Field, 
				 		  'Offchain.Inventory'		AS Entity, 
				 		  ins.InventoryTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[InventoryTransactionId]  =  del.[InventoryTransactionId]
				  WHERE ins.[DestinationSystem]  <> del.[DestinationSystem]; 
				  
				  INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   NodeCode
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[EventType]			AS OldValue, 
				 		  ins.[EventType]			AS NewValue, 
				 		  'EventType'				AS Field, 
				 		  'Offchain.Inventory'		AS Entity, 
				 		  ins.InventoryTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[InventoryTransactionId]  =  del.[InventoryTransactionId]
				  WHERE ins.[EventType]  <> del.[EventType]; 

				   INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   NodeCode
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[TankName]			AS OldValue, 
				 		  ins.[TankName]			AS NewValue, 
				 		  'TankName'				AS Field, 
				 		  'Offchain.Inventory'		AS Entity, 
				 		  ins.InventoryTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[InventoryTransactionId]  =  del.[InventoryTransactionId]
				  WHERE ins.[TankName]  <> del.[TankName];

				   INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   NodeCode
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[InventoryId]			AS OldValue, 
				 		  ins.[InventoryId]			AS NewValue, 
				 		  'InventoryId'				AS Field, 
				 		  'Offchain.Inventory'		AS Entity, 
				 		  ins.InventoryTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[InventoryTransactionId]  =  del.[InventoryTransactionId]
				  WHERE ins.[InventoryId]  <> del.[InventoryId];

				   INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   NodeCode
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[TicketId]			AS OldValue, 
				 		  ins.[TicketId]			AS NewValue, 
				 		  'TicketId'				AS Field, 
				 		  'Offchain.Inventory'		AS Entity, 
				 		  ins.InventoryTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[InventoryTransactionId]  =  del.[InventoryTransactionId]
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
							   NodeCode
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[InventoryDate]			AS OldValue, 
				 		  ins.[InventoryDate]			AS NewValue, 
				 		  'InventoryDate'				AS Field, 
				 		  'Offchain.Inventory'		AS Entity, 
				 		  ins.InventoryTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[InventoryTransactionId]  =  del.[InventoryTransactionId]
				  WHERE ins.[InventoryDate]  <> del.[InventoryDate];

				   INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   NodeCode
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[NodeId]			AS OldValue, 
				 		  ins.[NodeId]			AS NewValue, 
				 		  'NodeId'				AS Field, 
				 		  'Offchain.Inventory'		AS Entity, 
				 		  ins.InventoryTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[InventoryTransactionId]  =  del.[InventoryTransactionId]
				  WHERE ins.[NodeId]  <> del.[NodeId];

				   INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   NodeCode
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[Observations]			AS OldValue, 
				 		  ins.[Observations]			AS NewValue, 
				 		  'Observations'				AS Field, 
				 		  'Offchain.Inventory'		AS Entity, 
				 		  ins.InventoryTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[InventoryTransactionId]  =  del.[InventoryTransactionId]
				  WHERE ins.[Observations]  <> del.[Observations];

				   INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   NodeCode
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[Scenario]			AS OldValue, 
				 		  ins.[Scenario]			AS NewValue, 
				 		  'Scenario'				AS Field, 
				 		  'Offchain.Inventory'		AS Entity, 
				 		  ins.InventoryTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[InventoryTransactionId]  =  del.[InventoryTransactionId]
				  WHERE ins.[Scenario]  <> del.[Scenario];

				   INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   NodeCode
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[IsDeleted]			AS OldValue, 
				 		  ins.[IsDeleted]			AS NewValue, 
				 		  'IsDeleted'				AS Field, 
				 		  'Offchain.Inventory'		AS Entity, 
				 		  ins.InventoryTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[InventoryTransactionId]  =  del.[InventoryTransactionId]
				  WHERE ins.[IsDeleted]  <> del.[IsDeleted];

				  INSERT INTO [Audit].[AuditLog] 
							  (
							   LogDate, 
							   LogType, 
							   [User], 
							   OldValue, 
							   NewValue, 
							   Field, 
							   Entity, 
							   NodeCode
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[LastModifiedDate]AS OldValue, 
				 		  ins.[LastModifiedDate]AS NewValue, 
				 		  'LastModifiedDate'	AS Field, 
				 		  'Offchain.Inventory'		AS Entity, 
				 		  ins.InventoryTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[InventoryTransactionId]  =  del.[InventoryTransactionId]
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
                         NodeCode) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[SystemTypeId], 
                   'SystemTypeId', 
                   'Offchain.Inventory', 
                   inserted.InventoryTransactionId 
            FROM   inserted; 

            INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         NodeCode) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[SourceSystem], 
                   'SourceSystem', 
                   'Offchain.Inventory', 
                   inserted.InventoryTransactionId 
            FROM   inserted; 

            INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         NodeCode) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[DestinationSystem], 
                   'DestinationSystem', 
                   'Offchain.Inventory', 
                   inserted.InventoryTransactionId 
            FROM   inserted; 

            INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         NodeCode) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[EventType], 
                   'EventType', 
                   'Offchain.Inventory', 
                   inserted.InventoryTransactionId 
            FROM   inserted; 

			INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         NodeCode) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[TankName], 
                   'TankName', 
                   'Offchain.Inventory', 
                   inserted.InventoryTransactionId 
            FROM   inserted; 

			INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         NodeCode) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[InventoryId], 
                   'InventoryId', 
                   'Offchain.Inventory', 
                   inserted.InventoryTransactionId 
            FROM   inserted; 

			INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         NodeCode) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[TicketId], 
                   'TicketId', 
                   'Offchain.Inventory', 
                   inserted.InventoryTransactionId 
            FROM   inserted; 

			INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         NodeCode) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[InventoryDate], 
                   'InventoryDate', 
                   'Offchain.Inventory', 
                   inserted.InventoryTransactionId 
            FROM   inserted; 

			INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         NodeCode) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[NodeId], 
                   'NodeId', 
                   'Offchain.Inventory', 
                   inserted.InventoryTransactionId 
            FROM   inserted; 

			INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         NodeCode) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[Observations], 
                   'Observations', 
                   'Offchain.Inventory', 
                   inserted.InventoryTransactionId 
            FROM   inserted; 

			INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         NodeCode) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[Scenario], 
                   'Scenario', 
                   'Offchain.Inventory', 
                   inserted.InventoryTransactionId 
            FROM   inserted; 

			INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         NodeCode) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[IsDeleted], 
                   'IsDeleted', 
                   'Offchain.Inventory', 
                   inserted.InventoryTransactionId 
            FROM   inserted; 

            INSERT INTO [Audit].[AuditLog] 
                        (LogDate, 
                         LogType, 
                         [User], 
                         NewValue, 
                         Field, 
                         Entity, 
                         NodeCode) 
            SELECT @TodaysDate, 
                   'Insert', 
                   inserted.CreatedBy, 
                   inserted.[CreatedDate], 
                   'CreatedDate', 
                   'Offchain.Inventory', 
                   inserted.InventoryTransactionId 
            FROM   inserted; 
        END 		
		
  END 