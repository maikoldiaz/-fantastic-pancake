@sharedsteps=7539 @owner=jagudelos @api @testplan=55104 @testsuite=55112 @S14 @MVP2and3 @parallel=false
Feature: RegisterOfficialPointsSentBySAPPO
As TRUE system,
I need to register the official points sent by SAP PO
to identify the movements to be used in the operational cutoff and ownership calculation

Background: Login
Given I am authenticated as "admin"
@testcase=56772 @version=2
Scenario: Verify the functionality when the backup movement does not exist in TRUE
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And the "backup movement" is not registered in the TRUE application and all the other validations are met
Then backup movement information with the global identifier should be stored
And errors should be registered in pending transaction log when there are any errors occured while storing the backup movement for "registration"
And processing of the official movement should happen
@testcase=56773 @version=2
Scenario: Verify the functionality when the backup movement exists in TRUE and the last event type of the movement is a delete
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And the "backup movement" exists in TRUE
And all the validations are met and the last event type of the "backup movement" is a delete
Then backup movement information with the global identifier should be stored
And errors should be registered in pending transaction log when there are any errors occured while storing the backup movement for "last event type delete"
And processing of the official movement should happen
@testcase=56774 @version=2
Scenario: Verify the functionality when the backup movement exists in TRUE and the net quantity of the last event does not match with the net quantity of the movement received
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And the "backup movement" exists in TRUE
And all the validations are met and the net quantity of the last event does not match with the net quantity of the "backup movement" received
Then backup movement information should be stored
And "backup movement" with data of the last event type with negative net quantity should be stored
And errors should be registered in pending transaction log when there are any errors occured while storing the backup movement for "incorrect net quantity"
And processing of the official movement should happen
@testcase=56775 @version=2
Scenario: Verify the functionality when the backup movement exists in TRUE with the correct net quantity
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And the "backup movement" exists in TRUE
And all the validations are met and the net quantity of the last event matches with the net quantity of the "backup movement" received
Then last event type of the movement should be updated with the global identifier
@testcase=56776 @version=2
Scenario: Verify the functionality when the backup movement exists in TRUE and has events reported to SAP PO
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And the "backup movement" exists in TRUE
And all the validations are met and the "backup movement" has events reported to SAP PO as transfer points
Then the global identifier of last event type reported to SAP PO should be updated
@testcase=56777 @version=2
Scenario: Verify the functionality when the movement of the official point does not exist in TRUE
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And the "official movement" is not registered in the TRUE application and all the other validations are met
Then official movement information should be stored with all its details
And errors should be registered in pending transaction log when there are any errors occured while storing the official movement for "registration"
@testcase=56778 @version=2
Scenario: Verify the functionality when the official movement exists in TRUE and the last event type of the movement is a delete
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And the "official movement" exists in TRUE
And all the validations are met and the last event type of the "official movement" is a delete
Then official movement information should be stored with all its details
And errors should be registered in pending transaction log when there are any errors occured while storing the official movement for "last event type delete"
@testcase=56779 @version=2
Scenario: Verify the functionality when the official movement exists in TRUE and the net quantity of the last event does not match with the net quantity of the movement received
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And the "official movement" exists in TRUE
And all the validations are met and the net quantity of the last event does not match with the net quantity of the "official movement" received
Then "official movement" with data of the last event type with negative net quantity should be stored
And official movement information should be stored with all its details
And errors should be registered in pending transaction log when there are any errors occured while storing the official movement for "incorrect net quantity"
@testcase=56780 @version=2
Scenario: Verify the functionality when the official movement exists in TRUE with the correct net quantity
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And the "official movement" exists in TRUE
And all the validations are met and the net quantity of the last event matches with the net quantity of the "official movement" received
Then last event type of the movement should be updated with all the mentioned details
@testcase=56781 @version=2
Scenario: Verify the functionality when the official movement exists in TRUE and has events reported to SAP PO
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And the "official movement" exists in TRUE
And all the validations are met and the "official movement" has events reported to SAP PO as transfer points
Then last event type reported to SAP PO should be updated with all the mentioned details
@testcase=56782 @version=2
Scenario: Verify the functionality when only one official point movement arrives
Given I have data to process "Movements" in system
When I have 1 movement with all mandatory attributes
And I register "Movements" in system
And only one official point movement arrives
And all the other validations are met
Then last event type reported to SAP PO should be updated with all the mentioned details and backup movement identifier should be updated as null