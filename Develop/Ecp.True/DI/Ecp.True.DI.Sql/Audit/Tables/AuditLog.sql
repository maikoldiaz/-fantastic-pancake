/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Dec-19-2019
-- Updated Date:	Mar-20-2020
                    May-15-2020 Removed NodeCode, StoreLocationCode columns and renamed PK to Identity column
 <Description>:     This table holds the data for the Audit Log.  </Description>

-- ================================================================================================================================*/
CREATE TABLE [Audit].[AuditLog]
(
	--Columns
	[AuditLogId]			INT IDENTITY (1, 1) NOT NULL, 
    [LogDate]				DATETIME			NOT NULL, 
    [LogType]				NVARCHAR(10)		NOT NULL, 
    [User]					NVARCHAR (260)		NOT NULL, 
    [OldValue]				NVARCHAR(MAX)		NULL, 
    [NewValue]				NVARCHAR(MAX)		NULL, 
    [Field]					NVARCHAR(50)		NOT NULL,
    [Entity]				NVARCHAR(50)		NOT NULL,
    [Identity]		        NVARCHAR(20)		NULL,

	--Constraints
	CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED ([AuditLogId] ASC)
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the log record',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'AuditLog',
    @level2type = N'COLUMN',
    @level2name = N'AuditLogId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The date of the log record',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'AuditLog',
    @level2type = N'COLUMN',
    @level2name = N'LogDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of operation of the log record ',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'AuditLog',
    @level2type = N'COLUMN',
    @level2name = N'LogType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'AuditLog',
    @level2type = N'COLUMN',
    @level2name = N'User'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value before the change',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'AuditLog',
    @level2type = N'COLUMN',
    @level2name = N'OldValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value after the change',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'AuditLog',
    @level2type = N'COLUMN',
    @level2name = N'NewValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the attribute that has been modified',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'AuditLog',
    @level2type = N'COLUMN',
    @level2name = N'Field'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the entity that has been modified',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'AuditLog',
    @level2type = N'COLUMN',
    @level2name = N'Entity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for the Audit Log.',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'AuditLog',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The primary key value of the logged table',
    @level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'TABLE',
    @level1name = N'AuditLog',
    @level2type = N'COLUMN',
    @level2name = N'Identity'