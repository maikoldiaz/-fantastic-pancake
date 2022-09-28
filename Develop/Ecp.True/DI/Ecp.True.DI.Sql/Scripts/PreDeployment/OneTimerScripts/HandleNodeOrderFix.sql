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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='9495b516-59cb-4974-bfa9-2f8aef60e26c' AND Status = 1)
	BEGIN
		BEGIN TRY
			IF EXISTS (SELECT * FROM sys.objects WHERE [name] = N'tr_Node_Changes' AND [type] = 'TR')
			BEGIN
				DROP TRIGGER [Admin].[tr_Node_Changes];
			END
			UPDATE Admin.Node SET [Order]=0 WHERE [Order] IS NULL;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('9495b516-59cb-4974-bfa9-2f8aef60e26c', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('9495b516-59cb-4974-bfa9-2f8aef60e26c', 0);
		END CATCH
	END
END