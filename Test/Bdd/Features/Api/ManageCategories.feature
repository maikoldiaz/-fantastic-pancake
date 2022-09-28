@sharedsteps=7539 @owner=jagudelos @api @testplan=3993 @testsuite=4022
Feature: ManageCategories
In order to handle the Transport Network
As an application administrator
I want to manage Transport Categories

Background: Login
	Given I am authenticated as "admin"

@testcase=4516 @api @output=QueryAll(GetCategories) @bvt @version=2 @prodready
Scenario: Get all Transport Categories
	Given I have "Transport Categories" in the system
	When I Get all records
	Then the response should return all valid records

@testcase=4517 @api @output=QueryAll(GetCategories) @version=3 @ignore
Scenario: Get all Transport Categories when the Transport Categories list is empty
	Given I don't have any "Transport Category" in the system
	When I Get all records
	Then the response should have the message "No existen Categorías registradas"

@testcase=4518 @api @output=QueryAll(GetCategories) @bvt @version=3 @prodready
Scenario: Get Transport Category by valid Id
	Given I have "Transport Category" in the system
	When I Get record with valid Id
	Then the response should return requested record details

@testcase=4519 @api @output=QueryAll(GetCategories) @version=2 @prodready
Scenario: Get Transport Category by invalid Id
	Given I have "Transport Category" in the system
	When I Get record with invalid Id
	Then the response should fail with message "No existe la Categoría"

@testcase=4520 @api @output=QueryAll(GetCategories) @bvt @version=2 @prodready
Scenario: Create Transport Category with valid data
	Given I want to create a "Transport Category" in the system
	When I provide the required fields
	Then the response should succeed with message "Categoría creada con éxito"

@testcase=4521 @api @output=QueryAll(GetCategories) @version=2 @prodready
Scenario: Create valid Transport Category without Description field
	Given I want to create a "Transport Category" in the system
	When I don't provide "Description"
	Then the response should succeed with message "Categoría creada con éxito"

@testcase=4522 @api @output=QueryAll(GetCategories) @version=3 @prodready
Scenario: Create Transport Category without Category Name
	Given I want to create a "Transport Category" in the system
	When I don't provide "Name"
	Then the response should fail with message "El nombre es obligatorio"

@testcase=4523 @api @output=QueryAll(GetCategories) @version=3 @prodready
Scenario: Create Transport Category without any values
	Given I want to create a "Transport Category" in the system
	When I don't provide any values
	Then the response should fail with messages "El nombre es obligatorio", "Se requiere el estado de la categoría", "Se requiere un agrupador de categoría"

@testcase=4524 @api @output=QueryAll(GetCategories) @version=2 @prodready
Scenario: Create Transport Category with Category Name that exceeds 150 characters
	Given I want to create a "Transport Category" in the system
	When I provide "Name" that exceeds 150 characters
	Then the response should fail with message "El Nombre puede contener máximo 150 caracteres"

@testcase=4525 @api @output=QueryAll(GetCategories) @version=4 @prodready
Scenario: Create Transport Category with existing Category Name
	Given I want to create a "Transport Category" in the system
	When I provide an existing "Name"
	Then the response should fail with message "El nombre de la categoría ya existe"

@testcase=4526 @api @output=QueryAll(GetCategories) @version=4 @prodready
Scenario: Create Transport Category with Category Name that contains special characters other than ":", "_", "-"
	Given I want to create a "Transport Category" in the system
	When I provide "Name" that contains special characters other than expected
	Then the response should fail with message "El Nombre solo admite números, letras, espacios y los siguientes caracteres especiales: ':' '_' '-'"

@testcase=4527 @api @output=QueryAll(GetCategories) @version=3 @prodready
Scenario: Create Transport Category with Category Description that exceeds 1000 characters
	Given I want to create a "Transport Category" in the system
	When I provide "Description" that exceeds 1000 characters
	Then the response should fail with message "La descripción puede contener máximo 1000 caracteres"

@testcase=4528 @api @output=QueryAll(GetCategories) @bvt @version=3 @prodready
Scenario: Update Transport Category with valid data
	Given I have "Transport Category" in the system
	When I update a record with required fields
	Then the response should succeed with message "Categoría modificada con éxito"

@testcase=4529 @api @output=QueryAll(GetCategories) @version=4 @prodready
Scenario: Update Transport Category without Category Name
	Given I have "Transport Category" in the system
	When I update a record without "Name"
	Then the response should fail with message "El nombre es obligatorio"

@testcase=4530 @api @output=QueryAll(GetCategories) @version=3 @prodready
Scenario: Update Transport Category with Category Name that exceeds 150 characters
	Given I have "Transport Category" in the system
	When I update a record with "Name" that exceeds 150 characters
	Then the response should fail with message "El Nombre puede contener máximo 150 caracteres"

@testcase=4531 @api @output=QueryAll(GetCategories) @version=4 @prodready
Scenario: Update Transport Category with existing Category Name
	Given I have "Transport Category" in the system
	When I update a record with existing "Name"
	Then the response should fail with message "El nombre de la categoría ya existe"

@testcase=4532 @api @output=QueryAll(GetCategories) @version=4 @prodready
Scenario: Update Transport Category with Category Name that contains special characters other than ":", "_", "-"
	Given I have "Transport Category" in the system
	When I update a record with "Name" that contains special characters other than expected
	Then the response should fail with message "El Nombre solo admite números, letras, espacios y los siguientes caracteres especiales: ':' '_' '-'"

@testcase=4533 @api @output=QueryAll(GetCategories) @version=4 @prodready
Scenario: Update Transport Category with Category Description that exceeds 1000 characters
	Given I have "Transport Category" in the system
	When I update a record with "Description" that exceeds 1000 characters
	Then the response should fail with message "La descripción puede contener máximo 1000 caracteres"