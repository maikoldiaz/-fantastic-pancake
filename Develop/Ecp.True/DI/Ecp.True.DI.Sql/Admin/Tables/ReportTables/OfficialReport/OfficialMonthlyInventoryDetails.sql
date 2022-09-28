/* ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jul-07-2020
-- Updated date:	Aug-10-2020 -- Added identity column
-- Description:     This Table is to Get TimeOne Inventory Owner Monthly Details Data for Segment Category, Element, Node, StartDate, EndDate.
-- ==============================================================================================================================*/
CREATE TABLE [Admin].[OfficialMonthlyInventoryDetails]
(
	--Columns
	[RNo]							INT						NOT NULL,
    [System]                        NVARCHAR (150)          NULL,
    [Version]                       NVARCHAR (50)           NULL,
	[InventoryId]					VARCHAR  (50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
    [NodeName]                      NVARCHAR (150)			NULL,
    [Product]		                NVARCHAR (150)    		NULL,
    [NetStandardVolume]             DECIMAL  (18,2)   		NULL,
    [GrossStandardQuantity]         DECIMAL  (18,2)         NULL, 
    [MeasurementUnit]			    NVARCHAR (150)			NULL,
    [Owner]                         NVARCHAR (150)          NULL,
    [OwnershipVolume]               DECIMAL  (18,2)         NULL,
	[OwnershipPercentage]           DECIMAL  (5,2)          NULL,
    [Origin]                        NVARCHAR (150)          NULL,
    [RegistrationDate]              DATETIME                NULL,
	[InventoryProductId]			INT                     NULL,
    [ExecutionId]					INT         			NOT NULL,
          
     --Internal Common Columns                                                   
    [CreatedBy]                    	NVARCHAR (260)          NOT NULL,
    [CreatedDate]                  	DATETIME                NOT NULL, 
    
    --Constraints
    CONSTRAINT [FK_OfficialMonthlyInventoryDetails_ReportExecution]	FOREIGN KEY ([ExecutionId])			REFERENCES [Admin].[ReportExecution] ([ExecutionId]),
    
    --Indexes
    [OfficialMonthlyInventoryDetailsId] INT IDENTITY (1,1) CONSTRAINT PK_Official_Monthly_Inventory_Details_Id PRIMARY KEY CLUSTERED
)
GO

ALTER TABLE [Admin].[OfficialMonthlyInventoryDetails]	SET (LOCK_ESCALATION = DISABLE)
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The correlative number of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'RNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The System Name',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'System'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The version of the Movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'Version'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the inventory',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'InventoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'NodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'Product'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the net standard volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of GrossStandardQuantity',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the measurement units',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Name of the owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'Owner'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipVolume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of OwnershipPercentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'OwnershipPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Origin of movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'Origin'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when record is registered',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'RegistrationDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the inventory product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'InventoryProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Identity column',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = N'COLUMN',
    @level2name = N'OfficialMonthlyInventoryDetailsId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for inventories with owners before cutoff. This table is being used in before cutoff report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyInventoryDetails',
    @level2type = NULL,
    @level2name = NULL
