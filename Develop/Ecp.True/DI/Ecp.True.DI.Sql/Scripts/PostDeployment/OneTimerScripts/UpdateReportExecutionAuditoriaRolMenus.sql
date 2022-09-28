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
	IF (EXISTS (select * from admin.ReportExecution where ReportTypeId = 6  and StatusTypeId = 1))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='22052C58-221D-425F-AE63-A96AB628C81E' AND Status = 1)
		BEGIN
			BEGIN TRY
				  update admin.ReportExecution 
				  set StatusTypeId = 2
				  where ReportTypeId = 6  and StatusTypeId = 1 and cast(CreatedDate as date) <= cast(GETDATE()-1 as date)
				  INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('22052C58-221D-425F-AE63-A96AB628C81E', 1, 'POST');
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('22052C58-221D-425F-AE63-A96AB628C81E', 0, 'POST');
			END CATCH
		END
	END
END