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




/*
PR(s):		this
PBI(s):		NULL
Task(s):	8407, 8404
Description:
	Updating the existing value of master table [SystemType] so that the updated data of 
	[SystemType] in PostDeployment scripts remains in sync.
*/
IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='F4370CF7-C78C-4139-9C96-9C4412424378' AND Status = 1)
	BEGIN
		BEGIN TRY
		IF EXISTS (Select [Name] FROM [Admin].[SystemType] WHERE [Name] = 'CENIT')
			BEGIN
				Update [Admin].[SystemType] SET [Name] = 'EXCEL' WHERE [Name] = 'CENIT'
			END
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('F4370CF7-C78C-4139-9C96-9C4412424378', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('F4370CF7-C78C-4139-9C96-9C4412424378', 0);
		END CATCH
	END
END




/*
PR(s):		this
PBI(s):		NULL
Task(s):	NULL
Description:
	Update Record of  [Admin].[Scenario] table. transportBalance to balanceTransporters
*/
IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='DCA74346-941F-44E0-8917-F91249F411B9' AND Status = 1)
	BEGIN
		BEGIN TRY
		IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Scenario' AND TABLE_SCHEMA = 'ADMIN')
			BEGIN
				IF EXISTS(SELECT 1 FROM [Admin].[Scenario] WHERE Name = 'transportBalance')
				BEGIN
					UPDATE [Admin].[Scenario] 
					SET Name = 'balanceTransporters'
					WHERE Name = 'transportBalance'
				END 
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('DCA74346-941F-44E0-8917-F91249F411B9', 1);
			END
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('DCA74346-941F-44E0-8917-F91249F411B9', 0);
		END CATCH
	END
END



--------------------- Removal of UnUsed/NoMore Required Rows from Master Tables  - Start--------------------------------
IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='A329CA34-132B-4AD6-B59E-278E2B77F679' AND Status = 1)
	BEGIN
		BEGIN TRY
			IF EXISTS (SELECT FeatureId FROM [Admin].[Feature] WHERE Name = 'ticket')
			BEGIN
				Delete FROM [Admin].[Feature] WHERE Name = 'ticket';
			END
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('A329CA34-132B-4AD6-B59E-278E2B77F679', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('A329CA34-132B-4AD6-B59E-278E2B77F679', 0);
		END CATCH
	END
END

--------------------- Removal of UnUsed/NoMore Required Rows from Master Tables  - END--------------------------------



--------------------- Clearing Feature table to correct its Identity Column values  - Start--------------------------------
IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='BFEC9752-3E0E-441F-B0F9-CC85B69330BE' AND Status = 1)
	BEGIN
		BEGIN TRY
			IF EXISTS (SELECT FeatureId from Admin.Feature where FeatureId > 40 AND FeatureId<100)
			BEGIN
				Delete from Admin.Feature
				DBCC CHECKIDENT ('[Admin].[Feature]', RESEED, 0)
			END
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('BFEC9752-3E0E-441F-B0F9-CC85B69330BE', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('BFEC9752-3E0E-441F-B0F9-CC85B69330BE', 0);
		END CATCH
	END
END
--------------------- Clearing Feature table to correct its Identity Column values  - END--------------------------------
