/* ================================================================================================================================================================================================
-- Author:		    Microsoft
-- Created Date:    Oct-15-2019
-- Updated Date:	Mar-20-2020
--<Description>:	This trigger reads UnbalanceId, TicketId, NodeId, ProductId, Status, CreatedDate for insertions in the UnbalanceComment table and log them in AuditLog table. </Description>
-- ================================================================================================================================================================================================*/
CREATE TRIGGER [Admin].[tr_UnBalanceComment_Changes]
ON [Admin].[UnbalanceComment] 
AFTER INSERT, UPDATE 
AS 
    SET NOCOUNT ON; 

  BEGIN 
	  DECLARE @TodaysDate DATETIME = Admin.udf_GetTrueDate()	
	  
      IF EXISTS(SELECT 1 
                FROM   DELETED) 
        BEGIN 
				  -- Updated Case 
				  -- Currently there are no Update requirements in UnBalance Comment table.
				  SELECT 'X' FROM [Admin].[UnbalanceComment] where 1 = 2;
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
                   inserted.[UnbalanceId], 
                   'UnbalanceId', 
                   'Admin.UnbalanceComment', 
                   inserted.[UnbalanceId] 
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
                   'Admin.UnbalanceComment', 
                   inserted.[UnbalanceId] 
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
                   inserted.[NodeId], 
                   'NodeId', 
                   'Admin.UnbalanceComment', 
                   inserted.[UnbalanceId] 
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
                   inserted.[ProductId], 
                   'ProductId', 
                   'Admin.UnbalanceComment', 
                   inserted.[UnbalanceId] 
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
                   'Admin.UnbalanceComment', 
                   inserted.[UnbalanceId] 
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
                   'Admin.UnbalanceComment', 
                   inserted.[UnbalanceId] 
            FROM   inserted; 
        END 		
		
  END 

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This trigger reads UnbalanceId, TicketId, NodeId, ProductId, Status, CreatedDate for insertions in the UnbalanceComment table and log them in AuditLog table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'UnbalanceComment',
    @level2type = N'TRIGGER',
    @level2name = N'tr_UnbalanceComment_Changes'