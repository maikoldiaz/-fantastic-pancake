/*-- ========================================================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Oct-15-2019
-- Updated Date:	Dec-13-2015 (For PBI:13069; Task:18319)
--				    Apr-09-2020  -- Removed(BlockchainStatus = 1)   
--				    May-06-2020  -- updated UncertaintyPercentage 
--					Jun-12-2020  -- Modified Code For Movement Table Updating Logic for TicketType(1-->CutOff)
--					Jun-19-2020  -- Added Logic for Delta TicketType(4)
--					Jul-08-2020 --	Modified as per PBI 58650
--					Jul-08-2020 --	Modified as CategoryElementId Condition
--					Jul-15-2020 -- Included temp tables for updates to improve performance
--					Jul-16-2020 -- Fixed temp Table issues For TicketTypeId 1
--					Jul-30-2020 -- Modified for TicketTypeId 6 ( OfficialLogistics )
--					Aug-07-2020 -- Modified Procedure for TicketTypeId = 1 for performance reasons
--					Aug-14-2020 -- Reverted the Changes
--					Aug-26-2020 -- Included first time node changes
--					Mar-30-2021 -- Included LogisticNodes and ScenarioTypeId parameters and insert nodes of a ticket
--					May-03-2021 -- Set StatusProcessId from Failed to Canceled for the falied logistic movement selected by the user to include in a new batch
-- <Description>:   This Procedure is used to Modify Data into Ticket, UnBalance, Inventory, Movement, PendingTransaction and PendingTransactionError tables for a given CategoryElementId, StartDate, EndDate, UserId ,TicketTypeId ,PendingTransactionErrorMessagesType ,UnbalanceType ,UncertaintyPercentage ,OwnerId ,NodeId ,TicketGroupId. </Description>
-- Note: Incase of All Nodes for Parameter NUll will be passes Else Specific Node Id Will be passed 
-- ========================================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveTicket]
(
       @CategoryElementId                                    INT,
       @StartDate                                            DATE,
       @EndDate                                              DATE,
       @UserId                                               NVARCHAR(260),
	   @TicketTypeId										 INT,
	   @FirstTimeNodes                                       [Admin].[NodeListType] READONLY,
	   @ScenarioTypeId										 INT = NULL,
	   @LogisticNodes                                        [Admin].[NodeListType] READONLY,
	   @FailedLogisticsMovements                             [Admin].[KeyType] READONLY,
       @UncertaintyPercentage								 DECIMAL(29,16) NULL,
	   @OwnerId												 INT = NULL,
	   @NodeId												 INT = NULL,
	   @TicketGroupId										 NVARCHAR(255) NULL,
	   @SessionId											 NVARCHAR(50) NULL

)
AS
BEGIN
	   SET NOCOUNT ON

       BEGIN TRY
             BEGIN TRANSACTION

			 DECLARE @Out_TicketId	INT

			 IF EXISTS(SELECT 1 FROM [Admin].[CategoryElement] WHERE ElementId = @CategoryElementId)
			 OR (@CategoryElementId = 0 AND @TicketTypeId = 5)
			 BEGIN

					IF OBJECT_ID('tempdb..#TempUpdate1')IS NOT NULL
					DROP TABLE #TempUpdate1

					IF OBJECT_ID('tempdb..#TempUpdate2')IS NOT NULL
					DROP TABLE #TempUpdate2

					IF OBJECT_ID('tempdb..#TempUpdate3')IS NOT NULL
					DROP TABLE #TempUpdate3

                    DECLARE @TodaysDateTime     DATETIME = Admin.udf_GetTrueDate()

					IF @TicketTypeId = 2 --Ownership
					BEGIN
						--Insert data into Ticket table
						INSERT INTO [Admin].[Ticket]
								  (
									   [CategoryElementId]
									  ,[StartDate]
									  ,[EndDate]
									  ,[Status]
									  ,[TicketTypeId]
									  ,[TicketGroupId]
									  ,[OwnerId]
									  ,[NodeId]
									  ,[CreatedBy]
									  ,[CreatedDate]
								  )  
						SELECT    @CategoryElementId	AS [CategoryElementId]
								 ,dates					AS [StartTime]
								 ,dates					AS [EndTime]
								 ,1						AS [Status]		-- Initially setting the Status in Progress
								 ,@TicketTypeId			AS [TicketTypeId]
								 ,@TicketGroupId		AS [TicketGroupId]
								 ,CASE WHEN @TicketTypeId = 3 
									   THEN @OwnerId
									   ELSE NULL
									   END				AS [OwnerId]
								 ,@NodeId				AS [NodeId]
								 ,@UserId				AS [CreatedBy] 
								 ,@TodaysDateTime		AS [CreatedDate]
						FROM [Admin].[udf_GetAllDates]( @StartDate, 
														@EndDate, 
														1, 
														1) C

						--Fetch the First TicketID Created for a groupId
						SELECT TOP 1 *
						FROM Admin.Ticket
						WHERE TicketGroupId = @TicketGroupId
						ORDER BY TicketId;

					END
					ELSE
					IF @TicketTypeId = 3 OR @TicketTypeId = 6  --Logistics OR OfficialLogistics
					BEGIN
						--Insert data into Ticket table
						INSERT INTO [Admin].[Ticket]
								  (
									   [CategoryElementId]
									  ,[StartDate]
									  ,[EndDate]
									  ,[Status]
									  ,[TicketTypeId]
									  ,[OwnerId]
									  ,[NodeId]
									  ,[ScenarioTypeId]
									  ,[CreatedBy]
									  ,[CreatedDate]
								  )  
						SELECT    @CategoryElementId	AS [CategoryElementId]
								 ,@StartDate			AS [StartTime]
								 ,@EndDate				AS [EndTime]
								 ,1						AS [Status]		-- Initially setting the Status in Progress
								 ,@TicketTypeId			AS [TicketTypeId]
								 ,@OwnerId				AS [OwnerId]
								 ,@NodeId				AS [NodeId]
								 ,@ScenarioTypeId		AS [ScenarioTypeId]
								 ,@UserId				AS [CreatedBy]
								 ,@TodaysDateTime		AS [CreatedDate]

						--Fetch the identity Value Of TicketID inserted in table Ticket
						SELECT @Out_TicketId = SCOPE_IDENTITY();

						--Returning the Entire Column Set for the ticket Created
						SELECT *
						FROM Admin.Ticket 
						WHERE TicketId = @Out_TicketId

					END
					IF @TicketTypeId = 7  --LogisticMovements
					BEGIN
						
						--update to canceled the logistic movements
						UPDATE LM 
						SET StatusProcessId = 8
						FROM [Offchain].LogisticMovement LM
						WHERE LogisticMovementId IN
						(SELECT FLM.[Key] AS 'logisticMovementId' FROM @FailedLogisticsMovements FLM)
						
						--Insert data into Ticket table
						INSERT INTO [Admin].[Ticket]
								  (
									   [CategoryElementId]
									  ,[StartDate]
									  ,[EndDate]
									  ,[Status]
									  ,[TicketTypeId]
									  ,[OwnerId]
									  ,[NodeId]
									  ,[ScenarioTypeId]
									  ,[CreatedBy]
									  ,[CreatedDate]
								  )  
						SELECT    @CategoryElementId	AS [CategoryElementId]
								 ,@StartDate			AS [StartTime]
								 ,@EndDate				AS [EndTime]
								 ,1						AS [Status]		-- Initially setting the Status in Progress
								 ,@TicketTypeId			AS [TicketTypeId]
								 ,@OwnerId				AS [OwnerId]
								 ,@NodeId				AS [NodeId]
								 ,@ScenarioTypeId		AS [ScenarioTypeId]
								 ,@UserId				AS [CreatedBy]
								 ,@TodaysDateTime		AS [CreatedDate]

						--Fetch the identity Value Of TicketID inserted in table Ticket
						SELECT @Out_TicketId = SCOPE_IDENTITY();

						--Insert related nodes to ticket
						INSERT INTO [Admin].[TicketNode]([TicketId]
													   ,[NodeId]
													   ,[CreatedBy]
													   ,[CreatedDate])
						SELECT @Out_TicketId, LN.NodeId, @UserId, @TodaysDateTime FROM @LogisticNodes LN

						--Returning the Entire Column Set for the ticket Created
						SELECT *
						FROM Admin.Ticket 
						WHERE TicketId = @Out_TicketId

					END
					ELSE
					IF @TicketTypeId = 1 --Cutoff
					BEGIN

						IF OBJECT_ID('tempdb..#TempNodes')IS NOT NULL
						DROP TABLE #TempNodes

					   SELECT NT.NodeId
							 ,CASE WHEN @StartDate < NT.StartDate 
								   THEN NT.StartDate 
								   WHEN @StartDate > NT.StartDate 
								   THEN @StartDate 
								   WHEN @StartDate = NT.StartDate 
								   THEN NT.StartDate 
								   END StartDateConsidered
							,CASE WHEN @EndDate < NT.EndDate 
								  THEN @EndDate
								  WHEN @EndDate = NT.EndDate 
								  THEN @EndDate 
								  WHEN @EndDate > NT.EndDate 
								  THEN NT.EndDate 
								  END EndDateConsidered
						INTO #TempNodes
						FROM
						(
							SELECT   NodeId
									,CAST(StartDate AS DATE) AS StartDate
									,CASE WHEN YEAR(EndDate) = '9999' 
										  THEN @EndDate 
										  ELSE CAST(EndDate  AS DATE)
										  END AS EndDate
							FROM Admin.NodeTag NT
							WHERE ElementId = @CategoryElementId
							AND CASE WHEN @StartDate < NT.StartDate 
									 THEN NT.StartDate 
									 WHEN @StartDate > NT.StartDate 
									 THEN @StartDate 
									 WHEN @StartDate = NT.StartDate 
									 THEN NT.StartDate 
							END >= NT.StartDate
							AND CASE WHEN @EndDate < NT.EndDate 
									 THEN @EndDate
									 WHEN @EndDate = NT.EndDate 
									 THEN @EndDate 
									 WHEN @EndDate > NT.EndDate 
									 THEN NT.EndDate 
									 END <= NT.EndDate
						)NT

           
						--Insert data into Ticket table
						INSERT INTO [Admin].[Ticket]
								  (
									   [CategoryElementId]
									  ,[StartDate]
									  ,[EndDate]
									  ,[Status]
									  ,[TicketTypeId]
									  ,[CreatedBy] 
									  ,[CreatedDate]
								  )  
						SELECT    @CategoryElementId	AS [CategoryElementId]
								 ,@StartDate			AS [StartTime]
								 ,@EndDate				AS [EndTime]
								 ,1						AS [Status]
								 ,@TicketTypeId			AS [TicketTypeId]
								 ,@UserId				AS [CreatedBy] 
								 ,@TodaysDateTime		AS [CreatedDate]

						--Fetch the identity Value Of TicketID inserted in table Ticket
						SELECT @Out_TicketId = SCOPE_IDENTITY();

						--Returning the Entire Column Set for the ticket Created
						SELECT *
						FROM Admin.Ticket 
						WHERE TicketId = @Out_TicketId

						--Updating Table(PendingTransaction, Movement, UnbalanceComment) for Columns(TicketId, LastModifiedBy, LastModifiedDate) Based on StartDate and EndDate Parameters 
						UPDATE PT
						SET   TicketId             = @Out_TicketId
							 ,LastModifiedBy       = @UserId             
							 ,LastModifiedDate     = @TodaysDateTime     
						FROM [Admin].[PendingTransaction] PT
						INNER JOIN [Admin].[PendingTransactionError] PTE
						ON PT.[TransactionId] = PTE.[TransactionId]
						WHERE PTE.SessionId = @SessionId


						SELECT UnbalanceId
						INTO #TempUpdate3
						FROM [Admin].[UnbalanceComment] UC (NOLOCK)
						WHERE UC.SessionId = @SessionId
						AND UC.SegmentId = @CategoryElementId

						UPDATE UC
						SET   TicketId             = @Out_TicketId    
						FROM  [Admin].[UnbalanceComment] UC
						JOIN #TempUpdate3 TempUC
						ON UC.UnbalanceId=TempUC.UnbalanceId

						UPDATE MOV
						SET   TicketId             = @Out_TicketId
							 ,LastModifiedBy       = @UserId             
							 ,LastModifiedDate     = @TodaysDateTime     
						FROM [Offchain].[Movement] MOV
						INNER JOIN [Admin].[SapTracking] ST
						ON MOV.[MovementTransactionId] = ST.[MovementTransactionId]
						WHERE ST.SessionId = @SessionId
						
						SELECT  InvPrd.UncertaintyPercentage AS Inv_UncertaintyPercentage
							   ,sp.[UncertaintyPercentage]   AS Slp_UncertaintyPercentage
							   ,InvPrd.InventoryProductId
						INTO #TempUpdate1
						FROM OffChain.[InventoryProduct] InvPrd (NOLOCK)
						INNER JOIN #TempNodes TempNode
						ON InvPrd.NodeId = TempNode.NodeId
						AND [InvPrd].[InventoryDate] >= DATEADD(DAY,-1,TempNode.StartDateConsidered) 
						AND [InvPrd].[InventoryDate] <= TempNode.EndDateConsidered
						AND InvPrd.ScenarioId = 1 AND InvPrd.SegmentId = @CategoryElementId 
						INNER JOIN [Admin].[StorageLocationProduct] sp
						ON sp.ProductId = InvPrd.ProductId
						INNER JOIN [Admin].[NodeStorageLocation] ns 
						ON ns.NodeStorageLocationId = sp.NodeStorageLocationId 
						AND ns.NodeId = InvPrd.NodeId						
						
						--Update Inventory Table for (TicketId, LastModifiedBy, LastModifiedDate) Based on Ticket Table Column
						UPDATE  InvPrd
						SET     InvPrd.TicketId				 = @Out_TicketId
							   ,InvPrd.LastModifiedBy        = @UserId             
							   ,InvPrd.LastModifiedDate      = @TodaysDateTime
							   ,InvPrd.UncertaintyPercentage = COALESCE(InvPrd.UncertaintyPercentage,TempInv.Inv_UncertaintyPercentage,slp_UncertaintyPercentage,@UncertaintyPercentage)
						FROM OffChain.[InventoryProduct] InvPrd
						JOIN #TempUpdate1 TempInv
						ON InvPrd.InventoryProductId=TempInv.InventoryProductId
						LEFT JOIN @FirstTimeNodes FTN
                        ON InvPrd.NodeId = FTN.NodeId
                        WHERE [InvPrd].[InventoryDate] <> DATEADD(DAY, -1, @StartDate)
                        OR ([InvPrd].[InventoryDate] = DATEADD(DAY, -1, @StartDate) AND FTN.NodeId IS NOT NULL)
                        OR ([InvPrd].[InventoryDate] = DATEADD(DAY, -1, @StartDate) AND FTN.NodeId IS NULL AND InvPrd.TicketId IS NOT NULL)


						SELECT mov.UncertaintyPercentage as Mov_UncertaintyPercentage, 
							   ncp.UncertaintyPercentage as Ncp_UncertaintyPercentage, 
							   Mov.IsTransferPoint, 
							   Mov.MovementTransactionId
						INTO #TempUpdate2
                        FROM OffChain.[Movement] mov (NOLOCK)
                        LEFT JOIN OffChain.[MovementSource] ms (NOLOCK)
                        ON mov.MovementTransactionId = ms.MovementTransactionId
                        LEFT JOIN OffChain.[MovementDestination] md (NOLOCK)
                        ON  mov.MovementTransactionId = md.MovementTransactionId
                        INNER JOIN #TempNodes TempNode
                        ON (ms.SourceNodeId  = TempNode.NodeId OR md.DestinationNodeId = TempNode.NodeId)
                        AND mov.OperationalDate  >= TempNode.StartDateConsidered 
                        AND   mov.OperationalDate  <= TempNode.EndDateConsidered
                        LEFT JOIN [Admin].[NodeConnection] nc
                        ON ms.SourceNodeId = nc.SourceNodeId 
                        AND md.DestinationNodeId = nc.DestinationNodeId
                        LEFT JOIN [Admin].[NodeConnectionProduct] ncp
                        ON ncp.NodeConnectionId = nc.NodeConnectionId
                        AND Ncp.ProductId = ms.SourceProductId
						WHERE mov.VariableTypeId IS NULL
						AND mov.ScenarioId = 1

						--Update Movement Table for (TicketId, LastModifiedBy, LastModifiedDate) Based on Ticket Table Column
						UPDATE  mov
						SET     mov.TicketId			  = @Out_TicketId
							   ,mov.LastModifiedBy        = @UserId             
							   ,mov.LastModifiedDate      = @TodaysDateTime
							   ,Mov.UncertaintyPercentage = COALESCE(Mov.UncertaintyPercentage,TempMov.mov_UncertaintyPercentage,ncp_UncertaintyPercentage,@UncertaintyPercentage)
						FROM OffChain.[Movement] mov
						JOIN #TempUpdate2 TempMov
						ON mov.MovementTransactionId=TempMov.MovementTransactionId
						WHERE ((Mov.IsTransferPoint = 0 OR Mov.IsTransferPoint IS NULL)
						AND Mov.SegmentId = @CategoryElementId)


						UPDATE  mov
						SET     mov.TicketId			  = @Out_TicketId
							   ,mov.LastModifiedBy        = @UserId             
							   ,mov.LastModifiedDate      = @TodaysDateTime
							   ,Mov.UncertaintyPercentage = COALESCE(Mov.UncertaintyPercentage,TempMov.mov_UncertaintyPercentage,ncp_UncertaintyPercentage,@UncertaintyPercentage)
						FROM OffChain.[Movement] mov
						JOIN #TempUpdate2 TempMov
						ON mov.MovementTransactionId=TempMov.MovementTransactionId
						LEFT JOIN Admin.SapTracking SAP
                        ON Mov.MovementTransactionId = SAP.MovementTransactionId
						WHERE   (SAP.Comment IS NOT NULL OR (Mov.GlobalMovementId IS NOT NULL 
															 AND Mov.IsOfficial = 1)
															 )

						
					END
					ELSE
					IF @TicketTypeId = 4 --Delta
					BEGIN
						--Insert data into Ticket table
						INSERT INTO [Admin].[Ticket]
								  (
									   [CategoryElementId]
									  ,[StartDate]
									  ,[EndDate]
									  ,[Status]
									  ,[TicketTypeId]
									  ,[OwnerId]
									  ,[NodeId]
									  ,[CreatedBy]
									  ,[CreatedDate]
								  )  
						SELECT    @CategoryElementId	AS [CategoryElementId]
								 ,@StartDate			AS [StartTime]
								 ,@EndDate				AS [EndTime]
								 ,1						AS [Status]		-- Initially setting the Status in Progress
								 ,@TicketTypeId			AS [TicketTypeId]
								 ,NULL					AS [OwnerId]
								 ,NULL					AS [NodeId]
								 ,@UserId				AS [CreatedBy] 
								 ,@TodaysDateTime		AS [CreatedDate]

						--Fetch the identity Value Of TicketID inserted in table Ticket
						SELECT @Out_TicketId = SCOPE_IDENTITY();

						--Returning the Entire Column Set for the ticket Created
						SELECT *
						FROM Admin.Ticket 
						WHERE TicketId = @Out_TicketId

					END

					ELSE
					IF @TicketTypeId = 5 --Official Delta
                    BEGIN      
						--Insert data into Ticket table
                        INSERT INTO [Admin].[Ticket]
                             (
                                [CategoryElementId]
                               ,[StartDate]
                               ,[EndDate]
                               ,[Status]
                               ,[TicketTypeId]
							   ,[TicketGroupId]
                               ,[OwnerId]
                               ,[NodeId]
                               ,[CreatedBy]
                               ,[CreatedDate]
                             )  
                        SELECT ElementId                AS [CategoryElementId]
                              ,@StartDate               AS [StartTime]
                              ,@EndDate                 AS [EndTime]
                              ,1                        AS [Status]         -- Initially setting the Status in Progress
                              ,@TicketTypeId            AS [TicketTypeId]
							  ,@TicketGroupId		    AS [TicketGroupId]
                              ,NULL                     AS [OwnerId]
                              ,NULL                     AS [NodeId]
                              ,@UserId                  AS [CreatedBy]
                              ,@TodaysDateTime			AS [CreatedDate]
                        FROM Admin.CategoryElement CE
                        WHERE CategoryId = 2 AND IsActive = 1
                        AND (@CategoryElementId = 0 OR ElementId = @CategoryElementId)
						
						SELECT TicketId, CategoryElementId
						FROM Admin.Ticket
						WHERE TicketGroupId = @TicketGroupId
						ORDER BY TicketId;
					END
			  END
			  ELSE
			  BEGIN
				RAISERROR ('Invalid CategoryElementId',16,1) 
			  END

             COMMIT TRANSACTION
       END TRY

       BEGIN CATCH
             IF @@TRANCOUNT > 0
                    ROLLBACK TRANSACTION;
             THROW
       END CATCH
END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to Modify Data into Ticket, UnBalance, Inventory, Movement, PendingTransaction and PendingTransactionError tables for a given CategoryElementId, StartDate, EndDate, UserId ,TicketTypeId ,PendingTransactionErrorMessagesType ,UnbalanceType ,UncertaintyPercentage ,OwnerId ,NodeId ,TicketGroupId.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveTicket',
    @level2type = NULL,
    @level2name = NULL