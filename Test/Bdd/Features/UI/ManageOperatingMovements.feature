@sharedsteps=52365 @owner=jagudelos @ui @MVP2and3 @testsuite=49472 @testplan=49466
Feature: ManageOperatingMovements
In order to use operating movements by the time series analytical models
As an application administrator
I want to execute calculation of ownership and percentage ownership calculation

Background: Login
	Given I am logged in as "admin"

@testcase=52366 @manual
Scenario: Verify Operative Movement with Ownership value
	Given I want to execute calculation of ownership with different product, different logistics center and different Storage Location
	And Annulment is "No"
	Then I should see Operative Movemente with Ownership value is "Tr. Material a material"

@testcase=52367 @manual
Scenario: Verify Operative Movement with Ownership value with same Storage Location
	Given I want to execute calculation of ownership with different product, different logistics center and same Storage Location
	And Annulment is "No"
	Then I should see Operative Movemente with Ownership value is "Tr. Material a material"

@testcase=52368 @manual
Scenario: Verify Operative Movement with Ownership value with same logistics center
	Given I want to execute calculation of ownership with different product, same logistics center and different Storage Location
	And Annulment is "No"
	Then I should see Operative Movemente with Ownership value is "Tr. Material a material"

@testcase=52369 @manual
Scenario: Verify Operative Movement with Ownership value with same logistics center and Storage Location
	Given I want to execute calculation of ownership with different product, same logistics center and same Storage Location
	And Annulment is "No"
	Then I should see Operative Movemente with Ownership value is "Tr. Material a material"

@testcase=52370 @manual
Scenario: Verify Operative Movement with Ownership value with same product
	Given I want to execute calculation of ownership with same product, different logistics center and different Storage Location
	And Annulment is "No"
	Then I should see Operative Movemente with Ownership value is "Tr.trasladar ce a ce"

@testcase=52371 @manual
Scenario: Verify Operative Movement with Ownership value with same product and Storage Location
	Given I want to execute calculation of ownership with same product, different logistics center and same Storage Location
	And Annulment is "No"
	Then I should see Operative Movemente with Ownership value is "Tr.trasladar ce a ce"

@testcase=52372 @manual
Scenario: Verify Operative Movement with Ownership value with same product and logistics center
	Given I want to execute calculation of ownership with same product, same logistics center and different Storage Location
	And Annulment is "No"
	Then I should see Operative Movemente with Ownership value is "Tr. Almacen a Almacen"

@testcase=52373 @manual
Scenario: Verify Operative Movement with Ownership value with Annulment
	Given I want to execute calculation of ownership with different product, different logistics center and different Storage Location
	And Annulment is "Yes"
	Then I should see Operative Movemente with Ownership value is "Anul. Tr. Material a material"

@testcase=52374 @manual
Scenario: Verify Operative Movement with Ownership value with Annulment and Storage Location
	Given I want to execute calculation of ownership with different product, different logistics center and same Storage Location
	And Annulment is "Yes"
	Then I should see Operative Movemente with Ownership value is "Anul. Tr. Material a material"

@testcase=52375 @manual
Scenario: Verify Operative Movement with Ownership value with Annulment and logistics center
	Given I want to execute calculation of ownership with different product, same logistics center and different Storage Location
	And Annulment is "Yes"
	Then I should see Operative Movemente with Ownership value is "Anul. Tr. Material a material"

@testcase=52376 @manual
Scenario: Verify Operative Movement with Ownership value with Annulment, logistics center and Storage Location
	Given I want to execute calculation of ownership with different product, same logistics center and same Storage Location
	And Annulment is "Yes"
	Then I should see Operative Movemente with Ownership value is "Anul. Tr. Material a material"

@testcase=52377 @manual
Scenario: Verify Operative Movement with Ownership value with Annulment and product
	Given I want to execute calculation of ownership with same product, different logistics center and different Storage Location
	And Annulment is "Yes"
	Then I should see Operative Movemente with Ownership value is "Anul. Tr.trasladar ce a ce"

@testcase=52378 @manual
Scenario: Verify Operative Movement with Ownership value with Annulment, product and Storage Location
	Given I want to execute calculation of ownership with same product, different logistics center and same Storage Location
	And Annulment is "Yes"
	Then I should see Operative Movemente with Ownership value is "Anul. Tr.trasladar ce a ce"

@testcase=52379 @manual
Scenario: Verify Operative Movement with Ownership value with Annulment, product and logistics center
	Given I want to execute calculation of ownership with same product, same logistics center and different Storage Location
	And Annulment is "Yes"
	Then I should see Operative Movemente with Ownership value is "Anul. Tr. Almacen a Almacen"

@testcase=52380 @version=1 @manual
Scenario: Verify Invalid Operative Movement with Ownership value with Annulment, product, logistics center and Storage Location
	Given I want to execute calculation of ownership with same product, same logistics center and same Storage Location
	And Annulment is "Yes"
	Then I should not see Operative Movemente with Ownership value as "INVALID_COMBINATION_TO_SIV_MOVEMENT"

@testcase=52381 @version=1 @manual
Scenario: Verify Invalid Operative Movement with Ownership value with product, logistics center and Storage Location
	Given I want to execute calculation of ownership with same product, same logistics center and same Storage Location
	And Annulment is "No"
	Then I should not see Operative Movemente with Ownership value as "INVALID_COMBINATION_TO_SIV_MOVEMENT"

@testcase=52382 @version=1 @manual
Scenario: Verify ownership calculation process ends without error
	Given I want to execute calculation of ownership
	When I get status as "Success"
	Then Validate the insertion or update of the operational movements by node transfer point and operational date related to the approval of the node
	 
@testcase=52383 @version=1 @manual
Scenario: Verify ownership calculation process ends with error
	Given I want to execute calculation of ownership
	When I get status as "Failure"
	Then validate the operational movements per node transfer point and operational date related to the approval of the node have not been inserted or partially updated

@testcase=52384 @version=1 @manual
Scenario: Verify ownership calculation process ends without error and percentage ownership calculation have been executed with errors
	Given I want to execute calculation of ownership
	When I get status as "Success"
	And I get status as "Failure" while percentage ownership calculation
	Then Save in TRUE load indicators for node and ticket a failed value

@testcase=52385 @version=1 @manual
Scenario: Verify ownership calculation process ends without error and percentage ownership calculation have been executed without errors
	Given I want to execute calculation of ownership
	When I get status as "Success"
	And I get status as "Success" while percentage ownership calculation
	Then Save to the TRUE load indicators for node and ticket a successfully value

@testcase=52386 @manual
Scenario: Verify Operative Movement with Ownership value when Annulment is "Yes"
	Given I want to execute calculation of ownership
	And Annulment is "Yes"
	Then sign must be changed to negative for the net volume of the ownership