@sharedsteps=7539 @owner=jagudelos @api @testplan=19772 @testsuite=19792
Feature: DeleteAndQueryHomologationGroups
In order to handle the Data Mapping
As an application administrator
I want to Query and Delete Homologation Groups

Background: Login
Given I am authenticated as "admin"

@testcase=21117 @bvt @audit @output=QueryAll(GetHomologationGroups) @version=2
Scenario: Delete a homologation group when the homologation has more than one group
Given I have "HomologationWithTwoGroups" in the system
When I delete the existing Homologation Group from "CreatedHomologationWithTwoGroups"
Then the response should succeed with message "Grupo de homologación  eliminado con éxito"
And the data corresponding to the Homologation Group should be cascade removed
And "Delete" should be registered in the Audit-log for "HomologationGroup"

@testcase=21118 @bvt @audit @output=QueryAll(GetHomologationGroups) @version=2
Scenario: Delete a Homologation Group when a homologation has only one homologation group assigned
Given I have "Homologations" in the system
When I delete the existing Homologation Group from "CreatedHomologationWithTwoGroups"
Then the response should succeed with message "Grupo de homologación  eliminado con éxito"
And the data corresponding to the Homologation Group should be cascade removed
And the respective homologation should also be deleted
And "Delete" should be registered in the Audit-log for "Homologation"

@testcase=21119 @output=QueryAll(GetHomologationGroups) @version=2
Scenario: Delete a Homologation Group by invalid homologation group id
Given I have "Homologations" in the system
When I delete the existing Homologation Group by invalid id from "CreatedHomologationWithTwoGroups"
Then the response should fail with "BadRequest"

@testcase=21120 @bvt @output=QueryAll(GetHomologationGroups) @version=2
Scenario: Query all homologation groups
Given I have "Homologations" in the system
When I Get all Homologation Group records
Then the response should return all valid odata records

@testcase=21121 @bvt @output=QueryAll(GetHomologationGroups) @version=2
Scenario: Query a homologation group
Given I have "HomologationGroup" in the system
When I Get the record by "SourceSystem" "DestinationSystem" and "GroupType"
Then the response should return requested odata record details

@testcase=21122 @output=QueryAll(GetHomologationGroups) @version=2
Scenario: Query a homologation group by invalid group type
Given I have "Homologations" in the system
When I Get the record by "SourceSystem" "DestinationSystem" and invalid "GroupType"
Then the response should return empty data