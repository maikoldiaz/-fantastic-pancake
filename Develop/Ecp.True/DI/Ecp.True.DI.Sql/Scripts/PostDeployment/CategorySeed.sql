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

SET IDENTITY_INSERT [Admin].[Category] ON
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 1)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			) 
	SELECT  1				AS 	[CategoryId], 
			N'Tipo de Nodo' AS 	[Name], 
			N'Tipo de Nodo' AS 	[Description], 
			1 				AS 	[IsActive], 
			1 				AS 	[IsGrouper], 
			1 				AS 	[IsReadOnly], 
			N'System' 		AS 	[CreatedBy], 
			@CurrentTime 	AS 	[CreatedDate], 
			NULL 			AS 	[LastModifiedBy], 
			NULL			AS 	[LastModifiedDate]
	
END
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 1)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Tipo de Nodo'
		,[Description] =  'Tipo de Nodo'
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 1
END

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 2)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			) 
	SELECT 2				AS	[CategoryId], 
			N'Segmento' 	AS	[Name], 
			N'Segmento' 	AS	[Description], 
			1 				AS	[IsActive], 
			1 				AS	[IsGrouper], 
			1 				AS	[IsReadOnly], 
			N'System' 		AS	[CreatedBy], 
			@CurrentTime	AS	[CreatedDate], 
			NULL 			AS	[LastModifiedBy], 
			NULL 			AS	[LastModifiedDate]
	
END
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 2)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Segmento' 
		,[Description] =  'Segmento' 
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 2
END

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 3)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			) 
	SELECT 3				AS	[CategoryId], 
			N'Operador' 	AS	[Name], 
			N'Operador' 	AS	[Description], 
			1 				AS	[IsActive], 
			1 				AS	[IsGrouper], 
			1 				AS	[IsReadOnly], 
			N'System' 		AS	[CreatedBy], 
			@CurrentTime	AS	[CreatedDate], 
			NULL 			AS	[LastModifiedBy], 
			NULL 			AS	[LastModifiedDate]
	
END
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 3)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Operador'
		,[Description] =  'Operador'
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 3
END

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 4)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			) 
	SELECT 4				 AS	[CategoryId], 
			N'Organizacional'AS	[Name],  
			N'Organizacional'AS	[Description],  
			1 				 AS	[IsActive], 
			1 				 AS	[IsGrouper], 
			1 				 AS	[IsReadOnly], 
			N'System' 		 AS	[CreatedBy], 
			@CurrentTime 	 AS	[CreatedDate], 
			NULL 			 AS	[LastModifiedBy], 
			NULL			 AS	[LastModifiedDate]
	
END
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 4)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Organizacional'
		,[Description] =  'Organizacional'
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 4
END

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 5)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT 5				AS	[CategoryId], 
			N'Geografia' 	AS	[Name], 
			N'Geografia' 	AS	[Description], 
			1 				AS	[IsActive], 
			1 				AS	[IsGrouper], 
			1 				AS	[IsReadOnly], 
			N'System' 		AS	[CreatedBy], 
			@CurrentTime	AS	[CreatedDate], 
			NULL 			AS	[LastModifiedBy], 
			NULL 			AS	[LastModifiedDate]
	
END
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 5)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Geografia'
		,[Description] =  'Geografia'
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 5
END

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 6)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			) 
	SELECT 6					AS	[CategoryId], 
			N'Unidad de Medida' AS	[Name], 
			N'Unidad de Medida' AS	[Description], 
			1 					AS	[IsActive], 
			1 					AS	[IsGrouper], 
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL				AS	[LastModifiedDate]
	
END
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 6)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Unidad de Medida'
		,[Description] =  'Unidad de Medida'
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 6
END

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 7)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT 7					AS	[CategoryId], 
			N'Propietario' 		AS	[Name], 
			N'Propietario' 		AS	[Description], 
			1 					AS	[IsActive], 
			1 					AS	[IsGrouper], 
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime 		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]
	
END
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 7)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Propietario'
		,[Description] =  'Propietario'
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 7
END

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 8)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT 8					AS	[CategoryId], 
			N'Sistema' 			AS	[Name], 
			N'Sistema' 			AS	[Description], 
			1 					AS	[IsActive], 
			1 					AS	[IsGrouper], 
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]
	
END
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 8)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Sistema' 
		,[Description] =  'Sistema' 
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 8
END

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 9)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  9					AS	[CategoryId], 
			N'Tipo Movimiento' 	AS	[Name], 
			N'Tipo Movimiento' 	AS	[Description], 
			1 					AS	[IsActive], 
			1 					AS	[IsGrouper], 
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 9)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Tipo Movimiento'
		,[Description] =  'Tipo Movimiento'
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 9
END

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 10)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  10					AS	[CategoryId], 
			N'Reglas de propiedad' 	AS	[Name], 
			N'Reglas de propiedad FICO' 	AS	[Description], 
			1 					AS	[IsActive], 
			0 					AS	[IsGrouper], 
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 10)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Reglas de propiedad'
		,[Description] =  'Reglas de propiedad FICO'
		,[isActive] = 1
		,[isGrouper] = 0
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 10
END

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 11)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  11					AS	[CategoryId], 
			N'Tipo Producto' 	AS	[Name], 
			N'Tipo Producto' 	AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 11)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Tipo Producto'
		,[Description] =  'Tipo Producto'
		,[isActive] = 1
		,[isGrouper] = 1
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 11
END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 12)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  12					AS	[CategoryId], 
			N'Tipo Evento' 		AS	[Name], 
			N'Tipo Evento' 		AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 12)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Tipo Evento'
		,[Description] =  'Tipo Evento'
		,[isActive] = 1
		,[isGrouper] = 1
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 12
END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 13)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsHomologation],
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  13					AS	[CategoryId], 
			N'Nodos' 		AS	[Name], 
			N'Nodos' 		AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			1					AS	[IsHomologation],
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 13)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Nodos'
		,[Description] =  'Nodos'
		,[isActive] = 1
		,[isGrouper] = 1
		,[IsHomologation] = 1
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 13
END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 14)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsHomologation],
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  14					AS	[CategoryId], 
			N'Productos' 		AS	[Name], 
			N'Productos' 		AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			1					AS	[IsHomologation],
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 14)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Productos'
		,[Description] =  'Productos'
		,[isActive] = 1
		,[isGrouper] = 1
		,[IsHomologation] = 1
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 14
END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 15)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper],
			[IsHomologation],
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  15					AS	[CategoryId], 
			N'Almacenes' 		AS	[Name], 
			N'Almacenes' 		AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			1					AS	[IsHomologation],
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 15)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Almacenes'
		,[Description] =  'Almacenes'
		,[isActive] = 1
		,[isGrouper] = 1
		,[IsHomologation] = 1
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 15
END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 16)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsHomologation],
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  16					AS	[CategoryId], 
			N'Causal del Cambio' 		AS	[Name], 
			N'Causal del Cambio' 		AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			0					AS	[IsHomologation],
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 16)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Causal del Cambio'
		,[Description] =  'Causal del Cambio'
		,[isActive] = 1
		,[isGrouper] = 1
		,[IsHomologation] = 0
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 16
END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 17)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsHomologation],
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  17					AS	[CategoryId], 
			N'Comercial' 		AS	[Name], 
			N'Comercial' 		AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			0					AS	[IsHomologation],
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 17)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Comercial'
		,[Description] =  'Comercial'
		,[isActive] = 1
		,[isGrouper] = 1
		,[IsHomologation] = 0
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 17
END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 18)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsHomologation],
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  18					AS	[CategoryId], 
			N'Puntos de Transferencia' 		AS	[Name], 
			N'Puntos de Transferencia del modelo analítico' 		AS	[Description], 
			1 					AS	[IsActive], 
			0					AS	[IsGrouper], 
			0					AS	[IsHomologation],
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 18)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Puntos de Transferencia'
		,[Description] =  'Puntos de Transferencia del modelo analítico'
		,[isActive] = 1
		,[isGrouper] = 0
		,[IsHomologation] = 0
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 18
END

IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 18)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Puntos de Transferencia'
		,[Description] =  'Puntos de Transferencia del modelo analítico'
		,[isActive] = 1
		,[isGrouper] = 0
		,[IsHomologation] = 0
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 18
END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 19)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsHomologation],
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  19					AS	[CategoryId], 
			N'Cadena de Suministro' 		AS	[Name], 
			N'Cadena de Suministro' 		AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			0					AS	[IsHomologation],
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 19)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Cadena de Suministro'
		,[Description] =  'Cadena de Suministro'
		,[isActive] = 1
		,[isGrouper] = 1
		,[IsHomologation] = 0
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 19
END
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 20)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsHomologation],
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  20					AS	[CategoryId], 
			N'Atributos' 		AS	[Name], 
			N'Atributos' 		AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			0					AS	[IsHomologation],
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 20)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Atributos'
		,[Description] =  'Atributos'
		,[isActive] = 1
		,[isGrouper] = 1
		,[IsHomologation] = 0
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 20
END
/*
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 21)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper], 
			[IsHomologation],
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  21					AS	[CategoryId], 
			N'Unidades Atributos' 		AS	[Name], 
			N'Unidades Atributos' 		AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			0					AS	[IsHomologation],
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 21)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Unidades Atributos'
		,[Description] =  'Unidades Atributos'
		,[isActive] = 1
		,[isGrouper] = 1
		,[IsHomologation] = 0
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 21
END
*/
IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 22)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper],
			[IsHomologation],
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  22					AS	[CategoryId], 
			N'Sistema Origen' 		AS	[Name], 
			N'Sistema Origen' 		AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			0					AS	[IsHomologation],
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 22)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Sistema Origen'
		,[Description] =  'Sistema Origen'
		,[isActive] = 1
		,[isGrouper] = 1
		,[IsHomologation] = 0
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 22
END


IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 23)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper],
			[IsHomologation],
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  23					AS	[CategoryId], 
			N'Tipo Orden' 		AS	[Name], 
			N'Tipo Orden' 		AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			0					AS	[IsHomologation],
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 23)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Tipo Orden'
		,[Description] =  'Tipo Orden'
		,[isActive] = 1
		,[isGrouper] = 1
		,[IsHomologation] = 0
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 23
END

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 24)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper],
			[IsHomologation],
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  24					AS	[CategoryId], 
			N'Centro de Costo' 		AS	[Name], 
			N'Centro de Costo' 		AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			0					AS	[IsHomologation],
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 24)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Centro de Costo'
		,[Description] =  'Centro de Costo'
		,[isActive] = 1
		,[isGrouper] = 1
		,[IsHomologation] = 0
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 24
END

IF NOT EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 25)
BEGIN 
	
	INSERT [Admin].[Category] 
		   (
			[CategoryId], 
			[Name], 
			[Description], 
			[IsActive], 
			[IsGrouper],
			[IsHomologation],
			[IsReadOnly], 
			[CreatedBy], 
			[CreatedDate], 
			[LastModifiedBy], 
			[LastModifiedDate]
			)
	SELECT  25					AS	[CategoryId], 
			N'Tipo de Transacciones Sap' 		AS	[Name], 
			N'Tipo de Transacciones Sap' 		AS	[Description], 
			1 					AS	[IsActive], 
			1					AS	[IsGrouper], 
			0					AS	[IsHomologation],
			1 					AS	[IsReadOnly], 
			N'System' 			AS	[CreatedBy], 
			@CurrentTime		AS	[CreatedDate], 
			NULL 				AS	[LastModifiedBy], 
			NULL 				AS	[LastModifiedDate]

END 
ELSE
IF EXISTS (SELECT 'X' FROM [Admin].[Category] WHERE [CategoryId] = 25)
BEGIN 
	UPDATE [Admin].[Category] 
	SET  [Name]	=	'Tipo de Transacciones Sap'
		,[Description] =  'Tipo de Transacciones Sap'
		,[isActive] = 1
		,[isGrouper] = 1
		,[IsHomologation] = 0
		,[LastModifiedBy]	= NULL
		,[LastModifiedDate] = NULL
	WHERE [CategoryId] = 25
END

SET IDENTITY_INSERT [Admin].[Category] OFF
