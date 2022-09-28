/*-- =================================================================================================
-- Author:		Microsoft
-- Create date: Dec-14-2019
-- Updated date1: Dec-20-2019 :	Added TicketTypeId, CategoryName
-- Updated date2: Dec-23-2019 :	Added OwnerName.
-- Updated date3: May-04-2021 : Remove convert text from 'Propiedad' to 'Finalizado'
-- <Description>:	This View is to Fetch Data from Ticket Table </Description>
-- ===================================================================================================*/

CREATE VIEW [Admin].[view_Ticket]
AS
SELECT   TIC.TicketId		AS [ticketId]
		,TIC.TicketTypeId	AS [ticketTypeId]
		,CE.[Name]			AS [segment]
		,CAT.[Name]			AS [categoryName]
		,TIC.StartDate		AS [ticketStartDate]
		,TIC.EndDate		AS [ticketFinalDate]
		,TIC.CreatedDate	AS [cutoffExecutionDate]
		,TIC.CreatedBy		AS [createdBy]
		,CEOwner.[Name]		AS [ownerName]
		,TIC.ErrorMessage	AS [errorMessage]
		,TIC.BlobPath		AS [blobPath]
		,CEScenario.[Name]	AS [scenarioName]
		--> Rule: When TicketTypeId = 2 (Ownership) And status is Processing, then send the Status as 'Sent' (Enviado), else as it is.
        ,ST.StatusType      AS [state]
		,CASE WHEN TIC.TicketTypeId = 3 OR TIC.TicketTypeId = 6 --Logistic OR OfficialLogistics
			  THEN ISNULL(Nd.Name,'Todos')
			  ELSE NULL
			  END AS NodeName
FROM Admin.Ticket TIC
INNER JOIN Admin.CategoryElement CE
	On TIC.CategoryElementId = CE.ElementId
INNER JOIN Admin.StatusType ST
	ON ST.StatusTypeId = TIC.Status
INNER JOIN Admin.Category CAT
	ON CAT.CategoryId = CE.CategoryId
LEFT JOIN Admin.CategoryElement CEOwner
	ON TIC.OwnerId = CEOwner.ElementId
LEFT JOIN Admin.ScenarioType CEScenario
	ON TIC.ScenarioTypeId = CEScenario.ScenarioTypeId
LEFT JOIN Admin.Node ND
ON ND.NodeId = Tic.NodeId

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data from Ticket Table',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_Ticket',
    @level2type = NULL,
    @level2name = NULL
