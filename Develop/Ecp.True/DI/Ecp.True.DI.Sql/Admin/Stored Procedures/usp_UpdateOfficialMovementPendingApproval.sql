/*-- ========================================================================================================================================================================
-- Author:		 Microsoft
-- Create date:  05-Jul-2020
-- Updated Date: 12-Oct-2020 Added filter for official movements
-- Description:	 Update the pending Approval for the Official Movements

-- ========================================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_UpdateOfficialMovementPendingApproval]

	@DeltaNodeId int,
	@UserId nvarchar(520) = 'system'
AS
BEGIN
	SET NOCOUNT ON;

	IF OBJECT_ID('tempdb..#TempMovementTransaction')IS NOT NULL
    DROP TABLE #TempMovementTransaction

	IF OBJECT_ID('tempdb..#Ticket')IS NOT NULL
    DROP TABLE #Ticket

		BEGIN TRY
	         BEGIN TRANSACTION

			 DECLARE @NodeId     int,
			         @SegmentId  int,
					 @StartDate  date,
					 @EndDate    date
				 IF EXISTS(SELECT 1 FROM [Admin].[DeltaNode] WHERE DeltaNodeId = @DeltaNodeId)
				 BEGIN

				 -- set NodeId from [Admin].[DeltaNode]
				  SELECT @NodeId = DN.NodeId, @SegmentId = TC.CategoryElementId, @StartDate = TC.StartDate, @EndDate = TC.EndDate
		          FROM Admin.DeltaNode DN
				  INNER JOIN Admin.Ticket TC
				  ON TC.TicketId = DN.TicketId
		          WHERE DN.DeltaNodeId = @DeltaNodeId

				  -- get all official delta tickets for the period
				  SELECT TicketId
				  INTO #Ticket
				  FROM Admin.Ticket TC
				  WHERE TC.StartDate = @StartDate AND TC.EndDate = @EndDate
				  AND TicketTypeId = 5 AND CategoryElementId = @SegmentId AND Status != 2

                  SELECT MOV.MovementTransactionId
                  INTO #TempMovementTransaction
                  FROM Offchain.Movement MOV
                  LEFT JOIN Offchain.MovementSource MS
                  ON MS.MovementTransactionId = MOV.MovementTransactionId
                  AND MOV.ScenarioId = 2 AND MOV.OfficialDeltaMessageTypeId IS NOT NULL
				  AND MOV.OfficialDeltaTicketId in (SELECT TicketId FROM #Ticket)
                  AND MOV.PendingApproval = 1
                  LEFT JOIN Offchain.MovementDestination MD
                  ON MD.MovementTransactionId = MOV.MovementTransactionId
                  AND MOV.ScenarioId = 2 AND MOV.OfficialDeltaMessageTypeId IS NOT NULL
				  AND MOV.OfficialDeltaTicketId in (SELECT TicketId FROM #Ticket)
				  AND MOV.PendingApproval = 1
                  WHERE (MS.SourceNodeId = @NodeId OR MD.DestinationNodeId = @NodeId)

				  UPDATE    Offchain.Movement
                  SET       PendingApproval = 0,
                  LastModifiedBy = @UserId,
                  LastModifiedDate = Admin.udf_GETTRUEDATE()
                  FROM    Offchain.Movement Mov
                  INNER JOIN #TempMovementTransaction tmpMov
                  ON    Mov.MovementTransactionId = tmpMov.MovementTransactionId

		          SELECT MovementTransactionId  
	              FROM	#TempMovementTransaction 
				 END
			     ELSE
				 BEGIN
					RAISERROR ('Invalid DeltaNodeId',1,1) 
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
    @value = N'Update the pending Approval for the Official Movements',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_UpdateOfficialMovementPendingApproval',
    @level2type = NULL,
    @level2name = NULL

 
