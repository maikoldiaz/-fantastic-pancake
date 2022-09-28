@sharedsteps=4013 @owner=jagudelos @ui @testplan=6671 @testsuite=6698
Feature: Manage Transport Categories
In order to handle the Transport Network
As an application administrator
I want to manage Transport Categories

Background: Login
	Given I am logged in as "admin"

@testcase=7550 @bvt @ui @version=2 @prodready
Scenario: Create Transport Category with valid data
	When I navigate to "Category" page
	And I click on "Create Category" "button"
	Then I should see "Create Category" interface
	When I provide value for "category" "name" "textbox"
	And I provide value for "category" "description" "textarea"
	And I click on "category" "submit" "button"
	Then the "category" should be saved and showed in the list

@testcase=7551 @ui
Scenario: Verify Cancel functionality for Create Category Interface
	When I navigate to "Category" page
	And I click on "Create Category" "button"
	Then I should see "Create Category" interface
	When I click on "category" "cancel" "button"
	Then the popup should be closed

@testcase=7552 @ui @version=2 @prodready
Scenario: Create Transport Category without mandatory fields
	When I navigate to "Category" page
	And I click on "Create Category" "button"
	Then I should see "Create Category" interface
	When I don't provide value for "category" "name" "textbox"
	And I click on "category" "submit" "button"
	Then I should see error message "Requerido"

@testcase=7553 @ui @version=2 @prodready
Scenario: Create Transport Category without Description field
	When I navigate to "Category" page
	And I click on "Create Category" "button"
	Then I should see "Create Category" interface
	When I provide value for "category" "name" "textbox"
	And I don't provide value for "category" "description" "textarea"
	And I click on "category" "submit" "button"
	Then the "category" should be saved and showed in the list

@testcase=7554 @ui @prodready
Scenario Outline: Create Transport Category with values exceeding maximum limit
	When I navigate to "Category" page
	And I click on "Create Category" "button"
	Then I should see "Create Category" interface
	When I provide value for "category" "<Field>" "<Control>" that exceeds "<Limit>" characters
	Then I should see error message "<Message>"

	Examples:
		| Field       | Limit | Control  | Message                                              |
		| name        | 150   | textbox  | El Nombre puede contener máximo 150 caracteres       |
		| description | 1000  | textarea | La Descripción puede contener máximo 1000 caracteres |

@testcase=7555 @ui @version=3 @prodready
Scenario: Create Transport Category with Category Name that contains special characters other than ":", "_", "-"
	When I navigate to "Category" page
	And I click on "Create Category" "button"
	Then I should see "Create Category" interface
	When I provide value for "category" "name" "textbox" that contains special characters other than expected
	Then I should see error message "El Nombre solo admite números, letras, espacios y los siguientes caracteres especiales: ":", "_", "-""

@testcase=9250 @version=2 @prodready
Scenario: Create Transport Category with existing name
	When I navigate to "Category" page
	And I click on "Create Category" "button"
	Then I should see "Create Category" interface
	When I provide existing value for "category" "name" "textbox"
	And I click on "category" "submit" "button"
	Then I should see error message "El nombre de la categoría ya existe"

@testcase=7556 @bvt @ui @version=6 @prodready
Scenario: Edit Transport Category with valid data
	When I navigate to "Category" page
	And I search for existing "Category" record using filter
	And I click on "categories" "edit" "link" of any record
	Then I should see "Edit Category" interface
	When I update value for "category" "name" "textbox"
	And I click on "category" "submit" "button"
	Then the "category" should be updated in the list

@testcase=7557 @ui @version=5
Scenario: Verify Cancel functionality for Edit Category Interface
	When I navigate to "Category" page
	And I search for existing "Category" record using filter
	And I click on "categories" "edit" "link" of any record
	Then I should see "Edit Category" interface
	When I click on "category" "cancel" "button"
	Then the popup should be closed

@testcase=7558 @ui @version=6 @prodready
Scenario: Edit Transport Category without mandatory fields
	When I navigate to "Category" page
	And I search for existing "Category" record using filter
	And I click on "categories" "edit" "link" of any record
	Then I should see "Edit Category" interface
	When I remove value for "category" "name" "textbox"
	And I click on "category" "submit" "button"
	Then I should see error message "Requerido"

@testcase=7559 @ui @version=6 @prodready
Scenario: Edit Transport Category without Description field
	When I navigate to "Category" page
	And I search for existing "Category" record using filter
	And I click on "categories" "edit" "link" of any record
	Then I should see "Edit Category" interface
	When I remove value for "category" "description" "textarea"
	And I click on "category" "submit" "button"
	Then the "category" should be updated in the list

@testcase=7560 @ui @version=6 @prodready
Scenario Outline: Edit Transport Category with values exceeding maximum limit
	When I navigate to "Category" page
	And I search for existing "Category" record using filter
	And I click on "categories" "edit" "link" of any record
	Then I should see "Edit Category" interface
	When I update value for "category" "<Field>" "<Control>" that exceeds "<Limit>" characters
	Then I should see error message "<Message>"

	Examples:
		| Field       | Limit | Control  | Message                                              |
		| name        | 150   | textbox  | El Nombre puede contener máximo 150 caracteres       |
		| description | 1000  | textarea | La Descripción puede contener máximo 1000 caracteres |

@testcase=7561 @ui @version=5 @prodready
Scenario: Edit Transport Category with Category Name that contains special characters other than ":", "_", "-"
	When I navigate to "Category" page
	And I search for existing "Category" record using filter
	And I click on "categories" "edit" "link" of any record
	Then I should see "Edit Category" interface
	When I update value for "category" "name" "textbox" that contains special characters other than expected
	And I click on "category" "submit" "button"
	Then I should see error message "El Nombre solo admite números, letras, espacios y los siguientes caracteres especiales: ":", "_", "-""

@testcase=9251 @version=4 @prodready
Scenario: Edit Category with existing name
	When I navigate to "Category" page
	And I search for existing "Category" record using filter
	And I click on "categories" "edit" "link" of any record
	Then I should see "Edit Category" interface
	When I update existing value for "category" "name" "textbox"
	And I click on "category" "submit" "button"
	Then I should see error message "El nombre de la categoría ya existe"

@testcase=7562 @ui @version=4 @prodready
Scenario Outline: Verify Filters functionality
	When I navigate to "Category" page
	And I provide value for "categories" "<Field>" "<ControlType>" filter
	Then I should see the information that matches the data entered for the "<Field>"

	Examples:
		| Field       | ControlType |
		| Name        | textbox     |
		| Description | textbox     |
		| CreatedDate | date        |
		| IsActive    | combobox    |
		| IsGrouper   | combobox    |

@testcase=7563 @ui @version=4 @prodready
Scenario Outline: Verify Filters functionality when no records found
	When I navigate to "Category" page
	And I provide value for "categories" "<Field>" "<ControlType>" filter that doesn't matches with any record
	Then I should see message "Sin registros"

	Examples:
		| Field       | ControlType |
		| Name        | textbox     |
		| Description | textbox     |
		| CreatedDate | date        |