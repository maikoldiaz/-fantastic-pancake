--Unit testing in UAT for usp_GetLogisticNodeValidation issue fix
DECLARE @SegmentId            INT  = 72042,
        @StartDate            DATE = '2020-06-21 00:00:00.000',
        @EndDate              DATE = '2020-06-24 00:00:00.000',
        @NodeId               INT  = NULL


EXEC [Admin].[usp_GetLogisticNodeValidation_Test] @SegmentId         
                                                  ,@StartDate         
                                                  ,@EndDate           
                                                  ,@NodeId       
                                                 
--NodeId		NodeName           OperationDate    NodeStatus
--15845        Automation_1735i    2020-06-23        Propiedad
--15845        Automation_1735i    2020-06-22        Propiedad
--15845        Automation_1735i    2020-06-21        Propiedad
--15842        Automation_i8ndb    2020-06-24        Desbloqueado
--15842        Automation_i8ndb    2020-06-23        Propiedad
--15842        Automation_i8ndb    2020-06-22        Propiedad
--15842        Automation_i8ndb    2020-06-21        Propiedad