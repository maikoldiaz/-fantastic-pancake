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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='28BF5994-13D0-40BE-8597-270D94D8D3B0' AND Status = 1)
	BEGIN
		BEGIN TRY
			EXEC [Admin].[usp_SaveMovementsByProduct] @TicketId=-1
			EXEC [Admin].[usp_SaveMovementsByProductWithOwner] @OwnershipTicketId=-1
			EXEC [Admin].[usp_SaveAttributeDetailsWithOwner] @OwnershipTicketId = -1
			EXEC [Admin].[usp_SaveQualityDetailsWithOwner] @OwnershipTicketId = -1
			EXEC [Admin].[usp_SaveKPIDataByCategoryElementNode] @TicketId= -1
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('28BF5994-13D0-40BE-8597-270D94D8D3B0', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('28BF5994-13D0-40BE-8597-270D94D8D3B0', 0, 'POST');
		END CATCH
	END
END