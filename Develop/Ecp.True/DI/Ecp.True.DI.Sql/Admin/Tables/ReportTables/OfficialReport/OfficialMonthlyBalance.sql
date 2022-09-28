/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jul-07-2020
-- Updated date: Aug-10-2020 -- Added identity column
-- Updated date: Aug-20-2020 -- Changed table structure to show the calculations properly in report
--<Description>: This table holds the data for balance page for monthly data.
================================================================================================================================*/
CREATE TABLE [Admin].[OfficialMonthlyBalance]
(
	--Columns
    [MeasurementUnit]               VARCHAR  (300)                      NULL,
    [Product]                       NVARCHAR (150)                      NULL,
    [SystemName]                    NVARCHAR (150)                      NULL,
    [Version]                       NVARCHAR (150)                      NULL,
    [Owner]                         NVARCHAR (150)                      NULL,
	[Input]                         DECIMAL (29,2)                      NULL,
	[Output]                        DECIMAL (29,2)                      NULL,
	[InitialInventory]              DECIMAL (29,2)                      NULL,
	[FinalInventory]                DECIMAL (29,2)                      NULL,
	[Control]                       DECIMAL (29,2)                      NULL,
    [ExecutionId]					INT         						NOT NULL,
	
	 --Internal Common Columns													
	 [CreatedBy]					NVARCHAR (260)						NOT NULL,
	 [CreatedDate]					DATETIME							NOT NULL,  

     --Constraints
     CONSTRAINT [FK_OfficialMonthlyBalance_ReportExecution]	FOREIGN KEY ([ExecutionId])			REFERENCES [Admin].[ReportExecution] ([ExecutionId]),

     --Indexes
     [OfficialMonthlyBalanceId] INT IDENTITY (1,1) CONSTRAINT PK_Official_Monthly_Balance_Id PRIMARY KEY CLUSTERED
)
GO


ALTER TABLE [Admin].[OfficialMonthlyBalance]	SET (LOCK_ESCALATION = DISABLE)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The value of the measurement units',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The Name of the Product ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'Product'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the SystemName',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the version',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'Version'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the owner',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'Owner'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the execution ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Identity column',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'OfficialMonthlyBalanceId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for summary before cutoff. This table is being used in before cutoff report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Input value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'Input'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Output value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'Output'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'InitialInventory value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'InitialInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'FinalInventory value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'FinalInventory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Control value of the record',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'OfficialMonthlyBalance',
    @level2type = N'COLUMN',
    @level2name = N'Control'