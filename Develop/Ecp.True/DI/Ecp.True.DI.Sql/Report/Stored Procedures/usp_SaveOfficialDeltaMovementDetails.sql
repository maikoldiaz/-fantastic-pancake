/* ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-24-2020
-- Update date: 	Aug-26-2020  Changed consolidation and delta calculation logic
-- Update date: 	Aug-26-2020  Inserting Start and End date values into table
-- Update date: 	Aug-26-2020  Changed consolidation movement logic
-- Update date: 	Sep-17-2020  Changed Percentage calculation
-- Updated date:    Sep-29-2020  Changed logic to show manual movements in case of delta input/ delta output as per PBI 28634
-- Update date: 	Sep-30-2020  Need to consider only the movements where source system is "TRUE" 
                                 from ConsolidatedMovement table
-- Update date: 	Oct-05-2020  Deleting the movements from report table which are 
                                 deleted from movement table in between the given period
-- Update date: 	Oct-08-2020  Filtering Consolidated Movements based on the given period
                                 Get the Inventories if SourceSystemId is 189 OR OfficialDeltaMessageTypeId IN (1,2) / NULL.
-- Update date: 	Oct-09-2020  Passing ElementId and NodeId as inputs to SP
-- Update date: 	Oct-15-2020  While deleting the records along with start and end date now considering segmentid as well.
-- Description:     This Procedure is to populate the official delta movement details into "[Report].[OfficialDeltaMovements]".
   EXEC [Report].[usp_SaveOfficialDeltaMovementDetails] 1528,'2020-07-01','2020-07-31'
   SELECT * FROM [Report].[OfficialDeltaMovements] WHERE MovementTransactionId IN (89002,89003)
   AND NodeId = 881
-- ==============================================================================================================================*/
CREATE PROCEDURE [Report].[usp_SaveOfficialDeltaMovementDetails]
(
 @SegmentId INT
,@StartDate DATE
,@EndDate   DATE
)
AS
BEGIN
SET NOCOUNT ON


			IF OBJECT_ID('tempdb..#BalanceNodes')IS NOT NULL
			DROP TABLE #BalanceNodes

			IF OBJECT_ID('tempdb..#MovOfficialDeltaDetails')IS NOT NULL
			DROP TABLE #MovOfficialDeltaDetails
			
			IF OBJECT_ID('tempdb..#MovOfficialDeltaIODetails')IS NOT NULL
			DROP TABLE #MovOfficialDeltaIODetails

			IF OBJECT_ID('tempdb..#MovOfficialConsolidatedDetails')IS NOT NULL
			DROP TABLE #MovOfficialConsolidatedDetails	

			IF OBJECT_ID('tempdb..#MainTemp')IS NOT NULL
			DROP TABLE #MainTemp	

			IF OBJECT_ID('tempdb..#MovOfficialDeltaInventoryDetails')IS NOT NULL
			DROP TABLE #MovOfficialDeltaInventoryDetails

           DROP TABLE IF EXISTS #CategoryElement

        -- Variables Declaration
	        DECLARE @Todaysdate	  DATETIME =  [Admin].[udf_GetTrueDate] ()
         
         -- Fetching DISTINCT Segment and NodeId for a given period from OfficialDeltaBalance table
            SELECT DISTINCT ODB.SegmentId,ODB.NodeId,ND.[Name] AS NodeName
             INTO #BalanceNodes
			 FROM [Report].[DeltaBalance] ODB 
             JOIN [Admin].[Node] ND
               ON ODB.NodeId = ND.NodeId
            WHERE ODB.SegmentId = @SegmentId
			  AND ODB.StartDate = @StartDate 
              AND ODB.EndDate   = @EndDate


           
            SELECT * 
	          INTO #CategoryElement 
	          FROM [Admin].[CategoryElement]

/************* Start of common code for delta movement calculations **********************/
            SELECT  
			        DISTINCT
			        Mov.MovementTransactionId             AS MovementTransactionId
				   ,Mov.OperationalDate                   AS OperationalDate
				   ,Mov.SourceProductID					  AS SourceProductID
				   ,Mov.SourceProductName				  AS SourceProductName
				   ,Mov.DestinationProductId			  AS DestinationProductId
				   ,Mov.DestinationProductName			  AS DestinationProductName
				   ,Mov.SourceNodeId					  AS SourceNodeId
				   ,Mov.SourceNodeName					  AS SourceNodeName
				   ,Mov.DestinationNodeId				  AS DestinationNodeId
				   ,Mov.DestinationNodeName				  AS DestinationNodeName
				   ,Mov.[Version]                         AS [Version]
				   ,ST.[Name]                             AS Scenario
				   ,Mov.MovementTypeName				  AS MovementTypeName
				   ,CEUnits.[Name]      				  AS MeasurementUnit
				   ,Mov.SourceSystem                      AS SystemName	
				   ,Mov.SourceSystemId                    AS SourceSystemId
				   ,ISNULL(Mov.NetStandardVolume,'0.0')   AS NetStandardVolume
				   ,ISNULL(Mov.GrossStandardVolume,'0.0') AS GrossStandardVolume
				   ,[Mov].SegmentID					      AS SegmentID
				   ,ND.NodeId						      AS NodeId
				   ,DeltaOwner.[Name]                     AS OwnerName
				   ,[Owner].OwnershipValueUnit            AS OwnershipValueUnit
				   ,[Owner].OwnershipValue                AS OwnershipValue
				   ,[Mov].[CreatedDate]                   AS ExecutionDate
				   ,[Mov].[OfficialDeltaMessageTypeId]    AS OfficialDeltaMessageTypeId
				   ,[Mov].[OfficialDeltaTicketId]         AS OfficialDeltaTicketId
			INTO #MovOfficialDeltaDetails
			FROM [Admin].[view_MovementInformation] Mov
			INNER JOIN #BalanceNodes ND
			ON ND.SegmentId = Mov.SegmentId
			AND (   ND.NodeId = Mov.SourceNodeId
			     OR ND.NodeId = Mov.DestinationNodeId
				)
			INNER JOIN [Admin].[ScenarioType] ST
		    ON ST.ScenarioTypeId = Mov.ScenarioId
			INNER JOIN #CategoryElement [Element] 
			ON [Mov].[SegmentId] = [Element].[ElementId]
			INNER JOIN #CategoryElement CEUnits
		    ON CEUnits.ElementId = Mov.MeasurementUnit
		    AND CEUnits.[CategoryId] = 6 --'Unidad de Medida'
            INNER JOIN [offchain].[Owner] [Owner]
		    ON [Mov].[MovementTransactionId] = [Owner].[MovementTransactionId]
			INNER JOIN #CategoryElement DeltaOwner
		    ON DeltaOwner.ElementId = [Owner].OwnerId
            WHERE ISNULL(Mov.SourceSystem,'') != 'TRUE'
		      --AND Mov.OfficialDeltaMessageTypeId IN (1,2,3,4)

/************* End of common code for delta movement calculations **********************/

         -- Delta Input/ Output Movement Calculations
           SELECT Mov.*
		         ,CASE 
				      WHEN (  Mov.OfficialDeltaMessageTypeId IN (3,4) 
					       OR SourceSystemId = 190
						   )
				       AND ISNULL(Mov.DestinationNodeId,0) = NodeId
					   --AND ISNULL(Mov.SourceNodeId,0) != NodeId
				      THEN  'Delta entradas'  -- Delta Input 
				      WHEN (  Mov.OfficialDeltaMessageTypeId IN (3,4) 
					       OR SourceSystemId = 190
						   )
				       AND ISNULL(Mov.SourceNodeId,0) = NodeId
					   --AND ISNULL(Mov.DestinationNodeId,0) != NodeId
                      THEN  'Delta salidas' -- Delta Output
				  END AS [Movement]
                 ,CASE 
				      WHEN  CHARINDEX('%',OwnershipValueUnit) > 0
			          THEN (Mov.NetStandardVolume * OwnershipValue) /100
			          ELSE OwnershipValue
			      END AS OwnershipVolume
                 ,CASE WHEN Mov.NetStandardVolume = 0
				      THEN 0
				      WHEN CHARINDEX('%',OwnershipValueUnit) > 0
			          THEN OwnershipValue
			          ELSE (OwnershipValue / Mov.NetStandardVolume ) * 100 
			      END AS OwnershipPercentage
				  ,MP.StartTime
				  ,MP.EndTime
		     INTO #MovOfficialDeltaIODetails
		     FROM #MovOfficialDeltaDetails Mov
			 JOIN [Offchain].[MovementPeriod] MP
			   ON Mov.MovementTransactionId = MP.MovementTransactionId
			 WHERE OfficialDeltaTicketId IS NOT NULL
			    AND (   Mov.OfficialDeltaMessageTypeId IN (3,4)
			         OR Mov.OfficialDeltaMessageTypeId IS NULL
					 )
                AND CAST(MP.StartTime AS DATE) = @StartDate
				AND CAST(MP.EndTime   AS DATE) = @EndDate
                

         -- Delta Initial/ Final Inventory Movement Calculations
            SELECT  Mov.*
			       ,CASE
					    WHEN (  Mov.OfficialDeltaMessageTypeId IN (1,2)
						     OR SourceSystemId = 189
						     )
					     AND (   Mov.DestinationNodeId = NodeId     
				              OR Mov.SourceNodeId      = NodeId
							  )
                         AND CAST(Mov.OperationalDate AS DATE) = DATEADD(DAY,-1,@StartDate)
                        THEN  'Delta inv. inicial' -- Delta Initial Inventory
					    WHEN (  Mov.OfficialDeltaMessageTypeId IN (1,2)
						      OR SourceSystemId = 189
						     )
					     AND (   Mov.DestinationNodeId = NodeId
				              OR Mov.SourceNodeId      = NodeId
							  )
                         AND CAST(Mov.OperationalDate AS DATE) = @EndDate
                        THEN  'Delta inv. final' -- Delta Final Inventory
					END AS [Movement]
                   ,CASE 
				        WHEN  CHARINDEX('%',OwnershipValueUnit) > 0
			            THEN (Mov.NetStandardVolume * OwnershipValue) /100
			            ELSE OwnershipValue
			        END AS OwnershipVolume
                   ,CASE WHEN Mov.NetStandardVolume = 0
				        THEN 0
				        WHEN CHARINDEX('%',OwnershipValueUnit) > 0
			            THEN OwnershipValue
			            ELSE (OwnershipValue / Mov.NetStandardVolume ) * 100 
			        END AS OwnershipPercentage
			INTO #MovOfficialDeltaInventoryDetails
		    FROM #MovOfficialDeltaDetails Mov
			WHERE Mov.OfficialDeltaTicketId IS NOT NULL
			  AND (   Mov.OfficialDeltaMessageTypeId IN (1,2)
			       OR Mov.OfficialDeltaMessageTypeId IS NULL
					 )
              AND (  CAST(Mov.OperationalDate AS DATE)  = DATEADD(DAY,-1,@StartDate)
			       OR CAST(Mov.OperationalDate AS DATE)  = @EndDate
				  )


            SELECT  
			        DISTINCT
			        ConMov.ConsolidatedMovementId             AS MovementTransactionId
				   ,ConMov.SourceProductID					  AS SourceProductID
				   ,SrcPrd.[Name]				              AS SourceProductName
				   ,ConMov.DestinationProductId			      AS DestinationProductId
				   ,DestPrd.[Name]     			              AS DestinationProductName
				   ,ConMov.SourceNodeId					      AS SourceNodeId
				   ,SrcNd.[NodeName]					      AS SourceNodeName
				   ,ConMov.DestinationNodeId				  AS DestinationNodeId
				   ,DestNd.[NodeName]				          AS DestinationNodeName
				   ,''                                        AS [Version]
				   ,'Operativo'                               AS Scenario
				   ,MoveType.[Name]				              AS MovementTypeName
				   ,CEUnits.[Name]      				      AS MeasurementUnit
				   ,SrcSys.[Name]                             AS SystemName				   
				   ,ISNULL(ConMov.NetStandardVolume,'0.0')    AS NetStandardVolume
				   ,ISNULL(ConMov.GrossStandardVolume,'0.0')  AS GrossStandardVolume
				   ,[ConMov].SegmentID					      AS SegmentID
				   --,[Element].[Name]    			          AS ElementName
				   ,BND.NodeId                                AS NodeId
				   ,DeltaOwner.[Name]                         AS OwnerName
				   ,CASE 
					   WHEN ISNULL(ConMov.DestinationNodeId,0) = ISNULL(BND.NodeId,0)
					    --AND ISNULL(ConMov.SourceNodeId,0) != BND.NodeId
					   THEN  'Entradas' -- Consolidated Input
					   WHEN ISNULL(ConMov.SourceNodeId,0) = ISNULL(BND.NodeId,0)
					    --AND ISNULL(ConMov.DestinationNodeId,0) != BND.NodeId
                       THEN  'Salidas' -- Consolidated Output
					END AS [Movement]
				   ,[Owner].OwnershipVolume                   AS OwnershipVolume
				   ,[Owner].OwnershipPercentage               AS OwnershipPercentage
				   ,[ConMov].[CreatedDate]                    AS ExecutionDate
			INTO #MovOfficialConsolidatedDetails
			FROM [Admin].[ConsolidatedMovement] ConMov
			LEFT JOIN [Admin].[Product] SrcPrd
			ON ISNULL(SrcPrd.ProductId,'') = ISNULL(ConMov.SourceProductId,'')
			LEFT JOIN [Admin].[Product] DestPrd
			ON ISNULL(DestPrd.ProductId,'') = ISNULL(ConMov.DestinationProductId,'')
			LEFT JOIN #BalanceNodes SrcND
			ON SrcND.SegmentId = ConMov.SegmentId
			AND ISNULL(SrcND.NodeId,0)   = ISNULL(ConMov.SourceNodeId,0)
			LEFT JOIN #BalanceNodes DestND
			ON DestND.SegmentId = ConMov.SegmentId
			AND ISNULL(DestND.NodeId,0)   = ISNULL(ConMov.DestinationNodeId,0)
			INNER JOIN #CategoryElement [Element] 
			ON ConMov.[SegmentId] = [Element].[ElementId]
			INNER JOIN #CategoryElement CEUnits
		    ON CEUnits.ElementId = ConMov.MeasurementUnit
		    AND CEUnits.[CategoryId] = 6 --'Unidad de Medida'
			INNER JOIN #CategoryElement MoveType
            ON MoveType.ElementId = ConMov.MovementTypeId
			INNER JOIN #CategoryElement SrcSys
            ON SrcSys.ElementId = ConMov.SourceSystemId
            INNER JOIN [Admin].[ConsolidatedOwner] [Owner]
            ON ConMov.[ConsolidatedMovementId] = [Owner].[ConsolidatedMovementId]
        	INNER JOIN #CategoryElement DeltaOwner
            ON DeltaOwner.ElementId = [Owner].OwnerId
        	INNER JOIN #BalanceNodes BND
			ON (    BND.NodeId = ConMov.SourceNodeId
			     OR BND.NodeId = ConMov.DestinationNodeId
				)
            WHERE ISNULL(SrcSys.[Name],'') = 'TRUE'
			  AND ConMov.SegmentId = @SegmentId
			  AND ConMov.StartDate = @StartDate
			  AND ConMov.EndDate   = @EndDate

     

         SELECT * 
		 INTO #MainTemp 
		 FROM (
                 SELECT  MovementTransactionId
                        ,SourceProductID
                        ,SourceProductName
                        ,DestinationProductId
                        ,DestinationProductName
                        ,SourceNodeId
                        ,SourceNodeName
                        ,DestinationNodeId
                        ,DestinationNodeName
                        ,[Version]
                        ,Scenario
                        ,MovementTypeName
                        ,MeasurementUnit
                        ,SystemName				   
                        ,NetStandardVolume
                        ,GrossStandardVolume
                        ,SegmentID
                        ,NodeId
                        ,OwnerName
                        ,[Movement]
                        ,OwnershipVolume
                        ,OwnershipPercentage
                        ,ExecutionDate
                 FROM #MovOfficialDeltaIODetails 
        		 WHERE Movement IS NOT NULL
        		 UNION
        		 SELECT  MovementTransactionId
                        ,SourceProductID
                        ,SourceProductName
                        ,DestinationProductId
                        ,DestinationProductName
                        ,SourceNodeId
                        ,SourceNodeName
                        ,DestinationNodeId
                        ,DestinationNodeName
                        ,[Version]
                        ,Scenario
                        ,MovementTypeName
                        ,MeasurementUnit
                        ,SystemName				   
                        ,NetStandardVolume
                        ,GrossStandardVolume
                        ,SegmentID
                        ,NodeId
                        ,OwnerName
                        ,[Movement]
                        ,OwnershipVolume
                        ,OwnershipPercentage
                        ,ExecutionDate
        		 FROM #MovOfficialDeltaInventoryDetails 
        		 WHERE Movement IS NOT NULL
        		 UNION
        		 SELECT  MovementTransactionId
                        ,SourceProductID
                        ,SourceProductName
                        ,DestinationProductId
                        ,DestinationProductName
                        ,SourceNodeId
                        ,SourceNodeName
                        ,DestinationNodeId
                        ,DestinationNodeName
                        ,[Version]
                        ,Scenario
                        ,MovementTypeName
                        ,MeasurementUnit
                        ,SystemName				   
                        ,NetStandardVolume
                        ,GrossStandardVolume
                        ,SegmentID
                        ,NodeId
                        ,OwnerName
                        ,[Movement]
                        ,OwnershipVolume
                        ,OwnershipPercentage
                        ,ExecutionDate
        		 FROM #MovOfficialConsolidatedDetails 
        		 WHERE Movement IS NOT NULL
            )SQ


        -- To Delete the movements from report table which are deleted from movement table in between the given period
           DELETE 
		     FROM [Report].[OfficialDeltaMovements] 
		    WHERE MovementTransactionId IN (
                                            SELECT MovementTransactionid 
											  FROM [Report].[OfficialDeltaMovements] 
											 WHERE StartDate = @StartDate 
											   AND EndDate   = @EndDate
											   AND SegmentId = @SegmentId
                                            EXCEPT
                                            SELECT MovementTransactionid 
											  FROM #MainTemp 
                                            )

        -- UPSERTING THE DATA INTO [Report].[OfficialDeltaMovements] TABLE
           MERGE INTO [Report].[OfficialDeltaMovements] DEST
           USING #MainTemp SRC
           ON  Dest.[StartDate]                          = @StartDate
	       AND Dest.[EndDate]                            = @EndDate
           AND ISNULL(Dest.MovementTransactionId,0)      = ISNULL(Src.MovementTransactionId,0)
           AND ISNULL(Dest.SourceProduct,'')		     = ISNULL(Src.SourceProductName,'')
           AND ISNULL(Dest.DestinationProduct,'')	     = ISNULL(Src.DestinationProductName,'')
           AND ISNULL(Dest.SourceNodeId,0)				 = ISNULL(Src.SourceNodeId,0)
           AND ISNULL(Dest.SourceNode,'')			     = ISNULL(Src.SourceNodeName,'')
           AND ISNULL(Dest.DestinationNodeId,0)			 = ISNULL(Src.DestinationNodeId,0)
           AND ISNULL(Dest.DestinationNode,'')		     = ISNULL(Src.DestinationNodeName,'')
           AND ISNULL(Dest.[Version],'')				 = ISNULL(Src.[Version],'')
           AND ISNULL(Dest.Scenario,'')					 = ISNULL(Src.Scenario,'')
           AND ISNULL(Dest.TypeMovement,'')			     = ISNULL(Src.MovementTypeName,'')
           AND ISNULL(Dest.MeasurementUnit,'')			 = ISNULL(Src.MeasurementUnit,'')
           AND ISNULL(Dest.Origin,'')		   	    	 = ISNULL(Src.SystemName,'')		   
           AND ISNULL(Dest.SegmentID,0)					 = ISNULL(Src.SegmentID,0)
           AND ISNULL(Dest.NodeId,0)					 = ISNULL(Src.NodeId,0)
           AND ISNULL(Dest.[Owner],'')				     = ISNULL(Src.OwnerName,'')
           AND ISNULL(Dest.ExecutionDate,'9999-12-31')	 = ISNULL(Src.ExecutionDate,'9999-12-31')

	       WHEN MATCHED 
	        AND (  
                    ISNULL(Dest.[Movement],'')              <> ISNULL(Src.[Movement],'')
                 OR ISNULL(Dest.NetQuantity,'0.0')	        <> ISNULL(Src.NetStandardVolume,'0.0')
                 OR ISNULL(Dest.GrossQuantity,'0.0')	    <> ISNULL(Src.GrossStandardVolume,'0.0')
                 OR ISNULL(Dest.OwnershipVolume,'0.0')		<> ISNULL(Src.OwnershipVolume,'0.0')
                 OR ISNULL(Dest.OwnershipPercentage,'0.0')	<> ISNULL(Src.OwnershipPercentage,'0.0')
				 )
          THEN
    	  UPDATE
    	     SET Dest.[Movement]			= Src.[Movement]
                ,Dest.[NetQuantity]		    = Src.[NetStandardVolume]
                ,Dest.[GrossQuantity]	    = Src.[GrossStandardVolume]
                ,Dest.[OwnershipVolume]	    = Src.[OwnershipVolume]
                ,Dest.[OwnershipPercentage] = Src.[OwnershipPercentage]
    			,Dest.LastModifiedBy        = 'ReportUser'
    			,Dest.LastModifiedDate      = @Todaysdate

          WHEN NOT MATCHED THEN
    	  INSERT (
                  [StartDate]                               
                 ,[EndDate]                            
                 ,MovementTransactionId     
                 ,SourceProduct	     
                 ,DestinationProduct     
                 ,SourceNodeId			 
                 ,SourceNode			     
                 ,DestinationNodeId			 
                 ,DestinationNode	     
                 ,[Version]				 
                 ,Scenario					 
                 ,TypeMovement			     
                 ,MeasurementUnit			 
                 ,Origin	   	    	 
                 ,SegmentID					 
                 ,NodeId					 
                 ,[Owner]				     
                 ,ExecutionDate	 
                 ,[Movement]
                 ,NetQuantity
                 ,GrossQuantity
                 ,OwnershipVolume
                 ,OwnershipPercentage
				 ,CreatedBy
				 ,CreatedDate
				 ,LastModifiedBy
				 ,LastModifiedDate
				 )
          VALUES (
                  @StartDate
                 ,@EndDate
                 ,Src.MovementTransactionId     
                 ,Src.SourceProductName
                 ,Src.DestinationProductName     
                 ,Src.SourceNodeId			 
                 ,Src.SourceNodeName			     
                 ,Src.DestinationNodeId			 
                 ,Src.DestinationNodeName	     
                 ,Src.[Version]				 
                 ,Src.Scenario					 
                 ,Src.MovementTypeName			     
                 ,Src.MeasurementUnit			 
                 ,Src.SystemName	   	    	 
                 ,Src.SegmentID					 
                 ,Src.NodeId					 
                 ,Src.[OwnerName]				     
                 ,Src.ExecutionDate	 
                 ,Src.[Movement]
                 ,Src.NetStandardVolume
                 ,Src.GrossStandardVolume
                 ,Src.OwnershipVolume
                 ,Src.OwnershipPercentage
				 ,'ReportUser'
				 ,@TodaysDate
				 ,'ReportUser'
				 ,@TodaysDate
				 );

END



GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to populate the official delta movement details into "[Report].[OfficialDeltaMovements]".',
	@level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveOfficialDeltaMovementDetails',
    @level2type = NULL,
    @level2name = NULL