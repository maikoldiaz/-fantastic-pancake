@owner=jagudelos @ui @testsuite=49471 @testplan=49466 @MVP2and3
Feature: OperationalCutoffToIncludeInventoriesValidationStep
In order to ensure ownership initialization of the initial inventories
As a professional user
I need the Check Inventories step is included in the operational cutoff

@testcase=52399 @version=2
Scenario: Validate both validation have successful message with green icon
	Given I am logged in as "profesional"
	And I have nodes of the segment for the selected period already have an operational cutoff executed
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the EndDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	Then validate that "ValidateInitialInventory" "link" wizard is "active"
	And validate the message "Verificar inventarios para el corte del [start date in dd-MMM-yy format] al [end date in dd-MMM-yy format] del segmento [segment name]" in "header"
	And validate "Green" Icon displayed in "Initial Inventory"
	And validate the message "Los inventarios iniciales para los nodos del segmento y período seleccionado cuentan con la información de propiedad registrada desde la fuente." in "Initial Inventory"
	And validate "Green" Icon displayed in "Possible New Nodes"
	And validate the message "No existen nodos nuevos para el segmento y periodo seleccionado, estos ya fueron incluidos previamente en un corte operativo." in "Possible New Nodes"
	And validate that "validateInitialInventory" "submit "button" as enabled

@testcase=52400 @version=3 @BVT2
Scenario: Validate new node with initial inventories, but without owners
	Given I am logged in as "profesional"
	And I have nodes of the segment for the selected period already have an operational cutoff executed
	When I create new node "With Initial Inventory Without Owners" and "Movements"
	And I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the EndDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "ValidateInitialInventory" "link" wizard is "active"
	Then validate "Red" Icon displayed in "Initial Inventory"
	And validate the message "Existen nodos con inventarios iniciales sin propiedad registrada desde la fuente." in "Initial Inventory"
	And validate "New Node Name" without owner in "Initial Inventory"
	And validate "initial inventory date" corresponds to the "New Node" displayed in "Initial Inventory"
	And validate "TotalNumberOfNodes" corresponds to 'Initial Inventory Without Owner' in "Initial Inventory"
	And validate that "validateInitialInventory" "submit "button" as disabled

@testcase=52401 @version=2 @BVT2
Scenario: Validate new node with initial inventories and owners
	Given I am logged in as "profesional"
	And I have nodes of the segment for the selected period already have an operational cutoff executed
	When I create new node "With Initial Inventory With Owners" and "Movements"
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the EndDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "ValidateInitialInventory" "link" wizard is "active"
	Then validate "Green" Icon displayed in "Initial Inventory"
	And validate the message "Los inventarios iniciales para los nodos del segmento y período seleccionado cuentan con la información de propiedad registrada desde la fuente." in "Initial Inventory"
	And validate that "validateInitialInventory" "submit "button" as enabled

@testcase=52402 @version=3 @BVT2
Scenario: Validate new node without initial inventories
	Given I am logged in as "profesional"
	And I have nodes of the segment for the selected period already have an operational cutoff executed
	When I create new node "Without Initial Inventory" and "Movements"
	And I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the EndDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "ValidateInitialInventory" "link" wizard is "active"
	Then validate "Warning" Icon displayed in "Possible New Nodes"
	And validate the message "Existen nodos que no han sido incluidos previamente en un corte operativo y no tienen inventarios iniciales con propiedad registrados desde la fuente." in "Possible New Nodes"
	And validate "New Node Name" without initial inventory in "Possible New Nodes"
	And validate "initial inventory date" corresponds to the "New Node" displayed in "Possible New Nodes"
	And validate "TotalNumberOfNodes" corresponds to 'Without initial inventory' in "Possible New Nodes"
	And validate warning message "Asegúrese de cargar los inventarios iniciales para los nodos nuevos con sus propietarios o continúe con el proceso si los inventarios iniciales de estos nodos son iguales a cero."
	And validate that "validateInitialInventory" "submit "button" as enabled

@testcase=52403 @version=3
Scenario: Validate Node with updated segment, without initial inventories
	Given I am logged in as "profesional"
	And I have nodes of the segment for the selected period already have an operational cutoff executed
	When I create new node "Without Initial Inventory" and "Movements"
	And I update the Segment date different from initial date of next cutoff
	And I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the EndDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "ValidateInitialInventory" "link" wizard is "active"
	Then validate "Warning" Icon displayed in "Possible New Nodes"
	And validate the message "Existen nodos que no han sido incluidos previamente en un corte operativo y no tienen inventarios iniciales con propiedad registrados desde la fuente." in "Possible New Nodes"
	And validate "New Node Name" without initial inventory in "Possible New Nodes"
	And validate "initial inventory date" corresponds to the "New Node" displayed in "Possible New Nodes"
	And validate "TotalNumberOfNodes" corresponds to 'Without initial inventory' in "Possible New Nodes"
	And validate warning message "Asegúrese de cargar los inventarios iniciales para los nodos nuevos con sus propietarios o continúe con el proceso si los inventarios iniciales de estos nodos son iguales a cero."
	And validate that "validateInitialInventory" "submit "button" as enabled

@testcase=52404 @version=3
Scenario: Validate first operational cutoff of a segment with failed inventory verification
	Given I am logged in as "profesional"
	And I have "Node1" group 'With Initial Inventory Without Owners' before first day of cutoff and 'Movements' on the first day of cutoff
	And I have "Node2" group 'Without Initial Inventory' before first day of cutoff and 'Movements' on the first day of cutoff
	And I have "Node3" group 'Without Inventory' before first day of cutoff and 'No Movements' on the first day of cutoff
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "ValidateInitialInventory" "link" wizard is "active"
	Then validate "Red" Icon displayed in "Initial Inventory"
	And validate the message "Existen nodos con inventarios iniciales sin propiedad registrada desde la fuente." in "Initial Inventory"
	And validate "Node1" group without owner in "Initial Inventory"
	And validate "initial inventory date" corresponds to the "Node1" group displayed in "Initial Inventory"
	And validate "Warning" Icon displayed in "Possible New Nodes"
	And validate the message "Existen nodos que no han sido incluidos previamente en un corte operativo y no tienen inventarios iniciales con propiedad registrados desde la fuente." in "Possible New Nodes"
	And validate "Node2" group without initial inventory in "Possible New Nodes"
	And validate "initial inventory date" corresponds to the "Node2" group displayed in "Possible New Nodes"
	And validate "Node3" group not in "Initial Inventory"
	And validate "Node3" group not in "Possible New Nodes"
	And validate that "validateInitialInventory" "submit "button" as disabled

@testcase=52405 @version=3 @BVT2
Scenario:  Validate first operational cutoff of a segment with a warning in inventory verification
	Given I am logged in as "profesional"
	And I have "Node1" group 'Without Initial Inventory' before first day of cutoff and 'Movements' on the first day of cutoff
	And I have "Node2" group 'Without Inventory' before first day of cutoff and 'No Movements' on the first day of cutoff
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "ValidateInitialInventory" "link" wizard is "active"
	Then validate "Green" Icon displayed in "Initial Inventory"
	And validate the message "Los inventarios iniciales para los nodos del segmento y período seleccionado cuentan con la información de propiedad registrada desde la fuente." in "Initial Inventory"
	And validate "Warning" Icon displayed in "Possible New Nodes"
	And validate the message "Existen nodos que no han sido incluidos previamente en un corte operativo y no tienen inventarios iniciales con propiedad registrados desde la fuente." in "Possible New Nodes"
	And validate "Node1" group without initial inventory in "Possible New Nodes"
	And validate "initial inventory date" corresponds to the "Node1" group displayed in "Possible New Nodes"
	And validate warning message "Asegúrese de cargar los inventarios iniciales para los nodos nuevos con sus propietarios o continúe con el proceso si los inventarios iniciales de estos nodos son iguales a cero."
	And validate "Node2" group not in "Possible New Nodes"
	And validate that "validateInitialInventory" "submit "button" as enabled

@testcase=52406 @version=3 @BVT2
Scenario: Validate first operational cutoff of a segment with successful inventory verification
	Given I am logged in as "profesional"
	And I have "Node1" group 'With Initial Inventory With Owners' before first day of cutoff and 'Movements' on the first day of cutoff
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the StartDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
	And I select the EndDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "ValidateInitialInventory" "link" wizard is "active"
	Then validate "Green" Icon displayed in "Initial Inventory"
	And validate the message "Los inventarios iniciales para los nodos del segmento y período seleccionado cuentan con la información de propiedad registrada desde la fuente." in "Initial Inventory"
	And validate "Green" Icon displayed in "Possible New Nodes"
	And validate the message "No existen nodos nuevos para el segmento y periodo seleccionado, estos ya fueron incluidos previamente en un corte operativo." in "Possible New Nodes"
	And validate that "validateInitialInventory" "submit "button" as enabled
