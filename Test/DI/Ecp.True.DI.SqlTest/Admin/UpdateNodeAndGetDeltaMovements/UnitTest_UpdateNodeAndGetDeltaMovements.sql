DECLARE
 @TicketId				INT=29145,
@NodeList				[Admin].[NodeListType]

INSERT INTO @NodeList
VALUES(30812)
INSERT INTO @NodeList
VALUES(30816)

SELECT * FROM Admin.DeltaNode 
WHERE TicketId IN (29054, 29145 ) -- Before SP Execution

--DeltaNodeId	TicketId	NodeId	Status	CreatedBy	CreatedDate				LastModifiedBy	LastModifiedDate
--44				29145	30812	1		trueadmin	2020-07-16 08:39:11.550	NULL	NULL
--45				29145	30816	1		trueadmin	2020-07-16 08:39:11.550	NULL	NULL
--46				29054	30756	2		trueadmin	2020-07-16 08:40:03.797	NULL	NULL
--47				29054	30762	1		trueadmin	2020-07-16 08:40:11.750	NULL	NULL
--48				29145	30812	1		trueadmin	2020-07-16 08:42:54.413	NULL	NULL
--49				29145	30816	1		trueadmin	2020-07-16 08:42:54.413	NULL	NULL
--50				29145	30812	1		trueadmin	2020-07-16 08:43:29.510	NULL	NULL
--51				29145	30816	1		trueadmin	2020-07-16 08:43:29.510	NULL	NULL
--52				29145	30812	1		trueadmin	2020-07-16 08:45:07.700	NULL	NULL
--53				29145	30816	1		trueadmin	2020-07-16 08:45:07.700	NULL	NULL
--54				29145	30812	1		trueadmin	2020-07-16 08:46:04.673	NULL	NULL
--55				29145	30816	1		trueadmin	2020-07-16 08:46:04.673	NULL	NULL
--56				29145	30812	1		trueadmin	2020-07-16 08:46:51.583	NULL	NULL
--57				29145	30816	1		trueadmin	2020-07-16 08:46:51.583	NULL	NULL
--58				29145	30812	1		trueadmin	2020-07-16 08:50:35.487	NULL	NULL
--59				29145	30816	1		trueadmin	2020-07-16 08:50:35.487	NULL	NULL
--60				29145	30812	1		trueadmin	2020-07-16 08:51:02.850	NULL	NULL
--61				29145	30816	1		trueadmin	2020-07-16 08:51:02.850	NULL	NULL
--62				29145	30812	1		trueadmin	2020-07-16 08:51:25.173	NULL	NULL
--63				29145	30816	1		trueadmin	2020-07-16 08:51:25.173	NULL	NULL
--64				29145	30812	1		trueadmin	2020-07-16 08:51:33.907	NULL	NULL
--65				29145	30816	1		trueadmin	2020-07-16 08:51:33.907	NULL	NULL
--66				29145	30812	1		trueadmin	2020-07-16 08:51:58.857	NULL	NULL
--67				29145	30816	1		trueadmin	2020-07-16 08:51:58.857	NULL	NULL

EXEC Admin.usp_UpdateNodeAndGetDeltaMovements_Test   @TicketId	, 
													@NodeList	


SELECT * FROM Admin.DeltaNode 
WHERE TicketId IN (29054, 29145 ) --After SP Execution previous ticket with same start, end date and segment got deleted and new nodes from node list input inserted

--DeltaNodeId	TicketId	NodeId	Status	CreatedBy	CreatedDate				LastModifiedBy	LastModifiedDate
--44				29145	30812	1		trueadmin	2020-07-16 08:39:11.550		NULL		NULL
--45				29145	30816	1		trueadmin	2020-07-16 08:39:11.550		NULL		NULL
--48				29145	30812	1		trueadmin	2020-07-16 08:42:54.413		NULL		NULL
--49				29145	30816	1		trueadmin	2020-07-16 08:42:54.413		NULL		NULL
--50				29145	30812	1		trueadmin	2020-07-16 08:43:29.510		NULL		NULL
--51				29145	30816	1		trueadmin	2020-07-16 08:43:29.510		NULL		NULL
--52				29145	30812	1		trueadmin	2020-07-16 08:45:07.700		NULL		NULL
--53				29145	30816	1		trueadmin	2020-07-16 08:45:07.700		NULL		NULL
--54				29145	30812	1		trueadmin	2020-07-16 08:46:04.673		NULL		NULL
--55				29145	30816	1		trueadmin	2020-07-16 08:46:04.673		NULL		NULL
--56				29145	30812	1		trueadmin	2020-07-16 08:46:51.583		NULL		NULL
--57				29145	30816	1		trueadmin	2020-07-16 08:46:51.583		NULL		NULL
--58				29145	30812	1		trueadmin	2020-07-16 08:50:35.487		NULL		NULL
--59				29145	30816	1		trueadmin	2020-07-16 08:50:35.487		NULL		NULL
--60				29145	30812	1		trueadmin	2020-07-16 08:51:02.850		NULL		NULL
--61				29145	30816	1		trueadmin	2020-07-16 08:51:02.850		NULL		NULL
--62				29145	30812	1		trueadmin	2020-07-16 08:51:25.173		NULL		NULL
--63				29145	30816	1		trueadmin	2020-07-16 08:51:25.173		NULL		NULL
--64				29145	30812	1		trueadmin	2020-07-16 08:51:33.907		NULL		NULL
--65				29145	30816	1		trueadmin	2020-07-16 08:51:33.907		NULL		NULL
--66				29145	30812	1		trueadmin	2020-07-16 08:51:58.857		NULL		NULL
--67				29145	30816	1		trueadmin	2020-07-16 08:51:58.857		NULL		NULL
--68				29145	30812	1		trueadmin	2020-07-16 09:01:18.363		NULL		NULL
--69				29145	30816	1		trueadmin	2020-07-16 09:01:18.363		NULL		NULL




--Select * from admin.DeltaNode
--INSERT INTO Admin.DeltaNode (TicketId, NodeId, Status, CreatedBy)
----VALUES(29054, 30756, 2, 'trueadmin')
--VALUES(29054, 30762, 1, 'trueadmin')

--select convert(varchar,t.StartDate, 1) from admin.ticket t
--where t.StartDate = '2020-01-07'--@TicketStartDate
--	AND t.EndDate = '2020-10-07'--@TicketEndDate
--	AND t.CategoryElementId = 3

--select t.StartDate, t.EndDate, t.CategoryElementId, count(*) from admin.ticket t
--group by t.StartDate, t.EndDate, t.CategoryElementId
--having count(*)>1



--select b.nodeid,a.* from admin.Ticket a left join admin.nodetag b on a.categoryelementid=b.elementid
--where a.ticketid In (29054)