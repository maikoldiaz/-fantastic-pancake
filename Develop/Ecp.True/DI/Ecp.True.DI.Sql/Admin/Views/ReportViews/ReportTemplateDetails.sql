-- ================================================================================================================
-- Author: Microsoft
-- Created date: Feb-14-2020
-- Updated date: Mar-20-2020
-- <Description>: This View is to Fetch Data for the Report Template details from the ReportTemplateConfiguration table </Description>:
-- ================================================================================================================

CREATE VIEW [Admin].[ReportTemplateDetails]
AS
SELECT DISTINCT		ISNULL(ReportIdentifier,'')						AS ReportIdentifier,
					ISNULL(Area,'')									AS Area,
					ISNULL(InformationResponsible,'')				AS InformationResponsible,
					ISNULL(Frequency,'')							AS Frequency,
					ISNULL(InformationSource,'')					AS InformationSource,
					ISNULL(Datamart,'')								AS Datamart,
					ISNULL(Version,'')								AS Version,
					CAST(ISNULL(UpdateDate,'') AS VARCHAR(25))		AS UpdateDate,
					ISNULL(ChangeResponsible,'')					AS ChangeResponsible
FROM [Admin].[ReportTemplateConfiguration]

UNION
 
SELECT				''												AS ReportIdentifier,
					''												AS Area,
                    ''												AS InformationResponsible, 
                    ''												AS Frequency,
                    ''												AS InformationSource,
                    ''												AS Datamart,
                    ''												AS Version,
                    ''												AS UpdateDate,
                    ''												AS ChangeResponsible
FROM (SELECT 1 AS ID)A            
WHERE NOT EXISTS
(      
	SELECT	DISTINCT	ISNULL(ReportIdentifier,'')					AS ReportIdentifier,
						ISNULL(Area,'')								AS Area,
						ISNULL(InformationResponsible,'')			AS InformationResponsible,
						ISNULL(Frequency,'')						AS Frequency,
						ISNULL(InformationSource,'')				AS InformationSource,
						ISNULL(Datamart,'')							AS Datamart,
						ISNULL(Version,'')							AS Version,
						CAST(ISNULL(UpdateDate,'') AS VARCHAR(25))  AS UpdateDate,
						ISNULL(ChangeResponsible,'')				AS ChangeResponsible
    FROM [Admin].[ReportTemplateConfiguration]
)

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data for the Report Template details from the ReportTemplateConfiguration table',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'ReportTemplateDetails',
    @level2type = NULL,
    @level2name = NULL