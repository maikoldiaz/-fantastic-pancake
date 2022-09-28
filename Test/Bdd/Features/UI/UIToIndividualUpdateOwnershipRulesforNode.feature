@sharedsteps=4013 @owner=jagudelos @ui @testplan=35673 @testsuite=35695 @parallel=false
Feature: UIToIndividualUpdateOwnershipRulesforNode
	As an Administrator user, I need an UI to 
	bulk update ownership rules for node

Background: Login
	Given I am logged in as "admin"

@bvt @version=2 @testcase=37453 @bvt1.5
Scenario: Verify the graphic style at the top of the ownership strategy change page
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "massiveByNode" "link"
	Then Icon should be changed to the "warning" mode
	And I should see the title as "Vigencia de las estrategias de propiedad."
	And I should see an informational message as "Recuerde que puede actualizar la lista de estrategias de propiedad"
	And I should see "node" "ruleSynchronizer" "Button"
	Then I should see "Error" interface

@bvt @version=2 @testcase=37454 @bvt1.5
Scenario:  Verify the functionality when cache update at the node-level is in progress and the process is running
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "massiveByNode" "link"
	And I click on "node" "ruleSynchronizer" "Button"
	Then Icon should be changed to the "information" mode
	And I should see an informational message as "Actualizando las estrategias. Espere a que el proceso finalice."
	And verify that "node" "ruleSynchronizer" "Button" is "disabled"
	And verify that "changeStrategy" "button" is "disabled"
	And I should see "nodeOwnershipRules" "edit" "link" as disabled

@bvt @version=2 @testcase=37455 @bvt1.5
Scenario: Verify successful cache update at the node Level
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "massiveByNode" "link"
	And I get the detials from OwnershipRuleRefreshHistory Table
	And I click on "node" "ruleSynchronizer" "Button"
	Then Icon should be changed to the "information" mode
	And the strategy cache should be updated by invoking the FICO service when the process is completed
	And Icon should be changed to the "success" mode
	And I should see an informational message as "Se actualizaron las estrategias de propiedad."
	And verify that "node" "ruleSynchronizer" "Button" is "enabled"
	And verify that "changeStrategy" "button" is "disabled"
	And I should see "nodeOwnershipRules" "edit" "link" button as enabled

@bvt @version=2 @testcase=37456 @bvt1.5
Scenario: Verify Success message will automatically switch to warning mode after 10 sec of successfull cache update
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "massiveByNode" "link"
	And I get the detials from OwnershipRuleRefreshHistory Table
	And I click on "node" "ruleSynchronizer" "Button"
	Then Icon should be changed to the "information" mode
	And the strategy cache should be updated by invoking the FICO service when the process is completed
	And Icon should be changed to the "success" mode
	And wait for the "10" sec after the Fico Invocation completed with success
	And Icon should be changed to the "warning" mode
	And I should see an informational message as "Recuerde que puede actualizar la lista de estrategias de propiedad"
	And verify that "node" "ruleSynchronizer" "Button" is "enabled"
	And verify that "changeStrategy" "button" is "disabled"
	And I should see "nodeOwnershipRules" "edit" "link" button as enabled

@version=2 @testcase=37457
Scenario: Verify master rules at node level
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "massiveByNode" "link"
	Then the system must use the "estrategiaPropiedadNodo" collection of the cached service response to display the list of ownership strategies per node

@manual @version=2 @testcase=37458
Scenario: Verify failed cache update
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "massiveByNode" "link"
	And I click on "node" "ruleSynchronizer" "Button"
	Then Icon should be changed to the "information" mode
	And Fico invocation completed with error
	And Icon should be changed to the "Error" mode
	And I should see the title as "Error"
	And verify that "node" "ruleSynchronizer" "Button" is "enabled"
	And verify that "changeStrategy" "button" is "disabled"
	And I should see "nodeOwnershipRules" "edit" "link" button as disabled
	And I should see an informational message text must be the one delivered by the API

@bvt @version=2 @testcase=37459 @bvt1.5
Scenario: Verify individual strategy change
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "massiveByNode" "link"
	And I see "Change massive strategies" header in page
	And I click on "nodeOwnershipRules" "edit" "link" present at end of record
	And I should see "Edit Property Strategy" interface
	And I should see current ownership strategy on the interface
	And I click on "nodeAttributes" "to" "dropdown"
	And I select New ownership strategy in "New ownership strategy"
	And I click on "nodeAttributes" "functions" "submit" "button"
	Then I see record in the grid are updated with New ownership strategy

@bvt @version=2 @testcase=37460 @bvt1.5
Scenario: Verify cancel button functionality of edit ownership strategy popup at node level
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "massiveByNode" "link"
	And I see "Change massive strategies" header in page
	And I click on "nodeOwnershipRules" "edit" "link" present at end of record
	And I should see "Edit Property Strategy" interface
	And I click on "nodeOwnershipRules" "functions" "cancel" "button"
	Then the popup should be closed

@bvt @version=2 @testcase=37461 @bvt1.5
Scenario: Verify old ownership strategy should not be shown in the ownership strategy dropdown while updating strategy
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "massiveByNode" "link"
	And I see "Change massive strategies" header in page
	And I click on "nodeOwnershipRules" "edit" "link" present at end of record
	And I should see "Edit Property Strategy" interface
	And I should see current ownership strategy on the interface
	Then I should not see old ownership strategy in "New ownership strategy"

@bvt @version=2 @testcase=37462 @bvt1.5
Scenario:  Verify the error message when user click on the strategy button while FICO-Process is running
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "massiveByNode" "link"
	And I click on "node" "ruleSynchronizer" "Button"
	Then Icon should be changed to the "success" mode
	And wait for the "10" sec after the Fico Invocation completed with success
	And I update the OwnershipRuleRefreshHistory Table and set the status of last record as "1"
	And I click on "node" "ruleSynchronizer" "Button"
	And I see this message "El proceso de actualización de las estrategias ya está en ejecución" on "confirm" "message" "container"
	And I update the OwnershipRuleRefreshHistory Table and set the status of last record as "0"

@version=2 @testcase=37463
Scenario:  Verify the functionality when cache update is in Progress at the another user on another screen
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "massiveByNode" "link"
	And I click on "node" "ruleSynchronizer" "Button"
	And I open the new tab and navigate to the screen
	Then Icon should be changed to the "information" mode
	And I should see an informational message as "Actualizando las estrategias. Espere a que el proceso finalice."
	And I should see "node" "ruleSynchronizer" "Button" as disabled
	And I should see "changeStrategy" "button" as disabled
	And I should see "nodeOwnershipRules" "edit" "link" button as disabled

@version=2 @testcase=37464
Scenario: Verify the functionality when cache update is successfull at the another user on another screen
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "massiveByNode" "link"
	And I click on "node" "ruleSynchronizer" "Button"
	And I open the new tab and navigate to the screen
	Then Icon should be changed to the "information" mode
	And I should see an informational message as "Actualizando las estrategias. Espere a que el proceso finalice."
	And Icon should be changed to the "success" mode
	And I should see an informational message as "Se actualizaron las estrategias de propiedad."
	And verify that "node" "ruleSynchronizer" "Button" is "enabled"
	And verify that "changeStrategy" "button" is "disabled"
	And I should see "nodeOwnershipRules" "edit" "link" button as enabled

@version=2 @testcase=37465
Scenario: Verify Success message will automatically switch to warning mode after 10 sec of successfull cache update at the another user on another screen
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "changeOwnershipRules" "dropdown"
	And I click on "massiveByNode" "link"
	And I open the new tab and navigate to the screen
	And I click on "node" "ruleSynchronizer" "Button"
	Then Icon should be changed to the "information" mode
	And I should see an informational message as "Actualizando las estrategias. Espere a que el proceso finalice."
	Then Icon should be changed to the "success" mode
	And I should see an informational message as "Se actualizaron las estrategias de propiedad."
	And Icon should be changed to the "warning" mode
	And I should see an informational message as "Recuerde que puede actualizar la lista de estrategias de propiedad"
	And verify that "node" "ruleSynchronizer" "Button" is "enabled"
	And verify that "changeStrategy" "button" is "disabled"
	And I should see "nodeOwnershipRules" "edit" "link" button as enabled