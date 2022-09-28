/*-- =========================================================================================================================================
-- Author:			Microsoft
-- Created Date:	Sep-21-2019
-- Updated Date:	Mar-20-2020
--                  May-15-2020  Removed "RecordsProcessed" column as it is deleted from origin table and removed @RecordsProcessed variable
-- <Description>:	This Procedure is used to update the Homologation Blob Path and the Error Messages for a given Upload Id. </Description>
-- ===========================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_UpdateUploadId]
(
	@UploadID							VARCHAR(50),
	@HomologationInventoryBlobPath		NVARCHAR(MAX) = NULL,
	@HomologationMovementBlobPath		NVARCHAR(MAX) = NULL,
	@UploadIdErrorMessagesType			Admin.UploadIdErrorMessagesType READONLY,
	@CreatedBy							NVARCHAR (260) NULL

)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
		   IF EXISTS(SELECT 'X' FROM [Admin].[FileRegistration] WHERE UploadId = @UploadID)
		   BEGIN
				DECLARE	 @FileRegistrationID		INT		 = 0,
						 @CreatedOrLastModifiedDate DATETIME = Admin.udf_GetTrueDate()


				--Update the IsHomologated = 1 and HomologationBlobPath based on the input parameters(@UploadID,@HomologationInventoryBlobPath)  
				UPDATE	FR
				SET   HomologationInventoryBlobPath = CASE WHEN @HomologationInventoryBlobPath IS NULL 
														   THEN HomologationInventoryBlobPath 
														   ELSE @HomologationInventoryBlobPath END 
					 ,HomologationMovementBlobPath  = CASE WHEN @HomologationMovementBlobPath  IS NULL 
														   THEN HomologationMovementBlobPath  
														   ELSE @HomologationMovementBlobPath END 
					 ,LastModifiedBy			    = @CreatedBy
					 ,LastModifiedDate				= @CreatedOrLastModifiedDate						 
				FROM [Admin].[FileRegistration] FR
				WHERE UploadId = @UploadID

				--Get the FileRegistrationID of the UploadId Passed
				SELECT @FileRegistrationID = [FileRegistrationId]
				FROM [Admin].[FileRegistration] FR
				WHERE UploadId = @UploadID

				--Insert the error Messages into Table FileRegistrationError For the associated FileRegistrationId based on UploadId
				INSERT INTO [Admin].[FileRegistrationError]
						   (
							  [FileRegistrationId]
							 ,[ErrorMessage]
							 ,[CreatedBy]
							 ,[CreatedDate]
						   )
				SELECT	 	 @FileRegistrationID AS [FileRegistrationId]
							 ,ErrorMessage AS ErrorMessage
							 ,@CreatedBy AS [CreatedBy]
							 ,@CreatedOrLastModifiedDate AS [CreatedDate]
				FROM @UploadIdErrorMessagesType
		END
		ELSE
		BEGIN
			RAISERROR ('UploadId Doesn''t Exists',16,1)
		END

		COMMIT TRANSACTION
	END TRY

	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		THROW
	END CATCH
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to update the Homologation Blob Path and the Error Messages for a given Upload Id.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_UpdateUploadId',
    @level2type = NULL,
    @level2name = NULL