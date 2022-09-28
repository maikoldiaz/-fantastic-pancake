#parameters
param(
[Parameter(Mandatory=$true)]
[String]$resourceGroup,
[Parameter(Mandatory=$true)]
[String]$serviceBusNamespace
)

    $existing = Get-AzServiceBusAuthorizationRule -ResourceGroup $resourceGroup -Namespace $serviceBusNamespace -Name 'RootManagedSharedAccessKey' -ErrorAction SilentlyContinue
    if($existing){
        Remove-AzServiceBusAuthorizationRule -ResourceGroup $resourceGroup -Namespace $serviceBusNamespace -Name $existing.Name -force
        Write-Output "Service bus access policy deleted Successfully."
    } else {
                Write-Output "Service bus access policy not present."
    }