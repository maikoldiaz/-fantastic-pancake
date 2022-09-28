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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='4fc72578-e892-459b-ac45-66fc6b354df5' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Offchain].[InventoryProduct] SET OperatorId = 14 where isnull(OperatorId, 0) = 0;
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('4fc72578-e892-459b-ac45-66fc6b354df5', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('4fc72578-e892-459b-ac45-66fc6b354df5', 0, 'POST');
		END CATCH
	END
END