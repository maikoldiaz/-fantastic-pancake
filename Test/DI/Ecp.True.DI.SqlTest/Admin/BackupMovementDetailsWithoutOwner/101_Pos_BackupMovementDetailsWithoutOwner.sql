/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Jun-16-2020

-- Description:     These test cases are for View [Admin].[BackupMovementDetailsWithoutOwner] 

-- Database backup Used:	dbaeuecpdevtrue
-- ==============================================================================================================================*/

UPDATE [Offchain].[Movement]
   SET BackupMovementId = '637268412220504405'
 WHERE MovementId = '637268422516650249'

UPDATE [Offchain].[Movement]
   SET BackupMovementId = '637268422516652469'
 WHERE MovementId = '637268422518847992'

UPDATE [Offchain].[Movement]
   SET BackupMovementId = '637268483559866117'
 WHERE MovementId = '637268483559868380'

-- Expected Output : This View should return the list of backup movement details 
               -- BackupMovementId's
/*				                   1. 637268412220504405
				                   2. 637268422516652469
								   3. 637268483559866117 */


--Actual Output:
/*
MovementId	      BatchId	MovementTransactionId	OperationalDate	Operacion	              SourceNode	  DestinationNode	    SourceProduct	    DestinationProduct	NetStandardVolume	GrossStandardVolume	MeasurementUnit	EventType	SystemName	BackupMovementId	GlobalMovementId	ProductId		Category	Element	                 NodeName	         CalculationDate	RNo
637268412220504405	NULL	10880	                  2020-05-31	Pérdida no identificada	Automation_uhwma	NULL	            CRUDO CAMPO MAMBO	NULL	            528932.92	                NULL			Bbl			  Insert		TRUE		NULL				NULL			10000002318		Segmento	Automation_21gxw	-_-Automation_uhwma-_-	 2020-05-31			1
637268412220504405	NULL	10880	                  2020-05-31	Pérdida no identificada	Automation_uhwma	NULL	            CRUDO CAMPO MAMBO	NULL	            528932.92	                NULL			Bbl			  Insert		TRUE		NULL				NULL			10000002318		Segmento	Automation_21gxw	-_-Todos-_-	             2020-05-31			2
637268422516652469	NULL	10883	                  2020-05-31	Tolerancia	            Automation_0whu7	NULL	            CRUDO CAMPO MAMBO	NULL	            1942.63	                    NULL			Bbl			  Insert		TRUE		NULL				NULL			10000002318		Segmento	Automation_rro6x	-_-Todos-_-	             2020-05-31			3
637268422516652469	NULL	10883	                  2020-05-31	Tolerancia	            Automation_0whu7	NULL	            CRUDO CAMPO MAMBO	NULL	            1942.63	                    NULL			Bbl			  Insert		TRUE		NULL				NULL			10000002318		Segmento	Automation_rro6x	-_-Automation_0whu7-_-	 2020-05-31			4
637268483559866117	NULL	10918	                  2020-05-31	Tolerancia	            NULL	            Automation_021fe	NULL	            CRUDO CAMPO MAMBO	0.00	                    NULL			Bbl			  Insert		TRUE		NULL				NULL			10000002318		Segmento	Automation_dssu6	-_-Todos-_-	             2020-05-31			5
637268483559866117	NULL	10918	                  2020-05-31	Tolerancia	            NULL	            Automation_021fe	NULL	            CRUDO CAMPO MAMBO	0.00	                    NULL			Bbl			  Insert		TRUE		NULL				NULL			10000002318		Segmento	Automation_dssu6	-_-Automation_021fe-_-	 2020-05-31			6
*/