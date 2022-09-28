/*-- ===============================================================================================================================================
-- Author:           Microsoft
-- Created Date:     Dec-16-2019
-- Updated Date:     Jun-26-2020
-- <Description>:    This Procedure is used to get the Movement Data based on the input of Node List, Start Date, End Date, TicketId. </Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetMovements]
(
       @NodeList                    [Admin].[NodeListType] READONLY,
       @StartDate                   DATE,
       @EndDate                     DATE,
       @TicketId                    INT,
       @MovementTransactionIds      [Admin].[KeyType] READONLY
)
AS
BEGIN
        IF OBJECT_ID('tempdb..#TempGetMovementDetails')IS NOT NULL
        DROP TABLE #TempGetMovementDetails 

        SELECT  
                Mov.MovementId
               ,Mov.SourceNodeId
               ,Mov.DestinationNodeId
               ,Mov.SourceProductName
               ,Mov.SourceProductId
               ,Mov.DestinationProductName
               ,Mov.DestinationProductId
               ,Mov.OperationalDate
               ,Mov.NetStandardVolume
               ,Mov.UncertaintyPercentage
               ,CE.Name AS MeasurementUnit
               ,Mov.[MessageTypeId]
               ,Mov.GlobalMovementId
        INTO #TempGetMovementDetails
        FROM [Admin].[view_MovementInformation] Mov
        INNER JOIN Admin.CategoryElement CE
        ON CE.ElementId = Mov.MeasurementUnit
        WHERE Mov.OperationalDate BETWEEN @StartDate AND @EndDate
        AND (Mov.SourceNodeId IN (SELECT NodeId FROM @NodeList) OR Mov.DestinationNodeId IN (SELECT NodeId FROM @NodeList))
        AND ((@TicketId = 0 AND (Mov.IsTransferPoint = 0 OR (Mov.IsTransferPoint = 1 AND ((Mov.GlobalMovementId IS NOT NULL AND Mov.IsOfficial = 1)
                                    OR Mov.MovementTransactionId in (SELECT [Key] from @MovementTransactionIds)))))
            OR Mov.TicketId = @TicketId)
        AND CE.CategoryId = 6
        AND Mov.ScenarioId = 1 

        SELECT  
                Mov.MovementId
               ,Mov.SourceNodeId
               ,Mov.DestinationNodeId
               ,Mov.SourceProductName
               ,Mov.SourceProductId
               ,Mov.DestinationProductName
               ,Mov.DestinationProductId
               ,Mov.OperationalDate
               ,Mov.NetStandardVolume
               ,Mov.UncertaintyPercentage
               ,Mov.MeasurementUnit
               ,Mov.[MessageTypeId]
        FROM #TempGetMovementDetails Mov
END


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get the Movement Data based on the input of Node List, Start Date, End Date, TicketId.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetMovements',
    @level2type = NULL,
    @level2name = NULL