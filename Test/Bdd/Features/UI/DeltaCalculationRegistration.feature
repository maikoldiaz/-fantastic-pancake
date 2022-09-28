@sharedsteps=4013 @ui @owner=jagudelos  @testplan=55104 @testsuite=55110 @S15 @MVP2and3
Feature: DeltaCalculationRegistration
As a TRUE system, I need to process the operational deltas calculated by FICO to register them in TRUE

Background: Login
	Given I am logged in as "admin"

@testcase=57931 @manual
Scenario: To verify the FICO Web API response status code other than 200
	Given verify the TRUE application is processing the FICO Web API response that returns the operational deltas
	When verify Web API returns a status code other than 200.
	Then verify the ticket status updated as "failed"
	And verify the error message "Se presentó un error inesperado en el envío de movimientos e inventarios para el cálculo de deltas operativos. Por favor ejecute nuevamente el proceso" is stored
	And verify the exception in Application insights

@testcase=57932 @manual
Scenario: To verify the business errors respose from FICO web API
	Given verify the TRUE application is processing the FICO Web API response that returns the operational deltas
	When verify Web API returns a status code other than 200.
	Then verify the ticket status updated as "failed"
	And verify if "movimientosErrores" collection has data, then store the error of each movement as below
		| Data                     |
		| Movement transaction Id  |
		| delta calculation ticket |
		| error description        |
	And verify if "inventariosErrores" collection has data, then store the error of each movement as below
		| Data                     |
		| inventory  product  Id   |
		| delta calculation ticket |
		| error description        |

@testcase=57933 @Version=2
Scenario: To verify FICO Web API Successful response
	Given I have ownership calculation data generated in the system
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "update" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestDataCutOff_Daywise" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	And I create the Annulation Movement for these updated movements
	When I navigate to "Calculation of deltas by operational adjust" page
	And I click on "New Deltas Calculation" "button"
	And I select segment from "initDeltaTicket" "segment" "dropdown"
	And I click on "initDeltaTicket" "Submit" "button"
	And I click on "pendingInventoriesGrid" "Submit" "button"
	And I click on "pendingMovementsGrid" "Submit" "button"
	Then I should see "Modal" "confirmDeltaCalculation" "container"
	And I click on "confirmDeltaCalculation" "submit" "Button"
	Then verify the "resultadoMovimientos" has new movements in system with delta ticket and their owners
	Then verify the "resultadoInventarios" has new movements in system with delta ticket and their owners

@testcase=57934 @manual
Scenario: To verify the delta registration when the movement nodes belong to a different segment on the start date of the next operational cutoff
	Given verify the TRUE application is processing the FICO Web API response that returns the operational deltas
	When The movement nodes registered to the "idMovimientoTRUE" transaction Id returned by FICO, belong to a different segment on the start date of the next operational cutoff.
	Then verify the application should mark the movement with delta calculation ticket
	And verify application must update the movement and stores the message "No se realizó el registro del delta, debido a que los nodos del movimiento pertenecen a otro segmento en la fecha del corte operativo [start date of the next operational cutoff, format dd-MMM-yy]"

@testcase=57935 @Version=2
Scenario: To verify response of FICO web API which have movement only with source node and negative delta
	Given I have ownership calculation data generated in the system
	Given I update the excel "TestDataCutOff_Daywise" with only "source" node and to get negative delta
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "update" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestDataCutOff_Daywise" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	And I create the Annulation Movement for these updated movements
	When I navigate to "Calculation of deltas by operational adjust" page
	And I click on "New Deltas Calculation" "button"
	And I select segment from "initDeltaTicket" "segment" "dropdown"
	And I click on "initDeltaTicket" "Submit" "button"
	And I click on "pendingInventoriesGrid" "Submit" "button"
	And I click on "pendingMovementsGrid" "Submit" "button"
	Then I should see "Modal" "confirmDeltaCalculation" "container"
	And I click on "confirmDeltaCalculation" "submit" "Button"
	And Verify that Delta Calculation should be successful
	Then verify new movement "resultadoMovimientos" is created with destination node and destination product
	Then verify "source" node and product should be null for new movement "resultadoMovimientos"
	And Verify the product type must be the same as the operational movement of "resultadoMovimientos"
	And Verify the unit, segment, scenario must be the same as the operational movement of "resultadoMovimientos"
	And verify the net quantity value must be the "delta" value and source movement or inventory identifier must be the "idMovimientoTRUE" returned by FICO "resultadoMovimientos"

@testcase=57936 @Version=2
Scenario: To verify the response of FICO web API which have movement only with destination node and negative delta
	Given I have ownership calculation data generated in the system
	Given I update the excel "TestDataCutOff_Daywise" with only "destination" node and to get negative delta
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "update" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestDataCutOff_Daywise" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	And I create the Annulation Movement for these updated movements
	When I navigate to "Calculation of deltas by operational adjust" page
	And I click on "New Deltas Calculation" "button"
	And I select segment from "initDeltaTicket" "segment" "dropdown"
	And I click on "initDeltaTicket" "Submit" "button"
	And I click on "pendingInventoriesGrid" "Submit" "button"
	And I click on "pendingMovementsGrid" "Submit" "button"
	Then I should see "Modal" "confirmDeltaCalculation" "container"
	And I click on "confirmDeltaCalculation" "submit" "Button"
	And Verify that Delta Calculation should be successful
	Then verify new movement "resultadoMovimientos" is created with destination node and destination product
	Then verify "destination" node and product should be null for new movement "resultadoMovimientos"
	And Verify the product type must be the same as the operational movement of "resultadoMovimientos"
	And Verify the unit, segment, scenario must be the same as the operational movement of "resultadoMovimientos"
	And verify the net quantity value must be the "delta" value and source movement or inventory identifier must be the "idMovimientoTRUE" returned by FICO "resultadoMovimientos"

@testcase=57937 @Version=2 @BVT2
Scenario: To verify the response of FICO web API which have movement with source and destination node and negative delta.
	Given I have ownership calculation data generated in the system
	Given I update the excel "TestDataCutOff_Daywise" with only "DestinationAndSource" node and to get negative delta
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "update" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestDataCutOff_Daywise" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	And I create the Annulation Movement for these updated movements
	When I navigate to "Calculation of deltas by operational adjust" page
	And I click on "New Deltas Calculation" "button"
	And I select segment from "initDeltaTicket" "segment" "dropdown"
	And I click on "initDeltaTicket" "Submit" "button"
	And I click on "pendingInventoriesGrid" "Submit" "button"
	And I click on "pendingMovementsGrid" "Submit" "button"
	Then I should see "Modal" "confirmDeltaCalculation" "container"
	And I click on "confirmDeltaCalculation" "submit" "Button"
	And Verify that Delta Calculation should be successful
	Then verify "destination" node and product should be null for new movement "resultadoMovimientos"
	And Verify the product type must be the same as the operational movement of "resultadoMovimientos"
	And Verify the unit, segment, scenario must be the same as the operational movement of "resultadoMovimientos"
	And verify the net quantity value must be the "delta" value and source movement or inventory identifier must be the "idMovimientoTRUE" returned by FICO "resultadoMovimientos"

@testcase=57938 @Version=2
Scenario: TO verify the FICO web API response which have movement with positive delta.
	Given I have ownership calculation data generated in the system
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Update" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestDataCutOff_Daywise" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	And I create the Annulation Movement for these updated movements
	When I navigate to "Calculation of deltas by operational adjust" page
	And I click on "New Deltas Calculation" "button"
	And I select segment from "initDeltaTicket" "segment" "dropdown"
	And I click on "initDeltaTicket" "Submit" "button"
	And I click on "pendingInventoriesGrid" "Submit" "button"
	And I click on "pendingMovementsGrid" "Submit" "button"
	Then I should see "Modal" "confirmDeltaCalculation" "container"
	And I click on "confirmDeltaCalculation" "submit" "Button"
	And Verify that Delta Calculation should be successful
	Then I verify newly created movement sourceNode, destinationNode and movementType must be the same type of operational movement with the "resultadoMovimientos" which has a positive sign
	And Verify the product type must be the same as the operational movement of "resultadoMovimientos"
	And Verify the unit, segment, scenario must be the same as the operational movement of "resultadoMovimientos"
	And verify the net quantity value must be the "delta" value and source movement or inventory identifier must be the "idMovimientoTRUE" returned by FICO "resultadoMovimientos"

@testcase=57939 @manual
Scenario: To verify the delta registration when the inventory nodes belong to a different segment on the start date of the next operational cutoff
	Given verify the TRUE application is processing the FICO Web API response that returns the operational deltas
	When The inventory nodes registered into application corresponding to the "idMovimientoTRUE" product Id returned by FICO
	And Verify the inventory nodes registered into application belong to a different segment on the start date of the next operational cutoff.
	Then verify the application should mark the inventory with delta calculation ticket
	And verify application must update the inventory and stores the message "No se realizó el registro del delta, debido a que el nodo del inventario pertenece a otro segmento en la fecha del corte operativo [start date of the next operational cutoff, format dd-MMM-yy]"

@testcase=57940 @Version=2
Scenario: To verify the FICO web API response which has inventory with negative delta.
	Given I have ownership calculation data generated in the system
	Given I update the excel "TestDataCutOff_Daywise" with only "DestinationAndSource" node and to get negative delta
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Update" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestDataCutOff_Daywise" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	And I create the Annulation Movement for these updated movements
	When I navigate to "Calculation of deltas by operational adjust" page
	And I click on "New Deltas Calculation" "button"
	And I select segment from "initDeltaTicket" "segment" "dropdown"
	And I click on "initDeltaTicket" "Submit" "button"
	And I click on "pendingInventoriesGrid" "Submit" "button"
	And I click on "pendingMovementsGrid" "Submit" "button"
	Then I should see "Modal" "confirmDeltaCalculation" "container"
	And I click on "confirmDeltaCalculation" "submit" "Button"
	And Verify the unit, segment, scenario must be the same as the operational movement of "resultadoInventario"
	And verify the net quantity value must be the "delta" value and source movement or inventory identifier must be the "idInventarioTRUE" returned by FICO "resultadoInventario"

@testcase=57941 @Version=2 @BVT2
Scenario: TO verify the FICO web API response which have inventory with positive delta.
	Given I have ownership calculation data generated in the system
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Update" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestDataCutOff_Daywise" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	And I create the Annulation Movement for these updated movements
	When I navigate to "Calculation of deltas by operational adjust" page
	And I click on "New Deltas Calculation" "button"
	And I select segment from "initDeltaTicket" "segment" "dropdown"
	And I click on "initDeltaTicket" "Submit" "button"
	And I click on "pendingInventoriesGrid" "Submit" "button"
	And I click on "pendingMovementsGrid" "Submit" "button"
	Then I should see "Modal" "confirmDeltaCalculation" "container"
	And I click on "confirmDeltaCalculation" "submit" "Button"
	And Verify that Delta Calculation should be successful
	Then I verify newly created movement sourceNode, destinationNode and movementType must be the same type of operational movement with the "resultadoInventario" which has a positive sign
	And Verify the product type must be the same as the operational movement of "resultadoInventario"
	And Verify the unit, segment, scenario must be the same as the operational movement of "resultadoInventario"
	And verify the net quantity value must be the "delta" value and source movement or inventory identifier must be the "idInventarioTRUE" returned by FICO "resultadoInventario"
