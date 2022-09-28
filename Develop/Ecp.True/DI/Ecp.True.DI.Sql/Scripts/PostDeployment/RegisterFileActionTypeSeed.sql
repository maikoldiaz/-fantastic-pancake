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

IF	OBJECT_ID('Admin.RegisterFileActionType') IS NOT NULL
BEGIN

-- Clearing any junk data from the reserved space
--DELETE FROM [Admin].[RegisterFileActionType] WHERE [ActionTypeId] > 4 AND [ActionTypeId] < 101

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[RegisterFileActionType] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[RegisterFileActionType] WHERE [ActionTypeId] = 1)	BEGIN	INSERT [Admin].[RegisterFileActionType] ([ActionTypeId], [FileActionType], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (1, N'Insertar', N'System', @CurrentTime, NULL, NULL)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[RegisterFileActionType]		WHERE [ActionTypeId] = '1' AND ([FileActionType] <> 'Insertar'))	BEGIN	UPDATE [Admin].[RegisterFileActionType] SET [FileActionType] = 'Insertar', [LastModifiedBy] = 'System', [LastModifiedDate] = @CurrentTime WHERE [ActionTypeId] = '1'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[RegisterFileActionType] WHERE [ActionTypeId] = 2)	BEGIN	INSERT [Admin].[RegisterFileActionType] ([ActionTypeId], [FileActionType], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (2, N'Actualizar', N'System', @CurrentTime, NULL, NULL)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[RegisterFileActionType]	WHERE [ActionTypeId] = '2' AND ([FileActionType] <> 'Actualizar'))	BEGIN	UPDATE [Admin].[RegisterFileActionType] SET [FileActionType] = 'Actualizar', [LastModifiedBy] = 'System', [LastModifiedDate] = @CurrentTime WHERE [ActionTypeId] = '2'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[RegisterFileActionType] WHERE [ActionTypeId] = 3)	BEGIN	INSERT [Admin].[RegisterFileActionType] ([ActionTypeId], [FileActionType], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (3, N'Eliminar', N'System', @CurrentTime, NULL, NULL)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[RegisterFileActionType]		WHERE [ActionTypeId] = '3' AND ([FileActionType] <> 'Eliminar'))	BEGIN	UPDATE [Admin].[RegisterFileActionType] SET [FileActionType] = 'Eliminar', [LastModifiedBy] = 'System', [LastModifiedDate] = @CurrentTime WHERE [ActionTypeId] = '3'	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[RegisterFileActionType] WHERE [ActionTypeId] = 4)	BEGIN	INSERT [Admin].[RegisterFileActionType] ([ActionTypeId], [FileActionType], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate]) VALUES (4, N'Re Inyectar', N'System', @CurrentTime, NULL, NULL)	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[RegisterFileActionType]	WHERE [ActionTypeId] = '4' AND ([FileActionType] <> 'Re Inyectar'))	BEGIN	UPDATE [Admin].[RegisterFileActionType] SET [FileActionType] = 'Re Inyectar', [LastModifiedBy] = 'System', [LastModifiedDate] = @CurrentTime WHERE [ActionTypeId] = '4'	END


SET IDENTITY_INSERT [Admin].[RegisterFileActionType] OFF
END	
