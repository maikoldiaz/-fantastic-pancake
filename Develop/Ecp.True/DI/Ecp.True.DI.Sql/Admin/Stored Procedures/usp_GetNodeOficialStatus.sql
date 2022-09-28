/*-- ======================================================================================================================================================================================
-- Author:          InterGrupo 
-- Created Date:  Jun-15-2021 
-- :  This Procedure is used to show node oficial statuses for Power Bi report based on Segment, InitialDate, EndDate, ExecutionId.
-- ======================================================================================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_GetNodeOficialStatus] 
(
	@Segment               NVARCHAR(256), 
	@InitialDate           DATE, 
	@EndDate               DATE, 
	@ExecutionId           NVARCHAR(250)
)
AS 
  BEGIN 

	declare  @TodayDateTime	DATE  = admin.Udf_gettruedate(),
			    @PreviousDay	DATE  = admin.Udf_gettruedate() - 1

  	 IF OBJECT_ID('tempdb..#TempNodeDelta')IS NOT NULL
	 DROP TABLE #TempNodeDelta


	  DELETE FROM [Admin].NodeOficialStatus 
      WHERE  CreatedDate < (SELECT @PreviousDay) 
              OR ( ExecutionId = @ExecutionId )

	 select NodeName,NodeId,max(DeltaNodeId)  DeltaNodeId
	 into #TempNodeDelta
	 from [Admin].[view_DeltaNode]
	 where Segment = @Segment and StartDate = @InitialDate and EndDate = @EndDate
	 group by NodeName,NodeId

	 	 

	 INSERT INTO [Admin].NodeOficialStatus 
	 (
		SegmentName,
		SystemName,
		NodeName,
		statusNode,
		StatusDateChange,
		Approver,
		Comment,
		ExecutionId,
		CreatedBy,
		CreatedDate
	 )
		 select vwdn.Segment,[Systemgrp].[Name],vwdn.NodeName,
		 vwdn.[Status],
		 vwdn.LastModifiedDate ExecutionDate,
		 DN.Approvers,
		 DN.Comment,
		 @ExecutionId,
		 'ReportUser',
		 @TodayDateTime
		 from [Admin].[view_DeltaNode] vwdn
		 inner join [Admin].DeltaNode DN on vwdn.DeltaNodeId = DN.DeltaNodeId
		 left join (select NT.NodeId,[System].[Name] 
			from [admin].NodeTag NT
			inner join [Admin].CategoryElement [System] 
			on NT.ElementId = [System].ElementId and CategoryId = 8 and NT.EndDate >= getdate()) Systemgrp
		 on vwdn.NodeId = Systemgrp.NodeId
		 where vwdn.DeltaNodeId in (select DeltaNodeId from #TempNodeDelta) 


 END
