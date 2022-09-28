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

SET IDENTITY_INSERT [Admin].[Algorithm]  ON
IF NOT EXISTS (SELECT 1 FROM [Admin].[Algorithm]  WHERE [AlgorithmId] = 1)
BEGIN

	 INSERT INTO [Admin].[Algorithm]
           (
			[AlgorithmId]
		   ,[ModelName]
           ,[PeriodsToForecast]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[LastModifiedBy]
           ,[LastModifiedDate]
		   )
	 SELECT  1				AS	[AlgorithmId]
		    ,'ARIMA'		AS	[ModelName]
            ,1				AS	[PeriodsToForecast]
		    ,N'System' 		AS	[CreatedBy]
		    ,@CurrentTime	AS	[CreatedDate]
            ,NULL			AS	[LastModifiedBy]
            ,NULL			AS	[LastModifiedDate]
	
END
ELSE
IF EXISTS (SELECT 1 FROM [Admin].[Algorithm]  WHERE [AlgorithmId] = 1)
BEGIN 
	UPDATE [Admin].[Algorithm]  
	SET  [ModelName]		= 	'ARIMA'
		,[LastModifiedBy]	=   'System' 
		,[LastModifiedDate] =   @CurrentTime
	WHERE [AlgorithmId] = 1
END

IF NOT EXISTS (SELECT 1 FROM [Admin].[Algorithm]  WHERE [AlgorithmId] = 2)
BEGIN 
	
	 INSERT INTO [Admin].[Algorithm]
           (
			[AlgorithmId]
		   ,[ModelName]
           ,[PeriodsToForecast]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[LastModifiedBy]
           ,[LastModifiedDate]
		   )
	 SELECT  2				AS	[AlgorithmId]
		    ,'PROPHET'		AS	[ModelName]
            ,1				AS	[PeriodsToForecast]
		    ,N'System' 		AS	[CreatedBy]
		    ,@CurrentTime	AS	[CreatedDate]
            ,NULL			AS	[LastModifiedBy]
            ,NULL			AS	[LastModifiedDate]
	
END
ELSE
IF EXISTS (SELECT 1 FROM [Admin].[Algorithm]  WHERE [AlgorithmId] = 2)
BEGIN 
	UPDATE [Admin].[Algorithm]  
	SET  [ModelName]		= 	'PROPHET'
		,[LastModifiedBy]	=   'System' 
		,[LastModifiedDate] =   @CurrentTime
	WHERE [AlgorithmId] = 2
END

IF NOT EXISTS (SELECT 1 FROM [Admin].[Algorithm]  WHERE [AlgorithmId] = 3)
BEGIN 
	
	 INSERT INTO [Admin].[Algorithm]
           (
			[AlgorithmId]
		   ,[ModelName]
           ,[PeriodsToForecast]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[LastModifiedBy]
           ,[LastModifiedDate]
		   )
	 SELECT  3				AS	[AlgorithmId]
		    ,'XGBoost'		AS	[ModelName]
            ,1				AS	[PeriodsToForecast]
		    ,N'System' 		AS	[CreatedBy]
		    ,@CurrentTime	AS	[CreatedDate]
            ,NULL			AS	[LastModifiedBy]
            ,NULL			AS	[LastModifiedDate]
	
END
ELSE
IF EXISTS (SELECT 1 FROM [Admin].[Algorithm]  WHERE [AlgorithmId] = 3)
BEGIN 
	UPDATE [Admin].[Algorithm]  
	SET  [ModelName]		= 	'XGBoost'
		,[LastModifiedBy]	=   'System' 
		,[LastModifiedDate] =   @CurrentTime
	WHERE [AlgorithmId] = 3
END
SET IDENTITY_INSERT [Admin].[Algorithm]  OFF



