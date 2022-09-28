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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='db316226-a708-46f8-8101-fbe117fd75cb' AND Status = 1)
	BEGIN
		BEGIN TRY
		
			UPDATE Offchain.Movement SET IsDeleted=1 WHERE
			(EventType = 'Delete')
			OR
			(EventType = 'Update' AND PreviousBlockchainMovementTransactionId IS NULL)

			UPDATE Offchain.InventoryProduct SET IsDeleted=1 WHERE
			(EventType = 'Delete')
			OR
			(EventType = 'Update' AND PreviousBlockchainInventoryProductTransactionId IS NULL)
			
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('db316226-a708-46f8-8101-fbe117fd75cb', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('db316226-a708-46f8-8101-fbe117fd75cb', 0, 'POST');
		END CATCH
	END
END