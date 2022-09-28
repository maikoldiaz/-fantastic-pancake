@sharedsteps=4013 @owner=jagudelos @testsuite=70804 @testplan=70526 @ui @S16 @MVP2and3
Feature: OfficialDeltaRegistration
As a TRUE system, I need to process the movements of official deltas returned by FICO to register them in TRUE

Background: Login
Given I am logged in as "admin"
@testcase=72627
Scenario: To verify the application is processing the FICO Web API response that returns the official deltas with negative movement
Given I upload data of official movement or inventory for node A and B with two ownerships
When I navigate to "Calculation of deltas by official adjustment" page
When I click on "newDeltasCalculation" "button"
And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
And select a time period from the Processing Period dropdown
And I click on "initOfficialDeltaTicket" "submit" "button"
And I save the values of Period start and end dates from UI
And I click on "validateOfficialDeltaTicket" "submit" "button"
And I click on "confirmOfficialDeltaTicket" "submit" "button"
Then I wait for Official Delta Calculation process to complete
And Approve the movement or inventory of node A
Given I upload data of official movement or inventory for node A and B with same period and one ownerships
When I reopen the node A
And I navigate to "Calculation of deltas by official adjustment" page
When I click on "newDeltasCalculation" "button"
And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
And select a time period from the Processing Period dropdown
And I click on "initOfficialDeltaTicket" "submit" "button"
And I save the values of Period start and end dates from UI
And I click on "validateOfficialDeltaTicket" "submit" "button"
And I click on "confirmOfficialDeltaTicket" "submit" "button"
Then I wait for Official Delta Calculation process to complete
When A record in the collection "resultadoMovimientos" has a negative sign, the origin is official and the new movement with its ownership has not been stored.
Then verify new movement "resultadoMovimientos" is created using the value of the field "idMovimientoTRUE" returned by FICO
And verify the new movement is created with the same movement data stored in TRUE as below "<example>" which has a "negative" delta
| example             |
| operational date    |
| source node         |
| Destination node    |
| source product      |
| Destination product |
| product type        |
| unit                |
| segment             |
| start date          |
| end date            |
And verify the value of the net quantity must be the value returned by FICO in the "cantidadNeta" field.
And verify the " movementTypeIdentifier " configuration of relationships between movement types to obtain the corresponding cancellation type
And verify scenario of the new movement must be official "2"
And verify value for the source movement identifier must be the movement ID returned by FICO in the "idMovimientoTRUE" field
And source system must be "FICO"
And verify new movement "resultadoMovimientos" is assigned with official delta ticket of the segment
And Verify the ownership registration using the owner identifier returned by FICO in the “idMovimientoPropietario” field
And the value of the official delta returned in the "deltaOficial field" which should be multiplied by "-1"
And verify the ownership must be stored in the movement owners table
@testcase=72628
Scenario: To verify the application is processing the FICO Web API response that returns the official deltas with positive movement
Given I upload data of official movement or inventory for node A and B with two ownerships
When I navigate to "Calculation of deltas by official adjustment" page
When I click on "newDeltasCalculation" "button"
And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
And select a time period from the Processing Period dropdown
And I click on "initOfficialDeltaTicket" "submit" "button"
And I save the values of Period start and end dates from UI
And I click on "validateOfficialDeltaTicket" "submit" "button"
And I click on "confirmOfficialDeltaTicket" "submit" "button"
Then I wait for Official Delta Calculation process to complete
And The user upload a new negative movement originated by manual delta system for a new owner
And Approve the movement or inventory of node A
Given I upload data of official movement or inventory for node A and B with same date period
When I reopen the node A
And I navigate to "Calculation of deltas by official adjustment" page
When I click on "newDeltasCalculation" "button"
And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
And select a time period from the Processing Period dropdown
And I click on "initOfficialDeltaTicket" "submit" "button"
And I save the values of Period start and end dates from UI
And I click on "validateOfficialDeltaTicket" "submit" "button"
And I click on "confirmOfficialDeltaTicket" "submit" "button"
When A record in the collection "resultadoMovimientos" has a positive sign, the origin is official and the new movement with its ownership has not been stored.
Then I wait for Official Delta Calculation process to complete
Then verify new movement "resultadoMovimientos" is created using the value of the field "idMovimientoTRUE" returned by FICO
And verify the new movement is created with the same movement data stored in TRUE as below "<example>" which has a "positive" delta
| example             |
| operational date    |
| source node         |
| Destination node    |
| source product      |
| Destination product |
| product type        |
| unit                |
| segment             |
| start date          |
| end date            |
And verify the value of the net quantity must be the value returned by FICO in the "cantidadNeta" field.
And verify the " movementTypeIdentifier " configuration of relationships between movement types to obtain the corresponding cancellation type
And verify scenario of the new movement must be official "2"
And verify value for the source movement identifier must be the movement ID returned by FICO in the "idMovimientoTRUE" field
And source system must be "FICO"
And verify new movement "resultadoMovimientos" is assigned with official delta ticket of the segment
And Verify the ownership registration using the owner identifier returned by FICO in the “idMovimientoPropietario” field
And the value of the official delta returned in the "deltaOficial field" in in the movement owners table
@testcase=72629
Scenario: To verify the ownership Registration of a movement that already exists in the system
Given I update the "excel" that returns the official deltas with "positiveAndnegative" movement
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
When A record in the collection "resultadoMovimientos" is official source and the movement already exists in system
Then register the ownership of the movement using the owner returned by FICO in the field "idMovimientoPropietario"
And Verify sign is "positive" and store the ownership of the movement with the value of the official delta returned by FICO "deltaOficial"
And verify sign is "negative" and store the value of official delta returned by FICO "deltaOfficial" should multiple of "-1"
@testcase=72630
Scenario: To verify the application is processing the FICO Web API response that returns the official deltas with negative official inventory
Given I update the "excel" that returns the official deltas with "negative" inventory
Given I have ownership calculation data generated in the system for official delta
And I navigate to "Calculation of deltas by official adjustment" page
When I click on "newDeltasCalculation" "button"
And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
And select a time period from the Processing Period dropdown
And I click on "initOfficialDeltaTicket" "submit" "button"
And I save the values of Period start and end dates from UI
And I click on "validateOfficialDeltaTicket" "submit" "button"
And I click on "confirmOfficialDeltaTicket" "submit" "button"
When A record in the collection "resultadoInventarios" has a negative sign, the origin is “DELTAOFICIAL” and the new movement with its ownership has not been stored
Then I wait for Official Delta Calculation process to complete
Then verify new movement "resultadoMovimientos" is created using the value of the field "idMovimientoDeltaInv" returned by FICO
And verify the new movement with "sourceNode" and "sourceProduct" using the "destinationNode" and "destinationProduct" of the movement registered in system
And verify "destinationNode" and "destinationProduct" of the new movement must be "null"
And verify the new movement is created with the same movement data stored in TRUE as below "<example>" which has a "negative" delta
| example          |
| operational date |
| unit             |
| segment          |
| start date       |
| final date       |
And verify the value of the net quantity must be the value returned by FICO in the "cantidadNeta" field
And verify the value for the "sourceMovementIdentifier" must be the movement identifier returned by FICO in the "idMovimientoDeltaInv" field.
And verify the movement type must be "Delta inventario"
And verify scenario of the new movement must be official "2"
And source system must be "FICO"
And verify new movement "resultadoMovimientos" is assigned with official delta ticket of the segment
And verify the ownership registration using the owner identifier returned by FICO in the “idMovimientoPropietarioDeltaInv” field
And value of the official delta returned in the "deltaOfficialField"
And verify the ownership must be stored in the movement owners table
@testcase=72631
Scenario: To verify the application is processing the FICO Web API response that returns the official deltas with positive official inventory
Given I update the "excel" that returns the official deltas with "positive" inventory
Given I have ownership calculation data generated in the system for official delta
And I navigate to "Calculation of deltas by official adjustment" page
When I click on "newDeltasCalculation" "button"
And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
And select a time period from the Processing Period dropdown
And I click on "initOfficialDeltaTicket" "submit" "button"
And I save the values of Period start and end dates from UI
And I click on "validateOfficialDeltaTicket" "submit" "button"
And I click on "confirmOfficialDeltaTicket" "submit" "button"
When A record in the collection "resultadoInventarios" has a positive sign, the origin is official and the new movement with its ownership has not been stored.
Then I wait for Official Delta Calculation process to complete
And verify the value of the net quantity must be the value returned by FICO in the "cantidadNeta" field.
Then verify new movement "resultadoMovimientos" is created using the value of the field "idMovimientoDeltaInv" returned by FICO
And verify the new movement with "destinationNode" and "destinationProduct" using the "source Node" and "source Product" of the movement registered in system
And verify "sourceNode" and "sourceProduct" of the new movement must be "null"
And verify the new movement is created with the same movement data stored in TRUE as below "<example>" which has a "positive" delta
| example          |
| operational date |
| unit             |
| segment          |
| start date       |
| final date       |
And verify the value for the "sourceMovementIdentifier" must be the movement identifier returned by FICO in the "idMovimientoDeltaInv" field.
And verify the movement type must be "Delta inventario"
And verify scenario of the new movement must be official "2"
And source system must be "FICO"
And verify new movement "resultadoMovimientos" is assigned with official delta ticket of the segment
And verify the ownership registration using the owner identifier returned by FICO in the “idMovimientoPropietarioDeltaInv” field
And verify the ownership registration using the owner identifier the value of the official delta returned in the "deltaOficialfield"
@testcase=72632
Scenario: To verify the ownership Registration of a movement that already exists in the system with official delta origin.
Given I update the "excel" that returns the official deltas with record in the collection "resultadoInventarios" is “DELTAOFICIAL” origin and the movement already exists in system
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
When record in the collection "resultadoInventarios" is “DELTAOFICIAL” origin and the movement already exists in system
Then verify Register the ownership of the movement using the owner identifier returned by FICO in the “idMovimientoPropietarioDeltaInv” field
And value of the official delta returned in the "deltaOfficialField"
And verify the ownership must be stored in the movement owners table
@testcase=72633
Scenario: To verify the application is processing the FICO Web API response that returns Successful response
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
When the Web API returns a successful response "200" the collections "movimientosErrores" and "inventariosErrores" have no data
And verify the collections "resultadoMovimientos" or "resultadoInventarios" has data
Then verify the movements originated by movement deltas and movements originated by inventory deltas is updated with the segment ticket.
@testcase=72634
Scenario: To verify negative movements strorage of official or consolidated origin
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
When record in the collection "resultadoMovimientos" has "OFICIAL" or "CONSOLIDADO" origin, the sign is "NEGATIVO" and the movement has not been stored in TRUE system
Then verify the new movement is created with the net quantity "cantidadNeta" returned by FICO in system.
@testcase=72635
Scenario: To verify the collection with positive consolidated movement
Given I upload data of official movement or inventory for node A and B with two ownerships
When I navigate to "Calculation of deltas by official adjustment" page
When I click on "newDeltasCalculation" "button"
And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
And select a time period from the Processing Period dropdown
And I click on "initOfficialDeltaTicket" "submit" "button"
And I save the values of Period start and end dates from UI
And I click on "validateOfficialDeltaTicket" "submit" "button"
And I click on "confirmOfficialDeltaTicket" "submit" "button"
When A record in the collection "resultadoMovimientos" has a positive sign, the origin is consolidated and the new movement with its ownership has not been stored
Then I wait for Official Delta Calculation process to complete
And verify the data of the consolidated movement stored in TRUE using the value of the field "idMovimientoTRUE" returned by FICO
And verify the new movement is created with the same movement data stored in TRUE as below "<example>" which has a "positive" delta
| example                   |
| source node               |
| destination node          |
| source product            |
| destination product       |
| movement type             |
| unit                      |
| segment                   |
| start date                |
| end date                  |
| product type if it exists |
And verify the operational date must be equal to the start date of the consolidated movement
And Verify that the value of the net quantity must be the value returned by FICO in the "cantidadNeta" field
And verify he scenario of the new movement must be official "2"
And verify the value for the source movement identifier must be the movement ID returned by FICO in the "idMovimientoTRUE" field
And verify the source system must be "FICO"
And verify new movement "resultadoMovimientos" is assigned with official delta ticket of the segment
And verify the ownership registration using the owner identifier returned by FICO in the “idMovimientoPropietario” field
And verify the value of the official delta returned in the "deltaOficial"
And verify the ownership must be stored in the movement owners table
@testcase=72636 
Scenario: To verify the collection with Positive consolidated inventory
Given I upload data of official movement or inventory for node A and B with two ownerships
When I navigate to "Calculation of deltas by official adjustment" page
When I click on "newDeltasCalculation" "button"
And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
And select a time period from the Processing Period dropdown
And I click on "initOfficialDeltaTicket" "submit" "button"
And I save the values of Period start and end dates from UI
And I click on "validateOfficialDeltaTicket" "submit" "button"
And I click on "confirmOfficialDeltaTicket" "submit" "button"
When A record in the collection "resultadoInventarios" has a positive sign, the origin is consolidated and the new movement with its ownership has not been stored
Then I wait for Official Delta Calculation process to complete
And verify the data of the consolidated inventory stored in TRUE using the value of the field "idInventarioTRUE" returned by FICO
And verify the new movement is created with "destinatioNode" and "destination product" using the node and product of the inventory registered to TRUE
And verify the source "node" and "product" must be null.
And verify the movement type must be "Delta inventario"
And Verify that the value of the net quantity must be the value returned by FICO in the "cantidadNeta" field
And verify he scenario of the new movement must be official "2"
And verify the new movement is created with the same inventory data stored in TRUE as below "<example>" which has a "negative" delta
| example |
| unit    |
| segment |
And verify below "<values>" must be obtained from the consolidated inventory date stored in TRUE
| values           |
| operational date |
| start date       |
| end date         |
And verify the value for the source movement identifier must be the inventoryID returned by FICO in the "idInventarioTRUE" field
And verify the source system must be "FICO"
And verify new movement "resultadoInventarios" is assigned with official delta ticket of the segment
And verify the ownership registration using the owner identifier returned by FICO in the “idInventarioPropietario” field
And verify the value of the official delta returned in the "deltaOficial"
And verify the ownership must be stored in the movement owners table
