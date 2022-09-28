/*-- ========================================================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jun-18-2020
-- Updated Date:	
-- <Description>:   This Procedure is used to update comments after the operational cutoff create in SapTracking, UnbalanceComment, PendingTransactionError table.
-- ========================================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_UpdateCutOffComment]
(   
	   @SessionId											 NVARCHAR(50) NULL,
	   @Type												 NVARCHAR(255) NULL,
	   @UserIdValue											 NVARCHAR(260) NULL,
	   @SegmentId											 NVARCHAR(50) NULL,
	   @PendingTransactionErrorMessages						 Admin.PendingTransactionErrorMessagesType READONLY,
       @UnbalancesMessages                                   Admin.UnbalanceType READONLY,
	   @SapTrackingMessages									 Admin.SapTrackingType READONLY
)
AS
BEGIN
	BEGIN TRY
             BEGIN TRANSACTION
					DECLARE @TodaysDateTime     DATETIME = Admin.udf_GetTrueDate()

					-- Clear Orphaned record
					DELETE FROM [Admin].[UnbalanceComment] WHERE SegmentId = CAST(@SegmentId AS INT) AND SessionId != @SessionId AND TicketId IS NULL

					IF @Type = 1 -- Error
					BEGIN
						UPDATE PTE
						SET  Comment             = TVar.Comment
							,SessionId			 = @SessionId
							,LastModifiedBy      = @UserIdValue 
							,LastModifiedDate    = @TodaysDateTime     
						FROM [Admin].[PendingTransactionError] PTE
						INNER JOIN @PendingTransactionErrorMessages TVar
						ON PTE.[ErrorId] = Tvar.[ErrorId]
					END
					ELSE
					IF @Type = 2 --Unalances
					BEGIN
						INSERT INTO [Admin].[UnbalanceComment]
								  (
									 [NodeId]
									 ,[ProductId]
									 ,[Unbalance]
									 ,[Units]
									 ,[UnbalancePercentage]
									 ,[ControlLimit]
									 ,[Comment]
									 ,[SessionId]
									 ,[SegmentId]
									 ,[Status]
									 ,[CreatedBy]
									 ,[CreatedDate]
								  )
						SELECT        [NodeId]
									 ,[ProductId]
									 ,[Unbalance]
									 ,[Units]
									 ,[UnbalancePercentage]
									 ,[ControlLimit]
									 ,[Comment]
									 ,@SessionId AS [SessionId]
									 ,CAST(@SegmentId AS INT) AS [SegmentId]
									 ,0 AS [Status] 
									 ,@UserIdValue AS [CreatedBy]
									 ,@TodaysDateTime AS [CreatedDate]
						FROM @UnbalancesMessages
					END
					ELSE
					IF @Type = 3 -- SapTracking
					BEGIN
						UPDATE ST
						SET  Comment             = STVar.Comment
							,SessionId			 = @SessionId     
						FROM [Admin].[SapTracking] ST
						INNER JOIN @SapTrackingMessages STVar
						ON ST.[SapTrackingId] = STvar.[SapTrackingId]

						UPDATE Mov
						SET IsOfficial			 = 1
						FROM [Offchain].[Movement] Mov
						INNER JOIN @SapTrackingMessages STVar
						ON STVar.MovementTransactionId = Mov.MovementTransactionId
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
    @value = N'This Procedure is used to update comments after the operational cutoff create in SapTracking, UnbalanceComment, PendingTransactionError table.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_UpdateCutOffComment',
    @level2type = NULL,
    @level2name = NULL