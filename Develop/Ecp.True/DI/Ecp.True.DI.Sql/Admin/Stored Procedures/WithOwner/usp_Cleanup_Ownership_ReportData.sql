/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Dec-10-2021
-- Description:     This Procedure is used to delete the report data from Ownership related tables.
EXEC [Admin].[usp_Cleanup_Ownership_ReportData] -1
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_Cleanup_Ownership_ReportData] 
(
 @OwnershipTicketId INT,
 @NodeId INT = NULL
)
AS
BEGIN

    DECLARE @NodeName	 NVARCHAR(2000)
    IF @NodeId IS NOT NULL
    SET @NodeName =  (select Name from Admin.Node where NodeId = @NodeId)

	
   DELETE FROM [Admin].AttributeDetailsWithOwner WHERE OwnershipTicketId=@OwnershipTicketId AND (@NodeName IS NULL OR (SourceNodename = @NodeName or DestinationNodeName = @NodeName));

   DELETE FROM [Admin].BackupMovementDetailsWithOwner WHERE OwnershipTicketId=@OwnershipTicketId AND (@NodeName IS NULL OR (SourceNode = @NodeName or DestinationNode = @NodeName));

   DELETE FROM [Admin].InventoryDetailsWithOwner WHERE OwnershipTicketId=@OwnershipTicketId AND (@NodeName IS NULL OR NodeName = @NodeName);

   DELETE FROM [Admin].KPIDataByCategoryElementNodeWithOwnership WHERE OwnershipTicketId=@OwnershipTicketId AND (@NodeName IS NULL OR NodeName = @NodeName);

   DELETE FROM [Admin].KPIPreviousDateDataByCategoryElementNodeWithOwner WHERE OwnershipTicketId=@OwnershipTicketId AND (@NodeName IS NULL OR NodeNamePrev = @NodeName);

   DELETE FROM [Admin].MovementDetailsWithOwner WHERE OwnershipTicketId=@OwnershipTicketId AND (@NodeName IS NULL OR (SourceNode = @NodeName or DestinationNode = @NodeName));

   DELETE FROM [Admin].MovementsByProductWithOwner WHERE OwnershipTicketId=@OwnershipTicketId AND (@NodeName IS NULL OR NodeName = @NodeName);

   DELETE FROM [Admin].QualityDetailsWithOwner WHERE OwnershipTicketId=@OwnershipTicketId AND (@NodeName IS NULL OR NodeName = @NodeName);

   DELETE FROM [Admin].movementDetailsWithOwnerOtherSegment WHERE OwnershipTicketId=@OwnershipTicketId AND (@NodeName IS NULL OR (SourceNode = @NodeName or DestinationNode = @NodeName));


END

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to delete the report data from Ownership related tables.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_Cleanup_Ownership_ReportData',
    @level2type = NULL,
    @level2name = NULL
