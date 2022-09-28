@owner=jagudelos @backend @testplan=24148 @testsuite=24155
Feature: ReviewCurrentPurchaseAndSalesContractsDailyToCreateNewMovements
As TRUE system, I need to review the current purchase
and sales contracts daily to create new movements

@testcase=25264 @version=2
Scenario: Verify that TRUE system is running the automatic process to create movements daily when movement is already exist for same execution date of automatic process
Given I have a movement of contract identifier for same execution date of automatic process "daily"
Then movement should not register "daily"

@testcase=25265 @version=2
Scenario: Verify that TRUE system is running the automatic process to create movements daily when movement is not exist for same execution date of automatic process
Given I do not have a movement of contract identifier for same execution date of automatic process "daily"
Then movement should register "daily"

@testcase=25266 @version=2
Scenario: Verify that TRUE system is running the automatic process to create movements weekly when movement is already exist for same execution date of automatic process
Given I have a movement of contract identifier for same execution date of automatic process "weekly"
Then movement should not register "weekly"

@testcase=25267 @version=2
Scenario: Verify that TRUE system is running the automatic process to create movements weekly when movement is not exist for same execution date of automatic process
Given I do not have a movement of contract identifier for same execution date of automatic process "weekly"
Then movement should register "weekly"

@testcase=25268 @version=2
Scenario: Verify that TRUE system is running the automatic process to create movements biweekly when movement is already exist for same execution date of automatic process
Given I have a movement of contract identifier for same execution date of automatic process "biweekly"
Then movement should not register "weekly"

@testcase=25269 @version=2
Scenario: Verify that TRUE system is running the automatic process to create movements biweekly when movement is not exist for same execution date of automatic process
Given I do not have a movement of contract identifier for same execution date of automatic process "biweekly"
Then movement should register "biweekly"

@testcase=25270 @version=2
Scenario: Verify that TRUE system is running the automatic process to create movements monthly when movement is already exist for same execution date of automatic process
Given I have a movement of contract identifier for same execution date of automatic process "monthly"
Then movement should not register "biweekly"

@testcase=25271 @version=2
Scenario: Verify that TRUE system is running the automatic process to create movements monthly when movement is not exist for same execution date of automatic process
Given I do not have a movement of contract identifier for same execution date of automatic process "monthly"
Then movement should register "biweekly"