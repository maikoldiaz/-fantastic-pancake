/*-- =============================================================================================================================================================================
-- Author:          Microsoft
-- Created Date:	July-15-2020
-- Updated Date:	Oct-14-2020 -- Save previous ticket instead of previous period to find unapproved nodes
-- <Description>:	This Procedure is used to validate all nodes are approved in previous period. </Description>
-- ==============================================================================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_ValidatePreviousOfficialPeriod] 
(
		 @StartDate			DATETIME
		,@EndDate   		DATETIME
        ,@SegmentId         INT
)
AS 
BEGIN
    DECLARE  @PreviousTicket        INT
            ,@NotApprovedCount      INT

    SET @PreviousTicket = NULL

    IF OBJECT_ID('tempdb..#PreviousPeriodSegmentInfo') IS NOT NULL DROP TABLE #PreviousPeriodSegmentInfo
    CREATE TABLE #PreviousPeriodSegmentInfo ( SegmentId INT )
    IF @SegmentId = 0
    BEGIN
        INSERT INTO #PreviousPeriodSegmentInfo ( SegmentId )
        SELECT  ElementId
        FROM Admin.CategoryElement
        WHERE CategoryId = 2 AND IsActive = 1
    END
    ELSE
    BEGIN
        INSERT INTO #PreviousPeriodSegmentInfo ( SegmentId ) SELECT  @SegmentId
    END

    SELECT TOP 1 
    @PreviousTicket = TIC.TicketId
    FROM Admin.Ticket TIC 
    WHERE TIC.CategoryElementId IN (SELECT SegmentId FROM #PreviousPeriodSegmentInfo) AND
    TIC.TicketTypeId = 5 AND
    TIC.EndDate <= @StartDate
    ORDER BY Tic.CreatedDate DESC

    IF @PreviousTicket IS NOT NULL
    BEGIN
        SELECT @NotApprovedCount = COUNT(*)
        FROM Admin.Ticket TIC
        INNER JOIN Admin.DeltaNode DN
        ON DN.TicketId = TIC.TicketId
        WHERE TIC.CategoryElementId IN (SELECT SegmentId FROM #PreviousPeriodSegmentInfo)
        AND DN.Status != 9 AND TIC.TicketTypeId = 5 AND
        TIC.TicketId = @PreviousTicket
    END
    ELSE SET @NotApprovedCount = 0

    SELECT @NotApprovedCount AS [UnApprovedNodes]
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to validate all nodes are approved in previous period.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_ValidatePreviousOfficialPeriod',
    @level2type = NULL,
    @level2name = NULL