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




/*
PR(s):		this
PBI(s):		NULL
Task(s):	NULL
Description:
	Renaming the Identity column of [RuleType] and [RuleName] tables as per convention so that in PostDeployment scripts remains in sync.
*/
IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='B12B42A1-CCB9-47EC-9D00-A7119445FF51' AND Status = 1)
	BEGIN
		BEGIN TRY
			IF EXISTS(SELECT * FROM sys.columns
			WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'[Admin].[RuleType]'))
			BEGIN
				PRINT 'Your Column Exists'
				EXEC SP_RENAME '[Admin].[RuleType].Id', 'RuleTypeId' , 'COLUMN'
				EXEC SP_RENAME '[Admin].[RuleName].Id', 'RuleNameId' , 'COLUMN'
			END 
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('B12B42A1-CCB9-47EC-9D00-A7119445FF51', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('B12B42A1-CCB9-47EC-9D00-A7119445FF51', 0);
		END CATCH
	END
END





IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='B570AA99-12E2-426B-8BB0-0C0E688AE283' AND Status = 1)
	BEGIN
		BEGIN TRY
			IF	OBJECT_ID('Admin.CategoryElement') IS NOT NULL
			BEGIN
				IF NOT EXISTS (SELECT 1 
							FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
							WHERE CONSTRAINT_TYPE='UNIQUE'
							AND CONSTRAINT_NAME = 'UQ_CategoryElement_Name_CategoryId')
				BEGIN
					ALTER TABLE [Admin].[CategoryElement]
					ADD CONSTRAINT [UQ_CategoryElement_Name_CategoryId] UNIQUE NONCLUSTERED ([Name], [CategoryId])
				END
			END
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('B570AA99-12E2-426B-8BB0-0C0E688AE283', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('B570AA99-12E2-426B-8BB0-0C0E688AE283', 0);
		END CATCH
	END
END