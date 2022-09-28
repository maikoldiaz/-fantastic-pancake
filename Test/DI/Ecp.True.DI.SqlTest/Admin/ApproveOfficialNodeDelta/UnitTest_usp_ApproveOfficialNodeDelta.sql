 SELECT * FROM Admin.DeltaNode
-- DeltaNodeId	TicketId	NodeId	Status	CreatedBy	CreatedDate				LastModifiedBy	LastModifiedDate
--1				24141		30756	1		trueadmin	2020-07-08 17:22:18.380		NULL			NULL
--2				24141		30762	1		trueadmin	2020-07-08 17:22:18.380		NULL			NULL
--3				24142		30767	2		trueadmin	2020-07-08 17:22:18.380		NULL			NULL
--4				24142		30800	2		trueadmin	2020-07-08 17:22:18.380		NULL			NULL
--5				24142		30801	1		trueadmin	2020-07-09 06:12:13.680		NULL			NULL
 
 
 EXEC [Admin].[usp_ApproveOfficialNodeDelta] 137254, 30816 -- Predecessor node having same segment as input segment, and predecessor node is not set to status = '1' (assuming this is status for Sent to Approval)

--(8 rows affected)

--(2 rows affected)
--Msg 50000, Level 15, State 1, Procedure Admin.usp_ApproveOfficialNodeDelta , Line 114 [Batch Start Line 9]
--Approve Official Node Delta Fail

--Completion time: 2020-07-09T16:46:34.5631233+05:30

--Predecessor Nodes for 30816
--NodeId	SegmentId
--30817	137258
--30817	137254
--30817	137255
--30818	137260
--30818	137254
--30818	137255
--30817	137271
--30818	137271


 EXEC [Admin].[usp_ApproveOfficialNodeDelta ] 137182, 30800 -- Predecessor node having same segment as input segment, and predecessor node is  set to status = '1' (assuming this is status for Sent to Approval)

 
--(8 rows affected)

--(2 rows affected)

--Completion time: 2020-07-09T16:47:02.9096682+05:30

--Predecessor Nodes for 30800
--NodeId	SegmentId
--30801	137186
--30801	137182
--30801	137183
--30802	137188
--30802	137182
--30802	137183
--30801	137199
--30802	137199