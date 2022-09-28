@owner=jagudelos @ui @MVP2and3 @testsuite=70816 @testplan=70526 @S16 @manual
Feature: ReportsOnFlyCalculationsInAsynchronousWay
In order to improve the user experience
As a system architect
I need that reports doing calculations on the fly be processed in an asynchronous way

@testcase=71737 @version=2
Scenario Outline: Validate report generating which is not in the generated list
	Given I am logged in as "admin"
	When I navigate to "<Page>" page
	And I have "<Data>" generated in the system
	And I enter required filter values for "<Report>" which is not in the generated list
	And I click on "View Report" "button"
	Then I should see a pop up  with message "La solicitud de su reporte fue recibida y estará disponible en la opción de menú "Reportes generados"."
	And validate that "Go to generated reports" "button" is enabled
	And validate that "To Close" "button" is enabled
	And I click on "Go to generated reports" "button"
	And validate that "<Navigation>" is navigated
	And validate generated report in "ReportExecutions" "grid"
	And validate generated report "status" as "Finalizado" in "ReportExecutions" "grid"
	And validate generated report has "ReportExecutions" "Download" "link" enabled
	And validate generated report has "ReportExecutions" "View" "link" disabled
	And I click on "ReportExecutions" "Download" "link"
	And validate report is opened with filter values requested

	Examples:
		| Page                                                         | Report                                     | Navigation                                         | Data                               |
		| Reportes de ductos y estaciones - Balance Operational Report | Balance Operational Report - Before Cutoff | Reportes de ductos y estaciones - Report Generator | movements and inventories          |
		| Initial Official Balance Loaded                              | Initial Official Balance Loaded            | Gestión cadena de suministro - Report Generator    | official movements and inventories |
		| Gestión cadena de suministro - Balance Operational Report    | Balance Operational Report - Before Cutoff | Gestión cadena de suministro - Report Generator    | movements and inventories          |
		| Gestión cadena de suministro - Balance Operational Report    | Balance Operational Report - Ownership     | Gestión cadena de suministro - Report Generator    | movements and inventories          |

@testcase=71738 @version=2
Scenario Outline: Validate report generating which is existing in the generated list
	Given I am logged in as "admin"
	When I navigate to "<Page>" page
	And I have "<Data>" generated in the system
	And I enter required filter values for "<Report>" which is existing in the generated list
	And I click on "View Report" "button"
	Then I should see a pop up  with message "Otro usuario ya solicitó el reporte con los mismos filtros y está disponible en la opción de menú "Reportes generados"."
	And validate that "Go to generated reports" "button" is enabled
	And validate that "To Close" "button" is enabled
	And I click on "Go to generated reports" "button"
	And validate that "<Navigation>" is navigated
	And validate requested report in "ReportExecutions" "grid"
	And validate requested report "status" as "Finalizado" in "ReportExecutions" "grid"
	And validate requested report has "ReportExecutions" "Download" "link" enabled
	And validate requested report has "ReportExecutions" "View" "link" disabled
	And I click on "ReportExecutions" "Download" "link"
	And validate report is opened with filter values requested

	Examples:
		| Page                                                         | Report                                     | Navigation                                         | Data                               |
		| Reportes de ductos y estaciones - Balance Operational Report | Balance Operational Report - Before Cutoff | Reportes de ductos y estaciones - Report Generator | movements and inventories          |
		| Initial Official Balance Loaded                              | Initial Official Balance Loaded            | Gestión cadena de suministro - Report Generator    | official movements and inventories |
		| Gestión cadena de suministro - Balance Operational Report    | Balance Operational Report - Before Cutoff | Gestión cadena de suministro - Report Generator    | movements and inventories          |
		| Gestión cadena de suministro - Balance Operational Report    | Balance Operational Report - Ownership     | Gestión cadena de suministro - Report Generator    | movements and inventories          |

@testcase=71739 @version=2
Scenario Outline: Validate error message in report generation
	Given I am logged in as "admin"
	When I navigate to "<Page>" page
	And I have "<Data>" generated in the system
	And I disable the analytical service
	And I enter required filter values for "<Report>" which is not in the generated list
	And I click on "View Report" "button"
	Then I should see a pop up  with message "La solicitud de su reporte fue recibida y estará disponible en la opción de menú "Reportes generados"."
	And validate that "Go to generated reports" "button" is enabled
	And validate that "To Close" "button" is enabled
	And I click on "Go to generated reports" "button"
	And validate that "<Navigation>" is navigated
	And validate requested report in "ReportExecutions" "grid"
	And validate requested report "status" as "Fallido" in "ReportExecutions" "grid"
	And validate requested report has "ReportExecutions" "Download" "link" disabled
	And validate requested report has "ReportExecutions" "View" "link" enabled
	And I click on "ReportExecutions" "View" "link"
	And Validate the error message displayed

	Examples:
		| Page                                                         | Report                                     | Navigation                                         | Data                               |
		| Reportes de ductos y estaciones - Balance Operational Report | Balance Operational Report - Before Cutoff | Reportes de ductos y estaciones - Report Generator | movements and inventories          |
		| Initial Official Balance Loaded                              | Initial Official Balance Loaded            | Gestión cadena de suministro - Report Generator    | official movements and inventories |
		| Gestión cadena de suministro - Balance Operational Report    | Balance Operational Report - Before Cutoff | Gestión cadena de suministro - Report Generator    | movements and inventories          |
		| Gestión cadena de suministro - Balance Operational Report    | Balance Operational Report - Ownership     | Gestión cadena de suministro - Report Generator    | movements and inventories          |

@testcase=71740 @version=2
Scenario Outline: Validate report is viewed from report generation
	Given I am logged in as "admin"
	When I navigate to "<Page>" page
	And I have "<Report>" report generated in the system
	Then validate report related to "<Report>" that are already generated in "ReportExecutions" "grid"
	And validate the <columns> in "ReportExecutions" "grid"
		| columns         |
		| Reporte         |
		| Segmento        |
		| Sistema         |
		| Nodo            |
		| Fecha inicial   |
		| Fecha final     |
		| Fecha Ejecución |
		| Estado          |

	Examples:
		| Page                                               | Report                                     |
		| Reportes de ductos y estaciones - Report Generator | Balance Operational Report - Before Cutoff |
		| Gestión cadena de suministro - Report Generator    | Initial Official Balance Loaded            |
		| Gestión cadena de suministro - Report Generator    | Balance Operational Report - Before Cutoff |
		| Gestión cadena de suministro - Report Generator    | Balance Operational Report - Ownership     |

@testcase=71741 @version=2
Scenario: Validate reports in report generation are displayed where authenticated user has access
	Given I am logged in as "Programmer"
	When I navigate to "Gestión cadena de suministro - Report Generator" page
	And I have "Initial Official Balance Loaded", "Balance Official Per Node", "Balance Operational Report - Before Cutoff", "Balance Operational Report - Ownership", "Balance Operational Report - Before Cutoff" reports generated by "admin" user
	Then validate reports related to "Initial Official Balance Loaded" is not displayed in "ReportExecutions" "grid"
	And validate reports related to "Balance Operational Report - Before Cutoff", "Balance Operational Report - Ownership" are displayed in "ReportExecutions" "grid"
	And I navigate to "Reportes de ductos y estaciones - Report Generator" page
	And validate reports related to "Balance Operational Report - Before Cutoff" is displayed in "ReportExecutions" "grid"

@testcase=71742
Scenario: Validate cleaning up reports
	Given I am logged in as "admin"
	And I have all the reports generated more than Y hours older
	And I have all the reports generated within Y hours
	When the period to clean up the report time X is reached
	Then validate reports deleted that are more than Y hours older