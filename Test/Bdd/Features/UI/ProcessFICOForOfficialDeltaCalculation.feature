@sharedsteps=4013 @owner=jagudelos @ui @testsuite=61545 @testplan=61542 @MVP2and3 @S16
Feature: ProcessFICOForOfficialDeltaCalculation
As a TRUE System User,
I need to send to FICO consolidated operational information and official information
to execute the official deltas calculation

Background:  Login
	Given I am logged in as "admin"

@testcase=66897 @version=2
Scenario:Verify that system should send the information of the official inventories with a date equal to the period start date to "balanceOficialInventarios" object of FICO collection.
	Given I have ownership calculation data generated in the system for official delta
	And I create the Annulation Movement for these updated movements
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	And select a time period from the Processing Period dropdown
	And I click on "initOfficialDeltaTicket" "submit" "button"
	And I save the values of Period start and end dates from UI
	And I click on "validateOfficialDeltaTicket" "submit" "button"
	And I click on "confirmOfficialDeltaTicket" "submit" "button"
	Then I wait for Official Delta Calculation process to complete
	And Verify that System should send the information details to the "FICO Request"
	And Verify that system should send the official inventory details in the "balanceOficialInventarios" array of FICO Collection
	And Verify that the inventory date in "fecha" of "balanceOficialInventarios" should be minus one day from inventory date

@testcase=66898 @version=2
Scenario:Verify that system should send the information of the official inventories with a date not equal to the period start date to "balanceOficialInventarios" object of FICO collection.
	Given I have ownership calculation data generated in the system for official delta
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	And select a time period from the Processing Period dropdown
	And I click on "initOfficialDeltaTicket" "submit" "button"
	And I save the values of Period start and end dates from UI
	And I click on "validateOfficialDeltaTicket" "submit" "button"
	And I click on "confirmOfficialDeltaTicket" "submit" "button"
	Then I wait for Official Delta Calculation process to complete
	Then Verify that System should send the information details to the "FICO Request"
	And Verify that system should send the official inventory details in the "balanceOficialInventarios" array of FICO Collection
	And Verify that the inventory date in "fecha" of "balanceOficialInventarios" should be the inventory date of the segment

@testcase=66899 @version=2
Scenario:Verify that system should send the information of the official movements  to "balanceOficialMovimientos" object of FICO collection.
	Given I have ownership calculation data generated in the system for official delta
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	And select a time period from the Processing Period dropdown
	And I click on "initOfficialDeltaTicket" "submit" "button"
	And I save the values of Period start and end dates from UI
	And I click on "validateOfficialDeltaTicket" "submit" "button"
	And I click on "confirmOfficialDeltaTicket" "submit" "button"
	Then I wait for Official Delta Calculation process to complete
	And Verify that System should send the information details to the "FICO Request"
	And Verify that system should send the official movements details in the "balanceOficialMovimientos" array of FICO Collection

@testcase=66900 @version=2
Scenario:Verify that system should send the information of the consolidated inventories to "consolidadosInventarios" object of FICO collection.
	Given True System is processing the data to generate official ticket with consolidated details
	When I have an consolidated inventories in the system that correspond to the nodes of the segment official delta ticket
	And The inventory date is equal to the start date of the period minus one day or equal to the end date of the period
	Then Verify that System should send the information details to the "FICO Request"
	And Verify that system should send the information of the consolidated inventories details in the "consolidadosInventarios" array of FICO Collection

@testcase=66901 @version=2
Scenario:Verify that system should send the information of the consolidated movements to "consolidadosMovimientos" object of FICO collection.
	Given True System is processing the data to generate official ticket with consolidated details
	When I have an consolidated movements in the system where source or destination node is equal to the nodes of the segments official delta ticket
	And The movements start and end dates match the start and end dates of the period
	Then Verify that System should send the information details to the "FICO Request"
	And Verify that system should send the information of the consolidated movements details in the "consolidadosMovimientos" array of FICO Collection

@testcase=66902 @version=2
Scenario:Verify that system should send the configuration of active relationships between movement types to "configuracionTiposMovimientos" object of FICO collection.
	Given I have ownership calculation data generated in the system for official delta
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	And select a time period from the Processing Period dropdown
	And I click on "initOfficialDeltaTicket" "submit" "button"
	And I save the values of Period start and end dates from UI
	And I click on "validateOfficialDeltaTicket" "submit" "button"
	And I click on "confirmOfficialDeltaTicket" "submit" "button"
	Then I wait for Official Delta Calculation process to complete
	And Verify that System should send the information details to the "FICO Request"
	And Verify that system should send the configuration of active relationships between movement types in the "configuracionTiposMovimientos" array of FICO Collection

@testcase=66903 @manual @version=2
Scenario:Verify that delta ticket should be failed and exception should be logged when FICO Web API returns a status code other than 200
	And I have turn down the FICO Web API services to returns a status code other than 200
	And I have performed the delta calculation for the segment
	Then Verify that delta ticket status should be "failed"
	And "Se presentó un error técnico inesperado al enviar la información al motor de reglas para el cálculo de deltas oficiales. Por favor ejecute nuevamente el proceso o comuníquese con la mesa de ayuda." Error Message should be displayed
	And An Exception should be logged in the Application insights

@testcase=66904 @manual @version=2
Scenario:Verify that delta ticket should be failed when FICO Web API returns data in "movimientosErrores" collection
	And I have configure the FICO Web API services to returns data in "movimientosErrores" collection
	And I have performed the delta calculation for the segment
	Then Verify that delta ticket status should be "failed"
	And verify that system should store the error of each movement which is Movement transaction Id, official delta ticket of the segment, and error description.
	And the status of nodes that belong to the segment ticket should be failed

@testcase=66905 @manual @version=2
Scenario:Verify that delta ticket should be failed when FICO Web API returns data in "inventariosErrores" collection
	And I have configure the FICO Web API services to returns data in "inventariosErrores" collection
	And I have performed the delta calculation for the segment
	Then Verify that delta ticket status should be "failed"
	And verify that system should store the error of each inventories which is inventory product Id, official delta ticket of the segment, and error description.
	And the status of nodes that belong to the segment ticket should be failed

@testcase=66906 @version=2
Scenario:Verify the system should register movement and its ownership when service return data in the "resultadoMovimientos" object of FICO Collection and movement does not exist
	Given I have ownership calculation data generated in the system for official delta
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	And select a time period from the Processing Period dropdown
	And I click on "initOfficialDeltaTicket" "submit" "button"
	And I save the values of Period start and end dates from UI
	And I click on "validateOfficialDeltaTicket" "submit" "button"
	And I click on "confirmOfficialDeltaTicket" "submit" "button"
	And Verify that System should send the information details to get "FICO Response"
	When service return data in the "resultadoMovimientos" object of FICO Collection
	And there is no movement in system in which the id of the source movement is equal to the value of the "idMovimientoTRUE" field in "resultadoMovimientos" collection
	And the official delta ticket is equal to the segment ticket
	And the origin returned by FICO service is official in "origen" field in "resultadoMovimientos" collection
	Then Verify that delta ticket status should be DeltaStatus
	And movement should be registered into the system
	And ownership should be registered for these movement

@testcase=66907 @ignore @version=2
Scenario:Verify the system should register its ownership when service return data in the "resultadoMovimientos" object of FICO Collection and movement exist in system
	And I performed the delta calculation for the segment having official movement and consolidation movements
	And Verify that System should send the information details to get "FICO Response"
	When service return data in the "resultadoMovimientos" object of FICO Collection
	And there is already associated movement in system in which the id of the source movement is equal to the value of the "idMovimientoTRUE" field in "resultadoMovimientos" collection
	And the official delta ticket is equal to the segment ticket
	And the origin returned by FICO service is official in "origen" field in "resultadoMovimientos" collection
	Then Verify that delta ticket status should be DeltaStatus
	And ownership should be registered for these movement

@testcase=66908 @version=2
Scenario:Verify the system should register movement and its ownership when service return data in the "resultadoInventarios" object of FICO Collection and movement does not exist
	Given I have ownership calculation data generated in the system for official delta
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	And select a time period from the Processing Period dropdown
	And I click on "initOfficialDeltaTicket" "submit" "button"
	And I save the values of Period start and end dates from UI
	And I click on "validateOfficialDeltaTicket" "submit" "button"
	And I click on "confirmOfficialDeltaTicket" "submit" "button"
	And Verify that System should send the information details to get "FICO Response"
	And service return data in the "resultadoInventarios" object of FICO Collection
	And there is no movement in system in which the id of the source movement from inventory is equal to the value of the "idInventarioTRUE" field in "resultadoInventarios" collection
	And the official delta ticket is equal to the segment ticket
	And the origin returned by FICO service is official in "origen" field in "resultadoInventarios" collection
	Then Verify that delta ticket status should be DeltaStatus
	And movement should be registered into the system
	And ownership should be registered for these movement

@testcase=66909 @version=2 @ignore
Scenario:Verify the system should register its ownership when service return data in the "resultadoInventarios" object of FICO Collection and movement exist in system
	And I performed the delta calculation for the segment having official inventory and consolidation inventory
	And Verify that System should send the information details to get "FICO Response"
	When service return data in the "resultadoInventarios" object of FICO Collection
	And there is already associated movement in system in which the id of the source movement is equal to the value of the "idInventarioTRUE" field
	And the official delta ticket is equal to the segment ticket
	And the origin returned by FICO service is official
	Then Verify that delta ticket status should be "delta"
	And ownership should be registered for these movement

@testcase=66910 @version=2
Scenario:Verify the system should register movement and its ownership when service return data in "resultadoMovimientos" which has a negative sign, the origin is official and movement not exists in TRUE
	Given True System is processing the data to generate official ticket with consolidated details
	And Verify that System should send the information details to get "FICO Response"
	When service return data in the "resultadoMovimientos" object of FICO Collection
	And the origin returned by FICO service is official in "origen" field in "resultadoMovimientos" collection
	Then Verify that delta ticket status should be DeltaStatus
	And Verify that system should obtain the data of the official movement stored in TRUE using the value of the field "idMovimientoTRUE" returned by FICO for Negative Movements
	And Verify that system should Create the new movement with the same movement data stored in TRUE: operational date, source node, destination node, source product, destination product, product type if it exists, unit, segment, start date, and end date
	And Verify that the value of the net quantity must be the value returned by FICO in the "cantidadNeta" field multiplied by -1
	And Verify that Movement type should be corresponding cancellation type obtain by getting the movement type identifier and search it in the configuration of relationships between movement types
	And Verify that the scenario of the new movement must be official
	And Verify that the value for the source movement identifier must be the movement ID returned by FICO in the "idMovimientoTRUE" field.
	And Verify that the source system must be "FICO"
	And Verify that the new movement must be assigned the official delta ticket of the segment
	And Verify that System should register the ownership of the movement
	And Verify that system should get the owner of the information reported by the source applications. To get the information of a movement owner use the data of the field "idMovimientoPropietario" returned by FICO
	And Verify that system should register the ownership of the movement using the owner identifier obtained in the previous step and the value of the official delta returned by FICO "deltaOficial" which must be multiplied by -1.
	And Verify that the ownership must be stored in the movement owners table

@testcase=66911 @version=2
Scenario:Verify the system should register movement and its ownership when service return data in "resultadoMovimientos" which has a positive sign, the origin is official and movement not exists in TRUE
	Given I have ownership calculation data generated in the system for official delta
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	And select a time period from the Processing Period dropdown
	And I click on "initOfficialDeltaTicket" "submit" "button"
	And I save the values of Period start and end dates from UI
	And I click on "validateOfficialDeltaTicket" "submit" "button"
	And I click on "confirmOfficialDeltaTicket" "submit" "button"
	And I wait for Official Delta Calculation process to complete
	And Verify that System should send the information details to get "FICO Response"
	And service return data in the "resultadoMovimientos" object of FICO Collection
	And the origin returned by FICO service is official in "origen" field in "resultadoMovimientos" collection
	Then Verify that delta ticket status should be DeltaStatus
	And Verify that system should obtain the data of the official movement stored in TRUE using the value of the field "idMovimientoTRUE" returned by FICO for Positive Movements
	And Verify that system should Create the new movement with the same movement data stored in TRUE: operational date, source node, destination node, source product, destination product, product type if it exists, unit, segment, start date, and end date
	And Verify that the value of the net quantity must be the value returned by FICO in the "cantidadNeta" field
	And Verify that the scenario of the new movement must be official
	And Verify that the value for the source movement identifier must be the movement ID returned by FICO in the "idMovimientoTRUE" field.
	And Verify that the source system must be "FICO"
	And Verify that the new movement must be assigned the official delta ticket of the segment
	And Verify that System should register the ownership of the movement
	And Verify that system should get the owner of the information reported by the source applications. To get the information of a movement owner use the data of the field "idMovimientoPropietario" returned by FICO
	And Verify that system should register the ownership of the movement using the owner identifier obtained in the previous step and the value of the official delta returned by FICO "deltaOficial"
	And Verify that the ownership must be stored in the movement owners table

@testcase=66912 @version=2 @ignore
Scenario:Verify the system should register ownership when service return data in "resultadoMovimientos" having the origin is official and movement already exists in TRUE
	And I performed the delta calculation for the segment having official movements and inventories
	And Verify that System should send the information details to get "FICO Response"
	When service return data in the "resultadoMovimientos" object of FICO Collection
	And the movement is of official source and already exists in TRUE
	Then Verify that delta ticket status should be "delta"
	And Verify that System should register the ownership of the movement
	And Verify that system should get the owner of the information reported by the source applications. To get the information of a movement owner use the data of the field "idMovimientoPropietario" returned by FICO
	And Verify that If the sign is positive, system should store the ownership of the movement with the value of the official delta returned by FICO "deltaOficial"
	And Verify that If the sign is negative, system should multiply by -1 the value of the official delta returned by FICO "deltaOficial" and store the ownership of the movement

@testcase=66913 @version=2
Scenario:Verify the system should register movement and its ownership when service return data in "resultadoMovimientos" which has a negative sign, the origin is consolidated and movement not exists in TRUE
	Given True System is processing the data to generate official ticket with consolidated details
	And Verify that System should send the information details to get "FICO Response"
	When service return data in the "resultadoMovimientos" object of FICO Collection
	And the origin returned by FICO service is Consolidated in "origen" field in "resultadoMovimientos" collection
	Then Verify that delta ticket status should be DeltaStatus
	And Verify that system should obtain the data of the official movement stored in TRUE using the value of the field "idMovimientoTRUE" returned by FICO for Negative Operative Movements
	And Verify that system should Create the new movement with the same movement data stored in TRUE: operational date, source node, destination node, source product, destination product, product type if it exists, unit, segment, start date, and end date
	And Verify that the value of the net quantity must be the value returned by FICO in the "cantidadNeta" field multiplied by -1
	And verify that the operational date must be equal to the start date of the consolidated movement.
	And Verify that Movement type should be corresponding cancellation type obtain by getting the movement type identifier and search it in the configuration of relationships between movement types
	And Verify that the scenario of the new movement must be official
	And Verify that the value for the source movement identifier must be the movement ID returned by FICO in the "idMovimientoTRUE" field.
	And Verify that the source system must be "FICO"
	And Verify that the new movement must be assigned the official delta ticket of the segment
	And Verify that System should register the ownership of the movement
	And Verify that system should get the owner of the consolidated ownership information. To get the information of a movement owner use the data of the field "idMovimientoPropietario" returned by FICO
	And Verify that system should register the ownership of the movement using the owner identifier obtained in the previous step and the value of the official delta returned by FICO "deltaOficial" which must be multiplied by -1.
	And Verify that the ownership must be stored in the movement owners table

@testcase=66915 @ignore @version=2
Scenario:Verify the system should register ownership when service return data in "resultadoMovimientos" having the origin is consolidated and movement already exists in TRUE
	And I performed the delta calculation for the segment having consolidated movements and inventories
	And Verify that System should send the information details to get "FICO Response"
	When service return data in the "resultadoMovimientos" object of FICO Collection
	And the movement is of official source and already exists in TRUE
	Then Verify that delta ticket status should be "delta"
	And Verify that System should register the ownership of the movement
	And Verify that system should get the owner of the consolidated ownership information. To get the information of a movement owner use the data of the field "idMovimientoPropietario" returned by FICO
	And Verify that If the sign is positive, system should store the ownership of the movement with the value of the official delta returned by FICO "deltaOficial"
	And Verify that If the sign is negative, system should multiply by -1 the value of the official delta returned by FICO "deltaOficial" and store the ownership of the movement

@testcase=66916 @version=2
Scenario:Verify the system should register movement and its ownership when service return data in "resultadoInventarios" which has a negative sign, the origin is official and movement not exists in TRUE
	Given True System is processing the data to generate official ticket with consolidated details
	And Verify that System should send the information details to get "FICO Response"
	When service return data in the "resultadoInventarios" object of FICO Collection
	And the origin returned by FICO service is official in "origen" field in "resultadoInventarios" collection
	Then Verify that delta ticket status should be DeltaStatus
	And Verify that system should obtain the data of the official inventory stored in TRUE using the value of the field "idInventarioTRUE" returned by FICO for Negative Official Movements
	And Verify that system should create the new movement with source node and source product. Use the node and product of the inventory registered to TRUE. The destination node and product must be null
	And Verify that The movement type must be "Delta inventario"
	And Verify that the value of the net quantity must be the value returned by FICO in the "cantidadNeta" field
	And Verify that the scenario of the new movement must be official
	And Verify that source inventory identifier must be the inventory identifier returned by FICO in the "idInventarioTRUE" field.
	And Verify that the source system must be "FICO"
	And Verify that the new movement must be assigned the official delta ticket of the segment
	And Verify that System should register the ownership of the movement
	And Verify that system should get the owner of the information reported by the source applications. To get the information of a inventory owner use the data of the field "idInventarioPropietario" returned by FICO
	And Verify that system should register the ownership of the movement using the owner identifier obtained in the previous step and the value of the official delta returned by FICO "deltaOficial"
	And Verify that the ownership must be stored in the movement owners table

@testcase=66917 @version=2
Scenario:Verify the system should register movement and its ownership when service return data in "resultadoInventarios" which has a positive sign, the origin is official and movement not exists in TRUE
	Given I have ownership calculation data generated in the system for official delta
	And I navigate to "Calculation of deltas by official adjustment" page
	When I click on "newDeltasCalculation" "button"
	And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
	And select a time period from the Processing Period dropdown
	And I click on "initOfficialDeltaTicket" "submit" "button"
	And I save the values of Period start and end dates from UI
	And I click on "validateOfficialDeltaTicket" "submit" "button"
	And I click on "confirmOfficialDeltaTicket" "submit" "button"
	And I wait for Official Delta Calculation process to complete
	And Verify that System should send the information details to get "FICO Response"
	And service return data in the "resultadoInventarios" object of FICO Collection
	And the origin returned by FICO service is official in "origen" field in "resultadoInventarios" collection
	Then Verify that delta ticket status should be DeltaStatus
	And Verify that system should obtain the data of the official inventory stored in TRUE using the value of the field "idInventarioTRUE" returned by FICO for Positive Official Movements
	And Verify that system should create new movement with destination node and destination product. Use the node and product of the inventory registered to TRUE. The source node and product must be null.
	And Verify that The movement type must be "Delta inventario"
	And Verify that the value of the net quantity must be the value returned by FICO in the "cantidadNeta" field
	And Verify that the scenario of the new movement must be official
	And Verify that source inventory identifier must be the inventory identifier returned by FICO in the "idInventarioTRUE" field.
	And Verify that the source system must be "FICO"
	And Verify that the new movement must be assigned the official delta ticket of the segment
	And Verify that System should register the ownership of the movement
	And Verify that system should get the owner of the information reported by the source applications. To get the information of a inventory owner use the data of the field "idInventarioPropietario" returned by FICO
	And Verify that system should register the ownership of the movement using the owner identifier obtained in the previous step and the value of the official delta returned by FICO "deltaOficial".
	And Verify that the ownership must be stored in the movement owners table

@testcase=66918 @ignore @version=2
Scenario:Verify the system should register ownership when service return data in "resultadoInventarios" having the origin is official and movement already exists in TRUE
	And I performed the delta calculation for the segment having official movements and inventories
	And Verify that System should send the information details to get "FICO Response"
	When service return data in the "resultadoInventarios" object of FICO Collection
	And the movement is of official source and already exists in TRUE
	Then Verify that delta ticket status should be "delta"
	And Verify that System should register the ownership of the movement
	And Verify that system should get the owner of the information reported by the source applications. To get the information of a inventory owner use the data of the field "idInventarioPropietario" returned by FICO
	And Verify that system should register the ownership of the movement using the owner identifier obtained in the previous step and the value of the official delta returned by FICO "deltaOficial".
	And Verify that the ownership must be stored in the movement owners table

@testcase=66919 @version=2
Scenario:Verify the system should register movement and its ownership when service return data in "resultadoInventarios" which has a negative sign, the origin is consolidated and movement not exists in TRUE
	Given True System is processing the data to generate official ticket with consolidated details
	And Verify that System should send the information details to get "FICO Response"
	When service return data in the "resultadoInventarios" object of FICO Collection
	And the origin returned by FICO service is Consolidated in "origen" field in "resultadoInventarios" collection
	Then Verify that delta ticket status should be DeltaStatus
	And Verify that system should obtain the data of the official inventory stored in TRUE using the value of the field "idInventarioTRUE" returned by FICO for Negative Consolidation Movements
	And Verify that system should create new movement with source node and source product. Use the node and product of the inventory registered to TRUE. The destination node and product must be null.
	And Verify that The movement type must be "Delta inventario"
	And Verify that the value of the net quantity must be the value returned by FICO in the "cantidadNeta" field
	And Verify that the scenario of the new movement must be official
	And Verify that the source system must be "FICO"
	And Verify that the new movement must be assigned the official delta ticket of the segment
	And Verify that System should register the ownership of the movement
	And Verify that system should get the  owner of the consolidated ownership information registered in TRUE. To get the information of a inventory owner use the data of the field "idInventarioPropietario" returned by FICO
	And Verify that system should register the ownership of the movement using the owner identifier obtained in the previous step and the value of the official delta returned by FICO "deltaOficial".
	And Verify that the ownership must be stored in the movement owners table

@testcase=66921 @ignore @version=2
Scenario:Verify the system should register ownership when service return data in "resultadoInventarios" having the origin is consolidated and movement already exists in TRUE
	And I performed the delta calculation for the segment having consolidated movements and inventories
	And Verify that System should send the information details to get "FICO Response"
	When service return data in the "resultadoInventarios" object of FICO Collection
	And the movement is of official source and already exists in TRUE
	Then Verify that delta ticket status should be "delta"
	And Verify that System should register the ownership of the movement
	And Verify that system should get the owner of the consolidated ownership information registered in TRUE. To get the information of a inventory owner use the data of the field "idInventarioPropietario" returned by FICO
	And Verify that system should register the ownership of the movement using the owner identifier obtained in the previous step and the value of the official delta returned by FICO "deltaOficial".
	And Verify that the ownership must be stored in the movement owners table