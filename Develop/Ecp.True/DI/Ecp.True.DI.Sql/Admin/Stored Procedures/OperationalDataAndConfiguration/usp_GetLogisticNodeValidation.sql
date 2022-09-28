/*-- =============================================================================================================================================================================
-- Author:          Microsoft
-- Created Date:	Feb-13-2020
-- Updated Date:	Mar-20-2020
-- Updated Date:    Jul-01-2020    Added a condition to consider only the tickets which are successfull (Ticket Status = 0)
-- Updated Date:    Jul-09-2020    Added a condition to consider all segments (@SegmentId=0)
-- Updated Date:	Jul-13-2020	   Corrected join condition	
-- Updated Date:	Jul-22-2020	   Corrected @SegmentId=0 condition
-- <Description>:	This Procedure is used to get the Logistic Node Validation data for the Excel file based on the Segment Id, Node Id, Start Date and End Date. </Description>
-- ==============================================================================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_GetLogisticNodeValidation] 
(
		 @SegmentId			INT
		,@StartDate			DATE
		,@EndDate			DATE
		,@NodeId			INT		= NULL
)
AS 
BEGIN


		IF @StartDate <= @EndDate
		BEGIN
			DECLARE @Cnt				INT  = 0,
					@NodeName			NVARCHAR(250),
					@newStartDate		DATE, 
					@newEndDate			DATE,
					@TicketId			INT,
					@ActualTicketID		INT,
					@newSegmentId	    INT,
					@InputNodeId		INT = @NodeId


			IF OBJECT_ID('tempdb..#TempNodeTagNodeValidation') IS NOT NULL
			DROP TABLE #TempNodeTagNodeValidation

			IF OBJECT_ID('tempdb..#TempAllNodesForNodeValidation') IS NOT NULL
			DROP TABLE #TempAllNodesForNodeValidation

			IF OBJECT_ID('tempdb..#TempTicketNodeValidation') IS NOT NULL
			DROP TABLE #TempTicketNodeValidation

			IF OBJECT_ID('tempdb..#TempResultTicketsForNodeValidation') IS NOT NULL
			DROP TABLE #TempResultTicketsForNodeValidation

            IF OBJECT_ID('tempdb..#TempSegmentInfo') IS NOT NULL
            DROP TABLE #TempSegmentInfo

            CREATE TABLE #TempSegmentInfo
            (
                    SegmentId     INT
            )

            IF @SegmentId = 0
            BEGIN
                    INSERT INTO #TempSegmentInfo
                    (
                            SegmentId     
                    )
                    SELECT  ElementId  AS [CategoryElementId]
                    FROM Admin.CategoryElement CE
                    WHERE CategoryId = 2
                    AND IsActive = 1
            END
            ELSE
            BEGIN
                    INSERT INTO #TempSegmentInfo
                    (
                            SegmentId     
                    )
                    SELECT  @SegmentId
            END


			CREATE TABLE #TempAllNodesForNodeValidation
			(
				NodeId			INT,
				NodeName		NVARCHAR (150),
				OperationDate	DATE
			)

			CREATE TABLE #TempResultTicketsForNodeValidation
			(
				TicketId		INT,
				SegmentId		INT,
				OperationDate	DATE
			)

			--Get the Nodes in Segment
			SELECT Nt.NodeID
				  ,ND.Name AS NodeName
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
			INTO #TempNodeTagNodeValidation
			FROM Admin.NodeTag NT
			INNER JOIN Admin.CategoryElement CatEle
			ON CatEle.ElementId = NT.ElementId
			INNER JOIN Admin.Node ND
			ON Nd.NodeId = NT.NodeId
            WHERE NT.ElementId IN (SELECT SegmentId FROM #TempSegmentInfo)
			AND (Nt.NodeId = @NodeID OR @NodeID IS NULL)
			AND Nd.SendToSAP = 1


			SELECT Tic.TicketId,
				   Tic.StartDate,
				   Tic.EndDate,
				   Tic.CategoryElementId AS SegmentId,
				   ROW_NUMBER()OVER(ORDER BY TicketID DESC)Rnum
			INTO #TempTicketNodeValidation
			FROM Admin.Ticket Tic
            WHERE Tic.CategoryElementId IN (SELECT SegmentId FROM #TempSegmentInfo)
			AND Tic.TicketTypeId = 2 --OwnerShip
			AND Tic.[Status] = 0 -- To get only the tickets which are successful

			--Fetching RelevantTicket
			SELECT   @Cnt  = 0,
					 @newStartDate = NUll,
					 @newEndDate   = NULL

			SET	  @Cnt  = (SELECT COUNT(1) 
						   FROM #TempTicketNodeValidation
						   )


			WHILE @Cnt>0
			BEGIN
				SELECT  @TicketId = TicketId, 
						@newSegmentId = SegmentId, 
						@newStartDate = StartDate, 
						@newEndDate = EndDate
				FROM #TempTicketNodeValidation
				WHERE Rnum = @Cnt
				AND DATEDIFF(DD,StartDate,EndDate) < =100

				SELECT @Cnt = @Cnt-1

				INSERT INTO #TempResultTicketsForNodeValidation
				(
						 TicketID			
						,SegmentId		
						,OperationDate	
				)
				SELECT   udf.nodeId				AS 	TicketID
						,udf.elementId			AS  SegmentId
						,udf.dates				AS  OperationDate
				FROM [Admin].[udf_GetAllDates]( @newStartDate, @newEndDate, @TicketId, @newSegmentId)udf

				--Delete the irrelevant Data (Tickets which are not related to The Input Range)
				DELETE FROM #TempResultTicketsForNodeValidation
				WHERE OperationDate NOT BETWEEN @StartDate AND @EndDate

			END

			--NodeTag Validation
			SELECT   @Cnt  = 0,
					 @newStartDate = NUll,
					 @newEndDate = NULL

			SET	  @Cnt  = (SELECT COUNT(1) 
						   FROM #TempNodeTagNodeValidation
						   )


			WHILE @Cnt>0
			BEGIN
				SELECT  @NodeID = NodeId, 
						@NodeName = NodeName, 
						@newStartDate = StartDate, 
						@newEndDate = EndDate
				FROM #TempNodeTagNodeValidation
				WHERE Rnum = @Cnt

				SELECT @Cnt = @Cnt-1

				INSERT INTO #TempAllNodesForNodeValidation
				(
						 NodeId			
						,NodeName		
						,OperationDate	
				)
				SELECT   udf.nodeId				AS 	NodeId
						,udf.elementId			AS  NodeName
						,udf.dates				AS  OperationDate
				FROM [Admin].[udf_GetAllDates]( @newStartDate, @newEndDate, @NodeID, @NodeName)udf

				--Delete the irrelevant Data (Tickets which are not related to The Input Range)
				DELETE FROM #TempAllNodesForNodeValidation
				WHERE OperationDate NOT BETWEEN @StartDate AND @EndDate


			END


			SELECT 	DISTINCT
					 NVal.NodeId		AS NodeId		
					,NVal.NodeName		AS NodeName
					,Nval.OperationDate AS OperationDate
					,NodeStatus.Name	AS NodeStatus
					,CE.Name			AS SegmentName
			FROM #TempAllNodesForNodeValidation NVal
			INNER JOIN Admin.OwnershipNode OwnNode
			ON Nval.NodeId = OwnNode.NodeId
			INNER JOIN Admin.OwnershipNodeStatusType NodeStatus
			ON OwnNode.OwnershipStatusId =  NodeStatus.OwnershipNodeStatusTypeId
			INNER JOIN #TempResultTicketsForNodeValidation Tic
			ON Tic.TicketId = OwnNode.TicketId
			INNER JOIN Admin.CategoryElement CE
			ON CE.ElementId = TIC.SegmentId
			AND Tic.OperationDate = NVal.OperationDate
			WHERE NodeStatus.OwnershipNodeStatusTypeId NOT IN (9)--Get the Data for all NodeStatus which are not Approved
			AND NVal.OperationDate BETWEEN @StartDate AND @EndDate
			AND (OwnNode.NodeId = @InputNodeId OR @InputNodeId IS NULL)
			ORDER BY NVal.NodeName
					,Nval.OperationDate DESC

			IF OBJECT_ID('tempdb..#TempResultTicketsForNodeValidation') IS NOT NULL
			DROP TABLE #TempResultTicketsForNodeValidation

			IF OBJECT_ID('tempdb..#TempAllNodesForNodeValidation') IS NOT NULL
			DROP TABLE #TempAllNodesForNodeValidation

			IF OBJECT_ID('tempdb..#TempNodeTagNodeValidation') IS NOT NULL
			DROP TABLE #TempNodeTagNodeValidation

			IF OBJECT_ID('tempdb..#TempTicketNodeValidation') IS NOT NULL
			DROP TABLE #TempTicketNodeValidation

		END
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get the Logistic Node Validation data for the Excel file based on the Segment Id, Node Id, Start Date and End Date.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetLogisticNodeValidation',
    @level2type = NULL,
    @level2name = NULL
