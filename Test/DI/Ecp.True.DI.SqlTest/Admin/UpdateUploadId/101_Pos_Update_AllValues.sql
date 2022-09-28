﻿-------------------------------------------------------------------------------------------------------------------------------
--Insert sample data with error messages and value for both(HomologationInventoryBlobPath,HomologationMovementBlobPath)
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
'0B5D8AC0-FB0B-4D6E-BB31-ACAC0AE407BE',
'2019-09-23 10:38:25.923',
'FileName_0.xls',
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
DECLARE @UploadIdErrorMessagesType			Admin.UploadIdErrorMessagesType

INSERT INTO @UploadIdErrorMessagesType
(
	[TempId],
	[ErrorMessage]
)
SELECT 1,'Err1'
UNION
SELECT 2,'Err2'
UNION
SELECT 3,'Err3'

SELECT * FROM @UploadIdErrorMessagesType

SELECT IsHomologated,HomologationInventoryBlobPath,HomologationMovementBlobPath,* FROM [Admin].[FileRegistration]
WHERE UploadId = '0B5D8AC0-FB0B-4D6E-BB31-ACAC0AE407BE'
SELECT * FROM [Admin].FileRegistrationError

EXEC [Admin].[usp_UpdateUploadId] '0B5D8AC0-FB0B-4D6E-BB31-ACAC0AE407BE'
								 ,'MainFolderHomologationInventoryBlobPath/SubFolderHomologationInventoryBlobPath'
								 ,'MainFolderHomologationMovementBlobPath/SubFolderHomologationMovementBlobPath'
								 ,@UploadIdErrorMessagesType
								 ,'System'


SELECT IsHomologated,HomologationInventoryBlobPath,HomologationMovementBlobPath,* FROM [Admin].[FileRegistration]
SELECT * FROM [Admin].FileRegistrationError
