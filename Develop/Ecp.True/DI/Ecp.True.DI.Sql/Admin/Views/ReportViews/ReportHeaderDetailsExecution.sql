-- ======================================================================================================================
-- Author: InterGrupo
-- Create date:  Sep-02-2021
-- <Description>: This View is to Fetch header details For PowerBi Report From Tables(ReportExecution, ScenarioType)</Description>
-- ======================================================================================================================
CREATE VIEW [Admin].[ReportHeaderDetailsExecution]
AS

	select Re.ExecutionId,Re.ScenarioId,St.Name Scenario 
	from admin.ReportExecution Re
	inner join admin.ScenarioType St on St.ScenarioTypeId = Re.ScenarioId 
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
@value = N'This View is to Fetch header details For PowerBi Report From Tables(ReportExecution, ScenarioType)',
@level0type = N'SCHEMA',
@level0name = N'Admin',
@level1type = N'VIEW',
@level1name = N'ReportHeaderDetailsExecution',
@level2type = NULL,
@level2name = NULL
