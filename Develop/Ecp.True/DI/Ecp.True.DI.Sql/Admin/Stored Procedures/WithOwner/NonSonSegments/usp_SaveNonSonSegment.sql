/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-06-2020
-- Update date:     Sep-18-2020  Modified netstandardvolume to OwnershipVolume
-- Update date:     Oct-05-2020  Changed code to update the data into correct columns
-- Update date: 	Oct-06-2020  Multiplying * -1 for interface, tolerance depending on destination, source node.
-- Update date: 	Oct-09-2020  Modified logic to calculate interface, tolerance as per bug 83220
-- Description:     This Procedure is to Get TimeOne Data for Non SonsSegment , Element, Node, StartDate, EndDate.
-- EXEC [Admin].[usp_SaveNonSonSegment] 183411,43843,'2020-07-27','2020-08-01','4C1B89FC-B66D-4E1E-B1AA-FD4A6D6108C5'
   SELECT * FROM [Admin].[OperationalNonSon] 
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveNonSonSegment]
(
	   
	     @ElementId		INT 
		,@NodeId	    INT
		,@StartDate		DATE		  
		,@EndDate		DATE		  
		,@ExecutionId	INT 
)
AS
BEGIN
  SET NOCOUNT ON

	        DECLARE  @Previousdate			 DATETIME =  [Admin].[udf_GetTrueDate] ()-1,
					 @Todaysdate		     DATETIME =  [Admin].[udf_GetTrueDate] ()
					,@NoOfDays			     INT
					,@PreviousDayOfStartDate DATE
					,@IdentifiedLosses       VARCHAR (100)
			        ,@NodeName				 NVARCHAR(150);

			SELECT @NodeName = [Name]
			From [Admin].[Node]
			WHERE NodeId = @NodeId;
		     
		    SET @IdentifiedLosses = (SELECT DISTINCT CASE WHEN Classification = 'PerdidaIdentificada'
			                                              THEN 'Pérdidas Identificadas'
														  ELSE Classification
													 END
				                            FROM [Offchain].[Movement] Mov
											INNER JOIN Admin.CategoryElement CE
											ON Mov.SourceSystemId = CE.ElementId 
										   WHERE MessageTypeId = 2
										     AND ISNULL(CE.Name,'') != 'TRUE'
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

            DROP TABLE IF EXISTS #TempSecond;

         -- CREATE A TEMP TABLE WHICH CAN BE USED AS LAST TABLE BEFORE ACTUAL DESTINATION TABLE
			CREATE TABLE #MainTemp (
			                        ProductID			 NVARCHAR (20)	   NULL,	
								    ProductName          NVARCHAR (150)	   NULL,
									OwnerName			 NVARCHAR (150)	   NULL,
									MeasurementUnit		 VARCHAR (300)	   NULL,
								    InitialInventory     DECIMAL (18, 2)   NULL,
								    FinalInventory       DECIMAL (18, 2)   NULL,
								    IdentifiedLosses     DECIMAL (18, 2)   NULL,
								    Inputs               DECIMAL (18, 2)   NULL,
								    OutPuts              DECIMAL (18, 2)   NULL,
									Interface			 DECIMAL (18, 2)   NULL,
									Tolerance			 DECIMAL (18, 2)   NULL,
								    CalculationDate      DATE              NULL,
									TransactionId        INT               NULL
			                        )

         -- MOVEMENT AND INVENTORY INFORMATION INTO A TEMP TABLE 
            SELECT  TransactionId,
			        ProductId,
					Movement,
					OwnerName,
					MeasurementUnit,
					Product,
					NetStandardVolume,
					CAST(CalculationDate AS DATE) AS CalculationDate 
			  INTO #TempIntial 
              FROM (
					SELECT M.MovementTransactionId AS TransactionId,M.ProductID,iif(M.TypeMovement like '%CONVERSION DE PRODUCTOS%','Salidas',M.Movement) Movement
					,M.OwnerName,M.MeasurementUnit,M.SourceProduct AS Product,M.OwnershipVolume AS NetStandardVolume,M.CalculationDate 
					FROM [Admin].[OperationalMovementOwnerNonSon](NOLOCK) M
					INNER JOIN [Admin].[Product](NOLOCK) P
					ON P.ProductId = M.ProductID 
					AND P.[Name] = M.SourceProduct
                    WHERE  ExecutionId = @ExecutionId
					AND M.SourceNode = ISNULL(@NodeName,M.SourceNode)

					UNION
		       
                    SELECT M.MovementTransactionId AS TransactionId,M.ProductId,iif(M.TypeMovement like '%CONVERSION DE PRODUCTOS%','Entradas',M.Movement) Movement
					,M.OwnerName,M.MeasurementUnit,M.DestinationProduct AS Product,M.OwnershipVolume AS NetStandardVolume,M.CalculationDate 
					FROM [Admin].[OperationalMovementOwnerNonSon](NOLOCK) M
					INNER JOIN [Admin].[Product](NOLOCK) P
					ON P.ProductId = M.ProductID 
					AND P.[Name] = M.DestinationProduct
                    WHERE  ExecutionId = @ExecutionId
					AND M.DestinationNode = ISNULL(@NodeName,M.DestinationNode)

                    UNION
		                 
		   	        SELECT InventoryProductId AS TransactionId,ProductId,Movement,OwnerName,MeasurementUnit,Product,SUM(NetStandardVolume) AS NetStandardVolume,CalculationDate
		   	         FROM (
                           SELECT InventoryProductId,ProductId,'Inventory' AS Movement,OwnerName,MeasurementUnit,Product,OwnershipVolume AS NetStandardVolume,@PreviousDayOfStartDate AS CalculationDate 
						   FROM [Admin].[OperationalInventoryOwnerNonSon](NOLOCK) 
                           WHERE ExecutionId      = @ExecutionId
		   				      AND CAST(CalculationDate AS DATE) IN (@PreviousDayOfStartDate)
		   				    )SQ
                   GROUP BY InventoryProductId,ProductId,Movement,OwnerName,MeasurementUnit,Product,CalculationDate

				   UNION
				                      		                
		   	       SELECT InventoryProductId AS TransactionId,ProductId,Movement,OwnerName,MeasurementUnit,Product,SUM(NetStandardVolume) AS NetStandardVolume,CalculationDate
		   	         FROM (
                           SELECT InventoryProductId,ProductId,'Inventory' AS Movement,OwnerName,MeasurementUnit,Product,OwnershipVolume AS NetStandardVolume,@EndDate AS CalculationDate 
						   FROM [Admin].[OperationalInventoryOwnerNonSon](NOLOCK) 
                           WHERE  ExecutionId      = @ExecutionId
		   				      AND CAST(CalculationDate AS DATE) IN (@EndDate)
		   				    )SQ
                   GROUP BY InventoryProductId,ProductId,Movement,OwnerName,MeasurementUnit,Product,CalculationDate
		    	   ) SubQ

              -- MOVEMENT AND INVENTORY INFORMATION INTO A TEMP TABLE FOR TOLERANCE AND INTERFACE
                 SELECT     MovementTransactionId AS TransactionId
    				       ,ProductID
    					   ,Movement
    					   ,OwnerName
    					   ,MeasurementUnit
    					   ,SourceProduct
    					   ,DestinationProduct
    					   ,CASE WHEN SourceProductId = ProductID
    						     THEN OwnershipVolume * -1
								 WHEN DestinationProductId = ProductId
    						     THEN OwnershipVolume 
    						     ELSE OwnershipVolume
    						END AS NetStandardVolume
							,Ownershipvolume
    					   ,CAST(CalculationDate AS DATE) AS CalculationDate
    					   ,ProductName
				    INTO #TempSecond
					FROM [Admin].[OperationalMovementOwnerNonSon](NOLOCK) 
                   WHERE  ExecutionId      = @ExecutionId
					 AND Movement IN ('Tolerancia','Interfases')
					 AND ISNULL(SourceProduct,'') != ISNULL(DestinationProduct,'')
					 AND ProductName IS NOT NULL

         -- INSERTING ONLY UNIQUE STRING DATA INTO TEMP TABLE WHICH CAN BE LOADED LATER INTO MAIN TABLE
            INSERT INTO #MainTemp (ProductId,ProductName,OwnerName,MeasurementUnit,CalculationDate,TransactionId)
            SELECT DISTINCT ProductId,Product,OwnerName,MeasurementUnit,CalculationDate,TransactionId FROM #TempIntial


/***************** START OF AGGREGATING INPUT, OUTPUT,INTERFACES,TOLERANCE, IDENTIFIED LOSSES ********************************/
           -- INPUT
		      SELECT Product
			        ,ProductID
					,Movement
					,CalculationDate
					,TransactionId
					,OwnerName
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
					  ,OwnerName
					  ,MeasurementUnit

           -- OUTPUT
		      SELECT Product
			        ,ProductID
					,Movement
					,CalculationDate
					,TransactionId
					,OwnerName
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
					  ,OwnerName
					  ,MeasurementUnit

           -- IDENTIFIED LOSSES
		      SELECT Product
			        ,ProductID
			        ,Movement
					,CalculationDate
					,TransactionId
					,OwnerName
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
					  ,OwnerName
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
			  AND Dest.OwnerName	   = Src.OwnerName
			  AND Dest.MeasurementUnit = Src.MeasurementUnit
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
			  AND Dest.OwnerName	   = Src.OwnerName
			  AND Dest.MeasurementUnit = Src.MeasurementUnit
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
			  AND Dest.OwnerName	   = Src.OwnerName
			  AND Dest.MeasurementUnit = Src.MeasurementUnit
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
			  AND Dest.OwnerName	   = Src.OwnerName
			  AND Dest.MeasurementUnit = Src.MeasurementUnit
           WHERE Movement = 'Salidas'
		     AND Dest.CalculationDate BETWEEN @StartDate AND @EndDate

		 -- UPDATE TOLERANCE
          UPDATE Dest 
             SET Tolerance = NetStandardVolume
            FROM #MainTemp Dest
            JOIN #TempSecond Src
              ON Dest.TransactionId   = Src.TransactionId
             AND Dest.ProductID       = Src.ProductID
             AND Dest.ProductName     = Src.ProductName
             AND Dest.OwnerName       = Src.OwnerName
             AND Dest.MeasurementUnit = Src.MeasurementUnit
			 AND Movement = 'Tolerancia'

         --UPDATE INTERFACE
          UPDATE Dest 
             SET Interface = NetStandardVolume
            FROM #MainTemp Dest
            JOIN #TempSecond Src
              ON Dest.TransactionId   = Src.TransactionId
             AND Dest.ProductID       = Src.ProductID
             AND Dest.ProductName     = Src.ProductName
             AND Dest.OwnerName       = Src.OwnerName
             AND Dest.MeasurementUnit = Src.MeasurementUnit
			 AND Movement = 'Interfases'

       -- UPDATE IDENTIFIED LOSSES
		  UPDATE Dest
		     SET Dest.IdentifiedLosses = Losses
			FROM #MainTemp Dest
			JOIN #Losses Src
			  ON Dest.ProductName      = Src.Product
			  AND Dest.ProductID       = Src.ProductID
			  AND Dest.CalculationDate = Src.CalculationDate
			  AND Dest.TransactionId   = Src.TransactionId
			  AND Dest.OwnerName	   = Src.OwnerName
			  AND Dest.MeasurementUnit = Src.MeasurementUnit
           WHERE Movement = @IdentifiedLosses
		     AND Dest.CalculationDate BETWEEN @StartDate AND @EndDate

/******** END OF UPDATING AGGREGATED INPUT, OUTPUT, IDENTIFIED LOSSES, INITIAL AND FINAL INVENTORY *****/

			-- DELETE THE PRODUCTS WHICH HAS NO VALUES
			DELETE #MainTemp
			WHERE InitialInventory   = 0
				AND Inputs           = 0
				AND Outputs          = 0
				AND IdentifiedLosses = 0
				AND FinalInventory   = 0
				AND Tolerance		 = 0
				AND Interface		 = 0

           INSERT INTO [Admin].[OperationalNonSon] ( 
                                              [ProductID]					
                                             ,[ProductName]	
											 ,[OwnerName]		
											 ,[MeasurementUnit]	
                                             ,[CalculationDate]			
                                             ,[Inputs]						
                                             ,[Outputs]					
                                             ,[IdentifiedLosses]			
                                             ,[IntialInventory]			
                                             ,[FinalInventory]	
											 ,[Interface]
											 ,[Tolerance]
                                             ,[Control]					                                     						
                                             ,[ExecutionId]				
                                             ,[CreatedBy]				
                                             ,[CreatedDate]				
								             )
                                     SELECT ProductId
									       ,ProductName
										   ,OwnerName
										   ,MeasurementUnit
										   ,CalculationDate
										   ,ISNULL(Inputs,'0.0')
										   ,ISNULL(Outputs,'0.0')
										   ,ISNULL(IdentifiedLosses,'0.0')
										   ,ISNULL(InitialInventory,'0.0')
										   ,ISNULL(FinalInventory,'0.0')
										   ,ISNULL(Interface,'0.0')
										   ,ISNULL(Tolerance,'0.0')
                                           ,( ISNULL(InitialInventory,'0.0')
	                                         +ISNULL(Inputs,'0.0')
	                                      	 -ISNULL(Outputs,'0.0')
	                                      	 -ISNULL(IdentifiedLosses,'0.0')
											 +ISNULL(Interface,'0.0')
											 +ISNULL(Tolerance,'0.0')
	                                      	 -ISNULL(FinalInventory,'0.0')
	                                      	)
	                                        AS [Control]
	                                       ,@ExecutionId
										   ,'ReportUser'
										   ,@Todaysdate
                                      FROM #MainTemp
									  WHERE ProductName IS NOT NULL 
									  AND ProductID IS NOT NULL
END


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get TimeOne Data for Non Sons Segment , Element, Node, StartDate, EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveNonSonSegment',
    @level2type = NULL,
    @level2name = NULL
