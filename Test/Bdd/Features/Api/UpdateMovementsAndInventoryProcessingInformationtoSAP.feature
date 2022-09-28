@sharedsteps=7539 @owner=jagudelos @api @testplan=74716 @testsuite=108083 @manual
Feature: ServiceToSendTheInventoriesMovementsToSAP
As TRUE system, I need to modify the response sent to SAP PO
with the result of movements and inventories
processing to add the source system of the request.

Background: Login
Given I am authenticated as "admin"


@testcase=108091
Scenario: Verify sourceSystemID field is returned in processing result of movement when the processing is successful
Given I have data to process movement in the system
When I register one movement
Then the movement should be registered in the system
And sourceSystemID should be included in processing result of movement to SAP


@testcase=108092
Scenario: Verify sourceSystemID field is returned in processing result of inventory when the processing is successful
Given I have data to process inventory in the system
When I register one inventory
Then the inventory should be registered in the system
And sourceSystemID should be included in processing result of inventory to SAP

@testcase=108093
Scenario: Verify sourceSystemID field is returned in processing result of movement when the processing is unsuccessful
Given I have data to process movement in the system
When I register one already existing movement
Then the movement should not be registered in the system
And sourceSystemID should be included in processing result of movement to SAP


@testcase=108094
Scenario: Verify sourceSystemID field is returned in processing result of inventory when the processing is unsuccessful
Given I have data to process inventory in the system
When I register one already existing inventory
Then the inventory should not be registered in the system
And sourceSystemID should be included in processing result of inventory to SAP


@testcase=108095
Scenario: Verify sourceSystemID field is returned in processing result of inventories when the processing for some are successful and some are unsuccessful
Given I have data to process inventories in the system
When I register new and existing inventories
Then the valid inventory should be registered in the system
And invalid inventory should not be registered
And sourceSystemID should be included in processing result of inventories to SAP

@testcase=108096
Scenario: Verify sourceSystemID field is returned in processing result of movements when the processing for some are successful and some are unsuccessful
Given I have data to process movements in the system
When I register new and existing movements
Then the valid movement should be registered in the system
And invalid movement should not be registered
And sourceSystemID should be included in processing result of movements to SAP

@testcase=108097
Scenario: Verify sourceSystemID field is returned in processing result of movements when the Source System sent does not exist in true
Given I have data to process movements in the system
When I register new movements with Source System that does not exist in true
Then the movement should not be registered in the system
And sourceSystemID should be included in processing result of movement to SAP

@testcase=108098
Scenario: Verify sourceSystemID field is returned in processing result of inventory when the Source System sent does not exist in true
Given I have data to process inventory in the system
When I register new inventory with Source System that does not exist in true
Then the inventory should not be registered in the system
And sourceSystemID should be included in processing result of inventory to SAP

