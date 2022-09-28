#Parameters
param(
    [Parameter(Mandatory=$true)]
	[string]$PowerBIAppID,
	[Parameter(Mandatory=$true)]
	[string]$PowerBIAppSecret,
	[Parameter(Mandatory=$true)]
	[string]$TenantId,
	[Parameter(Mandatory=$true)]
	[string]$WorkspaceName,
	[Parameter(Mandatory=$true)]
	[string]$WorkspaceId
)


#Modules
Install-Module -Name MicrosoftPowerBIMgmt -Force

##Create Connection
$credentials = New-Object System.Management.Automation.PSCredential ($PowerBIAppID, (convertto-securestring $PowerBIAppSecret -asplaintext -force))
Connect-PowerBIServiceAccount -ServicePrincipal -Credential $credentials -Tenant $TenantId

##Check for the Workspace and Create one if not present.
$results = Invoke-PowerBIRestMethod -Url ('groups?$filter=name eq ''' + $WorkspaceName.ToUpper() + '''') -Method GET | ConvertFrom-Json
if ($results.value)
{
    Write-Output "Deleting workspace"
	$results = Invoke-PowerBIRestMethod -Url ('groups/' + $WorkspaceId) -Method DELETE | ConvertFrom-Json
	Write-Output $results
	Write-Output "Workspace Deleted"
}
else {
    Write-Output "PowerBi Workspace doesn't exist"
}