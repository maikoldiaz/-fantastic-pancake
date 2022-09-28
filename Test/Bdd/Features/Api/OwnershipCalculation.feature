@sharedsteps=12576 @owner=jagudelos @testplan=19772 @testsuite=19779
Feature: OwnershipCalculation
As a TRUE system, need to invoke the historical-based ownership calculation analytical model
according to the point of transfer configuration

@testcase=21127 @bvt
Scenario: Verify user can calculate ownership analytical model as per point of transfer
Given I want to create an "ownershipdetails" in the system
When I provide the required attributes
Then response should be successful












