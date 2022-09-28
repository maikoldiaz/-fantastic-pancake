/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Feb-03-2020
-- Updated Date:	Mar-20-2020
-- <Description>:   This Procedure is to update the executionid for the latest loaded records and delete the previous load data in table "Analytics.OperativeNodeRelationship"
</Description>
-- ================================================================================================================================*/
CREATE PROCEDURE [Analytics].[usp_UpdateOperativeNodeRelationshipRunId]
(
 @RunId			VARCHAR (100)
)
AS
BEGIN
   -- Update execution id's for the latest records
	UPDATE Analytics.OperativeNodeRelationship
	   SET ExecutionId =  @RunId
	 WHERE ExecutionId IS NULL

   -- Deleting the old records before the latest pipeline run
	DELETE FROM Analytics.OperativeNodeRelationship 
	 WHERE ExecutionId != @RunId 
	   AND SourceSystem ='CSV'

END

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to update the executionid for the latest loaded records and delete the previous load data in table "Analytics.OperativeNodeRelationship"',
	@level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_UpdateOperativeNodeRelationshipRunId',
    @level2type = NULL,
    @level2name = NULL