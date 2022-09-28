@sharedsteps=16581 @ui @testplan=35673 @testsuite=35680 @owner=jagudelos @parallel=false
Feature: OwnershipCalculationExecutedDailyToThatFICODetermineOwnership
In order to FICO can determine ownership
As a Professional User
I need the ownership calculation process to be executed daily

Background: Login
Given I am logged in as "profesional"

@testcase=37387 @version=2 @bvt1.5
Scenario: Validate the ticket creation
Given I have "MovementsWithOwnership" created
When I generate the operational cutoff
And I navigate to "OwnershipDeterminationBySegmnet" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
And I verify all validations passed
And I click on "ownershipCalulationValidations" "Submit" "button"
And I click on "ownershipCalculationConfirmation" "Submit" "button"
Then validate tickets generated for "4" days in "tickets" "grid" of the "segment"
When I click on "tickets" "viewSummary" "link" for the day '1'
Then validate "TicketId", "InitialDate" and "FinalDate" for the day "1" of the "segment"
And I click on "Return" "button"
When I click on "tickets" "viewSummary" "link" for the day '2'
Then validate "TicketId", "InitialDate" and "FinalDate" for the day "2" of the "segment"
And I click on "Return" "button"
When I click on "tickets" "viewSummary" "link" for the day '3'
Then validate "TicketId", "InitialDate" and "FinalDate" for the day "3" of the "segment"
And I click on "Return" "button"
When I click on "tickets" "viewSummary" "link" for the day '4'
Then validate "TicketId", "InitialDate" and "FinalDate" for the day "4" of the "segment"
And I click on "Return" "button"

@testcase=37388 @version=2
Scenario Outline: Input movements with ownership from the source Excel
Given I have "MovementsWithOwnership" created from the source '<Source>'
When I generate the operational cutoff
And I navigate to "OwnershipDeterminationBySegmnet" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
And I verify all validations passed
And I click on "ownershipCalulationValidations" "Submit" "button"
And I click on "ownershipCalculationConfirmation" "Submit" "button"
And validate tickets generated for "4" days in "tickets" "grid" of the "segment"
And validate tickets are processing day by day and ends in "Propiedad" state
Then validate the rule name as "Propiedad desde la Fuente"
And validate ticketnumber correspond to the ticketnumber generated for the destination node on the day

Examples:
| Source  |
| Excel   |
| SINOPER |
| SAP PO  |

@testcase=37389 @version=2
Scenario: Input movements with ownership defined by the analytical model
Given I have "MovementswithoutOwnership" created
When the ownership of the inputs from other segments is not calculated
And I have Transfer points for that segment
And I generate the operational cutoff
And I navigate to "OwnershipDeterminationBySegmnet" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
And I verify all validations passed
And I click on "ownershipCalulationValidations" "Submit" "button"
And I click on "ownershipCalculationConfirmation" "Submit" "button"
And validate tickets generated for "4" days in "tickets" "grid" of the "segment"
And validate tickets are processing day by day and ends in "Propiedad" state
Then validate the rule name applied must be the ownership strategy configured for the connection
And validate ticketnumber correspond to the ticketnumber generated for the destination node on the day

@testcase=37390 @version=2
Scenario: Input movements with default ownership
Given I have "NodesWithOwnershipButMovementswithoutOwnership" created
When I generate the operational cutoff
And I navigate to "OwnershipDeterminationBySegmnet" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
And I verify all validations passed
And I click on "ownershipCalulationValidations" "Submit" "button"
And I click on "ownershipCalculationConfirmation" "Submit" "button"
And validate tickets generated for "4" days in "tickets" "grid" of the "segment"
And validate tickets are processing day by day and ends in "Propiedad" state
Then validate the rule name as "Propiedad por defecto"
And validate ticketnumber correspond to the ticketnumber generated for the destination node on the day

@testcase=37391 @version=2 @bvt1.5
Scenario: Validate ticket of the period in failed state
Given I have "MovementsandNodesWithoutOwnership" created
When I generate the operational cutoff
And I navigate to "OwnershipDeterminationBySegmnet" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
And I verify all validations passed
And I click on "ownershipCalulationValidations" "Submit" "button"
And I click on "ownershipCalculationConfirmation" "Submit" "button"
And validate tickets generated for "4" days in "tickets" "grid" of the "segment"
And validate first ticket in "Fallido" state
And validate subsequent tickets are processing day by day and ends in "Fallido" state
Then validate the error message "Error al calcular la propiedad del tiquete [ticket number] en la fecha [date of the failed period day]"
And validate ticketnumber correspond to the ticketnumber generated for the destination node on the day are in "Fallido" state
And validate the ticket registration failed in BlockChain
