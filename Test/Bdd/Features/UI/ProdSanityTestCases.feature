@manual @ui @owner=jagudelos @ui @testplan=32700 @testsuite=32701
Feature: ProdSanityTestCases
In order to reinforce the build quality stated we are to run these basic test that verify build health

@testcase=32781
Scenario: Verify User is Able to Login As TRUE Admin
Given I am logged in as "admin"
When I am logged in as "admin"
Then I Verify i am logged in
@testcase=32782
Scenario: Verify User is Able to access navigational menus when Logged in As TRUE Admin
Given I am logged in as "administrador"
When I click on Administration tab
Then I should see "Category" tab
And I should see "Category elements" tab
And I should see "Configure group nodes" tab
And I should see "Configure attributes nodes" tab
And I should see "Configure connections attributes" tab
And I should see "Nodes" tab
And I should see "Homologation" tab
And I should see "Exceptions" tab
And I should see "" tab
When I click on conveyor balance with property menu
Then I should see "FileUpload" tab
And I should see "Operational Cutoff" tab
And I should see "Contract upload" tab
And I should see "Ownership Calculation" tab
And I should see "Ownership nodes" tab
And I should see "Reports" tab
And I should see "Analytical Model Prediction" tab
And I should see "Logistic Report Generation" tab
@testcase=32783
Scenario: Verify User is Able to Login As TRUE Professional
Given I am logged in as "professional"
When I am logged in as "professional"
Then I Verify i am logged in
@testcase=32784
Scenario: Verify User is Able to access navigational menus when Logged in As TRUE Professional
Given I am logged in as "professional"
When I click on Administration tab
Then I should see "Exceptions" tab
When I click on conveyor balance with property menu
Then I should see "FileUpload" tab
And I should see "Operational Cutoff" tab
And I should see "Contract upload" tab
And I should see "Ownership Calculation" tab
And I should see "Ownership nodes" tab
And I should see "Reports" tab
And I should see "Analytical Model Prediction" tab
And I should see "Logistic Report Generation" tab
@testcase=32785
Scenario: Verify I am able to logout of TRUE Admin
Given I am logged in as "professional"
When I click on Administration tab
When I click on Logout
Then I Verify that i am logged out
@testcase=32786
Scenario: Verify I am able to create Category Elements when logged in as TRUE Admin
When I navigate to "Category Elements" page
And I click on "Create Element" "button"
Then I should see "Create Element" interface
And I verify all seed data is present
@testcase=32787
Scenario: Verify I am able to create Categories when logged in as TRUE Admin
When I navigate to "Categories" page
And I click on "Create Category" "button"
Then I should see "Create Category" interface
And I verify all seed data is present
@testcase=32788
Scenario: Verify I am able to edit Category Elements when logged in as TRUE Admin
When I navigate to "Category Elements" page
And I click on "Edit Element" "button"
Then I should see "Edit Element" interface
@testcase=32789
Scenario: Verify I am able to edit Categories when logged in as TRUE Admin
When I navigate to "Categories" page
And I click on "Edit Category" "button"
Then I should see "Edit Category" interface
@testcase=32790
Scenario Outline: Verify Filters functionality for Category Element page
When I navigate to "Category Elements" page
And I provide value for "elements" "<Field>" "<ControlType>" filter
Then I should see the information that matches the data entered for the "<Field>"

Examples:
| Field       | ControlType |
| Name        | textbox     |
| Description | textbox     |
| CreatedDate | date        |
| IsActive    | combobox    |
@testcase=32791
Scenario Outline: Verify Filters functionality for Category page
When I navigate to "Categories" page
And I provide value for "elements" "<Field>" "<ControlType>" filter
Then I should see the information that matches the data entered for the "<Field>"

Examples:
| Field       | ControlType |
| Name        | textbox     |
| Description | textbox     |
| CreatedDate | date        |
| IsActive    | combobox    |
| Grouped     | textbox     |
@testcase=32792
Scenario Outline: Verify Sort functionality for category page
When I navigate to "Categories" page
And I click on "elements" "<Field>" "<ControlType>" filter
Then I should see the information that is sorted
And I click again on to sort in descending order

Examples:
| Field       | ControlType |
| Name        | textbox     |
| Description | textbox     |
| CreatedDate | date        |
| IsActive    | combobox    |
| Grouped     | textbox     |
@testcase=32793 
Scenario Outline: Verify Sort functionality for category element page
When I navigate to "Category Elements" page
And I click on "elements" "<Field>" "<ControlType>" filter
Then I should see the information that is sorted
And I click again on to sort in descending order

Examples:
| Field       | ControlType |
| Name        | textbox     |
| Description | textbox     |
| CreatedDate | date        |
| IsActive    | combobox    |
