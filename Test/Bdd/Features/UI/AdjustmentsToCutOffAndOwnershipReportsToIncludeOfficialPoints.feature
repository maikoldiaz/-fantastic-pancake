@sharedsteps=16534 @owner=jagudelos @ui @testplan=55104 @testsuite=55117
Feature: AdjustmentsToCutOffAndOwnershipReportsToIncludeOfficialPoints
As a query user, I need to adjust reports (with operational cut-off
and with ownership) to include information of official points

Background: Login
Given I am logged in as "consulta"

@testcase=56845 @version=2
Scenario Outline: Verify backup movementid and global movementid columns in movements detail page of operational cutoff report
Given I have "<Element>" for the operational cutoff report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category" "combobox"
And I select "<Element>" from "Element" "combobox"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select "Con corte operativo" option
And I select startdate on "startdate" datepicker
And I select finaldate on "finaldate" datepicker
And I click on "ReportFilter" "ViewReport" "button"
And I click on "Movements detail" page
Then validate "backup movementid" column details
And validate "global movementid" column details

Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=56846 @version=2
Scenario Outline: Verify backup movementid and global movementid columns in movements detail page of ownership report
Given I have "<Element>" for the ownership report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category" "combobox"
And I select "<Element>" from "Element" "combobox"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select "Con propiedad" option
And I select startdate on "startdate" datepicker
And I select finaldate on "finaldate" datepicker
And I click on "ReportFilter" "ViewReport" "button"
And I click on "Movements detail" page
Then validate "backup movementid" column details
And validate "global movementid" column details

Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=56847 @version=2
Scenario Outline: Verify empty values should be displayed insted of null values in backup movementid and global movementid columns in movements detail page of operational cutoff report
Given I have "<Element>" for the operational cutoff report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category" "combobox"
And I select "<Element>" from "Element" "combobox"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select "Con corte operativo" option
And I select startdate on "startdate" datepicker
And I select finaldate on "finaldate" datepicker
And I click on "ReportFilter" "ViewReport" "button"
And I click on "Movements detail" page
Then empty values should be displayed insted of null values in "backup movementid" column
And empty values should be displayed insted of null values in "global movementid" column

Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=56848 @version=2
Scenario Outline: Verify empty values should be displayed insted of null values in backup movementid and global movementid columns in movements detail page of ownership report
Given I have "<Element>" for the ownership report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category" "combobox"
And I select "<Element>" from "Element" "combobox"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select "Con propiedad" option
And I select startdate on "startdate" datepicker
And I select finaldate on "finaldate" datepicker
And I click on "ReportFilter" "ViewReport" "button"
And I click on "Movements detail" page
Then empty values should be displayed insted of null values in "backup movementid" column
And empty values should be displayed insted of null values in "global movementid" column

Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=56849
Scenario Outline: Verify column details in backup movements page of operational cutoff report
Given I have "<Element>" for the operational cutoff report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category" "combobox"
And I select "<Element>" from "Element" "combobox"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select "Con corte operativo" option
And I select startdate on "startdate" datepicker
And I select finaldate on "finaldate" datepicker
And I click on "ReportFilter" "ViewReport" "button"
And I should see "Backup movements details" page after "Detail Attributes" sheet
And I click on "Backup movements details" page
Then backup movements page should contain "Columns"
| Columns                |
| Id movimiento          |
| Id batch               |
| Fecha                  |
| Tipo movimiento        |
| Nodo origen            |
| Nodo destino           |
| Producto origen        |
| Producto destino       |
| Cantidad neta          |
| Cantidad bruta         |
| Unidad                 |
| Acción                 |
| Origen                 |
| Id movimiento respaldo |
| Id movimiento global   |

Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=56850
Scenario Outline: Verify column details in backup movements page of ownership report
Given I have "<Element>" for the ownership report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category" "combobox"
And I select "<Element>" from "Element" "combobox"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select "Con propiedad" option
And I select startdate on "startdate" datepicker
And I select finaldate on "finaldate" datepicker
And I click on "ReportFilter" "ViewReport" "button"
And I should see "Backup movements details" page after "Detail Attributes" sheet
And I click on "Backup movements details" page
Then backup movements page should contain "Columns"
| Columns                |
| Id movimiento          |
| Id batch               |
| Fecha                  |
| Tipo movimiento        |
| Nodo origen            |
| Nodo destino           |
| Producto origen        |
| Producto destino       |
| Cantidad neta          |
| Cantidad bruta         |
| Unidad                 |
| Origen                 |
| Id movimiento respaldo |
| Id movimiento global   |

Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=56851
Scenario Outline: Verify details page of backup movements to operational cutoff report
Given I have "<Element>" for the operational cutoff report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category" "combobox"
And I select "<Element>" from "Element" "combobox"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select "Con corte operativo" option
And I select startdate on "startdate" datepicker
And I select finaldate on "finaldate" datepicker
And I click on "ReportFilter" "ViewReport" "button"
And I should see "Backup movements details" page after "Detail Attributes" sheet
And I click on "Backup movements details" page
Then display all the movements whose identifiers correspond to those entered in the column "Backup movement Id" of the sheet "Detail Movements"
And data must be sorted by "Id movimiento" and "Fecha" columns in ascending order

Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=56852 
Scenario Outline: Verify details page of backup movements to ownership report
Given I have "<Element>" for the ownership report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category" "combobox"
And I select "<Element>" from "Element" "combobox"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select "Con propiedad" option
And I select startdate on "startdate" datepicker
And I select finaldate on "finaldate" datepicker
And I click on "ReportFilter" "ViewReport" "button"
And I should see "Backup movements details" page after "Detail Attributes" sheet
And I click on "Backup movements details" page
Then display all the movements whose identifiers correspond to those entered in the column "Backup movement Id" of the sheet "Detail Movements"
And data must be sorted by "Id movimiento" and "Fecha" columns in ascending order

Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |
