DECLARE
	   @TicketId				INT = 31366,
	   @NodeList				[Admin].[NodeListType]	
	   
	   INSERT INTo @NodeList (NodeId)
	   VALUES (42574)

	EXEC   [Admin].[usp_GetDeltaOfficialInventory] @TicketId, @NodeList
	   

--MovementTransactionId	MovementOwnerId	OperationalDate	NodeId	ProductID	OwnerId		OwnershipVolume
--116388					124				2020-05-31		42574	10000002318		124		-6400.00
--116384					30				2020-06-30		42574	10000002318		30		-9600.00
--116384					124				2020-06-30		42574	10000002318		124		-6400.00
--116380					29				2020-05-31		42574	10000002318		29		8000.00
--116380					30				2020-05-31		42574	10000002318		30		2400.00
		


