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

IF EXISTS (Select 'X' from sys.schemas Where name = 'Offchain')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='107ef824-7049-4a57-a9bc-9f18a5236836' AND Status = 1)
	BEGIN
		BEGIN TRY
			
          -- Backup Operator values into temp table
			IF (OBJECT_ID('TempDB..#Offchain_InventoryOperator') IS NULL) AND (COL_LENGTH('Offchain.InventoryProduct','Operator') IS NOT NULL)
              BEGIN
                  -- BACKUP DATA TO A TEMP TABLE
                   SELECT * INTO #Offchain_InventoryOperator 
				            FROM Offchain.InventoryProduct
				   DECLARE @UpdateOperatorInInventory NVARCHAR (4000)
				   SET @UpdateOperatorInInventory = N'UPDATE Offchain.InventoryProduct SET Operator = NULL'
				   EXEC sp_executesql @UpdateOperatorInInventory	
              END

			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('107ef824-7049-4a57-a9bc-9f18a5236836', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('107ef824-7049-4a57-a9bc-9f18a5236836', 0);
		END CATCH
	END
END