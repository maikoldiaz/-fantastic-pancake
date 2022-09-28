/* ===========================================================================================================
-- Author:		    Microsoft
-- Created Date:    Sep-21-2019
-- Updated Date:	Mar-20-2020
-- <Description>:	This trigger is to track the insert and update changes in the NodeTag table and log them in AuditLog table. </Description>
-- ===========================================================================================================*/
CREATE TRIGGER [Admin].[tr_NodeTag_Changes] 
ON [Admin].[NodeTag] 
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
				 		  del.[NodeId]			AS OldValue, 
				 		  ins.[NodeId]			AS NewValue, 
				 		  'NodeId'				AS Field, 
				 		  'Admin.NodeTag'		AS Entity, 
				 		  ins.NodeTagId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[NodeTagId]  =  del.[NodeTagId]
				  WHERE ins.[NodeId]  <> del.[NodeId]
				  UNION
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[ElementId]		AS OldValue, 
				 		  ins.[ElementId]		AS NewValue, 
				 		  'ElementId'			AS Field, 
				 		  'Admin.NodeTag'		AS Entity, 
				 		  ins.NodeTagId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[NodeTagId]  =  del.[NodeTagId]
				  WHERE ins.[ElementId]  <> del.[ElementId] 
				  UNION
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[StartDate]		AS OldValue, 
				 		  ins.[StartDate]		AS NewValue, 
				 		  'StartDate'			AS Field, 
				 		  'Admin.NodeTag'		AS Entity, 
				 		  ins.NodeTagId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[NodeTagId]  =  del.[NodeTagId]
				  WHERE ins.[StartDate]  <> del.[StartDate] 
				  UNION
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[EndDate]			AS OldValue, 
				 		  ins.[EndDate]			AS NewValue, 
				 		  'EndDate'				AS Field, 
				 		  'Admin.NodeTag'		AS Entity, 
				 		  ins.NodeTagId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[NodeTagId]  =  del.[NodeTagId]
				  WHERE ins.[EndDate]  <> del.[EndDate] 
				  UNION
 				  SELECT  @TodaysDate			AS LogDate, 
				 		  'Update'				AS LogType, 
				 		  ins.[LastModifiedBy]	AS [User], 
				 		  del.[LastModifiedDate]AS OldValue, 
				 		  ins.[LastModifiedDate]AS NewValue, 
				 		  'LastModifiedDate'	AS Field, 
				 		  'Admin.NodeTag'		AS Entity, 
				 		  ins.NodeTagId			AS [Identity]
				  FROM   inserted ins
				  INNER JOIN deleted del
				  ON ins.[NodeTagId]  =  del.[NodeTagId]
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
                   inserted.[NodeId], 
                   'NodeId', 
                   'Admin.NodeTag', 
                   inserted.NodeTagId 
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
                   inserted.[ElementId], 
                   'ElementId', 
                   'Admin.NodeTag', 
                   inserted.NodeTagId 
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
                   'Admin.NodeTag', 
                   inserted.NodeTagId 
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
                   'Admin.NodeTag', 
                   inserted.NodeTagId 
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
                   'Admin.NodeTag', 
                   inserted.NodeTagId 
            FROM   inserted; 
        END 		
		
  END 

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This trigger is to track the insert and update changes in the NodeTag table and log them in AuditLog table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'NodeTag',
    @level2type = N'TRIGGER',
    @level2name = N'tr_NodeTag_Changes'