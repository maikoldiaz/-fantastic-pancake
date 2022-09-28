@sharedsteps=16589 @owner=jagudelos @ui @testplan=35673 @testsuite=35692 @parallel=false
Feature: ValidationsAddedInOwnershipCalculationProcess
As a Balance Segment Professional User,
I need validations to be added in the ownership calculation process
to ensure the completeness of the data to be sent to FICO

Background: Login
Given I am logged in as "profesional"

@testcase=37491 @bvt @version=3 @bvt1.5
Scenario: Validate ownership calculation process is success when we have node with ownership strategy
And I have "ownershipnodes" created
And I have ownership strategy for node
And I have ownership strategy for node products
And I have ownership strategy for node connections
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And I change the elements count per page to 50
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
And I change the elements count per page to 50
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "ConfirmCutoff" "Submit" "button"
And I wait till cutoff ticket processing to complete
And I navigate to "Ownershipcalculation" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then I see "Todos los nodos tienen configurada una estrategia de propiedad." message for "Nodos con estrategia de propiedad"
And I verify all "9" validations passed
And I click on "ownershipCalulationValidations" "Submit" "button"
And I click on "ownershipCalculationConfirmation" "Submit" "button"

@testcase=37492 @version=3 @bvt1.5
Scenario: Validate ownership calculation process fails when we have node without ownership strategy
And I have "ownershipnodes" created
And I have ownership strategy removed for node
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And I change the elements count per page to 50
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
And I change the elements count per page to 50
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "ConfirmCutoff" "Submit" "button"
And I wait till cutoff ticket processing to complete
And I navigate to "Ownershipcalculation" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then I see "El nodo [node name] no tiene configurada una estrategia de propiedad." error message for "Nodos con estrategia de propiedad"

@testcase=37493 @version=3 @bvt1.5
Scenario: Validate ownership calculation process fails when we have product without ownership strategy
And I have "ownershipnodes" created
And I have ownership strategy removed for product
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And I change the elements count per page to 50
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
And I change the elements count per page to 50
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "ConfirmCutoff" "Submit" "button"
And I wait till cutoff ticket processing to complete
And I navigate to "Ownershipcalculation" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then I see "Los productos del nodo [node name] no tienen configurada una estrategia de propiedad." error message for "Nodos – Productos con estrategia de propiedad"

@testcase=37494 @version=3 @bvt1.5
Scenario: Validate ownership calculation process is success when we have product connections with ownership strategy
And I have "ownershipnodes" created
And I have ownership strategy for node
And I have ownership strategy for node products
And I have ownership strategy for node connections
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And I change the elements count per page to 50
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
And I change the elements count per page to 50
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "ConfirmCutoff" "Submit" "button"
And I wait till cutoff ticket processing to complete
And I navigate to "Ownershipcalculation" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then I see "Todas las conexiones tienen configurada una estrategia de propiedad para sus productos." message for "Conexiones – Productos con estrategia de propiedad"
And I verify all "9" validations passed
And I click on "ownershipCalulationValidations" "Submit" "button"
And I click on "ownershipCalculationConfirmation" "Submit" "button"

@testcase=37495 @version=3 @bvt1.5
Scenario: Validate ownership calculation process fails when we have product connections without ownership strategy
And I have "ownershipnodes" created
And I have ownership strategy removed for connections
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And I change the elements count per page to 50
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
And I change the elements count per page to 50
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "ConfirmCutoff" "Submit" "button"
And I wait till cutoff ticket processing to complete
And I navigate to "Ownershipcalculation" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then I see "Los productos de la conexión [source node - destination node] no tienen configurada una estrategia de propiedad." error message for "Conexiones - Productos con priorización definida"

@testcase=37496 @version=3 @bvt1.5
Scenario: Validate ownership calculation process is success when we have product connections with priority
And I have "ownershipnodes" created
And I have ownership strategy for node
And I have ownership strategy for node products
And I have ownership strategy for node connections with priority
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And I change the elements count per page to 50
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
And I change the elements count per page to 50
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "ConfirmCutoff" "Submit" "button"
And I wait till cutoff ticket processing to complete
And I navigate to "Ownershipcalculation" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then I see "Todas las conexiones tienen configurada una priorización para sus productos." message for "Conexiones - Productos con priorización definida"
And I verify all "9" validations passed
And I click on "ownershipCalulationValidations" "Submit" "button"
And I click on "ownershipCalculationConfirmation" "Submit" "button"

@testcase=37497 @version=3 @bvt1.5
Scenario: Validate ownership calculation process is success when we have active ownership strategy for node
And I have "ownershipnodes" created
And I have active ownership strategy for node 
And I have ownership strategy for node products
And I have ownership strategy for node connections
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And I change the elements count per page to 50
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
And I change the elements count per page to 50
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "ConfirmCutoff" "Submit" "button"
And I wait till cutoff ticket processing to complete
And I navigate to "Ownershipcalculation" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then I see "Todos los nodos, nodos - productos, conexiones - productos tienen estrategias de propiedad vigentes." message for "Estrategias de propiedad vigentes"
And I verify all "9" validations passed
And I click on "ownershipCalulationValidations" "Submit" "button"
And I click on "ownershipCalculationConfirmation" "Submit" "button"

@testcase=37498 @version=3 @bvt1.5
Scenario: Validate ownership calculation process fails when we have inactive ownership strategy for node
And I have node with inactive ownership strategy for from FICO
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And I change the elements count per page to 50
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
And I change the elements count per page to 50
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "ConfirmCutoff" "Submit" "button"
And I wait till cutoff ticket processing to complete
And I navigate to "Ownershipcalculation" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then I see "La estrategia de propiedad [inactiveOwnership strategy name] de tipo Nodo se encuentra inactiva." error message for "Estrategias de propiedad vigentes"

@testcase=37499 @version=3 @bvt1.5
Scenario: Validate ownership calculation process fails when we have inactive ownership strategy for connection
Given I have inactive ownership strategy for product connection from FICO
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And I change the elements count per page to 50
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
And I change the elements count per page to 50
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "ConfirmCutoff" "Submit" "button"
And I wait till cutoff ticket processing to complete
And I navigate to "Ownershipcalculation" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then I see "La estrategia de propiedad [inactiveOwnership strategy name] de tipo Conexión - Producto se encuentra inactiva." error message for "Estrategias de propiedad vigentes"

@testcase=37500 @version=3 @bvt1.5
Scenario: Validate ownership calculation process fails when we have inactive ownership strategy for Product
Given I have inactive ownership strategy for node product from FICO
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And I change the elements count per page to 50
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
And I change the elements count per page to 50
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "ConfirmCutoff" "Submit" "button"
And I wait till cutoff ticket processing to complete
And I navigate to "Ownershipcalculation" page
And I click on "NewBalance" "button"
And I select segment from "OwnershipCal" "Segment" "dropdown"
And I select the FinalDate lessthan "1" days from CurrentDate on "Ownership" DatePicker
And I click on "ownershipCalculationCriteria" "Submit" "button"
Then I see "La estrategia de propiedad [inactiveOwnership strategy name] de tipo Nodo - producto se encuentra inactiva." error message for "Estrategias de propiedad vigentes"