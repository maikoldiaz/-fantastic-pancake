/*-- ==============================================================================================================================
-- Author:					Microsoft
-- Created date:			Dec-12-2019
-- Updated Date:			Mar-20-2020
-- Modification Date:		Mar-30-2020			
-- <Description>:	This Procedure is used to get the Event details for the Excel file based on the Ticket Id. </Description>
					03/30/2020: Added IsAgreement and OwnerId2.
-- ================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetEventDetails] 
(
		 @TicketId		INT
)
AS 
BEGIN
		DECLARE   @SegementId		INT
				 ,@StartDate		DATE
				 ,@EndDate			DATE

		SELECT DISTINCT  @StartDate	  =   StartDate
					    ,@EndDate     =   EndDate
						,@SegementId  =   CategoryElementId
		FROM [Admin].Ticket Tic
		WHERE Tic.TicketId = @TicketId
		AND Tic.TicketTypeId = 2 --OwnerShip

		SELECT  DISTINCT 
				Eve.EventId									AS	EventIdentifier
			   ,CASE 
					WHEN OwnEvent.Name = 'AcuerdoColaboracion'
						THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)
				END											AS  IsAgreement
			   ,Eve.[SourceNodeId]							AS	SourceNodeId	
			   ,Eve.[DestinationNodeId]						AS	DestinationNodeId	
			   ,Eve.[SourceProductId]						AS	SourceProductId		
			   ,Eve.[DestinationProductId]					AS	DestinationProductId	
			   ,Eve.[Owner1Id]								AS	OwnerId1
			   ,Eve.[Owner2Id]								AS	OwnerId2
			   ,Eve.[Volume]								AS	OwnershipValue	
			   ,CASE When Eve.MeasurementUnit  = 159 
					 Then 'PORCENTAJE'	---- CategoryElementId-159 = 'Porcentaje'
					 ELSE 'VOLUMEN'
				End											AS	MeasurementUnit
		FROM [Admin].[Event] Eve
		INNER JOIN Admin.NodeTag NT
		ON ( [NT].[NodeId] = Eve.[SourceNodeId] OR [NT].[NodeId] = Eve.[DestinationNodeId] )		
		INNER JOIN Admin.CategoryElement OwnEvent
		ON OwnEvent.ElementId = Eve.[EventTypeId]
		WHERE [NT].ElementId		= @SegementId
		AND OwnEvent.CategoryId		= 12 --EventName
		AND Eve.IsDeleted			= 0  --Active events 
		AND (
				  (@StartDate BETWEEN CAST(Eve.StartDate AS DATE) AND CAST(Eve.EndDate AS DATE))
			OR    (@EndDate   BETWEEN CAST(Eve.StartDate AS DATE) AND CAST(Eve.EndDate AS DATE))
			)
		AND (
				  (@StartDate BETWEEN CAST(NT.StartDate AS DATE) AND CAST(NT.EndDate AS DATE))
			OR    (@EndDate   BETWEEN CAST(NT.StartDate AS DATE) AND CAST(NT.EndDate AS DATE))
			)

END 

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get the Event details for the Excel file based on the Ticket Id.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetEventDetails',
    @level2type = NULL,
    @level2name = NULL