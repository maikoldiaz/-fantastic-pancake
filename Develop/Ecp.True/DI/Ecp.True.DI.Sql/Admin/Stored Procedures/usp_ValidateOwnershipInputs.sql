/*-- =============================================================================================================================================================================================================================================================================
-- Author:          Microsoft
-- Created Date:	Oct-15-2019
-- Updated Date:	Mar-20-2020
--					Jun-23-2020 Modified Procedure to Remove Duplicate checks and also Removed UnNecessary Condition Reg bug 56994
--					July-07-2020 Added Nodeconnection.IsDeleted=0 to remove deleted connections as per PBI # 31880
--					Mar-03-2021 Validation is added for purchase orders, sales, transfers and self-consumption #PBI 109221
-- <Description>:   This Procedure is to do group validation for The following: 1. Existence of Initial Nodes; 
																				2. Existence of Final Nodes; 
																				3. Resolved Entries for Initial Nodes;
																				4. Nodes with configured ownership rule; 
																				5. Input Ownership Of Initial Nodes. 
																				6. Source node, destination node, owner 1 and 2 are not empty
                    For a given SegementId, StartDate, EndDate, DtNodeInActiveRules, DtNodeProductInActiveRules, DtConnectionProductInActiveRules.
				</Description>
-- =============================================================================================================================================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_ValidateOwnershipInputs]
(
	   @SegmentId							INT,
       @StartDate							DATETIME,
       @EndDate								DATETIME,
	   @DtNodeInActiveRules					Admin.[KeyValueType]		READONLY,
	   @DtNodeProductInActiveRules			Admin.[KeyValueType]		READONLY,
	   @DtConnectionProductInActiveRules	Admin.[KeyValueType]		READONLY
)
AS
BEGIN
	   IF @StartDate <= @EndDate
	   BEGIN

			DECLARE @AnalyticsStatus				INT,
					@AnalyticsErrorMessage			NVARCHAR (MAX),
					@Cnt							INT  = 0,			 
					@ElementId						INT, 
					@NodeID							INT, 
					@newStartDate					DATE, 
					@newEndDate						DATE;
						
			IF OBJECT_ID('tempdb..#TempResolvedEntries') IS NOT NULL
			DROP TABLE #TempResolvedEntries

			IF OBJECT_ID('tempdb..#TempOwnershipRule') IS NOT NULL
			DROP TABLE #TempOwnershipRule

			IF OBJECT_ID('tempdb..#TempNodeTag') IS NOT NULL
			DROP TABLE #TempNodeTag

			IF OBJECT_ID('tempdb..#TempNodesForSegment') IS NOT NULL
			DROP TABLE #TempNodesForSegment

			IF OBJECT_ID('tempdb..#TempGroupValidation') IS NOT NULL
			DROP TABLE #TempGroupValidation

			IF OBJECT_ID('tempdb..#TempNodeOwnershipRule') IS NOT NULL
			DROP TABLE #TempNodeOwnershipRule

			IF OBJECT_ID('tempdb..#TempconnectionProductWithStrategy') IS NOT NULL
			DROP TABLE #TempconnectionProductWithStrategy

			IF OBJECT_ID('tempdb..#TempNodeProductRuleData') IS NOT NULL
			DROP TABLE #TempNodeProductRuleData

			IF OBJECT_ID('tempdb..#TempValidateContractFields') IS NOT NULL
			DROP TABLE #TempValidateContractFields

			CREATE TABLE #TempNodesForSegment
			(
				SegmentId		INT,
				NodeId			INT,
				NodeName		NVARCHAR (150),
				OperationDate	DATE
			)

			CREATE TABLE #TempGroupValidation
			( 
				 NAME         VARCHAR(200), 
				 Result       BIT,  
				 ErrorMessage VARCHAR(4000), 
				 DisplayOrder INT 
			) 

			INSERT INTO #TempGroupValidation 
            (
				NAME, 
				Result, 
				DisplayOrder
			 ) 
			SELECT 'chainWithInitialNodes' AS NAME, 
				   0                       AS Result, 
				   1                       AS DisplayOrder 
			UNION 
			SELECT 'chainWithFinalNodes' AS NAME, 
				   0                     AS Result, 
				   2                     AS DisplayOrder 
			UNION 
			SELECT 'nodesWithStrategy' AS NAME, 
				   1                   AS Result, 
				   3                   AS DisplayOrder 
			UNION 
			SELECT 'nodesWithOwnershipRules' AS NAME, 
				   1                         AS Result, 
				   4                         AS DisplayOrder 
			UNION 
			SELECT 'connectionProductWithStrategy' AS NAME, 
				   1                               AS Result, 
				   5                               AS DisplayOrder 
			UNION 
			SELECT 'connectionProductWithPriority' AS NAME, 
				   1                               AS Result, 
				   6                               AS DisplayOrder 
			UNION 
			SELECT 'activeOwnershipStrategy' AS NAME, 
				   1                         AS Result, 
				   7                         AS DisplayOrder 
			UNION 
			SELECT 'nodeOwnershipCouldBeFound' AS NAME, 
				   1                           AS Result, 
				   8                           AS DisplayOrder 
			UNION 
			SELECT 'chainWithAnaliticalOperationalInfo' AS NAME, 
				   NULL                                 AS Result, 
				   9                                    AS DisplayOrder 
			UNION
			SELECT 'validateContractFields' AS NAME,
					1						AS Result,
					10						AS DisplayOrder

			SELECT   NT.ElementId
					,NT.NodeId
					,CASE WHEN @StartDate >= NT.StartDate 
						  THEN @StartDate
						  ELSE CAST(NT.StartDate		AS DATE) 
						  END AS StartDate
					,CASE WHEN Nt.EndDate = '9999-12-31' 
						   OR  @EndDate   <= Nt.EndDate
						  THEN @EndDate
						  ELSE CAST(NT.EndDate AS DATE)
						  END AS EndDate
					,ROW_NUMBER()OVER(ORDER BY NT.NodeId)Rnum
			INTO #TempNodeTag
			FROM Admin.NodeTag NT
			WHERE NT.ElementId = @SegmentId
			AND (	 (@StartDate BETWEEN NT.StartDate  AND Nt.EndDate )
				  OR (@EndDate   BETWEEN NT.StartDate  AND Nt.EndDate )
				)

			SET	  @Cnt  = (SELECT COUNT(1) 
						   FROM #TempNodeTag)

			WHILE @Cnt>0
			BEGIN
				SELECT  @NodeID = NodeId, 
						@ElementId = ElementId, 
						@newStartDate = StartDate, 
						@newEndDate = EndDate
				FROM #TempNodeTag
				WHERE Rnum = @Cnt

				SELECT @Cnt = @Cnt-1

				INSERT INTO #TempNodesForSegment
				(
						SegmentId,
						NodeId,
						OperationDate		
				)
				SELECT   Elementid		AS 	SegmentId
						,NodeId			AS  NodeId
						,dates			AS  OperationDate
				FROM [Admin].[udf_GetAllDates]( @newStartDate, 
												@newEndDate, 
												@NodeID, 
												@ElementId) C

			END

			DELETE FROM #TempNodesForSegment 
			WHERE OperationDate NOT BETWEEN @StartDate AND @EndDate

			--Update the NodeName Column
			UPDATE NdSeg
			SET NodeName = Nd.Name
			FROM #TempNodesForSegment NdSeg
			INNER JOIN Admin.Node ND
			ON Nd.NodeId = NdSeg.NodeId

			--Logic to Update #TempGroupValidation For nodesWithStrategy Start
			SELECT  NdSeg.NodeId
				   ,NdSeg.NodeName
				   ,Nd.NodeOwnershipRuleId
				   ,NdRule.RuleName AS NodeOwnershipRuleName
			INTO #TempNodeOwnershipRule
			FROM #TempNodesForSegment NdSeg
			INNER JOIN Admin.Node ND
			ON NdSeg.NodeId = Nd.NodeId
			LEFT JOIN [Admin].[NodeOwnershipRule] NdRule
			ON NdRule.RuleId = Nd.NodeOwnershipRuleId

			IF EXISTS(	SELECT 1
						FROM #TempNodeOwnershipRule ND
						WHERE ND.NodeOwnershipRuleId IS NULL
					)
			BEGIN

					DECLARE @NodeNameWithOutStrategy VARCHAR(4000) = (SELECT TOP 1 Nd.NodeName AS Name
																	  FROM #TempNodeOwnershipRule ND
																	  WHERE ND.NodeOwnershipRuleId IS NULL
																	  FOR JSON AUTO
																	  )

					SELECT @NodeNameWithOutStrategy = REPLACE(REPLACE(@NodeNameWithOutStrategy,'[',''),']','')

					UPDATE #TempGroupValidation
					SET Result = 0,
					ErrorMessage = @NodeNameWithOutStrategy
					WHERE Name = 'nodesWithStrategy'
		   END
		--Logic to Update #TempGroupValidation For nodesWithStrategy End
		
		--Logic to Update #TempGroupValidation For connectionProductWithStrategy Start
			SELECT NC.SourceNodeId,
				   CAST('' AS NVARCHAR (150)) AS SourceNodeName,
				   NC.DestinationNodeId,
				   CAST('' AS NVARCHAR (150)) AS DestinationNodeName,
				   NCP.ProductId,
				   NCP.[NodeConnectionProductRuleId],
				   NCP.Priority,
				   NCPrdRule.RuleName AS NodeConnectionProductRuleName
			INTO #TempconnectionProductWithStrategy
			FROM #TempNodesForSegment NdSeg
			INNER JOIN Admin.NodeConnection NC
			ON (NdSeg.NodeId = NC.SourceNodeId
				OR NdSeg.NodeId = NC.DestinationNodeId)
			AND NC.IsDeleted = 0
			INNER JOIN Admin.NodeConnectionProduct NCP
			ON NC.NodeConnectionId = NCP.NodeConnectionId
			LEFT JOIN Admin.[NodeConnectionProductRule] NCPrdRule
			ON NCP.[NodeConnectionProductRuleId] = NCPrdRule.RuleId

			UPDATE ProdStg
			SET  SourceNodeName = SrcND.Name
				,DestinationNodeName = DestND.Name
			FROM #TempconnectionProductWithStrategy ProdStg
			INNER JOIN Admin.Node SrcND
			ON SrcND.NodeId = ProdStg.SourceNodeId
			INNER JOIN Admin.Node DestND
			ON DestND.NodeId = ProdStg.DestinationNodeId

			IF EXISTS( SELECT 1
					   FROM #TempconnectionProductWithStrategy
					   WHERE [NodeConnectionProductRuleId] IS NULL
					)
			BEGIN

					DECLARE @connectionProductWithStrategy VARCHAR(4000) = (SELECT  TOP 1
																					SourceNodeName+ ' - '+DestinationNodeName AS Name
																			FROM #TempconnectionProductWithStrategy
																			WHERE [NodeConnectionProductRuleId] IS NULL
																			FOR JSON AUTO)

					SELECT @connectionProductWithStrategy = REPLACE(REPLACE(@connectionProductWithStrategy,'[',''),']','')

					UPDATE #TempGroupValidation
					SET Result = 0,
					ErrorMessage = @connectionProductWithStrategy
					WHERE Name = 'connectionProductWithStrategy'
		  END
		--Logic to Update #TempGroupValidation For connectionProductWithStrategy End

		--Logic to Update #TempGroupValidation For connectionProductWithPriority Start
			IF EXISTS(  SELECT 1
						FROM #TempconnectionProductWithStrategy
						WHERE Priority IS NULL
						)
			BEGIN
						DECLARE @connectionProductWithPriority VARCHAR(4000) = (SELECT  TOP 1
																						SourceNodeName+ ' - '+DestinationNodeName AS Name
																				FROM #TempconnectionProductWithStrategy
																				WHERE Priority IS NULL
																				FOR JSON AUTO)

						SELECT @connectionProductWithPriority = REPLACE(REPLACE(@connectionProductWithPriority,'[',''),']','')

						UPDATE #TempGroupValidation
						SET Result = 0,
						ErrorMessage = @connectionProductWithPriority
						WHERE Name = 'connectionProductWithPriority'
			END			
		--Logic to Update #TempGroupValidation For connectionProductWithPriority End

		--Logic to Update #TempGroupValidation For activeOwnershipStrategy Start
		  DECLARE @ActiveOwnershipStrategy VARCHAR(4000) 

		  SELECT SLP.NodeProductRuleId,
				 NpRule.RuleName AS NodeProductRuleName,
				 NdSeg.NodeName
		  INTO #TempNodeProductRuleData
		  FROM #TempNodesForSegment NdSeg
		  INNER JOIN Admin.NodeStorageLocation NSL
		  ON NdSeg.NodeId = NSL.NodeId
		  INNER JOIN Admin.StorageLocationProduct SLP
		  ON SLP.[NodeStorageLocationId] = NSL.[NodeStorageLocationId]
		  LEFT JOIN [Admin].[NodeProductRule] NpRule
		  ON NpRule.RuleId = Slp.NodeProductRuleId

		  IF EXISTS(SELECT 1
					FROM #TempNodeOwnershipRule NdRule
					INNER JOIN @DtNodeInActiveRules InActiv
					ON InActiv.Value = NdRule.NodeOwnershipRuleName)
		  BEGIN

					SELECT @ActiveOwnershipStrategy = ( SELECT  TOP 1  NdRule.NodeOwnershipRuleName AS Name
																	 ,'node' AS Type
														FROM #TempNodeOwnershipRule NdRule
														INNER JOIN @DtNodeInActiveRules InActiv
														ON InActiv.Value = NdRule.NodeOwnershipRuleName
														FOR JSON AUTO)

					SELECT @ActiveOwnershipStrategy = REPLACE(REPLACE(@ActiveOwnershipStrategy,'[',''),']','')

					UPDATE #TempGroupValidation
					SET Result = 0,
						ErrorMessage = @ActiveOwnershipStrategy
					WHERE Name = 'activeOwnershipStrategy'
		  END
		  ELSE
		  IF EXISTS(SELECT 1
					FROM #TempconnectionProductWithStrategy NdRule
					INNER JOIN @DtConnectionProductInActiveRules InActiv
					ON InActiv.Value = NdRule.NodeConnectionProductRuleName)
		  BEGIN
					SELECT @ActiveOwnershipStrategy = ( SELECT  TOP 1  NdRule.NodeConnectionProductRuleName  AS Name
																	 ,'connectionProduct' AS Type
														FROM #TempconnectionProductWithStrategy NdRule
														INNER JOIN @DtConnectionProductInActiveRules InActiv
														ON InActiv.Value = NdRule.NodeConnectionProductRuleName
														FOR JSON AUTO)

					SELECT @ActiveOwnershipStrategy = REPLACE(REPLACE(@ActiveOwnershipStrategy,'[',''),']','')

					UPDATE #TempGroupValidation
					SET Result = 0,
						ErrorMessage = @ActiveOwnershipStrategy
					WHERE Name = 'activeOwnershipStrategy'
		  END
		  ELSE
		  IF EXISTS(SELECT 1
					FROM #TempNodeProductRuleData NdRule
					INNER JOIN @DtNodeProductInActiveRules InActiv
					ON InActiv.Value = NdRule.NodeProductRuleName)
		  BEGIN
					SELECT @ActiveOwnershipStrategy = ( SELECT  TOP 1  NdRule.NodeProductRuleName  AS Name
																	 ,'nodeProduct' AS Type
														FROM #TempNodeProductRuleData NdRule
														INNER JOIN @DtNodeProductInActiveRules InActiv
														ON InActiv.Value = NdRule.NodeProductRuleName
														FOR JSON AUTO)

					SELECT @ActiveOwnershipStrategy = REPLACE(REPLACE(@ActiveOwnershipStrategy,'[',''),']','')

					UPDATE #TempGroupValidation
					SET Result = 0,
						ErrorMessage = @ActiveOwnershipStrategy
					WHERE Name = 'activeOwnershipStrategy'
		  END




		--Logic to Update #TempGroupValidation For ActiveOwnershipStrategy End

		--Logic to Update #TempGroupValidation For Initial Nodes Start
		/*
		   For the Inputs Passed(SegementId/ElementId, StartDate, EndDate) Find the Nodes which should have node Connections with SourceNodes 
		   But Not with Destinations and ( the SegementId/ElementId of the DestinationNode is Not same as the Input 
		   OR the NodeName(DestinationNodes) should contain Genérico in Nodename
		*/
		  IF EXISTS(  SELECT 1
					  FROM #TempNodesForSegment NT
					  INNER JOIN Admin.[NodeConnection] NC
					  ON NT.NodeId = Nc.[DestinationNodeId]
					  AND NT.NodeId <> Nc.SourceNodeID
					  AND NC.IsDeleted = 0
					  INNER JOIN Admin.NodeTag SrcSeg	/*Joined NodeTag Again Based on NodeId to get SegementId which shouldnot be matching*/
					  ON SrcSeg.NodeId = Nc.SourceNodeId
					  INNER JOIN Admin.Node ND  /*Joined Node Table To Get Name */
					  ON ND.NodeId = Nc.SourceNodeId
					  WHERE (    SrcSeg.ElementId <> NT.SegmentId
							OR ND.Name LIKE '%Genérico%'  ---Check if the NodeName Contains
						   )
		           )
		  BEGIN
				UPDATE #TempGroupValidation
				SET Result = 1
				WHERE Name = 'chainWithInitialNodes'
		  END
		--Logic to Update #TempGroupValidation For Initial Nodes End

		--Logic to Update #TempGroupValidation For Final Nodes Start
		/*
		   For the Inputs Passed(SegementId/ElementId, StartDate, EndDate) Find the Nodes which should have node Connections with DestinationNodes 
		   But Not with Source and ( the SegementId/ElementId of the SourceNode is Not same as the Input 
		   or the NodeName(DestinationNodes) should contain Genérico in Nodename
		*/
		  IF EXISTS(  SELECT 1
					  FROM #TempNodesForSegment NT
					  INNER JOIN Admin.[NodeConnection] NC
					  ON NT.NodeId = Nc.SourceNodeId
					  AND NT.NodeId <> Nc.DestinationNodeId
					  AND NC.IsDeleted = 0
					  INNER JOIN Admin.NodeTag DestSeg	/*Joined NodeTag Again Based on NodeId to get SegementId which shouldnot be matching*/
					  ON DestSeg.NodeId = Nc.DestinationNodeId
					  INNER JOIN Admin.Node ND  /*Joined Node Table To Get Name */
					  ON ND.NodeId = Nc.DestinationNodeId
					  WHERE (    DestSeg.ElementId <> NT.SegmentId
							OR ND.Name LIKE '%Genérico%'  ---Check if the NodeName Contains
						   )
		           )
		  BEGIN
				UPDATE #TempGroupValidation
				SET Result = 1
				WHERE Name = 'chainWithFinalNodes'
		  END		
		--Logic to Update #TempGroupValidation For Final Nodes End

		--Logic to Update #TempGroupValidation For Resolved Entries for Initial Nodes Start(All Nodes Should Have Ownership)


		 SELECT  NSL.NodeId				AS NSL_NodeId	
				 ,NT.NodeId				AS NT_NodeId
				 ,ND.Name               AS NodeName  
				 ,SLP.NodeStorageLocationId AS SLP_NodeStorageLocationId
				 ,NSL.NodeStorageLocationId	AS NSL_NodeStorageLocationId
		         ,SLP.NodeProductRuleId
				 ,NT.OperationDate
				 ,SLPO.StorageLocationProductOwnerId
		  INTO #TempResolvedEntries
		  FROM #TempNodesForSegment NT
		  LEFT JOIN Admin.NodeStorageLocation NSL
		  ON NSL.NodeId = NT.NodeId
		  INNER JOIN Admin.[Node] ND  
		  ON ND.NodeId = NT.NodeId
		  LEFT JOIN Admin.StorageLocationProduct SLP
		  ON SLP.NodeStorageLocationId = NSL.NodeStorageLocationId
		  LEFT JOIN Admin.StorageLocationProductOwner SLPO
		  ON SLPO.StorageLocationProductId = SLP.StorageLocationProductId

		  IF EXISTS(SELECT 1 FROM #TempResolvedEntries)--Check for existence for the segment
		  BEGIN
			  IF  EXISTS(SELECT 1 FROM #TempResolvedEntries WHERE StorageLocationProductOwnerId IS NULL)--Check Data for All Nodes
			  BEGIN
					DECLARE @NodeOwners VARCHAR(4000) = (SELECT TOP 1 NodeName  AS Name
														 FROM #TempResolvedEntries 
														 WHERE StorageLocationProductOwnerId IS NULL
														 FOR JSON AUTO)
					SELECT @NodeOwners = REPLACE(REPLACE(@NodeOwners,'[',''),']','')

					UPDATE #TempGroupValidation
					SET Result = 0,
					ErrorMessage = @NodeOwners
					WHERE Name = 'nodeOwnershipCouldBeFound'
			  END
		  END
		--Logic to Update #TempGroupValidation For Resolved Entries for Initial Nodes End

		--Logic to Update #TempGroupValidation For Ownership Rule Configured Start(All Nodes Should have)
		  SELECT  NSL.NodeId				AS NSL_NodeId	
				 ,NT.NodeId					AS NT_NodeId
				 ,ND.Name				    AS NodeName
				 ,SLP.NodeStorageLocationId AS SLP_NodeStorageLocationId
				 ,NSL.NodeStorageLocationId	AS NSL_NodeStorageLocationId
		         ,SLP.NodeProductRuleId
				 ,NT.OperationDate 
		  INTO #TempOwnershipRule
		  FROM #TempNodesForSegment NT
		  LEFT JOIN Admin.NodeStorageLocation NSL
		  ON NSL.NodeId = NT.NodeId
		  INNER JOIN Admin.[Node] ND
		  ON ND.NodeId = NT.NodeId
		  LEFT JOIN Admin.StorageLocationProduct SLP
		  ON SLP.NodeStorageLocationId = NSL.NodeStorageLocationId


		  IF EXISTS(SELECT 1 FROM #TempOwnershipRule)--Check for existence for the segment
		  BEGIN
			  IF EXISTS(SELECT 1 
							FROM #TempOwnershipRule 
							WHERE SLP_NodeStorageLocationId IS NULL
							OR    NSL_NodeStorageLocationId IS NULL
							OR    NodeProductRuleId    IS NULL 
							)--Check Data for All Nodes
			  BEGIN
					DECLARE @OwnRule VARCHAR(4000) = ( SELECT TOP 1 NodeName AS Name
											           FROM #TempOwnershipRule 
											           WHERE SLP_NodeStorageLocationId IS NULL
											           OR    NSL_NodeStorageLocationId IS NULL
											           OR    NodeProductRuleId    IS NULL
											           FOR JSON AUTO
										              )
					SELECT @OwnRule = REPLACE(REPLACE(@OwnRule,'[',''),']','')

					UPDATE #TempGroupValidation
					SET  Result			= 0
						,ErrorMessage   = @OwnRule
					WHERE Name = 'nodesWithOwnershipRules'			
			  END
		  END
		--Logic to Update #TempGroupValidation For Ownership Rule Configured End

		--Logic to Update #TempGroupValidation For ChainWithAnaliticalOperationalInfo Start

			SELECT TOP 1
					@AnalyticsStatus		  =	AnalyticsStatus,
				   @AnalyticsErrorMessage	  = AnalyticsErrorMessage
			FROM Admin.Ticket
			WHERE TicketTypeId = 1
			AND CategoryElementId = @SegmentId
			AND @EndDate <= EndDate
			AND AnalyticsStatus = 1
			ORDER BY TicketId DESC

			UPDATE #TempGroupValidation
			SET  Result			= @AnalyticsStatus
				,ErrorMessage   = @AnalyticsErrorMessage
			WHERE Name = 'chainWithAnaliticalOperationalInfo'

			UPDATE #TempGroupValidation
			SET  Result			= 0
			WHERE Name = 'chainWithAnaliticalOperationalInfo' 
			AND Result IS NULL
		--Logic to Update #TempGroupValidation For ChainWithAnaliticalOperationalInfo End

		--Logic to Update #TempGroupValidation For ValidateContractFields Start			
			SELECT DISTINCT c.DocumentNumber, c.Position
			INTO #TempValidateContractFields
			FROM Admin.Contract c
			INNER JOIN #TempNodeTag tempNT
			ON (	tempNT.NodeId = c.DestinationNodeId
					OR tempNT.NodeId = c.SourceNodeId
				) 
			WHERE (	 (@StartDate BETWEEN c.StartDate  AND c.EndDate )
				  OR (@EndDate   BETWEEN c.StartDate  AND c.EndDate )
				  )
			AND	  (
						c.Owner1Id IS NULL
						OR c.Owner2Id IS NULL
						OR c.SourceNodeId IS NULL
						OR c.DestinationNodeId IS NULL
				   )

			IF EXISTS(SELECT TOP 1 DocumentNumber FROM #TempValidateContractFields)
			BEGIN
				DECLARE @resultValidateContractFields NVARCHAR(4000) =  ( 
																			SELECT TOP 1 DocumentNumber AS [Name], 
																						 Position		AS [Type] 
																			FROM #TempValidateContractFields 
																			FOR JSON AUTO 
																		)

				SELECT @resultValidateContractFields = REPLACE(REPLACE(@resultValidateContractFields,'[',''),']','')

				UPDATE #TempGroupValidation
				SET  Result			= 0
					,ErrorMessage   = @resultValidateContractFields
				WHERE Name = 'validateContractFields'
			
			END

		--Logic to Update #TempGroupValidation For ValidateContractFields End


		  SELECT   Name  
			      ,Result   
			      ,ErrorMessage  
			      ,DisplayOrder
		  FROM #TempGroupValidation  
		  ORDER BY DisplayOrder

	   END
	   ELSE
	   IF @StartDate > @EndDate
	   BEGIN
		  RAISERROR ('StartDate is Greater than EndDate',16,1) 
	   END

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to do group validation for The following: 1. Existence of Initial Nodes; 
	2. Existence of Final Nodes; 
	3. Resolved Entries for Initial Nodes;
	4. Nodes with configured ownership rule; 
	5. Input Ownership Of Initial Nodes.
	For a given SegementId, StartDate, EndDate, DtNodeInActiveRules, DtNodeProductInActiveRules, DtConnectionProductInActiveRules.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_ValidateOwnershipInputs',
    @level2type = NULL,
    @level2name = NULL