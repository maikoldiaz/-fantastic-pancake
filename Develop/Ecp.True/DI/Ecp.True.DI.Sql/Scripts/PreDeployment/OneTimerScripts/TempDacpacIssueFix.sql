/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/



IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN	
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='2ec0f32f-3d51-4251-ae99-d10d5836eb34' AND Status = 1)
	BEGIN
		BEGIN TRY
			IF EXISTS (SELECT 'X' FROM  sys.foreign_keys AS f WHERE Name = 'FK_Ownership_CategoryElement')
			BEGIN
				DELETE FROM [Offchain].[Ownership];
				ALTER TABLE [Offchain].[Ownership] DROP CONSTRAINT [FK_Ownership_CategoryElement];
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('2ec0f32f-3d51-4251-ae99-d10d5836eb34', 1);
			END
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('2ec0f32f-3d51-4251-ae99-d10d5836eb34', 0);
		END CATCH
	END
END