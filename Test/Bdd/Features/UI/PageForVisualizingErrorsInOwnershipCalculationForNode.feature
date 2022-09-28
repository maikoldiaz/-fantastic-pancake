@sharedsteps=16681 @owner=jagudelos @ui @testplan=14709 @testsuite=14712
Feature: PageForVisualizingErrorsInOwnershipCalculationForNode
Give that as a professional user of the segment balance, I require
tracking ownership calculation errors in a node

Background: Login
	Given  I am logged in as "Professional Segment Balance User"

@testcase=16682 @ui @manual
Scenario Outline: Verify the grid page for the errors
Given I initiate the ownershipcalculation for a segment
When  I open up the calculation page
Then i click on the "error" "icon"
And I verify if all "<columns>" are present

	Examples:
		| columns             |
		| Operation ID        |
		| Classification      |
		| Operation           |
		| Date (Operational)  |
		| Execution Date      |
		| Segment             |
		| Net Volume          |
		| Source Product      |
		| Destination Product |
		| Error Description   |

@testcase=16683 @ui @manual
Scenario Outline: verify the error record for a node when classification is "Inventory"
Given I initiate the ownershipcalculation for a segment
When  I open up the calculation page
And i click on the "error" "icon"
Then I verify if all "<columns>" are present
And I validate all the values for the record

	Examples:
		| columns            |
		| Operation ID       |
		| Classification     |
		| Operation          |
		| Date (Operational) |
		| Execution Date     |
		| Segment            |
		| Net Volume         |
		| Source Product     |
		| Error Description  |

@testcase=16684 @ui @manual
Scenario Outline: Verify the error record for a node when classification is "Movement"
Given I initiate the ownership calculation for a segment
When I open up the calculation page
Then i click on the "error" "icon"
And I verify if all "<columns>" are present
And I validate all the values for the record

	Examples:
		| columns             |
		| Operation ID        |
		| Classification      |
		| Operation           |
		| Date (Operational)  |
		| Execution Date      |
		| Segment             |
		| Net Volume          |
		| Source Product      |
		| Destination Product |
		| Error Description   |

@testcase=16685 @ui @manual
Scenario: Verify the values within the grid page for any particular node
	Given I initiate the ownership calculation for a segment
	When I have errors in ownership calculation for a node
	And I open up the error list page
	Then i click on the "error" "icon"
	And I verify the data displayed in the record for that particular node

@testcase=16686 @ui @manual
Scenario: verify all the errors are displayed for a particular node
	Given I initiate the ownership calculation for a segment
	When I have errors in ownership calculation for a node
	And I open up the error list page
	Then i click on the "error" "icon"
	And I verify if all errors are displayed for that particular node

@testcase=16687 @manual @ui
Scenario Outline: Verify all columns are displayed when classification is "Movement"
	Given I initiate the ownership calculation for a segment
	When I have errors in ownership calculation for a node
	And I open up the calculation page
	Then i click on the "error" "icon"
	And I verify if all "<columns>" are present
	Examples:
		| columns             |
		| Operation ID        |
		| Classification      |
		| Operation           |
		| Date (Operational)  |
		| Execution Date      |
		| Segment             |
		| Net Volume          |
		| Source Product      |
		| Destination Product |
		| Error Description   |

@ui @manual @testcase=16688
Scenario Outline: Verify the required columns are displayed when the classification is "Inventory"
Given I initiate the ownership calculation for a segment
When I have errors in ownership calculation for a node
And I open up the calculation page
Then i click on the "error" "icon"
And I verify if all "<columns>" are present

	Examples:
		| columns            |
		| Operation ID       |
		| Classification     |
		| Operation          |
		| Date (Operational) |
		| Execution Date     |
		| Segment            |
		| Net Volume         |
		| Source Product     |
		| Error Description  |