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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='bf48a1e2-6c9e-45cf-bc9b-104fbfcb08c6' AND Status = 1)
	BEGIN
		BEGIN TRY
			 UPDATE Own
             SET [Own].[OwnerId] = [ElementOwner].[ElementId]
             FROM [offchain].[Owner] Own
             LEFT JOIN [Admin].CategoryElement ElementOwner
             ON CASE WHEN Own.OwnerId = 'ECOPETROL S.A.'
              THEN 'ECOPETROL'
              ELSE Own.OwnerId END = ElementOwner.Name
      AND ElementOwner.CategoryId = 7
      WHERE ISNUMERIC(Own.OwnerId) = 0
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('bf48a1e2-6c9e-45cf-bc9b-104fbfcb08c6', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('bf48a1e2-6c9e-45cf-bc9b-104fbfcb08c6', 0, 'POST');
		END CATCH
	END
END