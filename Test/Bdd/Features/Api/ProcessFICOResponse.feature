@sharedsteps=7539 @owner=jagudelos @api @testsuite=35675 @testplan=35673
Feature: ProcessFICOResponse
In order to calculate the ownership of movements and inventories
As a TRUE System
I need to consume the FICO service

Background: Login
Given I am authenticated as "admin"

@testcase=37274 @bvt @version=2
Scenario: Verify the functionality when the service returns status code other than 200
And I want to calculate the ownership of movements and inventories
When the service returns a status code other than 200
Then the status and error message of the segment ticket for the period day should be updated to "Failed" status
And nodes of the ticket should be updated to "Failed" status

@testcase=37275
Scenario: Verify the response time, error and audit steps logged in app insights
And I want to calculate the ownership of movements and inventories
When the service returns a status code other than 200
Then validate that the "response time" "error" "audit steps" are logged in app insights

@testcase=37276 @bvt @version=2
Scenario: Verify the functionality when there are errors in the both movements and inventories
And I want to calculate the ownership of movements and inventories
When the service returns data in "MovimientosErrores" and "InventariosErrores" collection
Then all the errors should be registered
And the status and error message of the segment ticket for the period day should be updated to "Failed" status
And the status of successful record should not be updated
And nodes of the ticket should be updated to "Failed" status

@testcase=37277 @version=2
Scenario: Verify the functionality when there are errors only in movements
And I want to calculate the ownership of movements and inventories
When the service returns data in "MovimientosErrores" collection
Then all the errors should be registered
And the status and error message of the segment ticket for the period day should be updated to "Failed" status
And the status of successful record should not be updated
And nodes of the ticket should be updated to "Failed" status

@testcase=37278 @version=2
Scenario: Verify the functionality when there are errors only in inventories
And I want to calculate the ownership of movements and inventories
When the service returns data in "InventariosErrores" collection
Then all the errors should be registered
And the status and error message of the segment ticket for the period day should be updated to "Failed" status
And the status of successful record should not be updated
And nodes of the ticket should be updated to "Failed" status

@testcase=37279 @bvt @version=2
Scenario: Verify the process of ownership calculation results when there are both movements and inventories
And I want to calculate the ownership of movements and inventories
When the service returns data in "ResultadoMovimientos" and "ResultadoInventarios" collection
Then the ownership of movements and inventories should be registered
And the status of the segment ticket for the period day should be updated to "Ownership" status
And nodes of the ticket should be updated to "Ownership" status

@testcase=37280 @version=2
Scenario: Verify the process of ownership calculation results when there are only movements
And I want to calculate the ownership of movements and inventories
When the service returns data in "ResultadoMovimientos" collection and there is no record for inventories
Then the ownership of movements should be registered
And the status of the segment ticket for the period day should be updated to "Ownership" status
And nodes of the ticket should be updated to "Ownership" status

@testcase=37281 @version=2
Scenario: Verify the process of ownership calculation results when there are only inventories
And I want to calculate the ownership of movements and inventories
When the service returns data in "ResultadoInventarios" collection and there is no record for movements
Then the ownership of inventories should be registered
And the status of the segment ticket for the period day should be updated to "Ownership" status
And nodes of the ticket should be updated to "Ownership" status

@testcase=37282 @version=2
Scenario: Verify the process of ownership calculation results when both movements and inventories are empty
And I want to calculate the ownership of movements and inventories
When the service does not return data in either "ResultadoMovimientos" or "ResultadoInventarios" collection
Then the status and error message of the segment ticket for the period day should be updated to "Failed" status
And nodes of the ticket should be updated to "Failed" status