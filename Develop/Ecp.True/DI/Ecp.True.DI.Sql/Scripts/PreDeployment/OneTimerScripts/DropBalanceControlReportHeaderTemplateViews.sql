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
	
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='EF5B04C7-832F-468D-A84B-D4777DA9EB90' AND Status = 1)
		BEGIN
			BEGIN TRY

				IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Admin' AND TABLE_NAME = 'BalanceControl'))
				BEGIN
					DROP VIEW [Admin].[BalanceControl];
				END

				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('EF5B04C7-832F-468D-A84B-D4777DA9EB90', 1);
			END TRY
			

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('EF5B04C7-832F-468D-A84B-D4777DA9EB90', 0);
			END CATCH
		END
END