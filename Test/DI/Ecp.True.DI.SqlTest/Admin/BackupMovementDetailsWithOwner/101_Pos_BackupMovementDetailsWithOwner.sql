/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Jun-16-2020

-- Description:     These test cases are for View [Admin].[BackupMovementDetailsWithOwner] 

-- Database backup Used:	dbaeuecpdevtrue
-- ==============================================================================================================================*/

UPDATE [Offchain].[Movement]
   SET BackupMovementId = '637278077563734992'
 WHERE MovementId = '637278077563735061'

UPDATE [Offchain].[Movement]
   SET BackupMovementId = '637278678975490524'
 WHERE MovementId = '637278678975497050'

UPDATE [Offchain].[Movement]
   SET BackupMovementId = '637278077563919954'
 WHERE MovementId = '637278077563920046'
/*
-- Expected Output : This View should return the list of backup movement details 
                --BackupMovementId's
				                   1. 637278077563734992
				                   2. 637278678975490524
								   3. 637278077563919954


-- Actual Output:

MovementId	      BatchId	MovementTransactionId	OperationalDate	Operacion	SourceNode	        DestinationNode	    SourceProduct	    DestinationProduct	NetStandardVolume	GrossStandardVolume	MeasurementUnit	EventType	SystemName	Category	Element	                 NodeName	         CalculationDate	      BackupmovementId	GlobalmovementId	RNo
637278077563734992	NULL			11494	          2020-06-13	Tolerancia	Automation_xfarm	NULL	            CRUDO CAMPO CUSUCO	NULL	            1472.04						NULL				Bbl		Insert			NULL	Segmento	Automation_599au	-_-Automation_xfarm-_-	 2020-06-13						NULL				NULL		1
637278077563734992	NULL			11494	          2020-06-13	Tolerancia	Automation_xfarm	NULL	            CRUDO CAMPO CUSUCO	NULL	            1472.04						NULL				Bbl		Insert			NULL	Segmento	Automation_599au	-_-Todos-_-				 2020-06-13						NULL				NULL		2
637278077563919954	NULL			11500	          2020-06-14	Tolerancia	NULL	            Automation_xfarm	NULL	            CRUDO CAMPO MAMBO	1699.32						NULL				Bbl		Insert			NULL	Segmento	Automation_599au	-_-Automation_xfarm-_-	 2020-06-14						NULL				NULL		3
637278077563919954	NULL			11500	          2020-06-14	Tolerancia	NULL	            Automation_xfarm	NULL	            CRUDO CAMPO MAMBO	1699.32						NULL				Bbl		Insert			NULL	Segmento	Automation_599au	-_-Todos-_-				 2020-06-14						NULL				NULL		4
637278678975490524	NULL			11529	          2020-06-14	Venta	    Automation_xfarm	Automation_hp9im	CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	1000.00						NULL				Bbl		Insert			NULL	Segmento	Automation_599au	-_-Automation_xfarm-_-	 2020-06-14						NULL				NULL		5
637278678975490524	NULL			11529	          2020-06-14	Venta	    Automation_xfarm	Automation_hp9im	CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	1000.00						NULL				Bbl		Insert			NULL	Segmento	Automation_599au	-_-Automation_hp9im-_-	 2020-06-14						NULL				NULL		6
637278678975490524	NULL			11529	          2020-06-14	Venta	    Automation_xfarm	Automation_hp9im	CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	1000.00						NULL				Bbl		Insert			NULL	Segmento	Automation_599au	-_-Todos-_-	             2020-06-14						NULL				NULL		7
*/