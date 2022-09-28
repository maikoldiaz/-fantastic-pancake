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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='2e8230f2-8349-445e-8aae-a2676e41e0eb' AND Status = 1)
	BEGIN
		BEGIN TRY
			DROP VIEW IF EXISTS Admin.view_KPIPreviousDateDataByCategoryElementNode;
			DROP VIEW IF EXISTS Admin.InventoryDetailsBeforeCutoff;
			DROP VIEW IF EXISTS Admin.InventoryQualityDetailsBeforeCutoff;
			DROP VIEW IF EXISTS Admin.view_RelKPI;
			DROP VIEW IF EXISTS Admin.view_MovementDetails;
			DROP VIEW IF EXISTS Admin.view_AttributeDetails;
			DROP VIEW IF EXISTS Admin.view_MovementsByProduct;
			DROP VIEW IF EXISTS Admin.MovementQualityDetailsBeforeCutoff;
			DROP VIEW IF EXISTS Admin.view_KPIDataByCategoryElementNode;
			DROP VIEW IF EXISTS Admin.MovementDetailsBeforeCutoff;
			DROP VIEW IF EXISTS Admin.view_FinalInventory;
			DROP VIEW IF EXISTS Admin.View_RelationShipView;
			DROP VIEW IF EXISTS Admin.MovementsInformationByOwner;
			DROP VIEW IF EXISTS Admin.ContractInformation;
			DROP VIEW IF EXISTS Admin.EventInformation;
			DROP TABLE IF EXISTS Admin.OfficialBalanceExecutionStatus;
			DROP TABLE IF EXISTS Admin.CutoffExecutionStatus;

			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('2e8230f2-8349-445e-8aae-a2676e41e0eb', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('2e8230f2-8349-445e-8aae-a2676e41e0eb', 0, 'POST');
		END CATCH
	END
END