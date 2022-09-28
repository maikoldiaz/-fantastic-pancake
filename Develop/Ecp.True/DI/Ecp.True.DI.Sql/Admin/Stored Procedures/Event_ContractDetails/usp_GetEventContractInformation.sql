/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Apr-30-2020 
-- Description:     This is consolidated SP to call two SP's [Admin].[usp_GetContractsInformation] and [Admin].[usp_GetEventsInformation]
                    by passing input paratmers ElementName, NodeName, StartDate, EndDate and ExecutionId.                  
-- EXEC [Admin].[usp_GetEventContractInformation] 'Automation_ppwgo','','2020-04-10','2020-04-20','A2C8DBCB-69BC-43E8-92BD-520D41AB4A23'
   SELECT * FROM [Admin].[ContractsInformation]
   SELECT * FROM [Admin].[EventsInformation]
============================================================================================================================ --*/
CREATE PROCEDURE [Admin].[usp_GetEventContractInformation]
(
	     @ElementName	NVARCHAR(250) 
		,@NodeName	    NVARCHAR(250) 
		,@StartDate		DATE		  
		,@EndDate		DATE		  
		,@ExecutionId	NVARCHAR(250) 
)
AS
BEGIN
	SET NOCOUNT ON
                
				EXEC [Admin].[usp_GetEventsInformation] @ElementName,@NodeName,@StartDate,@EndDate,@ExecutionId
				EXEC [Admin].[usp_GetContractsInformation] @ElementName,@NodeName,@StartDate,@EndDate,@ExecutionId
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is consolidated SP to call two SPs [Admin].[usp_GetContractsInformation] and [Admin].[usp_GetEventsInformation] by passing input paratmers ElementName, NodeName, StartDate, EndDate and ExecutionId.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetEventContractInformation',
    @level2type = NULL,
    @level2name = NULL