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


IF EXISTS (Select 'X' from sys.schemas Where name = 'Offchain')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='dfdb2e0f-fc73-46de-9d99-8e1390413ee1' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Offchain].[Movement] SET [ScenarioId] = 1;
			UPDATE [Offchain].[InventoryProduct] SET [ScenarioId] = 1;
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('dfdb2e0f-fc73-46de-9d99-8e1390413ee1', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('dfdb2e0f-fc73-46de-9d99-8e1390413ee1', 0, 'POST');
		END CATCH
	END
END