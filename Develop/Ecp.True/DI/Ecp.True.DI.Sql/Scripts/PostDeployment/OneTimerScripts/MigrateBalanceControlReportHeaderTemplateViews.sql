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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='DC08E1F3-3DC0-488A-8BAC-E225E99B120F' AND Status = 1)
	BEGIN
		BEGIN TRY
			EXEC [Admin].[usp_SaveBalanceControl] @TicketId = -1

			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('DC08E1F3-3DC0-488A-8BAC-E225E99B120F', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('DC08E1F3-3DC0-488A-8BAC-E225E99B120F', 0, 'POST');
		END CATCH
	END
END