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
	IF (COL_LENGTH('TempDB..#TempMovementForMigration','SourceSystem') IS NOT NULL) AND (COL_LENGTH('TempDB..#TempMovementForMigration','SystemName') IS NOT NULL) AND (COL_LENGTH('TempDB..#TempInventoryProductForMigration','SourceSystem') IS NOT NULL) AND (COL_LENGTH('TempDB..#TempInventoryProductForMigration','SystemName') IS NOT NULL)
	BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='A8A0083B-40FD-43A8-8198-FCA298081D7D' AND Status = 1)
	BEGIN
		BEGIN TRY

			DECLARE @PostMigrateUpdateQuery NVARCHAR(4000)
			SET @PostMigrateUpdateQuery = N'UPDATE Offchain.Movement 
			SET SourceSystemId = 164 
			FROM Offchain.Movement mov
			INNER JOIN #TempMovementForMigration tempmov
			ON mov.MovementTransactionId = tempmov.MovementTransactionId
			WHERE tempmov.SourceSystem = ''EXCEL''
			OR tempmov.SourceSystem = ''10'' 
			OR tempmov.SourceSystem = ''2'';

			UPDATE Offchain.Movement
			SET SourceSystemId = 165 
			FROM Offchain.Movement mov
			INNER JOIN #TempMovementForMigration tempmov
			ON mov.MovementTransactionId = tempmov.MovementTransactionId
			WHERE tempmov.SourceSystem = ''TRUE'';

			UPDATE Offchain.Movement 
			SET SourceSystemId = 166 
			FROM Offchain.Movement mov
			INNER JOIN #TempMovementForMigration tempmov
			ON mov.MovementTransactionId = tempmov.MovementTransactionId
			WHERE tempmov.SourceSystem = ''SINOPER'';

			UPDATE Offchain.Movement 
			SET SourceSystemId = 188 
			FROM Offchain.Movement mov
			INNER JOIN #TempMovementForMigration tempmov
			ON mov.MovementTransactionId = tempmov.MovementTransactionId
			WHERE tempmov.SourceSystem = ''FICO'';

			UPDATE Offchain.InventoryProduct 
			SET SourceSystemId = 164
			FROM Offchain.InventoryProduct InvPrd
			JOIN #TempInventoryProductForMigration TempInvPrd
			On InvPrd.InventoryProductId = TempInvPrd.InventoryProductId
			WHERE TempInvPrd.SourceSystem = ''EXCEL'' 
			OR TempInvPrd.SourceSystem = ''10'' 
			OR TempInvPrd.SourceSystem = ''2'';

			UPDATE Offchain.InventoryProduct 
			SET SourceSystemId = 165 
			FROM Offchain.InventoryProduct InvPrd
			JOIN #TempInventoryProductForMigration TempInvPrd
			On InvPrd.InventoryProductId = TempInvPrd.InventoryProductId
			WHERE TempInvPrd.SourceSystem = ''TRUE'';

			UPDATE Offchain.InventoryProduct 
			SET SourceSystemId = 166 
			FROM Offchain.InventoryProduct InvPrd
			JOIN #TempInventoryProductForMigration TempInvPrd
			On InvPrd.InventoryProductId = TempInvPrd.InventoryProductId
			WHERE TempInvPrd.SourceSystem = ''SINOPER'';

			UPDATE Offchain.InventoryProduct 
			SET SourceSystemId = 188 
			FROM Offchain.InventoryProduct InvPrd
			JOIN #TempInventoryProductForMigration TempInvPrd
			On InvPrd.InventoryProductId = TempInvPrd.InventoryProductId
			WHERE TempInvPrd.SourceSystem = ''FICO'';'

			EXEC (@PostMigrateUpdateQuery)

			--Update SourceSystemId and SystemId in Movement and InventoryProduct Tables
			DECLARE @MovUpdateQuery NVARCHAR(4000)
			SET @MovUpdateQuery = N'UPDATE Offchain.Movement 
			SET SourceSystemId = CASE WHEN  SourceSystemId IS NULL   THEN SrcName.ElementId  ELSE SourceSystemId END,
				SystemId	   = CASE WHEN  SystemId IS NULL		 THEN SystName.ElementId ELSE SystemId	   END
			FROM Offchain.Movement Mov
			JOIN #TempMovementForMigration TempMov
			ON Mov.MovementTransactionId=TempMov.MovementTransactionId
			LEFT JOIN [Admin].[CategoryElement] SystName
			ON SystName.Name = CASE WHEN TempMov.SystemName IN (''EXCEL - OCENSA'' , ''EXCEL CENIT'', ''EXCEL INVENTARIOS'', ''EXCEL SINOPER'') THEN ''EXCEL'' ELSE TempMov.SystemName END
			LEFT JOIN [Admin].[CategoryElement] SrcName
			ON SrcName.Name = TempMov.SourceSystem;'
			EXEC (@MovUpdateQuery)

			DECLARE @InvPrdUpdateQuery NVARCHAR(4000)
			SET @InvPrdUpdateQuery = N'	UPDATE Offchain.InventoryProduct 
			SET SourceSystemId = CASE WHEN  SourceSystemId IS NULL   THEN SrcName.ElementId  ELSE SourceSystemId END,
				SystemId	   = CASE WHEN  SystemId IS NULL		 THEN SystName.ElementId ELSE SystemId	    END
			FROM Offchain.InventoryProduct InvPrd
			JOIN #TempInventoryProductForMigration TempInvPrd
			On InvPrd.InventoryProductId = TempInvPrd.InventoryProductId
			LEFT JOIN [Admin].[CategoryElement] SystName
			ON SystName.Name = CASE WHEN TempInvPrd.SystemName IN (''EXCEL - OCENSA'' , ''EXCEL CENIT'', ''EXCEL INVENTARIOS'', ''EXCEL SINOPER'') THEN ''EXCEL'' ELSE TempInvPrd.SystemName END
			LEFT JOIN [Admin].[CategoryElement] SrcName
			ON SrcName.Name = TempInvPrd.SourceSystem;'
			EXEC (@InvPrdUpdateQuery)
	          

			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('A8A0083B-40FD-43A8-8198-FCA298081D7D', 1, 'POST');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('A8A0083B-40FD-43A8-8198-FCA298081D7D', 0, 'POST');
		END CATCH
	END
	END
END