#Parameters
param(
    [Parameter(Mandatory=$true)]
	[string]$PowerBIAppId,
	[Parameter(Mandatory=$true)]
	[string]$PowerBIAppSecret,
	[Parameter(Mandatory=$true)]
	[string]$TenantId,
	[Parameter(Mandatory=$true)]
	[string]$WorkspaceName,
	[string]$AdminEmail,
	[Parameter(Mandatory=$true)]
	[string]$EnableAdmin
	)

#Modules
Install-Module -Name MicrosoftPowerBIMgmt -Force

##Create Connection
$credentials = New-Object System.Management.Automation.PSCredential ($PowerBIAppId, (convertto-securestring $PowerBIAppSecret -asplaintext -force))
Connect-PowerBIServiceAccount -ServicePrincipal -Credential $credentials -Tenant $TenantId

##Check for the Workspace and Create one if not present.
$results = Invoke-PowerBIRestMethod -Url ('groups?$filter=name eq ''' + $WorkspaceName.ToUpper() + '''') -Method GET | ConvertFrom-Json
if ($results.value)
{
	Write-Output "Power BI App Workspace $WorkspaceName already exists..."
	$workspace =$results.value[0]
}
else
{
	$body = '{"name": "' + $WorkspaceName.ToUpper() + '"}'
	$workspace = Invoke-PowerBIRestMethod -Url 'groups' -Method POST -Body $body | ConvertFrom-Json

}
	if(($EnableAdmin -eq "true") -and ($null -ne $workspace.Id))
	{
        $existing=Invoke-PowerBIRestMethod -Url ('groups/' + $workspace.Id + ' /users') -Method GET
        if(!$existing.Contains($AdminEmail))
        {
            ##Assign Permissions to Admin on the workspace.
    		$AccessRights = "Admin" #N.B. Only Admin supported through API

	    	$body = '{"emailAddress": "' + $AdminEmail + '","groupUserAccessRight": "' + $AccessRights + '"}'
		    Invoke-PowerBIRestMethod -Url ('groups/' + $workspace.Id + ' /users') -Method POST -Body $body
        }
		Write-Output "Power BI admin assignment successful."
	}else{
		Write-Output "Power BI admin assignment skipped."
	}

##Set Output Variable.
$key="workspaceID"
$value = $workspace.Id
Write-Output "##vso[task.setvariable variable=$key;issecret=true]$value" #pragma: allowlist secret