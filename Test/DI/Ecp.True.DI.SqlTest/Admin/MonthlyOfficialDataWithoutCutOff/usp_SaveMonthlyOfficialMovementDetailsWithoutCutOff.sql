/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jul-10-2020
-- Description:     This Procedure is to Get Official Monthly InventoryQuality details based on Element, Node, StartDate, EndDate.
--EXEC [Admin].[usp_SaveMonthlyOfficialInventoryQualityDetailsWithoutCutOff] 137236,30812,'2020-06-26','2020-06-28','bd5b494e-ac14-466b-b507-0f93fd22ed7f'
--SELECT * FROM ADMIN.OfficialMonthlyInventoryQualityDetails
-- ==============================================================================================================================*/

--===================================  DATA FOR [Admin].[OfficialMovementInformation] ====================================================================================================================================================================================================================
INSERT INTO [Admin].[OfficialMovementInformation]([OperationalDate],[SourceProductId],[SourceProductName],[SegmentID],[SourceNodeId]					
,[SourceNodeName],[SourceNodeNameIsGeneric],[DestinationProductId],[DestinationProductName]	,[DestinationNodeId],[DestinationNodeName],[DestinationNodeNameIsGeneric]	
 ,[MessageTypeId],[Classification],[MovementID],[MovementTypeName],[MeasurementUnit],[MovementTransactionId],[EventType],[SystemName],[SourceSystem] ,[NetStandardVolume]				
 ,[GrossStandardVolume]	,[UncertaintyPercentage],[BatchId],[Version],[OwnerName],[InputElement],[InputNode],[InputStartDate],[InputEndDate],[ExecutionId],[CreatedBy],[CreatedDate])
					VALUES('2020-07-10',10000002318,'CRUDO CAMPO MAMBO',NULL,30812,'Automation_lfxsi'
					,1,10000002372,'CRUDO CAMPO CUSUCO',30750,'Automation_06zr6',1,1,NULL,5,NULL,'Bbl'
					,11026,NULL,NULL,'true',NULL,NULL,NULL,NULL,'1',NULL,137236,30812,'2020-07-03','2020-07-06',
					'E9522F80-38C8-4995-9577-1D3567189D4A','ReportUser','2020-07-10')

INSERT INTO [Admin].[OfficialMovementInformation]([OperationalDate],[SourceProductId],[SourceProductName],[SegmentID],[SourceNodeId]					
,[SourceNodeName],[SourceNodeNameIsGeneric],[DestinationProductId],[DestinationProductName]	,[DestinationNodeId],[DestinationNodeName],[DestinationNodeNameIsGeneric]	
 ,[MessageTypeId],[Classification],[MovementID],[MovementTypeName],[MeasurementUnit],[MovementTransactionId],[EventType],[SystemName],[SourceSystem] ,[NetStandardVolume]				
 ,[GrossStandardVolume]	,[UncertaintyPercentage],[BatchId],[Version],[OwnerName],[InputElement],[InputNode],[InputStartDate],[InputEndDate],[ExecutionId],[CreatedBy],[CreatedDate])
					VALUES('2020-07-10',10000002318,'CRUDO CAMPO MAMBO',NULL,30812,'Automation_lfxsi'
					,1,10000002372,'CRUDO CAMPO CUSUCO',30750,'Automation_06zr6',1,1,NULL,5,NULL,'Bbl'
					,20789,NULL,NULL,'true',NULL,NULL,NULL,NULL,'1',NULL,137236,30812,'2020-07-03','2020-07-06',
					'E9522F80-38C8-4995-9577-1D3567189D4A','ReportUser','2020-07-10')

--===================================  DATA FOR Offchain.Owner ====================================================================================================================================================================================================================

	
 Insert into Offchain.Owner([OwnerId],[OwnershipValue],[OwnershipValueUnit],[InventoryProductId],[MovementTransactionId]					
,[BlockchainStatus],[BlockchainMovementTransactionId],[BlockchainInventoryProductTransactionId],[CreatedBy]								
,[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES(137196,123,1234,NULL,11026,1,NULL,NULL,'ReportUser','2020-07-10',NULL,NULL)

 Insert into Offchain.Owner([OwnerId],[OwnershipValue],[OwnershipValueUnit],[InventoryProductId],[MovementTransactionId]					
,[BlockchainStatus],[BlockchainMovementTransactionId],[BlockchainInventoryProductTransactionId],[CreatedBy]								
,[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES(137214,123,1234,NULL,11026,1,NULL,NULL,'ReportUser','2020-07-10',NULL,NULL)

 Insert into Offchain.Owner([OwnerId],[OwnershipValue],[OwnershipValueUnit],[InventoryProductId],[MovementTransactionId]					
,[BlockchainStatus],[BlockchainMovementTransactionId],[BlockchainInventoryProductTransactionId],[CreatedBy]								
,[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES(137232,123,1234,NULL,11026,1,NULL,NULL,'ReportUser','2020-07-10',NULL,NULL)

Insert into Offchain.Owner([OwnerId],[OwnershipValue],[OwnershipValueUnit],[InventoryProductId],[MovementTransactionId]					
,[BlockchainStatus],[BlockchainMovementTransactionId],[BlockchainInventoryProductTransactionId],[CreatedBy]								
,[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES(137232,123,1234,NULL,20789,1,NULL,NULL,'ReportUser','2020-07-10',NULL,NULL)

 Insert into Offchain.Owner([OwnerId],[OwnershipValue],[OwnershipValueUnit],[InventoryProductId],[MovementTransactionId]					
,[BlockchainStatus],[BlockchainMovementTransactionId],[BlockchainInventoryProductTransactionId],[CreatedBy]								
,[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES(137214,123,1234,NULL,20789,1,NULL,NULL,'ReportUser','2020-07-10',NULL,NULL)

--===================== TestCase1: To check if the MonthlyInventoryQuality returns data for Segments within a given date range ====================================

		EXEC [Admin].[usp_SaveMonthlyOfficialMovementDetailsWithoutCutOff]
		 @ElementId = 137236
		,@NodeId   =  30812 
        ,@StartDate  ='2020-07-03'                     
        ,@EndDate    ='2020-07-06'                     
        ,@ExecutionId = 'E9522F80-38C8-4995-9577-1D3567189D4A'
		SELECT * FROM [Admin].[OfficialMonthlyMovmentDetails] WHERE EXECUTIONID = 'E9522F80-38C8-4995-9577-1D3567189D4A'

--========================================= Output Captured ====================================================================================================					

--RNo	System	Version	MovementId	TypeMovement	Movement	SourceNode	DestinationNode	SourceProduct	DestinationProduct	NetQuantity	GrossQuantity	MeasurementUnit	Owner	Ownershipvolume	Ownershippercentage	Origin	RegistrationDate	MovementTransactionId	InputElementName	InputNodeName	InputStartDate	InputEndDate	ExecutionId	CreatedBy	CreatedDate	
--1	NULL	1	5	NULL	Salidas	Automation_lfxsi	Automation_06zr6	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	0.00	0.00	Bbl	Automation_7srjk	123.00	NULL	true	1900-01-01 00:00:00.000	11026	137236	30812	2020-07-03	2020-07-06	E9522F80-38C8-4995-9577-1D3567189D4A	ReportUser	2020-07-10 06:48:21.057
--2	NULL	1	5	NULL	Salidas	Automation_lfxsi	Automation_06zr6	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	0.00	0.00	Bbl	Automation_7srjk	123.00	NULL	true	1900-01-01 00:00:00.000	11026	137236	30812	2020-07-03	2020-07-06	E9522F80-38C8-4995-9577-1D3567189D4A	ReportUser	2020-07-10 06:48:21.057
--3	NULL	1	5	NULL	Salidas	Automation_lfxsi	Automation_06zr6	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	0.00	0.00	Bbl	Automation_c0pyi	123.00	NULL	true	1900-01-01 00:00:00.000	11026	137236	30812	2020-07-03	2020-07-06	E9522F80-38C8-4995-9577-1D3567189D4A	ReportUser	2020-07-10 06:48:21.057
--4	NULL	1	5	NULL	Salidas	Automation_lfxsi	Automation_06zr6	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	0.00	0.00	Bbl	Automation_c0pyi	123.00	NULL	true	1900-01-01 00:00:00.000	11026	137236	30812	2020-07-03	2020-07-06	E9522F80-38C8-4995-9577-1D3567189D4A	ReportUser	2020-07-10 06:48:21.057
--5	NULL	1	5	NULL	Salidas	Automation_lfxsi	Automation_06zr6	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	0.00	0.00	Bbl	Automation_l3ewz	123.00	NULL	true	1900-01-01 00:00:00.000	11026	137236	30812	2020-07-03	2020-07-06	E9522F80-38C8-4995-9577-1D3567189D4A	ReportUser	2020-07-10 06:48:21.057
--6	NULL	1	5	NULL	Salidas	Automation_lfxsi	Automation_06zr6	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	0.00	0.00	Bbl	Automation_l3ewz	123.00	NULL	true	1900-01-01 00:00:00.000	11026	137236	30812	2020-07-03	2020-07-06	E9522F80-38C8-4995-9577-1D3567189D4A	ReportUser	2020-07-10 06:48:21.057
--7	NULL	1	5	NULL	Salidas	Automation_lfxsi	Automation_06zr6	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	0.00	0.00	Bbl	Automation_7srjk	123.00	NULL	true	1900-01-01 00:00:00.000	20789	137236	30812	2020-07-03	2020-07-06	E9522F80-38C8-4995-9577-1D3567189D4A	ReportUser	2020-07-10 06:48:21.057
--8	NULL	1	5	NULL	Salidas	Automation_lfxsi	Automation_06zr6	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	0.00	0.00	Bbl	Automation_7srjk	123.00	NULL	true	1900-01-01 00:00:00.000	20789	137236	30812	2020-07-03	2020-07-06	E9522F80-38C8-4995-9577-1D3567189D4A	ReportUser	2020-07-10 06:48:21.057
--9	NULL	1	5	NULL	Salidas	Automation_lfxsi	Automation_06zr6	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	0.00	0.00	Bbl	Automation_c0pyi	NULL	100.00	true	1900-01-01 00:00:00.000	20789	137236	30812	2020-07-03	2020-07-06	E9522F80-38C8-4995-9577-1D3567189D4A	ReportUser	2020-07-10 06:48:21.057
--10	NULL	1	5	NULL	Salidas	Automation_lfxsi	Automation_06zr6	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	0.00	0.00	Bbl	Automation_c0pyi	NULL	100.00	true	1900-01-01 00:00:00.000	20789	137236	30812	2020-07-03	2020-07-06	E9522F80-38C8-4995-9577-1D3567189D4A	ReportUser	2020-07-10 06:48:21.057
--11	NULL	1	5	NULL	Salidas	Automation_lfxsi	Automation_06zr6	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	0.00	0.00	Bbl	Automation_l3ewz	123.00	NULL	true	1900-01-01 00:00:00.000	20789	137236	30812	2020-07-03	2020-07-06	E9522F80-38C8-4995-9577-1D3567189D4A	ReportUser	2020-07-10 06:48:21.057
--12	NULL	1	5	NULL	Salidas	Automation_lfxsi	Automation_06zr6	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	0.00	0.00	Bbl	Automation_l3ewz	123.00	NULL	true	1900-01-01 00:00:00.000	20789	137236	30812	2020-07-03	2020-07-06	E9522F80-38C8-4995-9577-1D3567189D4A	ReportUser	2020-07-10 06:48:21.057