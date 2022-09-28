/* ====================================================================================================================================================================================================================================================================
-- Author:			Microsoft
-- Created Date:	Oct-15-2019
-- Updated Date:	Mar-20-2020
-- <Description>:	This trigger reads CategoryElementId, StartDate, EndDate, Status, CreatedDate for inserts and CategoryElementId, StartDate, EndDate, Status, LastModifiedDate for updates in the Ticket table and log them in AuditLog table. </Description>
-- ===================================================================================================================================================================================================================================================================*/
CREATE TRIGGER [Admin].[tr_Ticket_Changes]
ON [Admin].[Ticket] 
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
				 		  del.[CategoryElementId]			AS OldValue, 
				 		  ins.[CategoryElementId]			AS NewValue, 
				 		  'CategoryElementId'				AS Field, 
				 		  'Admin.Ticket'		AS Entity, 
				 		  ins.TicketId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TicketId]  =  del.[TicketId]
				  WHERE ins.[CategoryElementId]  <> del.[CategoryElementId]
				  
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
				 		  del.[StartDate]		AS OldValue, 
				 		  ins.[StartDate]		AS NewValue, 
				 		  'StartDate'			AS Field, 
				 		  'Admin.Ticket'		AS Entity, 
				 		  ins.TicketId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TicketId]  =  del.[TicketId]
				  WHERE ins.[StartDate]  <> del.[StartDate] 
				  
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
				 		  del.[EndDate]		AS OldValue, 
				 		  ins.[EndDate]		AS NewValue, 
				 		  'EndDate'			AS Field, 
				 		  'Admin.Ticket'		AS Entity, 
				 		  ins.TicketId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TicketId]  =  del.[TicketId]
				  WHERE ins.[EndDate]  <> del.[EndDate] 
				  
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
				 		  del.[Status]			AS OldValue, 
				 		  ins.[Status]			AS NewValue, 
				 		  'Status'				AS Field, 
				 		  'Admin.Ticket'		AS Entity, 
				 		  ins.TicketId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TicketId]  =  del.[TicketId]
				  WHERE ins.[Status]  <> del.[Status] 
				  
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
				 		  'Admin.Ticket'		AS Entity, 
				 		  ins.TicketId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[TicketId]  =  del.[TicketId]
				  WHERE ins.[LastModifiedDate]  <> del.[LastModifiedDate]
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
                   inserted.[CategoryElementId], 
                   'CategoryElementId', 
                   'Admin.Ticket', 
                   inserted.TicketId 
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
                   'Admin.Ticket', 
                   inserted.TicketId 
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
                   'Admin.Ticket', 
                   inserted.TicketId 
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
                   inserted.[Status], 
                   'Status', 
                   'Admin.Ticket', 
                   inserted.TicketId 
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
                   'Admin.Ticket', 
                   inserted.TicketId 
            FROM   inserted; 
        END 		
		
  END 
  
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This trigger reads CategoryElementId, StartDate, EndDate, Status, CreatedDate for inserts and CategoryElementId, StartDate, EndDate, Status, LastModifiedDate for updates in the Ticket table and log them in AuditLog table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Ticket',
    @level2type = N'TRIGGER',
    @level2name = N'tr_Ticket_Changes'