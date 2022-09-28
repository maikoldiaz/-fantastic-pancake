/*-- ===============================================================================================================================
-- Author:			Microsoft
-- Created Date:	Mar-30-2020
-- Updated Date:    Apr-09-2020  -- Removed(BlockchainStatus = 1) 
					Modified to remove (MovementType Condition in Join Criteria)
-- <Description>:	This Procedure is used to get the cancellation movement details for the Excel file based on the Ticket Id. 
					Get the Data from Movement Whose Cancellation Movements does not exist(Should be done in 2 steps)
						 - TicketId matches with the input TicketId(Of Type 2)
						 - (OperationalDate-1) is in Between the Ticket's StartDate and EndDate.
					Step1: 153 Compared with 155
					Step2: 154 Compared with 156
					and the Combine Result for Step 1 and Step 2 --> Join with Ownership to Fetch Ownership related Fields
					</Description>
    --Category Element Data
	153---> Evacuación Entrada	
	154---> Evacuación Salida	
	155---> Anulación Entrada	
	156---> Anulación Salida 
-- ================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetCancellationMovementDetails] 
(
	@TicketId INT
)
AS 
  BEGIN   	
			IF OBJECT_ID('tempdb..#TempPreResultGetCancellationMovementDetails')IS NOT NULL
			DROP TABLE #TempPreResultGetCancellationMovementDetails

			SELECT  TempMov.[MovementTransactionId]                     AS MovementTransactionId
				   ,TempMov.[SourceNodeId]                              AS SourceNodeId
				   ,TempMov.[DestinationNodeId]                         AS DestinationNodeId 
				   ,TempMov.[SourceProductId]                           AS SourceProductId 
				   ,TempMov.[DestinationProductId]                      AS DestinationProductId
				   ,TempMov.[MessageTypeId]								AS MessageTypeId
				   ,TempMov.[MovementTypeId]                            AS MovementTypeId
				   ,TempMov.MovementTypeName                            AS MovementType
				   ,TempMov.[NetStandardVolume]                         AS NetVolume
				   ,CASE WHEN TempMov.MovementTypeId	= 153
						 THEN TempMov.DestinationProductTypeId
						 WHEN TempMov.MovementTypeId	= 154
						 THEN TempMov.SourceProductTypeId
						 ELSE NULL--155,156
						 END											AS ProductType
				   ,TempMov.MeasurementUnit								AS Unit
				   ,TempMov.[SegmentId]									AS SegmentId
				   ,CAST([Tic].[StartDate]	AS DATE)                    AS OperationalDate--This is Because the cancellation Movement Should be Generated for the Ticket
				   ,Own.OwnerId											AS OwnerId				
				   ,Own.OwnershipPercentage								AS OwnershipPercentage
				   ,Own.OwnershipVolume									AS OwnershipVolume
				   ,Own.AppliedRule										AS AppliedRule
				   ,Own.RuleVersion										AS RuleVersion
			INTO #TempPreResultGetCancellationMovementDetails
			FROM [Admin].[Ticket] Tic
			INNER JOIN Admin.view_MovementInformation TempMov 
			ON TempMov.SegmentId = Tic.CategoryElementId	
			LEFT JOIN Offchain.Ownership Own
			ON TempMov.[MovementTransactionId] = Own.[MovementTransactionId] 
			WHERE Tic.TicketId = @TicketId 
			-- AND TempMov.MovementTypeId IN (153,154,155,156)--(Evacuación Entrada,Evacuación Salida,Anulación Entrada,Anulación Salida)
			AND (TempMov.MovementTypeId IN (153,154) OR (TempMov.MovementTypeId IN (155,156) AND TempMov.SourceSystemId IS NOT NULL))
			AND CAST(DATEADD(DAY,-1,[Tic].[StartDate]) AS DATE) = TempMov.[OperationalDate]
			AND Own.IsDeleted != 1


			SELECT  LftMov.MovementTransactionId
				   ,LftMov.SourceNodeId
				   ,LftMov.DestinationNodeId 
				   ,LftMov.SourceProductId 
				   ,LftMov.DestinationProductId
				   ,LftMov.MessageTypeId
				   ,LftMov.MovementTypeId
				   ,LftMov.MovementType
				   ,LftMov.NetVolume
				   ,LftMov.ProductType
				   ,LftMov.Unit
				   ,LftMov.SegmentId
				   ,LftMov.OperationalDate
				   ,LftMov.OwnerId				
				   ,LftMov.OwnershipPercentage
				   ,LftMov.OwnershipVolume
				   ,LftMov.AppliedRule
				   ,LftMov.RuleVersion			
			FROM #TempPreResultGetCancellationMovementDetails LftMov
			LEFT JOIN #TempPreResultGetCancellationMovementDetails RgtMov
			ON	LftMov.DestinationNodeId	  = RgtMov.SourceNodeId
			AND LftMov.DestinationProductId = RgtMov.SourceProductId
			AND LftMov.NetVolume		  = RgtMov.NetVolume
			AND LftMov.Unit				  = RgtMov.Unit
			AND LftMov.OperationalDate    = RgtMov.OperationalDate
			AND LftMov.OwnerId			  = RgtMov.OwnerId	
			AND LftMov.OwnershipPercentage= RgtMov.OwnershipPercentage
			AND LftMov.OwnershipVolume	  = RgtMov.OwnershipVolume
			--AND LftMov.MovementType       = RgtMov.MovementType
			AND RgtMov.MovementTypeId   IN (155)--Anulación Entrada
			WHERE LftMov.MovementTypeId IN (153)--Evacuación Entrada	
			AND   RgtMov.SourceNodeId IS NULL
			UNION
			SELECT  LftMov.MovementTransactionId
				   ,LftMov.SourceNodeId
				   ,LftMov.DestinationNodeId 
				   ,LftMov.SourceProductId 
				   ,LftMov.DestinationProductId
				   ,LftMov.MessageTypeId
				   ,LftMov.MovementTypeId
				   ,LftMov.MovementType
				   ,LftMov.NetVolume
				   ,LftMov.ProductType
				   ,LftMov.Unit
				   ,LftMov.SegmentId
				   ,LftMov.OperationalDate
				   ,LftMov.OwnerId				
				   ,LftMov.OwnershipPercentage
				   ,LftMov.OwnershipVolume
				   ,LftMov.AppliedRule
				   ,LftMov.RuleVersion
			FROM #TempPreResultGetCancellationMovementDetails LftMov
			LEFT JOIN #TempPreResultGetCancellationMovementDetails RgtMov
			ON	LftMov.SourceNodeId         = RgtMov.DestinationNodeId	  
			AND LftMov.SourceProductId      = RgtMov.DestinationProductId 
			AND LftMov.NetVolume		  = RgtMov.NetVolume
			AND LftMov.Unit				  = RgtMov.Unit
			AND LftMov.OperationalDate    = RgtMov.OperationalDate
			AND LftMov.OwnerId			  = RgtMov.OwnerId	
			AND LftMov.OwnershipPercentage= RgtMov.OwnershipPercentage
			AND LftMov.OwnershipVolume	  = RgtMov.OwnershipVolume
			--AND LftMov.MovementType       = RgtMov.MovementType	
			AND RgtMov.MovementTypeId   IN (156)--Anulación Salida
			WHERE LftMov.MovementTypeId IN (154)--Evacuación Salida
			AND   RgtMov.DestinationNodeId IS NULL
  
  END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get the cancellation movement details for the Excel file based on the Ticket Id. 
	Get the Data from Movement Whose Cancellation Movements does not exist(Should be done in 2 steps)
		- TicketId matches with the input TicketId(Of Type 2)
		- (OperationalDate-1) is in Between the Tickets StartDate and EndDate.
	Step1: 153 Compared with 155
	Step2: 154 Compared with 156
	and the Combine Result for Step 1 and Step 2 --> Join with Ownership to Fetch Ownership related Fields',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetCancellationMovementDetails',
    @level2type = NULL,
    @level2name = NULL