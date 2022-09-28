/*-- ==============================================================================================================================
-- Author:			Microsoft  
-- Created Date:	Mar-30-2020
-- Updated Date:	Jul-16-2020 -- Applied transactions
<Description>:		
                    This procedure will perform the delta load logic to load the data into Ownership Percentage table.  
</Description>
EXEC [Analytics].[usp_UpsertOwnershipPercentageValuesHistory] '4B640D4E-F067-45A2-B802-CAEA70929BF1'
-- ================================================================================================================================*/
CREATE PROCEDURE [Analytics].[usp_UpsertOwnershipPercentageValuesHistory]
(
@ExecutionId NVARCHAR (200)
)
AS
BEGIN
   SET NOCOUNT ON
		BEGIN TRY
		    BEGIN TRANSACTION
			    -- MERGING THE DATA FROM STAGE TABLE TO MAIN TABLE
				   MERGE INTO [Analytics].[OwnershipPercentageValues] DEST
				   USING [Analytics].[Stage_OwnershipPercentageValues] SRC
				      ON SRC.OperationalDate = DEST.OperationalDate
					 AND SRC.TransferPoint   = DEST.TransferPoint
                -- UPDATE PERCENTAGE VALUE IF KEY COMBINATION MATCHES
				   WHEN MATCHED AND SRC.OwnershipPercentage <> DEST.OwnershipPercentage THEN
				   UPDATE SET DEST.OwnershipPercentage = SRC.OwnershipPercentage
					        , DEST.ExecutionId         = @ExecutionId
					        , DEST.LastModifiedBy      = 'ADF'
						    , DEST.LastModifiedDate    = admin.udf_GetTrueDate()
                -- INSERT THE RECORD AS NEW RECORD IF KEY COMBINATION NOT MATCHES
				   WHEN NOT MATCHED THEN
				   INSERT ([OperationalDate],[TransferPoint],[OwnershipPercentage],[LoadDate]
				           ,[ExecutionID])
				   VALUES (SRC.[OperationalDate],SRC.[TransferPoint],SRC.[OwnershipPercentage],SRC.LoadDate
				           ,@ExecutionId)
				   ;
            -- TRUNCATE STAGE TABLE AFTER SUCCESSFULL RUN
			   TRUNCATE TABLE [Analytics].[Stage_OwnershipPercentageValues]

            COMMIT TRANSACTION
		END TRY

		BEGIN CATCH
		     ROLLBACK TRANSACTION
			 -- TRUNCATING THE STAGE TABLE AT THE TIME OF FAILURE TO AVOID THE DUPLICATES IN NEXT RUN
			    TRUNCATE TABLE [Analytics].[Stage_OwnershipPercentageValues]
		END CATCH
END


GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This procedure will perform the delta load logic to load the data into Ownership Percentage table.',
	@level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_UpsertOwnershipPercentageValuesHistory',
    @level2type = NULL,
    @level2name = NULL