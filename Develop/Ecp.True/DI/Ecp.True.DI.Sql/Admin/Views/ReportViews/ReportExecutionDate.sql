/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date:	Dec-02-2019
-- Updated date:	Mar-20-2020
-- <Description>:   This view is to get Current time in Colombian time zone </Description>:
-- ==============================================================================================================================*/
CREATE VIEW [Admin].[ReportExecutionDate]
AS
	SELECT GETDATE() AT TIME ZONE 'UTC' AT TIME ZONE 'SA Pacific Standard Time' AS ReportExecutionDate

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This view is to get Current time in Colombian time zone',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'ReportExecutionDate',
    @level2type = NULL,
    @level2name = NULL