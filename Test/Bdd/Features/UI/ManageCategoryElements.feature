@sharedsteps=4013 @owner=jagudelos @ui @testplan=8481 @testsuite=8485 @ThursRun
Feature: ManageCategoryElements
In order to handle the Transport Network
As an application administrator
I want to manage Transport Category Elements

Background: Login
	Given I am logged in as "admin"

@testcase=9254 @bvt @version=2 @prodready
Scenario: Create Category Element with valid data
	When I navigate to "Category Elements" page
	And I click on "Create Element" "button"
	Then I should see "Create Element" interface
	When I select value from "Category" dropdown
	And I provide value for "element" "name" "textbox"
	And I provide value for "element" "description" "textarea"
	And I click on "element" "submit" "button"
	Then the "category element" should be saved and showed in the list

@testcase=9255
Scenario: Verify Cancel functionality for Create Category Element Interface
	When I navigate to "Category Elements" page
	And I click on "Create Element" "button"
	Then I should see "Create Element" interface
	When I click on "element" "cancel" "button"
	Then the popup should be closed

@testcase=9256 @version=3 @prodready
Scenario: Create Category Element without name
	When I navigate to "Category Elements" page
	And I click on "Create Element" "button"
	Then I should see "Create Element" interface
	When I select value from "Category" dropdown
	And I don't provide value for "element" "name" "textbox"
	And I click on "element" "submit" "button"
	Then I should see error message "Requerido"

@testcase=9257 @version=2 @prodready
Scenario: Create Category Element without category
	When I navigate to "Category Elements" page
	And I click on "Create Element" "button"
	Then I should see "Create Element" interface
	When I provide value for "element" "name" "textbox"
	And I click on "element" "submit" "button"
	Then I should see the message "Requerido"

@testcase=9258 @version=2 @prodready
Scenario: Create Category Element without Description field
	When I navigate to "Category Elements" page
	And I click on "Create Element" "button"
	Then I should see "Create Element" interface
	When I select value from "Category" dropdown
	And I provide value for "element" "name" "textbox"
	And I don't provide value for "element" "description" "textarea"
	And I click on "element" "submit" "button"
	Then the "category element" should be saved and showed in the list

@testcase=9259 @prodready
Scenario Outline: Create Category Element with values exceeding maximum limit
	When I navigate to "Category Elements" page
	And I click on "Create Element" "button"
	Then I should see "Create Element" interface
	When I provide value for "element" "<Field>" "<Control>" that exceeds "<Limit>" characters
	Then I should see error message "<Message>"

	Examples:
		| Field       | Limit | Control  | Message                                              |
		| name        | 150   | textbox  | El Nombre puede contener máximo 150 caracteres       |
		| description | 1000  | textarea | La Descripción puede contener máximo 1000 caracteres |

@testcase=9260 @prodready
Scenario: Create Category Element with Name that contains special characters other than ":", "_", "-"
	When I navigate to "Category Elements" page
	And I click on "Create Element" "button"
	Then I should see "Create Element" interface
	When I provide value for "element" "name" "textbox" that contains special characters other than expected
	Then I should see error message "El Nombre solo admite números, letras, espacios y los siguientes caracteres especiales: ":", "_", "-""

@testcase=9261 @prodready
Scenario: Create Category Element with existing name
	When I navigate to "Category Elements" page
	And I click on "Create Element" "button"
	Then I should see "Create Element" interface
	When I select existing value from "Category"
	And I provide existing value for "element" "name" "textbox"
	And I click on "element" "submit" "button"
	Then I should see error message "El nombre del elemento ya existe"

@testcase=9262 @bvt @version=3 @prodready
Scenario: Edit Category Element with valid data
	When I navigate to "Category Elements" page
	And I search for existing "CategoryElement" record using filter
	And I click on "elements" "edit" "link" of any record
	Then I should see "Edit Element" interface
	When I update value for "element" "name" "textbox"
	And I click on "element" "submit" "button"
	Then the "category element" should be updated in the list

@testcase=9263 @version=3
Scenario: Verify Cancel functionality for Edit Element Interface
	When I navigate to "Category Elements" page
	And I search for existing "CategoryElement" record using filter
	And I click on "elements" "edit" "link" of any record
	Then I should see "Edit Element" interface
	When I click on "element" "cancel" "button"
	Then the popup should be closed

@testcase=9264 @version=4 @prodready
Scenario: Edit Category Element without name
	When I navigate to "Category Elements" page
	And I search for existing "CategoryElement" record using filter
	And I click on "elements" "edit" "link" of any record
	Then I should see "Edit Element" interface
	When I remove value for "element" "name" "textbox"
	And I click on "element" "submit" "button"
	Then I should see error message "Requerido"

@testcase=9265 @version=3 @prodready
Scenario: Edit Category Element without Description field
	When I navigate to "Category Elements" page
	And I search for existing "CategoryElement" record using filter
	And I click on "elements" "edit" "link" of any record
	Then I should see "Edit Element" interface
	When I remove value for "element" "description" "textarea"
	And I click on "element" "submit" "button"
	Then the "category element" should be updated in the list

@testcase=9266 @version=3 @prodready
Scenario Outline: Edit Category Element with values exceeding maximum limit
	When I navigate to "Category Elements" page
	And I search for existing "CategoryElement" record using filter
	And I click on "elements" "edit" "link" of any record
	Then I should see "Edit Element" interface
	When I update value for "element" "<Field>" "<Control>" that exceeds "<Limit>" characters
	And I click on "element" "submit" "button"
	Then I should see error message "<Message>"

	Examples:
		| Field       | Limit | Control  | Message                                              |
		| name        | 150   | textbox  | El Nombre puede contener máximo 150 caracteres       |
		| description | 1000  | textarea | La Descripción puede contener máximo 1000 caracteres |

@testcase=9267 @version=3 @prodready
Scenario: Edit Category Element with Name that contains special characters other than ":", "_", "-"
	When I navigate to "Category Elements" page
	And I search for existing "CategoryElement" record using filter
	And I click on "elements" "edit" "link" of any record
	Then I should see "Edit Element" interface
	When I update value for "element" "name" "textbox" that contains special characters other than expected
	And I click on "element" "submit" "button"
	Then I should see error message "El Nombre solo admite números, letras, espacios y los siguientes caracteres especiales: ":", "_", "-""

@testcase=9268 @version=3 @prodready
Scenario: Edit Category Element with existing name
	When I navigate to "Category Elements" page
	And I search for existing "CategoryElement" record using filter
	And I click on "elements" "edit" "link" of any record
	Then I should see "Edit Element" interface
	When I select existing value from "Category"
	When I update existing value for "element" "name" "textbox"
	And I click on "element" "submit" "button"
	Then I should see error message "El nombre del elemento ya existe"

@testcase=9269 @version=3 @prodready
Scenario Outline: Verify Filters functionality
	When I navigate to "Category Elements" page
	And I provide value for "elements" "<Field>" "<ControlType>" filter
	Then I should see the information that matches the data entered for the "<Field>"

	Examples:
		| Field       | ControlType |
		| Name        | textbox     |
		| Description | textbox     |
		| CreatedDate | date        |
		| IsActive    | combobox    |

@testcase=9270 @version=4 @prodready
Scenario Outline: Verify Filters functionality when no records found
	When I navigate to "Category Elements" page
	And I provide value for "elements" "<Field>" "<ControlType>" filter that doesn't matches with any record
	Then I should see message "Sin registros"

	Examples:
		| Field       | ControlType |
		| Name        | textbox     |
		| Description | textbox     |
		| CreatedDate | date        |

@testcase=9271 @version=2 @prodready
Scenario: Verify Category Filter functionality
	When I navigate to "Category Elements" page
	And I provide value for "elements" "category" "name" "textbox" filter
	Then I should see the information that matches the data entered for the "Category"

@testcase=9272 @version=3 @prodready
Scenario: Verify Category Filter functionality when no records found
	When I navigate to "Category Elements" page
	And I provide value for "elements" "category" "name" "textbox" filter that doesn't matches with any record
	Then I should see message "Sin registros"

@testcase=9273 @version=3 @prodready
Scenario Outline: Verify Sorting functionality
	When I navigate to "Category Elements" page
	And I click on the "<ColumnName>"
	Then the results should be sorted according to "<ColumnName>"

	Examples:
		| ColumnName  |
		| Name        |
		| Description |
		| CreatedDate |
		| Category    |
		| State       |