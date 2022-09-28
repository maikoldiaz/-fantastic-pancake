/*-- ==============================================================================================================================
-- Author:				Microsoft
-- Created date:		Mar-30-2020
-- Updated Date:		Aug-06-2020 -- Removed Casting from Cont.MeasurementUnit as it is already INT
						March-04-2021 -- Added filter to exclude self-consumption and transfers and unauthorized, delete, locked Contracts
						Jun-01-2021 -- Self-consumption type movement and all frequencies are included
						Jun-10-2021 -- Call udf_GetContractValue function to update contract value sent to fico
						Jun-17-2021 -- Add ownership date parameter into udf_GetContractValue
-- Description:			This Procedure is used to get the Contract details for the Excel file based on the Ticket Id.
-- [Admin].[usp_GetContractDetails] 40942
 ================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetContractDetails] 
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
		AND Tic.TicketTypeId = 2 -- OwnerShip

		SELECT  DISTINCT 
				Cont.ContractId										 AS	ContractId
			   ,Unit.[Name]											 AS	ContractUnit
			   ,Admin.udf_GetContractValue(Cont.ContractId
			   ,@EndDate)                                            AS	ContractValue	
			   ,CASE 
					WHEN MovementType.[Name] = 'Compra'
						THEN Cont.Owner1Id 
						ELSE Cont.Owner2Id		
				END													  AS BuyerOwnerId
			   ,CASE 
					WHEN MovementType.[Name] IN ('Venta', 'Autoconsumo')
						THEN Cont.Owner1Id 
						ELSE Cont.Owner2Id		
				END													  AS SellerOwnerId
			   ,Cont.[SourceNodeId]									  AS SourceNodeId	
			   ,Cont.[DestinationNodeId]							  AS DestinationNodeId	
			   ,Cont.[ProductId]									  AS ProductId
		FROM [Admin].[Contract] Cont
		INNER JOIN Admin.NodeTag NT
		ON ( [NT].[NodeId] = Cont.[SourceNodeId] OR [NT].[NodeId] = Cont.[DestinationNodeId] )		 
		INNER JOIN Admin.CategoryElement Unit
		ON Unit.ElementId = Cont.MeasurementUnit
		INNER JOIN Admin.CategoryElement MovementType
		ON MovementType.ElementId = Cont.[MovementTypeId]
		WHERE [NT].ElementId		= @SegementId
		AND Unit.CategoryId			= 6 --Unit of measurement
		AND MovementType.CategoryId	= 9 --MovementType
		AND Cont.IsDeleted			= 0 --Active contracts
		AND Cont.MovementTypeId NOT IN (48) --Not transfers 
		AND (Cont.Status!='Desautorizada' AND ISNULL(Cont.PositionStatus, '') NOT IN ('L','S','X')) -- exclude unauthorized, delete, locked
		AND (
				  (@StartDate BETWEEN CAST(Cont.StartDate AS DATE) AND CAST(Cont.EndDate AS DATE))
			OR    (@EndDate   BETWEEN CAST(Cont.StartDate AS DATE) AND CAST(Cont.EndDate AS DATE))
			)
		AND (
				  (@StartDate BETWEEN CAST(NT.StartDate AS DATE) AND CAST(NT.EndDate AS DATE))
			OR    (@EndDate   BETWEEN CAST(NT.StartDate AS DATE) AND CAST(NT.EndDate AS DATE))
			)
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to get the Contract details for the Excel file based on the Ticket Id.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetContractDetails',
    @level2type = NULL,
    @level2name = NULL