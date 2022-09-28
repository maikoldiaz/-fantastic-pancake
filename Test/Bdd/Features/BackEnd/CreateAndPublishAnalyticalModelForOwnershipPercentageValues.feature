@owner=jagudelos @backend @testplan=39221 @testsuite=39241
Feature: CreateAndPublishAnalyticalModelForOwnershipPercentageValues
As a TRUE system, I need to populate the
OwnershipPercentageValues table for the analytical
model training results generation process

@testcase=41369 @bvt
Scenario: Verify if the Historical information of ownership percentage values are loaded into the blob
Given I want to create and publish analytical models for ownership percentage values
When I want to review the historical files of ownership percentage values information
Then I verify if "OwnershipPercentageValues" historical data is loaded into the storage blob

@testcase=41370 @bvt
Scenario: Verify if the OwnershipPercentageValues table is present in the database
Given I have historical data to create analytical models
When I want to load data for creation and publishing of analytical model I need sql table to store the data
Then I verify if the "OwnershipPercentageValues" are present

@testcase=41371 @bvt
Scenario: Verify if the data in the csv is a direct copy of data that is loaded into OwnershipPercentageValues table
Given I have historical data to create analytical models for "OwnershipPercentageValues"
When I upload the "OwnershipPercentageValues" csv file into the blob
And I initiate the data load process
Then source system should be "CSV"
And I verified in "OwnershipPercentageValues" table whether data is properly loaded from csv

@testcase=41372 @bvt
Scenario: Verify that old records are not deleted and records will be updated if there any changes in the old records when ADF pipeline is successfully completed
Given I have historical data to create analytical models
When I have modified data in "OwnershipPercentageValues" csv
And I upload the "OwnershipPercentageValues" csv file into the blob
And I initiate the data load process
Then I verified in "OwnershipPercentageValues" table whether data is properly updated
And source system should be "CSV"
And loaddate should be current date or loaded date in the "OwnershipPercentageValues" table

@testcase=41373 
Scenario: Verify that old records are not deleted and new records are not loaded when ADF pipeline is failed at copy level
Given I have historical data to create analytical models
When I have information about old records in "OwnershipPercentageValues" table
And I upload the invalid data "OwnershipPercentageValues" csv file into the blob
And I initiate the data load process
Then old records are not deleted from "OwnershipPercentageValues" table
And new records are not loaded when ADF pipeline is failed at copy level into "OwnershipPercentageValues"
