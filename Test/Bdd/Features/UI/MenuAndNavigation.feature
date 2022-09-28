@owner=jagudelos @sharedsteps=4013 @ui  @testsuite=19774 @testplan=19772
Feature: MenuAndNavigation
In order to navigation and access the Menu
As a User
I want UI in the website

Background: Login
	Given I am logged in as "admin"

@testcase=21254 @bvt @prodready
Scenario: Verify the navigation links
	When I click on "Menu" toggler
	Then I should see the below "Menu" Options
		| Menu                                  |
		| Administración                        |
		| Balance transportadores con propiedad |
		| Gestión cadena de suministro          |

@testcase=21255 @bvt @prodready
Scenario: Verify the sub menu options under administration link
	When I click on "Menu" toggler
	And I click on "Administración" link
	Then I should see the below "SubMenu" Options
		| SubMenu                         |
		| Categoría                       |
		| Elementos de categorías         |
		| Nodos                           |
		| Configurar atributos nodos      |
		| Configurar agrupar nodos        |
		| Configurar atributos conexiones |
		| Homologaciones                  |
		| Excepciones                     |

@testcase=21256 @prodready
Scenario: Verify the sub menu options under Balance Transport link
	When I click on "Menu" toggler
	And I click on "Balance transportadores con propiedad" link
	Then I should see the below "SubMenu" Options
		| SubMenu                                               |
		| Configuración de transformaciones                     |
		| Cargue de movimientos e inventarios                   |
		| Cargue Otros Registros                                |
		| Corte Operativo                                       |
		| Balance volumétrico con propiedad por segmento ductos |
		| Balance volumétrico con Propiedad por Nodo            |
		| Reporte balance operativo con o sin propiedad         |
		| Generación reporte de movimientos logísticos          |

@testcase=21257 @prodready
Scenario: Verify the sub menu options under Supply Chain Management link
	When I click on "Menu" toggler
	And I click on "Gestión cadena de suministro" link
	Then I should see the "Cargue otros registros" option

@testcase=21258 @prodready
Scenario: : Verify the logout button
	When I click on "user" "name" "label"
	Then I should see "user" "logout" "link"