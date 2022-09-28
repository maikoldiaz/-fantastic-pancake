@owner=jagudelos @api @testplan=6671 @testsuite=6700
Feature: Nodes Additional Parameter
I need the node service to be extended
to include attributes that allow
to complete the balance and calculate the ownership

@testcase=7594 @api @output=QueryAll(GetNodes) @bvt @audit
Scenario: Update Control Limit and Acceptable Balance Percentage of Existing Node with valid values
	Given I have "Node" in the system
	When I provide the Acceptable Balance Percentage and Control Limit
	Then the response should succeed with Control Limit and Acceptable Balance Percentage updated
	And "Update" should be registered in Audit-log

@testcase=7595 @api @output=QueryAll(GetNodes) @bvt @audit
Scenario: Update Uncertainity of Products related to Node
	Given I have "Node" in the system
	When I provide the uncertainity percentage
	Then the response should succeed with uncertainity updated
	And "Update" should be registered in Audit-log

@testcase=7596 @api @output=QueryAll(GetNodes) @bvt @audit
Scenario: Update Owner of Products related to Node
	Given I have "Node" in the system
	When I provide the owner data
	Then the response should succeed with changes with owner updated
	And "Update" should be registered in Audit-log

@testcase=7597 @api @output=QueryAll(GetNodes)
Scenario: Update Owner of Products related to Node without OwnerId
	Given I have "Node" in the system
	When I provide the owner data without ownerId
	Then the response should fail with message "El identificador del propietario  es obligatorio"

@testcase=7598 @api @output=QueryAll(GetNodes)
Scenario: Update Owner of Products related to Node without Ownership Percentage
	Given I have "Node" in the system
	When I provide the owner data without ownership percentage
	Then the response should fail with message "El porcentaje de propiedad es obligatorio"

@testcase=7599 @api @output=QueryAll(GetNodes)
Scenario: Update Owner of Products related to Node with incorrect ownership value
	Given I have "Node" in the system
	When I provide the ownership value and the percentages sum of the owners list is more than 100%
	Then the response should fail with message "La sumatoria de los valores de propiedad debe ser 100%"

@testcase=7600 @api @output=QueryAll(GetNodes) @bvt @audit
Scenario: Update Control Limit of Existing Node with valid value
	Given I have "Node" in the system
	When I provide the Control Limit
	Then the response should succeed with Control Limit updated
	And "Update" should be registered in Audit-log

@testcase=7601 @api @output=QueryAll(GetNodes) @bvt @audit
Scenario: Update Acceptable Balance Percentage of Existing Node with valid value
	Given I have "Node" in the system
	When I provide the Acceptable Balance Percentage
	Then the response should succeed with Acceptable Balance Percentage updated
	And "Update" should be registered in Audit-log

@testcase=7602 @api @output=QueryAll(GetNodes) @bvt
Scenario: Get Node by Id with valid Id
	Given I have "Transport Nodes" in the system
	When I Get record with valid Id
	Then the response should return requested additional details