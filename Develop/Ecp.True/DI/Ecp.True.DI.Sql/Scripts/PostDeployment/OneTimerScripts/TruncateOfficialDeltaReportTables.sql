/*
 Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


IF EXISTS (Select 'X' from sys.schemas Where name = 'Report')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='a3edd9a4-b0d8-40cc-850f-1f2e7464fd40' AND Status = 1)
	BEGIN
		BEGIN TRY
				TRUNCATE TABLE [Report].[OfficialDeltaBalance];
				TRUNCATE TABLE [Report].[OfficialDeltaInventory];
				TRUNCATE TABLE [Report].[OfficialDeltaMovements];
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('a3edd9a4-b0d8-40cc-850f-1f2e7464fd40', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('a3edd9a4-b0d8-40cc-850f-1f2e7464fd40', 0, 'POST');
		END CATCH
	END
END