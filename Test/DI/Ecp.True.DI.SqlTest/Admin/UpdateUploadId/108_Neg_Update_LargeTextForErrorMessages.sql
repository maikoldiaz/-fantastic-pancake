-------------------------------------------------------------------------------------------------------------------------------
--Insert sample data with out error messages of big length and Same values for HomologationInventoryBlobPath,HomologationMovementBlobPath
-------------------------------------------------------------------------------------------------------------------------------
DECLARE @UploadIdErrorMessagesType Admin.UploadIdErrorMessagesType
DECLARE @UploadID UNIQUEIDENTIFIER = '0B5D8AC0-FB0B-4D6E-BB31-ACAC0AE407BE'

INSERT INTO @UploadIdErrorMessagesType
(
	[TempId],
	[ErrorMessage]
)
SELECT 1, 'Message1 This is the Error Message length testing This is the Error Message length testing This is the Error Message length testing This is the Error Message length testing This is the Error Message length testing This is the Error Message length testing Message Ending'
UNION
SELECT 2, 'Message2 This is the Error Message length testing This is the Error Message length testing This is the Error Message length testing This is the Error Message length testing This is the Error Message length testing This is the Error Message length testing  Message Ending'

SELECT * FROM @UploadIdErrorMessagesType

SELECT IsHomologated,HomologationInventoryBlobPath,HomologationMovementBlobPath,* FROM [Admin].[FileRegistration]
WHERE UploadId = @UploadID

SELECT COUNT(*) AS CountOfErrorMessages FROM [Admin].FileRegistrationError

EXEC [Admin].[usp_UpdateUploadId] @UploadID
								 ,'MainFolder\SubFolder'
								 ,'MainFolder\SubFolder'
								 ,@UploadIdErrorMessagesType
								 ,'System'


SELECT IsHomologated,HomologationInventoryBlobPath,HomologationMovementBlobPath,* FROM [Admin].[FileRegistration]
WHERE UploadId = @UploadID
SELECT TOP 2 * FROM [Admin].FileRegistrationError
ORDER BY CreatedDate Desc

