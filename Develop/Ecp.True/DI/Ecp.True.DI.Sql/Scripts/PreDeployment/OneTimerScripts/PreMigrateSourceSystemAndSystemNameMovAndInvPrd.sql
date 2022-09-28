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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='2AF3F3D8-E7EC-4235-85A5-EF3541A18BF6' AND Status = 1)
	BEGIN
		BEGIN TRY
			
			--read data and save in temp table
			SELECT * INTO #TempMovementForMigration
			FROM Offchain.Movement

			SELECT * INTO #TempInventoryProductForMigration
			FROM Offchain.InventoryProduct
			--use same temp table in update and delete columns from Movement and InventoryProduct tables
	          

			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('2AF3F3D8-E7EC-4235-85A5-EF3541A18BF6', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('2AF3F3D8-E7EC-4235-85A5-EF3541A18BF6', 0);
		END CATCH
	END
END