DECLARE
	   @TicketId				INT = 32055,
	   @NodeList				[Admin].[NodeListType]	

	  INSERT INTO @NodeList
	   VALUES (47292)
	   INSERT INTO @NodeList
	   VALUES (47268)
	    INSERT INTO @NodeList
	   VALUES (47269)
	   INSERT INTO @NodeList
	   VALUES (47291)

EXEC [Admin].[usp_UpdateNodeAndGetDeltaMovements] @TicketId, @NodeList

--OUTPUT
--MovementTransactionID
--141252
--141254
--141255
--141258
--141260
--141262
--141264

Select * from Admin.DeltaNodeApprovalHistory (Nolock)
Where TicketId = 32054								--Old ticket with same segment, startdate and enddate

--DeltaNodeApprovalHistoryId	TicketId	NodeId	Date						Status	CreatedBy	CreatedDate				LastModifiedBy	LastModifiedDate
--63							32054		47292	2020-08-10 06:36:17.220		10		System		2020-08-10 06:37:28.380		NULL				NULL
--64							32054		47268	2020-08-10 06:36:17.220		10		System		2020-08-10 06:37:28.380		NULL				NULL
--65							32054		47269	2020-08-10 06:36:17.220		12		System		2020-08-10 06:37:28.380		NULL				NULL
--66							32054		47291	2020-08-10 06:36:17.220		12		System		2020-08-10 06:37:28.380		NULL				NULL


	   --UPDATE Admin.DeltaNode
	   --SET LastModifiedDate = getdate()
	   --WHERE NodeId IN (47292,47268,47269, 47291 )
	   --AND TicketId = 32054

	   --INSERT INTO Admin.DeltaNode (TicketId, NodeId, Status,CreatedBy)
	   --VALUES(32054, 47292, 10, 'trueadmin')

	   --INSERT INTO Admin.DeltaNode (TicketId, NodeId, Status,CreatedBy)
	   --VALUES(32054, 47268, 10, 'trueadmin')

	   --   INSERT INTO Admin.DeltaNode (TicketId, NodeId, Status,CreatedBy)
	   --VALUES(32054, 47269, 12, 'trueadmin')

	   --      INSERT INTO Admin.DeltaNode (TicketId, NodeId, Status,CreatedBy)
	   --VALUES(32054, 47291, 12, 'trueadmin')

	   --DECLARE
	   --@TicketId				INT = 32055, --32291,
	   --@NodeList				[Admin].[NodeListType],	
	   --@TicketStartDate DATE = '2020-06-01',
	   --@TicketEndDate DATE = '2020-06-30'

	   --INSERT INTO @NodeList
	   --VALUES (47292)
	   --INSERT INTO @NodeList
	   --VALUES (47268)
	    --INSERT INTO @NodeList
	   --VALUES (47269)
	   --INSERT INTO @NodeList
	   --VALUES (47291)

--SELECT  Mov.[MovementTransactionID]	AS [MovementTransactionID] --INTO #MovementsToDelete
--			,Mov.SegmentId, Mov.OperationalDate, Mov.SourceNodeId, Mov.DestinationNodeId
--			FROM Admin.view_MovementInformation Mov
--			WHERE ((Mov.OfficialDeltaMessageTypeId IN (1,2) AND Mov.IsSystemGenerated = 1 AND Mov.OfficialDeltaTicketId IS NOT NULL)  --OfficialInventoryDelta,ConsolidatedInventoryDelta
--					OR Mov.SourceSystemId = 189) --ManualInvOficial
--			AND Mov.SegmentId = 197665
--			AND (Mov.OperationalDate = @TicketEndDate
--			OR Mov.OperationalDate = DATEADD(DAY,-1,@TicketStartDate))	