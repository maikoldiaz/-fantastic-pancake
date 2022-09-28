/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Jul-16-2020
-- <Description>:   This Procedure is to load the data from stage table to table "Analytics.Stage_OperativeNodeRelationshipWithOwnership". </Description>
-- ================================================================================================================================*/
CREATE PROCEDURE [Analytics].[usp_Insert_OperativeNodeRelationshipWithOwnership]
AS
BEGIN
   SET NOCOUNT ON
     BEGIN TRY
	      BEGIN TRANSACTION
		        
			    INSERT INTO [Analytics].[OperativeNodeRelationshipWithOwnership] 
				           ([SourceProduct],[LogisticSourceCenter],[DestinationProduct],[LogisticDestinationCenter],[TransferPoint],[IsDeleted],[Notes]
						   ,[SourceSystem],[LoadDate],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate]
						   )
				SELECT [SourceProduct],[LogisticSourceCenter],[DestinationProduct],[LogisticDestinationCenter],[TransferPoint],[IsDeleted],[Notes]
					  ,[SourceSystem],[LoadDate],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate]
				  FROM [Analytics].[Stage_OperativeNodeRelationshipWithOwnership] 
				  				   				   				  							 			 					  		   		   		  			 	 	   
     		  -- TRUNCATING THE STAGE TABLE AFTER THE LOAD IS COMPLETED INTO ACTUAL TABLE
     		     TRUNCATE TABLE [Analytics].[Stage_OperativeNodeRelationshipWithOwnership]
          COMMIT TRANSACTION
	  END TRY

	  BEGIN CATCH
	       ROLLBACK TRANSACTION
        -- TRUNCATING THE STAGE TABLE AT THE TIME OF FAILURE TO AVOID THE DUPLICATES IN NEXT RUN
		   TRUNCATE TABLE [Analytics].[Stage_OperativeNodeRelationshipWithOwnership]
	  END CATCH
END

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to load the data from stage table to table "Analytics.Stage_OperativeNodeRelationshipWithOwnership". ',
	@level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_Insert_OperativeNodeRelationshipWithOwnership',
    @level2type = NULL,
    @level2name = NULL