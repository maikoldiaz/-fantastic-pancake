/*-- =================================================================================================
-- Author:		Microsoft
-- Create date: Jan-05-2020
-- Updated date: Jan-08-2020; added OwnershipNodeId column
-- Updated date: May-06-2020; added isTransferPoint, nodeId and OwnershipStatusId columns
-- <Description>:	This View is to Fetch Data from OwnershipNode and Ticket Table.</Description>
-- ===================================================================================================*/


CREATE VIEW [Admin].[view_OwnerShipNode]
AS
SELECT	 OWN.OwnershipNodeId	AS [ownershipNodeId]
		,OWN.NodeId				AS [nodeId]
		,TIC.TicketId			AS [ticketId]
		,TIC.TicketTypeId		AS [ticketTypeId]
		,CE.[Name]				AS [segment]
		,CAT.[Name]				AS [categoryName]
		,TIC.StartDate			AS [ticketStartDate]
		,TIC.EndDate			AS [ticketFinalDate]
		,TIC.CreatedDate		AS [cutoffExecutionDate]
		,TIC.CreatedBy			AS [createdBy]
		,CEOwner.[Name]			AS [ownerName]
		,TIC.ErrorMessage		AS [errorMessage]
		,TIC.BlobPath			AS [blobPath]
		,ND.[Name]				AS [nodeName]
		,ONST.[Name]			AS [state]
		,[Admin].[udf_NodeIsTransferPoint](OWN.NodeId) AS [isTransferPoint]
		,OWN.OwnershipStatusId  AS [OwnershipStatusId]

FROM Admin.OwnershipNode OWN
INNER JOIN Admin.Node ND
	ON OWN.NodeId = ND.NodeId
INNER JOIN Admin.Ticket TIC
	ON OWN.TicketId = TIC.TicketId
INNER JOIN Admin.CategoryElement CE
	ON TIC.CategoryElementId = CE.ElementId
INNER JOIN Admin.Category CAT
	ON CAT.CategoryId = CE.CategoryId
INNER JOIN Admin.OwnershipNodeStatusType ONST
	ON ONST.OwnershipNodeStatusTypeId = OWN.OwnershipStatusId
LEFT JOIN Admin.CategoryElement CEOwner
	ON TIC.OwnerId = CEOwner.ElementId

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data from OwnershipNode and Ticket Table.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_OwnerShipNode',
    @level2type = NULL,
    @level2name = NULL
