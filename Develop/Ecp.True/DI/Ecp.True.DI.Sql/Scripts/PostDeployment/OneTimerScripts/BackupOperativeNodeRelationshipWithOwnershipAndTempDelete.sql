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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='E28F0D43-08E7-45D8-95B0-8ED0EED71C88' AND Status = 1)
	BEGIN
		BEGIN TRY
                  DECLARE @OperativeUpdateQuery NVARCHAR (4000)
	                  SET @OperativeUpdateQuery = N'UPDATE Actual
		                                              SET [LogisticSourceCenter] = SourceStorageLocation
													    , [LogisticDestinationCenter] = DestinationStorageLocation
		                                             FROM Analytics.OperativeNodeRelationshipWithOwnership Actual
		                                             JOIN #Analytics_OperativeNodeRelationshipWithOwnership Temp
		                                               ON Actual.OperativeNodeRelationshipWithOwnershipid = Temp.OperativeNodeRelationshipWithOwnershipid'
	              EXEC sp_executesql @OperativeUpdateQuery
        
			IF OBJECT_ID('tempdb..#Analytics_OperativeNodeRelationshipWithOwnership')IS NOT NULL
			DROP TABLE #Analytics_OperativeNodeRelationshipWithOwnership

			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('E28F0D43-08E7-45D8-95B0-8ED0EED71C88', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('E28F0D43-08E7-45D8-95B0-8ED0EED71C88', 0, 'POST');
		END CATCH
	END
END