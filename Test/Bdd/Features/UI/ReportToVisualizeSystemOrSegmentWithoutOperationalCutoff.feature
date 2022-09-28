@owner=jagudelos @ui @testplan=26817 @testsuite=26837
Feature: ReportToVisualizeSystemOrSegmentWithoutOperationalCutoff
As a query user, I need to view the operational report by system
or segment without operational cutoff to verify the unbalance

@testcase=28271 @bvt @ui
Scenario: Verify As a TRUE Query user I am able to view reports by System for the selected period without operational cutoff with movements and inventories
Given I have ownership calculation data generated in the system
And I have ownership calculation data for system element
When I navigate to the report page
And I enter the required values
Then I verify that all movements and inventories uploaded are being displayed

@testcase=28272 @bvt @ui
Scenario: Verify As a TRUE Query user I am able to view reports by Segment for the selected period without operational cutoff with movement and inventories
Given I have ownership calculation data generated in the system
And I have ownership calculation data for system element
When I navigate to the report page
And I enter the required values
Then I verify that all movements and inventories uploaded are being displayed

@testcase=28273 @ui
Scenario: Verify As a TRUE Query user I am able to view reports by System for the selected period without operational cutoff without movements and inventories
Given I have ownership calculation data generated in the system
And I have ownership calculation data for system element
When I navigate to the report page
And I enter the required values
Then I verify that all fields are blank as no movements were uploaded


@testcase=28274 @ui
Scenario: Verify As a TRUE Query user I am able to view reports by Segment for the selected period without operational cutoff without movement and inventories
Given I have ownership calculation data generated in the system
And I have ownership calculation data for system element
When I navigate to the report page
And I enter the required values
Then I verify that all fields are blank as no movements were uploaded

@testcase=28275 @ui
Scenario: Verify As a TRUE Query user I am able to view reports for a node by System for the selected period without operational cutoff with movements and inventories
Given I have ownership calculation data generated in the system
And I have ownership calculation data for system element
When I navigate to the report page
And I enter the required values
Then I verify that all movements and inventories uploaded are being displayed

@testcase=28276 @ui
Scenario: Verify As a TRUE Query user I am able to view reports for a node by Segment for the selected period without operational cutoff with movement and inventories
Given I have ownership calculation data generated in the system
And I have ownership calculation data for system element
When I navigate to the report page
And I enter the required values
Then I verify that all movements and inventories uploaded are being displayed

@testcase=28277 @ui
Scenario: Verify As a TRUE Query user I am able to view reports for a node by System for the selected period without operational cutoff without movements and inventories
Given I have ownership calculation data generated in the system
And I have ownership calculation data for system element
When I navigate to the report page
And I enter the required values
Then I verify that all fields are blank as no movements were uploaded

@testcase=28278 @ui
Scenario: Verify As a TRUE Query user I am able to view reports for a node by Segment for the selected period without operational cutoff without movement and inventories
Given I have ownership calculation data generated in the system
And I have ownership calculation data for system element
When I navigate to the report page
And I enter the required values
Then I verify that all fields are blank as no movements were uploaded







