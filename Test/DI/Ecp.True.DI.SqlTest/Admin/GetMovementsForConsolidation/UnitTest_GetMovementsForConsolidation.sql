/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jul-06-2020
-- <Description>:	This Procedure is used to get the Consolidated Movements on SegmentId , StartDate, EndDate. </Description>
Unit testing for procedure with SON=1 and SON=0
-- ================================================================================================================================================*/


EXEC admin.usp_GetMovementsForConsolidation 129341, '2020-07-01', '2020-07-01'  --for IsOperationalSegment=1, IsSegmentSON=1

--MovementTransactionId	SourceNodeId	SourceProductId	DestinationNodeId	DestinationProductId	SegmentId	MovementTypeId	MeasurementUnit	SourceSystemId	NetStandardVolume	GrossStandardVolume	SourceMovementTransactionId	SourceMovementTypeId	OwnerId	OwnershipPercentage	OwnershipVolume	OwnershipValueUnit
--19892					NULL			 NULL			29093					10000002372			129341		44				31					NULL			426871.82		NULL					NULL						NULL				30			100.00				426871.82		NULL
--19893					NULL			 NULL			29092					10000002318			129341		44				31					NULL			156871.82		NULL					NULL						NULL				30			100.00				156871.82		NULL
--19891					29094			 10000002372	NULL					NULL				129341		44				31					NULL			215216.10		NULL					NULL						NULL				30			100.00				215216.10		NULL
--19894					NULL			 NULL			29091					10000002318			129341		44				31					NULL			296771.82		NULL					NULL						NULL				30			100.00				296771.82		NULL
--19871					29093			 10000002372	29091					10000002372			129341		129351			31					164				426871.82		200.00					NULL						NULL				30			100.00				426871.82		NULL
--19870					29092			 10000002318	29091					10000002318			129341		129351			31					164				156871.82		200.00					NULL						NULL				30			100.00				156871.82		NULL
--19869					29091			 10000002318	29094					10000002318			129341		129351			31					164				316871.82		200.00					NULL						NULL				30			100.00				316871.82		NULL
--19868					29091			 10000002372	29094					10000002372			129341		129351			31					164				216871.82		200.00					NULL						NULL				30			100.00				216871.82		NULL
--19883					29094			 10000002372	NULL					NULL				129341		43				31					NULL			1655.72			NULL					NULL						NULL				30			100.00				1655.72			NULL
--19880					NULL			 NULL			29091					10000002372			129341		44				31					NULL			192871.82		NULL					NULL						NULL				30			100.00				192871.82		NULL



EXEC admin.usp_GetMovementsForConsolidation 121191, '2020-06-29', '2020-06-29'  --for IsOperationalSegment=0, IsSegmentSON=0

--MovementTransactionId	SourceNodeId	SourceProductId	DestinationNodeId	DestinationProductId	SegmentId	MovementTypeId	MeasurementUnit	SourceSystemId	NetStandardVolume	GrossStandardVolume	SourceMovementTransactionId	SourceMovementTypeId	OwnerId	OwnershipPercentage	OwnershipVolume	OwnershipValueUnit
--18522					27223			10000002372			27226			10000002372				121191		121201				31			164				416871.82			200.00				NULL							NULL				121205		NULL				100.00			%
--18523					27223			10000002318			27226			10000002318				121191		121201				31			164				216871.82			200.00				NULL							NULL				121205		NULL				100.00			%
--18524					27225			10000002372			27223			10000002372				121191		121201				31			164				116871.82			200.00				NULL							NULL				121205		NULL				100.00			%
--18526					27224			10000002318			27223			10000002318				121191		121201				31			164				516871.82			200.00				NULL							NULL				121205		NULL				100.00			%