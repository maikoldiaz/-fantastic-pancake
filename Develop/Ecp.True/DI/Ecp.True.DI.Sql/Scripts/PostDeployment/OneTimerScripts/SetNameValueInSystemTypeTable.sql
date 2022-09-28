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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='7B79E7D7-5F7E-4CBD-9AE3-D0B4B9319A49' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Admin].[SystemType] SET [Name]='Pedidos' WHERE [SystemTypeId]=4;
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('7B79E7D7-5F7E-4CBD-9AE3-D0B4B9319A49', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('7B79E7D7-5F7E-4CBD-9AE3-D0B4B9319A49', 0, 'POST');
		END CATCH
	END
END