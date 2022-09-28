@sharedsteps=4013 @owner=jagudelos @ui @testsuite=35691 @testplan=35673 @parallel=false
Feature: UIToBulkUpdateOwnershipRulesForConnectionProducts
As an application administrator
I need an UI to bulk update ownership rules for connection/products

Background: Login
Given I am logged in as "admin"

@testcase=37403 @bvt @version=2 @bvt1.5
Scenario: Verify massive start button in configure connections attributes page
When I navigate to "ConfigureAttributesConnections" page
Then I should see "massiveConnectionProduct" "button"

@testcase=37404 @version=2
Scenario: Verify change massive strategies page
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
Then I see "Change massive strategies" header

@testcase=37405 @version=2
Scenario: Verify column names in change massive strategies page at connection/product level
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
Then I should see the "Columns" on the page
| Columns                 |
| Operador origen         |
| Operador destino        |
| Nodo Origen             |
| Nodo Destino            |
| Producto                |
| Estrategia de propiedad |

@testcase=37406 @version=2
Scenario: Verify list of strategies at connection/product level
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
Then I should see the message "Recuerde que puede actualizar la lista de estrategias de propiedad." in the Page
And I should see "changeStrategy" "button" as disabled
And the record count in Grid shown per page should be "100"

@testcase=37407 @version=2 @output=QueryAll(GetActiveNodeConnection)
Scenario: Verify successful cache update
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
And I get last updated deatils from ownershipRuleRefreshHistory table
And I click on "nodeConnectionProduct" "ruleSynchronizer" "button"
Then I should see the message "Actualizando las estrategias. Espere a que el proceso finalice." in the Page
And strategy cache should be updated by invoking the FICO service
And I should see the message "Se actualizaron las estrategias de propiedad." in the Page
And wait for the 10 sec for the Fico Invocation to complete successfully
And I should see the message "Recuerde que puede actualizar la lista de estrategias de propiedad" in the Page
And I should see "changeStrategy" "button" as disabled
And I should see "nodeConnectionProduct" "ruleSynchronizer" "button" as enabled
And I should see "nodeConnectionProductRules" "edit" "link" as enabled

@testcase=37408 @manual
Scenario: Verify failed cache update
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
And I click on "Massive connection-product" "button"
And I click on "Update strategies" "button"
And when the invocation ends with an error
Then I should see an error message returned by the API when the invocation completed with error
And the title must be changed to "Error"
And verify "text" "icon" and "style" of the "error" message
And I should see "change strategies" and "edit individually" buttons as disabled
And I should see "update strategies" button as enabled

@testcase=37409 @verion=2 @output=QueryAll(GetActiveNodeConnection)
Scenario: Verify the functionality when cache update is in progress and the process is running
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
And I click on "nodeConnectionProduct" "ruleSynchronizer" "button"
Then I should see the message "Actualizando las estrategias. Espere a que el proceso finalice." in the Page
And I should see "changeStrategy" "button" as disabled
And I should see "nodeConnectionProduct" "ruleSynchronizer" "button" as disabled
And I should see "nodeConnectionProductRules" "edit" "link" as disabled

@testcase=37410 @version=2
Scenario: Try to update strategies while cache update is in progress
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
And I click on "nodeConnectionProduct" "ruleSynchronizer" "button"
And wait for the 10 sec for the Fico Invocation to complete successfully
Then I should see the message "Se actualizaron las estrategias de propiedad" in the Page
And I update the OwnershipRuleRefreshHistory Table and set the status of last record as "1"
And I click on "nodeConnectionProduct" "ruleSynchronizer" "button"
And I should see "El proceso de actualización de las estrategias ya está en ejecución." error message
And I update the OwnershipRuleRefreshHistory Table and set the status of last record as "0"

@testcase=37411 @version=2
Scenario: Verify master rules at connection/product level
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "link"
Then system must use "estrategiaConexiones" collection of the cached service response that invokes FICO

@testcase=37412 @bvt @version=2 @output=QueryAll(GetActiveNodeConnection) @bvt1.5
Scenario: Verify massive strategies change when filters are not applied and assuming that nodes are having different ownership strategies
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
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

@testcase=37413 @bvt @version=2 @output=QueryAll(GetActiveNodeConnection) @bvt1.5
Scenario: Verify massive strategies change when filter is applied on ownership strategy column
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
And I see "Change massive strategies" header
And I change the elements count per page to 10
And I enter operator in "nodeConnectionProductRules" "sourceOperators" "textbox"
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

@testcase=37414 @version=2
Scenario: Verify enabling a button to change strategies based on record selection
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
Then I see "Change massive strategies" header
And I should see "changeStrategy" "button" as disabled
And I change the elements count per page to 10
And I select all the records from grid
And I should see "changeStrategy" "button" as enabled

@testcase=37415 @bvt @version=2 @output=QueryAll(GetActiveNodeConnection) @bvt1.5
Scenario: Verify old ownership strategy should not be shown in the ownership strategy dropdown while updating strategy
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
And I see "Change massive strategies" header
And I change the elements count per page to 10
And I get all ownership strategies for 10 records in the grid
And I enter ownership strategy in "nodeConnectionProductRules" "Rule Name" "textbox"
And I select all the records from grid
And I click on "Change Strategy" "button"
And I should see "Node Ownership Rules" "Functions" "Interface"
And I see old strategies separated by comma on the interface
Then I should not see the old ownership strategy from "Node Attributes" "To" "dropdown"

@testcase=37416 @version=2
Scenario: Verify individual strategy change
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
And I see "Change massive strategies" header
And I click on "nodeConnectionProductRules" "edit" "link"
And I should see "Node Ownership Rules" "Functions" "Interface"
And I select new ownership strategy from "Node Attributes" "To" "dropdown"
And I click on "Node Attributes" "Functions" "Submit" "button"
And I click on "nodeAttributes" "functions" "submit" "button"
Then I see record is updated with new strategy

@testcase=37417 @version=2
Scenario: Verify cancel button functionality of confirmation popup at connection/product level
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
And I see "Change massive strategies" header
And I change the elements count per page to 10
And I select all the records from grid
And I click on "Change Strategy" "button"
And I see confirmation dialog with total number of records as 10 and ownership strategy information
And I click on "bulkUpdateConfirm" "cancel" "button"
Then the popup should be closed

@testcase=37418 @version=2
Scenario: Verify cancel button functionality of edit ownership strategy popup at connection/product level
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
And I see "Change massive strategies" header
And I click on "nodeConnectionProductRules" "edit" "link"
And I should see "Edit ownership strategy" "Interface"
And I click on "nodeOwnershipRules" "functions" cancel" "button"
Then the popup should be closed

@testcase=37419 @version=2
Scenario: Verify close button functionality of confirmation popup at connection/product level
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
And I see "Change massive strategies" header
And I change the elements count per page to 10
And I select all the records from grid
And I click on "Change Strategy" "button"
And I see confirmation dialog with total number of records as 10 and ownership strategy information
And I click on "modal" "close" "label"
Then the popup should be closed

@testcase=37420 @version=2
Scenario: Verify close button functionality of edit ownership strategy popup at connection/product level
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
And I click on "massiveConnectionProduct" "button"
And I see "Change massive strategies" header
And I click on "nodeConnectionProductRules" "edit" "link"
And I click on "Change Strategy" "button"
And I should see "Edit ownership strategy" "Interface"
And I click on "modal" "close" "label"
Then the popup should be closed