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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='D3FFCA0C-801F-4A94-8AB2-6DDB68A8013A' AND Status = 1)
    AND (OBJECT_ID('Offchain.Inventory') IS NOT NULL)
	BEGIN
		BEGIN TRY
			-- Backup Inventory and InventoryProduct tables into temp table.
			IF (OBJECT_ID('TempDB..#Offchain_Inventory') IS NULL)
              BEGIN
                  -- BACKUP DATA TO A TEMP TABLE
                   SELECT * INTO #Offchain_Inventory FROM [Offchain].[Inventory]
              END

			IF (OBJECT_ID('TempDB..#Offchain_InventoryProduct') IS NULL)
              BEGIN
                  -- BACKUP DATA TO A TEMP TABLE
                   SELECT * INTO #Offchain_InventoryProduct FROM [Offchain].[InventoryProduct]
              END

			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('D3FFCA0C-801F-4A94-8AB2-6DDB68A8013A', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('D3FFCA0C-801F-4A94-8AB2-6DDB68A8013A', 0);
		END CATCH
	END
END