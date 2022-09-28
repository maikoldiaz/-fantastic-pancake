/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Jul-16-2020
-- <Description>:   This Procedure is to load the data from stage table to table "Analytics.Stage_OperativeNodeRelationship". </Description>
-- ================================================================================================================================*/
CREATE PROCEDURE [Analytics].[usp_Insert_OperativeNodeRelationship]
AS
BEGIN
   SET NOCOUNT ON
     BEGIN TRY
	      BEGIN TRANSACTION
		        
			    INSERT INTO [Analytics].[OperativeNodeRelationship]
				           ([TransferPoint],[SourceField],[FieldWaterProduction],[RelatedSourceField],[DestinationNode],[DestinationNodeType],[MovementType],[SourceNode]
						   ,[SourceNodeType],[SourceProduct],[SourceProductType],[SourceSystem],[LoadDate],[Notes],[IsDeleted],[CreatedBy]
						   ,[CreatedDate],[LastModifiedBy],[LastModifiedDate]
						   )
				SELECT [TransferPoint],[SourceField],[FieldWaterProduction],[RelatedSourceField],[DestinationNode],[DestinationNodeType],[MovementType],[SourceNode]
					  ,[SourceNodeType],[SourceProduct],[SourceProductType],[SourceSystem],[LoadDate],[Notes],[IsDeleted],[CreatedBy]
					  ,[CreatedDate],[LastModifiedBy],[LastModifiedDate]
				  FROM [Analytics].[Stage_OperativeNodeRelationship] 
				  				   				   				  							 			 					  		   		   		  			 	 	   
     		  -- TRUNCATING THE STAGE TABLE AFTER THE LOAD IS COMPLETED INTO ACTUAL TABLE
     		     TRUNCATE TABLE [Analytics].[Stage_OperativeNodeRelationship]
          COMMIT TRANSACTION
	  END TRY

	  BEGIN CATCH
	       ROLLBACK TRANSACTION
        -- TRUNCATING THE STAGE TABLE AT THE TIME OF FAILURE TO AVOID THE DUPLICATES IN NEXT RUN
		   TRUNCATE TABLE [Analytics].[Stage_OperativeNodeRelationship]
	  END CATCH
END

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to load the data from stage table to table "Analytics.Stage_OperativeNodeRelationship". ',
	@level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_Insert_OperativeNodeRelationship',
    @level2type = NULL,
    @level2name = NULL