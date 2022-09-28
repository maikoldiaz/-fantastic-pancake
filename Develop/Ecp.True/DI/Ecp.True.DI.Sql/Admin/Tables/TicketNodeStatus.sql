/*==============================================================================================================================
--Author:        Microsoft
--Created date : Mar-27-2020
--Updated date : Apr-09-2020
--<Description>: This table is created to store Node statuses for a ticket and is used to create Power Bi report.
--Update: Added additional columns</Description>
================================================================================================================================*/
CREATE TABLE admin.TicketNodeStatus
(
 OwnershipNodeId				INT				NULL,	
 TicketId						INT				NULL,
 Startdate						DATETIME		NULL,
 Enddate						DATETIME		NULL,
 NodeId							INT				NULL,
 SegmentId						INT				NULL,
 SegmentName					VARCHAR(150)	NULL,
 SystemId						INT				NULL,
 SystemName						VARCHAR(150)	NULL,
 NodeName						VARCHAR(150)	NULL,
 OwnershipNodeStatusId          INT             NOT NULL,
 statusNode						VARCHAR(150)	NOT NULL,
 StatusDateChange				DATETIME		NULL,
 Approver					    NVARCHAR(50)	NULL,
 Comment						NVARCHAR(200)	NULL,
 ExecutionId					NVARCHAR (250)	NOT NULL,
 ReportConfiguartionValue       INT             NOT NULL,
 CalculatedDays                 NVARCHAR (250)  NULL,
 NotInApprovedState             INT             NOT NULL,

 --Internal Common Columns								
 [CreatedBy]					NVARCHAR (260)	NOT NULL,
 [CreatedDate]					DATETIME		NOT NULL    DEFAULT Admin.udf_GetTrueDate()
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ownership node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of ticket',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'TicketId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the ticket is started',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'Startdate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the ticket is ended',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'Enddate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'the identifier of the segment (category element of segment category)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the segment',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'SegmentName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the system (category element of system category)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'SystemId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the ownership node status',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipNodeStatusId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the status node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'statusNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the status changed',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'StatusDateChange'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the approver',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'Approver'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The comment provided by the user',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'Comment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The execution id unique to session received from UI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The number of days used for CASOS SIN GESTIÓN OPORTUNA (ANS) chart',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'ReportConfiguartionValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The number of the calculate days',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'CalculatedDays'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag indicating if the ticket is in approved state or not, 1 means approved state',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'NotInApprovedState'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table is created to store Node statuses for a ticket and is used to create Power Bi report.',
    @level0type = N'SCHEMA',
    @level0name = N'admin',
    @level1type = N'TABLE',
    @level1name = N'TicketNodeStatus',
    @level2type = NULL,
    @level2name = NULL