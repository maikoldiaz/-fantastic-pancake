/*-- =================================================================================================
-- Author:		  Microsoft
-- Create date:   Jul-08-2020
-- Updated date:  Jul-10-2020
-- <Description>: This View is to Fetch Data from Ticket Table for delta nodes. </Description>
-- ===================================================================================================*/

CREATE VIEW [Admin].[view_DeltaNode]
AS
SELECT   TIC.TicketId		AS [TicketId]
		,TIC.StartDate		AS [StartDate]
		,TIC.EndDate		AS [EndDate]
		,TIC.CreatedDate	AS [ExecutionDate]
		,TIC.CreatedBy		AS [CreatedBy]
		,OST.Name			AS [Status]
		,CE.[Name]			AS [Segment]
		,ND.Name			AS [NodeName]
		,ND.NodeId			AS [NodeId]
		,CE.ElementId		AS [SegmentId]
		,TIC.TicketTypeId	AS [TicketTypeId]
		,DN.DeltaNodeId		AS [DeltaNodeId]
		,DN.Status			AS [DeltaNodeStatus]
		,ST.StatusType		AS [TicketStatus]
		,TIC.Status			AS [TicketStatusId]
		,DN.lastModifiedDate as [LastModifiedDate]
FROM Admin.Ticket TIC
INNER JOIN Admin.DeltaNode DN
	ON Tic.TicketId = DN.TicketId
INNER JOIN Admin.CategoryElement CE
	On TIC.CategoryElementId = CE.ElementId
INNER JOIN Admin.OwnershipNodeStatusType OST
	ON OST.OwnershipNodeStatusTypeId = DN.Status
INNER JOIN Admin.StatusType ST
	ON ST.StatusTypeId = TIC.Status
LEFT JOIN Admin.Node ND
	ON ND.NodeId = DN.NodeId
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data from Ticket Table for delta nodes.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_DeltaNode',
    @level2type = NULL,
    @level2name = NULL
