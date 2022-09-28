@sharedsteps=4013 @owner=jagudelos @ui @testsuite=55114 @testplan=55104 @S15 @MVP2and3
Feature: InvokeOperationalInformationWithOwnership
In order to invoke/not invoke the process for operational data with ownership loading in the analytical model
As an Admin user
The node must be approved/rejected using automatic/manual process

Background: Login
Given I am logged in as "admin"

@testcase=57710
Scenario: Verify that the process for operational data is invoked when automatic approval process ends without any error
Given I have ownership calculation data generated in the system conditionally
When I navigate to "Operating balance with ownership per node" page
And I search for a node and a raise a "automatic" approval request against it
And I click on "actions" "dropdown"
And I click on "ownershipNodeDetails" "submitToApproval" "link"
And I wait for "60" seconds for the process to end
Then I validate that the TRUE system "invoked" the process for operational data with ownership loading in the analytical model

@testcase=57711 @manual
Scenario: Verify that the process for operational data is not invoked when automatic approval process ends with error
Given I have ownership calculation data generated in the system
When I navigate to "Operating balance with ownership per node" page
And I search for a node and a raise a "automatic" approval request against it
And I click on "actions" "link"
And I click on "ownershipNodeDetails" "submitToApproval" "link"
Given I make the approval process to end with error
Then I validate that the TRUE system "didn't invoke" the process for operational data with ownership loading in the analytical model

@testcase=57712
Scenario: Verify that the process for operational data is invoked when manual approval process ends without any error
Given I have ownership calculation data generated in the system conditionally
When I navigate to "Operating balance with ownership per node" page
And I search for a node and a raise a "manual" approval request against it
And I click on "actions" "dropdown"
And I click on "ownershipNodeDetails" "submitToApproval" "link"
And I wait for "60" seconds for the process to end
And I navigate to "Approval of balance with ownership by node" page
And I wait for "10" seconds for the process to end
And I "approve" the respective request
And I wait for "60" seconds for the process to end
Then I validate that the TRUE system "invoked" the process for operational data with ownership loading in the analytical model

@testcase=57713 @manual
Scenario: Verify that the process for operational data is not invoked when manual approval process ends with error
Given I have ownership calculation data generated in the system
When I navigate to "Operating balance with ownership per node" page
And I search for a node and a raise a "manual" approval request against it
And I click on "actions" "link"
And I click on "ownershipNodeDetails" "submitToApproval" "link"
When I navigate to "Approval of balance with ownership by node" page
And I "approve" the respective request
Given I make the approval process to end with error
Then I validate that the TRUE system "didn't invoke" the process for operational data with ownership loading in the analytical model

@testcase=57714
Scenario: Verify that the process for operational data is not invoked when manual approval is rejected
Given I have ownership calculation data generated in the system conditionally
When I navigate to "Operating balance with ownership per node" page
And I search for a node and a raise a "manual" approval request against it
And I click on "actions" "dropdown"
And I click on "ownershipNodeDetails" "submitToApproval" "link"
And I wait for "60" seconds for the process to end
And I navigate to "Approval of balance with ownership by node" page
And I wait for "10" seconds for the process to end
And I "reject" the respective request
And I wait for "60" seconds for the process to end
Then I validate that the TRUE system "didn't invoke" the process for operational data with ownership loading in the analytical model

@testcase=57715 @manual
Scenario: Verify that the Azure dashboard displays the process failure information when the process of loading operational information with property to feed the analytical model ends in error
Given the process of loading operational information with property to feed the analytical model ends in error
Then I validate that Azure dashboard should display a tile with process failure information

@testcase=57716 @manual
Scenario: Verify that respective error messages are displayed at designated places when the process of loading operational information with property to feed the analytical model fails
Given invocation of loading operational information with property to feed the analytical model fails
Then the user must see the error message "Ocurrió un error en el envío de información analítica: el proceso de aprobación fue exitoso, pero el envío a analítica presentó un problema y no se completó. Por favor contacte a la mesa de ayuda para mayor información."
And I validate that the technical exception is saved in the app insights
And I validate that Azure dashboard should display a tile with process failure information
