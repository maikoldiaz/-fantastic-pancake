/* ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jul-07-2020
-- Updated date:	Aug-10-2020 -- Added identity column
-- Description:     This Table is to Get TimeOne movement Owner Details Monthly Quality Data for Segment Category, Element, Node, StartDate, EndDate.
-- ==============================================================================================================================*/
CREATE TABLE [Admin].[OfficialMonthlyMovementQualityDetails]
(
  --Columns
  [RNo]                    INT                    NOT NULL,
  [System]                 NVARCHAR (150)         NULL,
  [Version]                NVARCHAR (50)          NULL,  
  [MovementId]             VARCHAR (50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
  [TypeMovement]           NVARCHAR (150)         NULL,
  [Movement]               NVARCHAR (150)	      NULL,
  [SourceNode]             NVARCHAR (150)         NULL,
  [DestinationNode]        NVARCHAR (150)         NULL,
  [SourceProduct]          NVARCHAR (150)         NULL,
  [DestinationProduct]     NVARCHAR (150)         NULL,
  [NetQuantity]            DECIMAL  (18,2)        NULL,
  [GrossQuantity]          DECIMAL  (18,2)        NULL,
  [MeasurementUnit]        VARCHAR  (300)         NULL,
  [Owner]                  NVARCHAR (150)         NULL,
  [Ownershipvolume]        DECIMAL  (18,2)        NULL,
  [Ownershippercentage]    DECIMAL  (18,2)        NULL,
  [Origin]                 NVARCHAR (150)         NULL,
  [RegistrationDate]       DATETIME               NULL,
  [Attribute]              NVARCHAR (150)         NULL,
  [AttributeValue]	       NVARCHAR (150)		  NULL,									
  [ValueAttributeUnit]	   NVARCHAR (50)		  NULL,										
  [AttributeDescription]   NVARCHAR (150)		  NULL,	
  [MovementTransactionId]  INT                    NULL,  
  [ExecutionId]			   INT         			  NOT NULL,
  
  --Internal Common Columns
  [CreatedBy]              NVARCHAR (260)         NOT NULL,
  [CreatedDate]            DATETIME               NOT NULL, 
    
    --Constraints
    CONSTRAINT [FK_OfficialMonthlyMovementQualityDetails_ReportExecution]	FOREIGN KEY ([ExecutionId])			REFERENCES [Admin].[ReportExecution] ([ExecutionId]),

  --Indexes
  [OfficialMonthlyMovementQualityDetailsId] INT IDENTITY (1,1) CONSTRAINT PK_Official_Monthly_Movement_Quality_Details_Id PRIMARY KEY CLUSTERED
)
GO

ALTER TABLE [Admin].[OfficialMonthlyMovementQualityDetails]	  SET (LOCK_ESCALATION = DISABLE)
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The correlative number of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'RNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The System Name',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'System'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version of the Movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'Version'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the Movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'MovementId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The type of the Movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'TypeMovement'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'Movement'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'SourceNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Net Quantity',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'NetQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of Gross Quantity',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'GrossQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the measurement units',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Name of the owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'Owner'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipVolume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipPercentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Origin of movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'Origin'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when record is registered',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'RegistrationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Id of the attribute',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'Attribute'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the attribute',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'AttributeValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the attribute unit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'ValueAttributeUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description of the attribute',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'AttributeDescription'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The transaction number of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Identity column',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'OfficialMonthlyMovementQualityDetailsId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for movement with owners before cutoff. This table is being used in before cutoff report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyMovementQualityDetails',
    @level2type = NULL,
    @level2name = NULL