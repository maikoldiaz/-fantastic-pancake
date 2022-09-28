/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Dec-10-2021
-- Description:     This Procedure is used to delete the report data from Cut Off related tables.
EXEC [Admin].[usp_Cleanup_CutOff_ReportData] -1
-- ==============================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_Cleanup_CutOff_ReportData] 
(
 @TicketId INT
)
AS
BEGIN

   DELETE FROM [Admin].AttributeDetailsWithoutOwner WHERE TicketId=@TicketId

   DELETE FROM [Admin].BackupMovementDetailsWithoutOwner WHERE TicketId=@TicketId

   DELETE FROM [Admin].InventoryDetailsWithoutOwner WHERE TicketId=@TicketId

   DELETE FROM [Admin].KPIDataByCategoryElementNode WHERE TicketId=@TicketId

   DELETE FROM [Admin].KPIPreviousDateDataByCategoryElementNode WHERE TicketId=@TicketId

   DELETE FROM [Admin].MovementDetailsWithoutOwner WHERE TicketId=@TicketId

   DELETE FROM [Admin].MovementsByProductWithoutOwner WHERE TicketId=@TicketId

   DELETE FROM [Admin].BalanceControl WHERE TicketId=@TicketId
END

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to delete the report data from Cut Off related tables.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_Cleanup_CutOff_ReportData',
    @level2type = NULL,
    @level2name = NULL