@owner=jagudelos @ui @testsuite=70797 @testplan=70526 @S16 @manual
Feature: AlertsForSystemCriticalEvents
As architect I need to implement an alerting strategy in order to report system critical events
@testcase=72747
Scenario Outline: Check Resource Group name
Given I am logged in the azure portal
When I navigate to Resource Groups
Then I should see Resource Group name as RG-AEU-ECP-<ENV>-TRUE
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72748
Scenario Outline: Check Application Group name
Given I am logged in the azure portal
When I navigate to Application Groups
Then I should see Application Group name as AG-ECP-<ENV>-ECP-TRUE
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72749
Scenario Outline: Check alerts are disabled in lower environments
Given I am logged in the azure portal
When I access the alerts option from Azure portal
Then Alerts for <ENV> environment should be disabled
And Alerts for PRD environment should be enabled
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
@testcase=72750
Scenario Outline: Verify alert configurations for Unhealthy Resources
Given Alert rules were defined for Unhealthy Resources
When I access the alerts option from Azure portal
Then Alert should have name as RA-AEU-ECP-<ENV>-TrueUnhealtyResources
And Rule must be defined for alert to be sent only when a resource is not available or is degraded
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72751
Scenario Outline: Verify alert configurations for UI Service Plan CPU Consumption
Given Alert rules were defined for UI Service Plan CPU Consumption
When I access the alerts option from Azure portal
Then Alert should have name as RA-AEU-ECP-<ENV>-UIServicePlanCPUConsumption
And Rule must be defined for alert to be sent only if the CPU percentage is in average greater than 75% for a period of 30 minutes taken every 15 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72752
Scenario: Verify alert is sent for UI Service Plan CPU Consumption
Given Alert rules are defined for UI Service Plan CPU Consumption
When CPU percentage is in average greater than 75% for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72753
Scenario: Verify alert is not sent for UI Service Plan CPU Consumption
Given Alert rules are defined for UI Service Plan CPU Consumption
When CPU percentage is in average less than 75% for a period of 30 minutes or CPU percentage is in average greater than 75% for a period of less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72754
Scenario Outline: Verify alert configurations for Functions Service Plan CPU Consumption
Given Alert rules were defined for Functions Service Plan CPU Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-FunctionsServicePlanCPUConsumption
And Rule must be defined for alert to be sent only if the CPU percentage is in average greater than 75% for a period of 30 minutes taken every 15 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72755
Scenario: Verify alert is sent for Functions Service Plan CPU Consumption
Given Alert rules are defined for Functions Service Plan CPU Consumption
When CPU percentage is in average greater than 75% for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72756
Scenario: Verify alert is not sent for Functions Service Plan CPU Consumption
Given Alert rules are defined for Functions Service Plan CPU Consumption
When CPU percentage is in average less than 75% for a period of 30 minutes or CPU percentage is in average greater than 75% for a period of less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72757
Scenario Outline: Verify alert configurations for Functions Service Plan Memory Consumption
Given Alert rules were defined for Functions Service Plan Memory Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-TRUEFunctionsServicePlanMemoryConsumption
And Rule must be defined for alert to be sent only if the memory percentage is in average greater than 75% for a period of 30 minutes taken every 15 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72758
Scenario: Verify alert is sent for Functions Service Plan Memory Consumption
Given Alert rules are defined for Functions Service Plan Memory Consumption
When Memory percentage is in average greater than 75% for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72759
Scenario: Verify alert is not sent for Functions Service Plan Memory Consumption
Given Alert rules are defined for Functions Service Plan Memory Consumption
When Memory percentage is in average less than 75% for a period of 30 minutes or Memory percentage is in average greater than 75% for a period of less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72760
Scenario Outline: Verify alert configurations for UI Service Plan Memory Consumption
Given Alert rules were defined for UI Service Plan Memory Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-TRUEUIServicePlanMemoryConsumption
And Rule must be defined for alert to be sent only if the memory percentage is in average greater than 75% for a period of 30 minutes taken every 15 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72761
Scenario: Verify alert is sent for UI Service Plan Memory Consumption
Given Alert rules are defined for UI Service Plan Memory Consumption
When Memory percentage is in average greater than 75% for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72762
Scenario: Verify alert is not sent for UI Service Plan Memory Consumption
Given Alert rules are defined for UI Service Plan Memory Consumption
When Memory percentage is in average less than 75% for a period of 30 minutes or Memory percentage is in average greater than 75% for a period of less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72763
Scenario Outline: Verify alert configurations for UI Server Errors
Given Alert rules were defined for UI Server Errors
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-TRUEUIServerErrors
And Rule must be defined for alert to be sent only if the number of total server errors (HTTP server errors) is greater than 500 for a period of 15 minutes taken every 15 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72764
Scenario: Verify alert is sent for UI Server Errors
Given Alert rules are defined for UI Service Plan Memory Consumption
When Memory percentage is in average greater than 75% for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72765
Scenario: Verify alert is not sent for UI Server Errors
Given Alert rules are defined for UI Service Plan Memory Consumption
When Memory percentage is in average less than 75% for a period of 30 minutes or Memory percentage is in average greater than 75% for a period of less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72766
Scenario Outline: Verify alert configurations for Service Bus CPU Consumption
Given Alert rules were defined for Service Bus CPU Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-TRUEServiceBusCPUConsumption
And Rule must be defined for alert to be sent only if the CPU percentage is in average greater than 75% for a period of 30 minutes taken every 15 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72767
Scenario: Verify alert is sent for Service Bus CPU Consumption
Given Alert rules are defined for Service Bus CPU Consumption
When CPU percentage is in average greater than 75% for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72768
Scenario: Verify alert is not sent for Service Bus CPU Consumption
Given Alert rules are defined for Service Bus CPU Consumption
When CPU percentage is in average less than 75% for a period of 30 minutes or CPU percentage is in average greater than 75% for a period of less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72769
Scenario Outline: Verify alert configurations for Service Bus Memory Consumption
Given Alert rules were defined for Service Bus Memory Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-TRUEServiceBusMemoryConsumption
And Rule must be defined for alert to be sent only if the memory percentage is in average greater than 75% for a period of 30 minutes taken every 15 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72770
Scenario: Verify alert is sent for Service Bus Memory Consumption
Given Alert rules are defined for Service Bus Memory Consumption
When Memory percentage is in average greater than 75% for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72771
Scenario: Verify alert is not sent for Service Bus Memory Consumption
Given Alert rules are defined for Service Bus Memory Consumption
When Memory percentage is in average less than 75% for a period of 30 minutes or memory percentage is in average greater than 75% for a period of less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72772
Scenario Outline: Verify alert configurations for Block Chain CPU Consumption
Given Alert rules were defined for Block Chain CPU Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-TRUEBlockChainCPUConsumption
And Rule must be defined for alert to be sent only if the CPU percentage is in average greater than 75% for a period of 30 minutes taken every 15 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72773
Scenario: Verify alert is sent for Block Chain CPU Consumption
Given Alert rules are defined for Block Chain CPU Consumption
When CPU percentage is in average greater than 75% for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72774
Scenario: Verify alert is not sent for Block Chain CPU Consumption
Given Alert rules are defined for Block Chain CPU Consumption
When CPU percentage is in average less than 75% for a period of 30 minutes or CPU percentage is in average greater than 75% for a period of less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72775
Scenario Outline: Verify alert configurations for Block Chain Memory Consumption
Given Alert rules were defined for Block Chain Memory Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-TRUEBlockChainMemoryConsumption
And Rule must be defined for alert to be sent only if the memory percentage is in average greater than 75% for a period of 30 minutes taken every 15 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72776
Scenario: Verify alert is sent for Block Chain Memory Consumption
Given Alert rules are defined for Block Chain Memory Consumption
When Memory percentage is in average greater than 75% for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72777
Scenario: Verify alert is not sent for Block Chain Memory Consumption
Given Alert rules are defined for Block Chain Memory Consumption
When Memory percentage is in average less than 75% for a period of 30 minutes or memory percentage is in average greater than 75% for a period of less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72778
Scenario Outline: Verify alert configurations for Controller CPU Consumption
Given Alert rules were defined for Controller CPU Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-trueControllerCPUConsumption
And Rule must be defined for alert to be sent with 75% as threshold, triggering the alert if there are 2 consecutive breaches, aggregate on container name, for a period of 60 minutes, every 5 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72779
Scenario: Verify alert is sent for Controller CPU Consumption
Given Alert rules are defined for Controller CPU Consumption
When CPU percentages reaches threshold of 75% and there are 2 consecutive such breaches, aggregate on container name for a period of 60 minutes
Then Alert should be triggered taken every 5 minutes
And I should receive the alert mail
@testcase=72780
Scenario: Verify alert is not sent for Controller CPU Consumption
Given Alert rules are defined for Controller CPU Consumption
When CPU percentage doesn't reach threshold of 75% or there are not 2 consecutive breaches or period is less than 60 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72781
Scenario Outline: Verify alert configurations for ana Controller CPU Consumption
Given Alert rules were defined for ana Controller CPU Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-true-anaControllerCPUConsumption
And Rule must be defined for alert to be sent with 75% as threshold, triggering the alert if there are 2 consecutive breaches, aggregate on container name, for a period of 60 minutes, every 5 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72782
Scenario: Verify alert is sent for ana Controller CPU Consumption
Given Alert rules are defined for ana Controller CPU Consumption
When CPU percentages reaches threshold of 75% and there are 2 consecutive such breaches, aggregate on container name for a period of 60 minutes
Then Alert should be triggered taken every 5 minutes
And I should receive the alert mail
@testcase=72783
Scenario: Verify alert is not sent for ana Controller CPU Consumption
Given Alert rules are defined for ana Controller CPU Consumption
When CPU percentage doesn't reach threshold of 75% or there are not 2 consecutive breaches or period is less than 60 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72784
Scenario Outline: Verify alert configurations for sap Controller CPU Consumption
Given Alert rules were defined for sap Controller CPU Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-true-sapControllerCPUConsumption
And Rule must be defined for alert to be sent with 75% as threshold, triggering the alert if there are 2 consecutive breaches, aggregate on container name, for a period of 60 minutes, every 5 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72785
Scenario: Verify alert is sent for sap Controller CPU Consumption
Given Alert rules are defined for sap Controller CPU Consumption
When CPU percentages reaches threshold of 75% and there are 2 consecutive such breaches, aggregate on container name for a period of 60 minutes
Then Alert should be triggered taken every 5 minutes
And I should receive the alert mail
@testcase=72786
Scenario: Verify alert is not sent for sap Controller CPU Consumption
Given Alert rules are defined for sap Controller CPU Consumption
When CPU percentage doesn't reach threshold of 75% or there are not 2 consecutive breaches or period is less than 60 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72787
Scenario Outline: Verify alert configurations for flow Controller CPU Consumption
Given Alert rules were defined for flow Controller CPU Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-true-flowControllerCPUConsumption
And Rule must be defined for alert to be sent with 75% as threshold, triggering the alert if there are 2 consecutive breaches, aggregate on container name, for a period of 60 minutes, every 5 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72788
Scenario: Verify alert is sent for flow Controller CPU Consumption
Given Alert rules are defined for flow Controller CPU Consumption
When CPU percentages reaches threshold of 75% and there are 2 consecutive such breaches, aggregate on container name for a period of 60 minutes
Then Alert should be triggered taken every 5 minutes
And I should receive the alert mail
@testcase=72789
Scenario: Verify alert is not sent for flow Controller CPU Consumption
Given Alert rules are defined for flow Controller CPU Consumption
When CPU percentage doesn't reach threshold of 75% or there are not 2 consecutive breaches or period is less than 60 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72790
Scenario Outline: Verify alert configurations for Controller Memory Consumption
Given Alert rules were defined for Controller Memory Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-trueControllerMemoryConsumption
And Rule must be defined for alert to be sent with 75% as threshold, triggering the alert if there are 2 consecutive breaches, aggregate on container name, for a period of 60 minutes, every 5 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72791
Scenario: Verify alert is sent for Controller Memory Consumption
Given Alert rules are defined for Controller Memory Consumption
When Memory percentages reaches threshold of 75% and there are 2 consecutive such breaches, aggregate on container name for a period of 60 minutes
Then Alert should be triggered taken every 5 minutes
And I should receive the alert mail
@testcase=72792
Scenario: Verify alert is not sent for Controller Memory Consumption
Given Alert rules are defined for Controller Memory Consumption
When Memory percentage doesn't reach threshold of 75% or there are not 2 consecutive breaches or period is less than 60 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72793
Scenario Outline: Verify alert configurations for ana Controller Memory Consumption
Given Alert rules were defined for ana Controller Memory Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-true-anaControllerMemoryConsumption
And Rule must be defined for alert to be sent with 75% as threshold, triggering the alert if there are 2 consecutive breaches, aggregate on container name, for a period of 60 minutes, every 5 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72794
Scenario: Verify alert is sent for ana Controller Memory Consumption
Given Alert rules are defined for ana Controller Memory Consumption
When Memory percentages reaches threshold of 75% and there are 2 consecutive such breaches, aggregate on container name for a period of 60 minutes
Then Alert should be triggered taken every 5 minutes
And I should receive the alert mail
@testcase=72795
Scenario: Verify alert is not sent for ana Controller Memory Consumption
Given Alert rules are defined for ana Controller Memory Consumption
When Memory percentage doesn't reach threshold of 75% or there are not 2 consecutive breaches or period is less than 60 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72796
Scenario Outline: Verify alert configurations for sap Controller Memory Consumption
Given Alert rules were defined for sap Controller Memory Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-true-sapControllerMemoryConsumption
And Rule must be defined for alert to be sent with 75% as threshold, triggering the alert if there are 2 consecutive breaches, aggregate on container name, for a period of 60 minutes, every 5 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72797
Scenario: Verify alert is sent for sap Controller Memory Consumption
Given Alert rules are defined for sap Controller Memory Consumption
When Memory percentages reaches threshold of 75% and there are 2 consecutive such breaches, aggregate on container name for a period of 60 minutes
Then Alert should be triggered taken every 5 minutes
And I should receive the alert mail
@testcase=72798
Scenario: Verify alert is not sent for sap Controller Memory Consumption
Given Alert rules are defined for sap Controller Memory Consumption
When Memory percentage doesn't reach threshold of 75% or there are not 2 consecutive breaches or period is less than 60 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72799
Scenario Outline: Verify alert configurations for flow Controller Memory Consumption
Given Alert rules were defined for flow Controller Memory Consumption
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-true-flowControllerMemoryConsumption
And Rule must be defined for alert to be sent with 75% as threshold, triggering the alert if there are 2 consecutive breaches, aggregate on container name, for a period of 60 minutes, every 5 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72800
Scenario: Verify alert is sent for flowController Memory Consumption
Given Alert rules are defined for flow Controller Memory Consumption
When Memory percentages reaches threshold of 75% and there are 2 consecutive such breaches, aggregate on container name for a period of 60 minutes
Then Alert should be triggered taken every 5 minutes
And I should receive the alert mail
@testcase=72801
Scenario: Verify alert is not sent for flow Controller Memory Consumption
Given Alert rules are defined for flow Controller Memory Consumption
When Memory percentage doesn't reach threshold of 75% or there are not 2 consecutive breaches or period is less than 60 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72802
Scenario Outline: Verify alert configurations for AKS Nodes Available Disk
Given Alert rules were defined for AKS Nodes Available Disk
When I access the alerts option from Azure portal
Then Alert should have name as AR-AEU-ECP-<ENV>-AKSNodesAvailableDisk
And Rule must be defined for query to return cluster nodes disks which exceed 90% free space used and alert to be sent with 90% as threshold, triggering the alert if there are 2 consecutive breaches, aggregate on container name, for a period of 60 minutes, every 5 minutes
Examples:
| ENV |
| DEV |
| QAS |
| UAT |
| STG |
| PRD |
@testcase=72803
Scenario: Verify alert is sent for AKS Nodes Available Disk
Given Alert rules are defined for AKS Nodes Available Disk
When Disk consumption percentage reaches threshold of 90% and there are 2 consecutive such breaches, aggregate on container name for a period of 60 minutes
Then Alert should be triggered taken every 5 minutes
And I should receive the alert mail
@testcase=72804
Scenario: Verify alert is not sent for AKS Nodes Available Disk
Given Alert rules are defined for AKS Nodes Available Disk
When Disk consumption percentage doesn't reach threshold of 90% or there are not 2 consecutive breaches or period is less than 60 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72805
Scenario: Verify alert configurations for Registration Failure
Given Alert rules were defined for Registration Failure
When I access the alerts option from Azure portal
Then Alert should have name as RegistrationFailure
And Rule must be defined for alert to be sent for more than 100 failure events of the movements and inventories registration process for a period of 30 minutes taken every 15 minutes
@testcase=72806
Scenario: Verify alert is sent for Registration Failure
Given Alert rules are defined for Registration Failure
When There are more than 100 failure events of the movements and inventories registration process for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72807
Scenario: Verify alert is not sent for Registration Failure
Given Alert rules are defined for Registration Failure
When There is less than or equal to 100 failure events of the movements and inventories registration process or period is less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72808
Scenario: Verify alert configurations for Cutoff Failure
Given Alert rules were defined for Cutoff Failure
When I access the alerts option from Azure portal
Then Alert should have name as CutoffFailure
And Rule must be defined for alert to be sent for more than 20 failure events of the operative cut off process for a period of 30 minutes taken every 15 minutes
@testcase=72809
Scenario: Verify alert is sent for Cutoff Failure
Given Alert rules are defined for Cutoff Failure
When There are more than 20 failure events of the operative cut off process for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72810
Scenario: Verify alert is not sent for Cutoff Failure
Given Alert rules are defined for Cutoff Failure
When There is less than 20 failure events of the operative cut off process or period is less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72811
Scenario: Verify alert configurations for Operative Delta Failure
Given Alert rules were defined for Operative Delta Failure
When I access the alerts option from Azure portal
Then Alert should have name as OperativeDeltaFailure
And Rule must be defined for alert to be sent for more than 20 failure events of the operative deltas calculation for a period of 30 minutes taken every 15 minutes
@testcase=72812
Scenario: Verify alert is sent for Operative Delta Failure
Given Alert rules are defined for Operative Delta Failure
When There are more than 20 failure events of the operative deltas calculation for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72813
Scenario: Verify alert is not sent for Operative Delta Failure
Given Alert rules are defined for Operative Delta Failure
When There is less than 20 failure events of the operative deltas calculation or period is less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72814
Scenario: Verify alert configurations for Ownership Failure
Given Alert rules were defined for Ownership Failure
When I access the alerts option from Azure portal
Then Alert should have name as OwnershipFailure
And Rule must be defined for alert to be sent for more than 20 failure events of the official deltas calculation for a period of 30 minutes taken every 15 minutes
@testcase=72815
Scenario: Verify alert is sent for Ownership Failure
Given Alert rules are defined for Ownership Failure
When There are more than 20 failure events of the official deltas calculation for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72816
Scenario: Verify alert is not sent for Ownership Failure
Given Alert rules are defined for Ownership Failure
When There is less than 20 failure events of the official deltas calculation or period is less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72817
Scenario: Verify alert configurations for Official Delta Failure
Given Alert rules were defined for Official Delta Failure
When I access the alerts option from Azure portal
Then Alert should have name as OfficialDeltaFailure
And Rule must be defined for alert to be sent for more than 20 failure events of the operation deltas calculation for a period of 30 minutes taken every 15 minutes
@testcase=72818
Scenario: Verify alert is sent for Official Delta Failure
Given Alert rules are defined for Official Delta Failure
When There are more than 20 failure events of the operation deltas calculation for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72819
Scenario: Verify alert is not sent for Official Delta Failure
Given Alert rules are defined for Official Delta Failure
When There is less than 20 failure events of the operation deltas calculation or period is less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
@testcase=72820
Scenario: Verify alert configurations for Official Transfer Point Registration Failure
Given Alert rules were defined for Official Transfer Point Registration Failure
When I access the alerts option from Azure portal
Then Alert should have name as OfficialTransferPointRegistrationFailure
And Rule must be defined for alert to be sent for more than 20 failure events of the registration of the official movements related with transfer points for a period of 30 minutes taken every 15 minutes
@testcase=72821
Scenario: Verify alert is sent for Official Transfer Point Registration Failure
Given Alert rules are defined for Official Transfer Point Registration Failure
When There are more than 20 failure events of the registration of the official movements related with transfer points for a period of 30 minutes
Then Alert should be triggered taken every 15 minutes
And I should receive the alert mail
@testcase=72822 
Scenario: Verify alert is not sent for Official Transfer Point Registration Failure
Given Alert rules are defined for Official Transfer Point Registration Failure
When There is less than 20 failure events of the registration of the official movements related with transfer points or period is less than 30 minutes
Then Alert should not be triggered
And I should not receive alert mail
