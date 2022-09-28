/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Mar-06-2020
-- Updated Date:	Mar-20-2020
-- Updated Date:	Jun-11-2020  1. New Parameter (LoadType) is added to differentiate the load type 
                                    (Values will be passed by ADF Pipeline)
                                    0 --> HistoricalLoad
									1 --> GAP Load
                                 2. StartDate and EndDate will be input paramters will get the values from ADF Pipeline.
                                    If no values passed from ADF then take StartDate: 01-06-2016 & EndDate: 31-05-2020 
								 3. Truncating stage table in case of rollback to avoid duplicate records issues in next pipeline trigger
-- <Description>:   This Procedure is to load the data from stage table to table "Analytics.OperativeMovements" by excluding the
                    records where operationaldate between '2016-06-01' AND '2019-07-31'. </Description>
-- ================================================================================================================================*/
CREATE PROCEDURE [Analytics].[usp_Insert_OperativeMovements]
(
 @LoadType INT
,@StartDate DATE = '2016-06-01'
,@EndDate DATE = '2020-05-31'
)
AS
BEGIN
   SET NOCOUNT ON
     BEGIN TRY
	      BEGIN TRANSACTION
		        IF (@LoadType = 1)
				   BEGIN
                        INSERT INTO [Analytics].[OperativeMovements] 
        				            ([OperationalDate],[DestinationNode],[DestinationNodeType],[MovementType],[SourceNode]
                                    ,[SourceNodeType],[SourceProduct],[SourceProductType],[TransferPoint],[FieldWaterProduction]
                                    ,[SourceField],[RelatedSourceField],[NetStandardVolume],[SourceSystem],[LoadDate],[ExecutionID]
                                    ,[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate]
            	        			)
                     -- EXCLUDING THE RECORDS WHERE OPERATIONAL BETWEEN @StartDate AND @EndDate
                        SELECT [OperationalDate],[DestinationNode],[DestinationNodeType],[MovementType],[SourceNode]
                              ,[SourceNodeType],[SourceProduct],[SourceProductType],[TransferPoint],[FieldWaterProduction]
                              ,[SourceField],[RelatedSourceField],[NetStandardVolume],[SourceSystem],[LoadDate],[ExecutionID]
                              ,[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate] 
            	           FROM [Analytics].[Stage_OperativeMovements]
            	          WHERE SourceSystem = 'CSV'
            	            AND OperationalDate NOT BETWEEN @StartDate AND @EndDate
	               END
				   
                ELSE
				   BEGIN
				     -- DELETE DATA FROM TABLE BEFORE DO THE HISTORICAL LOAD
						DELETE FROM [Analytics].[OperativeMovements]
						WHERE SourceSystem = 'CSV'
						  AND OperationalDate BETWEEN @StartDate AND @EndDate

                        INSERT INTO [Analytics].[OperativeMovements] 
        				            ([OperationalDate],[DestinationNode],[DestinationNodeType],[MovementType],[SourceNode]
                                    ,[SourceNodeType],[SourceProduct],[SourceProductType],[TransferPoint],[FieldWaterProduction]
                                    ,[SourceField],[RelatedSourceField],[NetStandardVolume],[SourceSystem],[LoadDate],[ExecutionID]
                                    ,[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate]
            	        			)
                     -- INCLUDING ONLY THE RECORDS WHERE OPERATIONAL BETWEEN @StartDate AND @EndDate
                        SELECT [OperationalDate],[DestinationNode],[DestinationNodeType],[MovementType],[SourceNode]
                              ,[SourceNodeType],[SourceProduct],[SourceProductType],[TransferPoint],[FieldWaterProduction]
                              ,[SourceField],[RelatedSourceField],[NetStandardVolume],[SourceSystem],[LoadDate],[ExecutionID]
                              ,[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate] 
            	           FROM [Analytics].[Stage_OperativeMovements]
            	          WHERE SourceSystem = 'CSV'
						    AND OperationalDate BETWEEN @StartDate AND @EndDate
	               END
                   
     			  -- TRUNCATING THE STAGE TABLE AFTER THE LOAD IS COMPLETED INTO ACTUAL TABLE
     		        TRUNCATE TABLE [Analytics].[Stage_OperativeMovements]
          COMMIT TRANSACTION
	  END TRY

	  BEGIN CATCH
	       ROLLBACK TRANSACTION
        -- TRUNCATING THE STAGE TABLE AT THE TIME OF FAILURE TO AVOID THE DUPLICATES IN NEXT RUN
		   TRUNCATE TABLE [Analytics].[Stage_OperativeMovements]
	  END CATCH
END

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to load the data from stage table to table "Analytics.OperativeMovements" by excluding the records where operationaldate between 2016-06-01 AND 2019-07-31.',
	@level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_Insert_OperativeMovements',
    @level2type = NULL,
    @level2name = NULL