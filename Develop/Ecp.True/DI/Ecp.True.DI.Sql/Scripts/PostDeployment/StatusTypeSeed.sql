/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF	OBJECT_ID('Admin.StatusType') IS NOT NULL

BEGIN

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[StatusType] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '0')	BEGIN	INSERT [Admin].[StatusType] ([StatusTypeId], [StatusType], [CreatedBy], [CreatedDate]) VALUES (0, N'Finalizado', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '0' AND (StatusType <> 'Finalizado'))	BEGIN	UPDATE [Admin].[StatusType] SET [StatusType] = 'Finalizado' WHERE [StatusTypeId] = 0	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '1')	BEGIN	INSERT [Admin].[StatusType] ([StatusTypeId], [StatusType], [CreatedBy], [CreatedDate]) VALUES (1, N'Procesando', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '1' AND (StatusType <> 'Procesando'))	BEGIN	UPDATE [Admin].[StatusType] SET [StatusType] = 'Procesando' WHERE [StatusTypeId] = 1	END 
IF NOT EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '2')	BEGIN	INSERT [Admin].[StatusType] ([StatusTypeId], [StatusType], [CreatedBy], [CreatedDate]) VALUES (2, N'Fallido', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '2' AND (StatusType <> 'Fallido'))		BEGIN	UPDATE [Admin].[StatusType] SET [StatusType] = 'Fallido' WHERE [StatusTypeId] = 2	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '3')	BEGIN	INSERT [Admin].[StatusType] ([StatusTypeId], [StatusType], [CreatedBy], [CreatedDate]) VALUES (3, N'Enviado', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '3' AND (StatusType <> 'Enviado'))		BEGIN	UPDATE [Admin].[StatusType] SET [StatusType] = 'Enviado'	WHERE [StatusTypeId] = 3	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '4')	BEGIN	INSERT [Admin].[StatusType] ([StatusTypeId], [StatusType], [CreatedBy], [CreatedDate]) VALUES (4, N'Deltas', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '4' AND (StatusType <> 'Deltas'))		BEGIN	UPDATE [Admin].[StatusType] SET [StatusType] = 'Deltas'	WHERE [StatusTypeId] = 4	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '5')	BEGIN	INSERT [Admin].[StatusType] ([StatusTypeId], [StatusType], [CreatedBy], [CreatedDate]) VALUES (5, N'Visualizacion', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '5' AND (StatusType <> 'Visualizacion'))		BEGIN	UPDATE [Admin].[StatusType] SET [StatusType] = 'Visualizacion'	WHERE [StatusTypeId] = 5	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '6')	BEGIN	INSERT [Admin].[StatusType] ([StatusTypeId], [StatusType], [CreatedBy], [CreatedDate]) VALUES (6, N'Error', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '6' AND (StatusType <> 'Error'))		BEGIN	UPDATE [Admin].[StatusType] SET [StatusType] = 'Error'	WHERE [StatusTypeId] = 6	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '7')	BEGIN	INSERT [Admin].[StatusType] ([StatusTypeId], [StatusType], [CreatedBy], [CreatedDate]) VALUES (7, N'', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '7' AND (StatusType <> ''))		BEGIN	UPDATE [Admin].[StatusType] SET [StatusType] = ''	WHERE [StatusTypeId] = 7	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '8')	BEGIN	INSERT [Admin].[StatusType] ([StatusTypeId], [StatusType], [CreatedBy], [CreatedDate]) VALUES (8, N'Cancelado', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '8' AND (StatusType <> 'Cancelado'))		BEGIN	UPDATE [Admin].[StatusType] SET [StatusType] = 'Cancelado'	WHERE [StatusTypeId] = 8	END

IF NOT EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '9')	BEGIN	INSERT [Admin].[StatusType] ([StatusTypeId], [StatusType], [CreatedBy], [CreatedDate]) VALUES (9, N'Reenviado', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '9' AND (StatusType <> 'Reenviado'))		BEGIN	UPDATE [Admin].[StatusType] SET [StatusType] = 'Reenviado'	WHERE [StatusTypeId] = 9	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '10')	BEGIN	INSERT [Admin].[StatusType] ([StatusTypeId], [StatusType], [CreatedBy], [CreatedDate]) VALUES (10, N'Fallo conciliación', N'System', @CurrentTime)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[StatusType] WHERE [StatusTypeId] = '10' AND (StatusType <> 'Fallo conciliación'))		BEGIN	UPDATE [Admin].[StatusType] SET [StatusType] = 'Fallo conciliación'	WHERE [StatusTypeId] = 10	END

SET IDENTITY_INSERT [Admin].[StatusType] OFF
END