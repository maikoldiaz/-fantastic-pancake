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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='7afd5767-87c4-43aa-94a3-67dce86eeaaa' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Admin].[CategoryElement] SET [Name]='OTROS_OLD' WHERE [Name]='OTROS' AND [CategoryId]=7;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('7afd5767-87c4-43aa-94a3-67dce86eeaaa', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('7afd5767-87c4-43aa-94a3-67dce86eeaaa', 0);
		END CATCH
	END
END