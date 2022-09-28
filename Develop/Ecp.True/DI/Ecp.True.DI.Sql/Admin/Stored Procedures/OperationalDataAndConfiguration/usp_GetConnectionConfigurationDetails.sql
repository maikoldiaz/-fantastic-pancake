/*-- ============================================================================================================================================
-- Author:          Microsoft  
-- Create date:     Nov-22-2019  
-- Updated Date:	Mar-20-2020
-- <Description>:   This Procedure is used to get the Connection Configuration data for the Excel file based on the Ticket Id. </Description>
-- =============================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetConnectionConfigurationDetails] 
(
	@TicketId INT
) 
AS 
BEGIN 
	  DECLARE @StartDate DATETIME 
      DECLARE @EndDate   DATETIME

      SET @StartDate= (SELECT CAST(StartDate  AS DATE)
                       FROM   [Admin].[Ticket] 
                       WHERE  TicketId = @TicketId) 

      SET @EndDate=  (SELECT CAST(EndDate  AS DATE)
                      FROM   [Admin].[Ticket] 
                      WHERE  TicketId = @TicketId)  

      SELECT  DISTINCT [NC].[SourceNodeId]				 AS SourceNodeId,
                       [NC].[DestinationNodeId]			 AS DestinationNodeId,
                       [NCP].[ProductId]				 AS ProductId,
                       [NCP].[Priority]					 AS Prioritization,
                       [NCP].NodeConnectionProductRuleId AS NodeConnectionProductRuleId
      FROM   [Admin].[NodeTag] NT 
      INNER JOIN [Admin].[Ticket] Tic 
      ON [NT].[ElementId] = [Tic].[CategoryElementId] 
      INNER JOIN [Admin].[NodeConnection] NC 
      ON ( [NT].[NodeId] = [NC].[SourceNodeId] OR [NT].[NodeId] = [NC].[DestinationNodeId] )
      LEFT JOIN [Admin].[NodeConnectionProduct] NCP 
      ON [NCP].[NodeConnectionId] = [NC].[NodeConnectionId]
      WHERE  [Tic].[TicketId] = @TicketId 
	  		AND [Tic].[TicketTypeId]=2				-- TicketTypeID 2 represents Ownership
            AND @StartDate >= NT.StartDate 
			AND @EndDate   <= Nt.EndDate
            AND NC.IsDeleted = 0
END 

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get the Connection Configuration data for the Excel file based on the Ticket Id.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetConnectionConfigurationDetails',
    @level2type = NULL,
    @level2name = NULL