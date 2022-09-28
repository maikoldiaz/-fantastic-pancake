-------------------------------------------------------------------------------------------------------------------------------
--Insert sample data with error messages and value for Only(HomologationMovementBlobPath)
-------------------------------------------------------------------------------------------------------------------------------
INSERT INTO [Admin].[FileRegistration]
           ([UploadId]
           ,[UploadDate]
           ,[Name]
           ,[Action]
           ,[Status]
           ,[SystemTypeId]
           ,[RecordsProcessed]
           ,[IsActive]
           ,[IsHomologated]
           ,[BlobPath]
           ,[HomologationInventoryBlobPath]
           ,[HomologationMovementBlobPath]
           ,[PreviousUploadId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[LastModifiedBy]
           ,[LastModifiedDate])
SELECT 
'A4CE15CF-E78F-4588-BAA4-5A622B4C05C7',
'2019-09-23 10:38:25.923',
'FileName_2.xls',
2,
1,
3,
0,
0,	
0,	
'/container/registerfiles',
NULL,
NULL,
NULL,
'system',
'2019-09-23 10:38:25.923',
NULL,
NULL

DECLARE @UploadIdErrorMessagesType Admin.UploadIdErrorMessagesType

INSERT INTO @UploadIdErrorMessagesType
(
	[TempId],
	[ErrorMessage]
)
SELECT 1, 'Err7'
UNION
SELECT 2, 'Err8'

SELECT * FROM @UploadIdErrorMessagesType

SELECT IsHomologated,HomologationInventoryBlobPath,HomologationMovementBlobPath,* FROM [Admin].[FileRegistration]
WHERE UploadId = 'A4CE15CF-E78F-4588-BAA4-5A622B4C05C7'

SELECT * FROM [Admin].FileRegistrationError

EXEC [Admin].[usp_UpdateUploadId] 'A4CE15CF-E78F-4588-BAA4-5A622B4C05C7'
								 ,NULL
								 ,'MainFolderHomologationMovementBlobPath/SubFolderHomologationMovementBlobPath'
								 ,@UploadIdErrorMessagesType
								 ,'System'


SELECT IsHomologated,HomologationInventoryBlobPath,HomologationMovementBlobPath,* FROM [Admin].[FileRegistration]
SELECT * FROM [Admin].FileRegistrationError


