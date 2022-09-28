/*==============================================================================================================================
--Author:        Microsoft
--Created Date : July-29-2020
--Updated Date : 
--<Description>: This table holds the data for delta node approvers per levels.  </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[DeltaNodeApproval]
(
	[DeltaNodeApprovalId]			INT IDENTITY (1, 1)		   NOT NULL,
    [NodeId]						INT						   NOT NULL,
    [Level]                         INT                        NOT NULL,
    [Approvers]                     NVARCHAR(1000)             NOT NULL,

	--Internal Common Columns
	[CreatedBy]											NVARCHAR (260)			NOT NULL    DEFAULT 'ADF',
	[CreatedDate]										DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]									NVARCHAR (260)			NULL,
	[LastModifiedDate]									DATETIME				NULL,

	--Constraints
	CONSTRAINT [PK_DeltaNodeApproval]						PRIMARY KEY CLUSTERED ([DeltaNodeApprovalId] ASC),
    CONSTRAINT [FK_DeltaNodeApproval_Node]					FOREIGN KEY ([NodeId])						REFERENCES [Admin].[Node] ([NodeId]),
    CONSTRAINT [Unique_DeltaNodeApproval]                   UNIQUE                                      ([NodeId], [Level])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of delta node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApproval',
    @level2type = N'COLUMN',
    @level2name = N'DeltaNodeApprovalId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApproval',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The level',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApproval',
    @level2type = N'COLUMN',
    @level2name = N'Level'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The approvers',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApproval',
    @level2type = N'COLUMN',
    @level2name = N'Approvers'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApproval',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApproval',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApproval',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApproval',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for delta node approvers per levels.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'DeltaNodeApproval',
    @level2type = NULL,
    @level2name = NULL