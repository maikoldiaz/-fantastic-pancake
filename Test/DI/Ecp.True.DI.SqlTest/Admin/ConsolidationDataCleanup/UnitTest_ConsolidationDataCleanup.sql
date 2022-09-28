/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created Date:    July-08-2020
-- Updated Date:
-- <Description>:   Unit Test the procedure by providing TicketId. </Description>
-- EXEC [Admin].[usp_ConsolidationDataCleanup] 6 , ''
-- ==============================================================================================================================*/
DECLARE  @TicketId        INT =24143,
          @ErrorMessage   NVARCHAR(MAX)

EXEC [Admin].[usp_ConsolidationDataCleanup]  @TicketId,        
											 @ErrorMessage   




--INSERT INTO admin.deltaerror (MovementTransactionId, InventoryProductId, TicketId, ErrorMessage, Createdby)
--VALUES(23584, 9419, 26924, 'Error test', 'trueadmin')

--select * from admin.view_MovementInformation
--where MovementTransactionId=23584

--select * from admin.view_InventoryInformation
--where TicketId=26924

--select * from admin.DeltaError

--select * from admin.Ticket
--where TicketId=24143

--SELECT MovementTransactionId,  TicketId, OfficialDeltaMessageTypeId						  					  
--FROM  Offchain.Movement
--WHERE OfficialDeltaMessageTypeId IS NOT NULL
--  and TicketId=24143

--UPDATE offchain.Movement
--SET OfficialDeltaTicketId = 24143,
--OfficialDeltaMessageTypeId=4
--where TicketId = 24143
--and MovementTransactionId=18901


--SELECT MovementTransactionId FROM [Admin].DeltaError	
--INTERSECT
--SELECT MovementTransactionId FROM [Admin].Attribute	
--INTERSECT
--SELECT MovementTransactionId FROM [Offchain].Movement	
--INTERSECT
--SELECT MovementTransactionId FROM [Offchain].MovementDestination
--INTERSECT
--SELECT MovementTransactionId FROM [Offchain].MovementPeriod	
--INTERSECT
--SELECT MovementTransactionId FROM [Offchain].MovementSource	
--INTERSECT
--SELECT MovementTransactionId FROM [Offchain].Owner	
--INTERSECT
--SELECT MovementTransactionId FROM [Offchain].Ownership