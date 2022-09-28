/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Feb-18-2020
-- Description:    Unit test cases for view [Admin].[BalanceControl]
-- Database backup Used:	local backup copy of appdb
-- ==============================================================================================================================*/
/*
The following update command is used to update the data in newly added calculation fields in Unbalance table.
*/
--===================================  UPDATE [Admin].[BalanceControl] ===========================================================================================================================================================================
UPDATE 
	Offchain.Unbalance
SET 
	CalculationDate = '2020-01-01',
	Inputs = 874768.10,
	Unbalance = 1444.70,
	AverageUncertainty = 0.00022,
	Warning = 0.00037,
	[Action] = 0.00045,
	ControlTolerance = 0.00067
WHERE 
	TicketId = 23716
AND NodeId = 132
AND UnbalanceId = 25


UPDATE 
	Offchain.Unbalance
SET
	CalculationDate = '2020-01-02' ,
	Inputs = 667475.99,
	Unbalance = 1102.62,
	AverageUncertainty = 0.00039,
	Warning = 0.00065,
	[Action] = 0.00079,
	ControlTolerance = 0.00118
WHERE 
	TicketId = 23716
AND NodeId = 132
AND UnbalanceId = 26
--===================== TestCase: To check if the View returns data from the table for a particular segment, node and calculation date =============================================================================================================

SELECT * FROM Admin.BalanceControl
WHERE Element = 'Transporte' 
AND NodeName = 'Automation_c8k67' 
AND CalculationDate BETWEEN '2020-01-01' AND '2020-01-02'

/*
========================================================== Output Captured ===================================================================================================================================================================================
Category	Element		NodeName			CalculationDate	Product			ProductId	TicketId	NodeId	Input					Unbalance				StandardUncertainty		AverageUncertainty	Warning				Action				ControlTolerance	-AverageUncertainty	-Warning	-Action	-ControlTolerance
Segmento	Transporte	Automation_c8k67	2020-01-01		CRUDO MEZCLA	10000003006	23716		132		874768.1000000000000000	1444.7000000000000000	344.2900000000000000	0.0002200000000000	0.0003700000000000	0.0004500000000000	0.0006700000000000	-0.0002200000000000	-0.0003700000000000	-0.0004500000000000	-0.0006700000000000
Segmento	Transporte	Automation_c8k67	2020-01-02		CRUDO GALAN		10000003004	23716		132		667475.9900000000000000	1102.6200000000000000	262.7000000000000000	0.0003900000000000	0.0006500000000000	0.0007900000000000	0.0011800000000000	-0.0003900000000000	-0.0006500000000000	-0.0007900000000000	-0.0011800000000000
*/

--##############################################################################################################################################################################################################################################################