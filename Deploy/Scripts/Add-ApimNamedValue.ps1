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
    $propertyId,

    [Parameter(Mandatory = $true)]
    [string]
    $propertyName,

    [Parameter(Mandatory = $true)]
    [string]
    $propertyValue
)

Set-Item Env:\SuppressAzurePowerShellBreakingChangeWarnings "true"

$context = New-AzApiManagementContext -ResourceGroupName $resourceGroupName -ServiceName $resourceName
Write-Output "Setting the named value to APIM"
New-AzApiManagementNamedValue -Context $context -NamedValueId $propertyId -Name $propertyName -Value $propertyValue
