@sharedsteps=4013 @owner=jagudelos @ui @testplan=24148 @testsuite=24162 @parallel=false
Feature: AdjustmentsToOperationalCutoff
As TRUE system, I need to include general adjustments
in the operational cutoff to improve the process

Background: Login
Given  I am logged in as "admin"

@testcase=25121 @version=2 @bvt1.5
Scenario: Verify date of the last operational cutoff is displayed in upper right corner of each page
Given I have a segment where operational cutoff is already calculated
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
Then I should see "Start" "link"
And I should see final date of the last operational cutoff executed and the execution date
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
When I click on "InitTicket" "submit" "button"
Then I should see final date of the last operational cutoff executed and the execution date
When I select all pending records from grid
And I click on "Notes" "button"
Then I should see "Agregar Nota Functions" interface
When I enter valid value into "Agregar Nota" "textbox"
And I click on "AddComment" "submit" "button"
Then I should see "ErrorsGrid" "Submit" "button" as enabled
When I click on "ErrorsGrid" "Submit" "button"
Then I should see final date of the last operational cutoff executed and the execution date

@testcase=25122
Scenario: Verify column names in the Check consistency page of Operational Cutoff
Given I have a segment where operational cutoff is already calculated
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I select all pending records from grid
And I click on "Notes" "button"
Then I should see "Agregar Nota Functions" interface
When I enter valid value into "Agregar Nota" "textbox"
And I click on "AddComment" "submit" "button"
Then I should see "ErrorsGrid" "Submit" "button" as enabled
When I click on "ErrorsGrid" "Submit" "button"
Then I should see "CheckConsistency" "link"
And I should see the "Columns" on the page
| Columns             |
| Nodo                |
| Producto            |
| Vol. Control        |
| Unidad              |
| % Control           |
| % Control Aceptable |


@testcase=25123
Scenario: Verify %control column when input value of a product is equal to zero
Given I have a product with input value is zero
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I select all pending records from grid
And I click on "Notes" "button"
Then I should see "Agregar Nota Functions" interface
When I enter valid value into "Agregar Nota" "textbox"
And I click on "AddComment" "submit" "button"
Then I should see "ErrorsGrid" "Submit" "button" as enabled
When I click on "ErrorsGrid" "Submit" "button"
Then I should see "CheckConsistency" "link"
And I should see "Error" in the %Control column for that product

@testcase=25124
Scenario: Verify error message when movement with the empty unit field is processed in Excel
Given I have movement with empty unit field is processed in Excel
Then Then it must be stored in a Pendingtransactions repository with validation "La unidad es obligatoria"

@testcase=25125
Scenario: Verify error message when movement with the empty unit field is processed in Sinoper
Given I have movement with empty unit field is processed in Sinoper
Then Then it must be stored in a Pendingtransactions repository with validation "La unidad es obligatoria"

@testcase=25126
Scenario: Verify error message when inventory with the empty unit field is processed in Excel
Given I have inventory with empty unit field is processed in Excel
Then Then it must be stored in a Pendingtransactions repository with validation "La unidad es obligatoria"

@testcase=25127 
Scenario: Verify error message when inventory with the empty unit field is processed in Sinoper
Given I have inventory with empty unit field is processed in Sinoper
Then Then it must be stored in a Pendingtransactions repository with validation "La unidad es obligatoria"