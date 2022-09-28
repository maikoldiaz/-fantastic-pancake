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

-- Old GUID - 7a44df0e-3bc4-4dd1-b6a5-20ca89d0e625

IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='88b83c7c-2e0b-432c-99bc-bce96f78503c' AND Status = 1)
	BEGIN
		BEGIN TRY
                TRUNCATE TABLE [Admin].[OfficialMovementInformation];
				TRUNCATE TABLE [Admin].[OfficialNodeTagCalculationDate];
				TRUNCATE TABLE [Admin].[OfficialMonthlyMovementDetails];
				TRUNCATE TABLE [Admin].[OfficialMonthlyMovementQualityDetails];
				TRUNCATE TABLE [Admin].[OfficialMonthlyInventoryDetails];
				TRUNCATE TABLE [Admin].[OfficialMonthlyInventoryQualityDetails];
				TRUNCATE TABLE [Admin].[OfficialMonthlyBalance];

				TRUNCATE TABLE [Admin].[MovementInformationMovforSegmentReport];
				TRUNCATE TABLE [Admin].[MovementInformationMovforSystemReport];
				TRUNCATE TABLE [Admin].[NodeTagCalculationDateForSegmentReport]; 
				TRUNCATE TABLE [Admin].[NodeTagCalculationDateForSystemReport]; 
				TRUNCATE TABLE [Admin].[OperationalInventory];
				TRUNCATE TABLE [Admin].[OperationalInventoryOwner];
				TRUNCATE TABLE [Admin].[OperationalInventoryQuality]; 
				TRUNCATE TABLE [Admin].[OperationalMovement];
				TRUNCATE TABLE [Admin].[OperationalMovementOwner];
				TRUNCATE TABLE [Admin].[OperationalMovementQuality]; 
				TRUNCATE TABLE [Admin].[Operational];

				TRUNCATE TABLE [Admin].[OperationalNonSon];
				TRUNCATE TABLE [Admin].[OperationalInventoryOwnerNonSon];
				TRUNCATE TABLE [Admin].[OperationalMovementQualityNonSon];
				TRUNCATE TABLE [Admin].[OperationalMovementOwnerNonSon];
				TRUNCATE TABLE [Admin].[OperationalInventoryQualityNonSon];

				DELETE FROM [Admin].[ReportExecution];
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('88b83c7c-2e0b-432c-99bc-bce96f78503c', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('88b83c7c-2e0b-432c-99bc-bce96f78503c', 0, 'POST');
		END CATCH
	END
END