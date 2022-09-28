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


IF EXISTS (Select 'X' from sys.schemas Where name = 'Offchain')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='794b4fe8-d7e3-4b6e-8098-b84a0e70c485' AND Status = 1)
	BEGIN
		BEGIN TRY

			UPDATE Offchain.Movement SET RetryCount=0 WHERE BlockchainStatus <> 0 AND RetryCount > 0;
			UPDATE Offchain.InventoryProduct SET RetryCount=0 WHERE BlockchainStatus <> 0 AND RetryCount > 0;

			UPDATE Offchain.Ownership SET BlockNumber = '0x'+FORMAT(CAST(BlockNumber AS INT),'x2') WHERE BlockchainStatus=0 AND BlockNumber NOT LIKE '0x%';
			UPDATE Offchain.Ownership SET RetryCount=0 WHERE BlockchainStatus <> 0 AND RetryCount > 0;

			UPDATE Offchain.Owner SET RetryCount=0 WHERE BlockchainStatus <> 0 AND RetryCount > 0;

			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('794b4fe8-d7e3-4b6e-8098-b84a0e70c485', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('794b4fe8-d7e3-4b6e-8098-b84a0e70c485', 0, 'POST');
		END CATCH
	END
END