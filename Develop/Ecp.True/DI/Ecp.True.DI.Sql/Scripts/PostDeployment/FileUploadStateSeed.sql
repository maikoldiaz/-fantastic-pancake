/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF	OBJECT_ID('Admin.FileUploadState') IS NOT NULL

BEGIN

IF NOT EXISTS (SELECT 'X' FROM [Admin].[FileUploadState] WHERE [FileUploadStateId] = '0')	BEGIN	INSERT [Admin].[FileUploadState] ([FileUploadStateId], [FileUploadState], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (0, N'FINALIZED', N'System', @CurrentTime, NULL, NULL)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[FileUploadState] WHERE [FileUploadStateId] = '0' AND (FileUploadState <> 'FINALIZED'))	BEGIN	UPDATE [Admin].[FileUploadState] SET [FileUploadState] = 'FINALIZED',[LastModifiedBy] = 'System' ,[LastModifiedDate] = @CurrentTime  WHERE [FileUploadStateId] = 0	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[FileUploadState] WHERE [FileUploadStateId] = '1')	BEGIN	INSERT [Admin].[FileUploadState] ([FileUploadStateId], [FileUploadState], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (1, N'PROCESSING', N'System', @CurrentTime, NULL, NULL)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[FileUploadState] WHERE [FileUploadStateId] = '1' AND (FileUploadState <> 'PROCESSING'))	BEGIN	UPDATE [Admin].[FileUploadState] SET [FileUploadState] = 'PROCESSING',[LastModifiedBy] = 'System' ,[LastModifiedDate] = @CurrentTime  WHERE [FileUploadStateId] = 1	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[FileUploadState] WHERE [FileUploadStateId] = '2')	BEGIN	INSERT [Admin].[FileUploadState] ([FileUploadStateId], [FileUploadState], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (2, N'FAILED', N'System', @CurrentTime, NULL, NULL)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[FileUploadState] WHERE [FileUploadStateId] = '2' AND (FileUploadState <> 'FAILED'))	BEGIN	UPDATE [Admin].[FileUploadState] SET [FileUploadState] = 'FAILED',[LastModifiedBy] = 'System' ,[LastModifiedDate] = @CurrentTime  WHERE [FileUploadStateId] = 2	END

END	