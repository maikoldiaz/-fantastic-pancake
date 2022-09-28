@sharedsteps=16534 @owner=jagudelos @ui @testplan=31102 @testsuite=31108
Feature: ConfigureTemplateDataForEachReport
As a query user, I require that report template data
to be obtained from a configuration table to that this
information is centralized


Background: Login
Given I am logged in as "consulta"

@testcase=32743 @manual
Scenario Outline: Verify the report has the template data configured
	And the "<Report>" has the template data configured
	When pbix file is downloaded for "<Report>"
	And it is opened in power bi desktop
	Then area field on the cover page should be displayed as "<Area>" next to the title "Nombre del área que genera el reporte"
	And infromation responsible field on the cover page should be displayed as "<Information Responsible>" next to the title "Responsable por la información publicada"
	And frequency field on the cover page should be displayed as "<Frequency>" next to the title "Frecuencia de Actualización"
	And information source field on the cover page should be displayed as "<Information Source>" next to the title "Fuentes Principales de Información"
	And datamart field on confidentiality agreement page should be displayed as "<Datamart>" next to the word "Datamart"
	And version field on log page should be displayed as "<Version>" next to the title "Versión"
	And update date on log page should be displayed as "<Update date>" field next to the title "Fecha Actualización Informe"
	And change responsible on log page should be displayed as "<Change Responsible>" next to the title "Responsable del Cambio"
	And verify report identifier for each "<Report>" in the database
	
	Examples:
	| Report                                                                       | Area                                                               | Information Responsible                                           | Frequency      | Information Source                                                                                                                  | Datamart | Version | Update date | Change Responsible   |
	| Operational report without operational cutoff by node or segment or system   | Coordinación de Medición y Balance en Sistemas de Transporte - CMB | Coordinador de Medición y Balance en Sistemas de Transporte - CMB | Diaria         | SINOPER, Información operativa de los transportadores                                                                               | TRUE     | 1.0     | Feb-20      | Líder Funcional TRUE |
	| Operating balance report without ownership by node or segment or system      | Coordinación de Medición y Balance en Sistemas de Transporte - CMB | Coordinador de Medición y Balance en Sistemas de Transporte - CMB | Diaria         | SINOPER, Información operativa de los transportadores                                                                               | TRUE     | 1.0     | Feb-20      | Líder Funcional TRUE |
	| Operating balance report with ownership by node by node or segment or system | Coordinación de Medición y Balance en Sistemas de Transporte - CMB | Coordinador de Medición y Balance en Sistemas de Transporte - CMB | Diaria         | SINOPER, Información operativa de los transportadores, premisas programación, ACE, estrategías de Propiedad, Premisas contractuales | TRUE     | 1.0     | Feb-20      | Líder Funcional TRUE |
	| Results of the analytical model                                              | Coordinación de Medición y Balance en Sistemas de Transporte - CMB | Coordinador de Medición y Balance en Sistemas de Transporte - CMB | Mensual        | SIV, Información operativa de los segmentos de producción, comercialización y de transporte - Ductos y Estaciones                   | TRUE     | 1.0     | Feb-20      | Líder Funcional TRUE |

@testcase=32744 @manual
Scenario Outline: Verify the report does not have configured the template data
	And the "<Report>" does not have configured the template data
	When pbix file is downloaded for "<Report>"
	And it is opened in power bi desktop
	Then area field on the cover page should be displayed only title as "Nombre del área que genera el reporte"
	And infromation responsible field on the cover page should be displayed only title as "Responsable por la información publicada"
	And frequency field on the cover page should be displayed only title as "Frecuencia de Actualización"
	And information source field on the cover page should be displayed only title as "Fuentes Principales de Información"
	And datamart field on confidentiality agreement page should be displayed only title as "Datamart"
	And version field on log page should be displayed only title as "Versión"
	And update date on log page should be displayed only title as "Fecha Actualización Informe"
	And change responsible on log page should be displayed only title as "Responsable del Cambio"
	
	Examples:
	| Report                                                                       |
	| Operational report without operational cutoff by node or segment or system   |
	| Operating balance report without ownership by node or segment or system      |
	| Operating balance report with ownership by node by node or segment or system |
	| Results of the analytical model                                              |