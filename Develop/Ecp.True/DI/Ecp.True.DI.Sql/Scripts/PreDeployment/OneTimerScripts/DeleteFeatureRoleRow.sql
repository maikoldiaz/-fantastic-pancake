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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='34337e6d-e3d0-47d9-9572-3caabf66132a' AND Status = 1)
	BEGIN
		BEGIN TRY
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureRoleId] = 23;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('34337e6d-e3d0-47d9-9572-3caabf66132a', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('34337e6d-e3d0-47d9-9572-3caabf66132a', 0);
		END CATCH
	END
END

IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='11e7ff4a-2e54-471f-addd-a797529a0d6e' AND Status = 1)
	BEGIN
		BEGIN TRY
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureRoleId] = 23;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('11e7ff4a-2e54-471f-addd-a797529a0d6e', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('11e7ff4a-2e54-471f-addd-a797529a0d6e', 0);
		END CATCH
	END
END

-- Deleting Ownership and Ownershipnodes for Query User
IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='c68522b7-09b8-4f2c-942a-2a0a00d6df57' AND Status = 1)
	BEGIN
		BEGIN TRY
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureRoleId] = 29;
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureRoleId] = 30;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('c68522b7-09b8-4f2c-942a-2a0a00d6df57', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('c68522b7-09b8-4f2c-942a-2a0a00d6df57', 0);
		END CATCH
	END
END

-- Deleting NodeApproval and FlowSettings
IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='cd7b4acc-5dae-4761-a599-0d4ed8f829d9' AND Status = 1)
	BEGIN
		BEGIN TRY
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureId] = 19;
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureId] = 20;
			DELETE FROM [Admin].[Feature] WHERE [FeatureId] = 19;
			DELETE FROM [Admin].[Feature] WHERE [FeatureId] = 20;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('cd7b4acc-5dae-4761-a599-0d4ed8f829d9', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('cd7b4acc-5dae-4761-a599-0d4ed8f829d9', 0);
		END CATCH
	END
END