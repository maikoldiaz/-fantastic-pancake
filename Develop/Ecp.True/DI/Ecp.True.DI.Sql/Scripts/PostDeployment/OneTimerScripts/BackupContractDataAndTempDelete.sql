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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='A04C5340-D661-40EA-9A86-F4E196A3831D' AND Status = 1)
	BEGIN
		BEGIN TRY
                  DECLARE @ContractUpdateQuery NVARCHAR (4000)
	                  SET @ContractUpdateQuery = N'UPDATE Actual
		                                              SET [Owner1Id] = CommercialId
		                                             FROM [Admin].[Contract] Actual
		                                             JOIN #Admin_Contract Temp
		                                               ON Actual.ContractId = Temp.ContractId'
	              EXEC sp_executesql @ContractUpdateQuery
        
			IF OBJECT_ID('tempdb..#Admin_Contract')IS NOT NULL
			DROP TABLE #Admin_Contract
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('A04C5340-D661-40EA-9A86-F4E196A3831D', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('A04C5340-D661-40EA-9A86-F4E196A3831D', 0, 'POST');
		END CATCH
	END
END