/*==============================================================================================================================
--Author:        Microsoft
--Created date : Feb-12-2020
--Updated date : Mar-20-2020
--<Description>: This table holds the data to be shown in the hidden pages of the reports (Portada and Bitcora). This is a master table and has seeded data. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[ReportTemplateConfiguration]
(
       -- Columns
		[TemplateConfigurationId]			INT IDENTITY (1, 1)		NOT NULL,
		[ReportIdentifier]					NVARCHAR(250)			NOT NULL,
		[Area]								NVARCHAR(250)			NOT NULL,
		[InformationResponsible]			NVARCHAR(250)			NOT NULL,
		[Frequency]							NVARCHAR(250)			NOT NULL,
		[InformationSource]					NVARCHAR(400)			NOT NULL,
		[Datamart]							NVARCHAR(250)			NOT NULL,
		[Version]							NVARCHAR(250)			NOT NULL,
		[UpdateDate]						DATETIME				NOT NULL,
		[ChangeResponsible]					NVARCHAR(250)			NOT NULL,

       -- Internal Common Columns
		[CreatedBy]							NVARCHAR (260)			NOT NULL,
		[CreatedDate]						DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),
		[LastModifiedBy]					NVARCHAR (260)			NULL,
		[LastModifiedDate]					DATETIME				NULL,

       -- Constraints
		CONSTRAINT [PK_TemplateConfiguration] PRIMARY KEY CLUSTERED ([TemplateConfigurationId] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data to be shown in the hidden pages of the reports (Portada and Bitcora). This is a master table and has seeded data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier for the template configuration',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'TemplateConfigurationId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the PBIX file',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'ReportIdentifier'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Nombre del área que genera el reporte',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'Area'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Responsable por la información publicada',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'InformationResponsible'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Frecuencia de Actualización:',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'Frequency'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fuentes Principales de Información',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'InformationSource'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version of the report',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'Version'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fecha Actualización Informe',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'UpdateDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Responsable del Cambio',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'ChangeResponsible'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The data mart name',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'Datamart'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The modifier of the record, normally system  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is updated  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'ReportTemplateConfiguration',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'