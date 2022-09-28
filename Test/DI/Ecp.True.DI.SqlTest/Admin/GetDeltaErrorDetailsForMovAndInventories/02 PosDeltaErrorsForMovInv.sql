
EXEC [Admin].[usp_GetDeltaErrorDetailsForMovAndInventories] @TicketId=23751;



--Identifier	Type		SourceNode			DestinationNode		SourceProduct		DestinationProduct	Quantity	Unit	Date		Error
--5229520		Movement	Automation_xxoy4	Automation_ox7qu	CRUDO CAMPO MAMBO	CRUDO CAMPO MAMBO	2450.00		Bbl		2020-06-02	Error 500
--1				Movement	Automation_mptiw	Automation_9197y	CRUDO CAMPO CUSUCO	CRUDO CAMPO MAMBO	2345.82		Bbl		2020-05-25	Error 500
--1				Movement	Automation_mptiw	Automation_9197y	CRUDO CAMPO CUSUCO	CRUDO CAMPO MAMBO	2345.82		Bbl		2020-05-25	Error 501
--DEFECT 531521	Inventory	Automation_j8vyg	NULL				CRUDO CAMPO CUSUCO	NULL				-3000.00	Bbl		2020-06-07	Error 500
--DEFECT 325951	Inventory	Automation_uhwma	NULL				CRUDO CAMPO MAMBO	NULL				88317.00	Bbl		2020-05-30	Error 500
--DEFECT 164594	Inventory	Automation_2mbpe	NULL				CRUDO CAMPO MAMBO	NULL				88317.00	Bbl		2020-05-30	Error 501