
/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Jan-27-2020

-- Description:     These test cases are for SP [usp_BalanceSummaryAggregate]

-- Database backup Used:	dev_appdb_27Jan2020_130006
-- ==============================================================================================================================*/


/*
The below update statements are required to create the data to populate the fields of [Admin].[OwnershipCalculation]
corresponding to Admin.OwnershipNode where OwnershipNodeId = 361.

*/


--===================== TestCase1: When the [Inputs] value is equal to 0.0 ===============================
-- Expected output: The value of Control column should be 'error' and for remaining all columns it should be the Sum.

Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 1.0, FinalInventoryVolume = 7.0  Where OwnershipCalculationId = 1415
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 2.0, FinalInventoryVolume = 8.0  Where OwnershipCalculationId = 1404
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 3.0, FinalInventoryVolume = 9.0  Where OwnershipCalculationId = 1400
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 4.0, FinalInventoryVolume = 10.0 Where OwnershipCalculationId = 1417
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 5.0, FinalInventoryVolume = 11.0 Where OwnershipCalculationId = 1387
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 6.0, FinalInventoryVolume = 12.0 Where OwnershipCalculationId = 1401

Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 111.0, FinalInventoryVolume = 117.0  Where OwnershipCalculationId = 1426
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 211.0, FinalInventoryVolume = 118.0  Where OwnershipCalculationId = 1388
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 311.0, FinalInventoryVolume = 119.0  Where OwnershipCalculationId = 1402
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 411.0, FinalInventoryVolume = 1110.0 Where OwnershipCalculationId = 1418
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 511.0, FinalInventoryVolume = 1111.0 Where OwnershipCalculationId = 1389
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 611.0, FinalInventoryVolume = 1112.0 Where OwnershipCalculationId = 1403


EXEC [Admin].[usp_BalanceSummaryWithOwnership] 361

EXEC [Admin].[usp_BalanceSummaryAggregate] 361		-->	 Actual SP execution.

/*
========================================= Outputs captured =========================================

// Output for [Admin].[usp_BalanceSummaryWithOwnership]
--------------------------------------------------------

ProductId	Product	Owner	InitialInventory	Inputs	Outputs	IdentifiedLosses	Interface	Tolerance	UnidentifiedLosses	FinalInventory	Volume	MeasurementUnit	Control
10000002318	CRUDO CAMPO MAMBO	ECOPETROL	1.0000000000000000	0.0000000000000000	623736.8166018262000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	-623735.8166018262000000	Bbl	0.0
10000002318	CRUDO CAMPO MAMBO	EQUION	4.0000000000000000	0.0000000000000000	432000.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	-431996.0000000000000000	Bbl	0.0
10000002318	CRUDO CAMPO MAMBO	REFICAR	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	Bbl	0.0
10000002372	CRUDO CAMPO CUSUCO	ECOPETROL	111.0000000000000000	0.0000000000000000	1003.5735986200000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	-892.5735986200000000	Bbl	0.0
10000002372	CRUDO CAMPO CUSUCO	EQUION	411.0000000000000000	0.0000000000000000	1003.5735986200000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	-592.5735986200000000	Bbl	0.0
10000002372	CRUDO CAMPO CUSUCO	REFICAR	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	Bbl	0.0


// Output for [Admin].[usp_BalanceSummaryAggregate]
--------------------------------------------------------

InitialInventory	Inputs	Outputs	IdentifiedLosses	Interface	Tolerance	UnidentifiedLosses	FinalInventory	Volume	MeasurementUnit	Control	OwnershipStatusId
527.0000000000000000	0.0000000000000000	1057743.9637990662000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	0.0000000000000000	-1057216.9637990662000000	Bbl	error	5


*/




--===================== TestCase2: When the [Inputs] value is not equal to 0.0 ===============================
-- Expected output: The value all columns it should be the Sum, and Control column should be calculated as (Volume/Inputs).

Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 1.0, FinalInventoryVolume = 7.0  Where OwnershipCalculationId = 1415
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 2.0, FinalInventoryVolume = 8.0  Where OwnershipCalculationId = 1404
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 3.0, FinalInventoryVolume = 9.0  Where OwnershipCalculationId = 1400
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 4.0, FinalInventoryVolume = 10.0 Where OwnershipCalculationId = 1417
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 5.0, FinalInventoryVolume = 11.0 Where OwnershipCalculationId = 1387
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 6.0, FinalInventoryVolume = 12.0 Where OwnershipCalculationId = 1401

Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 111.0, FinalInventoryVolume = 117.0  Where OwnershipCalculationId = 1426
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 211.0, FinalInventoryVolume = 118.0  Where OwnershipCalculationId = 1388
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 311.0, FinalInventoryVolume = 119.0  Where OwnershipCalculationId = 1402
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 411.0, FinalInventoryVolume = 1110.0 Where OwnershipCalculationId = 1418
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 511.0, FinalInventoryVolume = 1111.0 Where OwnershipCalculationId = 1389
Update [Admin].[OwnershipCalculation] Set InitialInventoryVolume = 611.0, FinalInventoryVolume = 1112.0 Where OwnershipCalculationId = 1403

Update [Admin].[OwnershipCalculation] Set InputVolume = 102.0  Where OwnershipCalculationId = 1415


EXEC [Admin].[usp_BalanceSummaryWithOwnership] 361

EXEC [Admin].[usp_BalanceSummaryAggregate] 361		-->	 Actual SP execution.

/*
========================================= Outputs captured =========================================

// Output for [Admin].[usp_BalanceSummaryWithOwnership]
--------------------------------------------------------

ProductId	Product	Owner	InitialInventory	Inputs	Outputs	IdentifiedLosses	Interface	Tolerance	UnidentifiedLosses	FinalInventory	Volume	MeasurementUnit	Control
10000002318	CRUDO CAMPO MAMBO	ECOPETROL	1	102	623736.8166	0	0	0	0	0	-623633.8166	Bbl	0
10000002318	CRUDO CAMPO MAMBO	EQUION	4	0	432000	0	0	0	0	0	-431996	Bbl	0
10000002318	CRUDO CAMPO MAMBO	REFICAR	0	0	0	0	0	0	0	0	0	Bbl	0
10000002372	CRUDO CAMPO CUSUCO	ECOPETROL	111	0	1003.573599	0	0	0	0	0	-892.5735986	Bbl	0
10000002372	CRUDO CAMPO CUSUCO	EQUION	411	0	1003.573599	0	0	0	0	0	-592.5735986	Bbl	0
10000002372	CRUDO CAMPO CUSUCO	REFICAR	0	0	0	0	0	0	0	0	0	Bbl	0


// Output for [Admin].[usp_BalanceSummaryAggregate]
--------------------------------------------------------

InitialInventory	Inputs	Outputs	IdentifiedLosses	Interface	Tolerance	UnidentifiedLosses	FinalInventory	Volume	MeasurementUnit	Control	OwnershipStatusId
527	102	1057743.964	0	0	0	0	0	-1057114.964	Bbl	-10363.87219	5


*/