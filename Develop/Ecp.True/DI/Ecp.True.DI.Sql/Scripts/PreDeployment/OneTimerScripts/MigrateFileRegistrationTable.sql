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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='F8619E11-7B61-453A-86B1-2982B8A10C07' AND Status = 1)
	BEGIN
		BEGIN TRY
			UPDATE [Admin].[FileRegistration] SET IsParsed = 1 WHERE IsParsed IS NULL;
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('F8619E11-7B61-453A-86B1-2982B8A10C07', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('F8619E11-7B61-453A-86B1-2982B8A10C07', 0);
		END CATCH
	END
END