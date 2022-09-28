/* ===========================================================================================================
-- Author:		Microsoft
-- Create date: Oct-15-2019
-- Description:	This Trigger is to Track the Insert and Update Changes in the table([Offchain].[Movement] ) 
--				and Log them in [Audit].[AuditLog] 
-- ===========================================================================================================*/
CREATE TRIGGER [Offchain].[tr_Movement_Changes]
ON [Offchain].[Movement] 
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
				 		  del.[MessageTypeId]			AS OldValue, 
				 		  ins.[MessageTypeId]			AS NewValue, 
				 		  'MessageTypeId'				AS Field, 
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
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
							   NodeCode
							   )
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[SystemTypeId]		AS OldValue, 
				 		  ins.[SystemTypeId]		AS NewValue, 
				 		  'SystemTypeId'			AS Field, 
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
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
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
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
				 		  del.[EventType]			AS OldValue, 
				 		  ins.[EventType]			AS NewValue, 
				 		  'EventType'				AS Field, 
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
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
				 		  del.[MovementId]			AS OldValue, 
				 		  ins.[MovementId]			AS NewValue, 
				 		  'MovementId'				AS Field, 
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
				  WHERE ins.[MovementId]  <> del.[MovementId];

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
				 		  del.[MovementTypeId]			AS OldValue, 
				 		  ins.[MovementTypeId]			AS NewValue, 
				 		  'MovementTypeId'				AS Field, 
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
				  WHERE ins.[MovementTypeId]  <> del.[MovementTypeId];

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
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
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
				 		  del.[OperationalDate]			AS OldValue, 
				 		  ins.[OperationalDate]			AS NewValue, 
				 		  'OperationalDate'				AS Field, 
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
				  WHERE ins.[OperationalDate]  <> del.[OperationalDate];

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
				 		  del.[GrossStandardVolume]			AS OldValue, 
				 		  ins.[GrossStandardVolume]			AS NewValue, 
				 		  'GrossStandardVolume'				AS Field, 
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
				  WHERE ins.[GrossStandardVolume]  <> del.[GrossStandardVolume];

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
				 		  del.[NetStandardVolume]			AS OldValue, 
				 		  ins.[NetStandardVolume]			AS NewValue, 
				 		  'NetStandardVolume'				AS Field, 
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
				  WHERE ins.[NetStandardVolume]  <> del.[NetStandardVolume];

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
				 		  del.[MeasurementUnit]			AS OldValue, 
				 		  ins.[MeasurementUnit]			AS NewValue, 
				 		  'MeasurementUnit'				AS Field, 
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
				  WHERE ins.[MeasurementUnit]  <> del.[MeasurementUnit];

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
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
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
				 		  del.[Observations]			AS OldValue, 
				 		  ins.[Observations]			AS NewValue, 
				 		  'Observations'				AS Field, 
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
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
				 		  del.[Classification]			AS OldValue, 
				 		  ins.[Classification]			AS NewValue, 
				 		  'Classification'				AS Field, 
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
				  WHERE ins.[Classification]  <> del.[Classification];

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
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
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
				 		  del.[VariableTypeId]			AS OldValue, 
				 		  ins.[VariableTypeId]			AS NewValue, 
				 		  'VariableTypeId'				AS Field, 
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
				  WHERE ins.[VariableTypeId]  <> del.[VariableTypeId];

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
				 		  'Offchain.Movement'		AS Entity, 
				 		  ins.MovementTransactionId			AS NodeCode
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[MovementTransactionId]  =  del.[MovementTransactionId]
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
                   inserted.[MessageTypeId], 
                   'MessageTypeId', 
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   inserted.[SystemTypeId], 
                   'SystemTypeId', 
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   inserted.[MovementId], 
                   'MovementId', 
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   inserted.[MovementTypeId], 
                   'MovementTypeId', 
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   inserted.[OperationalDate], 
                   'OperationalDate', 
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   inserted.[GrossStandardVolume], 
                   'GrossStandardVolume', 
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   inserted.[NetStandardVolume], 
                   'NetStandardVolume', 
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   inserted.[MeasurementUnit], 
                   'MeasurementUnit', 
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   inserted.[Classification], 
                   'Classification', 
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   inserted.[VariableTypeId], 
                   'VariableTypeId', 
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
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
                   'Offchain.Movement', 
                   inserted.MovementTransactionId 
            FROM   inserted; 
        END 		
		
  END 