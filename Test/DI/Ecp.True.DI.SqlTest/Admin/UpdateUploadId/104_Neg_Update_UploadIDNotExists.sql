-------------------------------------------------------------------------------------------------------------------------------
--Insert sample data with error messages and value for UploadId Not Existing DB '4DFCCF1D-C1C6-4665-B47C-881EC91AAC59'
-------------------------------------------------------------------------------------------------------------------------------
DECLARE @UploadIdErrorMessagesType Admin.UploadIdErrorMessagesType
DECLARE @UploadID UNIQUEIDENTIFIER = NEWID()

INSERT INTO @UploadIdErrorMessagesType
(
	[TempId],
	[ErrorMessage]
)
SELECT 1, 'Err9'
UNION
SELECT 2, 'Err10'

SELECT * FROM @UploadIdErrorMessagesType

SELECT IsHomologated,HomologationInventoryBlobPath,HomologationMovementBlobPath,* FROM [Admin].[FileRegistration]
WHERE UploadId = @UploadID

SELECT * FROM [Admin].FileRegistrationError

EXEC [Admin].[usp_UpdateUploadId] @UploadID
								 ,'MainFolderHomologationInventoryBlobPath/SubFolderHomologationInventoryBlobPath'
								 ,'MainFolderHomologationMovementBlobPath/SubFolderHomologationMovementBlobPath'
								 ,@UploadIdErrorMessagesType
								 ,'System'


SELECT IsHomologated,HomologationInventoryBlobPath,HomologationMovementBlobPath,* FROM [Admin].[FileRegistration]
SELECT * FROM [Admin].FileRegistrationError

