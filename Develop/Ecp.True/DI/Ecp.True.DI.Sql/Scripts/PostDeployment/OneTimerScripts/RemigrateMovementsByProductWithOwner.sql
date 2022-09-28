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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='2ed913f1-b37f-47d5-91da-105ba527e988' AND Status = 1)
	BEGIN
		BEGIN TRY
				TRUNCATE TABLE [Admin].[MovementsByProductWithOwner];
				EXEC [Admin].[usp_SaveMovementsByProductWithOwner] @OwnershipTicketId=-1;
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('2ed913f1-b37f-47d5-91da-105ba527e988', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('2ed913f1-b37f-47d5-91da-105ba527e988', 0, 'POST');
		END CATCH
	END
END