﻿/*-- ===============================================================================================================================
-- Author:			Microsoft
-- Created Date:	Nov-22-2019
-- Updated Date:	Mar-20-2020
				    Apr-09-2020--> Modified the Code to Update Movement Details in MovementTable and Removed( BlockchainStatus = 1)
					Apr-14-2010 --> Removed OwnershipAnalytics references.
--					Jun-17-2020	   Modified Code to Get the OWnerShip Columns In the final Stage as per the bug 55547 after discussion with leads
--					Jun-24-2020	   Modified Code Of adding(AND TempMov.TicketId IS NOT NULL) for the bug 57068 
--					Jun-26-2020	   Modified the Code for TicketID Filter and data Fetching Logic 	
--					Jul-07-2020	   Modified Procedures for PBI #32146(Modified MovementTypeId)
--					Jul-10-2020	   Modified Procedures for PBI #32146(Added Delta TicketId Logic)
--					Aug-06-2020	   Modified Procedure for Performance Reasons
--					Aug-06-2020	   Removed Cast from Owner.OwnerId
--					May-13-2021	   Added MovementId and MovementTransactionId
-- <Description>:	This Procedure is used to get the Movement details for the Excel file based on the Ticket  Id. </Description>
-- ================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetMovementDetails]
(
	@TicketId INT
)
AS 
   BEGIN 
		IF OBJECT_ID('tempdb..#TempGetMovementDetails')IS NOT NULL
		DROP TABLE #TempGetMovementDetails

		DECLARE @StartDate		DATE,
				@EndDate		DATE,
			    @CutOffTicketId INT,
				@SegmentId INT

		--StartDate and EndDate of OWnerShipTicket
		SELECT @StartDate = StartDate
			 ,@EndDate = EndDate
			 ,@SegmentId = CategoryElementId
		FROM Admin.Ticket
		WHERE TicketId = @TicketId

		--Get the associated CutOffTicketID
		SELECT TOP 1  @CutOffTicketId = TicketId
		FROM Admin.Ticket
		WHERE TicketTypeId = 1
		AND Status = 0 
		AND @StartDate BETWEEN StartDate AND EndDate
		AND CategoryElementId = @SegmentId
		ORDER By TicketId DESC

 	    ;WITH CTE
		AS
		(			   	
			SELECT @TicketId															AS Ticket, 
				   [TempMov].[MovementTransactionId]									AS MovementTransactionId,
				   [TempMov].[MovementId]												AS MovementId, 
				   [TempMov].[OperationalDate]											AS OperationalDate, 
				   [TempMov].[SourceNodeId]												AS SourceNodeId,  
				   [TempMov].[DestinationNodeId]										AS DestinationNodeId,
				   [TempMov].[SourceProductId]											AS SourceProductId, 
				   [TempMov].[DestinationProductId]										AS DestinationProductId, 
				   [TempMov].[NetStandardVolume]										AS NetVolume, 
				   [TempMov].[MessageTypeId]											AS MessageTypeId,
				   [TempMov].[MovementTypeId]											AS MovementTypeId, 
				   [TempMov].[DeltaTicketId]											AS DeltaTicketId,
				   [TempMov].[SourceMovementTransactionId]								AS SourceMovementTransactionId,
				   [TempMov].[SourceInventoryProductId]									AS SourceInventoryProductId,
				   [TempMOv].[IsSystemGenerated]										AS IsSystemGenerated,
				   ROW_NUMBER() OVER( PARTITION BY  [TempMov].[MovementId]
									  ORDER BY [TempMov].[MovementTransactionId] DESC)  AS Rnum 
			FROM Admin.view_MovementInformation TempMov 		 
			WHERE  TempMov.TicketId = @CutOffTicketId 
			AND    [TempMov].[OperationalDate] BETWEEN @StartDate AND @EndDate
		)
		SELECT CTE.Ticket, 
			   CTE.MovementTransactionId,
			   CTE.MovementId,
			   CTE.OperationalDate, 
			   CTE.SourceNodeId, 
			   CTE.DestinationNodeId, 
			   CTE.SourceProductId,		
			   CTE.DeltaTicketId,
			   CTE.DestinationProductId,
			   CTE.NetVolume, 
			   CTE.MessageTypeId,			   
			   CTE.MovementTypeId, 
			   O.OwnerId   AS OwnerId, 
			   CatEle.Name AS Owner,
			   O.OwnershipValue, 
			   O.OwnershipValueUnit	AS OwnershipUnit,
			   CTE.SourceMovementTransactionId,
			   CTE.SourceInventoryProductId,
			   CTE.IsSystemGenerated
		INTO #TempGetMovementDetails
		FROM   CTE 
		LEFT JOIN [Offchain].[Owner] o 
		ON CTE.[MovementTransactionId] = [o].[MovementTransactionId]
		LEFT JOIN Admin.CategoryElement CatEle
	    ON o.OwnerId = CatEle.ElementId
		WHERE  Rnum = 1
		AND NetVolume > 0 	

		UPDATE Mov
		SET OwnershipTicketId = @TicketId
		FROM Offchain.Movement Mov
		INNER JOIN #TempGetMovementDetails MovDtls
		ON Mov.[MovementTransactionId] = MovDtls.MovementTransactionId


		--change usp_GetMovementDetails, if the movement is cancellation generated by operative delta, then movement type should annulation
		--CASE For the moveTran where [DeltaTicketId] is not nULL for this SourceMovementTranId Or SourceInventoryProductId  one of them is not null
		--[IsSystemGenerated] = 1 --> 
		--MovementType Is Cancellation in SubQ Annulation Table of Column AnnulationMovementTypeId THEN 'Annulation'
		--ELSE SAME END AS MovementTypeId
		SELECT Mov.Ticket, 
		       Mov.OwnerId,
			   Mov.MovementTransactionId,
			   Mov.MovementId,
			   Mov.SourceNodeId, 			   
			   Mov.DestinationNodeId,			    
			   Mov.SourceProductId,			   
			   Mov.DestinationProductId,
			   Mov.NetVolume,			   
			   Mov.OwnershipValue,
			   Mov.OwnershipUnit,			   			
			   CASE WHEN Mov.DeltaTicketId IS NOT NULL
					AND  Mov.IsSystemGenerated = 1
					AND  Ann.AnnulationMovementTypeId  IS NOT NULL
					THEN 'ANULACION' 
					ELSE CAST (MovementTypeId AS VARCHAR(10)) 
					END AS MovementTypeId,
			   Mov.MessageTypeId,			   
			   Mov.OperationalDate
		FROM #TempGetMovementDetails  Mov
		LEFT JOIN ADMin.Annulation Ann
		ON Mov.MovementTypeId = Ann.AnnulationMovementTypeId
  END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
							@value = N'This Procedure is used to get the Movement details for the Excel file based on the Ticket  Id.',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetMovementDetails',
							@level2type = NULL,
							@level2name = NULL