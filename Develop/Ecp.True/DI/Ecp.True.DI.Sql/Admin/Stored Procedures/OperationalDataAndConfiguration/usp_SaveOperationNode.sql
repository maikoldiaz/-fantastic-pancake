/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:	Nov-26-2019
-- Updated Date:	Mar-20-2020
--					Jun-30-2020	Added Filter (Check Ticket startDate Between NodeTag Start And NodeTag EndDate)
-- <Description>:	This Procedure is used to Seed Operational Node data into Admin.OwnershipNode table based on the Ticket Id.  </Description>
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveOperationNode]
(       
       @TicketId	            INT,
	   @NodeList				[Admin].[NodeListType]	READONLY
)
AS
BEGIN
       BEGIN TRY
             BEGIN TRANSACTION

			 DECLARE @TodaysDateTime     DATETIME = Admin.udf_GetTrueDate()

			 IF EXISTS(SELECT 1 FROM [Admin].[Ticket] WHERE TicketId = @TicketId)
				 BEGIN
				 -- Seed data into [Admin].[OwnershipNode] based on TicketId
					INSERT INTO [Admin].[OwnershipNode]
					(
						 [TicketId]
						,[NodeId]
						,[Status]
						,[OwnershipStatusId]
						,[CreatedBy]
						,[CreatedDate]
						,[LastModifiedBy]
						,[LastModifiedDate]
					)
					SELECT	Tic.TicketId				As [TicketId]
							,NT.NodeId					As [NodeId]
							,1							As [Status]		-- Default Status to be set as Processing.
							,1							As [OwnershipStatusId]	-- Default Enviado/Sent
							,Tic.CreatedBy				As [CreatedBy]
							,@TodaysDateTime			As CreatedDate
							,NULL						As LastModifiedBy
							,NULL						As LastModifiedDate
					From [Admin].[NodeTag] NT
					JOIN Admin.Ticket Tic
					ON NT.ElementId = Tic.CategoryElementId
					LEFT JOIN [Admin].[OwnershipNode] OwnNode
					ON  OwnNode.TicketId = Tic.TicketId
					AND OwnNode.NodeId = NT.NodeId
					WHERE Tic.TicketId = @TicketId
					AND Tic.StartDate BETWEEN NT.StartDate AND NT.EndDate
					AND NT.NodeId IN (SELECT NodeId FROM @NodeList)
					AND OwnNode.NodeId  IS NULL
			
				 END
			 ELSE
				 BEGIN
					RAISERROR ('Invalid TicketId',1,1) 
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
    @value = N'This Procedure is used to Seed Operational Node data into Admin.OwnershipNode table based on the Ticket Id.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveOperationNode',
    @level2type = NULL,
    @level2name = NULL