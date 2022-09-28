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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='b48218c6-c358-44cd-8dd8-e4db531c8925' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Admin].[FileRegistration] SET [SystemTypeId] = 2 WHERE [SystemTypeId]=1;
			UPDATE [Admin].[FileRegistration] SET [SegmentId] = 10;
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('b48218c6-c358-44cd-8dd8-e4db531c8925', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('b48218c6-c358-44cd-8dd8-e4db531c8925', 0, 'POST');
		END CATCH
	END
END