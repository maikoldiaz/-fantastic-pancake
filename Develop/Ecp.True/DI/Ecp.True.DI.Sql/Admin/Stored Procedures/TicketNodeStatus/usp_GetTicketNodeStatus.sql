/*-- ======================================================================================================================================================================================
-- Author:          Microsoft 
-- Created Date:  Mar-27-2020 
-- Updated Date:  Apr-13-2020 
--                Apr-30-2020  Modified Code For Performance Reasons
-- :  This Procedure is used to show node statuses for a ticket for Power Bi report based on Segment, InitialDate, EndDate, ExecutionId and ANSConfigurationValue .
--                  Updates: Added Configuration parameter for number of days 
-- EXEC ADMIN.Usp_getticketnodestatus  'perfelement_1_637237662600637501','2020-04-25','2020-04-28','','5'-->2.836-->16
-- ======================================================================================================================================================================================*/
CREATE PROCEDURE ADMIN.Usp_GetTicketNodeStatus 
(
	@Segment               NVARCHAR(256), 
	@InitialDate           DATE, 
	@EndDate               DATE, 
	@ExecutionId           NVARCHAR(250), 
	@ANSConfigurationValue INT 
)
AS 
  BEGIN 
		DECLARE @maxdate		DATE, 
                @SegmentID		INT,
			    @TodayDateTime	DATE  = admin.Udf_gettruedate(),
			    @PreviousDay	DATE  = admin.Udf_gettruedate() - 1

	 IF OBJECT_ID('tempdb..#TempOwnershipNodeTicketNodeStatus')IS NOT NULL
	 DROP TABLE #TempOwnershipNodeTicketNodeStatus

      -- Deleting records from [Admin].[Operational] table which are older than 24 hours based on colombian timestamp 
      DELETE FROM [Admin].TicketNodeStatus 
      WHERE  CreatedDate < (SELECT @PreviousDay) 
              OR ( ExecutionId = @ExecutionId ) 

      SELECT @SegmentID = elementid 
      FROM   ADMIN.CategoryElement 
      WHERE  NAME = @Segment 
	  AND CategoryId = 2--Segment 


      --Pushing the Required Data To TempTable
      SELECT		  Own.OwnershipNodeId			AS OwnershipNodeId,
					  Own.OwnershipStatusId			AS OwnershipStatusId,
                      Own.TicketId					AS TicketId, 
                      Tick.StartDate				AS StartDate, 
                      Tick.EndDate					AS EndDate, 
                      Own.NodeId					AS NodeId,
                      COALESCE(Own.RegistrationDate, Own.LastModifiedDate,Own.CreatedDate) AS StatusDateChange, 
                      Own.ApproverAlias				AS Approver, 
                      Own.Comment					AS Comment,
					  Tick.categoryelementid		AS CategoryElementid
	  INTO #TempOwnershipNodeTicketNodeStatus
      FROM   ADMIN.Ticket Tick 
      INNER JOIN [Admin].[OwnershipNode] Own 
      ON Tick.TicketId = Own.TicketId 
      AND Tick.TicketTypeId = 2 
      WHERE  Tick.Status <> 2 -- Status is not Failed
      AND Tick.StartDate BETWEEN @InitialDate AND @EndDate 
      AND Tick.EndDate BETWEEN @InitialDate AND @EndDate 

      -- SELECTING MAXIMUM OPERATIONAL DATE AS CURRENT DATE 
      SET @maxdate = Admin.udf_GETTRUEDATE();

      INSERT INTO [Admin].TicketNodeStatus 
                  (
                   OwnershipNodeId, 
                   TicketId, 
                   Startdate, 
                   Enddate, 
                   NodeId, 
                   SegmentId, 
                   SegmentName, 
                   SystemId, 
                   SystemName, 
                   NodeName, 
                   OwnershipNodeStatusId, 
                   statusNode, 
                   StatusDateChange, 
                   Approver, 
                   Comment, 
                   ExecutionId, 
                   ReportConfiguartionValue, 
                   CalculatedDays, 
                   NotInApprovedState, 
                   [CreatedBy]
                   ) 
      SELECT DISTINCT Own.OwnershipNodeId AS OwnershipNodeId, 
                      Own.TicketId AS TicketId, 
                      Own.StartDate AS StartDate, 
                      Own.EndDate AS EndDate, 
                      Own.NodeId AS NodeId, 
                      Segment.ElementId AS SegmentId, 
                      Segment.NAME AS SegmentName, 
                      Syst.ElementId AS SystemId, 
                      Syst.NAME AS systemName, 
                      n.NAME AS NodeName, 
                      owns.OwnershipNodeStatusTypeid 
                      AS OwnershipNodeStatusid, 
                      owns.NAME AS StatusNode, 
                      Own.StatusDateChange AS  StatusDateChange, 
                      Own.Approver AS Approver, 
                      Own.Comment AS Comment, 
                      @executionId AS ExecutionId, 
                      @ANSConfigurationValue AS ReportConfiguartionValue, 
                      --DATEDIFF(day,tick.enddate,@maxdate) as daydiff 
                      CASE 
                        WHEN owns.NAME != 'Aprobado' 
                             AND Datediff(day, Own.EndDate, @maxdate) >= 
                                 @ANSConfigurationValue + 5 
                      THEN Cast(@ANSConfigurationValue+5 AS NVARCHAR(50)) 
                      + '+ dias' 
                        WHEN owns.NAME != 'Aprobado' 
                             AND Datediff(day, Own.EndDate, @maxdate) >= 
                                 @ANSConfigurationValue + 2 
                      THEN Cast(@ANSConfigurationValue+3 AS NVARCHAR(50)) 
                      + '-' 
                      + Cast(@ANSConfigurationValue+4 AS NVARCHAR(50)) 
                      + ' dias' 
                        WHEN owns.NAME != 'Aprobado' 
                             AND Datediff(day, Own.EndDate, @maxdate) >= 
                                 @ANSConfigurationValue + 1 
                      THEN Cast(@ANSConfigurationValue+1 AS NVARCHAR(50)) 
                      + '-' 
                      + Cast(@ANSConfigurationValue+2 AS NVARCHAR(50)) 
                      + ' dias' 
                        ELSE NULL 
                      END 
                      AS CalculatedDays, 
                      CASE 
                        WHEN owns.NAME != 'Aprobado' 
                             AND Datediff(day, Own.EndDate, @maxdate) > 
                                 @ANSConfigurationValue THEN 1 
                        ELSE 0 
                      END 
                      AS NotInApprovedState, 
                      'ReportUser' AS CreatedBy 
      FROM   #TempOwnershipNodeTicketNodeStatus Own     
      INNER JOIN [Admin].NodeTag SegmentGrp 
      ON SegmentGrp.NodeId = Own.NodeId 
	  AND SegmentGrp.ElementId = @SegmentID
      AND @TodayDateTime BETWEEN SegmentGrp.StartDate AND SegmentGrp.EndDate  
	  
      --add the logic to history  
      INNER JOIN [Admin].CategoryElement Segment 
      ON Segment.ElementId = SegmentGrp.ElementId 
      INNER JOIN [Admin].[Node] N 
      ON Own.nodeid = N.nodeid 
      INNER JOIN [Admin].[OwnershipNodeStatusType] owns 
      ON Own.[OwnershipStatusId] = owns.[OwnershipNodeStatusTypeId]
      LEFT JOIN [Admin].NodeTag SystemGrp 
      ON SystemGrp.NodeId = Own.NodeId 
      AND SystemGrp.ElementId IN 
								  (SELECT ElementId 
								   FROM   [Admin].[CategoryElement] 
								   WHERE  [CategoryId] = 8
								   )--System 
      AND @TodayDateTime BETWEEN SystemGrp.StartDate AND SystemGrp.EndDate 
      --add the logic to history  

      LEFT JOIN [Admin].CategoryElement Syst 
      ON Syst.ElementId = SystemGrp.ElementId
  END 

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to show node statuses for a ticket for Power Bi report based on Segment, InitialDate, EndDate, ExecutionId and ANSConfigurationValue .',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'Usp_GetTicketNodeStatus',
    @level2type = NULL,
    @level2name = NULL