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



IF	OBJECT_ID('Admin.Version') IS NOT NULL
BEGIN

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[Version] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Version] WHERE [VersionId] = 1)	BEGIN	INSERT [Admin].[Version] ([VersionId], [Number], [Type]) VALUES (1, 0, 'Homologation')		END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Version] WHERE [VersionId] = '1' AND ([Type] <> 'Homologation'))		BEGIN	UPDATE [Admin].[Version] SET [Type] = 'Homologation' WHERE [VersionId] = '1'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Version] WHERE [VersionId] = 2)	BEGIN	INSERT [Admin].[Version] ([VersionId], [Number], [Type]) VALUES (2, 0, 'Transformation')	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Version] WHERE [VersionId] = '2' AND ([Type] <> 'Transformation'))		BEGIN	UPDATE [Admin].[Version] SET [Type] = 'Transformation' WHERE [VersionId] = '2'	END

SET IDENTITY_INSERT [Admin].Version OFF
END	
