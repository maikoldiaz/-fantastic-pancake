@sharedsteps=7539 @owner=jagudelos @api @testplan=55104 @testsuite=55124 @S14 @MVP2and3 @parallel=false
Feature: ValidateOfficialPointsInformationReportedBySAPPO
As TRUE system,
I need to validate the information of the official points reported by SAP PO
to ensure data consistency in the application

Background: Login
Given I am authenticated as "admin"
@testcase=56785 @bvt @manual @version=2
Scenario: Verify the validation for malformed JSON
Given I have data to process "Movements" in system
When I have 1 movement with all mandatory attributes
And I register "Movements" in system
And the json object do not have the defined structure
Then the response should fail with technical exception

@testcase=56786 @bvt @version=2
Scenario: Verify the validation for data type errors
Given I have data to process "Movements" in system
When I have 1 movement with all mandatory attributes
And I register "Movements" in system
And the json object do not have the data types defined
Then the response should fail with data type related technical exception

@testcase=56787 @bvt @version=2
Scenario: Verify the validation when the json object has more than two movements
Given I have data to process "Movements" in system
When I have 3 movement with all mandatory attributes
And I register "Movements" in system
And the json object has more than two objects
Then the response should fail with message "Solo puede existir un movimiento como punto oficial y un movimiento de respaldo."

@testcase=56788 @bvt @version=2
Scenario: Verify the validation when the movements do not have the "officialInformation" collection
Given I have data to process "Movements" in system
When I have 1 movement without official information attribute
And I register "Movements" in system
And the movements do not have the officialInformation collection
Then the response should fail with message "La colección {officialInformation} es obligatoria."

@testcase=56789 @bvt @version=2
Scenario: Verify the validation when any of the movements do not have the global identifier
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And any of the movements do not have the global identifier
Then the response should fail with message "Id Movimiento Global (GlobalMovementId) es obligatorio"

@testcase=56790 @bvt @version=2
Scenario: Verify the validation when both of the movements are official movements
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And two movements arrive and both are marked as official points
Then the response should fail with message "Solo puede existir un movimiento como punto oficial."

@testcase=56791 @bvt @version=2
Scenario: Verify the validation when received a official movement without backup movement
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And two movements arrive and only one of them is marked as an official point but does not have the backup movement identifier
Then the response should fail with message "El movimiento del punto oficial debe tener el identificador del movimiento de respaldo."

@testcase=56792 @bvt @version=2
Scenario: Verify the validation when received a official movement with incorrect backup movement
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And two movements arrive and only one of them is marked as an official point but it has the incorrect backup movement identifier because this does not match the movement identifier marked as unofficial
Then the response should fail with message "El identificador del movimiento marcado como No oficial debe ser igual al identificador del movimiento de respaldo asignado al movimiento del punto oficial."

@testcase=56793 @bvt @version=2
Scenario: Verify the validation when received two different global identifiers
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And two movements arrive and the global identifier of them is different
Then the response should fail with message "Los movimientos deben tener el mismo identificador global."

@testcase=56794 @bvt @version=2
Scenario: Verify the validation when received a single unofficial movement
Given I have data to process "Movements" in system
When I have 1 movement with isOfficial and globalMovementId and backupMovementId under OfficialInformation
And I register "Movements" in system
And only one movement arrives and is not marked as official
Then the response should fail with message "Los movimientos sin movimiento de respaldo deben estar marcados como puntos oficiales."

@testcase=56795 @bvt @version=2
Scenario: Verify the validation when the movement is not registered in TRUE
Given I have data to process "Movements" in system
When I have 1 movement with isOfficial and globalMovementId and backupMovementId under OfficialInformation
And I register "Movements" in system
And only one movement arrives and the movement is marked as official and it has global identifier but is not registered in TRUE
Then the response should fail with message "El movimiento no se encuentra registrado en la aplicación."

@testcase=56796 @bvt @version=2
Scenario: Verify the validation when received a single movement with incorrect data
Given I have data to process "Movements" in system
When I have 1 movement with isOfficial and globalMovementId and backupMovementId under OfficialInformation
And I register "Movements" in system
And only one movement arrives and the movement has incorrect data
Then the response should fail with message "Los datos del movimiento recibido no corresponden con los datos del último movimiento reportado por la aplicación TRUE como  punto de transferencia."

@testcase=56797 @bvt @version=2
Scenario: Verify the validation when both the movements are not registered in TRUE
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And two movements arrive and both are not registered in TRUE
Then the response should fail with message "Los movimientos no se encuentran registrados en la aplicación. Para realizar el proceso por lo menos uno de los movimientos debe estar registrado en la aplicación."

@testcase=56798 @bvt @version=2
Scenario: Verify the validation when received both movements with incorrect data
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And two movements arrive and both the movements have incorrect data
Then the response should fail with message "Los datos del movimiento (Movement Id) no corresponden con los datos del último movimiento reportado por la aplicación TRUE como punto de transferencia." and with relevant movementid

@testcase=56799 @bvt @version=2
Scenario: Verify the functionality when all the validations are met
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And all the validations are met
Then the response should return a successful status code and processing of movements should happen

@testcase=56800 @bvt @version=2
Scenario: Verify the validation when received two movements with different data
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And two movements arrive and both the movements have different data on the source node destination node source product and the destination product
Then the response should fail with message "Los movimientos deben tener el mismo nodo origen, nodo destino, producto origen y producto destino."

@testcase=57657 @bvt
Scenario: Verify the validation when received a single movement and the movement was not reported to SAP
Given I have data to process "Movements" in system
When I have 1 movement with isOfficial and globalMovementId and backupMovementId under OfficialInformation
And I register "Movements" in system
And only one movement arrives and the movement was not reported to SAP
Then the response should fail with message "El movimiento no ha sido reportado a SAP PO como punto de transferencia."

@testcase=57658 @bvt
Scenario: Verify the validation when received two movements and both the movements were not reported to SAP
Given I have data to process "Movements" in system
When I have 2 movement with all mandatory attributes
And I register "Movements" in system
And two movements arrive and both of the movements were not reported to SAP
Then the response should fail with message "Ninguno de los movimientos ha sido reportado a SAP PO como punto de transferencia."
