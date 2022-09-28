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

IF EXISTS (Select 'X' from sys.schemas Where name = 'Offchain')
BEGIN
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Offchain' AND  TABLE_NAME = 'Inventory'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='744d86e8-476a-4756-b979-6ab2b59cd12e' AND Status = 1)
		BEGIN
			BEGIN TRY
				EXEC SP_RENAME '[Offchain].[Inventory]', 'Obsolete_Inventory'
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('744d86e8-476a-4756-b979-6ab2b59cd12e', 1, 'POST');
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('744d86e8-476a-4756-b979-6ab2b59cd12e', 0, 'POST');
			END CATCH
		END
	END
END