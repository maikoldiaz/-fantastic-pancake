
INSERT INTO [Admin].[DeltaError] 
			(
		     [MovementTransactionId]
			,[InventoryProductId]
			,[TicketId]
			,[ErrorMessage]
			,[CreatedBy]
			,[LastModifiedBy]
			,[LastModifiedDate]
			) 
VALUES		(10880,3918,89,'Error','System1',null,null)
			,(10900,3963,23751,'Error 500','System2',null,null)
			,(11022,3879,23751,'Error 500','System3',null,null)
			,(11023,3880,89,'Error 400','System4',null,null)
			,(11024,3883,89,'Error 404','System5',null,null)
			,(11027,3885,23751,'Error 501','System6',null,null);

EXEC [Admin].[usp_GetDeltaErrorDetailsForMovAndInventories] @TicketId=89;
--Identifier			Type		SourceNode			DestinationNode		SourceProduct		DestinationProduct	Quantity	Unit	Date		Error
--637268412220504405	Movement	Automation_uhwma	NULL				CRUDO CAMPO MAMBO	NULL				528932.92	Bbl		2020-05-31	Error
--2					Movement	Automation_9197y	Automation_mptiw	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	6888.82		Bbl		2020-05-25	Error 400
--3					Movement	Automation_9197y	Automation_9197y	CRUDO CAMPO MAMBO	CRUDO CAMPO CUSUCO	3456.82		Bbl		2020-05-25	Error 404
--TK456321			Inventory	Automation_wyzbh	NULL				CRUDO CAMPO MAMBO	NULL				9600.00		Bbl		2020-06-02	Error
--DEFECT 691408		Inventory	Automation_wd1lm	NULL				CRUDO CAMPO MAMBO	NULL				19508.00	Bbl		2020-05-30	Error 400
--DEFECT 465057		Inventory	Automation_8jvo5	NULL				CRUDO CAMPO MAMBO	NULL				19508.00	Bbl		2020-05-30	Error 404

