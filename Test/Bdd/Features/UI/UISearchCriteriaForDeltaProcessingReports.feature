@owner=jagudelos @ui @MVP2and3 @S15 @testsuite=61556 @testplan=61542  @S16
Feature: UISearchCriteriaForDeltaProcessingReports
In order to Access delta reports
As ADMINISTRADOR/Consulta/USUARIO DE CADENA user
I want search criteria UI in TRUE

@testcase=68092 @version2 @BVT2
Scenario Outline: Verify that Official balance per node page should present for administrator, consulta and chain user role under Gestión cadena de suministro menu
	Given I am logged in as "<User>"
	When I navigate to "Official balance per node" page

	Examples:
		| User       |
		| admin      |
		| consulta   |
		| chain User |

@testcase=68093 @version2
Scenario Outline: Verify that Balance oficial inicial cargado page should present for administrator, consulta and chain user role under Gestión cadena de suministro menu
	Given I am logged in as "<User>"
	When I navigate to "Official initial balance charged" page

	Examples:
		| User       |
		| admin      |
		| consulta   |
		| chain User |

@testcase=68094 @version2
Scenario Outline: Verify that PENDIENTES OFICIALES DE PERÍODOS ANTERIORES should present for administrator, consulta and chain user role under Gestión cadena de suministro menu
	Given I am logged in as "<User>"
	When I navigate to "Pendientes oficiales de períodos anteriores" page

	Examples:
		| User       |
		| admin      |
		| consulta   |
		| chain User |

@testcase=68095 @version2 @BVT2
Scenario Outline: Verify that below pages are not available for specified users
	Given I am logged in as "<User>"
	Then I should not see "Page"
		| Page                                        |
		| Balance oficial por nodo                    |
		| Balance oficial inicial cargado             |
		| Pendientes oficiales de períodos anteriores |

	Examples:
		| User        |
		| aprobador   |
		| profesional |
		| programador |
		| audit       |

@testcase=68096
Scenario: Verify breadcrumb for Official balance per node page
	Given I am logged in as "admin"
	When I navigate to "Official balance per node" page
	Then I should see breadcrumb "Balance oficial por nodo"

@testcase=68097
Scenario: Verify breadcrumb for Balance oficial inicial cargado page
	Given I am logged in as "admin"
	When I navigate to "Official initial balance charged" page
	Then I should see breadcrumb "Balance oficial inicial cargado"

@testcase=68098
Scenario: Verify breadcrumb for Pendientes oficiales de períodos anteriores page
	Given I am logged in as "admin"
	When I navigate to "Official pending from previous periods" page
	Then I should see breadcrumb "Pendientes oficiales de períodos anteriores"

@testcase=68099 @version=3
Scenario: Verify UI controls validations on Official balance per node page
	Given I am logged in as "admin"
	When I navigate to "Official balance per node" page
	When I select first item from Segment dropdown by keyingin
	When I select first item from Node dropdown by keyingin "Data"
	And I click on filter year
	And I select first item from Period dropdown
	And I click on "nodeFilter" "viewReport" "button"
	Then I should see sucessful report Generation "Balance Oficial por Nodo"

@testcase=68100 @version=2
Scenario: Verify Periodo fields values formats on "Official balance per node" page
	Given I am logged in as "admin"
	When I navigate to "Official balance per node" page
	When I select first item from Segment dropdown by keyingin
	When I select first item from Node dropdown by keyingin "Automation"
	And I click on filter year
	And I select first item from Period dropdown
	Then Format of the period should be XXX-XX

@testcase=68101 @version=2
Scenario: Verify when Periodo field does not have values on "Official balance per node" page
	Given I am logged in as "admin"
	When I navigate to "Official balance per node" page
	When I select first item from Not in Segment dropdown by keyingin
	Then validation message "No se puede continuar:No se encontró información de deltas calculados para el segmento y año elegido. Por favor, primero ejecute el cálculo de deltas."
	Then Ver Reporte button should be disabled

@testcase=68102
Scenario: Verify mandatory fields on "Official balance per node" page
	Given I am logged in as "admin"
	When I navigate to "Official balance per node" page
	And I click on "nodeFilter" "viewReport" "button"
	Then I should see error message "Requerido"
	Then I should see error message "Requerido"

@testcase=68103 @version=2
Scenario: Verify UI controls validations on Balance oficial inicial cargado page
	Given I am logged in as "admin"
	When I navigate to "Official initial balance charged" page
	When I select first item from Segment dropdown by keyingin
	When I select first item from Node dropdown by keyingin "Data"
	And I click on filter year
	And I select first item from Period dropdown
	And I click on "nodeFilter" "viewReport" "button"
	Then I should see sucessful report Generation "Balance Oficial Inicial Cargado"

@testcase=68104 @version=2
Scenario: Verify Periodo fields values formats on "Balance oficial inicial cargado" page
	Given I am logged in as "admin"
	When I navigate to "Official initial balance charged" page
	When I select first item from Segment dropdown by keyingin
	When I select first item from Node dropdown by keyingin "Automation"
	And I click on filter year
	And I select first item from Period dropdown
	Then Format of the period should be XXX-XX

@testcase=68105 @version=2
Scenario: Verify when Periodo field does not have values on "Balance oficial inicial cargado" page
	Given I am logged in as "admin"
	When I navigate to "Official initial balance charged" page
	When I select first item from Not in Segment dropdown by keyingin
	Then validation message "No se puede continuar:No se encontró información oficial para el segmento y año elegido. Por favor, primero cargue la información oficial."
	Then Ver Reporte button should be disabled

@testcase=68106  @version=2
Scenario: Verify mandatory fields on "Balance oficial inicial cargado" page
	Given I am logged in as "admin"
	When I navigate to "Official initial balance charged" page
	And I click on "nodeFilter" "viewReport" "button"
	Then I should see error message "Requerido"
	Then I should see error message "Requerido"

@testcase=68107  @version=2 @BVT2
Scenario: Verify UI controls validations on Pendientes oficiales de períodos anteriores page
	Given I am logged in as "admin"
	When I navigate to "Official pending from previous periods" page
	When I select first item from Segment dropdown by keyingin
	And I click on "nodeFilter" "viewReport" "button"
	Then I should see sucessful report Generation "Pendientes Oficiales de Períodos Anteriores"