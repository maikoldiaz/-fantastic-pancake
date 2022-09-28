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
	IF (EXISTS (SELECT * FROM Admin.Ticket WHERE Status= 2 AND  TicketTypeId IN(3,6)) )
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='7C1DC10D-B2EA-48F8-9141-B66F735C540D' AND Status = 1)
		BEGIN
			BEGIN TRY
				  UPDATE Admin.Ticket
				  SET Status=6
				  WHERE Status=2 AND TicketTypeId IN(3,6) 	
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('7C1DC10D-B2EA-48F8-9141-B66F735C540D', 1, 'POST');
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('7C1DC10D-B2EA-48F8-9141-B66F735C540D', 0, 'POST');
			END CATCH
		END
	END
END