@sharedsteps=4013 @owner=jagudelos @ui @testplan=35673 @testsuite=35684 @parallel=false
Feature: UIToBulkUploadOwnershipRulesForNodeProducts
As an Administrator user,
I need an UI to
bulk update ownership rules for node/products

Background: Login
Given I am logged in as "admin"
@testcase=37435
Scenario: Validate Massive strategy button per node products in configuration attributes page
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "Change Ownership Rules" "dropdown"
	Then I should see "Massive By Node Products" "link"

@testcase=37436
Scenario: Validate list of strategies at node-product level and page refresh
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "Change Ownership Rules" "dropdown"
	And I click on "Massive By Node Products" "link"
	Then the record count in Grid shown per page should be 100
	And I should see the message "Recuerde que puede actualizar la lista de estrategias de propiedad" in the Page
	And I should see "changeStrategy" "button" as disabled
	And I should see "nodeProductRules" "edit" "link" as enabled
	When I refresh the page
	Then I should see the message "Recuerde que puede actualizar la lista de estrategias de propiedad" in the Page

@testcase=37437
Scenario: Validate column names of change massive strategies grid
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "Change Ownership Rules" "dropdown"
	And I click on "Massive By Node Products" "link"
	Then I should see the "Columns" on the page
	| Columns                 |
	| Segmento                |
	| Operador                |
	| Tipo de nodo            |
	| Nodo                    |
	| Almacén                 |
	| Producto                |
	| Estrategia de propiedad |

@testcase=37438 @bvt @version=2 @bvt1.5
Scenario: Validate success cache update
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "Change Ownership Rules" "dropdown"
	And I click on "Massive By Node Products" "link"	
	And I get last updated deatils from ownershipRuleRefreshHistory table
	And I click on "storageLocationProduct" "ruleSynchronizer" "button"
	Then I should see the message "Actualizando las estrategias. Espere a que el proceso finalice." in the Page
	And I should see "changeStrategy" "button" as disabled
	And I should see "storageLocationProduct" "ruleSynchronizer" "button" as disabled
	And I should see "nodeProductRules" "edit" "link" as disabled
	And the strategy cache should be updated by invoking the FICO service when the process is completed
	And I should see the message "Se actualizaron las estrategias de propiedad." in the Page
	And wait for the 10 sec for the Fico Invocation to complete successfully
	Then I should see the message "Recuerde que puede actualizar la lista de estrategias de propiedad" in the Page
	And I should see "changeStrategy" "button" as disabled
	And I should see "storageLocationProduct" "ruleSynchronizer" "button" as enabled
	And I should see "nodeProductRules" "edit" "link" as disabled

@testcase=37439 @manual 
Scenario: Validate failed cache update
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "Massive per node product" "link"
	And I click on "update" "strategies" "button"
	Then the strategy cache should be updated by invoking the FICO service when the process is completed
	And I should see an informational message as "Actualizando las estrategias. Espere a que el proceso finalice."
	And I should see "change strategies" "button" as disabled
	And I should see "update" "strategies" "button" as disabled
	And I should see "edit" "strategies" "button" as disabled
	And Fico invocation completed with error
	Then Icon should be changed to the "Error mode"
	And I should see the title as "Error"
	And I should see "change" "strategies" "button" as disabled
	And I should see "edit" "strategies" "button" as disabled
	And I should see "update strategies" "button" as enabled

@testcase=37440 @version=2
Scenario: Validate master rules
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "Massive per node product" "link"
	Then system must use "estrategiaPropiedadNodoProducto" collection of the cached service response that invokes FICO

@testcase=37441 @bvt @version=2 @bvt1.5
Scenario: Validate Change massive strategies selection without filters applied
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "Massive By Node Products" "link"
	And I change the elements count per page to 10
	And I select all records from grid
	And I get ownership strategies for "10" records in the grid	
	And I click on "changeStrategy" "button"
	Then I see confirmation dialog with "10" records and ownership strategy information
	And I see button name "Aceptar" in confirmation window
	And I see message as "Los nodos seleccionados tienen más de una estrategia de propiedad. Por favor confirme que desea editar los registros:" in "transferPointLogistics" "name" "label"
	And I click on "Bulk Update Confirm" "Accept" "button"
	And I should see "Node Ownership Rules" "Functions" "Interface"
	And I see strategies separated by comma on the interface
	And I select ownership strategy from "Node Attributes" "To" "dropdown"
	And I click on "Node Attributes" "Functions" "Submit" "button"
	Then I see "10" records in the grid with new ownership strategy

@testcase=37442 @version=2 @bvt1.5
Scenario: Validate change massive strategies when filter is applied
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "Massive By Node Products" "link"
	And I see "Change massive strategies" header
	And I change the elements count per page to 10
	And I input value in "Node Product Rules" "Operator" "textbox"
	And I get ownership strategies for "10" records in the grid	
	And I select all records from grid
	And I click on "changeStrategy" "button"
	Then I see confirmation dialog with "10" records and ownership strategy information
	And I see button name "Aceptar" in confirmation window
	And I see message as "Los nodos seleccionados tienen más de una estrategia de propiedad. Por favor confirme que desea editar los registros:" in "transferPointLogistics" "name" "label"
	And I click on "Bulk Update Confirm" "Accept" "button"
	And I should see "Node Ownership Rules" "Functions" "Interface"
	And I see strategies separated by comma on the interface
	And I select ownership strategy from "Node Attributes" "To" "dropdown"
	And I click on "Node Attributes" "Functions" "Submit" "button"
	Then I see "10" records in the grid with new ownership strategy

@testcase=37443 @version=2
Scenario: validate close button of confirmation pop up for the modification with different current rules
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "Massive By Node Products" "link"
	And I change the elements count per page to 10
	And I select all records from grid
	And I click on "changeStrategy" "button"
	Then I see button name "Aceptar" in confirmation window
	And I click on "bulkUpdateConfirm" "cancel" "button"
	Then the popup should be closed

@testcase=37444 @version=2
Scenario:  Validate cancel button of Edit property strategy interface
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "Massive By Node Products" "link"
	And I change the elements count per page to 10
	And I get ownership strategies for "10" records in the grid
	And I select all records from grid
	And I click on "changeStrategy" "button"
	Then I see button name "Aceptar" in confirmation window
	And I click on "Bulk Update Confirm" "Accept" "button"
	And I click on "nodeOwnershipRules" "functions" "cancel" "button"
	Then the popup should be closed

@testcase=37445 @version=2
Scenario: Validate individual strategy change
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "Massive By Node Products" "link"
	And I click on "nodeProductRules" "edit" "link"
	And I should see "Node Ownership Rules" "Functions" "Interface"
	Then I select ownership strategy from "Node Attributes" "To" "dropdown"
	And I click on "Node Attributes" "Functions" "Submit" "button"
	And I click on "save" "button"
	Then I see record is updated with new strategy

@testcase=37446 @version=2
Scenario: Validate source item is removed in destination list
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "Massive per node product" "link"
	And I should see "Node Ownership Rules" "Functions" "Interface"
	And I should see current ownership strategy on the interface
	Then I should not see old ownership strategy in "New ownership strategy"

@testcase=37447 @version=2
Scenario: Validate source item is not removed in destination list when we have multiple strategies
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "Massive per node product" "link"
	And I see "Change massive strategies" header
	And I change the elements count per page to 10
	And I select all records from grid
	And I get ownership strategies for "10" records in the grid	
	And I click on "changeStrategy" "button"
	And I click on "Bulk Update Confirm" "Accept" "button"
	And I should see "Node Ownership Rules" "Functions" "Interface"
	Then all the strategies should be displayed in destination list dropdown

@testcase=37448 @version=2
Scenario: Validate cache update is in progress
	Given I have "node" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "Massive per node product" "link"
	And I click on "storageLocationProduct" "ruleSynchronizer" "Button"
	Then the strategy cache should be updated by invoking the FICO service when the process is completed
	And I should see an informational message as "Actualizando las estrategias. Espere a que el proceso finalice."
	And I should see "change strategies" "button" as disabled
	And I should see "update" "strategies" "button" as disabled
	And I should see "edit" "strategies" "button" as disabled
	And I should see an informational message as "Se actualizaron las estrategias de propiedad."
	And wait for the 10 sec for the Fico Invocation to complete successfully
	Then I should see the message "Recuerde que puede actualizar la lista de estrategias de propiedad" in the Page

@testcase=37449 @version=2
Scenario: validate updating strategies while cache update is in progress
	Given I have "node" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "Massive per node product" "link"
	And I click on "storageLocationProduct" "ruleSynchronizer" "Button"
	Then Icon should be changed to the "success" mode
	And wait for the "10" sec after the Fico Invocation completed with success
	And I update the OwnershipRuleRefreshHistory Table and set the status of last record as "1"
	And I click on "storageLocationProduct" "ruleSynchronizer" "Button"
	And I see this message "El proceso de actualización de las estrategias ya está en ejecución" on "confirm" "message" "container"
	And I update the OwnershipRuleRefreshHistory Table and set the status of last record as "0"

@testcase=37450 @version=2
Scenario: Validate change strategies button based on record selection
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "Massive per node product" "link"
	Then I should see "change strategy" "button" as disabled
	And I change the elements count per page to 10
	And I select all the records from grid
	And I should see "change" "strategy" "button" as enabled

@testcase=37451 
Scenario: Validate unique rules in rules label 
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "Massive per node product" "link"
	And I change the elements count per page to 10
	And I get all ownership strategies for 10 records in the grid
	And I enter ownership strategy in "nodeProductRules" "Rule Name" "textbox"
	And I select all the records from grid
	And I click on "Change Strategy" "button"
	And I should see "Node Ownership Rules" "Functions" "Interface"
	And I see old strategies separated by comma on the interface