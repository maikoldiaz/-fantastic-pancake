/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-20-2020
--Updated date : Aug-20-2020  Changed the datatype of ProductId
--                            Added SegmentId column
--<Description>: This table holds the pre calculated data for balance official initial cargido </Description>
=================================================================================================================================*/
CREATE TABLE [Report].[OfficialDeltaBalance] 
    (
      [ElementOwnerId]                          INT                             NOT NULL,
      [SegmentId]                               INT                             NOT NULL,
      [NodeId]                                  INT                             NOT NULL,
      [ProductId]                               NVARCHAR (20)                   NOT NULL,
      [StartDate]                               DATE                            NOT NULL,
      [EndDate]                                 DATE                            NOT NULL,
      [MeasurementUnit]                         NVARCHAR (150)                  NULL,
      [Product]                                 NVARCHAR (150)                  NULL,
      [Owner]                                   NVARCHAR (150)                  NULL,
      [Partner]                                 NVARCHAR (150)                  NULL,
      [Input]                                   DECIMAL (21,2)                  NULL,
      [Output]                                  DECIMAL (21,2)                  NULL,
      [DeltaInput]                              DECIMAL (21,2)                  NULL,
      [DeltaOutput]                             DECIMAL (21,2)                  NULL,
      [InitialInventory]                        DECIMAL (21,2)                  NULL,
      [FinalInventory]                          DECIMAL (21,2)                  NULL,
      [DeltaInitialInventory]                   DECIMAL (21,2)                  NULL,
      [DeltaFinalInventory]                     DECIMAL (21,2)                  NULL,
      [Control]                                 DECIMAL (21,2)                  NULL,
       
       --Internal Common Columns 
      [CreatedBy]                               NVARCHAR (260)                  NULL, 
      [CreatedDate]                             DATETIME                        NULL,
      [LastModifiedBy]                          NVARCHAR (260)                  NULL, 
      [LastModifiedDate]                        DATETIME                        NULL,
    )
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Node id of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Node id of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Start Date of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'End Date of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Input value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'Input'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Output value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'Output'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Delta input value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'DeltaInput'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Delta output value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'DeltaOutput'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'InitialInventory value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'InitialInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'FinalInventory value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'FinalInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'DeltaInitialInventory value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'DeltaInitialInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'DeltaFinalInventory value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'DeltaFinalInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Control value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'Control'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'when the record is created',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Who created the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Element owner id the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'ElementOwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ProductId of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Measurement Unit of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Product Name of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'Product'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Owner Name of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'Owner'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Partner Name of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'Partner'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Who modified the record for the last time',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'When the record modified for the last time',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the pre calculated data for balance official initial cargido',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaBalance',
    @level2type = NULL,
    @level2name = NULL
