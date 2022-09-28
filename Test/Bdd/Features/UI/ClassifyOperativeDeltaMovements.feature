@sharedsteps=66825 @owner=jagudelos @ui @MVP2and3 S15 @testsuite=61555 @testplan=61542 @parallel=false
Feature: ClassifyOperativeDeltaMovements
As TRUE system, I need to classify operative delta movements
to send to FICO in the ownership calculation request

Background: Login
	Given I am logged in as "admin"

@testcase=66826 @bvt @version=2 @BVT2
Scenario: Verify the value in idTipoMivimiento field when service return data in the "movimientos" of FICO collection for the annulation movements generated by opeartive Delta
	Given I have "ownershipnodes" created
	When I have an operational movement with a cutoff ticket and an operational delta ticket
	And I have an operational date of the movement is equal to the date of the period day
	And I have source or destination node of the movement belongs to the segment on the day of the period
	And this movement has a source movement identifier
	And I create the Annulation Movement for these updated movements
	And I have generate Operative Delta Ticket for this segment
	And I should generate cutoff and ownership for operational delta for the next day
	And I Verify that System should send the information details to the FICO with Data
	Then I Verify that value in the "tipoMovimiento" field should be "ANULACION" in "ownershiprule" collection for all the annulation movements generated by Operative Delta

@testcase=66827 
Scenario: Verify Store a movement generated by a collaboration agreement
	Given I have ownershipcalculation for segment with events information for same segment within the Range of OwnershipCalculation date
	And Verify that System should send the information details to the FICO "with" Data
	When the service returns data in "movimientosNuevos" collection
	And the value of the field "tipoAcuerdo" is equal to "COLABORACION"
	Then system should register the new movements with ownership details
	And Verify that Store input and output movements using the node identifier and product identifier returned by FICO