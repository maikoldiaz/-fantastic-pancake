/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Mar-30-2020
-- Description:     These test cases are to test stored procedure usp_GetTicketNodeStatus
-- Database backup Used:	appdb_dev_0327
-- ==============================================================================================================================*/

-- Case 1 : CORRECT DATE RANGE

DECLARE @Segment NVARCHAR(256)='Transporte'
,@InitialDate DATE='2020-02-22'
,@EndDate DATE='2020-02-29'
,@ExecutionId	NVARCHAR(250)='b1fa39be-84e3-48bd-955d-44b714a4cfa2'

EXEC  admin.usp_GetTicketNodeStatus @Segment ,@InitialDate,@EndDate ,@ExecutionId	

SELECT * FROM [Admin].TicketNodeStatus 
-- 769 rows within date range inserted into the table

--- Case 2 : INCORRECT DATE RANGE
DECLARE @Segment NVARCHAR(256)='Transporte'
,@InitialDate DATE='2020-02-22'
,@EndDate DATE='2020-02-26'
,@ExecutionId	NVARCHAR(250)='b1fa39be-84e3-48bd-955d-44b714a4cfa2_2'

EXEC  admin.usp_GetNodeStatus @Segment ,@InitialDate,@EndDate ,@ExecutionId	

SELECT * FROM [Admin].TicketNodeStatus 

-- 0 rows inserted ; because no matching date range values