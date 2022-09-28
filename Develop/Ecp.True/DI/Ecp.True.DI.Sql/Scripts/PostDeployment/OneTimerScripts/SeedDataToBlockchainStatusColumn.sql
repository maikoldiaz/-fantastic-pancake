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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='A361BF4D-2635-4B40-BB77-C150B6EC1469' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE Offchain.Unbalance			SET BlockChainStatus = 1 WHERE BlockChainStatus IS NULL;
			UPDATE Offchain.InventoryProduct	SET BlockChainStatus = 1 WHERE BlockChainStatus IS NULL;
			UPDATE Offchain.Movement			SET BlockChainStatus = 1 WHERE BlockChainStatus IS NULL;
			UPDATE Offchain.Owner				SET BlockChainStatus = 1 WHERE BlockChainStatus IS NULL;
			UPDATE Offchain.Ownership			SET BlockChainStatus = 1 WHERE BlockChainStatus IS NULL;
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('A361BF4D-2635-4B40-BB77-C150B6EC1469', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('A361BF4D-2635-4B40-BB77-C150B6EC1469', 0, 'POST');
		END CATCH
	END
END