-------------------------------------------------------------------------------------------------------------------------------
--Insert sample data with out error messages and No values for HomologationInventoryBlobPath,HomologationMovementBlobPath
-------------------------------------------------------------------------------------------------------------------------------
DECLARE @UploadIdErrorMessagesType Admin.UploadIdErrorMessagesType
DECLARE @UploadID UNIQUEIDENTIFIER = '0B5D8AC0-FB0B-4D6E-BB31-ACAC0AE407BE'

--INSERT INTO @UploadIdErrorMessagesType
--SELECT 'Err9'
--UNION
--SELECT 'Err10'

SELECT * FROM @UploadIdErrorMessagesType

SELECT IsHomologated,HomologationInventoryBlobPath,HomologationMovementBlobPath,* FROM [Admin].[FileRegistration]
WHERE UploadId = @UploadID

SELECT COUNT(*) AS CountOfErrorMessages FROM [Admin].FileRegistrationError

EXEC [Admin].[usp_UpdateUploadId] @UploadID
								 ,NULL
								 ,NULL
								 ,@UploadIdErrorMessagesType
								 ,'System'


SELECT IsHomologated,HomologationInventoryBlobPath,HomologationMovementBlobPath,* FROM [Admin].[FileRegistration]
WHERE UploadId = @UploadID
SELECT COUNT(*)AS CountOfErrorMessages  FROM [Admin].FileRegistrationError
