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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='7c967ca5-0f2b-4773-b082-eed2dfa418a3' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Offchain].[Movement] SET [Classification]='Movimiento' WHERE [Classification] = '' OR [Classification] = 'cls'
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('7c967ca5-0f2b-4773-b082-eed2dfa418a3', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('7c967ca5-0f2b-4773-b082-eed2dfa418a3', 0, 'POST');
		END CATCH
	END
END