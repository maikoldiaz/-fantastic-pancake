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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='8f2ef610-7549-4be1-84f7-1f171dcb8b11' AND Status = 1)
	BEGIN
		BEGIN TRY
				TRUNCATE TABLE Admin.[InventoryDetailsWithOwner];
				TRUNCATE TABLE Admin.[QualityDetailsWithOwner];
				TRUNCATE TABLE Admin.[InventoryDetailsWithoutOwner];
				TRUNCATE TABLE Admin.[QualityDetailsWithoutOwner];

				EXEC [Admin].[usp_SaveInventoryDetails] @TicketId=-1;
				EXEC [Admin].[usp_SaveQualityDetails] @TicketId=-1;
				EXEC [Admin].[usp_SaveInventoryDetailsWithOwner] @OwnershipTicketId=-1;
				EXEC [Admin].[usp_SaveQualityDetailsWithOwner] @OwnershipTicketId=-1;

				EXEC [Admin].[usp_Cleanup_OperationalDataWithoutCutOff] @Hour=0;
				EXEC [Admin].[usp_Cleanup_NonSonSegmentDataWithoutCutOff] @Hour=0;
				EXEC [Admin].[usp_Cleanup_MonthlyOfficialDataWithoutCutOff]  @Hour=0;

			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('8f2ef610-7549-4be1-84f7-1f171dcb8b11', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('8f2ef610-7549-4be1-84f7-1f171dcb8b11', 0, 'POST');
		END CATCH
	END
END