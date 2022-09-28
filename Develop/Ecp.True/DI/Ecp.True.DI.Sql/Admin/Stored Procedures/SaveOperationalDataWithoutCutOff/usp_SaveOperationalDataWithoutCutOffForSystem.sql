/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jan-28-2020
-- Description:     This Procedure is to Get TimeOne Data for System Category, Element, Node, StartDate, EndDate.
-- Updated Date:	Apr-08-2020 -- Replaced Inventory, InventoryProduct with generic view_InventoryInformation
--				    Apr-09-2020  -- Removed(BlockchainStatus = 1)   
--				    Apr-23-2020  -- Removed Distinct to get all Inventories and Movements
--              	May-05-2020  -- Changed Join criteria and added a temp table
--              	May-07-2020  -- Made a fix related to single node calculation (Input & Output)
--				 	May-12-2020  -- Splitted deleting the data from target table condition to improve the performance
-- Updated date     Jun-24-2020  -- Updated scenarioid = 1 for movement and inventory as part of #PBI31874
-- Updated date     Jun-29-2020  -- Modified MessageTypeId condition while calculating INPUT/ OUTPUT values.
-- Updated date:    Jul-01-2020  -- Added MovementTransactionId column to generate distinct values of product by transaction
-- Updated date     Jul-02-2020  -- Updated parallel execution logic
-- Updated date:    Jul-13-2020  -- Adding One more condition to get all inventory details when user passed one date where it has different products for a single node
                                    And in the end before loading into Main table deleting the rows which has 0 values.
-- Updated date:	Jul-17-2020	 -- Added temp table To Improve Performance
-- Updated date:    Aug-04-2020  -- Changed input paramters from STRING to INTEGER AND removed delete old data
-- Updated date:    Aug-07-2020  -- Removing unrequired columns and conditions
-- Updated date:    Sep-01-2020  -- Removing UnNecessary Castings
-- Updated date:    Sep-03-2020  -- Shifted Delete Operation to Temp Table
-- Updated date:	Jun-06-2021  -- Se Permitio que la conversiones de producto se mostrara de manera correcta en el balance.
-- Updated date:	Jun-11-2021	 -- Se Agrego campo de unidad para ser visualizada en el reporte.
-- Updated date:	Sep-28-2021	 -- Se Agrega Validacion que permite visualizar las conversiones y traslados de producto en el balance del reporte.
-- EXEC  [Admin].[usp_SaveOperationalDataWithoutCutOffForSystem]'Sistema','Automation_vepja_System','Automation_2tvc6','2020-03-30','2020-04-01','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'
-- EXEC  [Admin].[usp_SaveOperationalDataWithoutCutOffForSystem]'Sistema','Automation_vepja_System','Todos','2020-03-30','2020-04-01','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'
   SELECT * FROM [Admin].[Operational] WHERE InputCategory = 'Sistema' AND ExecutionId = '738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveOperationalDataWithoutCutOffForSystem]
(
        @CategoryId                 INT 
       ,@ElementId                  INT 
       ,@NodeId                     INT 
       ,@StartDate                  DATE                      
       ,@EndDate                    DATE                      
       ,@ExecutionId                INT
)
AS
BEGIN
  SET NOCOUNT ON

         -- Variables Declaration
	        DECLARE @Previousdate  DATETIME =  [Admin].[udf_GetTrueDate] ()-1,
					@Todaysdate	   DATETIME =  [Admin].[udf_GetTrueDate] (),
                    @PreviousDayOfStartDate DATE,
                    @IdentifiedLosses       VARCHAR (100),
					@NodeName	   NVARCHAR(150);

			SELECT @NodeName = [Name]
			From [Admin].[Node]
			WHERE NodeId = @NodeId;
			     
				 SET @IdentifiedLosses = (SELECT DISTINCT Classification 
				                            FROM [Offchain].[Movement] Mov
											INNER JOIN [Admin].CategoryElement CE
											ON Mov.SourceSystemId = CE.ElementId 
										   WHERE MessageTypeId = 2
										     AND ISNULL(CE.[Name],'') != 'TRUE'
										 )

				 SET @PreviousdayOfStartDate = DATEADD(DAY,-1,@StartDate)


			IF OBJECT_ID('tempdb..#TempIntial')IS NOT NULL
			DROP TABLE #TempIntial

			IF OBJECT_ID('tempdb..#MainTemp')IS NOT NULL
			DROP TABLE #MainTemp

			IF OBJECT_ID('tempdb..#Inputs')IS NOT NULL
			DROP TABLE #Inputs

			IF OBJECT_ID('tempdb..#Outputs')IS NOT NULL
			DROP TABLE #Outputs

			IF OBJECT_ID('tempdb..#Losses')IS NOT NULL
			DROP TABLE #Losses

         -- CREATE A TEMP TABLE WHICH CAN BE USED AS LAST TABLE BEFORE ACTUAL DESTINATION TABLE
			CREATE TABLE #MainTemp (
			                        ProductID			 NVARCHAR (20)	   NULL,	
								    ProductName          NVARCHAR (150)	   NULL,
								    InitialInventory     DECIMAL (18, 2)   NULL,
								    FinalInventory       DECIMAL (18, 2)   NULL,
								    IdentifiedLosses     DECIMAL (18, 2)   NULL,
								    Inputs               DECIMAL (18, 2)   NULL,
								    OutPuts              DECIMAL (18, 2)   NULL,
								    CalculationDate      DATE              NULL,
									TransactionId        INT               NULL,
									MeasurementUnit		 NVARCHAR(20)      NULL
			                        )

         -- MOVEMENT AND INVENTORY INFORMATION INTO A TEMP TABLE
            SELECT  TransactionId,ProductId,Movement,Product,NetStandardVolume,CalculationDate AS CalculationDate,MeasurementUnit  INTO #TempIntial 
              FROM (
					SELECT M.MovementTransactionId AS TransactionId,M.ProductId,case when [Admin].[udf_NodeIsProductConversion](@CategoryId,M.SourceNode,M.DestinationNode,@StartDate,@EndDate,@ExecutionId) = 3 and M.SourceProduct != M.DestinationProduct then 'Salidas' else M.Movement end Movement
					,M.SourceProduct AS Product,M.NetStandardVolume,M.CalculationDate ,M.MeasurementUnit
					FROM [Admin].[OperationalMovement](NOLOCK) M
					INNER JOIN [Admin].[Product](NOLOCK) P
					ON P.ProductId = M.ProductID 
					AND P.[Name] = M.SourceProduct 
                    WHERE M.ExecutionId = @ExecutionId
                    AND M.SourceNode = ISNULL(@NodeName,M.SourceNode) and [Admin].[udf_NodeIsProductConversion](@CategoryId,M.SourceNode,M.DestinationNode,@StartDate,@EndDate,@ExecutionId) in (1,3)
                    
		   	        UNION
		       
	 				SELECT M.MovementTransactionId AS TransactionId,M.ProductId,case when [Admin].[udf_NodeIsProductConversion](@CategoryId,M.SourceNode,M.DestinationNode,@StartDate,@EndDate,@ExecutionId) = 3 and M.SourceProduct != M.DestinationProduct then 'Entradas' else M.Movement end Movement
					   ,M.DestinationProduct AS Product,M.NetStandardVolume,M.CalculationDate,M.MeasurementUnit 
						FROM [Admin].[OperationalMovement](NOLOCK) M
						INNER JOIN [Admin].[Product](NOLOCK) P
						ON P.ProductId = M.ProductID 
						AND P.[Name] = M.DestinationProduct 
						WHERE ExecutionId = @ExecutionId
						AND M.DestinationNode = ISNULL(@NodeName,M.DestinationNode) and [Admin].[udf_NodeIsProductConversion](@CategoryId,M.SourceNode,M.DestinationNode,@StartDate,@EndDate,@ExecutionId) in (2,3)
                    
                    UNION
		                 
		   	        SELECT InventoryProductId AS TransactionId,ProductId,Movement,Product,SUM(NetStandardVolume) AS NetStandardVolume,CalculationDate,MeasurementUnit
		   	         FROM (
                           SELECT InventoryProductId,ProductId,'Inventory' AS Movement,Product,NetStandardVolume,@PreviousDayOfStartDate AS CalculationDate,MeasurementUnit
						   FROM [Admin].[OperationalInventory](NOLOCK) 
                           WHERE ExecutionId      = @ExecutionId
		   				   AND CalculationDate    = @PreviousDayOfStartDate
		   				    )SQ
                   GROUP BY InventoryProductId,ProductId,Movement,Product,CalculationDate,MeasurementUnit

				   UNION
				                      		                
		   	       SELECT InventoryProductId AS TransactionId,ProductId,Movement,Product,SUM(NetStandardVolume) AS NetStandardVolume,CalculationDate,MeasurementUnit
		   	         FROM (
                           SELECT InventoryProductId,ProductId,'Inventory' AS Movement,Product,NetStandardVolume,@EndDate AS CalculationDate,MeasurementUnit 
						   FROM [Admin].[OperationalInventory](NOLOCK) 
                           WHERE ExecutionId      = @ExecutionId
		   				   AND CalculationDate    = @EndDate
		   			  )SQ
                   GROUP BY InventoryProductId,ProductId,Movement,Product,CalculationDate,MeasurementUnit
		    	   ) SubQ

         -- INSERTING ONLY UNIQUE STRING DATA INTO TEMP TABLE WHICH CAN BE LOADED LATER INTO MAIN TABLE
            INSERT INTO #MainTemp (ProductId,ProductName,CalculationDate,TransactionId,MeasurementUnit)
            SELECT DISTINCT ProductId,Product,CalculationDate,TransactionId,MeasurementUnit FROM #TempIntial

/***************** START OF AGGREGATING INPUT, OUTPUT, IDENTIFIED LOSSES ********************************/
           -- INPUT
		      SELECT Product
			        ,ProductID
					,Movement
					,CalculationDate
					,TransactionId
					,MeasurementUnit
			        ,SUM(NetStandardVolume) AS Inputs
			    INTO #Inputs
				FROM #TempIntial
			   WHERE Movement = 'Entradas'
			     AND CalculationDate BETWEEN @StartDate AND @EndDate
              GROUP BY Product
			          ,ProductID
					  ,Movement
					  ,CalculationDate
					  ,TransactionId
					  ,MeasurementUnit

           -- OUTPUT
		      SELECT Product
			        ,ProductID
					,Movement
					,CalculationDate
					,TransactionId
					,MeasurementUnit
			        ,SUM(NetStandardVolume) AS Outputs
			    INTO #Outputs
				FROM #TempIntial
			   WHERE Movement = 'Salidas'
			     AND CalculationDate BETWEEN @StartDate AND @EndDate
              GROUP BY Product
			          ,ProductID
					  ,Movement
					  ,CalculationDate
					  ,TransactionId
					  ,MeasurementUnit

           -- IDENTIFIED LOSSES
		      SELECT Product
			        ,ProductID
			        ,Movement
					,CalculationDate
					,TransactionId
					,MeasurementUnit
			        ,SUM(NetStandardVolume) AS Losses
			    INTO #Losses
				FROM #TempIntial
			   WHERE Movement = @IdentifiedLosses
			     AND CalculationDate BETWEEN @StartDate AND @EndDate
              GROUP BY Product
			          ,ProductID
			          ,Movement
					  ,CalculationDate
					  ,TransactionId
					  ,MeasurementUnit

/***************** END OF AGGREGATING INPUT, OUTPUT, IDENTIFIED LOSSES ***********************************/

/******** START OF UPDATING AGGREGATED INPUT, OUTPUT, IDENTIFIED LOSSES, INITIAL AND FINAL INVENTORY *****/
       -- UPDATE INTIAL INVENTORY
		  UPDATE Dest
		     SET Dest.InitialInventory = NetStandardVolume
			    ,Dest.CalculationDate  = @StartDate
			FROM #MainTemp Dest
			JOIN #TempIntial Src
			  ON  Dest.ProductName     = Src.Product
			  AND Dest.ProductID       = Src.ProductID
			  AND Dest.CalculationDate = Src.CalculationDate
			  AND Dest.TransactionId   = Src.TransactionId
           WHERE Movement = 'Inventory'
		     AND Dest.CalculationDate = (@PreviousDayOfStartDate)

       -- UPDATE FINAL INVENTORY
		  UPDATE Dest
		     SET FinalInventory = NetStandardVolume
			FROM #MainTemp Dest
			JOIN #TempIntial Src
			  ON Dest.ProductName      = Src.Product
			  AND Dest.ProductID       = Src.ProductID
			  AND Dest.CalculationDate = Src.CalculationDate
			  AND Dest.TransactionId   = Src.TransactionId
           WHERE Movement = 'Inventory'
		     AND Dest.CalculationDate = @EndDate

       -- UPDATE INPUTS
		  UPDATE Dest
		     SET Dest.Inputs = Src.Inputs
			FROM #MainTemp Dest
			JOIN #Inputs Src
			  ON Dest.ProductName      = Src.Product
			  AND Dest.ProductID       = Src.ProductID
			  AND Dest.CalculationDate = Src.CalculationDate
			  AND Dest.TransactionId   = Src.TransactionId
           WHERE Movement = 'Entradas'
		     AND Dest.CalculationDate BETWEEN @StartDate AND @EndDate

       -- UPDATE OUTPUTS
		  UPDATE Dest
		     SET Dest.OutPuts = Src.Outputs
			FROM #MainTemp Dest
			JOIN #Outputs Src
			  ON Dest.ProductName      = Src.Product
			  AND Dest.ProductID       = Src.ProductID
			  AND Dest.CalculationDate = Src.CalculationDate
			  AND Dest.TransactionId   = Src.TransactionId
           WHERE Movement = 'Salidas'
		     AND Dest.CalculationDate BETWEEN @StartDate AND @EndDate

       -- UPDATE IDENTIFIED LOSSES
		  UPDATE Dest
		     SET Dest.IdentifiedLosses = Losses
			FROM #MainTemp Dest
			JOIN #Losses Src
			  ON Dest.ProductName      = Src.Product
			  AND Dest.ProductID       = Src.ProductID
			  AND Dest.CalculationDate = Src.CalculationDate
			  AND Dest.TransactionId   = Src.TransactionId
           WHERE Movement = @IdentifiedLosses
		     AND Dest.CalculationDate BETWEEN @StartDate AND @EndDate

			 -- DELETE THE PRODUCTS WHICH HAS NO VALUES
            DELETE #MainTemp ---Shift to temp Table
	        WHERE InitialInventory  = 0
	        AND Inputs           = 0
	        AND Outputs          = 0
	        AND IdentifiedLosses = 0
            AND FinalInventory   = 0

/******** END OF UPDATING AGGREGATED INPUT, OUTPUT, IDENTIFIED LOSSES, INITIAL AND FINAL INVENTORY *****/

           INSERT INTO [Admin].[Operational] ( 
                                              ProductID					
                                             ,ProductName				
                                             ,CalculationDate
											 ,MeasurementUnit
                                             ,Inputs						
                                             ,Outputs					
                                             ,IdentifiedLosses			
                                             ,IntialInventory			
                                             ,FinalInventory				
                                             ,UnBalance					
                                             ,ExecutionId				
                                             ,[CreatedBy]				
                                             ,[CreatedDate]				
								             )
                                     SELECT ProductId
									       ,ProductName
										   ,CalculationDate
										   ,MeasurementUnit
										   ,ISNULL(Inputs,'0.0')
										   ,ISNULL(Outputs,'0.0')
										   ,ISNULL(IdentifiedLosses,'0.0')
										   ,ISNULL(InitialInventory,'0.0')
										   ,ISNULL(FinalInventory,'0.0')
                                           ,( ISNULL(InitialInventory,'0.0')
	                                         +ISNULL(Inputs,'0.0')
	                                      	 -ISNULL(Outputs,'0.0')
	                                      	 -ISNULL(IdentifiedLosses,'0.0')
	                                      	 -ISNULL(FinalInventory,'0.0')
	                                      	)
	                                        AS UnBalance
	                                       ,@ExecutionId
										   ,'ReportUser'
										   ,@Todaysdate
                                      FROM #MainTemp
									  WHERE ProductName IS NOT NULL 
									  AND ProductID IS NOT NULL
                 

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne Data for System, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveOperationalDataWithoutCutOffForSystem',
    @level2type = NULL,
    @level2name = NULL