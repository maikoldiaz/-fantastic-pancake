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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='2a4aafd8-b2ba-4b87-a6fa-21a94de8ab5b' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Admin].[HomologationObjectType] SET [Name] = [Name] + '_1' WHERE HomologationObjectTypeId > 32;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('2a4aafd8-b2ba-4b87-a6fa-21a94de8ab5b', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('2a4aafd8-b2ba-4b87-a6fa-21a94de8ab5b', 0);
		END CATCH
	END
END



IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='E636DCB6-E391-4D09-B0D7-01DBEB23DD79' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 1 WHERE HomologationObjectTypeId = 3;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 2 WHERE HomologationObjectTypeId = 4;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 3 WHERE HomologationObjectTypeId = 7;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 4 WHERE HomologationObjectTypeId = 8;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 5 WHERE HomologationObjectTypeId = 9;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 6 WHERE HomologationObjectTypeId = 12;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 7 WHERE HomologationObjectTypeId = 14;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 8 WHERE HomologationObjectTypeId = 16;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 9 WHERE HomologationObjectTypeId = 18;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 10 WHERE HomologationObjectTypeId = 24;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 11 WHERE HomologationObjectTypeId = 25;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 12 WHERE HomologationObjectTypeId = 26;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 13 WHERE HomologationObjectTypeId = 27;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 14 WHERE HomologationObjectTypeId = 28;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 15 WHERE HomologationObjectTypeId = 29;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 16 WHERE HomologationObjectTypeId = 30;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 17 WHERE HomologationObjectTypeId = 31;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 18 WHERE HomologationObjectTypeId = 33;
			UPDATE [Admin].[HomologationObject] SET HomologationObjectTypeId = 18 WHERE HomologationObjectTypeId = 1001;

			DELETE FROM [Admin].[HomologationObjectType] where HomologationObjectTypeId > 18

			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('E636DCB6-E391-4D09-B0D7-01DBEB23DD79', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('E636DCB6-E391-4D09-B0D7-01DBEB23DD79', 0);
		END CATCH
	END
END

