/* ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-21-2020
-- Update date: 	Aug-24-2020 Added a new column (NodeName)
--                  Added segmentId and NodeId
-- Update date: 	Aug-26-2020 Added 2 new columns Start and End Date.
-- Description:     This Table is to store official delta movements.
-- ==============================================================================================================================*/
CREATE TABLE [Report].[OfficialDeltaMovements]
(
  --Columns
  [StartDate]              DATE                   NOT NULL,
  [EndDate]                DATE                   NOT NULL,  
  [SegmentId]              INT                    NULL,
  [NodeId]                 INT                    NULL,
  [NodeName]               NVARCHAR (150)         NULL,  
  [Version]                NVARCHAR (50)          NULL,  
  [Scenario]               NVARCHAR (50)          NULL,
  [TypeMovement]           NVARCHAR (150)         NULL,
  [Movement]               NVARCHAR (150)	      NULL,
  [SourceNodeId]           INT                    NULL,
  [SourceNode]             NVARCHAR (150)         NULL,
  [DestinationNodeId]      INT                    NULL,
  [DestinationNode]        NVARCHAR (150)         NULL,
  [SourceProduct]          NVARCHAR (150)         NULL,
  [DestinationProduct]     NVARCHAR (150)         NULL,
  [NetQuantity]            DECIMAL (18,2)         NULL,
  [GrossQuantity]          DECIMAL (18,2)         NULL,
  [MeasurementUnit]        VARCHAR (300)          NULL,
  [Owner]                  NVARCHAR (150)         NULL,
  [OwnershipVolume]        DECIMAL (18,2)         NULL,
  [OwnershipPercentage]    DECIMAL (18,2)         NULL,
  [Origin]                 NVARCHAR (150)         NULL,
  [MovementTransactionId]  INT                    NULL,
  [ExecutionDate]          DATETIME               NULL,
  
  --Internal Common Columns
  [CreatedBy]              NVARCHAR (260)         NOT NULL,
  [CreatedDate]            DATETIME               NOT NULL,
  [LastModifiedBy]         NVARCHAR (260)   NOT NULL,
  [LastModifiedDate]       DATETIME         NOT NULL
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Start date of the movement ',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'End date of the movement ',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The segment id ',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'SegmentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The segment id ',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'NodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The node name of the Movement',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version of the Movement',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'Version'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the Movement',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'TypeMovement'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'Movement'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'SourceNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Net Quantity',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'NetQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Gross Quantity',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'GrossQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the measurement units',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Name of the owner',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'Owner'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipVolume',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipPercentage',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Origin of movement',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'Origin'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The transaction number of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'TExecution date of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Scenario type of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'Scenario'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Source Node Id of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Destination Node Id of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'who updated the record for the last time (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'when the record updated for the last time (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = N'COLUMN',
    @level2name = N'LastModifiedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Table is to store official delta movements',
    @level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'TABLE',
    @level1name = N'OfficialDeltaMovements',
    @level2type = NULL,
    @level2name = NULL