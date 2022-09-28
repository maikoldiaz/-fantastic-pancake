@sharedsteps=4013 @owner=jagudelos @ui @testsuite=70811 @testplan=70526 @S16 @MVP2and3
Feature: OfficialMonthlyBalanceFileToBeProcessedByTheSivSystem
As a Professional Segment Balance User, I want an UI to generate
the official monthly balance  file to be processed by the SIV system

Background: Login
Given I am logged in as "admin"

@testcase=71716 @bvt @version=2 @parallel=false @independent
Scenario: Verify official monthly balance file structure that to be processed by the SIV system
Given I have logistic center with official movements information
When I navigate to "Official logistics movements and inventories" page
And I have generated official monthly balance file
And I click on "download" link on the grid
Then file should be generated with name "ReporteLogisticoOficial_["SegmentName"]_["OwnerName"]_["TicketID"].xlsx"
And it should contains columns and values as per the mapping
And I should see sheet name as "Movimientos"
And format of "ORDEN" as number without decimal places
And format of columns as text
And format of "VALOR" as number with two decimals
And "FECHA-INICIO" "FECHA-FIN" and "FECHA-CONTABILIZACION" should be in date format

@testcase=71717 @version=2 @parallel=false @independent
Scenario: Verify balance file that to be processed by the SIV system with single node
Given I have logistic center with official movements information
When I navigate to "Official logistics movements and inventories" page
And generated official monthly balance file with single node
And I click on "download" link on the grid
Then file should be generated with name "ReporteLogisticoOficial_["SegmentName"]_["OwnerName"]_["TicketID"].xlsx"
And it should contains columns and values as per the mapping
And information related to selected node only should be shown in the official balance file

@testcase=71718 @version=2 @parallel=false @independent
Scenario: Verify balance file that to be processed by the SIV system with all nodes
Given I have logistic center with official movements information
When I navigate to "Official logistics movements and inventories" page
And generated official monthly balance file with all nodes
And I click on "download" link on the grid
Then file should be generated with name "ReporteLogisticoOficial_["SegmentName"]_["OwnerName"]_["TicketID"].xlsx"
And it should contains columns and values as per the mapping
And all nodes information should be shown in the official balance file

@testcase=71719 @version=2 @parallel=false @independent
Scenario: Verify balance file that to be processed by the SIV system should include movements that meet required criteria
Given I have logistic center with official movements information
And I have movements information that is linked to selected segment
And movements within chosen period
And movements source node or destination node is linked to selected segment
And scenario equal to "Oficial"
And nodes should be send to sap is true
And owner is either EcoPetrol or Reficar
And "OfficialDeltaMessageTypeId" is equal to either "OfficialMovementDelta" or "ConsolidatedMovementDelta" or the sourceSystem is equal to "ManualMovOficial"
When I navigate to "Official logistics movements and inventories" page
And generated official monthly balance file
And I click on "download" link on the grid
Then file should be generated with name "ReporteLogisticoOficial_["SegmentName"]_["OwnerName"]_["TicketID"].xlsx"
And it should contains columns and values as per the mapping
And include all these movements into official balance file that to be processed by the SIV system

@testcase=71720 @version=2 @parallel=false @independent
Scenario: Verify balance file that to be processed by the SIV system should not include movements that not meet required criteria
Given I have logistic center with official movements information
And I have movements information that is not linked to selected segment
And movements are not within chosen period
And movements source node or destination node is not linked to selected segment
And nodes should be send to sap is false
And owner is neither EcoPetrol nor Reficar
And "OfficialDeltaMessageTypeId" is not equal to neither "OfficialMovementDelta" nor "ConsolidatedMovementDelta" or the sourceSystem is not equal to "ManualMovOficial"
When I navigate to "Official logistics movements and inventories" page
And generated official monthly balance file that not meet required criteria
And I click on "viewError" link on the grid
Then it is failed with error message as "No hay información logística oficial para los criterios dados."

@testcase=71721 @version=2 @parallel=false @independent
Scenario: Verify whether balance file consider cancellation type movements when it meet required criteria
Given I have logistic center with official movements information that have annulation
And I have a cancelaltion movement type linked to movement type
And it is in active state
When I navigate to "Official logistics movements and inventories" page
And generated official monthly balance file
And I click on "download" link on the grid
Then file should be generated with name "ReporteLogisticoOficial_["SegmentName"]_["OwnerName"]_["TicketID"].xlsx"
And it should contains columns and values as per the mapping
And cancellation movement type movements should be included in official balance file

@testcase=71722 @bvt @version=2 @parallel=false @independent
Scenario: Verify value of movement column in balance file when it meet tautology table and cancelation criterion
Given I have logistic center with official movements information that have annulation as per tautology
And I have movements as per tautology table
And I have cancellation movements that met cancellation criteria
When I navigate to "Official logistics movements and inventories" page
And generated official monthly balance file
And I click on "download" link on the grid
Then file should be generated with name "ReporteLogisticoOficial_["SegmentName"]_["OwnerName"]_["TicketID"].xlsx"
And it should contains columns and values as per the mapping
And value of movement column in balance file should be based on tautology table and cancelation criterion

@testcase=71723 @version=2 @parallel=false @independent
Scenario: Verify balance file could not be generated when no homologation exists between True to Siv
Given I have logistic center with official movements information
And I do not have homologation between true to siv
When I navigate to "Official logistics movements and inventories" page
And generated official monthly balance file
And I click on "viewError" link on the grid
Then error message should be displayed as "No se encontró una homologación entre TRUE y SIV para el tipo de movimiento [MovementType]."
And stop the process

@testcase=71724 @version=2 @parallel=false @independent
Scenario: Verify balance file is generated when it met required parameters and have homologation exists between True to Siv
Given I have logistic center with official movements information
And I have homologation between true to siv for logistic movement type
And I have homologation between true to siv for original movement type
And I have movements as per tautology table
And I have cancellation movements that met cancellation criteria
When I navigate to "Official logistics movements and inventories" page
And generated official monthly balance file
And I click on "download" link on the grid
Then file should be generated with name "ReporteLogisticoOficial_["SegmentName"]_["OwnerName"]_["TicketID"].xlsx"
And it should contains columns and values as per the mapping
And official balance file should contain all movements details
And the original value of the movement type should not be overrided with the value found in new property


@testcase=71725 @bvt @version=2 @parallel=false @independent
Scenario: Verify balance file is having information that is transformed by relationship table
Given I have logistic center with official movements information that have annulation as per relationship table
And I have homologation between true to siv for logistic movement type
And I have homologation between true to siv for original movement type
And I have movements as per tautology table
And I have cancellation movements that met cancellation criteria
When I navigate to "Official logistics movements and inventories" page
And I have generated official monthly balance file
And I click on "download" link on the grid
Then file should be generated with name "ReporteLogisticoOficial_["SegmentName"]_["OwnerName"]_["TicketID"].xlsx"
And it should contains columns and values as per the mapping
And transformations for all cancelation movements according to the relationship settings should be performed
And value of the net amount of the movement should be replaced with the absolute value of the net amount of the annulation movement

@testcase=71726 @manual @version=2
Scenario: Verify official monthly balance file generation process is failed
Given I have logistic center with official movements information
When I navigate to "Official logistics movements and inventories" page
And I have generated official monthly balance file
But file generation process is failed
Then stop the processing
And save the error
And do not generate any file

@testcase=72637 @version=2 @parallel=false @independent
Scenario: Verify official monthly balance file generation process is failed when same product, logistic centre and storage location without annulation
Given I have official movements information with same product, logistic centre and storage location without annulation
When I navigate to "Official logistics movements and inventories" page
And I have generated official monthly balance file
But file generation process is failed
And I click on "viewError" link on the grid
Then error message should be displayed as "La combinación de producto, centro logístico y almacén es inválida y no está considerada para envío a SIV." 
And stop the process

@testcase=72638 @version=2 @parallel=false @independent
Scenario: Verify official monthly balance file generation process is failed when same product, logistic centre and storage location with annulation
Given I have official movements information with same product, logistic centre and storage location with annulation
When I navigate to "Official logistics movements and inventories" page
And I have generated official monthly balance file
But file generation process is failed
And I click on "viewError" link on the grid
Then error message should be displayed as "La combinación de producto, centro logístico y almacén es inválida y no está considerada para envío a SIV." 
And stop the process