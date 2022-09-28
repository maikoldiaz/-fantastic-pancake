/******************************************************************************
-- Type = After this stores procedure execute data should e loaded into actual table and stage table should be truncated
-- Stage Table : [Analytics].[Stage_OperativeMovements]
-- Actual Table : [Analytics].[OperativeMovements]
********************************************************************************/

INSERT INTO [Analytics].[Stage_OperativeMovements]
                            ([OperationalDate],[DestinationNode],[DestinationNodeType],[MovementType],[SourceNode]
                            ,[SourceNodeType],[SourceProduct],[SourceProductType],[TransferPoint],[FieldWaterProduction]
                            ,[SourceField],[RelatedSourceField],[NetStandardVolume],[SourceSystem],[LoadDate],[ExecutionID]
                            ,[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate]
    	        			)
                     VALUES ('2016-05-01','ARAGUANEY - OBC','Estación','ENTREGAS','EPF FLOREÑA'
							,'Limite','CRUDO FLOREÑA','CRUDO','FLOREÑA_OBC','na','na','CUS_CUP_FLO'
							,'35775.05','CSV','2020-02-14 13:04:33.290','C6DEC884-E777-4E23-9122-C569C1F10055'
							,'ADF','2020-02-14 13:04:33.290','',''
							)
EXEC  [Analytics].[usp_Insert_OperativeMovements]

IF ((SELECT COUNT(*) FROM [Analytics].[Stage_OperativeMovements]) = 0 )
BEGIN
   PRINT 'STAGE TABLE TRUNCATED'
END
ELSE
BEGIN
  PRINT 'STAGE TABLE NOT TRUNCATED'
END
