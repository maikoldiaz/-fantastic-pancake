/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Dec-19-2019
-- Updated Date:	Mar-20-2020
 <Description>:		This table holds the data for the Audit Status.  </Description>

-- ================================================================================================================================*/
CREATE TABLE [Audit].[AuditStatus]
(
	--Columns
	[StatusId]			INT	IDENTITY (1, 1)				NOT NULL,
	[Status]			VARCHAR (20)					NOT NULL,

	--Constraints
	CONSTRAINT [PK_AuditStatus] PRIMARY KEY CLUSTERED ([StatusId] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier to the status',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'AuditStatus',
    @level2type = N'COLUMN',
    @level2name = N'StatusId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the status',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'AuditStatus',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for the Audit Status.',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'AuditStatus',
    @level2type = NULL,
    @level2name = NULL