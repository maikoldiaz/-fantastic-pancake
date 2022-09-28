/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jul-07-2020
--<Description>: This table holds the data for Sap Mapping information with names of entities</Description>
================================================================================================================================*/
CREATE TABLE [Admin].[SapMappingDetail]
(
	--Columns
	[Sistema origen]						NVARCHAR (150)							,
	[Tipo de movimiento_o]				    NVARCHAR (150)					,
	[Producto_o]						    NVARCHAR (150)				,
	[Nodo origen_o]			                NVARCHAR (150),
	[Nodo destino_o]		                NVARCHAR (150),
	[Sistema destino]					    NVARCHAR (150),
	[Tipo de movimiento_d]			        NVARCHAR (150),
	[Producto_d]				            NVARCHAR (150),
	[Nodo origen_d]		                    NVARCHAR (150),
	[Nodo destino_d]	                    NVARCHAR (150),
	[Sistema oficial]					    NVARCHAR (150),
  
    --Internal Common Columns                                                   
    [CreatedBy]                    			NVARCHAR (260)          NOT NULL,
    [CreatedDate]                  			DATETIME                NOT NULL    DEFAULT Admin.udf_GetTrueDate()
);
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for Sap Mapping information with names of entities.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name or id of source system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = N'COLUMN',
    @level2name = N'Sistema origen'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name or id of source movement type ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = N'COLUMN',
    @level2name = N'Tipo de movimiento_o'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name or id of source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = N'COLUMN',
    @level2name = N'Producto_o'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name or id of source node in source system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = N'COLUMN',
    @level2name = N'Nodo origen_o'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'he name or id of destination node in source system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = N'COLUMN',
    @level2name = N'Nodo destino_o'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name or id of destination system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = N'COLUMN',
    @level2name = N'Sistema destino'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name or id of destination system movement type ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = N'COLUMN',
    @level2name = N'Tipo de movimiento_d'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name or id of destination system product  ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = N'COLUMN',
    @level2name = N'Producto_d'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name or id of source node of destination system  ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = N'COLUMN',
    @level2name = N'Nodo origen_d'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name or id of destination node of destination system  ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = N'COLUMN',
    @level2name = N'Nodo destino_d'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name or id of official system ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = N'COLUMN',
    @level2name = N'Sistema oficial'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'SapMappingDetail',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'