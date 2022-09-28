﻿/*
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

IF	OBJECT_ID('Admin.Feature') IS NOT NULL
BEGIN

-- Clearing any junk data from the reserved space
--DELETE FROM [Admin].[Feature] WHERE FeatureId > 8 AND FeatureId < 101

-- Inserting Seed data with Identity
SET IDENTITY_INSERT [Admin].[Feature] ON 

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '1')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (1, 1, N'category', N'Categoría', 1000, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '1' AND (ScenarioId <> '1' OR Name <> 'category' OR ISNULL(Description, '') <> 'Categoría' OR Sequence <> '1000'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'category', [Description] = 'Categoría', [Sequence] = 1000 WHERE [FeatureId] = 1	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '2')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (2, 1, N'categoryElements', N'Elementos de categorías', 1005, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '2' AND (ScenarioId <> '1' OR Name <> 'categoryElements' OR ISNULL(Description, '') <> 'Elementos de categorías' OR Sequence <> '1005'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'categoryElements', [Description] = 'Elementos de categorías', [Sequence] = 1005 WHERE [FeatureId] = 2	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '3')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (3, 1, N'nodeTags', N'Configuración para agrupar nodos', 1025, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '3' AND (ScenarioId <> '1' OR Name <> 'nodeTags' OR ISNULL(Description, '') <> 'Configuración para agrupar nodos' OR Sequence <> '1025'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'nodeTags', [Description] = 'Configuración para agrupar nodos', [Sequence] = 1025 WHERE [FeatureId] = 3	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '4')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (4, 1, N'nodeAttributes', N'Configuración de atributos nodos', 1015, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '4' AND (ScenarioId <> '1' OR Name <> 'nodeAttributes' OR ISNULL(Description, '') <> 'Configuración de atributos nodos' OR Sequence <> '1015'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'nodeAttributes', [Description] = 'Configuración de atributos nodos', [Sequence] = 1015 WHERE [FeatureId] = 4	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '5')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (5, 1, N'connectionAttributes', N'Configuración de atributos conexiones', 1020, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '5' AND (ScenarioId <> '1' OR Name <> 'connectionAttributes' OR ISNULL(Description, '') <> 'Configuración de atributos conexiones' OR Sequence <> '1020'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'connectionAttributes', [Description] = 'Configuración de atributos conexiones', [Sequence] = 1020 WHERE [FeatureId] = 5	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '6')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (6, 2, N'fileUpload', N'Cargue de movimientos e inventarios', 2000, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '6' AND (ScenarioId <> '2' OR Name <> 'fileUpload' OR ISNULL(Description, '') <> 'Cargue de movimientos e inventarios' OR Sequence <> '2000'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 2, [Name] = 'fileUpload', [Description] = 'Cargue de movimientos e inventarios', [Sequence] = 2000 WHERE [FeatureId] = 6	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '7')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (7, 2, N'cutOff', N'Ejecución del corte operativo', 2020, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '7' AND (ScenarioId <> '2' OR Name <> 'cutOff' OR ISNULL(Description, '') <> 'Ejecución del corte operativo' OR Sequence <> '2020'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 2, [Name] = 'cutOff', [Description] = 'Ejecución del corte operativo', [Sequence] = 2020 WHERE [FeatureId] = 7	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '8')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (8, 1, N'nodes', N'Nodos', 1010, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '8' AND (ScenarioId <> '1' OR Name <> 'nodes' OR ISNULL(Description, '') <> 'Nodos' OR Sequence <> '1010'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'nodes', [Description] = 'Nodos', [Sequence] = 1010 WHERE [FeatureId] = 8	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '9')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (9, 5, N'cutOffReport', N'Balance operativo con o sin propiedad', 5030, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '9' AND (ScenarioId <> '5' OR Name <> 'cutOffReport' OR ISNULL(Description, '') <> 'Balance operativo con o sin propiedad' OR Sequence <> '5030'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 5, [Name] = 'cutOffReport', [Description] = 'Balance operativo con o sin propiedad', [Sequence] = 5030 WHERE [FeatureId] = 9	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '10')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (10, 2, N'ownership', N'Determinación de propiedad por segmento', 2040, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '10' AND (ScenarioId <> '2' OR Name <> 'ownership' OR ISNULL(Description, '') <> 'Determinación de propiedad por segmento' OR Sequence <> '2040'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 2, [Name] = 'ownership', [Description] = 'Determinación de propiedad por segmento', [Sequence] = 2040 WHERE [FeatureId] = 10	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '11')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (11, 1, N'transformSettings', N'Configuración de transformaciones', 1040, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '11' AND (ScenarioId <> '1' OR Name <> 'transformSettings' OR ISNULL(Description, '') <> 'Configuración de transformaciones' OR Sequence <> '1040'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'transformSettings', [Description] = 'Configuración de transformaciones', [Sequence] = 1040 WHERE [FeatureId] = 11	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '12')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (12, 2, N'ownershipNodes', N'Balance operativo con propiedad por nodo',2050,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '12' AND (ScenarioId <> '2' OR Name <> 'ownershipNodes' OR ISNULL(Description, '') <> 'Balance operativo con propiedad por nodo' OR Sequence <> '2050'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 2, [Name] = 'ownershipNodes', [Description] = 'Balance operativo con propiedad por nodo', [Sequence] = 2050 WHERE [FeatureId] = 12	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '13')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (13, 2, N'transportContracts', N'Cargue de pedidos y eventos PPA',2010,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '13' AND (ScenarioId <> '2' OR Name <> 'transportContracts' OR ISNULL(Description, '') <> 'Cargue de pedidos y eventos PPA' OR Sequence <> '2010'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 2, [Name] = 'transportContracts', [Description] = 'Cargue de pedidos y eventos PPA', [Sequence] = 2010 WHERE [FeatureId] = 13	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '14')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (14, 3, N'dailyContracts', N'Cargue de pedidos y eventos PPA',3003,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '14' AND (ScenarioId <> '3' OR Name <> 'dailyContracts' OR ISNULL(Description, '') <> 'Cargue de pedidos y eventos PPA' OR Sequence <> '3003'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'dailyContracts', [Description] = 'Cargue de pedidos y eventos PPA', [Sequence] = 3003 WHERE [FeatureId] = 14	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '15')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (15, 1, N'exceptions', N'Gestión de excepciones',1045,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '15' AND (ScenarioId <> '1' OR Name <> 'exceptions' OR ISNULL(Description, '') <> 'Gestión de excepciones' OR Sequence <> '1045'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'exceptions', [Description] = 'Gestión de excepciones', [Sequence] = 1045 WHERE [FeatureId] = 15	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '16')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (16, 5, N'logistics', N'Movimientos e inventarios logísticos',5060,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '16' AND (ScenarioId <> '5' OR Name <> 'logistics' OR ISNULL(Description, '') <> 'Movimientos e inventarios logísticos' OR Sequence <> '5060'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 5, [Name] = 'logistics', [Description] = 'Movimientos e inventarios logísticos', [Sequence] = 5060 WHERE [FeatureId] = 16	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '17')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (17, 1, N'homologations', N'Configuración de homologaciones',1035,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '17' AND (ScenarioId <> '1' OR Name <> 'homologations' OR ISNULL(Description, '') <> 'Configuración de homologaciones' OR Sequence <> '1035'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'homologations', [Description] = 'Configuración de homologaciones', [Sequence] = 1035 WHERE [FeatureId] = 17	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '18')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (18, 5, N'analyticalModel', N'Modelos analíticos predictivos', 5020, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '18' AND (ScenarioId <> '5' OR Name <> 'analyticalModel' OR ISNULL(Description, '') <> 'Modelos analíticos predictivos' OR Sequence <> '5020'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 5, [Name] = 'analyticalModel', [Description] = 'Modelos analíticos predictivos', [Sequence] = 5020 WHERE [FeatureId] = 18	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '21')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (21, 1, N'transferPointsOperational', N'Configuración de puntos transferencia - nodos operativos', 1050, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '21' AND (ScenarioId <> '1' OR Name <> 'transferPointsOperational' OR ISNULL(Description, '') <> 'Configuración de puntos transferencia - nodos operativos' OR Sequence <> '1050'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'transferPointsOperational', [Description] = 'Configuración de puntos transferencia - nodos operativos', [Sequence] = 1050 WHERE [FeatureId] = 21	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '23')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (23, 1, N'flowSettings', N'Configuración de flujos de aprobación', 1060, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '23' AND (ScenarioId <> '1' OR Name <> 'flowSettings' OR ISNULL(Description, '') <> 'Configuración de flujos de aprobación' OR Sequence <> '1060'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'flowSettings', [Description] = 'Configuración de flujos de aprobación', [Sequence] = 1060 WHERE [FeatureId] = 23	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '24')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (24, 2, N'nodeApproval', N'Aprobación del balance con propiedad por nodo', 2060, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '24' AND (ScenarioId <> '2' OR Name <> 'nodeApproval' OR ISNULL(Description, '') <> 'Aprobación del balance con propiedad por nodo' OR Sequence <> '2060'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 2, [Name] = 'nodeApproval', [Description] = 'Aprobación del balance con propiedad por nodo', [Sequence] = 2060 WHERE [FeatureId] = 24	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '25')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (25, 5, N'eventContractReport', N'Configuración de pedidos y eventos PPA', 5000, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '25' AND (ScenarioId <> '5' OR Name <> 'eventContractReport' OR ISNULL(Description, '') <> 'Configuración de pedidos y eventos PPA' OR Sequence <> '5000'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 5, [Name] = 'eventContractReport', [Description] = 'Configuración de pedidos y eventos PPA', [Sequence] = 5000 WHERE [FeatureId] = 25	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '26')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (26, 5, N'balanceControlChart', N'Carta de control', 5050, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '26' AND (ScenarioId <> '5' OR Name <> 'balanceControlChart' OR ISNULL(Description, '') <> 'Carta de control' OR Sequence <> '5050'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 5, [Name] = 'balanceControlChart', [Description] = 'Carta de control', [Sequence] = 5050 WHERE [FeatureId] = 26	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '27')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (27, 1, N'graphicConfigurationNetwork', N'Configuración gráfica de la red', 1030, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '27' AND (ScenarioId <> '1' OR Name <> 'graphicConfigurationNetwork' OR ISNULL(Description, '') <> 'Configuración gráfica de la red' OR Sequence <> '1030'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'graphicConfigurationNetwork', [Description] = 'Configuración gráfica de la red', [Sequence] = 1030 WHERE [FeatureId] = 27	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '28')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (28, 1, N'transferPointsLogistics', N'Configuración de puntos transferencia - nodos logísticos', 1055, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '28' AND (ScenarioId <> '1' OR Name <> 'transferPointsLogistics' OR ISNULL(Description, '') <> 'Configuración de puntos transferencia - nodos logísticos' OR Sequence <> '1055'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'transferPointsLogistics', [Description] = 'Configuración de puntos transferencia - nodos logísticos', [Sequence] = 1055 WHERE [FeatureId] = 28	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '29')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (29, 5, N'nodeConfigurationReport', N'Configuración detallada por nodo', 5010, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '29' AND (ScenarioId <> '5' OR Name <> 'nodeConfigurationReport' OR ISNULL(Description, '') <> 'Configuración detallada por nodo' OR Sequence <> '5010'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 5, [Name] = 'nodeConfigurationReport', [Description] = 'Configuración detallada por nodo', [Sequence] = 5010 WHERE [FeatureId] = 29	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '30')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (30, 5, N'nodeStatusReport', N'Estados de los nodos', 5040, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '30' AND (ScenarioId <> '5' OR Name <> 'nodeStatusReport' OR ISNULL(Description, '') <> 'Estados de los nodos' OR Sequence <> '5040'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 5, [Name] = 'nodeStatusReport', [Description] = 'Estados de los nodos', [Sequence] = 5040 WHERE [FeatureId] = 30	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '31')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (31, 1, N'settingsAudit', N'Auditoría de configuraciones', 1070, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '31' AND (ScenarioId <> '1' OR Name <> 'settingsAudit' OR ISNULL(Description, '') <> 'Auditoría de configuraciones' OR Sequence <> '1070'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'settingsAudit', [Description] = 'Auditoría de configuraciones', [Sequence] = 1070 WHERE [FeatureId] = 31	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '32')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (32, 1, N'annulations', N'Relación tipos de movimientos', 1065, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '32' AND (ScenarioId <> '1' OR Name <> 'annulations' OR ISNULL(Description, '') <> 'Relación tipos de movimientos' OR Sequence <> '1065'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'annulations', [Description] = 'Relación tipos de movimientos', [Sequence] = 1065 WHERE [FeatureId] = 32	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '33')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (33, 3, N'transactionsAudit', N'Auditoría de movimientos e inventarios',3020,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '33' AND (ScenarioId <> '3' OR Name <> 'transactionsAudit' OR ISNULL(Description, '') <> 'Auditoría de movimientos e inventarios' OR Sequence <> '3020'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'transactionsAudit', [Description] = 'Auditoría de movimientos e inventarios', [Sequence] = 3020 WHERE [FeatureId] = 33	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '34')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (34, 2, N'deltaCalculation', N'Cálculo de deltas por ajuste operativo',2030,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '34' AND (ScenarioId <> '2' OR Name <> 'deltaCalculation' OR ISNULL(Description, '') <> 'Cálculo de deltas por ajuste operativo' OR Sequence <> '2030'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 2, [Name] = 'deltaCalculation', [Description] = 'Cálculo de deltas por ajuste operativo', [Sequence] = 2030 WHERE [FeatureId] = 34	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '35')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (35, 1, N'operationalSegments', N'Configuración de segmentos en TRUE como SON',1075,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '35' AND (ScenarioId <> '1' OR Name <> 'operationalSegments' OR ISNULL(Description, '') <> 'Configuración de segmentos en TRUE como SON' OR Sequence <> '1075'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'operationalSegments', [Description] = 'Configuración de segmentos en TRUE como SON', [Sequence] = 1075 WHERE [FeatureId] = 35	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '36')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (36, 3, N'officialDelta', N'Cálculo de deltas por ajuste oficial', 3005,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '36' AND (ScenarioId <> '3' OR Name <> 'officialDelta' OR ISNULL(Description, '') <> 'Cálculo de deltas por ajuste oficial' OR Sequence <> '3005'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'officialDelta', [Description] = 'Cálculo de deltas por ajuste oficial', [Sequence] = 3005 WHERE [FeatureId] = 36 END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '37')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (37, 3, N'officialDeltaPerNode', N'Deltas oficiales por nodo', 3010,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '37' AND (ScenarioId <> '3' OR Name <> 'officialDeltaPerNode' OR ISNULL(Description, '') <> 'Deltas oficiales por nodo' OR Sequence <> '3010'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'officialDeltaPerNode', [Description] = 'Deltas oficiales por nodo', [Sequence] = 3010 WHERE [FeatureId] = 37 END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '38')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (38, 3, N'officialCustodyTransferPoints', N'Puntos oficiales de transferencia de custodia', 3015,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '38' AND (ScenarioId <> '3' OR Name <> 'officialCustodyTransferPoints' OR ISNULL(Description, '') <> 'Puntos oficiales de transferencia de custodia' OR Sequence <> '3015'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'officialCustodyTransferPoints', [Description] = 'Puntos oficiales de transferencia de custodia', [Sequence] = 3015 WHERE [FeatureId] = 38	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '39')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (39, 3, N'operativeBalanceReport', N'Balance operativo con o sin propiedad', 3025,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '39' AND (ScenarioId <> '3' OR Name <> 'operativeBalanceReport' OR ISNULL(Description, '') <> 'Balance operativo con o sin propiedad' OR Sequence <> '3025'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'operativeBalanceReport', [Description] = 'Balance operativo con o sin propiedad', [Sequence] = 3025 WHERE [FeatureId] = 39	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '40')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (40, 3, N'officialDeltaNode', N'Balance oficial por nodo', 3030,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '40' AND (ScenarioId <> '3' OR Name <> 'officialDeltaNode' OR ISNULL(Description, '') <> 'Balance oficial por nodo' OR Sequence <> '3030'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'officialDeltaNode', [Description] = 'Balance oficial por nodo', [Sequence] = 3030 WHERE [FeatureId] = 40	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '41')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (41, 3, N'officialBalanceLoaded', N'Balance oficial inicial cargado', 3035,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '41' AND (ScenarioId <> '3' OR Name <> 'officialBalanceLoaded' OR ISNULL(Description, '') <> 'Balance oficial inicial cargado' OR Sequence <> '3035'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'officialBalanceLoaded', [Description] = 'Balance oficial inicial cargado', [Sequence] = 3035 WHERE [FeatureId] = 41	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '42')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (42, 3, N'previousPeriodPendingOfficial', N'Pendientes oficiales de períodos anteriores', 3040,N'System',@CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '42' AND (ScenarioId <> '3' OR Name <> 'previousPeriodPendingOfficial' OR ISNULL(Description, '') <> 'Pendientes oficiales de períodos anteriores' OR Sequence <> '3040'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'previousPeriodPendingOfficial', [Description] = 'Pendientes oficiales de períodos anteriores', [Sequence] = 3040 WHERE [FeatureId] = 42	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '43')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (43, 1, N'blockchain', N'Consulta blockchain', 1080, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '43' AND (ScenarioId <> '1' OR Name <> 'blockchain' OR ISNULL(Description, '') <> 'Consulta blockchain' OR Sequence <> '1080'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'blockchain', [Description] = 'Consulta blockchain', [Sequence] = 1080 WHERE [FeatureId] = 43	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '44')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (44, 3, N'officialNodeApproval', N'Aprobación del balance oficial por nodo', 3045, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '44' AND (ScenarioId <> '3' OR Name <> 'officialNodeApproval' OR ISNULL(Description, '') <> 'Aprobación del balance oficial por nodo' OR Sequence <> '3045'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'officialNodeApproval', [Description] = 'Aprobación del balance oficial por nodo', [Sequence] = 3045 WHERE [FeatureId] = 44	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '45')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (45, 3, N'officialLogistics', N'Movimientos e inventarios logísticos oficiales', 3050, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '45' AND (ScenarioId <> '3' OR Name <> 'officialLogistics' OR ISNULL(Description, '') <> 'Movimientos e inventarios logísticos oficiales' OR Sequence <> '3050'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'officialLogistics', [Description] = 'Movimientos e inventarios logísticos oficiales', [Sequence] = 3050 WHERE [FeatureId] = 45	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '46')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (46, 3, N'generatedSupplyChainReport', N'Reportes generados', 3055, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '46' AND (ScenarioId <> '3' OR Name <> 'generatedSupplyChainReport' OR ISNULL(Description, '') <> 'Reportes generados' OR Sequence <> '3055'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'generatedSupplyChainReport', [Description] = 'Reportes generados', [Sequence] = 3055 WHERE [FeatureId] = 46	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '47')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (47, 5, N'generatedReport', N'Reportes generados', 5070, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '47' AND (ScenarioId <> '5' OR Name <> 'generatedReport' OR ISNULL(Description, '') <> 'Reportes generados' OR Sequence <> '5070'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 5, [Name] = 'generatedReport', [Description] = 'Reportes generados', [Sequence] = 5070 WHERE [FeatureId] = 47	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '48')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (48, 3, N'sentToSap', N'Proceso de envíos a SAP', 3060, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '48' AND (ScenarioId <> '3' OR Name <> 'sentToSap' OR ISNULL(Description, '') <> 'Proceso de envíos a SAP' OR Sequence <> '3060'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'sentToSap', [Description] = 'Proceso de envíos a SAP', [Sequence] = 3060 WHERE [FeatureId] = 48	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '49')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (49, 1, N'registryDeviationConfiguration', N'Configuración de desviación de registro', 1012, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '49' AND (ScenarioId <> '1' OR Name <> 'registryDeviationConfiguration' OR ISNULL(Description, '') <> 'Configuración de desviación de registro' OR Sequence <> '1012'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'registryDeviationConfiguration', [Description] = 'Configuración de desviación de registro', [Sequence] = 1012 WHERE [FeatureId] = 49	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '50')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (50, 3, N'sentToSapReport', N'Estado de envíos a SAP', 3065, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '50' AND (ScenarioId <> '3' OR Name <> 'sentToSapReport' OR ISNULL(Description, '') <> 'Estado de envíos a SAP' OR Sequence <> '3065'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'sentToSapReport', [Description] = 'Estado de envíos a SAP', [Sequence] = 3065 WHERE [FeatureId] = 50	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '51')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (51, 3, N'userRolesAndPermissions', N'Permisos y roles de usuario', 3000, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '51' AND (ScenarioId <> '3' OR Name <> 'userRolesAndPermissions' OR ISNULL(Description, '') <> 'Permisos y roles de usuario' OR Sequence <> '3000'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'userRolesAndPermissions', [Description] = 'Permisos y roles de usuario', [Sequence] = 3000 WHERE [FeatureId] = 51	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '52')	BEGIN	INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (52, 1, N'productConfiguration', N'Configuración de productos', 1062, N'System', @CurrentTime);	END	ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '52' AND (ScenarioId <> '1' OR Name <> 'productConfiguration' OR ISNULL(Description, '') <> 'Configuración de productos' OR Sequence <> '1062'))	BEGIN	UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'productConfiguration', [Description] = 'Configuración de productos', [Sequence] = 1062 WHERE [FeatureId] = 52	END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '53')  BEGIN   INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (53, 3, N'officialNodeStatusReport', N'Estado de nodos oficiales', 3047, N'System', @CurrentTime ); END ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '53' AND (ScenarioId <> '3' OR Name <> 'officialNodeStatusReport' OR ISNULL(Description, '') <> 'Estado de nodos oficiales' OR Sequence <> '3047')) BEGIN UPDATE [Admin].[Feature] SET [ScenarioId] = 3, [Name] = 'officialNodeStatusReport', [Description] = 'Estado de nodos oficiales', [Sequence] = 3047 WHERE [FeatureId] = 53 END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '55')  BEGIN   INSERT [Admin].[Feature] ([FeatureId], [ScenarioId], [Name], [Description], [Sequence], [CreatedBy], [CreatedDate]) VALUES (55, 1, N'integrationManagement', N'Gestión de Integraciones', 1100, N'System', @CurrentTime ); END ELSE IF EXISTS (SELECT 'X' FROM [Admin].[Feature] WHERE [FeatureId] = '55' AND (ScenarioId <> '1' OR Name <> 'integrationManagement' OR ISNULL(Description, '') <> 'Gestión de Integraciones' OR Sequence <> '3065')) BEGIN UPDATE [Admin].[Feature] SET [ScenarioId] = 1, [Name] = 'integrationManagement', [Description] = 'Gestión de Integraciones', [Sequence] = 1100 WHERE [FeatureId] = 55 END

SET IDENTITY_INSERT [Admin].[Feature] OFF
END	
