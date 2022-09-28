param(
 [Parameter(Mandatory=$true)]
 [hashtable]
 $AppSettings,

 [string]
 $webAppName,

 [string]
 $resourceGroupName,

 [string]
 $slot
 )

Write-Output $AppSettings

#$newAppSettings = @{"newSetting01"="newValue01";"newSetting02"="newValue02";"newSetting03"="newValue03"}
Set-AzWebAppSlot -Slot $slot -AppSettings $AppSettings -Name $webAppName -ResourceGroupName $resourceGroupName

Set-AzWebApp -AppSettings $AppSettings -Name $webAppName -ResourceGroupName $resourceGroupName
