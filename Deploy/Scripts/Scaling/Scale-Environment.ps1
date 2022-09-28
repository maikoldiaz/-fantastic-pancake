param(
   [string]
   $resourceGroupName,

   [string]
   $keyvaultName,

   [string]
   $scaleup = "false",

   [int]
   $numberOfworkersUI,

   [int]
   $numberOfworkersFA,

   [string]
   $webAppsForAppSettingsUpdate,

   [string]
   $appServicePlanNameFA,

   [string]
   $appServicePlanNameUI,

   [string]
   $tierUI,

   [string]
   $tierFA,

   [string]
   $workerSizeUI,

   [string]
   $workerSizeFA,

   [string]
   $serviceBusNameSpaceName,

   [string]
   $deployRegion,

   [string]
   $webAppNameUI,

   [string]
   $funcAppArrayForScaling
)

function Set-WorkersCount($workersCount, $webAppArray) {
   $webAppArray = $webAppArray.split(",")

   foreach ($webapp in $webAppArray) {
      # Get the app we want to configure to use "PerSiteScaling"
      $app = Get-AzWebApp -ResourceGroupName $resourceGroupName -Name $webapp

      # Modify the NumberOfWorkers setting to the desired value.
      $app.SiteConfig.NumberOfWorkers = $workersCount

      # Post updated app back to azure
      Set-AzWebApp $app
   }
}

function Set-SecretInKeyVault($secretName, $secretValue) {
   $Secret = ConvertTo-SecureString -String $secretValue -AsPlainText -Force #pragma: allowlist secret
   Set-AzKeyVaultSecret -VaultName $keyvaultName -Name $secretName -SecretValue $Secret
}

function Set-AppSettingsServiceBusConnString($serviceBusConnectionString) {
   $webAppsForAppSettingsUpdate = $webAppsForAppSettingsUpdate.split(",")
   #Update the connection string in function apps
   foreach ($webapp in $webAppsForAppSettingsUpdate) {
      # Get the app we want to configure to use "PerSiteScaling"
      $app = Get-AzWebApp -ResourceGroupName $resourceGroupName -Name $webapp -ErrorAction SilentlyContinue

    if($app -ne $null){
            $newAppSettingList = @{}

      Write-Verbose "Read all existing settings..."
      ForEach ($kvp in $app.SiteConfig.AppSettings) {
         $newAppSettingList[$kvp.Name] = $kvp.Value
      }

      $newAppSettingList.IntegrationServiceBusConnectionString = $serviceBusConnectionString

      # Post updated app back to azure
      Set-AzWebApp -ResourceGroupName $resourceGroupName -Name $webapp -AppSettings $newAppSettingList
     }
   }
}

function Set-EnvVariable($keyName, $value) {
   ##Set Output Variable.
   Write-Output "##vso[task.setvariable variable=$keyName;]$value"
}

$tag = @{description = $env:description; createdBy = $env:createdBy; tier = $tierForTag; responsible = $env:responsible; projectName = $env:projectName; companyName = $env:companyName; environment = $env:env; organizationUnit = $env:organizationUnit; creationDate = $env:creationDate; dataProfile = $env:dataProfile; }


if ($scaleup -eq "true") {

   #key vault scale Up
   $keyvaultdata = Get-AzResource -Name $keyvaultName -ResourceType "Microsoft.KeyVault/vaults" -ResourceGroupName $resourceGroupName -ExpandProperties
   $keyvaultdata.Properties.sku.name = "premium"
   Set-AzResource -ResourceId $keyvaultdata.ResourceId -Tags $keyvaultdata.Tags -Properties $keyvaultdata.Properties -Force
   Write-Output "KeyVault Scaled."

   #Service Bus ScaleUp
   #Remove the standard service bus
   #Remove-AzServiceBusNamespace -ResourceGroupName $resourceGroupName -Name $serviceBusNameSpaceName -PassThru
   #Start-Sleep 10
   #New Service Bus Creation
   $serviceBusNameSpaceName = $serviceBusNameSpaceName + "-PREMIUM"
   New-AzServiceBusNamespace -ResourceGroupName $resourceGroupName -Name $serviceBusNameSpaceName -Location $deployRegion -SkuName "Premium" -Tag $tag
   #Step 1: Get the Connection String
   $serviceBusKeys = Get-AzServiceBusKey -ResourceGroupName $resourceGroupName -Namespace $serviceBusNameSpaceName  -Name "RootManageSharedAccessKey"
   $primaryConnectionString = $serviceBusKeys.PrimaryConnectionString
   #Step 2: Set the value in the Key vault
   Set-SecretInKeyVault -secretName "intservicebusconnectionstring" -secretValue $primaryConnectionString

   Set-AppSettingsServiceBusConnString -serviceBusConnectionString $primaryConnectionString


   #Web App service plan Scaling for UI
   Set-AzAppServicePlan -Name $appServicePlanNameUI -ResourceGroupName $resourceGroupName -Tier $tierUI -WorkerSize $workerSizeUI -PerSiteScaling $true -NumberofWorkers $numberOfworkersUI
   Set-WorkersCount -workersCount $numberOfworkersUI -webAppArray $webAppNameUI

   #Function App service plan Scaling for UI
   Set-AzAppServicePlan -Name $appServicePlanNameFA -ResourceGroupName $resourceGroupName -Tier $tierFA -WorkerSize $workerSizeFA -PerSiteScaling $true -NumberofWorkers $numberOfworkersFA
   Set-WorkersCount -workersCount $numberOfworkersFA -webAppArray $funcAppArrayForScaling

   Write-Output "App Service Upgraded."

   Set-EnvVariable -keyName "serviceBusConnectionString" -value $primaryConnectionString
}
else {

   #key vault scale Down
   $keyvaultdata = Get-AzResource -Name $keyvaultName -ResourceType Microsoft.KeyVault/vaults -ResourceGroupName $resourceGroupName -ExpandProperties
   $keyvaultdata.Properties.sku.name = "standard"
   Set-AzResource -ResourceId $keyvaultdata.ResourceId -Tags $keyvaultdata.Tags -Properties $keyvaultdata.Properties -Force
   Write-Output "KeyVault Downgraded."


   #Web App service plan Downgrade for UI
   Set-AzAppServicePlan -Name $appServicePlanNameUI -ResourceGroupName $resourceGroupName -Tier $tierUI -WorkerSize $workerSizeUI -PerSiteScaling $true -NumberofWorkers $numberOfworkersUI
   Set-WorkersCount -workersCount 1 -webAppArray $webAppNameUI
   #Function App service plan Scaling for UI
   Set-AzAppServicePlan -Name $appServicePlanNameFA -ResourceGroupName $resourceGroupName -Tier $tierFA -WorkerSize $workerSizeFA -PerSiteScaling $true -NumberofWorkers $numberOfworkersFA
   Set-WorkersCount -workersCount $numberOfworkersFA -webAppArray $funcAppArrayForScaling
   Write-Output "App Service Downgraded."

   #Put the SB Connection string in key vault.
   $serviceBusNameSpaceNamePremium = $serviceBusNameSpaceName + "-PREMIUM"
   Remove-AzServiceBusNamespace -ResourceGroupName $resourceGroupName -Name $serviceBusNameSpaceNamePremium -PassThru
   $serviceBusKeys = Get-AzServiceBusKey -ResourceGroupName $resourceGroupName -Namespace $serviceBusNameSpaceName  -Name "RootManageSharedAccessKey"
   $primaryConnectionString = $serviceBusKeys.PrimaryConnectionString
   #Set the value in the Key vault
   Set-SecretInKeyVault -secretName "intservicebusconnectionstring" -secretValue $primaryConnectionString

   Set-AppSettingsServiceBusConnString -serviceBusConnectionString $primaryConnectionString

   Set-EnvVariable -keyName "serviceBusConnectionString" -value $primaryConnectionString

}