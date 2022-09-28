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
	IF COL_LENGTH('TempDB..#Admin_Event','OwnerId') IS NOT NULL
	BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='C926D4B7-A023-4FCE-92B9-49009E5E823F' AND Status = 1)
	BEGIN
		BEGIN TRY
                  DECLARE @EventUpdateQuery NVARCHAR (4000)
	                  SET @EventUpdateQuery = N'UPDATE Actual
		                                           SET [Owner1Id] = OwnerId
		                                          FROM [Admin].[Event] Actual
		                                          JOIN #Admin_Event Temp
		                                            ON Actual.EventId = Temp.EventId'
	               EXEC sp_executesql @EventUpdateQuery
        
			IF OBJECT_ID('tempdb..#Admin_Event')IS NOT NULL
			DROP TABLE #Admin_Event

			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('C926D4B7-A023-4FCE-92B9-49009E5E823F', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('C926D4B7-A023-4FCE-92B9-49009E5E823F', 0, 'POST');
		END CATCH
	END
	END
END