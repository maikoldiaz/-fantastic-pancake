-- /*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Feb-14-2020

-- Description:     These test cases are for View [Admin].[[eventinformation]]
-- Expectations: This view must return event details for 1 segment  all nodes and 1 segment 1 node
-- Contract dates should be within Note tage dates
-- Contract isDeleted flag must be 0

-- Database backup Used:	appdb_tst_0207
-- ==============================================================================================================================*/


select * from admin.eventinformation where element='Automation_9nppw' and nodename like '%-_-Automation_6ppb7-_-%'
	-- VALIDATION
	SELECT * FROM ADMIN.CategoryElement WHERE NAME ='Automation_9nppw'--46834
	SELECT * FROM ADMIN.NODE WHERE NAME= 'Automation_6ppb7' --16784
	SELECT * FROM ADMIN.NodeTag where ElementId=46834 -- Check -> conrtact start date>= nodetage start date and contract end date<= nodetage end date  
	SELECT  distinct * FROM ADMIN.EVENT where SourceNodeId=16784 or DestinationNodeId=16784 and IsDeleted=0
	-- CASE ELEMENT ALL NODES
	select * from admin.eventinformation where element='Automation_wfknr'

	-- checking all nodes with this element 
	select * from admin.NodeTag where ElementId=46841  --
	SELECT * FROM ADMIN.CategoryElement WHERE NAME ='Automation_wfknr'
	select * FROM ADMIN.Event where SourceNodeId in (select NodeId from admin.NodeTag where ElementId=46841 ) or DestinationNodeId in (select NodeId from admin.NodeTag where ElementId=46841)
