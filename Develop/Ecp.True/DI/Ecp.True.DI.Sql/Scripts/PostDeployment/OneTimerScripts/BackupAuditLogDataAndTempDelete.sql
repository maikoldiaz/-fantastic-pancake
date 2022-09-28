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


IF EXISTS (Select 'X' from sys.schemas Where name = 'Audit')
BEGIN
	IF COL_LENGTH('TempDB..#Audit_AuditLog','NodeCode') IS NOT NULL AND COL_LENGTH('TempDB..#Audit_AuditLog','StoreLocationCode') IS NOT NULL AND COL_LENGTH('TempDB..#Audit_AuditLog','PK') IS NOT NULL
	BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='7621659f-7f56-462b-9cf2-d7fbea9be0c7' AND Status = 1)
	BEGIN
		BEGIN TRY
                  DECLARE @UpdateQuery NVARCHAR (4000)
	              SET @UpdateQuery = N'UPDATE Actual
		                               		SET [Identity] = NodeCode
		                               		FROM [Audit].[AuditLog] Actual
		                               		JOIN #Audit_AuditLog Temp
		                                	ON Actual.AuditLogId = Temp.AuditLogId
											WHERE Temp.PK IS NULL AND Temp.NodeCode IS NOT NULL';

	              EXEC sp_executesql @UpdateQuery
				  SET @UpdateQuery = N'UPDATE Actual
		                               		SET [Identity] = StoreLocationCode
		                               		FROM [Audit].[AuditLog] Actual
		                               		JOIN #Audit_AuditLog Temp
		                               		ON Actual.AuditLogId = Temp.AuditLogId
											WHERE Temp.PK IS NULL AND Temp.StoreLocationCode IS NOT NULL';

	              EXEC sp_executesql @UpdateQuery
				  SET @UpdateQuery = N'UPDATE Actual
											SET [Identity] = PK
		                                	FROM [Audit].[AuditLog] Actual
		                                	JOIN #Audit_AuditLog Temp
		                                	ON Actual.AuditLogId = Temp.AuditLogId
											WHERE Temp.PK IS NOT NULL';
				  
				  EXEC sp_executesql @UpdateQuery
 
			IF OBJECT_ID('tempdb..#Audit_AuditLog')IS NOT NULL
			DROP TABLE #Audit_AuditLog

			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('7621659f-7f56-462b-9cf2-d7fbea9be0c7', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('7621659f-7f56-462b-9cf2-d7fbea9be0c7', 0, 'POST');
		END CATCH
	END
	END
END