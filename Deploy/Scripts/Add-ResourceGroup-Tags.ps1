param(
    [string]$tier,
    [Parameter(Mandatory=$true)]
    [string]$resourceGroupName
)

if ($tier)
{
    $tag = @{description=$env:description; createdBy=$env:createdBy; tier=$tier; responsible=$env:responsible; projectName=$env:projectName; companyName=$env:companyName; environment=$env:env; organizationUnit=$env:organizationUnit; creationDate=$env:creationDate; dataProfile=$env:dataProfile;}
}
else{
    $tag = @{description=$env:description; createdBy=$env:createdBy; responsible=$env:responsible; projectName=$env:projectName; companyName=$env:companyName; environment=$env:env; organizationUnit=$env:organizationUnit; creationDate=$env:creationDate; dataProfile=$env:dataProfile;}
}

$resource = Get-AzResourceGroup -Name $resourceGroupName

Set-AzResource -ResourceId $resource.ResourceId -Tag $tag -Force