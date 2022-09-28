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

IF EXISTS (Select 'X' from sys.schemas Where name = 'Offchain')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='312a6692-e37c-4d92-a08b-57d3a3cb3e06' AND Status = 1)
	BEGIN
		BEGIN TRY
			
          -- Backup Operator values into temp table
			IF (OBJECT_ID('TempDB..#Offchain_MovementOperator') IS NULL) AND (COL_LENGTH('Offchain.Movement','Operator') IS NOT NULL)
              BEGIN
                  -- BACKUP DATA TO A TEMP TABLE
                   SELECT * INTO #Offchain_MovementOperator 
				            FROM Offchain.Movement
				   DECLARE @UpdateOperatorInMovement NVARCHAR (4000)
				   SET @UpdateOperatorInMovement = N'UPDATE Offchain.Movement SET Operator = NULL'
				   EXEC sp_executesql @UpdateOperatorInMovement			   
              END

			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('312a6692-e37c-4d92-a08b-57d3a3cb3e06', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('312a6692-e37c-4d92-a08b-57d3a3cb3e06', 0);
		END CATCH
	END
END