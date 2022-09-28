/******************************************************************************
-- Type = After this stores procedure execute data should e loaded into actual table and stage table should be truncated
-- Stage Table : [Analytics].[Stage_OperativeMovementsWithOwnership]
-- Actual Table : [Analytics].[OperativeMovementsWithOwnership]
********************************************************************************/

INSERT INTO [Analytics].[Stage_OperativeMovementsWithOwnership]
                            ([OperationalDate],[MovementType],[SourceProduct],[SourceStorageLocation]
                             ,[DestinationProduct],[DestinationStorageLocation],[OwnershipVolume]
            				 ,[TransferPoint],[Month],[Year],[DayOfMonth],[SourceSystem],[LoadDate]
                             ,[ExecutionID],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate]
            				 )
                     VALUES ('2016-06-28','Tr. Material a material','CRUDO CAMPO CAÑADA NORTE','PR SAN JACINTO:MATERIA PRIMA'
					        ,'CRUDO MEZCLA','PR SAN JACINTO:MATERIA PRIMA','14669.00','CAÑADA_NORTE_OAM','6','2016','30','CSV'
							,'2020-02-05 02:49:11.460','809E1DDF-C933-4D55-B16E-36AB58169CB0','ADF','2020-02-05 02:49:11.460'
							,'',''
							)
EXEC  [Analytics].[usp_Insert_OperativeMovementsWithOwnership]

IF ((SELECT COUNT(*) FROM [Analytics].[Stage_OperativeMovementsWithOwnership]) = 0 )
BEGIN
   PRINT 'STAGE TABLE TRUNCATED'
END
ELSE
BEGIN
  PRINT 'STAGE TABLE NOT TRUNCATED'
END
