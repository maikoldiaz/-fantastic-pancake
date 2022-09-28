/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Apr-06-2020
-- Description:     These test cases are to test stored procedure usp_OwnershipCleanup
-- Database backup Used:	dbaeuecpdevtrue_mi-asc-ecp-dev-mainsqlmidev.public.c105ff79c574.database.windows.net,3342_2020_04_06_13_05
-- ==============================================================================================================================*/

-- Case 1 : CORRECT Ticket

DECLARE @TicketId INT = 6

EXEC  admin.usp_GetTicketNodeStatus @TicketId

SELECT * FROM [Offchain].[Ownership]
SELECT * FROM [Admin].[OwnershipNode]
SELECT * FROM [Admin].[OwnershipNodeError]
SELECT * FROM [Admin].[Ticket]
-- Records deleted.

--- Case 2 : Ticket corresponding to the provided TicketId not exists
DECLARE @TicketId INT = 9

EXEC  admin.usp_GetTicketNodeStatus @TicketId

SELECT * FROM [Offchain].[Ownership]
SELECT * FROM [Admin].[OwnershipNode]
SELECT * FROM [Admin].[OwnershipNodeError]
SELECT * FROM [Admin].[Ticket]
-- No such ticket exists