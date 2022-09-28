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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='321b2eeb-cce5-4efb-bd7a-445566662ed7' AND Status = 1) and
	(COL_LENGTH('TempDB..#Offchain_InventoryOperator', 'Operator') IS NOT NULL)
	BEGIN
		BEGIN TRY
                  DECLARE @InventoryOperatorUpdateQuery NVARCHAR (4000)
	                  SET @InventoryOperatorUpdateQuery = N'UPDATE Actual
		                                              SET [OperatorId] = CE.ElementId
		                                             FROM [Offchain].[InventoryProduct] Actual
		                                             JOIN #Offchain_InventoryOperator Temp
													 ON Actual.InventoryProductId = Temp.InventoryProductId
													 JOIN [Admin].[CategoryElement] CE
		                                             ON lower(Temp.Operator) = lower(CE.Name) and CE.CategoryId = 3'
	              EXEC sp_executesql @InventoryOperatorUpdateQuery
        
			IF OBJECT_ID('TempDB..#Offchain_InventoryOperator')IS NOT NULL
			DROP TABLE #Offchain_InventoryOperator
			UPDATE [Offchain].[InventoryProduct] SET OperatorId = 14 where isnull(OperatorId, 0) = 0;
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('321b2eeb-cce5-4efb-bd7a-445566662ed7', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('321b2eeb-cce5-4efb-bd7a-445566662ed7', 0, 'POST');
		END CATCH
	END
END