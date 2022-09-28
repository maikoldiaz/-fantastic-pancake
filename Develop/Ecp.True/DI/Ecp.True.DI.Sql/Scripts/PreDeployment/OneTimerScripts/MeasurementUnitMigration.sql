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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='D1C166CE-48B3-4AA4-A28C-4DFB6A44516E' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE Admin.PendingTransaction SET Units = 31, LastModifiedBy = 'System', LastModifiedDate = Admin.udf_GetTrueDate() Where Units IS NOT NULL
			UPDATE Admin.UnbalanceComment   SET Units = 31 Where Units IS NOT NULL
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('D1C166CE-48B3-4AA4-A28C-4DFB6A44516E', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('D1C166CE-48B3-4AA4-A28C-4DFB6A44516E', 0);
		END CATCH
	END
END