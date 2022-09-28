@sharedsteps=16659 @owner=jagudelos @ui @testplan=14709 @testsuite=14711
Feature: PageForStartingTheOwnershipCalculationForASegment
As a Professional Segment Balance User,
I want an UI to start the ownership calculation
process for a segment

Background: Login
Given  I am logged in as "admin"

@testcase=16660 @ui @version=5 @prodready
Scenario: Verify TRUE user can start the ownership calculation for a segment
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
When I click on "InitTicket" "submit" "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
When I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
Then validate that "ErrorsGrid" "Submit" "button" as enabled
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
When I click on "unbalancesGrid" "submit" "button"
When I click on "ConfirmCutoff" "Submit" "button"
When I wait till cutoff ticket processing to complete
When I navigate to "Ownershipcalculation" page
And I click on "NewBalance" "button"
When I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
And I verify all validations passed
And I click on "ownershipCalulationValidations" "Submit" "button"
And I click on "ownershipCalculationConfirmation" "Submit" "button"
Then verify the ownership is calculated successfully

@testcase=16661 @ui @version=2
Scenario: Validate the ownership calculation for a segment is completed sucessfully
Given I have "ownershipnodes" created
When I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
And I verify all validations passed
And I click on "ownershipCalulationValidations" "Submit" "button"
Then verify the ownership is calculated successfully

@testcase=16662 @ui @version=2
Scenario: Verify Next button isn't enabled if any of the validations fail
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then Verify the next button is not enabled

@testcase=16663 @ui @version=2
Scenario: Verify the message displayed when ownership calculation completes sucessfully
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
And I verify all validations passed
And I click on "ownershipCalulationValidations" "Submit" "button"
Then Verify the message displayed "Ejecución de balance Volumétrico con Propiedad para"
And I verify im redirected to the ownership calculations tracking page for segment
And I verify if the ownership calculation started

@testcase=16664 @ui @version=2
Scenario: Verify the message displayed when Initial node ownership is not determined
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then Verify the message displayed "La propiedad para el nodo node*, producto product*, propietario owner* y fecha ExecutionDate* no pudo ser encontrada."

@testcase=16665 @ui @version=2
Scenario: Verify the message displayed when Initial node ownership is determined
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then Verify the message displayed "Las entradas con propiedad de los nodos iniciales de la cadena pueden ser determinadas."

@testcase=16666 @ui @version=2
Scenario: Verify the message displayed when calculation fails at Initial Node Determination
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then Verify the message displayed "No se pueden determinar los nodos iniciales de la cadena"

@testcase=16668 @ui @version=2
Scenario: Verify the message displayed when calculation succedes at Initial Node Determination
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then Verify the message displayed "Nodos iniciales determinados para la cadena."

@testcase=16669 @ui @version=2
Scenario: Verify the message displayed when calculation fails at Final Node Determination
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then Verify the message displayed "No se pueden determinar los nodos finales de la cadena."

@testcase=16670 @ui @version=2
Scenario: Verify the message displayed when Final node is determined
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then Verify the message displayed "Nodos finales determinados para la cadena."

@testcase=16671 @ui @version=2
Scenario: Verify the message displayed when calculation fails at Node Ownership rules
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then Verify the message displayed "El nodo node* no tiene configurada una estrategia de propiedad"

@testcase=16672 @ui @version=2
Scenario: Verify the message displayed when calculation succedes at Node Ownership Rules
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then Verify the message displayed "Todos los nodos tienen configurada una estrategia de propiedad para sus productos"

@testcase=16673 @ui @version=2
Scenario: Verify the message displayed when caluclation fails due to no Operational Cutoff found at Criteria
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then Verify the message displayed "No se encontró corte operativo para el segmento. Por favor realice el corte operativo primero."

@testcase=16674 @ui @version=2
Scenario: Verify the message displayed when input data element is not given
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then Verify the error displayed "Requerido"

@testcase=16675 @ui @version=2
Scenario: Verify the message displayed when input data Final date is not given
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then Verify the message displayed "Requerido"

@testcase=16676 @ui @version=2
Scenario: Verify TRUE user is not able to move forward when input data Final date is greater than initial date
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate greaterthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then I should see error message "La fecha final debe ser mayor o igual a la fecha inicial"

@testcase=16677 @ui @version=2
Scenario: Verify when Ownership Calculation for a segment has already been completed
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
And I select the FinalDate lessthan CurrentDate from DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
And I verify all validations passed
And I click on "ownershipCalulationValidations" "Submit" "button"
And I click on "ownershipCalConfirmation" "Next" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
Then I verify the Initial Date to be Final Date of last balance with ownership calculated plus one day

@testcase=16678 @ui @version=2
Scenario: Verify when ownership calculation for a segment has not been done
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
When I click on "NewCut" "button"
Then I should see "Start" "link"
When I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan CurrentDate from DatePicker
When I click on "InitTicket" "submit" "button"
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I provide value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "NewCut" "button"
And I navigate to "ownershipcalculation" page
And I select a value from "Segment"
Then I verify the Initial Date, Initial Date of first operational cutoff

@testcase=16679 @ui @manual @version=2
Scenario: Verify when User start calculation for a segment when other ticket is in process
Given I have "ownershipnodes" created
When I navigate to "ownershipcalculation" page
And I select an existing segment with no operational cutoff
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then I should see the error message "El proceso de cálculo de propiedad para el segmento elegido ya fue enviado y se encuentra pendiente de respuesta"

@testcase=16680 @ui @manual @version=2
Scenario: Verify when TRUE user starts an ownership calculation wizard all segment items should be displaye in the list
Given I have "ownershipnodes" created
When I navigate to "ownershipcalculation" page
And I click on "Segment" "Filter"
Then I verify if all segments are displayed in the combobox

@testcase=68018 @e2e @ui
Scenario: Verify ownership ticket is generated for segment in the given period
Given I have ownership calculation data generated in the system
When I navigate to ownershipcalculation page
Then I should see ownership calculated segment is processed

@ficoe2e @ui
Scenario: Verify ownership ticket is generated for segment in the given period with FICO endpoint
Given I have ownership calculation data generated in the system with fico endpoint
When I navigate to ownershipcalculation page
Then I should see ownership calculated segment is processed