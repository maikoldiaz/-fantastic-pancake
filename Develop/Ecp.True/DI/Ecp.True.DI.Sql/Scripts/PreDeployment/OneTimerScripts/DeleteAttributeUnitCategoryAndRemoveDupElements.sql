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
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Offchain' AND  TABLE_NAME = 'Attribute'))
	AND (EXISTS (SELECT 'X' FROM Admin.Category WHERE CategoryId=21))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='1df4e733-5c09-4cf3-9ef0-411f60400243' AND Status = 1)
		BEGIN
			BEGIN TRY
				IF OBJECT_ID('tempdb..#TempHomologation') IS NOT NULL
								DROP TABLE #TempHomologation
				SELECT Hg.* INTO #TempHomologation
				FROM [Admin].HomologationGroup Hg
				WHERE GroupTypeId=6
				DELETE FROM Admin.HomologationDataMapping WHERE HomologationGroupId IN (SELECT HomologationGroupId FROM Admin.HomologationGroup WHERE GroupTypeId=21 AND HomologationId IN (SELECT HomologationId FROM #TempHomologation))
				DELETE FROM Admin.HomologationGroup WHERE GroupTypeId=21 AND HomologationId IN (SELECT HomologationId FROM #TempHomologation)
				UPDATE Admin.HomologationGroup SET GroupTypeId=6 WHERE GroupTypeId=21 AND HomologationId NOT IN (SELECT HomologationId FROM #TempHomologation)
				DROP TABLE #TempHomologation
				IF OBJECT_ID('tempdb..#TempElements') IS NOT NULL
					DROP TABLE #TempElements
				CREATE TABLE #TempElements
				( 
					Element1 INT,
					Element2 INT 
				)
				INSERT INTO #TempElements
				SELECT CE1.ElementId as ElementId1, CE2.ElementId as ElementId2
				FROM [Admin].CategoryElement CE1
				INNER JOIN [Admin].CategoryElement CE2
				ON CE1.[Name] = CE2.[Name]
				AND CE1.CategoryId=6
				AND CE2.categoryid=21

				DECLARE @AttributeElementQuery NVARCHAR (4000);
				SET @AttributeElementQuery = N'
				UPDATE Offchain.Attribute 
				SET ValueAttributeUnit= A.Element1
				FROM Offchain.Attribute Att
				INNER JOIN #TempElements A
				ON Att.ValueAttributeUnit = A.Element2
				AND ISNUMERIC(Att.ValueAttributeUnit) = 1';
				EXEC sp_executesql @AttributeElementQuery

				IF OBJECT_ID('tempdb..#TempElements') IS NOT NULL
					DROP TABLE #TempElements
				UPDATE [Admin].[CategoryElement] SET [CategoryId] = 6 WHERE [CategoryId]=21
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('1df4e733-5c09-4cf3-9ef0-411f60400243', 1);
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('1df4e733-5c09-4cf3-9ef0-411f60400243', 0);
			END CATCH
		END
	END
END