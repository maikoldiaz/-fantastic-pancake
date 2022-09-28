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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='56f35a26-6753-4e4e-a62d-b9490a1c8e09' AND Status = 1)
	BEGIN
		BEGIN TRY
			DROP TABLE IF EXISTS [Admin].[MonthlyOfficialIntermittent];
			DROP TABLE IF EXISTS [Admin].[OperationalIntermittentNonSon];
			INSERT [Admin].[ControlScript] ([Id], [DeploymentType], [Status]) VALUES ('56f35a26-6753-4e4e-a62d-b9490a1c8e09', 'POST', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [DeploymentType], [Status]) VALUES ('56f35a26-6753-4e4e-a62d-b9490a1c8e09', 'POST', 0);
		END CATCH
	END
END