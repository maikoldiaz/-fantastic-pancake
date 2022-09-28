/*-- =================================================================================================
-- Author:		Microsoft
-- Create date: Aug-12-2020
-- <Description>:	This View is to Fetch Data from Report Execution Table </Description>
-- ===================================================================================================*/
CREATE VIEW [Admin].[view_ReportExecution]
AS
	SELECT RE.ExecutionId, RE.CategoryId, CE.ElementId, RE.NodeId,
	CASE WHEN RE.CategoryId = 2 THEN CE.[Name] END AS Segment,
	CASE WHEN RE.CategoryId = 8 THEN CE.[Name] END AS [System],
	ISNULL(ND.Name,iif(RE.ReportTypeId != 6,'Todos',NULL)) AS NodeName,iif(RE.ReportTypeId != 6,StartDate,'9999/12/31') StartDate, iif(RE.ReportTypeId != 6,EndDate,'9999/12/31') EndDate , StatusType,
	RT.[Name] AS ReportType, SCT.[Name] as ScenarioType,
	CASE WHEN RE.[Name] = 'OperativeBalance' THEN 'Balance operativo'
		WHEN RE.[Name] = 'WithoutCutoff' THEN 'Balance operativo'
		WHEN RE.[Name] = 'NonSonWithOwnerReport' THEN 'Balance operativo con propiedad'
		WHEN RE.[Name] = 'OfficialInitialBalanceReport' THEN 'Balance oficial inicial cargado'
		WHEN RE.[Name] = 'SapBalance' THEN 'Estado de envíos a SAP'
		WHEN RE.[Name] = 'UserGroupAccessReport' THEN 'Accesos grupos de usuarios'
		WHEN RE.[Name] = 'UserGroupAssignmentReport' THEN 'Asignación grupo de usuarios'
		WHEN RE.[Name] = 'UserGroupAndAssignedUserAccessReport' THEN 'Accesos grupos de usuarios y usuarios asignados'
	END AS ReportName, RE.[Name] AS [Name], RE.CreatedDate
	FROM Admin.ReportExecution RE
	LEFT JOIN Admin.Category CAT ON CAT.CategoryId=RE.CategoryId
	LEFT JOIN Admin.CategoryElement CE ON CE.ElementId=RE.ElementId
	LEFT JOIN Admin.Node ND ON ND.NodeId=RE.NodeId
	INNER JOIN Admin.StatusType ST ON ST.StatusTypeId=RE.StatusTypeId
	INNER JOIN Admin.ReportType RT ON RT.ReportTypeId=RE.ReportTypeId
	INNER JOIN Admin.ScenarioType SCT ON SCT.ScenarioTypeId=RE.ScenarioId
	WHERE RE.ReportTypeId <> 2
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data from Report Execution Table',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_ReportExecution',
    @level2type = NULL,
    @level2name = NULL