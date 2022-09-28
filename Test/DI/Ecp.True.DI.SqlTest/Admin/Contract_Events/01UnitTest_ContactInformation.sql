-- /*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Feb-14-2020

-- Description:     These test cases are for View [Admin].[[ContractInformation]]
-- Expectations: This view must return contract details for 1 segment  all nodes and 1 segment 1 node
-- Contract dates should be within Note tage dates
-- Contract isDeleted flag must be 0

-- Database backup Used:	appdb_tst_0207
-- ==============================================================================================================================*/

-- CASE Element and 1 node
	select  * from [Admin].[ContractInformation] WHERE Element = 'Automation_6i2k5' and nodename like '%-_-Automation_olmli-_-%'
	-- VALIDATION
	SELECT * FROM ADMIN.NODE WHERE NAME= 'Automation_olmli' --16899
	SELECT * FROM ADMIN.NodeTag where ElementId=47338 -- Check -> conrtact start date>= nodetage start date and contract end date<= nodetage end date  
	SELECT  distinct * FROM ADMIN.Contract where SourceNodeId=16899 or DestinationNodeId=16899 and IsDeleted=0
-- CASE Element and all nodes
	select  * from [Admin].[ContractInformation] WHERE Element = 'Automation_6i2k5'
	select * From admin.CategoryElement WHERE NAME='Automation_6i2k5' -- ELEMENTID - 47338
	-- checking all nodes with this element 
	select * from admin.NodeTag where ElementId=47338  --16896,16897,16898,16899
	select * FROM ADMIN.Contract where SourceNodeId in (select NodeId from admin.NodeTag where ElementId=47338 ) or DestinationNodeId in (select NodeId from admin.NodeTag where ElementId=47338)

	
	-- CASE Element and 1 node
	select  * from [Admin].[ContractInformation] WHERE Element = 'Automation_xqeqh' and nodename like '%-_-Automation_y6b93-_-%'
	-- VALIDATION
	SELECT * FROM ADMIN.NODE WHERE NAME= 'Automation_y6b93' --16891
	SELECT * FROM ADMIN.NodeTag where ElementId=47312 -- Check -> conrtact start date>= nodetage start date and contract end date<= nodetage end date  
	SELECT  distinct * FROM ADMIN.Contract where SourceNodeId=16891 or DestinationNodeId=16891 and IsDeleted=0
-- CASE Element and all nodes
	select  * from [Admin].[ContractInformation] WHERE Element = 'Automation_xqeqh'
	select * From admin.CategoryElement WHERE NAME='Automation_xqeqh' -- ELEMENTID - 47312
	-- checking all nodes with this element 
	select * from admin.NodeTag where ElementId=47312  --16896,16897,16898,16899
	select * FROM ADMIN.Contract where SourceNodeId in (select NodeId from admin.NodeTag where ElementId=47312 ) or DestinationNodeId in (select NodeId from admin.NodeTag where ElementId=47312)

	-------- UNIT TEST CASE: FOR BUG 33409-------------
	/*
	-- ENVIRONMENT- DEV 
	-- PARAMTERES - Select "Automation_3e12u" as segment
	--				select "Todos" in Node
	--				select start date as "21/01/2020"
	--				select end date as "22/01/2023"
	-- EXPECTED OUTCOME-	Go to Report header check the dates being displayed
	--						Date in header should be same as date passed.
	*/
	-- Check Dimdate table - the max date in the table should be greater than enddate passed, otherwise by default max date of table will be displayed
		select * from admin.dimdate order by date desc
