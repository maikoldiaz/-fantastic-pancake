DECLARE
	   @TicketId				INT = 31366,
	   @NodeList				[Admin].[NodeListType]	
	   
	   INSERT INTO @NodeList
	   VALUES(42574)

		EXEC [Admin].[usp_GetDeltaOfficialMovements] @TicketId, @NodeList

--MovementTransactionId	MovementOwnerID	SourceNodeId	DestinationNodeId	SourceProductId	DestinationProductId	MovementTypeId	OwnerId	OwnershipVolume
--116382						30				42574			42576				10000002372		10000002372				154			 30			-324000.00
--116376						30				42574			42576				10000002318		10000002318				156			 30			6600.00
--116376						29				42574			42576				10000002318		10000002318				156			 29			364400.00
--116375						30				42574			42577				10000002318		10000002318				154			 30			-6600.00
--116375						29				42574			42577				10000002318		10000002318				154			 29			151600.00
--116391						124				42574			42577				10000002318		10000002318				154			 124		-156000.00
--116383						124				42574			42576				10000002318		10000002318				154			 124		-360000.00
--116382						124				42574			42576				10000002372		10000002372				154			 124		-216000.00

