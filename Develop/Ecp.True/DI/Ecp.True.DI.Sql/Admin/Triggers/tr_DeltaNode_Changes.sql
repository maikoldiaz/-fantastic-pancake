/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Sept-4-2020
-- <Description>:  This trigger is used to insert data into DeltaNodeApprovalhistory for newly inserted data into DeltaNode table.  </Description>
-- ==============================================================================================================================*/


CREATE TRIGGER [Admin].[tr_DeltaNode_Changes]
ON [Admin].[DeltaNode]  
AFTER UPDATE, DELETE
AS
SET NOCOUNT ON;
BEGIN  

    DECLARE @utcDate DATETIME
    SET @utcDate = Admin.udf_GetTrueDate();
    IF EXISTS(SELECT * FROM DELETED) AND NOT EXISTS(SELECT * FROM INSERTED)
    BEGIN -- Delete case
        INSERT INTO [Admin].DeltaNodeApprovalHistory (TicketId, NodeId, [Date], [Status], [CreatedBy]) SELECT DELETED.TicketId, DELETED.NodeId, CASE WHEN DELETED.LastModifiedDate IS NULL THEN DELETED.CreatedDate ELSE DELETED.LastModifiedDate END,DELETED.STATUS, 'System' FROM DELETED;
    END
    IF EXISTS(SELECT * FROM DELETED) AND EXISTS(SELECT * FROM INSERTED)
    BEGIN	-- Update Case
        INSERT INTO [Admin].DeltaNodeApprovalHistory (TicketId, NodeId, [Date], [Status], [CreatedBy]) SELECT DELETED.TicketId, DELETED.NodeId, @utcDate,DELETED.STATUS, 'System' FROM DELETED;
    END  
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This trigger is used to insert data into DeltaNodeApprovalhistory for newly inserted data into DeltaNode table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNode',
    @level2type = N'TRIGGER',
    @level2name = N'tr_DeltaNode_Changes'