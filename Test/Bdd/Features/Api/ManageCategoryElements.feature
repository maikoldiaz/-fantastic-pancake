@sharedsteps=7539 @owner=jagudelos @api  @testplan=3993 @testsuite=4023
Feature: ManageCategoryElements
In order to handle the Category
As an application administrator
I want to create Category Elements

Background: Login
	Given I am authenticated as "admin"

@testcase=4535 @api @output=QueryAll(GetCategoryElements) @bvt @version=2 @prodready
Scenario: Create Category Element with valid data
	Given I want to create a "Category Element" in the system
	When I provide the required fields
	Then the response should succeed with message "Elemento creado con éxito"

@testcase=4536 @api @output=QueryAll(GetCategoryElements) @version=2 @prodready
Scenario: Create valid Category Element without Description field
	Given I want to create a "Category Element" in the system
	When I don't provide "Description"
	Then the response should succeed with message "Elemento creado con éxito"

@testcase=4537 @api @output=QueryAll(GetCategoryElements) @bvt @version=3 @prodready
Scenario: Create valid Category Element without Name field
	Given I want to create a "Category Element" in the system
	When I don't provide "Name"
	Then the response should fail with message "El Nombre es obligatorio"

@testcase=4538 @api @output=QueryAll(GetCategoryElements) @version=2 @prodready
Scenario: Create valid Category Element without Category Id field
	Given I want to create a "Category Element" in the system
	When I don't provide "CategoryId"
	Then the response should fail with message "La Categoría es obligatoria"

@testcase=4539 @api @output=QueryAll(GetCategoryElements) @prodready
Scenario: Create Category Element without any values
	Given I want to create a "Category Element" in the system
	When I don't provide any values
	Then the response should fail with messages "El Nombre es obligatorio", "Se requiere el estado del elemento", "La Categoría es obligatoria"

@testcase=4540 @api @output=QueryAll(GetCategoryElements) @version=2 @prodready
Scenario: Create Category Element with existing Name
	Given I want to create a "Category Element" in the system
	When I provide an existing "Name"
	Then the response should fail with message "El nombre del elemento ya existe"

@testcase=4541 @api @output=QueryAll(GetCategoryElements) @prodready
Scenario: Create Category Element with Name that contains special characters other than ":", "_", "-"
	Given I want to create a "Category Element" in the system
	When I provide "Name" that contains special characters other than expected
	Then the response should fail with message "El Nombre solo admite números, letras, espacios y los siguientes caracteres especiales: ':' '_' '-'"

@testcase=4542 @api @output=QueryAll(GetCategoryElements) @version=2 @prodready
Scenario: Create Category Element with Name that exceeds 150 characters
	Given I want to create a "Category Element" in the system
	When I provide "Name" that exceeds 150 characters
	Then the response should fail with message "El Nombre puede contener máximo 150 caracteres"

@testcase=4543 @api @output=QueryAll(GetCategoryElements) @version=2 @prodready
Scenario: Create Category Element with Description that exceeds 1000 characters
	Given I want to create a "Category Element" in the system
	When I provide "Description" that exceeds 1000 characters
	Then the response should fail with message "La descripción puede contener máximo 1000 caracteres"

@testcase=4544 @api @output=QueryAll(GetCategoryElements) @bvt @version=3 @prodready
Scenario: Get all Category Elements
	Given I have "Category Elements" in the system
	When I Get all records
	Then the response should return all valid records

@testcase=4545 @api @output=QueryAll(GetCategoryElements) @ignore
Scenario: Get all Category Elements When Element list is empty
	Given I don't have any "Category Element" in the system
	When I Get all records
	Then the response should fail with message "No existen Elementos registrados"

@testcase=4546 @api @output=QueryAll(GetCategoryElements) @bvt @version=3 @prodready
Scenario: Get Category Element by Id with valid Id
	Given I have "Category Elements" in the system
	When I Get record with valid Id
	Then the response should return requested record details

@testcase=4547 @api @output=QueryAll(GetCategoryElements) @prodready
Scenario: Get Category Element by Id with invalid Id
	Given I have "Category Elements" in the system
	When I Get record with invalid Id
	Then the response should fail with message "No existe el Elemento"

@testcase=4548 @api @output=QueryAll(GetActiveCategoryElements) @bvt @version=3 @prodready
Scenario: Get Active Category Elements where Active column is True
	Given there are "Category's Elements" where Active column is True in the system
	When I Get records with Active field is True
	Then the response should return requested records where Active column is True

@testcase=4549 @api @output=QueryAll(GetActiveCategoryElements) @version=2 @ignore
Scenario: Validate when I don't have any Active Category Element
	Given I don't have any Active "Category's Elements" in the system
	When I Get records with Active field is True
	Then the response should fail with message "No existen Elementos asociados a la categoría"

@testcase=4550 @api @output=QueryAll(GetCategoryElements) @bvt @version=2 @prodready
Scenario: Edit Category Element with valid data
	Given I have "Category Element" in the system
	When I update a record with required fields
	Then the response should succeed with message "Elemento modificado con éxito"

@testcase=4551 @api @output=QueryAll(GetCategoryElements) @version=3 @prodready
Scenario: Edit Category Element without Description field
	Given I have "Category Element" in the system
	When I update a record without "Description"
	Then the response should succeed with message "Elemento modificado con éxito"

@testcase=4552 @api @output=QueryAll(GetCategoryElements) @version=3 @prodready
Scenario: Edit Category Element without Name field
	Given I have "Category Element" in the system
	When I update a record without "Name"
	Then the response should fail with message "El Nombre es obligatorio"

@testcase=4553 @api @output=QueryAll(GetCategoryElements) @version=3 @prodready
Scenario: Edit Category Element with existing Name
	Given I have "Category Element" in the system
	When I update a record with existing "Name"
	Then the response should fail with message "El nombre del elemento ya existe"

@testcase=4554 @api @output=QueryAll(GetCategoryElements) @version=3 @prodready
Scenario: Edit Category Element with Name that exceeds 150 characters
	Given I have "Category Element" in the system
	When I update a record with "Name" that exceeds 150 characters
	Then the response should fail with message "El Nombre puede contener máximo 150 caracteres"

@testcase=4555 @api @output=QueryAll(GetCategoryElements) @version=3 @prodready
Scenario: Edit Category Element with Description that exceeds 1000 characters
	Given I have "Category Element" in the system
	When I update a record with "Description" that exceeds 1000 characters
	Then the response should fail with message "La descripción puede contener máximo 1000 caracteres"

@testcase=4556 @api @output=QueryAll(GetCategoryElements) @version=3 @prodready
Scenario: Edit Category Element with Name that contains special characters other than ";", "_", "-"
	Given I have "Category Element" in the system
	When I update a record with "Name" that contains special characters other than expected
	Then the response should fail with message "El Nombre solo admite números, letras, espacios y los siguientes caracteres especiales: ':' '_' '-'"