/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Sept-09-2020
-- Description:     This Procedure is used to delete the data from Inventory Movement index tables.
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_Cleanup_InventoryMovementIndex]

AS
BEGIN
    BEGIN TRY
		    BEGIN TRANSACTION
             TRUNCATE TABLE [Admin].[InventoryMovementIndex]
            COMMIT TRANSACTION
    END TRY

	BEGIN CATCH
	       ROLLBACK TRANSACTION
	END CATCH
END


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to delete the data from Inventory Movement index tables.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_Cleanup_InventoryMovementIndex',
    @level2type = NULL,
    @level2name = NULL