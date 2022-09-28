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


IF EXISTS (Select 'X' from sys.schemas Where name = 'Offchain')
BEGIN
	IF COL_LENGTH('TempDB..#Offchain_MovementOperator','Operator') IS NOT NULL
	BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='50cef64e-d81c-490b-b559-9e95741b2796' AND Status = 1)
	BEGIN
		BEGIN TRY
                  DECLARE @MovementOperatorUpdateQuery NVARCHAR (4000)
	                  SET @MovementOperatorUpdateQuery = N'UPDATE Actual
		                                              SET [OperatorId] = CE.ElementId
		                                             FROM [Offchain].[Movement] Actual
		                                             JOIN #Offchain_MovementOperator Temp
													 ON Actual.MovementTransactionId = Temp.MovementTransactionId
													 JOIN [Admin].[CategoryElement] CE
		                                             ON lower(Temp.Operator) = lower(CE.Name) and CE.CategoryId = 3'
	              EXEC sp_executesql @MovementOperatorUpdateQuery
        
			IF OBJECT_ID('TempDB..#Offchain_MovementOperator')IS NOT NULL
			DROP TABLE #Offchain_MovementOperator
			UPDATE [Offchain].[Movement] SET OperatorId = 14 where isnull(OperatorId, 0) = 0;
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('50cef64e-d81c-490b-b559-9e95741b2796', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('50cef64e-d81c-490b-b559-9e95741b2796', 0, 'POST');
		END CATCH
	END
	END
END