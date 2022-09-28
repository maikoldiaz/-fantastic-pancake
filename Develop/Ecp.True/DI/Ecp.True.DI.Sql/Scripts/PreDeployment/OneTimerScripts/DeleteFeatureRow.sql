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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='5de2470c-05a2-4580-8482-53f8149bc614' AND Status = 1)
	BEGIN
		BEGIN TRY
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureId] = 15;
			DELETE FROM [Admin].[Feature] WHERE [FeatureId] = 15;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('5de2470c-05a2-4580-8482-53f8149bc614', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('5de2470c-05a2-4580-8482-53f8149bc614', 0);
		END CATCH
	END
END

-- Deleting NodeApproval and FlowSettings
IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='b5e66162-9340-4762-b758-0701d2994f0b' AND Status = 1)
	BEGIN
		BEGIN TRY
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureId] = 19;
			DELETE FROM [Admin].[FeatureRole] WHERE [FeatureId] = 20;
			DELETE FROM [Admin].[Feature] WHERE [FeatureId] = 19;
			DELETE FROM [Admin].[Feature] WHERE [FeatureId] = 20;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('b5e66162-9340-4762-b758-0701d2994f0b', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('b5e66162-9340-4762-b758-0701d2994f0b', 0);
		END CATCH
	END
END