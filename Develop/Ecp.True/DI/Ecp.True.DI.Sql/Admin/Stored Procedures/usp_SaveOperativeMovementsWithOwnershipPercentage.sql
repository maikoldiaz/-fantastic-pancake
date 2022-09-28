/*-- =================================================================================================================================
-- Author:          Microsoft  
-- Created Date:    May-22-2020  
-- Updated Date:    May-29-2020  1. To Calculate OwnershipPercentage using "NetStandardVolume" from OperativeMovementsWithOwnership 
                                    since it is similar to NetStandardVolume in OperativeMovements as per the discussion board 18394.
                                 2. Added an extra check in Step 1 to consider only the nodes which belongs to Ownership
                                 3. We need to Consider only ECOPETROL Owner related Movement Details.
                                 4. Added TicketId dependency
                    May-29-2020  Changed Merge condition for Decimal Columns
				    Jun-03-2020  1. Changed input parameter from TicketId to OwnershipNodeId
				                 2. Nodes which belong to OwnershipNodeId either as source or destination
                    Jun-09-2020  Raise an exception at the time of Invalid Movements
					Jun-17-2020  Removed 2 condition checks to calculate invalid movements and added them in ELSE
                    Oct-23-2020  Changed code to remove logistic center and storage location name concatenation as the name is concatenated in SL table
                    Oct-28-2020  Excluding the deleted records from the [Analytics].[OperativeNodeRelationshipWithOwnership] table
                    Nov-03-2020  Changed the logic to get only the first storage location when node has more than one storage locations
                    Nov-05-2020  Added ProductId in partition by clause to get the first storage location that contains the product
-- <Description>:	This Procedure is used to save the Operative Movements with Ownership and Ownership Percentage Information for a given Ticket Id.  </Description>
-- EXEC [Admin].[usp_SaveOperativeMovementsWithOwnershipPercentage] 11155
-- ==================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveOperativeMovementsWithOwnershipPercentage] 
(
       @OwnershipNodeId          INT
)
AS 
  BEGIN 
    SET NOCOUNT ON
	
	     BEGIN TRY
         DECLARE @lnvErrorMessage NVARCHAR(MAX)

               BEGIN TRANSACTION

                      IF OBJECT_ID('tempdb..#TempTrueData_Step1')IS NOT NULL
                      DROP TABLE #TempTrueData_Step1
					  
					  IF OBJECT_ID('tempdb..#TempTrueDataWithNodeRelationWithOwner_Step234')IS NOT NULL
                      DROP TABLE #TempTrueDataWithNodeRelationWithOwner_Step234
                    
                      IF OBJECT_ID('tempdb..#TempOperativeMovements_WO_Step7')IS NOT NULL
                      DROP TABLE #TempOperativeMovements_WO_Step7
                    
					  IF OBJECT_ID('tempdb..#TempOwnerNodes_Step0')IS NOT NULL
                      DROP TABLE #TempOwnerNodes_Step0

					  IF OBJECT_ID('tempdb..#TempAnulaElements_Stpe0')IS NOT NULL
                      DROP TABLE #TempAnulaElements_Stpe0

                      DECLARE @TodaysDate DATETIME
					         ,@ElementId INT
							 ,@TicketId INT
							 ,@OwnershipNodeStatus INT
                         SET @TodaysDate = (SELECT [Admin].udf_GetTrueDate())
            
			       -- Get ECOPETROL ElementId
					  SET @ElementId = (SELECT ElementId FROM [Admin].[CategoryElement] WHERE CategoryId = 7 AND [Name] = 'ECOPETROL')
                    
				   -- Get Ticket   
					  SET @TicketId = (SELECT Ticketid FROM [Admin].[OwnerShipNode] WHERE OwnershipNodeid = @OwnershipNodeId)

				   -- Get Ticket OwnershipNodeStatus
					  SET @OwnershipNodeStatus = (SELECT OwnershipNodeStatusTypeid 
					                                FROM [Admin].[OwnershipNodeStatusType] 
												   WHERE [Name] = 'APROBADO'
												  )

				   -- To consider only ownership related nodes
					  SELECT OwnershipNodeId,TicketId,NodeId,OwnershipStatusId 
					    INTO #TempOwnerNodes_Step0 
						FROM [Admin].[OwnershipNode] 
					   WHERE OwnershipStatusId = @OwnershipNodeStatus
                      
				   -- To Identify the elements are of Anula type or not
					  SELECT [ElementId]
                            ,CHARINDEX('Anula', [Name]) AS Anula -- To Categorize the Anula type of movements
                    	INTO #TempAnulaElements_Stpe0
						FROM [Admin].[CategoryElement]
					  
                SELECT *
				  INTO #TempTrueData_Step1
				  FROM (                       
					       SELECT DISTINCT 
                                  Mov.MovementID
                    	    	 ,Mov.OperationalDate
								 ,CASE WHEN ISNULL(Mov.SourceProductId,0)  <> ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) <> ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) <> ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 0
                    	    	       THEN 'Tr. Material a material'
                    	    		   WHEN ISNULL(Mov.SourceProductId,0) <> ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) <> ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) = ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 0
                    	    	       THEN 'Tr. Material a material'
                    	    		   WHEN ISNULL(Mov.SourceProductId,0) <> ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) = ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) <> ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 0
                    	    	       THEN 'Tr. Material a material'
                    	    		   WHEN ISNULL(Mov.SourceProductId,0) <> ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) = ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) = ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 0
                    	    	       THEN 'Tr. Material a material'
                    	    		   WHEN ISNULL(Mov.SourceProductId,0) = ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) <> ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) <> ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 0
                    	    	       THEN 'Tr.trasladar ce a ce'
                    	    		   WHEN ISNULL(Mov.SourceProductId,0) = ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) <> ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) = ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 0
                    	    	       THEN 'Tr.trasladar ce a ce'
                    	    		   WHEN ISNULL(Mov.SourceProductId,0) = ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) = ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) <> ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 0
                    	    	       THEN 'Tr. Almacen a Almacen'
                                       WHEN ISNULL(Mov.SourceProductId,0) <> ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) <> ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) <> ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 1
                    	    	       THEN 'Anul. Tr. Material a material'
                    	    		   WHEN ISNULL(Mov.SourceProductId,0) <> ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) <> ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) = ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 1
                    	    	       THEN 'Anul. Tr. Material a material'
                    	    		   WHEN ISNULL(Mov.SourceProductId,0) <> ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) = ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) <> ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 1
                    	    	       THEN 'Anul. Tr. Material a material'
                    	    		   WHEN ISNULL(Mov.SourceProductId,0) <> ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) = ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) = ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 1
                    	    	       THEN 'Anul. Tr. Material a material'
                    	    		   WHEN ISNULL(Mov.SourceProductId,0) = ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) <> ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) <> ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 1
                    	    	       THEN 'Anul. Tr.trasladar ce a ce'
                    	    		   WHEN ISNULL(Mov.SourceProductId,0) = ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) <> ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) = ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 1
                    	    	       THEN 'Anul. Tr.trasladar ce a ce'
                    	    		   WHEN ISNULL(Mov.SourceProductId,0) = ISNULL(Mov.DestinationProductId,0)
                    	    	        AND ISNULL(SrcNode.LogisticCenterId,0) = ISNULL(DestNode.LogisticCenterId,0)
                    	    			AND ISNULL(SrcNode.StorageLocationId,0) <> ISNULL(DestNode.StorageLocationId,0)
                    	    			AND EsAnulacion.Anula = 1
                    	    	       THEN 'Anul. Tr. Almacen a Almacen'
                    	    		   ELSE 'INVALID_COMBINATION_TO_SIV_MOVEMENT'
						          END AS MovementType
                                 ,Mov.SourceProductName
                    	    	 ,Mov.DestinationProductName
                    	    	 ,CASE WHEN SourceLogisticName IS NULL 
                    	    	        AND SourceStorageLocationName IS NULL
                                       THEN NULL
                    	    		   ELSE SourceStorageLocationName
                    	    	  END AS SourceStorageLocationName
                    	    	 ,CASE WHEN DestinationLogisticName IS NULL
                    	    	        AND DestinationStorageLocationName IS NULL
                    	    		   THEN NULL
                    	    		   ELSE DestinationStorageLocationName
                    	    	  END AS DestinationStorageLocationName
                    	    	 ,CASE WHEN EsAnulacion.Anula = 1
                    	    	       THEN ISNULL([Ownership].OwnershipVolume,0)*-1
                    	    		   ELSE [Ownership].OwnershipVolume
                    	    	  END AS [OwnershipVolume]
                    	    	 ,[Ownership].OwnershipPercentage  AS [OwnershipPercentage]
                    	    	 ,Mov.NetStandardVolume
                            FROM [Admin].[view_MovementInformation] Mov
                      INNER JOIN [offchain].[Ownership] [Ownership] -- Get only the movements which has Owner information
                              ON [Mov].[MovementTransactionId] = [Ownership].[MovementTransactionId]
                             AND [Mov].[OwnershipTicketId] = [Ownership].[TicketId] -- To Consider only Owner related tickets
                             AND [Ownership].[OwnerId] = @ElementId -- As we need to consider only ECOPETROL Owner movements
                             AND [Ownership].[TicketId] = @TicketId -- Extrating the movements which are related to a perticular ticket id.
							 And [Ownership].[IsDeleted] = 0
                      INNER JOIN #TempOwnerNodes_Step0 OSN -- To Consider only the nodes which related to Ownership
                              ON [Ownership].[TicketId] = OSN.[TicketId]
                      INNER JOIN #TempAnulaElements_Stpe0 EsAnulacion
                             ON Mov.MovementTypeId = EsAnulacion.ElementId
                      /*Calculation for source related info */
                      LEFT JOIN (SELECT ROW_NUMBER () OVER (PARTITION BY NodeId, SLP.ProductId ORDER BY SrcStrLoc.StorageLocationId) AS SRNo
								       ,NodeId
									   ,SrcStrLoc.StorageLocationId
									   ,SrcStrLoc.[Name] AS SourceStorageLocationName
									   ,SrcLogistic.LogisticCenterId
									   ,ProductId
									   ,SrcLogistic.[Name] AS SourceLogisticName 
                                  FROM [Admin].[NodeStorageLocation] SrcNode  
                                  INNER JOIN [Admin].[StorageLocation] SrcStrLoc
                                          ON SrcStrLoc.StorageLocationId = SrcNode.StorageLocationId
                                  INNER JOIN [Admin].[StorageLocationProduct] SLP
                                          ON SLP.NodeStorageLocationId = SrcNode.NodeStorageLocationId
                                  INNER JOIN [Admin].[LogisticCenter] SrcLogistic
                                          ON SrcStrLoc.LogisticCenterId = SrcLogistic.LogisticCenterId
							    )SrcNode
                               ON Mov.SourceNodeId = SrcNode.NodeId
                              AND SrcNode.ProductId = Mov.SourceProductId
							  AND SRNo = 1
                      /*Calculation for destination related info */
                      LEFT JOIN (SELECT ROW_NUMBER () OVER (PARTITION BY NodeId, DLP.ProductId ORDER BY DestStrLoc.StorageLocationId) AS DRNo
								       ,NodeId
					                   ,DestStrLoc.StorageLocationId
									   ,DestStrLoc.[Name] AS DestinationStorageLocationName 
									   ,DestLogistic.LogisticCenterId
									   ,ProductId
					                   ,DestLogistic.[Name] AS DestinationLogisticName
					               FROM [Admin].[NodeStorageLocation] DestNode
                                   INNER JOIN [Admin].[StorageLocation] DestStrLoc
                                           ON DestStrLoc.StorageLocationId = DestNode.StorageLocationId
                                   INNER JOIN [Admin].[StorageLocationProduct] DLP
           					               ON DLP.NodeStorageLocationId = DestNode.NodeStorageLocationId
                                   INNER JOIN [Admin].[LogisticCenter] DestLogistic
                                           ON DestStrLoc.LogisticCenterId = DestLogistic.LogisticCenterId
         					    )DestNode
                               ON Mov.DestinationNodeId = DestNode.NodeId
        					  AND DestNode.ProductId = Mov.DestinationProductId
							  AND DRNo = 1
                      WHERE [OSN].[OwnershipNodeId] = @OwnershipNodeId 
   					  AND (   [OSN].[NodeId] = [Mov].SourceNodeId 
   					       OR [OSN].[NodeId] = [Mov].DestinationNodeId
   						   )	
   					  AND [OSN].[ownershipstatusid] = @OwnershipNodeStatus
   					 ) SubQ
			    --WHERE MovementType != 'INVALID_COMBINATION_TO_SIV_MOVEMENT' -- We need to consider only the movement types which are valid

				IF EXISTS (SELECT 1 FROM #TempTrueData_Step1 WHERE MovementType = 'INVALID_COMBINATION_TO_SIV_MOVEMENT')
					 RAISERROR('La combinación de producto, centro logístico y almacén es invalida y no esta considerada para envió a SIV.',16,1)
				
				             					
					/* Get transfer point by joining the true tables with NodeRelationshipWithOwnership based on 
                      (SourceProduct,DestinationProduct,SourceStorageLocation,DestinationStorageLocation) 
                    */
                        SELECT  
                    	       Temp1.MovementID 
                    	      ,Temp1.OperationalDate
                               ,Temp1.MovementType
                    	   	   ,Temp1.SourceProductName
                    	   	   ,Temp1.SourceStorageLocationName
                    	   	   ,Temp1.DestinationProductName
                    	   	   ,Temp1.DestinationStorageLocationName
                    	   	   ,CAST(Temp1.OwnershipVolume AS DECIMAL(18,2)) AS OwnershipVolume
                    	   	   ,CAST(Temp1.NetStandardVolume AS DECIMAL (18,2)) AS NetStandardVolume
                    	   	   ,CAST(Temp1.OwnershipPercentage AS DECIMAL (5,2)) AS OwnershipPercentage
                    	   	   ,ONRWO.TransferPoint
							   ,DAY(OperationalDate) AS [Day]
							   ,MONTH(OperationalDate) AS [Month]
							   ,YEAR(OperationalDate) AS [Year]
                          INTO #TempTrueDataWithNodeRelationWithOwner_Step234
                    	  FROM #TempTrueData_Step1 Temp1
                    INNER JOIN [Analytics].[OperativeNodeRelationshipWithOwnership] ONRWO
                            ON Temp1.SourceProductName              = ONRWO.SourceProduct
                           AND Temp1.DestinationProductName         = ONRWO.DestinationProduct
                           AND Temp1.SourceStorageLocationName      = ONRWO.LogisticSourceCenter
                           AND Temp1.DestinationStorageLocationName = ONRWO.LogisticDestinationCenter
						   And ONRWO.IsDeleted = 0 
                    
                    -- STEP 5 
                    -- MERGE DATA INTO [Analytics].[OperativeMovementsWithOwnership] 
                    -- KEY COLUMNS : OperationalDate, MovementType, , SourceStorageLocation, DestinationProduct, DestinationStorageLocation
                       MERGE [Analytics].[OperativeMovementsWithOwnership] AS Dest
                       USING #TempTrueDataWithNodeRelationWithOwner_Step234 AS Src
                       ON (    Dest.MovementId                   = Src.MovementId 
                           AND Dest.OperationalDate              = Src.OperationalDate 
                           AND Dest.MovementType                 = Src.MovementType 
                           AND Dest.SourceProduct                = Src.SourceProductName
                           AND Dest.SourceStorageLocation        = Src.SourceStorageLocationName 
                           AND Dest.DestinationProduct           = Src.DestinationProductName 
                           AND Dest.DestinationStorageLocation   = Src.DestinationStorageLocationName
                    	   )
                     
					 WHEN MATCHED AND (    Dest.TransferPoint       <> Src.TransferPoint
                    	                OR Dest.OwnershipVolume     <> Src.OwnershipVolume
                    	                OR Dest.NetStandardVolume   <> Src.NetStandardVolume
                                        OR Dest.OwnershipPercentage <> Src.OwnershipPercentage
                    		            OR Dest.[DayOfMonth]        <> Src.[Day]
                    		            OR Dest.[Month]	            <> Src.[Month]
                    		            OR Dest.[Year]	            <> Src.[Year]
								        )
					  THEN
                      UPDATE 
                         SET Dest.TransferPoint       = Src.TransferPoint
                    	    ,Dest.OwnershipVolume     = Src.OwnershipVolume
                    	    ,Dest.NetStandardVolume   = Src.NetStandardVolume
                            ,Dest.OwnershipPercentage = Src.OwnershipPercentage
                    		,Dest.[DayOfMonth]        = Src.[Day]
                    		,Dest.[Month]	          = Src.[Month]
                    		,Dest.[Year]	          = Src.[Year]
                    		,Dest.[LastModifiedBy]    = 'System'
                    		,Dest.[LastModifiedDate]  = @TodaysDate
                    
                       WHEN NOT MATCHED THEN
                       INSERT ( [MovementId]
					           ,[OperationalDate]
                               ,[MovementType]									
                               ,[SourceProduct]									
                               ,[SourceStorageLocation]							
                               ,[DestinationProduct]							
                               ,[DestinationStorageLocation]					
                               ,[OwnershipVolume]								
                               ,[TransferPoint]									
                               ,[Month]											
                               ,[Year]											
                               ,[DayOfMonth]									
                               ,[SourceSystem]									
                               ,[LoadDate]										
                               --,[ExecutionID]									
                               ,[NetStandardVolume]                             
                               ,[OwnershipPercentage]                           
                               ,[CreatedBy]										
                               ,[CreatedDate]
                              ) 
                    	VALUES (Src.[MovementId]
						       ,Src.[OperationalDate]								
                               ,Src.[MovementType]									
                               ,Src.[SourceProductName]									
                               ,Src.[SourceStorageLocationName]							
                               ,Src.[DestinationProductName]							
                               ,Src.[DestinationStorageLocationName]					
                               ,Src.[OwnershipVolume]								
                               ,Src.[TransferPoint]									
                               ,Src.[Month]
                               ,Src.[Year]
                               ,Src.[Day]
                               ,'TRUE'	    -- SourceSystem  								
                               ,@TodaysDate	-- LoadDate  									
                               --,''        -- [ExecutionID]									
                               ,Src.[NetStandardVolume]                             
                               ,Src.[OwnershipPercentage]                           
                               ,'System'    --[CreatedBy]										
                               ,@TodaysDate --[CreatedDate] 
                    		   )
                    		   ;
                    
                    -- STEP 7 
                    -- JOIN [Analytics].[OperativeMovementsWithOwnership], [Analytics].[OperativeMovements] table Daywise
                      
                      SELECT OperationalDate
					        ,TransferPoint
							,CASE WHEN OwnershipPercentage < 0 THEN 0
                                  WHEN OwnershipPercentage > 1 THEN 1
			                      ELSE OwnershipPercentage
			                      END AS TotalOwnershipPercentage
					    INTO #TempOperativeMovements_WO_Step7
						FROM (
        					  SELECT OperationalDate
                                    ,TransferPoint
                            		,CASE WHEN TotalOwnershipVolume = 0   -- OR TotalOwnershipVolume IS NULL (Excluding NULL check coz incase null inner query returns "0")
                            		      THEN 0
                            			  WHEN TotalNetStandardVolume = 0 -- OR TotalNetStandardVolume IS NULL (Excluding NULL check coz incase null inner query returns "0")
                            		      THEN 1
                            			  ELSE TotalOwnershipVolume/TotalNetStandardVolume 
                            		  END 
                            		  AS OwnershipPercentage
                                
                                FROM (
                                      SELECT OperationalDate
        			                        ,TransferPoint
        					                ,SUM(ISNULL(OwnershipVolume,0)) AS TotalOwnershipVolume
        					                ,SUM(ISNULL(NetStandardVolume,0)) AS TotalNetStandardVolume
        					           FROM [Analytics].[OperativeMovementsWithOwnership] OMWO
        						       GROUP BY OMWO.OperationalDate
                                               ,OMWO.TransferPoint
                            		 ) InnSubQ	
							   )SubQ
                       
                    
                    -- STEP 8 
                    -- UPSERT [Analytics].[OwnershipPercentage] table
                    -- Key Columns OperationalDate, TransferPoint
                    
                       MERGE [Analytics].[OwnershipPercentageValues] Dest
                       USING #TempOperativeMovements_WO_Step7 Src
                          ON Dest.OperationalDate = Src.OperationalDate
                    	 AND Dest.TransferPoint = Src.TransferPoint
                    
                       WHEN MATCHED AND Dest.OwnershipPercentage <> CAST(Src.TotalOwnershipPercentage AS DECIMAL (5,2))
					   THEN 
                       UPDATE
                          SET Dest.OwnershipPercentage = Src.TotalOwnershipPercentage
                    	     ,Dest.LastModifiedBy      = 'System'
                    		 ,Dest.LastModifiedDate    = @TodaysDate
                    
                       WHEN NOT MATCHED THEN
                       INSERT ( OperationalDate
                               ,TransferPoint
                    		   ,OwnershipPercentage
                    		   ,SourceSystem
                    		   ,LoadDate
                    		   --,ExecutionID
                    		   ,CreatedBy
                    		   ,CreatedDate
                    		   )
                       VALUES ( Src.OperationalDate
                               ,Src.TransferPoint
                    		   ,Src.TotalOwnershipPercentage
                    		   ,'TRUE'      -- SourceSystem 
                    		   ,@TodaysDate -- LoadDate 
                    		   --,''        -- ExecutionID 
                    		   ,'System'    -- CreatedBy 
                    		   ,@TodaysDate -- CreatedDate 
                    		   );

			      -- Update SP execuion status in ticket table againest the ticket id on successful execution
                     UPDATE [Admin].[OwnershipNode]
                     SET [OwnershipAnalyticsStatus]       = 1 -- SUCCESS
                        ,[OwnershipAnalyticsErrorMessage] = NULL
                     WHERE OwnershipNodeId = @OwnershipNodeId

                     IF OBJECT_ID('tempdb..#TempTrueData_Step1')IS NOT NULL
                     DROP TABLE #TempTrueData_Step1
                     
                     IF OBJECT_ID('tempdb..#TempTrueDataWithNodeRelationWithOwner_Step234')IS NOT NULL
                     DROP TABLE #TempTrueDataWithNodeRelationWithOwner_Step234
                     
                     IF OBJECT_ID('tempdb..#TempOperativeMovements_WO_Step7')IS NOT NULL
                     DROP TABLE #TempOperativeMovements_WO_Step7

                    COMMIT TRANSACTION
      END TRY


	 BEGIN CATCH    
		SELECT @lnvErrorMessage = ERROR_MESSAGE() 
		ROLLBACK TRANSACTION
      -- Update SP exectuion status in ticekt table againest the ticket id on failure execution
         UPDATE [Admin].[OwnershipNode]
            SET [OwnershipAnalyticsStatus]       = 0 -- FAILURE
               ,[OwnershipAnalyticsErrorMessage] = @lnvErrorMessage
          WHERE OwnershipNodeId = @OwnershipNodeId

		RAISERROR (@lnvErrorMessage,16,1)  
	 END CATCH                         
   END





GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to save the Operative Movements with Ownership and Ownership Percentage Information for a given Ticket Id.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveOperativeMovementsWithOwnershipPercentage',
    @level2type = NULL,
    @level2name = NULL
