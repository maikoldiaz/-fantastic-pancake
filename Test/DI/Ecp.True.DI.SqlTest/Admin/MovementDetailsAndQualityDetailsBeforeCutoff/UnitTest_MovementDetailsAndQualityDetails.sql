/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Jan-28-2020
-- Description:     These test cases are for View [Admin].[MovementDetailsBeforeCutoff]
-- Database backup Used:	appdb_dev_0128
-- ==============================================================================================================================*/
/*
The below insert statements are required to populate Segment and System related data in [Offchain].[Movement],[Offchain].[MovementSource], [Offchain].[MovementDestination] tables.
*/

--=================================== SEGMENT DATA FOR [Offchain].[Movement] =================================================================================================================================================================================================================================================
SET IDENTITY_INSERT [Offchain].[Movement] ON 

INSERT INTO [Offchain].[Movement] ([MovementTransactionID],[MessageTypeId],[SystemTypeId],[SourceSystem],[EventType],[MovementId],[MovementTypeId],[TicketId],[SegmentId],[OperationalDate],[GrossStandardVolume],[NetStandardVolume],[UncertaintyPercentage],[MeasurementUnit],[Scenario],[Observations],[Classification],[IsDeleted],[VariableTypeId],[FileRegistrationTransactionId],[OwnershipTicketId],[SystemName],[ReasonId],[Comment],[ContractId],[BlockchainStatus],[BlockchainMovementTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10001,1,2,'SINOPER','INSERT','1031141',10702,NULL,10689,'2030-01-06 23:59:59.000',200.0000000000000000,316871.8200000000000000,NULL,31,'Operativo','Reporte Operativo Cusiana -Fecha','cls',0,NULL,9641,NULL,'EXCEL - OCENSA',NULL,	NULL,	NULL,	NULL,	NULL,	'System', GETDATE(), NULL,NULL)

INSERT INTO [Offchain].[Movement] ([MovementTransactionID],[MessageTypeId],[SystemTypeId],[SourceSystem],[EventType],[MovementId],[MovementTypeId],[TicketId],[SegmentId],[OperationalDate],[GrossStandardVolume],[NetStandardVolume],[UncertaintyPercentage],[MeasurementUnit],[Scenario],[Observations],[Classification],[IsDeleted],[VariableTypeId],[FileRegistrationTransactionId],[OwnershipTicketId],[SystemName],[ReasonId],[Comment],[ContractId],[BlockchainStatus],[BlockchainMovementTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10002,1,2,'SINOPER','INSERT','1031142',10830,NULL,10817,'2030-01-06 23:59:59.000',200.0000000000000000,442558.5500000000000000,NULL,31,'Operativo','Reporte Operativo Cusiana -Fecha','cls',0,NULL,9849,NULL,'EXCEL - OCENSA',NULL,	NULL,	NULL,	NULL,	NULL,	'System', GETDATE(), NULL,NULL)

INSERT INTO [Offchain].[Movement] ([MovementTransactionID],[MessageTypeId],[SystemTypeId],[SourceSystem],[EventType],[MovementId],[MovementTypeId],[TicketId],[SegmentId],[OperationalDate],[GrossStandardVolume],[NetStandardVolume],[UncertaintyPercentage],[MeasurementUnit],[Scenario],[Observations],[Classification],[IsDeleted],[VariableTypeId],[FileRegistrationTransactionId],[OwnershipTicketId],[SystemName],[ReasonId],[Comment],[ContractId],[BlockchainStatus],[BlockchainMovementTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES (10003,2,2,'SINOPER','INSERT', '1031143', 42, NULL,	10,	'2030-01-03 00:00:00.000',	0.0000000000000000,	118020.4300000000000000, 0.2000000000000000, 31, 'Operativo', NULL,	'PerdidaIdentificada',	0,	NULL,	4855,	NULL,	NULL,	NULL,	NULL,	NULL,	NULL,	NULL,	'System', GETDATE(), NULL,NULL)

--=================================== SYSTEM DATA FOR [Offchain].[Movement] =================================================================================================================================================================================================================================================

INSERT INTO [Offchain].[Movement] ([MovementTransactionID],[MessageTypeId],[SystemTypeId],[SourceSystem],[EventType],[MovementId],[MovementTypeId],[TicketId],[SegmentId],[OperationalDate],[GrossStandardVolume],[NetStandardVolume],[UncertaintyPercentage],[MeasurementUnit],[Scenario],[Observations],[Classification],[IsDeleted],[VariableTypeId],[FileRegistrationTransactionId],[OwnershipTicketId],[SystemName],[ReasonId],[Comment],[ContractId],[BlockchainStatus],[BlockchainMovementTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES ( 10004,1,3,	'EXCEL',	'Insert','1031144',	9920,	24057,	9907,	'2030-01-02 23:59:59.000',	200.0000000000000000,	116000.2500000000000000,	0.2000000000000000,	31,	'Operativo',	'Reporte Operativo Cusiana -Fecha'	,'cls',	0,	NULL,	8950,	NULL,	'EXCEL - OCENSA',	NULL,	NULL,	NULL,	NULL,	NULL,	'System',	GETDATE(),	NULL,  	NULL)

INSERT INTO [Offchain].[Movement] ([MovementTransactionID],[MessageTypeId],[SystemTypeId],[SourceSystem],[EventType],[MovementId],[MovementTypeId],[TicketId],[SegmentId],[OperationalDate],[GrossStandardVolume],[NetStandardVolume],[UncertaintyPercentage],[MeasurementUnit],[Scenario],[Observations],[Classification],[IsDeleted],[VariableTypeId],[FileRegistrationTransactionId],[OwnershipTicketId],[SystemName],[ReasonId],[Comment],[ContractId],[BlockchainStatus],[BlockchainMovementTransactionId],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES( 10005,	2,	3,	'EXCEL'	,'Insert',	'1031145',	9920,	24057,	9907,'2030-01-02 23:59:59.000',	200.0000000000000000,	401475.7400000000000000,	0.2000000000000000,	31,	'Operativo',	'Reporte Operativo Cusiana -Fecha'	,'PerdidaIdentificada',	0,	NULL,	8951,	NULL,	'EXCEL - OCENSA',	NULL,	NULL,	NULL,	NULL,	NULL,	'System',	GETDATE(),	NULL,  	NULL)


SET IDENTITY_INSERT [Offchain].[Movement] OFF
--######################################################################################################################################################################################################################################################################################################################################


--===================================SEGMENT DATA FOR  [Offchain].[MovementSource] =================================================================================================================================================================================================================================================


INSERT INTO Offchain.MovementSource ( MovementTransactionId,SourceNodeId,SourceStorageLocationId,SourceProductId,SourceProductTypeId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate )
VALUES (10001,3411,NULL,10000002372,7252,	'System',GETDATE(),	NULL, NULL)


INSERT INTO Offchain.MovementSource ( MovementTransactionId,SourceNodeId,SourceStorageLocationId,SourceProductId,SourceProductTypeId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate )
VALUES (10002,3412,NULL,10000002318,7252,'System',GETDATE(),	NULL, NULL)


INSERT INTO Offchain.MovementSource ( MovementTransactionId,SourceNodeId,SourceStorageLocationId,SourceProductId,SourceProductTypeId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate )
VALUES (10003,3411,NULL,10000002372,7252,	'System',GETDATE(),	NULL, NULL)

--===================================SYSTEM DATA FOR  [Offchain].[MovementSource] =================================================================================================================================================================================================================================================


INSERT INTO Offchain.MovementSource ( MovementTransactionId,SourceNodeId,SourceStorageLocationId,SourceProductId,SourceProductTypeId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate )
VALUES (10004,3412,NULL,10000002318,7252,	'System',GETDATE(),	NULL, NULL)


INSERT INTO Offchain.MovementSource ( MovementTransactionId,SourceNodeId,SourceStorageLocationId,SourceProductId,SourceProductTypeId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate )
VALUES (10005,3413,NULL,10000002318,7252,	'System',GETDATE(),	NULL, NULL)

--===================================SEGMENT DATA FOR  [Offchain].[MovementDestination] =================================================================================================================================================================================================================================================


INSERT INTO Offchain.MovementDestination ( MovementTransactionId,DestinationNodeID,DestinationStorageLocationId,DestinationProductId,DestinationProductTypeId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate )
VALUES (10001,3412,NULL,10000002372,7260,	'System',GETDATE(),	NULL, NULL)


INSERT INTO Offchain.MovementDestination ( MovementTransactionId,DestinationNodeID,DestinationStorageLocationId,DestinationProductId,DestinationProductTypeId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate )
VALUES (10002,3411,NULL,10000002318,7260,'System',GETDATE(),	NULL, NULL)


INSERT INTO Offchain.MovementDestination ( MovementTransactionId,DestinationNodeID,DestinationStorageLocationId,DestinationProductId,DestinationProductTypeId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate )
VALUES (10003,3412,NULL,10000002372,7260,	'System',GETDATE(),	NULL, NULL)

--===================================SYSTEM DATA FOR  [Offchain].[MovementDestination] =================================================================================================================================================================================================================================================


INSERT INTO Offchain.MovementDestination ( MovementTransactionId,DestinationNodeID,DestinationStorageLocationId,DestinationProductId,DestinationProductTypeId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate )
VALUES (10004,3413,NULL,10000002318,7260,	'System',GETDATE(),	NULL, NULL)


INSERT INTO Offchain.MovementDestination ( MovementTransactionId,DestinationNodeID,DestinationStorageLocationId,DestinationProductId,DestinationProductTypeId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate )
VALUES (10005,3411,NULL,10000002318,7260,	'System',GETDATE(),	NULL, NULL)


--=================================== SEGMENT DATA FOR [Admin].[Attribute] =================================================================================================================================================================================================================================================
INSERT INTO [Admin].[Attribute] (AttributeId,AttributeValue,ValueAttributeUnit,AttributeDescription,InventoryProductId,MovementTransactionId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES ('API','22.4','API',NULL,NULL,10001,'System',GETDATE(),NULL,NULL)


INSERT INTO [Admin].[Attribute] (AttributeId,AttributeValue,ValueAttributeUnit,AttributeDescription,InventoryProductId,MovementTransactionId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES ('Contenido Azufre','2.31','% peso',NULL,NULL,10002,'System',GETDATE(),NULL,NULL)


INSERT INTO [Admin].[Attribute] (AttributeId,AttributeValue,ValueAttributeUnit,AttributeDescription,InventoryProductId,MovementTransactionId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES ('Mayorista','ECOPETROL S.A.','ADM',NULL,NULL,10003,'System',GETDATE(),NULL,NULL)

--=================================== SYSTEM DATA FOR [Admin].[Attribute] =================================================================================================================================================================================================================================================

INSERT INTO [Admin].[Attribute] (AttributeId,AttributeValue,ValueAttributeUnit,AttributeDescription,InventoryProductId,MovementTransactionId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES ('BSW','0.832.','% volumen',NULL,NULL,10004,'System',GETDATE(),NULL,NULL)


INSERT INTO [Admin].[Attribute] (AttributeId,AttributeValue,ValueAttributeUnit,AttributeDescription,InventoryProductId,MovementTransactionId,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
VALUES ('Neto-Volumen Líquido','Bls.','% volumen',NULL,NULL,10005,'System',GETDATE(),NULL,NULL)

--===================== TestCase1: To check if the Movement Details View returns data for Segments within a given date range ====================================
SELECT * FROM [Admin].[MovementDetailsBeforeCutoff]
WHERE Category = 'Segmento'
AND CalculationDate BETWEEN '2030-01-01' AND '2030-01-10'


--========================================= Output captured ====================================================================================================

/*
MovementId	Category	Element				CalculationDate	NodeName							Operacion				SourceNode			DestinationNode		SourceProduct		DestinationProduct	NetStandardVolume	GrossStandardVolume	MeasurementUnit	Movement				PercentStandardUnCertainty	Uncertainty	SourceProductId	RNo
1031143		Segmento	Transporte			2030-01-03		Automation_5u3ukAutomation_d3mj5	Traslado de productos	Automation_5u3uk	Automation_d3mj5	CRUDO CAMPO CUSUCO	CRUDO CAMPO CUSUCO	118020.43			0.00				Bbl				Pérdidas Identificadas	0.20						23604.09	10000002372		402
1031141		Segmento	Automation_4et1g	2030-01-06		Automation_5u3ukAutomation_d3mj5	Automation_m8e07	Automation_5u3uk	Automation_d3mj5		CRUDO CAMPO CUSUCO	CRUDO CAMPO CUSUCO	316871.82			200.00				Bbl				Movimiento				NULL						316871.82	10000002372		403
1031142		Segmento	Automation_3zl03	2030-01-06		Automation_d3mj5Automation_5u3uk	Automation_h2j83	Automation_d3mj5	Automation_5u3uk		CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	442558.55			200.00				Bbl				Movimiento				NULL						442558.55	10000002318		886
1031144		Segmento	Automation_3ck5s	2030-01-02		Automation_d3mj5Automation_i6ide	Automation_d5f6r	Automation_d3mj5	Automation_i6ide		CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	116000.25			200.00				Bbl				Movimiento				0.20						23200.05	10000002318		888
1031145		Segmento	Automation_3ck5s	2030-01-02		Automation_i6ideAutomation_5u3uk	Automation_d5f6r	Automation_i6ide	Automation_5u3uk		CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	401475.74			200.00				Bbl				Pérdidas Identificadas	0.20						80295.15	10000002318		1211
*/

--===================== TestCase2: To check if the Movement Details View returns data for Systems within a given date range ====================================

SELECT * FROM [Admin].[MovementDetailsBeforeCutoff]
WHERE Category = 'Sistema' 
AND CalculationDate BETWEEN '2030-10-04' AND '2030-10-06'

--========================================= Output captured ====================================================================================================
SELECT * FROM [Admin].[MovementDetailsBeforeCutoff]
WHERE Category = 'Sistema' 
AND CalculationDate BETWEEN '2030-01-01' AND '2030-01-10'

--========================================= Output captured ====================================================================================================
/*
MovementId	CalculationDate	Category	Element			Operacion			SourceNode			DestinationNode		SourceProduct		DestinationProduct	NodeName							NetStandardVolume	GrossStandardVolume	MeasurementUnit	Movement				PercentStandardUnCertainty	Uncertainty	SourceProductId	RNo
1031144		2030-01-02		Sistema		SystemElement2	Automation_d5f6r	Automation_d3mj5	Automation_i6ide	CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	Automation_d3mj5Automation_i6ide	116000.25			200.00				Bbl				NULL					0.20						23200.05	10000002318	887
1031145		2030-01-02		Sistema		SystemElement2	Automation_d5f6r	Automation_i6ide	Automation_5u3uk	CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	Automation_i6ideAutomation_5u3uk	401475.74			200.00				Bbl				Pérdidas Identificadas	0.20						80295.15	10000002318	1212

*/

--===================== TestCase3: To check if the Movement Quality Details View returns data for Segments within a given date range ====================================


SELECT * FROM [Admin].[MovementQualityDetailsBeforeCutoff]
WHERE Category = 'Segmento'
AND CalculationDate BETWEEN '2030-01-01' AND '2030-01-10'

--========================================= Output captured ====================================================================================================
/*
MovementId	CalculationDate	Category	Element				Operacion				SourceNode			DestinationNode		SourceProduct		DestinationProduct	NetStandardVolume	GrossStandardVolume	MeasurementUnit	Movement				AttributeValue	ValueAttributeUnit	AttributeDescription	PercentStandardUnCertainty		Uncertainty		NodeName							SourceProductId	RNo
1031143		2030-01-03		Segmento	Transporte			Traslado de productos	Automation_5u3uk	Automation_d3mj5	CRUDO CAMPO CUSUCO	CRUDO CAMPO CUSUCO	118020.43			0.00				Bbl				Pérdidas Identificadas	ECOPETROL S.A.	ADM					NULL					0.20							23604.09		Automation_5u3ukAutomation_d3mj5	10000002372		199
1031141		2030-01-06		Segmento	Automation_4et1g	Automation_m8e07		Automation_5u3uk	Automation_d3mj5	CRUDO CAMPO CUSUCO	CRUDO CAMPO CUSUCO	316871.82			200.00				Bbl				Movimiento				22.4			API					NULL					NULL							316871.82		Automation_5u3ukAutomation_d3mj5	10000002372		200
1031142		2030-01-06		Segmento	Automation_3zl03	Automation_h2j83		Automation_d3mj5	Automation_5u3uk	CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	442558.55			200.00				Bbl				Movimiento				2.31			% peso				NULL					NULL							442558.55		Automation_d3mj5Automation_5u3uk	10000002318		377
1031144		2030-01-02		Segmento	Automation_3ck5s	Automation_d5f6r		Automation_d3mj5	Automation_i6ide	CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	116000.25			200.00				Bbl				Movimiento				0.832.			% volumen			NULL					0.20							23200.05		Automation_d3mj5Automation_i6ide	10000002318		379
1031145		2030-01-02		Segmento	Automation_3ck5s	Automation_d5f6r		Automation_i6ide	Automation_5u3uk	CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	401475.74			200.00				Bbl				Pérdidas Identificadas	Bls.			% volumen			NULL					0.20							80295.15		Automation_i6ideAutomation_5u3uk	10000002318		506
*/


--===================== TestCase4: To check if the Movement Quality Details View returns data for Systems within a given date range ====================================
SELECT * FROM [Admin].[MovementQualityDetailsBeforeCutoff]
WHERE Category = 'Sistema' 
AND CalculationDate BETWEEN '2030-01-01' AND '2030-01-10'

--========================================= Output captured ====================================================================================================
/*
MovementId	CalculationDate	Category	Element			Operacion			SourceNode			DestinationNode		SourceProduct		DestinationProduct	NetStandardVolume	GrossStandardVolume	MeasurementUnit	SourceProductId	Movement				PercentStandardUnCertainty	Uncertainty	AttributeValue	ValueAttributeUnit	AttributeDescription	NodeName							RNo
1031144		2030-01-02		Sistema		SystemElement2	Automation_d5f6r	Automation_d3mj5	Automation_i6ide	CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	116000.25			200.00				Bbl				10000002318		NULL					0.20						23200.05	0.832.			% volumen			NULL					Automation_d3mj5Automation_i6ide	378
1031145		2030-01-02		Sistema		SystemElement2	Automation_d5f6r	Automation_i6ide	Automation_5u3uk	CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	401475.74			200.00				Bbl				10000002318		Pérdidas Identificadas	0.20						80295.15	Bls.			% volumen			NULL					Automation_i6ideAutomation_5u3uk	507
*/