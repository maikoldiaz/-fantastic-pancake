@owner=jagudelos @manual @ui @testsuite=55120 @testplan=55104
Feature: AdjustReportsToIncludeNewColumnsAndUpdateSorting
In order to include new columns and update the sorting
As a query user
I need to adjust the transport segment reports
@testcase=57660
Scenario: Validate owner details of movement in operational report
Given I have ownership calculation data generated in the system
When I navigate to "Balance Operational Report" page
And I enter the required fields with "Operational Report" option
And I click on "ViewReport" "button"
Then validate tab "Propietarios Movimientos" in the report
And I click on "Propietarios Movimiento" "tab"
And validate the following columns in "Propietarios Movimiento"
| columns              |
| Id movimiento        |
| Id batch             |
| Fecha                |
| Tipo movimiento      |
| Nodo origen          |
| Nodo destino         |
| Producto origen      |
| Producto destino     |
| Cantidad neta        |
| Cantidad bruta       |
| Unidad               |
| Acción               |
| Origen               |
| Propietario          |
| Volumen propiedad    |
| Porcentaje propiedad |
@testcase=57661
Scenario: Validate owner details of inventory in operational report
GGiven I have ownership calculation data generated in the system
When I navigate to "Balance Operational Report" page
And I enter the required fields with "Operational Report" option
And I click on "ViewReport" "button"
Then validate tab "Propietarios Inventarios" in the report
And I click on "Propietarios Inventarios" "tab"
And validate the following columns in "Propietarios Inventarios"
| columns              |
| Id inventario        |
| Fecha                |
| Nodo                 |
| Tanque               |
| Id batch             |
| Producto             |
| Cantidad neta        |
| Cantidad bruta       |
| Unidad               |
| Acción               |
| Origen               |
| Propietario          |
| Volumen propiedad    |
| Porcentaje propiedad |
@testcase=57662
Scenario Outline: Validate the renamed columns in reports
Given I have ownership calculation data generated in the system
When I navigate to "Balance Operational Report" page
And I enter the required fields with "Operational Report" option
And I click on "ViewReport" "button"
Then I click on "Detalle Movimientos" "tab"
And validate "<OlderFieldName>" changed to "<NewerFieldName>"
And I click on "Calidad Movimientos" "tab"
And validate "<OlderFieldName>" changed to "<NewerFieldName>"
And I click on "Detalle de inventarios" "tab"
And validate "<OlderFieldName>" changed to "<NewerFieldName>"
And I click on "Calidad Inventarios" "tab"
And validate "<OlderFieldName>" changed to "<NewerFieldName>"

Examples:
| OlderFieldName | NewerFieldName | Report                         |
| Volumen neto   | Cantidad neta  | Operational Report             |
| Volumen bruto  | Cantidad bruta | Operational Report             |
| Volumen neto   | Cantidad neta  | Report With Operational Cutoff |
| Volumen bruto  | Cantidad bruta | Report With Operational Cutoff |
| Volumen neto   | Cantidad neta  | Report With Ownership          |
| Volumen bruto  | Cantidad bruta | Report With Ownership          |
@testcase=57663
Scenario Outline: Validate column Cantidad bruta in reports
Given I have ownership calculation data generated in the system
When I navigate to "Balance Operational Report" page
And I enter the required fields with "<Report> option
And I click on "ViewReport" "button"
Then I click on "Detalle de inventarios" "tab"
And validate "Cantidad bruta" added after "Cantidad neta" in "Detalle de inventarios" tab
And I click on "Calidad Inventarios" "tab"
And validate "Cantidad bruta" added after "Cantidad neta" in "Calidad Inventarios" tab

Examples:
| Report                         |
| Operational Report             |
| Report With Operational Cutoff |
| Report With Ownership          |
@testcase=57664
Scenario Outline: Validate column Id atributo in reports
Given I have ownership calculation data generated in the system
When I navigate to "Balance Operational Report" page
And I enter the required fields with "<Report>" option
And I click on "ViewReport" "button"
Then I click on "Calidad Atributos" "tab"
And validate "Id atributo" added before "Valor atributo" in "Calidad Atributos" tab
And I click on "Calidad inventarios" "tab"
And validate "Id atributo" added before "Valor atributo" in "Calidad inventarios" tab

Examples:
| Report                         |
| Operational Report             |
| Report With Operational Cutoff |
| Report With Ownership          |
@testcase=57665
Scenario Outline: Validate column Id batch in reports
Given I have ownership calculation data generated in the system
When I navigate to "Balance Operational Report" page
And I enter the required fields with "<Report>" option
And I click on "ViewReport" "button"
Then I click on "Detalle de inventarios" "tab"
And validate "Id batch" added after "Id movimiento" in "Detalle de inventarios" tab
And I click on "Detalle Atributos" "tab"
And validate "Id batch" added after "Id movimiento" in "Detalle Atributos" tab

Examples:
| Report                         |
| Operational Report             |
| Report With Operational Cutoff |
| Report With Ownership          |
@testcase=57666
Scenario Outline: Validate movements ordering in the reports
Given I have ownership calculation data generated in the system
When I navigate to "Balance Operational Report" page
And I enter the required fields with "<Report>" option
And I click on "ViewReport" "button"
Then I click on "Detalle Movimientos" "tab"
And validate "Detalle Movimientos" sorted with ascending by "Id movimiento" and "Fecha" column
And I click on "Calidad Movimientos" "tab"
And validate "Calidad Movimientos" sorted with ascending by "Id movimiento" and "Fecha" column

Examples:
| Report                         |
| Operational Report             |
| Report With Operational Cutoff |
| Report With Ownership          |
@testcase=57667
Scenario Outline: Validate inventories ordering in the reports
Given I have ownership calculation data generated in the system
When I navigate to "Balance Operational Report" page
And I enter the required fields with "<Report>" option
And I click on "ViewReport" "button"
Then I click on "Detalle inventarios" "tab"
And validate "Detalle inventarios" sorted with ascending by "Id Inventario" and "Fecha" column
And I click on "Calidad Inventarios" "tab"
And validate "Calidad Inventarios" sorted with ascending by "Id Inventario" and "Fecha" column

Examples:
| Report                         |
| Operational Report             |
| Report With Operational Cutoff |
| Report With Ownership          |
@testcase=57668
Scenario Outline: Validate data source of Origen in reports
Given I have ownership calculation data generated in the system
When I navigate to "Balance Operational Report" page
And I enter the required fields with "<Report>" option
And I click on "ViewReport" "button"
Then I click on "Detalle Movimientos" "tab"
And validate data source taken from respective category element for "Origen" in "Detalle Movimientos" "tab"
And I click on "Detalle Atributos" "tab"
And validate data source taken from respective category element for "Origen" in "Detalle Atributos" "tab"
And I click on "Detalle de inventarios" "tab"
And validate data source taken from respective category element for "Origen" in "Detalle de inventarios" "tab"
And I click on "Detalle de calidad de los inventarios" "tab"
And validate data source taken from respective category element for "Origen" in "Detalle Atributos" "tab"

Examples:
| Report                         |
| Operational Report             |
| Report With Operational Cutoff |
| Report With Ownership          |
@testcase=57669
Scenario Outline: Validate data source of Unidad atributo in reports
Given I have ownership calculation data generated in the system
When I navigate to "Balance Operational Report" page
And I enter the required fields with "<Report>" option
And I click on "ViewReport" "button"
Then I click on "Detalle Atributos" "tab"
And validate data source taken from respective category element for "Unidad atributo" in "Detalle Atributos"
And I click on "Detalle de calidad de los inventarios" "tab"
And validate data source taken from respective category element for "Unidad atributo" in "Detalle de calidad de los inventarios" "tab"

Examples:
| Report                         |
| Operational Report             |
| Report With Operational Cutoff |
| Report With Ownership          |
@testcase=57670 
Scenario Outline: Validate Operación renamed to Tipo movimiento in reports
Given I have ownership calculation data generated in the system
When I navigate to "Balance Operational Report" page
And I enter the required fields with "<Report>" option
And I click on "ViewReport" "button"
Then I click on "Detalle Movimientos" "tab"
And validate "Operación" changed to "Tipo movimiento"
And I click on "Detalle Atributos" "tab"
And validate "Operación" changed to "Tipo movimiento"

Examples:
| Report                         |
| Operational Report             |
| Report With Operational Cutoff |
| Report With Ownership          |
