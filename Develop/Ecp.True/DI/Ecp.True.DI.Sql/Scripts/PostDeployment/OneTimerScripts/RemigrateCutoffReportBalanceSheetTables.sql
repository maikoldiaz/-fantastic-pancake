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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='a39b2a77-b20c-471c-90d1-64105e42098b' AND Status = 1)
	BEGIN
		BEGIN TRY
			TRUNCATE TABLE Admin.[KPIDataByCategoryElementNode]
			TRUNCATE TABLE Admin.[KPIPreviousDateDataByCategoryElementNode]
			TRUNCATE TABLE Admin.[MovementsByProductWithoutOwner]

			EXEC [Admin].[usp_SaveKPIDataByCategoryElementNode] -1
			EXEC [Admin].[usp_SaveMovementsByProduct] -1
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('a39b2a77-b20c-471c-90d1-64105e42098b', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('a39b2a77-b20c-471c-90d1-64105e42098b', 0, 'POST');
		END CATCH
	END
END