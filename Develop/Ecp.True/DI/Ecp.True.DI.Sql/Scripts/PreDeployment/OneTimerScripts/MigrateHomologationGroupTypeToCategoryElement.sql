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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='27f2b272-578f-4bdf-ac97-002a3ca7579b' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Admin].[HomologationGroup] SET [GroupTypeId]=13 WHERE [GroupTypeId]=1
			UPDATE [Admin].[HomologationGroup] SET [GroupTypeId]=14 WHERE [GroupTypeId]=2
			UPDATE [Admin].[HomologationGroup] SET [GroupTypeId]=15 WHERE [GroupTypeId]=3
			UPDATE [Admin].[HomologationGroup] SET [GroupTypeId]=6 WHERE [GroupTypeId]=4
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('27f2b272-578f-4bdf-ac97-002a3ca7579b', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('27f2b272-578f-4bdf-ac97-002a3ca7579b', 0);
		END CATCH
	END
END