<#
    .SYNOPSIS
        Remove the APIM named value pair.
    .DESCRIPTION
        This script calls a Az command to delete the name value pair.
#>

param
(
    [Parameter(Mandatory = $true)]
    [string]
    $resourceGroupName,

    [Parameter(Mandatory = $true)]
    [string]
    $resourceName,

    [Parameter(Mandatory = $true)]
    [string]
    $propertyIds
)

$context = New-AzApiManagementContext -ResourceGroupName $resourceGroupName -ServiceName $resourceName
$propertyIdList = $propertyIds.split(",").Trim()

foreach($propertyId in $propertyIdList){
    Write-Output "Removing the named value $propertyId from APIM"
    Remove-AzApiManagementProperty -Context $context -PropertyId $propertyId
    Write-Output "Removed the named value $propertyId from APIM"
}
