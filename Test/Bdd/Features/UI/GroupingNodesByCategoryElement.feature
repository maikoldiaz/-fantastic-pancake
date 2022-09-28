@sharedsteps=4013 @owner=jagudelos @ui  @testsuite=6697 @testplan=6671
Feature: GroupingNodesByCategoryElement
In order to display information by Category Element
As an application administrator
I want to group Nodes by Category Element

Background: Login
	Given I am logged in as "admin"

@testcase=7606 @bvt @ui @version=3
Scenario: Create Grouping of Nodes in a ​Category Element
	And I create Nodes in the system
	When I navigate to "Grouping Categories" page
	And I click on "Search" "button"
	And I select any "Node Type" from "NodeTags" "category" "dropdown"
	And I select any "Value" from "NodeTags" "element" "dropdown"
	When I click on "NodeTags" "apply" "button"
	And I select required Nodes from "Nodes" "checkbox"
	And I select any "ChangeValue" from "GroupingTitle" "dropdown"
	When I select new "CategoryElement" from dropdown
	And I select required Date from "NodeTag" "operationDate" "datepicker"
	And I click on "NodeTag" "save" "button"
	Then I should see the new Nodes grouped based on the chosen Category Element

@testcase=7607 @bvt @output=QueryAll(GetNodes) @ui @version=3 @prodready
Scenario: Edit Grouping of Nodes in a ​Category Element
    And I create Nodes in the system
	When I navigate to "Grouping Categories" page
	And I click on "Search" "button"
	And I select any "Node Type" from "NodeTags" "category" "dropdown"
	And I select any "Value" from "NodeTags" "element" "dropdown"
	When I click on "NodeTags" "apply" "button"
	And I select required Nodes from "Nodes" "checkbox"
	And I select any "ChangeValue" from "GroupingTitle" "dropdown"
	And I select required Date from "NodeTag" "operationDate" "datepicker"
	And I click on "NodeTag" "save" "button"
	Then I should see the new Nodes grouped based on the chosen Category Element
	And the end date of new groupings should be maximum date

@testcase=7608 @bvt @ui
Scenario: Expire Grouping of Nodes in a ​Category Element
    And I create Nodes in the system
	When I navigate to "Grouping Categories" page
	And I click on "Search" "button"
	And I select any "Node Type" from "NodeTags" "category" "dropdown"
	And I select any "Value" from "NodeTags" "element" "dropdown"
	When I click on "NodeTags" "apply" "button"
	And I select required Nodes from "Nodes" "checkbox"
	And I select any "ExpireValue" from "GroupingTitle" "dropdown"
	And I select required Date from "NodeTag" "operationDate" "datepicker"
	And I click on "NodeTag" "save" "button"
	Then the life time range of Nodes to the Category Element should be changed

@testcase=7609 @ui
Scenario: List all Nodes grouping by Category Element
	Given I have "Nodes" in the system
	When I navigate to "GroupingCategories" page
	And I click on "Search" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "ApplyFilters" "button"
	Then I should see the list of all Nodes in the system
	And the checkbox should be preselected for the Nodes in the Category Element with current association

@testcase=7610 @ui
Scenario: Verify the end date of Nodes which are already associated to the Category Element
	Given I have "Nodes" in the system
	When I navigate to "GroupingCategories" page
	And I click on "Search" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "ApplyFilters" "button"
	Then I should see the list of all Nodes in the system
	And the end date should be indefinite for the Nodes in the Category Element with current association

@testcase=7611 @ui
Scenario: Verify that the Node selection is preselected and disabled for Nodes with existing association to the Category Element
	Given I have "Nodes" in the system
	When I navigate to "GroupingCategories" page
	And I click on "Search" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "ApplyFilters" "button"
	Then I should see the list of all Nodes in the system
	And the selection should be disabeld
	And I should not allow to uncheck

@testcase=7612 @ui
Scenario: Verify the effective dates of associated Nodes
	Given I have "Nodes" in the system
	When I navigate to "GroupingCategories" page
	And I click on "Search" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "ApplyFilters" "button"
	Then I should see the list of all Nodes in the system
	And I should see that the effective date of the associated nodes is equal or greater than the current date

@testcase=7613 @ui
Scenario Outline: List all Nodes grouping by Category Element using filter
	Given I have "Nodes" in the system
	When I navigate to "GroupingCategories" page
	And I click on "Search" "button"
	And I click on "<Filter_type>" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "ApplyFilters" "button"
	Then I should see the list of all Nodes in the system
	And the checkbox should be preselected for the Nodes in the Category Element in the first filter

	Examples:
		| Filter_type |
		| AddFilterO  |
		| AddFilterY  |

@testcase=7614 @ui
Scenario Outline: List all Nodes grouping by Category Element using mutiple filters
	Given I have "Nodes" in the system
	When I navigate to "GroupingCategories" page
	And I click on "Search" "button"
	And I click on "<Filter_type>" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "<Filter_type>" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "ApplyFilters" "button"
	Then I should see the list of all Nodes in the system
	And the checkbox should be preselected for the Nodes in the Category Element in the first filter

	Examples:
		| Filter_type1 | Filter_type2 |
		| AddFilterO   | AddFilterY   |
		| AddFilterY   | AddFilterO   |

@testcase=7615 @ui
Scenario Outline: Verify the end date of Nodes which are already associated to the Category Element using filter
	Given I have "Nodes" in the system
	When I navigate to "GroupingCategories" page
	And I click on "Search" "button"
	And I click on "<Filter_type>" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "ApplyFilters" "button"
	Then I should see the list of all Nodes in the system
	And the end date should be indefinite for the Nodes in the Category Element with current association

	Examples:
		| Filter_type |
		| AddFilterO  |
		| AddFilterY  |

@testcase=7616 @ui
Scenario Outline: Verify that the Node selection is preselected and disabled for Nodes with existing association to the Category Element using filter
	Given I have "Nodes" in the system
	When I navigate to "GroupingCategories" page
	And I click on "Search" "button"
	And I click on "<Filter_type>" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "ApplyFilters" "button"
	Then I should see the list of all Nodes in the system
	And the nodes that will be preselected and disabled for the match in the first filter

	Examples:
		| Filter_type |
		| AddFilterO  |
		| AddFilterY  |

@testcase=7617 @ui
Scenario Outline: Verify the effective dates of associated Nodes using filters
	Given I have "Nodes" in the system
	When I navigate to "GroupingCategories" page
	And I click on "Search" "button"
	And I click on "<Filter_type>" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "ApplyFilters" "button"
	Then I should see the list of all Nodes in the system
	And And I should see that the effective date of the associated nodes is equal or greater than the current date

	Examples:
		| Filter_type |
		| AddFilterO  |
		| AddFilterY  |

@testcase=7618 @ui
Scenario Outline: Verify the selection of new Nodes for grouping using filters
	Given I have "Nodes" in the system
	When I navigate to "GroupingCategories" page
	And I click on "Search" "button"
	And I click on "<Filter_type1>" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "<Filter_type2>" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "ApplyFilters" "button"
	Then I should see the list of all Nodes in the system
	And I should be able to select the new Nodes for grouping that match with second or third filter

	Examples:
		| Filter_type1 | Filter_type2 |
		| AddFilterO   | AddFilterY   |
		| AddFilterY   | AddFilterO   |

@testcase=7619 @ui
Scenario Outline: List all Nodes grouping by Category Element using filters to check the future due dates
	Given I have "Nodes" in the system
	When I navigate to "GroupingCategories" page
	And I click on "Search" "button"
	And I click on "<Filter_type>" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "ApplyFilters" "button"
	Then I should see the list of all Nodes in the system
	And I should see the future due dates for the Nodes

	Examples:
		| Filter_type |
		| AddFilterO  |
		| AddFilterY  |