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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='8EB6F446-0471-491D-B126-8F63F456B02B' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Offchain].[Movement] SET [MeasurementUnit] = '31' WHERE [MeasurementUnit] <> '31' AND [MeasurementUnit] IS NOT NULL
			UPDATE [Offchain].[InventoryProduct] SET [MeasurementUnit] = '31' WHERE [MeasurementUnit] <> '31' AND [MeasurementUnit] IS NOT NULL
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('8EB6F446-0471-491D-B126-8F63F456B02B', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('8EB6F446-0471-491D-B126-8F63F456B02B', 0);
		END CATCH
	END
END