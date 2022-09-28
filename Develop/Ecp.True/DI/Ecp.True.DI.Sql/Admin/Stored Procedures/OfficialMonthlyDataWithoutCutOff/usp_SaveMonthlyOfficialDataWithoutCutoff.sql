/*-- ============================================================================================================================
-- Author:          Microsoft 
-- Create date:  Jul-10-2020 
-- Updated date: Jul-13-2020  -- Converted pivot column values to spanish to server data in power bi report 
-- Updated date: Jul-13-2020  -- Updated Inv.ScenarioId = 2 
-- Updated date: Aug-10-2020  -- Using Origin a column as System instead systemName
-- Updated date: Aug-13-2020  -- removing unnecessary column from group by
-- Updated date: Aug-14-2020  -- changed system name column values back to system as per PBI 3540
-- Updated date: Aug-25-2020  -- changed Initial Inventory Calculation
-- Updated date: Sep-11-2020  -- Eliminating the records where product is NULL
-- Updated date: Oct-01-2020  -- To make consistent with other SP's changing @TodaysDate datatype to DATE
-- Updated date: Ago-08-2020  -- To add missing transfer to the Official Monthly Balance

,@NodeId        = 37258 
,@StartDate     = '2020-08-08' 
,@EndDate       = '2020-08-08' 
,@ExecutionId   = '79' 
SELECT * FROM [Admin].[OfficialMonthlyBalance] WHERE Executionid = '79' 
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveMonthlyOfficialDataWithoutCutOff] 
(
 @ElementId             INT
,@NodeId                INT
,@StartDate             DATE                      
,@EndDate               DATE                      
,@ExecutionId           INT
)
AS
BEGIN
    DECLARE @Previousdate   DATETIME =  [Admin].[udf_GetTrueDate] ()-1,
			@Todaysdate	    DATE     =  [Admin].[udf_GetTrueDate] (),
            @NodeName       NVARCHAR (50)


		IF OBJECT_ID('tempdb..#OfficialBalance')IS NOT NULL
		DROP TABLE #OfficialBalance

		IF OBJECT_ID('tempdb..#BalanceOfficialTemp')IS NOT NULL
		DROP TABLE #BalanceOfficialTemp
		
		IF OBJECT_ID('tempdb..#OfficialMonthlyBalance')IS NOT NULL
		DROP TABLE #OfficialMonthlyBalance

		CREATE TABLE #OfficialBalance (
		                               [MeasurementUnit]               VARCHAR  (300)                      NULL,
                                       [Product]                       NVARCHAR (150)                      NULL,
                                       [SystemName]                    NVARCHAR (150)                      NULL,
                                       [Version]                       NVARCHAR (150)                      NULL,
                                       [Owner]                         NVARCHAR (150)                      NULL,
									   [InitialInventory]              DECIMAL (18,2)                      NULL,
									   [FinalInventory]                DECIMAL (18,2)                      NULL,
									   [Inputs]                        DECIMAL (18,2)                      NULL,
									   [Outputs]                       DECIMAL (18,2)                      NULL
									   )

        SELECT TOP(1)
            @NodeName = Name
        FROM [Admin].[Node]
        WHERE NodeId = @NodeId

      -- MOVEMENT AND INVENTORY INFORMATION INTO A TEMP TABLE

      DROP TABLE IF EXISTS #BalanceOfficialTemp;

        --- MOVEMENT AND INVENTORY INFORMATION INTO A TEMP TABLE
        SELECT
            [MeasurementUnit],
            [Product],
            [System],
            [Version],
            [Owner],
            [OwnershipVolume],
            [Movement],
            TransactionId,
            RegistrationDate,
            SourceNode,
            DestinationNode,
            SourceProduct,
            DestinationProduct
        INTO #BalanceOfficialTemp
        FROM (
                                        SELECT
                    MeasurementUnit,
                    Product,
                    [System],
                    [Version],
                    [Owner],
                    'Inventory' AS Movement,
                    SUM([OwnershipVolume]) AS [OwnershipVolume],
                    InventoryProductId AS TransactionId,
                    RegistrationDate,

                    NULL AS SourceNode,
                    NULL AS DestinationNode,
                    NULL AS SourceProduct,
                    NULL AS DestinationProduct
                FROM [Admin].[OfficialMonthlyInventoryDetails]
                WHERE ExecutionId = @ExecutionId
                    AND RegistrationDate IN (@StartDate,@EndDate)
                    AND NodeName        = @NodeName
                GROUP BY 
                                    [MeasurementUnit],
                                    Product,
                                    [System],
                                    [Version],
                                    [Owner],
                                    InventoryProductId,
                                    RegistrationDate
            UNION
                SELECT
                    MeasurementUnit,
                    SourceProduct,
                    [System],
                    [Version],
                    [Owner],
                    [Movement],
                    [OwnershipVolume],
                    MovementTransactionId AS TransactionId,
                    RegistrationDate,

                    SourceNode,
                    DestinationNode,
                    SourceProduct,
                    DestinationProduct
                FROM [Admin].[OfficialMonthlyMovementDetails]
                WHERE ExecutionId     = @ExecutionId
                    AND SourceNode      = @NodeName
            UNION
                SELECT
                    MeasurementUnit,
                    DestinationProduct,
                    [System],
                    [Version],
                    [Owner],
                    [Movement],
                    [OwnershipVolume],
                    MovementTransactionId AS TransactionId,
                    RegistrationDate,

                    SourceNode,
                    DestinationNode,
                    SourceProduct,
                    DestinationProduct
                FROM [Admin].[OfficialMonthlyMovementDetails]
                WHERE ExecutionId     = @ExecutionId
                    AND DestinationNode = @NodeName
                        ) SubQ

        -- INSERTING ONLY UNIQUE STRING DATA INTO TEMP TABLE WHICH CAN BE LOADED LATER INTO MAIN TABLE
           INSERT INTO #OfficialBalance ([MeasurementUnit],[Product],[SystemName],[Version],[Owner])
                SELECT DISTINCT [MeasurementUnit],[Product],[System],[Version],[Owner] FROM #BalanceOfficialTemp

        -- Getting input movements into temp table

        DROP TABLE IF EXISTS #MovementInputs;
        SELECT DISTINCT
            B.[MeasurementUnit]
		                   , B.[Product]
		                   , B.[System]
		                   , B.[Version]
		                   , B.[Owner]
		                   , SUM(B.OwnershipVolume) AS Inputs
        INTO #MovementInputs
        FROM #BalanceOfficialTemp B
        WHERE B.Movement        = 'Entradas'
        GROUP BY  B.[MeasurementUnit]
		                   ,B.[Product]
		                   ,B.[System]
		                   ,B.[Version]
		                   ,B.[Owner]

        -- Getting transformation inputs into temp table

        DROP TABLE IF EXISTS #TransformationInputs;
        SELECT DISTINCT
            B.[MeasurementUnit]
                    , B.[Product]
                    , B.[System]
                    , B.[Version]
                    , B.[Owner]
                    , SUM(B.OwnershipVolume) AS Inputs
        INTO #TransformationInputs
        FROM #BalanceOfficialTemp B
        WHERE B.Movement        = 'Movimiento'
            AND (
                    B.SourceNode = B.DestinationNode
            AND
            B.SourceProduct <> B.DestinationProduct
            AND
            B.DestinationProduct = B.Product
                )
        GROUP BY  B.[MeasurementUnit]
		                   ,B.[Product]
		                   ,B.[System]
		                   ,B.[Version]
		                   ,B.[Owner]

        -- Getting final inputs from movement and transformation inputs

        DROP TABLE IF EXISTS #Inputs;
        SELECT
            B.[MeasurementUnit]
                    , B.[Product]
                    , B.[System]
                    , B.[Version]
                    , B.[Owner]
                    , SUM(Inputs) AS Inputs
        INTO #Inputs
        FROM (
                                          SELECT DISTINCT *
                FROM #MovementInputs
            UNION
                SELECT DISTINCT *
                FROM #TransformationInputs
                ) B
        GROUP BY  B.[MeasurementUnit]
		                   ,B.[Product]
		                   ,B.[System]
		                   ,B.[Version]
		                   ,B.[Owner]
		                   ,B.[Owner]

        -- Getting movement outputs into temp table

        DROP TABLE IF EXISTS #MovementOutputs;
        SELECT DISTINCT
            [MeasurementUnit]
		           , [Product]
		           , [System]
		           , [Version]
		           , [Owner]
		           , SUM(B.OwnershipVolume)  AS Outputs
        INTO #MovementOutputs
        FROM #BalanceOfficialTemp B
        WHERE Movement = 'Salidas'
        GROUP BY [MeasurementUnit]
		           ,[Product]
		           ,[System]
		           ,[Version]
		           ,[Owner]

        -- Getting transformation outputs into temp table

        DROP TABLE IF EXISTS #TransformationOutputs;
        SELECT DISTINCT
            B.[MeasurementUnit]
            , B.[Product]
            , B.[System]
            , B.[Version]
            , B.[Owner]
            , SUM(B.OwnershipVolume) AS Outputs
        INTO #TransformationOutputs
        FROM #BalanceOfficialTemp B
        WHERE B.Movement        = 'Movimiento'
            AND (
            B.SourceNode = B.DestinationNode
            AND
            B.SourceProduct <> B.DestinationProduct
            AND
            B.SourceProduct = B.Product
        )
        GROUP BY  B.[MeasurementUnit]
		           ,B.[Product]
		           ,B.[System]
		           ,B.[Version]
		           ,B.[Owner]

        -- Getting final outputs from 

        DROP TABLE IF EXISTS #Outputs;
        SELECT
            B.[MeasurementUnit]
            , B.[Product]
            , B.[System]
            , B.[Version]
            , B.[Owner]
            , SUM(Outputs) AS Outputs
        INTO #Outputs
        FROM (
                        SELECT DISTINCT *
                FROM #MovementOutputs
            UNION
                SELECT DISTINCT *
                FROM #TransformationOutputs
        ) B
        GROUP BY  B.[MeasurementUnit]
		           ,B.[Product]
		           ,B.[System]
		           ,B.[Version]
		           ,B.[Owner]
	
        -- UPDATE INITIAL INVENTORY
			UPDATE Dest 
			   SET InitialInventory = (OwnershipVolume)
			  FROM #OfficialBalance Dest
        INNER JOIN  #BalanceOfficialTemp Src
                ON Src.MeasurementUnit = Dest.MeasurementUnit
        	   AND Src.Product         = Dest.Product
        	   AND Src.[System]        = Dest.SystemName
        	   AND Src.[Version]       = Dest.[Version]
        	   AND Src.[Owner]         = Dest.[Owner]
			   AND Src.[Movement]      = 'Inventory'
		WHERE Src.RegistrationDate = @StartDate
		 
        -- UPDATE FINAL INVENTORY
			UPDATE Dest 
			   SET FinalInventory = (OwnershipVolume)
			  FROM #OfficialBalance Dest
        INNER JOIN  #BalanceOfficialTemp Src
                ON Src.MeasurementUnit = Dest.MeasurementUnit
        	   AND Src.Product         = Dest.Product
        	   AND Src.[System]        = Dest.SystemName
        	   AND Src.[Version]       = Dest.[Version]
        	   AND Src.[Owner]         = Dest.[Owner]
			   AND Src.[Movement]      = 'Inventory'
		WHERE Src.RegistrationDate = @EndDate
		  
         -- UPDATE INPUTS
			UPDATE Dest 
			   SET Inputs = Src.Inputs
	        FROM #OfficialBalance Dest
          INNER JOIN  #Inputs Src
                  ON Src.MeasurementUnit = Dest.MeasurementUnit
          	   AND Src.Product         = Dest.Product
          	   AND Src.[System]        = Dest.SystemName
          	   AND Src.[Version]       = Dest.[Version]
          	   AND Src.[Owner]         = Dest.[Owner]

          --UPDATE OUTPUTS
			UPDATE Dest 
			   SET Outputs = Src.Outputs
			  FROM #OfficialBalance Dest
        INNER JOIN  #Outputs Src
                ON Src.MeasurementUnit = Dest.MeasurementUnit
        	   AND Src.Product         = Dest.Product
        	   AND Src.[System]        = Dest.SystemName
        	   AND Src.[Version]       = Dest.[Version]
        	   AND Src.[Owner]         = Dest.[Owner]

		  
-- INSERTING DATA FROM TEMP TO MAIN TABLE

 SELECT MeasurementUnit,
        Product,
        SystemName,
        [Version],
        [Owner],
        ISNULL(InitialInventory,'0.0') AS [InitialInventory],
        ISNULL(FinalInventory,'0.0')   AS [FinalInventory],
        ISNULL(Inputs,'0.0')           AS [Input],
        ISNULL(Outputs,'0.0')          AS [Output],
        CAST( ISNULL(InitialInventory,0.00)
             +ISNULL(Inputs,0.00)
             -ISNULL(Outputs,0.00)
             -ISNULL(FinalInventory,0.00) AS DECIMAL(18,2)) 
                               AS [Control],
		@ExecutionId           AS ExecutionId,
		'ReportUser'           AS CreatedBy,
		@Todaysdate            AS CreatedDate
  INTO #OfficialMonthlyBalance
  FROM #OfficialBalance

  DELETE FROM #OfficialMonthlyBalance
  WHERE [Input]             = '0.00'
	AND [Output]            = '0.00'
	AND [InitialInventory]  = '0.00'
	AND [FinalInventory] 	= '0.00'

INSERT INTO [Admin].[OfficialMonthlyBalance] 
							            ( 
                                          [MeasurementUnit]           
                                         ,[Product]                   
                                         ,[SystemName]                
                                         ,[Version]                   
                                         ,[Owner]                     
	                                     ,[Input]                     
	                                     ,[Output]                    
	                                     ,[InitialInventory]          
	                                     ,[FinalInventory]            
	                                     ,[Control]                   
                                         ,[ExecutionId]				 
                                         ,[CreatedBy]
                                         ,[CreatedDate]
										 )
                              SELECT [MeasurementUnit]           
                                    ,[Product]                   
                                    ,[SystemName]                
                                    ,[Version]                   
                                    ,[Owner]                     
	                                ,[Input]                     
	                                ,[Output]                    
	                                ,[InitialInventory]          
	                                ,[FinalInventory]            
	                                ,[Control]                   
                                    ,[ExecutionId]				 
                                    ,[CreatedBy]
                                    ,[CreatedDate]
                               FROM #OfficialMonthlyBalance
 END
 GO
 EXEC sp_addextendedproperty @name = N'MS_Description',
     @value = N'This Procedure is to feed the OfficialMonthlyBalance table',
     @level0type = N'SCHEMA',
     @level0name = N'Admin',
     @level1type = N'PROCEDURE',
     @level1name = N'usp_SaveMonthlyOfficialDataWithoutCutOff',
     @level2type = NULL,
     @level2name = NULL
