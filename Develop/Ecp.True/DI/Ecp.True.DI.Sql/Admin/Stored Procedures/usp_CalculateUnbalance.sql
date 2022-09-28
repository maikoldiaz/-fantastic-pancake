/*-- ==============================================================================================================================
-- Author:			Microsoft  
-- Created Date:    Nov-11-2019
-- Updated Date:	Mar-20-2020
--				    Apr-09-2020  -- Removed(BlockchainStatus = 1) 
--					Aug-06-2020	 -- Removed Casting on m.measurementunit
-- <Description>:	This Procedure is used to calculate the Unbalance without ownership for a given NodeId, StartDate, EndDate and OutputTableType.  </Description>
-- ================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_CalculateUnbalance]
	@NodeId INT,
	@StartDate DATETIME,
	@EndDate DATETIME,
	@OutputTableType INT = 0		-- The six possible values are 0 = GetSummaryTable, 1 = GetInputs, 2 = GetOutputs, 3 = IdentifiedLoss, 4 = UnBalance Table, 5 = GetAllTables
AS
BEGIN
	BEGIN TRY

		-- Validation1 = Check if NodeId is not null.
		IF (@NodeId IS NULL)
			THROW 52001, 'INVENTORY_NODEID_REQUIREDVALIDATION' , 1

		-- Validation2 = Check if the StartDate or EndDate is not null.
		IF (@StartDate IS NULL)
			THROW 52002, 'STARTDATE_REQUIREDVALIDATION' , 2

		-- Validation3 = Check if the StartDate or EndDate is null.
		IF (@EndDate IS NULL)
			THROW 52003, 'ENDDATE_REQUIREDVALIDATION' , 3

		DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, Admin.udf_GetTrueDate()), 0));	
		SET @StartDate	= (select DATEADD(dd, DATEDIFF(dd, 0, @StartDate), 0));
		SET @EndDate	= (select DATEADD(dd, DATEDIFF(dd, 0, @EndDate), 0));

		-- Validation4 = Check if the StartDate is not greater than EndDate
		IF (@StartDate > @EndDate)
			THROW 52004, 'DATES_INCONSISTENT' , 4

		-- Validation5 = Check if the StartDate and EndDate is not today's date.
		IF (@EndDate >= @todaysDate)
			THROW 52005, 'ENDDATE_BEFORENOWVALIDATION' , 5



				-- Inputs
		DECLARE @tempInputs AS TABLE 
		  ( 
			 movementid         BIGINT, 
			 movementtypeid     NVARCHAR(150), 
			 operationaldate    DATETIME, 
			 sourcenode         NVARCHAR(30), 
			 destiationnode     NVARCHAR(50), 
			 sourceproduct      NVARCHAR(50), 
			 destinationproduct NVARCHAR(50), 
			 netstandardvolume  DECIMAL(29, 16), 
			 scenario           NVARCHAR(30), 
			 classification     NVARCHAR (30), 
			 measurementunit    NVARCHAR (50) 
		  ); 

			INSERT INTO @tempInputs 
			SELECT DISTINCT m.movementid, 
							m.movementtypeid, 
							m.operationaldate, 
							ms.sourcenode, 
							md.destinationnode, 
							ms.sourceproduct, 
							md.destinationproduct, 
							m.netstandardvolume, 
							st.name as Scenario, 
							mt.NAME, 
							ce.NAME 
			FROM   [Offchain].[movement] m 
				   JOIN [Admin].[view_movementsourcewithnodeandproductname] ms 
					 ON m.movementtransactionid = ms.movementtransactionid 
				   JOIN [Admin].[view_movementdestinationwithnodeandproductname] md 
					 ON m.movementtransactionid = md.movementtransactionid 
					 JOIN [Admin].[ScenarioType] st
					 ON st.ScenarioTypeId = m.ScenarioId
				   LEFT JOIN [Admin].[messagetype] mt 
						  ON mt.messagetypeid = m.messagetypeid 
				   INNER JOIN [Admin].[categoryelement] ce 
						   ON m.measurementunit = ce.elementid 
			WHERE  md.destinationnodeid = @NodeId 
				   AND mt.messagetypeid = 1 -- Taking only 'Movement' type 
				   AND @StartDate <= m.operationaldate 
				   AND m.operationaldate < (SELECT Dateadd(dd, Datediff(dd, 0, @EndDate), 1) 
										   ); 

				IF ( @OutputTableType = 1 
					  OR @OutputTableType = 5 ) 
				  SELECT * 
				  FROM   @tempInputs; 

				-- Outputs 
				DECLARE @tempOutputs AS TABLE 
				  ( 
					 movementid         BIGINT, 
					 movementtypeid     NVARCHAR(150), 
					 operationaldate    DATETIME, 
					 sourcenode         NVARCHAR(30), 
					 destiationnode     NVARCHAR(50), 
					 sourceproduct      NVARCHAR(50), 
					 destinationproduct NVARCHAR(50), 
					 netstandardvolume  DECIMAL(29, 16), 
					 scenario           NVARCHAR(30), 
					 classification     NVARCHAR (30), 
					 measurementunit    NVARCHAR (50) 
				  ); 

			INSERT INTO @tempOutputs 
			SELECT DISTINCT m.movementid, 
							m.movementtypeid, 
							m.operationaldate, 
							ms.sourcenode, 
							md.destinationnode, 
							ms.sourceproduct, 
							md.destinationproduct, 
							m.netstandardvolume, 
							st.name as Scenario, 
							mt.NAME, 
							ce.NAME 
			FROM   [Offchain].[movement] m 
				   JOIN [Admin].[view_movementsourcewithnodeandproductname] ms 
					 ON m.movementtransactionid = ms.movementtransactionid 
				   JOIN [Admin].[view_movementdestinationwithnodeandproductname] md 
					 ON m.movementtransactionid = md.movementtransactionid 
					 JOIN [Admin].[ScenarioType] st
					 ON st.ScenarioTypeId = m.ScenarioId
				   LEFT JOIN [Admin].[messagetype] mt 
						  ON mt.messagetypeid = m.messagetypeid 
				   INNER JOIN [Admin].[categoryelement] ce 
						   ON m.measurementunit = ce.elementid 
			WHERE  ms.sourcenodeid = @NodeId 
				   AND mt.messagetypeid = 1 -- Taking only 'Movement' type 
				   AND @StartDate <= m.operationaldate 
				   AND m.operationaldate < (SELECT Dateadd(dd, Datediff(dd, 0, @EndDate), 1) 
										   ); 

			IF ( @OutputTableType = 2 
				  OR @OutputTableType = 5 ) 
			  SELECT * 
			  FROM   @tempOutputs; 



			-- IdentifiedLoss 
			DECLARE @tempIdentifiedLoss AS TABLE 
			  ( 
				 movementid         BIGINT, 
				 movementtypeid     NVARCHAR(150), 
				 operationaldate    DATETIME, 
				 sourcenode         NVARCHAR(30), 
				 destiationnode     NVARCHAR(50), 
				 sourceproduct      NVARCHAR(50), 
				 destinationproduct NVARCHAR(50), 
				 netstandardvolume  DECIMAL(29, 16), 
				 scenario           NVARCHAR(30), 
				 classification     NVARCHAR (30), 
				 measurementunit    NVARCHAR (50) 
			  ); 

			INSERT INTO @tempIdentifiedLoss 
			SELECT DISTINCT m.movementid, 
							m.movementtypeid, 
							m.operationaldate, 
							ms.sourcenode, 
							NULL, 
							ms.sourceproduct, 
							NULL, 
							m.netstandardvolume, 
							st.name as Scenario, 
							mt.NAME, 
							ce.NAME 
			FROM   [Offchain].[movement] m 
				   JOIN [Admin].[view_movementsourcewithnodeandproductname] ms 
					 ON m.movementtransactionid = ms.movementtransactionid 
					 JOIN [Admin].[ScenarioType] st
					 ON st.ScenarioTypeId = m.ScenarioId
				   --JOIN [Admin].[view_MovementDestinationWithNodeAndProductName] md  ON m.MovementTransactionId = md.MovementTransactionId
				   LEFT JOIN [Admin].[messagetype] mt 
						  ON mt.messagetypeid = m.messagetypeid 
				   INNER JOIN [Admin].[categoryelement] ce 
						   ON m.measurementunit = ce.elementid 
			WHERE  ms.sourcenodeid = @NodeId 
				   AND mt.messagetypeid = 2 -- Taking only 'Loss' type 
				   AND @StartDate <= m.operationaldate 
				   AND m.operationaldate < (SELECT Dateadd(dd, Datediff(dd, 0, @EndDate), 1) 
										   ); 

			IF ( @OutputTableType = 3 
				  OR @OutputTableType = 5 ) 
			  SELECT * 
			  FROM   @tempIdentifiedLoss; 

			-- Getting Unique products from SourceProduct and DestinationProduct of Movements (Inputs + Outputs + Losses).
			DECLARE @tempUniqueProducts AS TABLE 
			  ( 
				 productname     NVARCHAR(30), 
				 measurementunit NVARCHAR (50) 
			  ); 

			INSERT INTO @tempUniqueProducts 
			SELECT DISTINCT destinationproduct, 
							measurementunit 
			FROM   @tempInputs 
			UNION 
			SELECT DISTINCT sourceproduct, 
							measurementunit 
			FROM   @tempOutputs 
			UNION 
			SELECT DISTINCT sourceproduct, 
							measurementunit 
			FROM   @tempIdentifiedLoss; 
		IF (@OutputTableType = 5)
			Select * from @tempUniqueProducts;

		
		-- Initial Inventory 
		DECLARE @tempInitalInventory AS TABLE 
		  ( 
			 productname      NVARCHAR(30), 
			 initialinventory DECIMAL(29, 16) 
		  ) 

		INSERT INTO @tempInitalInventory 
		SELECT ip.productname, 
			   Isnull(Sum(ip.ProductVolume), 0.0) AS InitialInventory 
		FROM   [Admin].[view_inventoryproductwithproductname] ip 
			   LEFT JOIN Admin.view_InventoryInformation inv
					  ON ip.InventoryProductId = inv.inventorytransactionid 
		WHERE  inv.nodeid = @NodeId 
			   AND inv.inventorydate >= (SELECT Dateadd(dd, Datediff(dd, 0, @StartDate),-1)) 
			   AND inv.inventorydate < (SELECT Dateadd(dd, Datediff(dd, 0, @StartDate), 0)) 
			   AND ip.productname IN (SELECT productname 
									  FROM   @tempUniqueProducts)
		GROUP  BY ip.productname; 

		IF ( @OutputTableType = 5 ) 
		  SELECT * 
		  FROM   @tempInitalInventory; 

		-- Final Inventory 
		DECLARE @tempFinalInventory AS TABLE 
		  ( 
			 productname    NVARCHAR(30), 
			 finalinventory DECIMAL(29, 16) 
		  ) 

		INSERT INTO @tempFinalInventory 
		SELECT ip.productname, 
			   Isnull(Sum(ip.ProductVolume), 0.0) AS FinalInventory 
		FROM   [Admin].[view_inventoryproductwithproductname] ip 
			   LEFT JOIN Admin.view_InventoryInformation inv 
					  ON ip.InventoryProductId = inv.inventorytransactionid 
		WHERE  inv.nodeid = @NodeId 
			   AND inv.inventorydate >= (SELECT Dateadd(dd, Datediff(dd, 0, @EndDate), 0)) 
			   AND inv.inventorydate < (SELECT Dateadd(dd, Datediff(dd, 0, @EndDate), 1)) 
			   AND ip.productname IN (SELECT productname 
									  FROM   @tempUniqueProducts)
		GROUP  BY ip.productname; 

		IF ( @OutputTableType = 5 ) 
		  SELECT * 
		  FROM   @tempFinalInventory; 

		-- Aggregated Inputs per product 
		DECLARE @tempAggregatedInputs AS TABLE 
		  ( 
			 destinationproduct NVARCHAR(30), 
			 inputs             DECIMAL(29, 16) 
		  ); 

		INSERT INTO @tempAggregatedInputs 
		SELECT destinationproduct                  AS Product, 
			   Isnull(Sum(netstandardvolume), 0.0) AS Inputs 
		FROM   @tempInputs 
		WHERE  destinationproduct IN (SELECT productname 
									  FROM   @tempUniqueProducts) 
		GROUP  BY destinationproduct; 

		IF ( @OutputTableType = 5 ) 
		  SELECT * 
		  FROM   @tempAggregatedInputs; 

		-- Aggregated Outputs per product 
		DECLARE @tempAggregatedOutputs AS TABLE 
		  ( 
			 sourceproduct NVARCHAR(30), 
			 outputs       DECIMAL(29, 16) 
		  ); 

		INSERT INTO @tempAggregatedOutputs 
		SELECT sourceproduct                       AS Product, 
			   Isnull(Sum(netstandardvolume), 0.0) AS Outputs 
		FROM   @tempOutputs 
		WHERE  sourceproduct IN (SELECT productname 
								 FROM   @tempUniqueProducts) 
		GROUP  BY sourceproduct; 

		IF ( @OutputTableType = 5 ) 
		  SELECT * 
		  FROM   @tempAggregatedOutputs; 

		-- Aggregated IdentifiedLosses per product 
		DECLARE @tempAggregatedIdentifiedLoss AS TABLE 
		  ( 
			 product        NVARCHAR(30), 
			 identifiedloss DECIMAL(29, 16) 
		  ); 

		INSERT INTO @tempAggregatedIdentifiedLoss 
		SELECT sourceproduct, 
			   Isnull(Sum(netstandardvolume), 0.0) AS IdentifiedLoss 
		FROM   @tempIdentifiedLoss 
		WHERE  sourceproduct IN (SELECT productname 
								 FROM   @tempUniqueProducts) 
		GROUP  BY sourceproduct; 

		IF ( @OutputTableType = 5 ) 
		  SELECT * 
		  FROM   @tempAggregatedIdentifiedLoss; 

		-- Final UnBalance table 
		DECLARE @tempUnbalance AS TABLE 
		  ( 
			 productname      NVARCHAR(30), 
			 initialinventory DECIMAL(29, 16), 
			 inputs           DECIMAL(29, 16), 
			 outputs          DECIMAL(29, 16), 
			 identifiedloss   DECIMAL(29, 16), 
			 finalinventory   DECIMAL(29, 16), 
			 measurementunit  NVARCHAR (50) 
		  ); 

		INSERT INTO @tempUnbalance 
		SELECT up.productname, 
			   Isnull(ii.initialinventory, 0.0) AS InitialInventory, 
			   Isnull(ai.inputs, 0.0)           AS Inputs, 
			   Isnull(ao.outputs, 0.0)          AS Outputs, 
			   Isnull(il.identifiedloss, 0.0)   AS IdentifiedLoss, 
			   Isnull(fi.finalinventory, 0.0)   AS FinalInventory, 
			   up.measurementunit               AS MeasurementUnit 
		FROM   @tempUniqueProducts up 
			   LEFT JOIN @tempInitalInventory ii 
					  ON ii.productname = up.productname 
			   LEFT JOIN @tempAggregatedInputs ai 
					  ON ai.destinationproduct = up.productname 
			   LEFT JOIN @tempAggregatedOutputs ao 
					  ON ao.sourceproduct = up.productname 
			   LEFT JOIN @tempAggregatedIdentifiedLoss il 
					  ON il.product = up.productname 
			   LEFT JOIN @tempFinalInventory fi 
					  ON fi.productname = up.productname 
		WHERE  up.productname IS NOT NULL; 

		IF ( @OutputTableType = 0 
			  OR @OutputTableType = 4 
			  OR @OutputTableType = 5 ) 
		  SELECT productname, 
				 initialinventory, 
				 inputs, 
				 outputs, 
				 identifiedloss, 
				 finalinventory, 
				 ( initialinventory + inputs - outputs - finalinventory - identifiedloss 
				 ) 
												 AS 
				 Unbalance, 
				 measurementunit 
		  FROM   @tempUnbalance; 
	END TRY
	
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		THROW
	END CATCH
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to calculate the Unbalance without ownership for a given NodeId, StartDate, EndDate and OutputTableType.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_CalculateUnbalance',
    @level2type = NULL,
    @level2name = NULL