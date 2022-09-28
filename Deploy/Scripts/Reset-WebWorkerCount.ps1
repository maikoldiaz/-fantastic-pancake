
param(
 [Parameter(Mandatory=$true)]
 [string]
 $resourceName,

 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroupName
 )

$webAppSettings = Get-AzWebApp -ResourceGroupName $resourceGroupName -Name $resourceName -ErrorAction SilentlyContinue

if($webAppSettings)
{
    if($webAppSettings.SiteConfig.NumberOfWorkers -eq 1)
    {
        Write-Output "Settings already in place."
    }else{
        $webAppSettings.SiteConfig.NumberOfWorkers=1     
        Set-AzWebApp $webAppSettings
    }

    Write-Output "Settings Updated."
}