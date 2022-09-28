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
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND  TABLE_NAME = 'RegistrationProgress'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='A5F8F94E-E6BA-48CE-A5DF-7A06067B3B27' AND Status = 1)
		BEGIN
			BEGIN TRY
				DROP TABLE [Admin].[RegistrationProgress];
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('A5F8F94E-E6BA-48CE-A5DF-7A06067B3B27', 1, 'POST');
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('A5F8F94E-E6BA-48CE-A5DF-7A06067B3B27', 0, 'POST');
			END CATCH
		END
	END
END