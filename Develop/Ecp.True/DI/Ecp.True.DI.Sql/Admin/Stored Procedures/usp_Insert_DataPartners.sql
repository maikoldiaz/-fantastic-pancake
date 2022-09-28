/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-12-2020
-- Update date: 	Aug-13-2020 Raising an error in case of failure
-- Description:     This Procedure is to insert the partner Owner data from stage table to main table.
-- EXEC [Admin].[usp_Insert_DataPartners]
   SELECT * FROM [Admin].[PartnerOwnerMapping]
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_Insert_DataPartners]
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRAN
	      BEGIN TRY
                DELETE FROM [Admin].[PartnerOwnerMapping]

				INSERT INTO [Admin].[PartnerOwnerMapping] (GrandOwnerId,PartnerOwnerId)
	            SELECT GrandOwnerId,PartnerOwnerId  FROM [Admin].[Stage_PartnerOwnerMapping]

                COMMIT TRAN
          END TRY
          BEGIN CATCH
		        DECLARE @ERROR NVARCHAR (MAX) = ERROR_MESSAGE()
		        ROLLBACK TRAN
				RAISERROR (@ERROR,16,1)
          END CATCH

END


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to insert the partner Owner data from stage table to main table.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_Insert_DataPartners',
    @level2type = NULL,
    @level2name = NULL
