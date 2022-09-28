/*-- ===============================================================================================================================================
-- Author:           InterGrupo
-- Created Date:     May-06-2021
-- Updated Date:     Ago-10-2021 Add logistic movement validation
-- Updated Date:     Ago-17-2021 Change nodes and product to not null
-- Updated Date:     Sep-02-2021 Change validations for ScenarioType
-- <Description>:    This Procedure is used to get the Movement Sent to Sap. </Description>
-- ================================================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_GetMovementSendSapInformation]
		@ElementId		int 
		,@OwnerId	    int
		,@ScenarioId    int
		,@StartDate		DATE		  
		,@EndDate		DATE		  
		,@ExecutionId	NVARCHAR(250)
AS
BEGIN
	SET NOCOUNT ON

		--Borrar ejecuciones anteriores
	DELETE FROM [Admin].[MovementSendSapInformation]
	WHERE CreatedDate < (
	                     SELECT [Admin].[udf_GetTrueDate] ()-1
						 )
	   OR (    InputElement = @ElementId
	       AND ExecutionId = @ExecutionId
          )

	declare @TempMovementSendSap as table (	
	[SapStatus]				NVARCHAR (150)	 NOT NULL,
	[TypeOfMovement]	    NVARCHAR (150)	 NOT NULL,
	[SourceNode]			NVARCHAR (150)	 NULL,
	[DestinationNode]		NVARCHAR (150)	 NULL,
	[SourceProduct]			NVARCHAR (150)	 NULL,
	[DestinationProduct]	NVARCHAR (150)	 NULL,
	[StartDate]             DATETIME         NOT NULL,		 
	[EndDate]               DATETIME         NOT NULL,		 
	[OwnerName]				NVARCHAR (150)	 NOT NULL,
	[OwnershipVolume]		DECIMAL(18,2)	 NOT NULL,
	[MovementId]			VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
	[MovementTransactionId]	INT				 NOT NULL,
	[Scenario]				NVARCHAR (150)	 NOT NULL)             
			


if @ScenarioId = 1--'Operativo'
	begin 
			insert into @TempMovementSendSap
			select distinct 
				st.StatusType,
				MoveType.[Name] MovementTypeName,
				MovSrc.SourceNodeName,
				MovDest.DestinationNodeName,
				MovSrc.SourceProductName,
				MovDest.DestinationProductName,
				m.OperationalDate StartTime,
				m.OperationalDate EndTime,
				ce.[Description] [OwnerName],
				lm.OwnershipVolume,
				m.MovementID,
				m.MovementTransactionId,
				'Operativo'		
			from Offchain.LogisticMovement lm
			inner join Offchain.Movement m on lm.MovementTransactionId = m.MovementTransactionId AND m.ScenarioId = @ScenarioId
			LEFT JOIN (SELECT  
				   MovSrc.MovementTransactionId
				  ,MovSrc.SourceNodeId
				  ,SrcNd.[Name] AS SourceNodeName
				  ,MovSrc.SourceProductId		
				  ,SrcPrd.[Name] AS SourceProductName				  
				  ,MovSrc.SourceProductTypeId
				  ,MovSrc.SourceStorageLocationId				  
		   FROM [Offchain].[MovementSource] MovSrc
		   INNER JOIN [Admin].[Node] SrcNd
		   ON SrcNd.NodeId = MovSrc.SourceNodeId 
		   INNER JOIN [Admin].Product SrcPrd
		   ON SrcPrd.ProductId = MovSrc.SourceProductId) MovSrc 
		   ON m.MovementTransactionId = MovSrc.MovementTransactionId
			LEFT JOIN (SELECT  
			       MovDest.MovementTransactionId
				  ,MovDest.DestinationNodeId
				  ,DesNd.[Name] AS DestinationNodeName
				  ,MovDest.DestinationProductId		
				  ,DestPrd.[Name] AS DestinationProductName				  
				  ,MovDest.DestinationProductTypeId
				  ,MovDest.DestinationStorageLocationId				  
		   FROM [Offchain].[MovementDestination] MovDest
		   INNER JOIN [Admin].[Node] DesNd
		   ON DesNd.NodeId = MovDest.DestinationNodeId 
		   INNER JOIN [Admin].Product DestPrd
		   ON DestPrd.ProductId = MovDest.DestinationProductId) MovDest 
		   ON m.MovementTransactionId = MovDest.MovementTransactionId
			INNER JOIN admin.Ticket t on lm.TicketId = t.TicketId AND t.TicketTypeId = 7
			INNER JOIN admin.StatusType st on lm.StatusProcessId = st.StatusTypeId	
			INNER JOIN admin.[ScenarioType] sct on m.ScenarioId = sct.ScenarioTypeId 
			LEFT JOIN offchain.MovementPeriod mp on lm.MovementTransactionId = mp.MovementTransactionId
			INNER JOIN admin.CategoryElement ce on t.OwnerId = ce.elementid 
			LEFT JOIN [Admin].CategoryElement MoveType ON MoveType.ElementId = m.MovementTypeId
			INNER JOIN (		select lmm.MovementTransactionId,ti.OwnerId,max(lmm.CreatedDate) CreatedDate
								from Offchain.LogisticMovement lmm  
								INNER JOIN admin.Ticket ti on lmm.TicketId = ti.TicketId AND ti.TicketTypeId = 7
								group by lmm.MovementTransactionId,ti.OwnerId
						) itemsel
			on lm.MovementTransactionId = itemsel.MovementTransactionId AND lm.CreatedDate = itemsel.CreatedDate and t.OwnerId = itemsel.OwnerId
			where m.segmentid = @ElementId AND (t.OwnerId = @OwnerId OR @OwnerId is null)
			AND  m.OperationalDate BETWEEN @StartDate AND @EndDate
			AND t.ScenarioTypeId = @ScenarioId

			
	end

	if @ScenarioId = 2--'Oficial'
	begin 
			insert into @TempMovementSendSap
			select distinct 
				st.StatusType,
				MoveType.[Name] MovementTypeName,
				MovSrc.SourceNodeName,
				MovDest.DestinationNodeName,
				MovSrc.SourceProductName,
				MovDest.DestinationProductName,
				CASE WHEN mp.StartTime IS NULL THEN M.OperationalDate ELSE mp.StartTime END StartTime,
				CASE WHEN mp.EndTime IS NULL THEN M.OperationalDate ELSE mp.EndTime END EndTime,
				ce.[Description] [OwnerName],
				lm.OwnershipVolume,
				m.MovementID,
				m.MovementTransactionId,
				'Oficial'
			from Offchain.LogisticMovement lm
			INNER JOIN Offchain.Movement m on lm.MovementTransactionId = m.MovementTransactionId and m.ScenarioId = @ScenarioId
			LEFT JOIN (SELECT  
			       MovSrc.MovementTransactionId
				  ,MovSrc.SourceNodeId
				  ,SrcNd.[Name] AS SourceNodeName
				  ,MovSrc.SourceProductId		
				  ,SrcPrd.[Name] AS SourceProductName				  
				  ,MovSrc.SourceProductTypeId
				  ,MovSrc.SourceStorageLocationId				  
		   FROM [Offchain].[MovementSource] MovSrc
		   INNER JOIN [Admin].[Node] SrcNd
		   ON SrcNd.NodeId = MovSrc.SourceNodeId 
		   INNER JOIN [Admin].Product SrcPrd
		   ON SrcPrd.ProductId = MovSrc.SourceProductId) MovSrc 
		   ON m.MovementTransactionId = MovSrc.MovementTransactionId
		  LEFT JOIN (SELECT  
			       MovDest.MovementTransactionId
				  ,MovDest.DestinationNodeId
				  ,DesNd.[Name] AS DestinationNodeName
				  ,MovDest.DestinationProductId		
				  ,DestPrd.[Name] AS DestinationProductName				  
				  ,MovDest.DestinationProductTypeId
				  ,MovDest.DestinationStorageLocationId				  
		   FROM [Offchain].[MovementDestination] MovDest
		   INNER JOIN [Admin].[Node] DesNd
		   ON DesNd.NodeId = MovDest.DestinationNodeId 
		   INNER JOIN [Admin].Product DestPrd
		   ON DestPrd.ProductId = MovDest.DestinationProductId) MovDest
		   ON m.MovementTransactionId = MovDest.MovementTransactionId
			INNER JOIN admin.Ticket t on lm.TicketId = t.TicketId AND t.TicketTypeId = 7
			INNER JOIN admin.StatusType st on lm.StatusProcessId = st.StatusTypeId	
			INNER JOIN admin.[ScenarioType] sct on m.ScenarioId = sct.ScenarioTypeId 
			LEFT JOIN offchain.MovementPeriod mp on lm.MovementTransactionId = mp.MovementTransactionId
			INNER JOIN admin.CategoryElement ce on t.OwnerId = ce.elementid 
			LEFT JOIN [Admin].CategoryElement MoveType ON MoveType.ElementId = m.MovementTypeId
			INNER JOIN (		select lmm.MovementTransactionId,ti.OwnerId,max(lmm.CreatedDate) CreatedDate
								from Offchain.LogisticMovement lmm  
								INNER JOIN admin.Ticket ti on lmm.TicketId = ti.TicketId AND ti.TicketTypeId = 7
								group by lmm.MovementTransactionId,ti.OwnerId
						) itemsel
			on lm.MovementTransactionId = itemsel.MovementTransactionId AND lm.CreatedDate = itemsel.CreatedDate and t.OwnerId = itemsel.OwnerId
			where m.segmentid = @ElementId  AND (t.OwnerId = @OwnerId OR @OwnerId is null)
			AND  m.OperationalDate between @StartDate AND @EndDate
			
			
	end


	insert into [admin].MovementSendSapInformation(
	[SapStatus],
	[TypeOfMovement],
	[SourceNode],
	[DestinationNode],
	[SourceProduct],
	[DestinationProduct],
	[Scenario],
	[StartDate],
	[EndDate],
	[OwnerName],
	[OwnershipVolume],
	[MovementId],
	[MovementTransactionId],
	[InputElement],
	[ExecutionId],
	[CreatedBy]
	)
	Select 
	a.SapStatus
	,a.TypeOfMovement
	,a.SourceNode
	,a.DestinationNode
	,a.SourceProduct
	,a.DestinationProduct
	,a.Scenario
	,a.StartDate
	,a.EndDate
	,a.OwnerName
	,a.OwnershipVolume
	,a.MovementID
	,a.MovementTransactionId
	,@Elementid
	,@ExecutionId
	,'ReportUser'
	from @TempMovementSendSap a

END
