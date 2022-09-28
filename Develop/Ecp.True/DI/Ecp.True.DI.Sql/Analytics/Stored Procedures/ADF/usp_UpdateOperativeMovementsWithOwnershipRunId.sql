/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Feb-03-2020
-- Updated Date:	Mar-20-2020
-- <Description>:   This Procedure is to update the executionid for the latest loaded records and delete the previous load data in table "Analytics.OperativeMovementsWithOwnership" </Description>
-- ================================================================================================================================*/
CREATE PROCEDURE [Analytics].[usp_UpdateOperativeMovementsWithOwnershipRunId]
(
 @RunId			VARCHAR (100)
)
AS
BEGIN
   -- Update execution id's for the latest records
	UPDATE Analytics.OperativeMovementsWithOwnership
	   SET ExecutionId =  @RunId
	 WHERE ExecutionId IS NULL
END

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to update the executionid for the latest loaded records and delete the previous load data in table "Analytics.OperativeMovementsWithOwnership"',
	@level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_UpdateOperativeMovementsWithOwnershipRunId',
    @level2type = NULL,
    @level2name = NULL