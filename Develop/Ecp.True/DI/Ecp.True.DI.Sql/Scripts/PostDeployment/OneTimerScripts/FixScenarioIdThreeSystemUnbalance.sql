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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='8dbd968d-b025-4f9e-b825-1d88d154d7db' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Offchain].[Movement] SET [TicketId]=NULL, [OwnershipTicketId]=NULL WHERE [MovementTransactionId] = 53441;
			UPDATE [Admin].[SystemUnbalance] SET [InputVolume] = 377608.46 WHERE [SystemUnbalanceId]=2478;
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('8dbd968d-b025-4f9e-b825-1d88d154d7db', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('8dbd968d-b025-4f9e-b825-1d88d154d7db', 0, 'POST');
		END CATCH
	END
END