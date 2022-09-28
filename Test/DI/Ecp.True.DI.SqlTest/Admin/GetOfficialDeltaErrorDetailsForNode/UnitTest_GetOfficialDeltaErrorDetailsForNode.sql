/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jul-09-2020
-- <Description>:	Unit test Admin.[usp_GetOfficialDeltaErrorDetailsForNode] by input parameter @DeltaNodeId
-- ================================================================================================================================================*/

SELECT * FROM Admin.DeltaNode
--DeltaNodeId	TicketId	NodeId	Status	CreatedBy	CreatedDate				LastModifiedBy	LastModifiedDate
--1				24141		30756	1		trueadmin	2020-07-08 17:22:18.380		NULL		NULL
--2				24141		30762	1		trueadmin	2020-07-08 17:22:18.380		NULL		NULL
--3				24142		30767	2		trueadmin	2020-07-08 17:22:18.380		NULL		NULL
--4				24142		30800	2		trueadmin	2020-07-08 17:22:18.380		NULL		NULL


INSERT INTO Admin.DeltaNodeError ( [DeltaNodeId], [InventoryProductId] , [MovementTransactionId], [ErrorMessage], [CreatedBy] )
VALUES(1,NULL, 11022, 'Unit Test' ,'trueadmin' )

INSERT INTO Admin.DeltaNodeError ( [DeltaNodeId], [InventoryProductId] , [MovementTransactionId], [ErrorMessage], [CreatedBy] )
VALUES(1,NULL, 11023, 'Unit Test' ,'trueadmin' )

INSERT INTO Admin.DeltaNodeError ( [DeltaNodeId], [InventoryProductId] , [MovementTransactionId], [ErrorMessage], [CreatedBy] )
VALUES(2,NULL, 21742, 'Unit Test' ,'trueadmin' )	

INSERT INTO Admin.DeltaNodeError ( [DeltaNodeId], [InventoryProductId] , [MovementTransactionId], [ErrorMessage], [CreatedBy] )
VALUES(2,8661, NULL, 'Unit Test' ,'trueadmin' )	

EXEC Admin.[usp_GetOfficialDeltaErrorDetailsForNode] 2

--Identifier		Type		SourceNode			DestinationNode		SourceProduct		DestinationProduct	Quantity	Unit	Date		Error
--8596941			Movimiento	Automation_wd2xe	Automation_f7p35	CRUDO CAMPO CUSUCO	CRUDO CAMPO CUSUCO	416871.82	Bbl		2020-07-08	Unit Test
--DEFECT 666419		Inventario	Automation_9tas7	NULL				CRUDO CAMPO MAMBO	NULL				54550.00	Bbl		2020-07-04	Unit Test

EXEC Admin.[usp_GetOfficialDeltaErrorDetailsForNode] 1

--Identifier	Type		SourceNode	DestinationNode	SourceProduct	DestinationProduct	Quantity	Unit	Date		Error
--1				Movimiento	NULL		NULL			NULL			NULL				2345.82		Bbl		2020-05-25	Unit Test
--2				Movimiento	NULL		NULL			NULL			NULL				6888.82		Bbl		2020-05-25	Unit Test
 