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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='f0092ed5-73a4-4629-99db-e9546c2143a1' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Admin].[OwnershipNode] SET OwnershipStatusId=2 WHERE STATUS=0
			UPDATE [Admin].[OwnershipNode] SET OwnershipStatusId=1 WHERE STATUS=1
			UPDATE [Admin].[OwnershipNode] SET OwnershipStatusId=1 WHERE STATUS=3
			UPDATE [Admin].[OwnershipNode] SET OwnershipStatusId=3 WHERE STATUS=2
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('f0092ed5-73a4-4629-99db-e9546c2143a1', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('f0092ed5-73a4-4629-99db-e9546c2143a1', 0, 'POST');
		END CATCH
	END
END