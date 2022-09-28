/*==============================================================================================================================
--Author:        Microsoft
--Created Date : Aug-30-2019
--Updated Date : Mar-20-2020
--<Description>: This table holds the data for Homologation service for the registration of data mapping between TRUE and other Ecopetrol systems.</Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[Homologation]
(
	--Columns
    [HomologationId]				INT IDENTITY (1, 1)		NOT NULL,
    [SourceSystemId]				INT						NOT NULL,
    [DestinationSystemId]			INT						NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)   NOT NULL,
	[CreatedDate]					DATETIME        NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
	[LastModifiedBy]				NVARCHAR (260)   NULL,
	[LastModifiedDate]				DATETIME        NULL,

	--Constraints
	CONSTRAINT [PK_Homologation]			PRIMARY KEY CLUSTERED	([HomologationId] ASC),
    CONSTRAINT [FK_Homologation_System1]	FOREIGN KEY	([SourceSystemId])						REFERENCES [Admin].[SystemType] ([SystemTypeId]),
    CONSTRAINT [FK_Homologation_System2]	FOREIGN KEY	([DestinationSystemId])					REFERENCES [Admin].[SystemType] ([SystemTypeId])
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the homologation',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Homologation',
    @level2type = N'COLUMN',
    @level2name = N'HomologationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source system (like for Sinoper, Excel, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Homologation',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystemId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination system (like for Sinoper, Excel, etc)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Homologation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationSystemId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Homologation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Homologation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Homologation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Homologation',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for Homologation service for the registration of data mapping between TRUE and other Ecopetrol systems.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Homologation',
    @level2type = NULL,
    @level2name = NULL