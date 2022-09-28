##Parameters
param
(
	[string]$availabilityClientId,
	[string]$tenantId,
	[string]$trueResourceGroup,
	[string]$sharedResourceGroup,
	[string]$appGatewayRG,
	[string]$appGatewaySubscriptionId,
	[string]$subscriptionId,
	[string]$apim,
	[string]$appGateway,
	[string]$sqlDatabase,
	[string]$keyVault,
	[string]$storage,
	[string]$redis,
	[string]$serviceBus,
	[string]$aks,
	[string]$analysisServer,
	[string]$homologateFunctionApp,
	[string]$calculatorFunctionApp,
	[string]$blockchainFunctionApp,
	[string]$deadletterFunctionApp,
	[string]$sapFunctionApp,
	[string]$ownershipFunctionApp,
	[string]$deltaFunctionApp,
	[string]$reportingFunctionApp,
	[string]$abs,
	[string]$appServiceName
)

#ModuleAvailabilityTrueAdminSettings
$data = '{\"ModuleAvailabilityTrueAdminSettings.Resources\":[\"'+$apim+'\",\"'+$appServiceName+'\",\"'+$keyVault+'\",\"'+$appGateway+'\",\"'+$sqlDatabase+'\",\"'+$storage+'\",\"'+$redis+'\",\"'+$serviceBus+'\",\"'+$aks+'\",\"'+$analysisServer+'\"]}'
Write-Output $data
##Set Output Variable.
$key = "ModuleAvailabilityTrueAdminSettings"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"



#ModuleAvailabilityTrueApprovalsSettings
$data = '{\"ModuleAvailabilityTrueApprovalsSettings.Resources\":[\"'+$appServiceName+'\",\"'+$appGateway+'\",\"'+$sqlDatabase+'\",\"'+$keyVault+'\",\"'+$apim+'\"]}'
Write-Output $data
##Set Output Variable.
$key = "ModuleAvailabilityTrueApprovalsSettings"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"



#ModuleAvailabilityTrueLoadFilesTransportSettings
$data = '{\"ModuleAvailabilityTrueLoadFilesTransportSettings.Resources\":[\"'+$apim+'\",\"'+$appServiceName+'\",\"'+$keyVault+'\",\"'+$appGateway+'\",\"'+$sqlDatabase+'\",\"'+$storage+'\",\"'+$redis+'\",\"'+$serviceBus+'\",\"'+$aks+'\",\"'+$abs+'\",\"'+$homologateFunctionApp+'\",\"'+$blockchainFunctionApp+'\",\"'+$deadletterFunctionApp+'\",\"'+$sapFunctionApp+'\"]}'
Write-Output $data
##Set Output Variable.
$key = "ModuleAvailabilityTrueLoadFilesTransportSettings"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"



#ModuleAvailabilityTrueCutOffSettings
$data = '{\"ModuleAvailabilityTrueCutOffSettings.Resources\":[\"'+$apim+'\",\"'+$appServiceName+'\",\"'+$keyVault+'\",\"'+$appGateway+'\",\"'+$sqlDatabase+'\",\"'+$storage+'\",\"'+$redis+'\",\"'+$serviceBus+'\",\"'+$aks+'\",\"'+$abs+'\",\"'+$homologateFunctionApp+'\",\"'+$blockchainFunctionApp+'\",\"'+$deadletterFunctionApp+'\",\"'+$calculatorFunctionApp+'\",\"'+$analysisServer+'\"]}'
Write-Output $data
##Set Output Variable.
$key = "ModuleAvailabilityTrueCutOffSettings"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"



#ModuleAvailabilityTrueOperativeDeltasSettings
$data = '{\"ModuleAvailabilityTrueOperativeDeltasSettings.Resources\":[\"'+$apim+'\",\"'+$appServiceName+'\",\"'+$keyVault+'\",\"'+$appGateway+'\",\"'+$sqlDatabase+'\",\"'+$storage+'\",\"'+$redis+'\",\"'+$serviceBus+'\",\"'+$aks+'\",\"'+$abs+'\",\"'+$deltaFunctionApp+'\",\"'+$blockchainFunctionApp+'\",\"'+$deadletterFunctionApp+'\",\"'+$analysisServer+'\"]}'
Write-Output $data
##Set Output Variable.
$key = "ModuleAvailabilityTrueOperativeDeltasSettings"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"



#ModuleAvailabilityTrueOwnershipSettings
$data = '{\"ModuleAvailabilityTrueOwnershipSettings.Resources\":[\"'+$apim+'\",\"'+$appServiceName+'\",\"'+$keyVault+'\",\"'+$appGateway+'\",\"'+$sqlDatabase+'\",\"'+$storage+'\",\"'+$redis+'\",\"'+$serviceBus+'\",\"'+$aks+'\",\"'+$abs+'\",\"'+$ownershipFunctionApp+'\",\"'+$blockchainFunctionApp+'\",\"'+$deadletterFunctionApp+'\",\"'+$analysisServer+'\"]}'
Write-Output $data
##Set Output Variable.
$key = "ModuleAvailabilityTrueOwnershipSettings"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"


#ModuleAvailabilityTrueOwnershipAdjSettings
$data = '{\"ModuleAvailabilityTrueOwnershipAdjSettings.Resources\":[\"'+$apim+'\",\"'+$appServiceName+'\",\"'+$keyVault+'\",\"'+$appGateway+'\",\"'+$sqlDatabase+'\",\"'+$storage+'\",\"'+$redis+'\",\"'+$serviceBus+'\",\"'+$aks+'\",\"'+$abs+'\",\"'+$ownershipFunctionApp+'\",\"'+$blockchainFunctionApp+'\",\"'+$deadletterFunctionApp+'\",\"'+$analysisServer+'\"]}'
Write-Output $data
##Set Output Variable.
$key = "ModuleAvailabilityTrueOwnershipAdjSettings"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"


#ModuleAvailabilityTrueLoadFilesChainSettings
$data = '{\"ModuleAvailabilityTrueLoadFilesChainSettings.Resources\":[\"'+$apim+'\",\"'+$appServiceName+'\",\"'+$keyVault+'\",\"'+$appGateway+'\",\"'+$sqlDatabase+'\",\"'+$storage+'\",\"'+$redis+'\",\"'+$serviceBus+'\",\"'+$aks+'\",\"'+$abs+'\",\"'+$homologateFunctionApp+'\",\"'+$blockchainFunctionApp+'\",\"'+$deadletterFunctionApp+'\",\"'+$sapFunctionApp+'\",\"'+$analysisServer+'\"]}'
Write-Output $data
##Set Output Variable.
$key = "ModuleAvailabilityTrueLoadFilesChainSettings"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"


#ModuleAvailabilityTrueOfficialDeltasSettings
$data = '{\"ModuleAvailabilityTrueOfficialDeltasSettings.Resources\":[\"'+$apim+'\",\"'+$appServiceName+'\",\"'+$keyVault+'\",\"'+$appGateway+'\",\"'+$sqlDatabase+'\",\"'+$storage+'\",\"'+$redis+'\",\"'+$serviceBus+'\",\"'+$aks+'\",\"'+$abs+'\",\"'+$deltaFunctionApp+'\",\"'+$blockchainFunctionApp+'\",\"'+$deadletterFunctionApp+'\",\"'+$analysisServer+'\"]}'
Write-Output $data
##Set Output Variable.
$key = "ModuleAvailabilityTrueOfficialDeltasSettings"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"



#ModuleAvailabilityTrueReportsSettings
$data = '{\"ModuleAvailabilityTrueReportsSettings.Resources\":[\"'+$apim+'\",\"'+$appServiceName+'\",\"'+$keyVault+'\",\"'+$appGateway+'\",\"'+$sqlDatabase+'\",\"'+$storage+'\",\"'+$redis+'\",\"'+$serviceBus+'\",\"'+$aks+'\",\"'+$abs+'\",\"'+$reportingFunctionApp+'\",\"'+$analysisServer+'\"]}'
Write-Output $data
##Set Output Variable.
$key = "ModuleAvailabilityTrueReportsSettings"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"


#ModuleAvailabilityTrueSapPoApiSettings
$data = '{\"ModuleAvailabilityTrueSapPoApiSettings.Resources\":[\"'+$apim+'\",\"'+$appServiceName+'\",\"'+$keyVault+'\",\"'+$appGateway+'\",\"'+$sqlDatabase+'\",\"'+$storage+'\",\"'+$redis+'\",\"'+$serviceBus+'\",\"'+$aks+'\",\"'+$abs+'\",\"'+$homologateFunctionApp+'\",\"'+$blockchainFunctionApp+'\",\"'+$deadletterFunctionApp+'\",\"'+$analysisServer+'\"]}'
Write-Output $data
##Set Output Variable.
$key = "ModuleAvailabilityTrueSapPoApiSettings"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"


#AvailabilitySetting
$data= '{\"AvailabilitySettings.ResourceGroups\":[{\"Name\":\"'+$trueResourceGroup+'\",\"SubscriptionId\":\"'+$subscriptionId+'\"},{\"Name\":\"'+$sharedResourceGroup+'\",\"SubscriptionId\":\"'+$subscriptionId+'\"},{\"Name\":\"'+$appGatewayRG+'\",\"SubscriptionId\":\"'+$appGatewaySubscriptionId+'\"}],\"AvailabilitySettings.ClientId\":\"'+$availabilityClientId+'\",\"AvailabilitySettings.TenantId\":\"'+$tenantId+'\"}'
Write-Output $data
##Set Output Variable.
$key = "AvailabilitySettings"
$value = $data
Write-Output "##vso[task.setvariable variable=$key;]$value"