@sharedsteps=89386 @owner=jagudelos @ui @testplan=74716 @testsuite=89373 @CR @MVP2and3 @parallel=false
Feature: AdjustmentsToOfficialDeltasReturnedByFICO
As TRUE system, I need to process the official
deltas returned by FICO to store them one by one

Background: Login
Given I am logged in as "admin"


@testcase=89387
Scenario: Verify new movements is created when inventory date of the period is start date of the period and record in the collection "resultadoInventarios" has a negative sign and the origin is "OFICIAL"
Given I have an inventory date is equal to start date of the period
And TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoInventarios" has a negative sign and the origin is "OFICIAL"
Then new movement is created with source node and source product as node and product of the inventory registered to TRUE
And destination node and product must be null
And the movement type must be "Delta inventario"
And the net quantity must be the value returned by FICO in the "deltaOficial" field
And the unit and segment must be obtained from inventory stored in TRUE
And the scenario of the new movement must be official
And the operational date of the movement is inventory date minus 1 day must be used
And the start date and end date should be inventory date minus 1 day must be used
And the value for the source inventory identifier must be the inventory identifier returned by FICO in the "idInventarioTRUE" field
And the source system must be "FICO"
And assign to the movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idInventarioPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO
And the ownership unit use the text "Volumen"


@testcase=89388
Scenario: Verify new movements is created when inventory date of the period is end date of the period and record in the collection "resultadoInventarios" has a negative sign and the origin is "OFICIAL"
Given I have an inventory date is equal to end date of the period
And TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoInventarios" has a negative sign and the origin is "OFICIAL"
Then new movement is created with source node and source product as node and product of the inventory registered to TRUE
And destination node and product must be null
And the movement type must be "Delta inventario"
And the net quantity must be the value returned by FICO in the "deltaOficial" field
And the unit and segment must be obtained from inventory stored in TRUE
And the scenario of the new movement must be official
And the operational date of the movement is inventory date must be used
And the start date and end date should be inventory date must be used
And the value for the source inventory identifier must be the inventory identifier returned by FICO in the "idInventarioTRUE" field
And the source system must be "FICO"
And assign to the movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idInventarioPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO
And the ownership unit use the text "Volumen"


@testcase=89389
Scenario: Verify new movements is created when inventory date of the period is start date of the period and record in the collection "resultadoInventarios" has a positive sign and the origin is "OFICIAL"
Given I have an inventory date is equal to start date of the period
And TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoInventarios" has a positive sign and the origin is "OFICIAL"
Then new movement is created with destination node and destination product as node and product of the inventory registered to TRUE
And source node and product must be null
And the movement type must be "Delta inventario"
And the net quantity must be the value returned by FICO in the "deltaOficial" field
And the unit and segment must be obtained from inventory stored in TRUE
And the scenario of the new movement must be official
And the operational date of the movement is inventory date minus 1 day must be used
And the start date and end date should be inventory date minus 1 day must be used
And the value for the source inventory identifier must be the inventory identifier returned by FICO in the "idInventarioTRUE" field
And the source system must be "FICO"
And assign to the movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idInventarioPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO
And the ownership unit use the text "Volumen"


@testcase=89390
Scenario: Verify new movements is created when inventory date of the period is end date of the period and record in the collection "resultadoInventarios" has a positive sign and the origin is "OFICIAL"
Given I have an inventory date is equal to end date of the period
And TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoInventarios" has a positive sign and the origin is "OFICIAL"
Then new movement is created with destination node and destination product as node and product of the inventory registered to TRUE
And source node and product must be null
And the movement type must be "Delta inventario"
And the net quantity must be the value returned by FICO in the "deltaOficial" field
And the unit and segment must be obtained from inventory stored in TRUE
And the scenario of the new movement must be official
And the operational date of the movement is inventory date must be used
And the start date and end date should be inventory date must be used
And the value for the source inventory identifier must be the inventory identifier returned by FICO in the "idInventarioTRUE" field
And the source system must be "FICO"
And assign to the movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idInventarioPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO
And the ownership unit use the text "Volumen"


@testcase=89391
Scenario: Verify new movements is created when record in the collection "resultadoInventarios" has a negative sign and the origin is "CONSOLIDADO"
Given TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoInventarios" has a negative sign and the origin is "CONSOLIDADO"
Then new movement is created with source node and source product as node and product of the inventory registered to TRUE
And the destination node and product must be null
And the movement type must be "Delta inventario"
And the net quantity must be the value returned by FICO in the "deltaOficial" field
And the unit and segment must be obtained from inventory stored in TRUE
And the scenario of the new movement must be official
And the operational date, start date, and end date must be obtained from the consolidated inventory date stored in TRUE
And the value for the source inventory identifier must be the inventory identifier returned by FICO in the "idInventarioTRUE" field
And the source system must be "FICO"
And assign to the movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idInventarioPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO
And the ownership unit use the text "Volumen"


@testcase=89392
Scenario: Verify new movements is created when record in the collection "resultadoInventarios" has a positive sign and the origin is "CONSOLIDADO"
Given TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoInventarios" has a positive sign and the origin is "CONSOLIDADO"
Then new movement is created with destination node and destination product as node and product of the inventory registered to TRUE
And source node and product must be null
And the movement type must be "Delta inventario"
And the net quantity must be the value returned by FICO in the "deltaOficial" field
And the unit and segment must be obtained from inventory stored in TRUE
And the scenario of the new movement must be official
And the operational date, start date, and end date must be obtained from the consolidated inventory date stored in TRUE
And the value for the source inventory identifier must be the inventory identifier returned by FICO in the "idInventarioTRUE" field
And the source system must be "FICO"
And assign to the movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idInventarioPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO
And the ownership unit use the text "Volumen"


@testcase=89393
Scenario: Verify new movements is created when record in the collection "resultadoInventarios" has a negative sign and the origin is "DELTAOFICIAL"
Given TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoInventarios" has a negative sign and the origin is “DELTAOFICIAL”
Then new movement is created with source node and source product as the destination node and destination product of the movement registered in TRUE
And the destination node and product of the new movement must be null
And the movement type must be "Delta inventario"
And the net quantity must be the value returned by FICO in the "deltaOficial" field
And the operational date, unit, segment, start date and final date must be obtained from movement stored in TRUE
And the scenario of the new movement must be official
And the value for the source movement identifier must be the movement identifier returned by FICO in the "idMovimientoDeltaInv" field
And the source system must be "FICO"
And assign to the movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idMovimientoPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO
And the ownership unit use the text "Volumen"


@testcase=89394
Scenario: Verify new movements is created when record in the collection "resultadoInventarios" has a positive sign and the origin is "DELTAOFICIAL"
Given TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoInventarios" has a positive sign and the origin is “DELTAOFICIAL”
Then new movement is created with destination node and destination product as the source node and source product of the movement registered in TRUE
And the source node and product of the new movement must be null
And the movement type must be "Delta inventario"
And the net quantity must be the value returned by FICO in the "deltaOficial" field
And the operational date, unit, segment, start date and final date must be obtained from movement stored in TRUE
And the scenario of the new movement must be official
And the value for the source movement identifier must be the movement identifier returned by FICO in the "idMovimientoDeltaInv" field
And the source system must be "FICO"
And assign to the movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idMovimientoPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO
And the ownership unit use the text "Volumen"


@testcase=89395
Scenario: Verify new movements is created when record in the collection "resultadoMovimientos" has a negative sign and the origin is "OFICIAL"
Given TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoMovimientos" has a negative sign and the origin is "OFICIAL"
Then new movement is created with the same movement data stored in TRUE as operational date, source node, destination node, source product, destination product, product type if it exists, unit, segment, start date, and end date
And the value of the net quantity must be the value returned by FICO in the "deltaOficial" field multiplied by -1
And the movement type identifier is configures cancellation movement type
And the scenario of the new movement must be official
And the value for the source movement identifier must be the movement ID returned by FICO in the "idMovimientoTRUE" field
And the source system must be "FICO"
And assign to the new movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idMovimientoPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO and multiply it by -1
And the ownership unit use the text "Volumen"


@testcase=89396
Scenario: Verify new movements is created when record in the collection "resultadoMovimientos" has a positive sign and the origin is "OFICIAL"
Given TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoMovimientos" has a positive sign and the origin is "OFICIAL"
Then new movement is created with the same movement data stored in TRUE as operational date, source node, destination node, source product, destination product, movement type, product type if it exists, unit, segment, start date, and end date
And the value of the net quantity must be the value returned by FICO in the "deltaOficial" field
And the scenario of the new movement must be official
And the value for the source movement identifier must be the movement ID returned by FICO in the "idMovimientoTRUE" field
And the source system must be "FICO"
And assign to the new movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idMovimientoPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO
And the ownership unit use the text "Volumen"


@testcase=89397
Scenario: Verify new movements is created when record in the collection "resultadoMovimientos" has a negative sign and the origin is "CONSOLIDADO"
Given TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoMovimientos" has a negative sign and the origin is "CONSOLIDADO"
Then new movement is created with the same movement data stored in TRUE as source node, destination node, source product, destination product, product type if it exists, unit, segment, start date, and end date
And the value of the net quantity must be the value returned by FICO in the "deltaOficial" field multiplied by -1
And the movement type identifier is configures cancellation movement type
And the operational date must be equal to the start date of the consolidated movement
And the scenario of the new movement must be official
And the value for the source movement identifier must be the movement ID returned by FICO in the "idMovimientoTRUE" field
And the source system must be "FICO"
And assign to the new movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idMovimientoPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO and multiply it by -1
And the ownership unit use the text "Volumen"


@testcase=89398
Scenario: Verify new movements is created when record in the collection "resultadoMovimientos" has a positive sign and the origin is "CONSOLIDADO"
Given TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoMovimientos" has a positive sign and the origin is "CONSOLIDADO"
Then new movement is created with the same movement data stored in TRUE as source node, destination node, source product, destination product, movement type, product type if it exists, unit, segment, start date, and end date
And the operational date must be equal to the start date of the consolidated movement
And the value of the net quantity must be the value returned by FICO in the "deltaOficial" field
And the scenario of the new movement must be official
And the value for the source movement identifier must be the movement ID returned by FICO in the "idMovimientoTRUE" field
And the source system must be "FICO"
And assign to the new movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idMovimientoPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO
And the ownership unit use the text "Volumen"


@testcase=89399
Scenario: Verify new movements is created when record in the collection "resultadoMovimientos" has a negative sign and the origin is "DELTAOFICIAL"
Given TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoMovimientos" has a negative sign and the origin is "DELTAOFICIAL"
Then new movement is created with the same movement data stored in TRUE as operational date, source node, destination node, source product, destination product, product type if it exists, unit, segment, start date, and end date
And the value of the net quantity must be the value returned by FICO in the "deltaOficial" field multiplied by -1
And the movement type identifier is configures cancellation movement type
And the scenario of the new movement must be official
And the value for the source movement identifier must be the movement ID returned by FICO in the "idMovimientoTRUE" field
And the source system must be "FICO"
And assign to the new movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idMovimientoPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO and multiply it by -1
And the ownership unit use the text "Volumen"


@testcase=89400
Scenario: Verify new movements is created when record in the collection "resultadoMovimientos" has a positive sign and the origin is "DELTAOFICIAL"
Given TRUE application is processing the FICO Web API response that returns the official deltas
When record in the collection "resultadoMovimientos" has a positive sign and the origin is "DELTAOFICIAL"
Then new movement is created with the same movement data stored in TRUE as operational date, source node, destination node, source product, destination product, product type if it exists, unit, segment, start date, and end date
And the movement type must be the movement type corresponding to the cancellation type of the original movement stored in TRUE
And the value of the net quantity must be the value returned by FICO in the "deltaOficial" field
And the scenario of the new movement must be official
And the value for the source movement identifier must be the movement ID returned by FICO in the "idMovimientoTRUE" field
And the source system must be "FICO"
And assign to the new movement the official delta ticket of the segment
And ownership is registered with the owner identifier of the "idMovimientoPropietario" field returned by FICO
And the ownership value with the data of the "deltaOficial" field returned by FICO
And the ownership unit use the text "Volumen"
