/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Feb-14-2020

-- Description:     These test cases are for View [Admin].[ReportTemplateDetails]

-- Database backup Used:	appdb_0214
-- ==============================================================================================================================*/

/*
The below insert statements are required to populate some data in [Admin].[ReportTemplateConfiguration] table.

*/


--===================================  DATA FOR [Admin].[ReportTemplateConfiguration] ===========================================================================================================================================================================

INSERT INTO [Admin].[ReportTemplateConfiguration]([ReportIdentifier],[Area],[InformationResponsible],[Frequency],[InformationSource],[Datamart],[Version],[UpdateDate],[ChangeResponsible],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES('ID1','Area1','Author1','Daily' ,'Source1' ,'True','1.5','2/20/2020  5:08:37 AM','Change1' ,'System','2/12/2020  5:08:37 AM' ,NULL ,NULL)

INSERT INTO [Admin].[ReportTemplateConfiguration]([ReportIdentifier],[Area],[InformationResponsible],[Frequency],[InformationSource],[Datamart],[Version],[UpdateDate],[ChangeResponsible],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES('ID2','Area2','Author2','Daily' ,'Source2' ,'True','2.0','2/20/2020  5:08:37 AM','Change2' ,'System','2/12/2020  5:08:37 AM' ,NULL ,NULL)

INSERT INTO [Admin].[ReportTemplateConfiguration]([ReportIdentifier],[Area],[InformationResponsible],[Frequency],[InformationSource],[Datamart],[Version],[UpdateDate],[ChangeResponsible],[CreatedBy],[CreatedDate],[LastModifiedBy],[LastModifiedDate])
VALUES('ID3','Area3','Author3','Daily' ,'Source3' ,'True','2.5','2/20/2020  5:08:37 AM','Change3' ,'System','2/12/2020  5:08:37 AM' ,NULL ,NULL)


--===================== TestCase1: To check if the Report template Details View returns data from the table for a particular Report ID that exists =============================================================================================================

SELECT * FROM [Admin].[ReportTemplateDetails]
WHERE ReportIdentifier = 'ID2'

--========================================================== Output Captured ===================================================================================================================================================================================
ReportIdentifier	Area	InformationResponsible	Frequency	InformationSource	Datamart	Version	UpdateDate	ChangeResponsible
ID2	Area2	Author2	Daily	Source2	True	2.0	Feb 20 2020  5:08AM	Change2


--===================== TestCase2: To check if the Report template Details View returns blank data from the table for a particular Report ID that does not exist ===============================================================================================

SELECT * FROM [Admin].[ReportTemplateDetails]
WHERE ReportIdentifier = 'ID4'


--========================================================== Output Captured ==================================================================================================================================================================================
ReportIdentifier	Area	InformationResponsible	Frequency	InformationSource	Datamart	Version	UpdateDate	ChangeResponsible



--##############################################################################################################################################################################################################################################################