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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='50C7FD6C-A0D1-48B6-BA00-5D9465F5B6A1' AND Status = 1)
	BEGIN
		BEGIN TRY
		--1.Node
		IF EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Node' AND COLUMN_NAME = 'blockchainstatus' and Data_Type = 'bit')
		BEGIN
			Update [Offchain].[Node]
			set BlockchainStatus =
			case when BlockchainStatus = 0
				Then 1
				ELSE 0
				END
		END

		--2.NodeConnection
		IF EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'NodeConnection' AND COLUMN_NAME = 'blockchainstatus' and Data_Type = 'bit')
		BEGIN
			Update [Offchain].[NodeConnection]
			set BlockchainStatus =
			case when BlockchainStatus = 0
				Then 1
				ELSE 0
				END
		END

		--3.Movement
		IF EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Movement' AND COLUMN_NAME = 'blockchainstatus' and Data_Type = 'bit')
		BEGIN
			Update [Offchain].[Movement]
			set BlockchainStatus =
			case when BlockchainStatus = 0
				Then 1
				ELSE 0
				END
		END

		--4.InventoryProduct
		IF EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'InventoryProduct' AND COLUMN_NAME = 'blockchainstatus' and Data_Type = 'bit')
		BEGIN
			Update [Offchain].[InventoryProduct]
			set BlockchainStatus =
			case when BlockchainStatus = 0
				Then 1
				ELSE 0
				END
		END

		--5.Owner
		IF EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Owner' AND COLUMN_NAME = 'blockchainstatus' and Data_Type = 'bit')
		BEGIN
			Update [Offchain].[Owner]
			set BlockchainStatus =
			case when BlockchainStatus = 0
				Then 1
				ELSE 0
				END
		END

		--6.Ownership
		IF EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Ownership' AND COLUMN_NAME = 'blockchainstatus' and Data_Type = 'bit')
		BEGIN
			Update [Offchain].[Ownership]
			set BlockchainStatus =
			case when BlockchainStatus = 0
				Then 1
				ELSE 0
				END
		END

		DECLARE @UnbalanceBlockshainStatusUpdateQuery NVARCHAR(4000);
		--7.Unbalance
		IF EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Unbalance' AND COLUMN_NAME = 'blockchainstatus' and Data_Type = 'bit' and TABLE_SCHEMA = 'Offchain')
		BEGIN
			SET @UnbalanceBlockshainStatusUpdateQuery = N'Update [Offchain].[Unbalance]
			set BlockchainStatus =
			case when BlockchainStatus = 0
				Then 1
				ELSE 0
				END';
			EXEC (@UnbalanceBlockshainStatusUpdateQuery)
		END
		ELSE IF EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Unbalance' AND COLUMN_NAME = 'blockchainstatus' and Data_Type = 'bit' and TABLE_SCHEMA = 'Admin')
		BEGIN
			SET @UnbalanceBlockshainStatusUpdateQuery = N'Update [Admin].[Unbalance]
			set BlockchainStatus =
			case when BlockchainStatus = 0
				Then 1
				ELSE 0
				END';
			EXEC (@UnbalanceBlockshainStatusUpdateQuery)
		END

		INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('50C7FD6C-A0D1-48B6-BA00-5D9465F5B6A1', 1);
			
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('50C7FD6C-A0D1-48B6-BA00-5D9465F5B6A1', 0);
		END CATCH
	END
END