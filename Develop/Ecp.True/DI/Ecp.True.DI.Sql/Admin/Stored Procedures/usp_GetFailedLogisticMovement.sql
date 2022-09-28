/*-- =================================================================================================================================
-- Author: IG Service
-- Created Date: Abril-27-2021
-- Updated Date: Jul-21-2021 change the DestinationStorage, SourceStorage, SourceProduct, DestinationProduct query.
-- Updated Date:	Jul-26-2021 add contract validation.
-- Updated Date:	Jul-29-2021 change get documentNumber and Position for sales and purchases.
-- Updated Date:	Aug-02-2021 change netStandarVolumen by ownershipVolumen.
-- Updated Date:	Oct-11-2021 change storage location, product.
-- Updated Date:	Oct-19-2021  Add owner validation
-- <Description>: This Procedure is used to get the failed logistic movemements.
* get the nodes by segment.
* get the failes logistic movement.
</Description>
-- ==================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetFailedLogisticMovement]
(
		@SegmentId							INT,
		@StartDate							DATETIME,
		@EndDate							DATETIME,
		@ScenarioTypeId						INT,
		@OwnerId							INT,
		@DtNodes								[Admin].[NodeListType] READONLY
)
AS
 BEGIN 
	
	IF @StartDate <= @EndDate 
	BEGIN 
	DECLARE @IsSelectedAllSegment BIT; 
		
	IF OBJECT_ID('tempdb..#TempNodeTagForSegment')IS NOT NULL
		DROP TABLE #TempNodeTagForSegment 
		
	IF OBJECT_ID('tempdb..#TempFailedMovement')IS NOT NULL
		DROP TABLE #TempFailedMovement 

	IF OBJECT_ID('tempdb..#TempNodes')IS NOT NULL
		DROP TABLE #TempNodes 
	
	IF OBJECT_ID('tempdb..#TempStorage')IS NOT NULL
			DROP TABLE #TempStorage 

	CREATE TABLE #TempNodeTagForSegment
	(
		NodeId						INT,
		NodeName					NVARCHAR (150),
		ElementId					INT,
		StartDate					DATE,
		EndDate						DATE,
		IsEnabledForSendToSap		INT,
		RowNumber					INT
	)
			
	-- Validate if all the nodes of a segment are being used

	SET @IsSelectedAllSegment = CASE WHEN (
	SELECT  COUNT(NodeId)
	FROM @DtNodes) = 0 THEN 1 ELSE 0 END; 

		-- Consult nodes used by segment
		SELECT  NT.NodeId
				,N.[Name] AS NodeName
				,NT.ElementId
				,CASE WHEN @StartDate >= NT.StartDate 
						THEN @StartDate
						ELSE CAST(NT.StartDate AS DATE) 
						END AS StartDate
				,CASE WHEN Nt.EndDate = '9999-12-31' 
						OR  @EndDate   <= Nt.EndDate
						THEN @EndDate
						ELSE CAST(NT.EndDate AS DATE)
						END AS EndDate
				,N.SendToSAP AS IsEnabledForSendToSap
		INTO #TempNodes
		FROM [Admin].NodeTag NT JOIN [Admin].Node N ON NT.NodeId = N.NodeId 
		WHERE NT.ElementId = @SegmentId
		AND N.SendToSAP = 1
		AND n.IsActive = 1
		AND ((@StartDate BETWEEN NT.StartDate  AND Nt.EndDate )
				OR (@EndDate   BETWEEN NT.StartDate  AND Nt.EndDate ))


		-- Consult nodes
		IF(@IsSelectedAllSegment = 1)
		BEGIN
			INSERT INTO #TempNodeTagForSegment 
			SELECT *, ROW_NUMBER()OVER(ORDER BY NT.NodeId)RowNumber FROM #TempNodes NT
		END
		ELSE
		BEGIN
			INSERT INTO #TempNodeTagForSegment 
			SELECT *, ROW_NUMBER()OVER(ORDER BY NT.NodeId)RowNumber FROM #TempNodes NT WHERE NT.NodeId IN (SELECT NodeId FROM @DtNodes)
		END


	-- Search movements IN failed（2）
	SELECT  DISTINCT MVL.*
			INTO #TempFailedMovement
	FROM 
	(
		SELECT  MVT.*
		FROM Offchain.LogisticMovement AS MVT
		INNER JOIN [Admin].Ticket T
		ON T.TicketId = MVT.TicketId
		INNER JOIN Offchain.MovementSource AS MS
		ON MS.MovementTransactionId = MVT.MovementTransactionId 
		INNER JOIN #TempNodeTagForSegment AS NBS
		ON NBS.ElementId = T.CategoryElementId AND NBS.NodeId = MS.SourceNodeId
		WHERE mvt.StatusProcessId = 2 -- failed logistic movement 
		AND t.CategoryElementId = @SegmentId 
		AND T.TicketTypeId = 7 -- logisticMovement 
		AND T.ScenarioTypeId = @ScenarioTypeId
		AND T.OwnerId = @OwnerId
		AND CONCAT(MVT.MovementTransactionId,MVT.StatusProcessId,MVT.CreatedDate)IN
			(SELECT Movement FROM (SELECT TOP(1) 
				CONCAT(Logistic.MovementTransactionId,Logistic.StatusProcessId,Logistic.CreatedDate) AS Movement, 
				Logistic.TicketId, 
				Logistic.StatusProcessId
				FROM [Offchain].LogisticMovement Logistic
				WHERE Logistic.MovementTransactionId = MVT.MovementTransactionId
				ORDER BY  Logistic.CreatedDate desc) LM
				INNER JOIN [Admin].Ticket Ticket 
				ON LM.TicketId = Ticket.TicketId  AND Ticket.OwnerId = @OwnerId  AND (LM.StatusProcessId = 2))
		UNION ALL
		SELECT  MVT.*
		FROM Offchain.LogisticMovement AS MVT
		INNER JOIN [Admin].Ticket T
		ON T.TicketId = MVT.TicketId 
		INNER JOIN Offchain.MovementDestination AS MD
		ON MD.MovementTransactionId = MVT.MovementTransactionId
		INNER JOIN #TempNodeTagForSegment AS NBS
		ON NBS.ElementId = T.CategoryElementId AND NBS.NodeId = MD.DestinationNodeId
		WHERE mvt.StatusProcessId = 2 -- failed logistic movement 
		AND t.CategoryElementId = @SegmentId 
		AND T.TicketTypeId = 7 -- logisticMovement 
		AND T.ScenarioTypeId = @ScenarioTypeId  
		AND T.OwnerId = @OwnerId
		AND CONCAT(MVT.MovementTransactionId,MVT.StatusProcessId,MVT.CreatedDate)IN
			(SELECT Movement FROM (SELECT TOP(1) 
				CONCAT(Logistic.MovementTransactionId,Logistic.StatusProcessId,Logistic.CreatedDate) AS Movement, 
				Logistic.TicketId, 
				Logistic.StatusProcessId
				FROM [Offchain].LogisticMovement Logistic
				WHERE Logistic.MovementTransactionId = MVT.MovementTransactionId
				ORDER BY  Logistic.CreatedDate desc) LM
				INNER JOIN [Admin].Ticket Ticket 
				ON LM.TicketId = Ticket.TicketId AND Ticket.OwnerId = @OwnerId  AND (LM.StatusProcessId = 2))
	) AS MVL


		SELECT  
			N.LogisticCenterId
		,MIN (SL.Name) AS SapStorage
		,p.ProductId AS ProductId
		,n.NodeId,
		p.Name AS ProductName
		INTO #TempStorage
		FROM Admin.Node N
		LEFT JOIN #TempFailedMovement AS LM 
		on LM.DestinationLogisticCenterId = N.LogisticCenterId OR LM.SourceLogisticCenterId = N.LogisticCenterId
		LEFT JOIN Admin.NodeStorageLocation NLS ON NLS.NodeId = N.NodeId
		LEFT JOIN Admin.StorageLocation SL ON SL.LogisticCenterId = N.LogisticCenterId
		LEFT JOIN admin.StorageLocationProduct SLP ON SLP.NodeStorageLocationId = NLS.NodeStorageLocationId
		LEFT JOIN admin.Product P ON P.ProductId = SLP.ProductId
		WHERE P.ProductId IN(LM.SourceProductId ,LM.DestinationProductId)
		AND N.NodeId  IN(LM.SourceLogisticNodeId, LM.DestinationLogisticNodeId) 
		AND N.LogisticCenterId IN (LM.SourceLogisticCenterId, LM.DestinationLogisticCenterId)
		GROUP BY  N.LogisticCenterId ,P.ProductId ,N.NodeId, p.Name 


		SELECT 
		[LM].[MovementTransactionId] AS 'movementTransactionId',
		[ST].[StatusType] AS 'state',
		[LM].[MessageProcess] AS 'description',
		[MT].[Name] AS 'movementType',
		[SLC].[Name] AS 'sourceCenter',
		SUBSTRING([SSL].SapStorage,CHARINDEX(':',[SSL].SapStorage)+1,LEN([SSL].SapStorage)) AS 'sourceStorage',
		SSL.ProductName AS 'sourceProduct',
		[DLC].[Name] AS 'destinationCenter',
		SUBSTRING([DSL].SapStorage,CHARINDEX(':',[DSL].SapStorage)+1,LEN([DSL].SapStorage)) AS 'destinationStorage',
		DSL.ProductName AS 'destinationProduct',
		[LM].[OwnershipVolume] AS 'ownershipVolume',
		[MU].[Name] AS 'units',
		[M].[OperationalDate] AS 'operationalDate',
		[LM].[LogisticMovementId] AS 'logisticMovementId',
		[M].[MovementId] AS 'movementId',
		[CC].[Name] AS 'costCenter', 
		[LM].[SapTransactionCode] AS 'gmCode', 
		[LM].[DocumentNumber] AS 'documentNumber',
		[LM].[Position] AS 'position',
		[LM].[MovementOrder] AS 'order',
		[LM].[SapSentDate]  AS 'accountingDate',
		NULL AS 'segment',
		NULL AS 'scenario',
		NULL AS 'owner'
		FROM #TempFailedMovement AS LM
		INNER JOIN [Admin].[Ticket] AS T
		ON [LM].[TicketId] = [T].[TicketId]
		LEFT JOIN [Offchain].[Movement] AS M
		ON [LM].[MovementTransactionId] = [M].[MovementTransactionId]
		LEFT JOIN [Admin].[StatusType] ST
		ON [LM].[StatusProcessId] = [ST].[StatusTypeId]
		LEFT JOIN [Admin].[CategoryElement] AS MT
		ON [LM].[LogisticMovementTypeId] = [MT].[ElementId]
		LEFT JOIN [Admin].[LogisticCenter] AS SLC
		ON [LM].[SourceLogisticCenterId] = [SLC].[LogisticCenterId]
		LEFT JOIN [Admin].[LogisticCenter] AS DLC
		ON [LM].[DestinationLogisticCenterId] = [DLC].[LogisticCenterId]
		LEFT JOIN [Admin].[CategoryElement] AS MU
		ON [LM].[MeasurementUnit] = [MU].[ElementId]
		INNER JOIN [Admin].[CategoryElement] AS SEG
		ON [M].[SegmentId] = [SEG].[ElementId]
		INNER JOIN [Admin].[ScenarioType] AS SCE
		ON [M].[ScenarioId] = [SCE].[ScenarioTypeId]
		INNER JOIN [Admin].[CategoryElement] AS OWN
		ON [T].[OwnerId] = [OWN].[ElementId]
		LEFT JOIN #TempStorage AS SSL
		on SSL.LogisticCenterId = LM.SourceLogisticCenterId AND SSL.NodeId = LM.SourceLogisticNodeId AND SSL.ProductId = LM.SourceProductId
		LEFT JOIN #TempStorage AS DSL
		on DSL.LogisticCenterId = LM.DestinationLogisticCenterId AND DSL.NodeId = LM.DestinationLogisticNodeId AND DSL.ProductId = LM.DestinationProductId
		LEFT JOIN [Admin].[CategoryElement] AS CC
		ON [LM].[CostCenterId] = [CC].[ElementId]
		
	END 	
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', 
	@value=N'This Procedure is used to get the failed logistic movemements.
		* get the nodes by segment.
		* get the failes logistic movement.' , 
	@level0type=N'SCHEMA',
	@level0name=N'Admin', 
	@level1type=N'PROCEDURE',
	@level1name=N'usp_GetFailedLogisticMovement'
GO
