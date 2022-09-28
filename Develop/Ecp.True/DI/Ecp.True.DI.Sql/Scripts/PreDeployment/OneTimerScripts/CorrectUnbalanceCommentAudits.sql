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

IF EXISTS (Select 'X' from sys.schemas Where name = 'Audit')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='9c912300-5cef-489e-96e5-dd167e6dbabb' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Audit].AuditLog SET Entity='Admin.UnbalanceComment' WHERE Entity='Admin.Unbalance';
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('9c912300-5cef-489e-96e5-dd167e6dbabb', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('9c912300-5cef-489e-96e5-dd167e6dbabb', 0);
		END CATCH
	END
END