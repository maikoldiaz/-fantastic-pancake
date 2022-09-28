-------------------------------------------------------------------------------------------------------------------------------
--Insert sample data with  error messages of big length and Special Characters values for HomologationInventoryBlobPath,HomologationMovementBlobPath
-------------------------------------------------------------------------------------------------------------------------------
DECLARE @UploadIdErrorMessagesType Admin.UploadIdErrorMessagesType
DECLARE @UploadID UNIQUEIDENTIFIER = '0B5D8AC0-FB0B-4D6E-BB31-ACAC0AE407BE'

INSERT INTO @UploadIdErrorMessagesType
(
	[TempId],
	[ErrorMessage]
)
SELECT 1, 'ErrMessage Sample 1'
UNION
SELECT 2,  'ErrMessage Sample 100'

SELECT * FROM @UploadIdErrorMessagesType

SELECT IsHomologated,HomologationInventoryBlobPath,HomologationMovementBlobPath,* FROM [Admin].[FileRegistration]
WHERE UploadId = @UploadID

SELECT COUNT(*) AS CountOfErrorMessages FROM [Admin].FileRegistrationError

EXEC [Admin].[usp_UpdateUploadId] @UploadID
								 ,'123@!@****456'
								 ,'123@!@****456'
								 ,@UploadIdErrorMessagesType
								 ,'System'


SELECT IsHomologated,HomologationInventoryBlobPath,HomologationMovementBlobPath,* FROM [Admin].[FileRegistration]
WHERE UploadId = @UploadID

SELECT TOP 2 * FROM [Admin].FileRegistrationError
ORDER BY CreatedDate Desc