/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jul-06-2020
-- <Description>:	This Procedure is used to get the Consolidated Movements on SegmentId , StartDate, EndDate. </Description>
Unit testing for the procedure with SON=1 and SON=0
-- ================================================================================================================================================*/


EXEC admin.usp_GetInventoriesForConsolidation 121054, '2020-06-29', '2020-06-29'  --for IsOperationalSegment=1, IsSegmentSON=1

--InventoryProductId	NodeId		ProductId	SegmentId	MeasurementUnit	SourceSystemId	ProductVolume	GrossStandardQuantity	OwnerId	OwnershipPercentage	OwnershipVolume	OwnershipValueUnit
--7836					27191		10000002318		121054		31			164				41500.00		120.00					30			100.00				41500.00			NULL
--7832					27191		10000002372		121054		31			164				1700.00			120.00					30			100.00				1700.00				NULL


EXEC admin.usp_GetInventoriesForConsolidation 121090, '2020-06-29', '2020-06-29'  --for IsOperationalSegment=1, IsSegmentSON=1

--InventoryProductId	NodeId	ProductId		SegmentId	MeasurementUnit	SourceSystemId	ProductVolume	GrossStandardQuantity	OwnerId	OwnershipPercentage	OwnershipVolume	OwnershipValueUnit
--7846					27199	10000002372		121090		31					164			1700.00			120.00					30		100.00				1700.00			NULL
--7842					27199	10000002318		121090		31					164			41500.00		120.00					30		100.00				41500.00		NULL


EXEC admin.usp_GetInventoriesForConsolidation 121191, '2020-06-29', '2020-06-29'  --for IsOperationalSegment=0, IsSegmentSON=0

--InventoryProductId	NodeId	ProductId	SegmentId	MeasurementUnit	SourceSystemId	ProductVolume	GrossStandardQuantity	OwnerId	OwnershipPercentage	OwnershipVolume	OwnershipValueUnit
--7861					27223	10000002372		121191		31				164			1700.00				120.00				121205		NULL				100.00			%
--7862					27223	10000002318		121191		31				164			41500.00			120.00				121205		NULL				100.00			%