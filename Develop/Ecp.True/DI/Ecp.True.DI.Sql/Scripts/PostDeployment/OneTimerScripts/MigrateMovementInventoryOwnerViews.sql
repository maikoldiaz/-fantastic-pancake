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


IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='90074c26-35a3-4e34-99d7-f8cb9c88fefc' AND Status = 1)
	BEGIN
		BEGIN TRY
			EXEC [Admin].[usp_SaveMovementDetailsWithOwner] @OwnershipTicketId=-1
			EXEC [Admin].[usp_SaveInventoryDetailsWithOwner] @OwnershipTicketId=-1
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('90074c26-35a3-4e34-99d7-f8cb9c88fefc', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('90074c26-35a3-4e34-99d7-f8cb9c88fefc', 0, 'POST');
		END CATCH
	END
END