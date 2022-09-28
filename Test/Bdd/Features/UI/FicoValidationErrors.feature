@sharedsteps=4013 @owner=jagudelos @testsuite=55109 @testplan=55104 @ui @S14 @MVP2and3
Feature: FicoValidationErrors
As a supply chain user, I require an Interface to view the errors sent in the validations applied by FICO

Background: Login
Given I am logged in as "admin"
@testcase=57955
Scenario: To verify business error sent in the validations applied by the FICO as a supply chain user
Given I have ticket with "business" error
When I navigate to "DeltaCalculation" page
And I click on error icon and see the error popup "Business" appear
And I should see Segmento as a section header
Then I see the error popup closed on click of Aceptar

@testcase=57956 
Scenario: To verify technical error sent in the validations applied by the FICO as a supply chain user
Given I have ticket with "Technical" error
When I navigate to "DeltaCalculation" page
And I click on error icon and see the error popup "Technical" appear
And I see the error message "Se presentó un error técnico y no es recuperable. Por favor intente de nuevo más tarde" in popup wizard
And I should see Segmento as a section header
Then I see the error popup closed on click of Aceptar
