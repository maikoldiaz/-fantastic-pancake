@sharedsteps=4013 @owner=jagudelos @ui @testsuite=14721 @testplan=14709
Feature: RowHighlightForTables
In order to increase readability
As a user
I need to have accessible components to be able to use the system

@testcase=16689 @ui @manual
Scenario: Category Module Row should highlight when I mousehover
	When I navigate to "CreateCategory" page
	And I mousehover on any row in "category" grid
	Then the row should highlight in color

@testcase=16690 @ui @manual
Scenario: Node Module Row should highlight when I mousehover
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	And I mousehover on any row in "node" grid
	Then the row should highlight in color

@testcase=16691 @ui @manual
Scenario: Category Element Module Row should highlight when I mousehover
	When I navigate to "Category Elements" page
	And I mousehover on any row in "CategoryElement" grid
	Then the row should highlight in color

@testcase=16692 @ui @manual
Scenario: Grouping Node Module Row should highlight when I mousehover
	When I navigate to "GroupingCategories" page
	And I click on "Search" "button"
	When I select value from "Category" "dropdown"
	And I select value from "Element" "dropdown"
	And I click on "ApplyFilters" "button"
	And I mousehover on any row in "CategoryElement" grid
	Then the row should highlight in color

@testcase=16693 @ui @manual
Scenario: Node Attribute Module Row should highlight when I mousehover
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "nodeAttributes" apply" "button" in filter
	And I mousehover on any row in "NodeAttribute" grid
	Then the row should highlight in color

@testcase=16694 @ui @manual
Scenario: Connection Attribute Module Row should highlight when I mousehover
	When I navigate to "ConfigureAttributesConnections" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "connAttributes" apply" "button" in filter
	And I mousehover on any row in "NodeAttribute" grid
	Then the row should highlight in color

@testcase=16695 @ui @manual
Scenario: Upload Excel Module Row should highlight when I mousehover
	When I navigate to "FileUpload" page
	And I mousehover on any row in "FileUploads" grid
	Then the row should highlight in color

@testcase=16696 @ui @manual
Scenario: Operational Cutoff Module Row should highlight when I mousehover
	When I navigate to "OperationalCutoff" page
	And I mousehover on any row in "Tickets" grid
	Then the row should highlight in color