param(
    [string]$tier,
    [Parameter(Mandatory=$true)]
    [string]$resourceGroupName,
    [Parameter(Mandatory=$true)]
    [string]$resourceName,
    [string]$apiVersion
)

if ($tier)
{
    $tag = @{description=$env:description; createdBy=$env:createdBy; tier=$tier; responsible=$env:responsible; projectName=$env:projectName; companyName=$env:companyName; environment=$env:env; organizationUnit=$env:organizationUnit; creationDate=$env:creationDate; dataProfile=$env:dataProfile;}
}
else{
    $tag = @{description=$env:description; createdBy=$env:createdBy; responsible=$env:responsible; projectName=$env:projectName; companyName=$env:companyName; environment=$env:env; organizationUnit=$env:organizationUnit; creationDate=$env:creationDate; dataProfile=$env:dataProfile;}
}


$resource = Get-AzResource -ResourceGroupName $resourceGroupName -Name $resourceName

if($apiVersion)
{
    Set-AzResource -ResourceId $resource.Id -Tag $tag -ApiVersion $apiVersion -Force
} else {
    Set-AzResource -ResourceId $resource.Id -Tag $tag -Force
}