param(
[Parameter(Mandatory=$True)]
[string]
$resourceGroup,

[Parameter(Mandatory=$True)]
[string]
$virtualNetworkName,

[Parameter(Mandatory=$True)]
[string]
$subnetName,

[Parameter(Mandatory=$True)]
[string]
$AddServiceEndpoint

)

Set-Item Env:\SuppressAzurePowerShellBreakingChangeWarnings "true"

#Validate virtual network
$vnet= Get-AzVirtualNetwork -Name $virtualNetworkName -ResourceGroup $resourceGroup -ErrorAction SilentlyContinue
if(!$vnet) {
    Write-Output "Virtual Network '$virtualNetworkName' does not exists";
    Write-Output "Create a Virtual Network"
}
else {
    Write-Output "Using Virtual Network: '$virtualNetworkName'"
}


$vnet= Get-AzVirtualNetwork -Name $virtualNetworkName -ResourceGroup $resourceGroup -ErrorAction SilentlyContinue | Get-AzVirtualNetworkSubnetConfig -Name $subnetName

#Get existing service endpoints
$ServiceEndPoint = New-Object 'System.Collections.Generic.List[String]'
$vnet.ServiceEndpoints | ForEach-Object { $ServiceEndPoint.Add($_.service) }
$delegations=$vnet.Delegations
$ArrayServiceEndpoint=$AddServiceEndpoint.Split(',')

$seactual=$ServiceEndPoint.Count

$NewServiceEndpoints=$ArrayServiceEndpoint.Count

$contar= 0


if($NewServiceEndpoints -eq 0){
    Write-Output "No need to add service endpoints."
}

else{
    if($seactual -eq 0){
        Write-Output "There is no service endpoint on the network"
        while ($contar -ilt $NewServiceEndpoints) {
            $ServiceEndPoint.Add($ArrayServiceEndpoint[$contar])
            $contar++
        }

    }else {

            while ($contar -ilt $NewServiceEndpoints) {

                  $seactual=$ServiceEndPoint.Count
                  $secontar=0
                  while($secontar -ilt $seactual) {

                      if($ArrayServiceEndpoint[$contar] -eq $ServiceEndPoint[$secontar]){
                        $implementar= 0
                        $secontar=$seactual

                      }else {
                        $implementar=1
                        $secontar++
                      }
                  }

                  if($implementar -eq '1'){
                    $ServiceEndPoint.Add($ArrayServiceEndpoint[$contar])
                    $contar++
                  }else{ $contar++ }
            }
    }
}

#Add new service endpoint
Get-AzVirtualNetwork -Name $virtualNetworkName -ResourceGroup $resourceGroup -ErrorAction SilentlyContinue | Set-AzVirtualNetworkSubnetConfig -Name $subnetName -AddressPrefix $vnet.AddressPrefix -ServiceEndpoint $ServiceEndPoint -Delegation $delegations | Set-AzVirtualNetwork
