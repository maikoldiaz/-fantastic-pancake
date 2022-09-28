/******************************************************************************
-- Type = After this stores procedure execute data should e loaded into 
          [Analytics].[OperativeMovementsWithOwnership and [Analytics].[OwnershipPercentageValues]
********************************************************************************/

-- INSERT MOVEMENTS
INSERT INTO [Analytics].[OperativeNodeRelationshipWithOwnership] (SourceProduct,LogisticSourceCenter,DestinationProduct,LogisticDestinationCenter
                                                                 ,TransferPoint,IsDeleted,SourceSystem,CreatedBy)
                                                          VALUES ('CRUDO CAMPO CUSUCO','PR TISQUIRAMA ASO:PR TISQUIRAMA ASO : MATERIA PRIMA'
														         ,'CRUDO CAMPO CUSUCO','PR TISQUIRAMA ASO:PR TISQUIRAMA ASO : MATERIA PRIMA'
																 ,'CustomTransferPoint',0,'CSV','Demo')

INSERT INTO [Analytics].[OperativeNodeRelationshipWithOwnership] (SourceProduct,LogisticSourceCenter,DestinationProduct,LogisticDestinationCenter
                                                                 ,TransferPoint,IsDeleted,SourceSystem,CreatedBy)
                                                          VALUES ('CRUDO CAMPO CUSUCO','PR TISQUIRAMA ASO:PR TISQUIRAMA ASO : MATERIA PRIMA'
														         ,'CRUDO CAMPO MAMBO','PR TISQUIRAMA ASO:PR TISQUIRAMA ASO : MATERIA PRIMA'
																 ,'CustomTransferPoint1',0,'CSV','Demo')

INSERT INTO [Analytics].[OperativeNodeRelationshipWithOwnership] (SourceProduct,LogisticSourceCenter,DestinationProduct,LogisticDestinationCenter
                                                                 ,TransferPoint,IsDeleted,SourceSystem,CreatedBy)
                                                          VALUES ('CRUDO CAMPO MAMBO','PR TISQUIRAMA ASO:PR TISQUIRAMA ASO : MATERIA PRIMA'
														         ,'CRUDO CAMPO CUSUCO','PR TISQUIRAMA ASO:PR TISQUIRAMA ASO : MATERIA PRIMA'
																 ,'CustomTransferPoint1',0,'CSV','Demo')

GO
CREATE  TABLE #TempIdValues (
                            OwnershipNodeId INT
	                       ,TicketId INT
                           ,MovementTransactionId INT
	                       ,OwnershipId INT
	                       ,MovementSourceId INT
	                       ,MovementDestinationId INT
						   ,OperationalDate DATE
	                       )
GO
TRUNCATE TABLE #TempIdValues

DECLARE @OwnershipNodeId INT
       ,@TicketId INT
       ,@MovementTransactionId INT
	   ,@OwnershipId INT
	   ,@MovementSourceId INT
	   ,@MovementDestinationId INT
	   ,@OperationalDate DATE

	   SET @OperationalDate = '2020-05-15'


INSERT INTO [Admin].[Ticket] ([CategoryElementId],[StartDate],[EndDate],[Status],CreatedBy,CreatedDate) VALUES
                             (10,GETDATE(),GETDATE()+10,1,'Demo',GETDATE())
SELECT @TicketId = SCOPE_IDENTITY()
--SELECT @TicketId

INSERT INTO [Admin].[OwnershipNode] (TicketId,NodeId,[Status],OwnershipStatusId,CreatedBy,CreatedDate)
                             VALUES (@TicketId,11780,0,9,'Demo',GETDATE()) -- 11780 OwnershipNodeId
SELECT @OwnershipNodeId = SCOPE_IDENTITY()
--SELECT @OwnershipNodeId

/********************************** Destinatination Node Matching with Ownership Node ******************************************/
-- FIRST RECORD
INSERT INTO [Offchain].[Movement] (MessageTypeId,SystemTypeId,SourceSystem,EventType,MovementId,MovementTypeId,TicketId,SegmentId,OperationalDate
                                  ,GrossStandardVolume,NetStandardVolume,UncertaintyPercentage,MeasurementUnit,Observations,Classification
								  ,IsDeleted,FileRegistrationTransactionId,OwnershipTicketId,SystemName,BlockchainStatus,BlockchainMovementTransactionId
								  ,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate)
	                      VALUES (1,2,'SINOPER','Insert','937251470671658340',53404,@TicketId,'10',@OperationalDate,'200.00','2345.82','0.22'
							      ,31,'Reporte Operativo Cusiana -Fecha','cls',0,6313,@TicketId,'EXCEL - OCENSA',1
								  ,'6B20704E-75FA-4AB9-80F1-433EC58A4E51','0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24'
								  ,'0x15e650',0,'Demo','2020-04-30 06:34:02.317'
								  )
SELECT @MovementTransactionId = SCOPE_IDENTITY()
--SELECT @MovementTransactionId


INSERT INTO [Offchain].[MovementSource] (MovementTransactionId,SourceNodeId,SourceProductId,SourceProductTypeId,CreatedBy,CreatedDate)
                                 VALUES (@MovementTransactionId,11777,10000002372,53504,'Demo',GETDATE())
SELECT @MovementSourceId = SCOPE_IDENTITY()
--SELECT @MovementSourceId

INSERT INTO [Offchain].[MovementDestination] (MovementTransactionId,DestinationNodeId,DestinationProductId,DestinationProductTypeId,CreatedBy,CreatedDate)
                                 VALUES (@MovementTransactionId,11780,10000002318,53505,'Demo',GETDATE())

SELECT @MovementDestinationId = SCOPE_IDENTITY()
--SELECT @MovementDestinationId

INSERT INTO [Offchain].[Ownership] (MessageTypeId,TicketId,	MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume
                                   ,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,	BlockchainInventoryProductTransactionId,IsDeleted,EventType
								   ,BlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate)
                            VALUES (7,@TicketId,@MovementTransactionId,3415,30,'70.00','2600.00',1,1,GETDATE(),1,'343A23D9-596B-4E29-B336-9340708F0EA7'
							       ,0,'Insert','30AC82E0-CA5C-47E5-B5A2-305F8DEBA820','0x46ba288d20ac33a17e6b4f61fdbcc01f5449a7ecfcc09a1c5571c3f3a6018ec7','0x15e746'
								   ,0,'Demo',GETDATE())
SELECT @OwnershipId = SCOPE_IDENTITY()
--SELECT @OwnershipId



INSERT INTO #TempIdValues VALUES (
                                   @OwnershipNodeId
	                              ,@TicketId
                                  ,@MovementTransactionId
	                              ,@OwnershipId
	                              ,@MovementSourceId
	                              ,@MovementDestinationId
								  ,@OperationalDate
	                              )
								  
/********************************** Source Node Matching with Ownership Node ******************************************/
-- Second Record  
INSERT INTO [Offchain].[Movement] (MessageTypeId,SystemTypeId,SourceSystem,EventType,MovementId,MovementTypeId,TicketId,SegmentId,OperationalDate
                                  ,GrossStandardVolume,NetStandardVolume,UncertaintyPercentage,MeasurementUnit,Observations,Classification
								  ,IsDeleted,FileRegistrationTransactionId,OwnershipTicketId,SystemName,BlockchainStatus,BlockchainMovementTransactionId
								  ,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate)
	                      VALUES (1,3,'EXCEL','Insert','937251470671658341',53404,@TicketId,10,@OperationalDate,'200.00','6888.82','0.22'
							      ,31,'Reporte Operativo Cusiana -Fecha','cls',0,6313,@TicketId,'EXCEL - OCENSA',1
								  ,'6B20704E-75FA-4AB9-80F1-433EC58A4E51','0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24'
								  ,'0x15e650',0,'Demo','2020-04-30 06:34:02.317'
								  )
SELECT @MovementTransactionId = SCOPE_IDENTITY()
--SELECT @MovementTransactionId


INSERT INTO [Offchain].[MovementSource] (MovementTransactionId,SourceNodeId,SourceProductId,SourceProductTypeId,CreatedBy,CreatedDate)
                                 VALUES (@MovementTransactionId,11780,10000002318,53504,'Demo',GETDATE())
SELECT @MovementSourceId = SCOPE_IDENTITY()
--SELECT @MovementSourceId

INSERT INTO [Offchain].[MovementDestination] (MovementTransactionId,DestinationNodeId,DestinationProductId,DestinationProductTypeId,CreatedBy,CreatedDate)
                                 VALUES (@MovementTransactionId,11777,10000002372,53504,'Demo',GETDATE())

SELECT @MovementDestinationId = SCOPE_IDENTITY()
--SELECT @MovementDestinationId

INSERT INTO [Offchain].[Ownership] (MessageTypeId,TicketId,	MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume
                                   ,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,	BlockchainInventoryProductTransactionId,IsDeleted,EventType
								   ,BlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate)
                            VALUES (7,@TicketId,@MovementTransactionId,3415,30,'50.00','2800.00',1,1,GETDATE(),1,'343A23D9-596B-4E29-B336-9340708F0EA7'
							       ,0,'Insert','30AC82E0-CA5C-47E5-B5A2-305F8DEBA820','0x46ba288d20ac33a17e6b4f61fdbcc01f5449a7ecfcc09a1c5571c3f3a6018ec7','0x15e746'
								   ,0,'Demo',GETDATE())
SELECT @OwnershipId = SCOPE_IDENTITY()
--SELECT @OwnershipId

INSERT INTO #TempIdValues VALUES (
                                   @OwnershipNodeId
	                              ,@TicketId
                                  ,@MovementTransactionId
	                              ,@OwnershipId
	                              ,@MovementSourceId
	                              ,@MovementDestinationId
								  ,@OperationalDate
	                              )

/********************************** Source & Destination Node Matching with Ownership Node ******************************************/
-- Third Record 
INSERT INTO [Offchain].[Movement] (MessageTypeId,SystemTypeId,SourceSystem,EventType,MovementId,MovementTypeId,TicketId,SegmentId,OperationalDate
                                  ,GrossStandardVolume,NetStandardVolume,UncertaintyPercentage,MeasurementUnit,Observations,Classification
								  ,IsDeleted,FileRegistrationTransactionId,OwnershipTicketId,SystemName,BlockchainStatus,BlockchainMovementTransactionId
								  ,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate)
	                      VALUES (1,3,'EXCEL','Insert','937251470671658342',53404,@TicketId,10,@OperationalDate,'200.00','54732.82','0.22'
							      ,31,'Reporte Operativo Cusiana -Fecha','cls',0,6313,@TicketId,'EXCEL - OCENSA',1
								  ,'6B20704E-75FA-4AB9-80F1-433EC58A4E51','0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24'
								  ,'0x15e650',0,'Demo','2020-04-30 06:34:02.317'
								  )
SELECT @MovementTransactionId = SCOPE_IDENTITY()
--SELECT @MovementTransactionId


INSERT INTO [Offchain].[MovementSource] (MovementTransactionId,SourceNodeId,SourceProductId,SourceProductTypeId,CreatedBy,CreatedDate)
                                 VALUES (@MovementTransactionId,11780,10000002318,53504,'Demo',GETDATE())
SELECT @MovementSourceId = SCOPE_IDENTITY()
--SELECT @MovementSourceId

INSERT INTO [Offchain].[MovementDestination] (MovementTransactionId,DestinationNodeId,DestinationProductId,DestinationProductTypeId,CreatedBy,CreatedDate)
                                 VALUES (@MovementTransactionId,11780,10000002372,53504,'Demo',GETDATE())

SELECT @MovementDestinationId = SCOPE_IDENTITY()
--SELECT @MovementDestinationId

INSERT INTO [Offchain].[Ownership] (MessageTypeId,TicketId,	MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume
                                   ,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,	BlockchainInventoryProductTransactionId,IsDeleted,EventType
								   ,BlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate)
                            VALUES (7,@TicketId,@MovementTransactionId,3415,30,'50.00','6723.00',1,1,GETDATE(),1,'343A23D9-596B-4E29-B336-9340708F0EA7'
							       ,0,'Insert','30AC82E0-CA5C-47E5-B5A2-305F8DEBA820','0x46ba288d20ac33a17e6b4f61fdbcc01f5449a7ecfcc09a1c5571c3f3a6018ec7','0x15e746'
								   ,0,'Demo',GETDATE())
SELECT @OwnershipId = SCOPE_IDENTITY()
--SELECT @OwnershipId

INSERT INTO #TempIdValues VALUES (
                                   @OwnershipNodeId
	                              ,@TicketId
                                  ,@MovementTransactionId
	                              ,@OwnershipId
	                              ,@MovementSourceId
	                              ,@MovementDestinationId
								  ,@OperationalDate
	                              )

/********************************** Source & Destination Node NOT Matching with Ownership Node ******************************************/
-- Fourth Record  
INSERT INTO [Offchain].[Movement] (MessageTypeId,SystemTypeId,SourceSystem,EventType,MovementId,MovementTypeId,TicketId,SegmentId,OperationalDate
                                  ,GrossStandardVolume,NetStandardVolume,UncertaintyPercentage,MeasurementUnit,Observations,Classification
								  ,IsDeleted,FileRegistrationTransactionId,OwnershipTicketId,SystemName,BlockchainStatus,BlockchainMovementTransactionId
								  ,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate)
	                      VALUES (1,3,'EXCEL','Insert','937251470671658343',53404,@TicketId,10,@OperationalDate,'200.00','98765.82','0.22'
							      ,31,'Reporte Operativo Cusiana -Fecha','cls',0,6313,@TicketId,'EXCEL - OCENSA',1
								  ,'6B20704E-75FA-4AB9-80F1-433EC58A4E51','0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24'
								  ,'0x15e650',0,'Demo','2020-04-30 06:34:02.317'
								  )
SELECT @MovementTransactionId = SCOPE_IDENTITY()
--SELECT @MovementTransactionId


INSERT INTO [Offchain].[MovementSource] (MovementTransactionId,SourceNodeId,SourceProductId,SourceProductTypeId,CreatedBy,CreatedDate)
                                 VALUES (@MovementTransactionId,11781,10000002318,53504,'Demo',GETDATE())
SELECT @MovementSourceId = SCOPE_IDENTITY()
--SELECT @MovementSourceId

INSERT INTO [Offchain].[MovementDestination] (MovementTransactionId,DestinationNodeId,DestinationProductId,DestinationProductTypeId,CreatedBy,CreatedDate)
                                 VALUES (@MovementTransactionId,11777,10000002372,53504,'Demo',GETDATE())

SELECT @MovementDestinationId = SCOPE_IDENTITY()
--SELECT @MovementDestinationId

INSERT INTO [Offchain].[Ownership] (MessageTypeId,TicketId,	MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume
                                   ,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,	BlockchainInventoryProductTransactionId,IsDeleted,EventType
								   ,BlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate)
                            VALUES (7,@TicketId,@MovementTransactionId,3415,30,'50.00','98765.00',1,1,GETDATE(),1,'343A23D9-596B-4E29-B336-9340708F0EA7'
							       ,0,'Insert','30AC82E0-CA5C-47E5-B5A2-305F8DEBA820','0x46ba288d20ac33a17e6b4f61fdbcc01f5449a7ecfcc09a1c5571c3f3a6018ec7','0x15e746'
								   ,0,'Demo',GETDATE())
SELECT @OwnershipId = SCOPE_IDENTITY()
--SELECT @OwnershipId

INSERT INTO #TempIdValues VALUES (
                                   @OwnershipNodeId
	                              ,@TicketId
                                  ,@MovementTransactionId
	                              ,@OwnershipId
	                              ,@MovementSourceId
	                              ,@MovementDestinationId
								  ,@OperationalDate
	                              )

/********************************** INVALID MOVEMENT ******************************************/
-- Fifth Record
INSERT INTO [Offchain].[Movement] (MessageTypeId,SystemTypeId,SourceSystem,EventType,MovementId,MovementTypeId,TicketId,SegmentId,OperationalDate
                                  ,GrossStandardVolume,NetStandardVolume,UncertaintyPercentage,MeasurementUnit,Observations,Classification
								  ,IsDeleted,FileRegistrationTransactionId,OwnershipTicketId,SystemName,BlockchainStatus,BlockchainMovementTransactionId
								  ,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate)
	                      VALUES (1,3,'EXCEL','Insert','937251470671658344',53404,@TicketId,10,@OperationalDate,'200.00','108765.82','0.22'
							      ,31,'Reporte Operativo Cusiana -Fecha','cls',0,6313,@TicketId,'EXCEL - OCENSA',1
								  ,'6B20704E-75FA-4AB9-80F1-433EC58A4E51','0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24'
								  ,'0x15e650',0,'Demo','2020-04-30 06:34:02.317'
								  )
SELECT @MovementTransactionId = SCOPE_IDENTITY()
--SELECT @MovementTransactionId


INSERT INTO [Offchain].[MovementSource] (MovementTransactionId,SourceNodeId,SourceProductId,SourceProductTypeId,CreatedBy,CreatedDate)
                                 VALUES (@MovementTransactionId,11777,10000002318,53504,'Demo',GETDATE())
SELECT @MovementSourceId = SCOPE_IDENTITY()
--SELECT @MovementSourceId

INSERT INTO [Offchain].[MovementDestination] (MovementTransactionId,DestinationNodeId,DestinationProductId,DestinationProductTypeId,CreatedBy,CreatedDate)
                                 VALUES (@MovementTransactionId,11780,10000002318,53505,'Demo',GETDATE())

SELECT @MovementDestinationId = SCOPE_IDENTITY()
--SELECT @MovementDestinationId

INSERT INTO [Offchain].[Ownership] (MessageTypeId,TicketId,	MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume
                                   ,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,	BlockchainInventoryProductTransactionId,IsDeleted,EventType
								   ,BlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate)
                            VALUES (7,@TicketId,@MovementTransactionId,3415,30,'70.00','108657.00',1,1,GETDATE(),1,'343A23D9-596B-4E29-B336-9340708F0EA7'
							       ,0,'Insert','30AC82E0-CA5C-47E5-B5A2-305F8DEBA820','0x46ba288d20ac33a17e6b4f61fdbcc01f5449a7ecfcc09a1c5571c3f3a6018ec7','0x15e746'
								   ,0,'Demo',GETDATE())
SELECT @OwnershipId = SCOPE_IDENTITY()

INSERT INTO #TempIdValues VALUES (
                                   @OwnershipNodeId
	                              ,@TicketId
                                  ,@MovementTransactionId
	                              ,@OwnershipId
	                              ,@MovementSourceId
	                              ,@MovementDestinationId
								  ,@OperationalDate
	                              )

SELECT * FROM #TempIdValues


/************************************************************************************************
*************************************************************************************************
***********************************EXECUTE STORED PROEDURE***************************************
*************************************************************************************************
************************************************************************************************/

DECLARE @OwnershipNodeId INT, @Date DATE = '2020-05-15'
    SET @OwnershipNodeId = 3270
	
EXEC [Admin].[usp_SaveOperativeMovementsWithOwnershipPercentage] @OwnershipNodeId

SELECT * FROM [Analytics].[OperativeMovementsWithOwnership] WHERE OperationalDate = @Date
SELECT * FROM [Analytics].[OwnershipPercentageValues] WHERE OperationalDate = @Date
SELECT * FROM [Admin].[OwnerShipNode] WHERE OwnershipNodeId = @OwnershipNodeId

/* EXPECTED OUTPUT:  IT WILL INSERT BELOW MOVEMENTID RECORDS

937251470671658340
937251470671658341
937251470671658342

*/

/************************************************************************************************
*************************************************************************************************
***************************************UPSERT CASE***********************************************
*************************************************************************************************
************************************************************************************************/


/**** UPDATE EXISTING RECORDS ******/
UPDATE [Offchain].[Movement] SET NetStandardVolume = 7562.99 WHERE MovementTransactionId = 10864
UPDATE [Offchain].[Ownership] SET OwnershipVolume = 5400 WHERE MovementTransactionId = 10864

/******** NEW RECORDS *********/
DECLARE @OwnershipNodeId_1 INT = 3270
	   ,@TicketId_1 INT = 23702
       ,@MovementTransactionId_1 INT
	   ,@OwnershipId_1 INT
	   ,@MovementSourceId_1 INT
	   ,@MovementDestinationId_1 INT
	   ,@OperationalDate_1 DATE 

	   SET @OperationalDate_1 = '2020-05-15'

INSERT INTO [Offchain].[Movement] (MessageTypeId,SystemTypeId,SourceSystem,EventType,MovementId,MovementTypeId,TicketId,SegmentId,OperationalDate
                                  ,GrossStandardVolume,NetStandardVolume,UncertaintyPercentage,MeasurementUnit,Observations,Classification
								  ,IsDeleted,FileRegistrationTransactionId,OwnershipTicketId,SystemName,BlockchainStatus,BlockchainMovementTransactionId
								  ,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate)
	                      VALUES (1,3,'EXCEL','Insert','937251470671658345',53404,@TicketId_1,10,@OperationalDate_1,'200.00','108765.82','0.22'
							      ,31,'Reporte Operativo Cusiana -Fecha','cls',0,6313,@TicketId_1,'EXCEL - OCENSA',1
								  ,'6B20704E-75FA-4AB9-80F1-433EC58A4E51','0xb87b9522598e4ebffb34df496c3b9332904afcff0b33aa16106b8444283a0f24'
								  ,'0x15e650',0,'Demo','2020-04-30 06:34:02.317'
								  )
SELECT @MovementTransactionId_1 = SCOPE_IDENTITY()
--SELECT @MovementTransactionId


INSERT INTO [Offchain].[MovementSource] (MovementTransactionId,SourceNodeId,SourceProductId,SourceProductTypeId,CreatedBy,CreatedDate)
                                 VALUES (@MovementTransactionId_1,11777,10000002372,53504,'Demo',GETDATE())
SELECT @MovementSourceId_1 = SCOPE_IDENTITY()
--SELECT @MovementSourceId

INSERT INTO [Offchain].[MovementDestination] (MovementTransactionId,DestinationNodeId,DestinationProductId,DestinationProductTypeId,CreatedBy,CreatedDate)
                                 VALUES (@MovementTransactionId_1,11780,10000002318,53505,'Demo',GETDATE())

SELECT @MovementDestinationId_1 = SCOPE_IDENTITY()
--SELECT @MovementDestinationId

INSERT INTO [Offchain].[Ownership] (MessageTypeId,TicketId,	MovementTransactionId,InventoryProductId,OwnerId,OwnershipPercentage,OwnershipVolume
                                   ,AppliedRule,RuleVersion,ExecutionDate,BlockchainStatus,	BlockchainInventoryProductTransactionId,IsDeleted,EventType
								   ,BlockchainOwnershipId,TransactionHash,BlockNumber,RetryCount,CreatedBy,CreatedDate)
                            VALUES (7,@TicketId_1,@MovementTransactionId_1,3415,30,'70.00','108657.00',1,1,GETDATE(),1,'343A23D9-596B-4E29-B336-9340708F0EA7'
							       ,0,'Insert','30AC82E0-CA5C-47E5-B5A2-305F8DEBA820','0x46ba288d20ac33a17e6b4f61fdbcc01f5449a7ecfcc09a1c5571c3f3a6018ec7','0x15e746'
								   ,0,'Demo',GETDATE())
SELECT @OwnershipId_1 = SCOPE_IDENTITY()

INSERT INTO #TempIdValues VALUES (
                                   @OwnershipNodeId_1
	                              ,@TicketId_1
                                  ,@MovementTransactionId_1
	                              ,@OwnershipId_1
	                              ,@MovementSourceId_1
	                              ,@MovementDestinationId_1
								  ,@OperationalDate_1
	                              )

SELECT * FROM #TempIdValues



/************************************************************************************************
*************************************************************************************************
***********************************EXECUTE STORED PROEDURE***************************************
*************************************************************************************************
************************************************************************************************/

DECLARE @OwnershipNodeId INT, @Date DATE = '2020-05-15'
    SET @OwnershipNodeId = 3270
	
EXEC [Admin].[usp_SaveOperativeMovementsWithOwnershipPercentage] @OwnershipNodeId

SELECT * FROM [Analytics].[OperativeMovementsWithOwnership] WHERE OperationalDate = @Date
SELECT * FROM [Analytics].[OwnershipPercentageValues] WHERE OperationalDate = @Date
SELECT * FROM [Admin].[OwnerShipNode] WHERE OwnershipNodeId = @OwnershipNodeId

/* EXPECTED OUTPUT:  IT WILL UPDATE BELOW MOVEMENTID RECORDS

                     937251470671658340

					 AND IT WILL INSERT BELOW MOVEMENTID RECORDS
                     
					 937251470671658345
*/
