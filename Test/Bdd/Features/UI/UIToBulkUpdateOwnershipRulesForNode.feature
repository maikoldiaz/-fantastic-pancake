@sharedsteps=4013 @owner=jagudelos @ui @testplan=36673 @testsuite=35689 @parallel=false
Feature: UIToBulkUpdateOwnershipRulesForNode
As an Administrator user, I need an UI to
bulk update ownership rules for node

Background: Login
Given I am logged in as "admin"

@testcase=37422 @version=2
Scenario: Verify Massive strategy change button in node configuration attributes page
When I navigate to "ConfigureAttributesNodes" page
And I click on "Change Ownership Rules" "dropdown"
Then I should see "Massive By Node" "link"

@testcase=37423 @version=2
Scenario: Verify Massive strategy change button functionality in node configuration attributes page
When I navigate to "ConfigureAttributesNodes" page
And I click on "Change Ownership Rules" "dropdown"
And I click on "Massive By Node" "link"
Then I see "Change massive strategies" header

@testcase=37424 @version=2
Scenario: Verify Column names in node configuration attributes page
When I navigate to "ConfigureAttributesNodes" page
Then I should see the "Columns" on the page
| Columns                 |
| Nodo                    |
| Límite de Control       |
| Balance Aceptable (%)   |
| Estrategia de propiedad |

@version=2 @bvt @testcase=37425 @bvt1.5
Scenario: Verify Column names in Change massive strategies page
When I navigate to "ConfigureAttributesNodes" page
And I click on "Change Ownership Rules" "dropdown"
And I click on "Massive By Node" "link"
And I see "Change massive strategies" header
Then I should see the "Columns" on the page
| Columns                 |
| Segmento                |
| Operador                |
| Tipo de nodo            |
| Nodo                    |
| Estrategia de propiedad |

@version=2 @bvt @testcase=37426 @bvt1.5
Scenario: Verify massive strategies change page is shown default by 100 records
When I navigate to "ConfigureAttributesNodes" page
And I click on "Change Ownership Rules" "dropdown"
And I click on "Massive By Node" "link"
And I see "Change massive strategies" header
Then 100 records should be shown by default in the grid

@testcase=37427 @bvt @version=2 @bvt1.5
Scenario: Verify massive strategies change when filters are not applied and assuming that nodes are having different ownership strategies
When I navigate to "ConfigureAttributesNodes" page
And I click on "Change Ownership Rules" "dropdown"
And I click on "Massive By Node" "link"
And I see "Change massive strategies" header
And I change the elements count per page to 10
And I select all the records from grid
And I get all ownership strategies for 10 records in the grid
And I click on "Change Strategy" "button"
And I see confirmation dialog with total number of records as 10 and ownership strategy information
And I see button name as "Aceptar"
And I click on "Bulk Update Confirm" "Accept" "button"
And I should see "Node Ownership Rules" "Functions" "Interface"
And I see old strategies separated by comma on the interface
And I select new ownership strategy from "Node Attributes" "To" "dropdown"
And I click on "Node Attributes" "Functions" "Submit" "button"
Then I see 10 records in the grid are updated with new ownership strategy

@testcase=37428 @bvt @version=2 @bvt1.5
Scenario: Verify massive strategies change when filter is applied on ownership strategy column
When I navigate to "ConfigureAttributesNodes" page
And I click on "Change Ownership Rules" "dropdown"
And I click on "Massive By Node" "link"
And I see "Change massive strategies" header
And I change the elements count per page to 10
And I enter operator in "Node Ownership Rules" "Operator" "textbox"
And I get all ownership strategies for 10 records in the grid
And I select all the records from grid
And I click on "Change Strategy" "button"
And I see confirmation dialog with total number of records as 10 and ownership strategy information
And I see button name as "Aceptar"
And I click on "Bulk Update Confirm" "Accept" "button" on confirmation popup
And I should see "Node Ownership Rules" "Functions" "Interface"
And I see old strategies separated by comma on the interface
And I select new ownership strategy from "Node Attributes" "To" "dropdown"
And I click on "Node Attributes" "Functions" "Submit" "button"
Then I see 10 records in the grid are updated with new ownership strategy

@testcase=37429 @bvt @version=2 @bvt1.5
Scenario: Verify old ownership strategy should not be shown in the ownership strategy dropdown while updating strategy
When I navigate to "ConfigureAttributesNodes" page
And I click on "Change Ownership Rules" "dropdown"
And I click on "Massive By Node" "link"
And I see "Change massive strategies" header
And I change the elements count per page to 10
And I get all ownership strategies for 10 records in the grid
And I enter ownership strategy in "Node Ownership Rules" "Rule Name" "textbox"
And I select all the records from grid
And I click on "Change Strategy" "button"
And I should see "Node Ownership Rules" "Functions" "Interface"
And I see old strategies separated by comma on the interface
Then I should not see the old ownership strategy from "Node Attributes" "To" "dropdown"

@testcase=37430 @version=2
Scenario: Verify cancel button functionality of confirmation popup
When I navigate to "ConfigureAttributesNodes" page
And I click on "Change Ownership Rules" "dropdown"
And I click on "Massive By Node" "link"
And I see "Change massive strategies" header
And I get all ownership strategies for 100 records in the grid
And I select all the records from grid
And I click on "Change Strategy" "button"
And I see confirmation dialog with total number of records as 100 and ownership strategy information
And I click on "Bulk Update Confirm" "Cancel" "button"
Then the popup should be closed

@testcase=37431 @version=2
Scenario: Verify cancel button functionality of edit ownership strategy popup
When I navigate to "ConfigureAttributesNodes" page
And I click on "Change Ownership Rules" "dropdown"
And I click on "Massive By Node" "link"
And I see "Change massive strategies" header
And I enter operator in "Node Ownership Rules" "Operator" "textbox"
And I change the elements count per page to 10
And I get all ownership strategies for 10 records in the grid
And I select all the records from grid
And I click on "Change Strategy" "button"
And I see confirmation dialog with total number of records as 10 and ownership strategy information
And I see button name as "Aceptar"
And I click on "Bulk Update Confirm" "Accept" "button" on confirmation popup
And I should see "Node Ownership Rules" "Functions" "Interface"
And I click on "Node Ownership Rules" "Functions" "Cancel" "button"
Then the popup should be closed

@testcase=37432 @version=2
Scenario: Verify close button functionality of confirmation popup
When I navigate to "ConfigureAttributesNodes" page
And I click on "Change Ownership Rules" "dropdown"
And I click on "Massive By Node" "link"
And I see "Change massive strategies" header
And I change the elements count per page to 10
And I get all ownership strategies for 10 records in the grid
And I select all the records from grid
And I click on "Change Strategy" "button"
And I see confirmation dialog with total number of records as 10 and ownership strategy information
And I click on "modal" "close" "label"
Then the popup should be closed

@testcase=37433 @version=2
Scenario: Verify close button functionality of edit ownership strategy popup
When I navigate to "ConfigureAttributesNodes" page
And I click on "Change Ownership Rules" "dropdown"
And I click on "Massive By Node" "link"
And I see "Change massive strategies" header
And I change the elements count per page to 10
And I enter operator in "Node Ownership Rules" "Operator" "textbox"
And I get all ownership strategies for 10 records in the grid
And I select all the records from grid
And I click on "Change Strategy" "button"
And I see confirmation dialog with total number of records as 10 and ownership strategy information
And I see button name as "Aceptar"
And I click on "Bulk Update Confirm" "Accept" "button" on confirmation popup
And I should see "Node Ownership Rules" "Functions" "Interface"
And I click on "modal" "close" "label"
Then the popup should be closed

@testcase=41498
Scenario: Verify change strategy button is enabled when one or more records selected in the grid
When I navigate to "ConfigureAttributesNodes" page
And I click on "Change Ownership Rules" "dropdown"
And I click on "Massive By Node" "link"
And I see "Change massive strategies" header
And I select one or more records in the grid
Then verify that "Change Strategy" "button" is "enabled"

@testcase=41499 
Scenario: Verify change strategy button is disabled when none of the records selected in the grid
When I navigate to "ConfigureAttributesNodes" page
And I click on "Change Ownership Rules" "dropdown"
And I click on "Massive By Node" "link"
And I see "Change massive strategies" header
And none of the records selected in the grid
Then verify that "Change Strategy" "button" is "disabled"
