/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jun-12-2020
-- <Description>:	This Procedure is used to get the Delta Inventores Data based on the input of SegmentId, Start Date, End Date</Description>
	
	Delta Inventories:
	Step 1) Find all the Inventories for a segment based on date Range
	Step 2) From Step 1 at least one Record Should have TicketID, so that Segment is Eligible for Delta---> Cutoff is generated or not for the segment
	Step 3) Now For each Inventory Pick the Latest Record Based on The Inventory Productid
	Step 4)The Latest record should not have the ticketID and ProductVolume > 0 then this is Inventory is eligible for delta calculation 
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetDeltaInventories]
(
	   @SegmentId				INT,
       @StartDate				DATE,
	   @EndDate					DATE
	   
)
AS
BEGIN

			 SELECT  InvPrd.InventoryId
					,InvPrd.NodeName
					,InvPrd.ProductName
					,InvPrd.ProductVolume
					,InvPrd.InventoryProductId
					,InvPrd.EventType
			 FROM Admin.view_InventoryInformation InvPrd
			 WHERE InvPrd.SegmentId = @SegmentId
			 AND InvPrd.InventoryDate BETWEEN  @StartDate AND @EndDate
END

GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Delta Inventores Data based on the input of SegmentId, Start Date, End Date',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetDeltaInventories',
							@level2type = NULL,
							@level2name = NULL