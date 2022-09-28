param(
	[Parameter(Mandatory=$true)]
    [string]$AccessLevel,
	[Parameter(Mandatory=$true)]
    [string]$AppId,
	[Parameter(Mandatory=$true)]
    [string]$Scope
 )
# Remove-AzRoleAssignment -ServicePrincipalName $AppId -Scope $Scope -RoleDefinitionName $AccessLevel -ErrorAction SilentlyContinue
# Start-Sleep -s 50
 
 $existing = Get-AzRoleAssignment -ServicePrincipalName $AppId -RoleDefinitionName $AccessLevel -Scope $Scope -ErrorAction SilentlyContinue

 if(!$existing) {
    New-AzRoleAssignment -RoleDefinitionName $AccessLevel -ServicePrincipalName $AppId -scope $Scope -ErrorAction SilentlyContinue
	Start-Sleep -s 50
    $existing = Get-AzRoleAssignment -ServicePrincipalName $AppId -ErrorAction SilentlyContinue
 }

if(!$existing){
	Write-Error "Role Assignment Failed for access level: $AccessLevel on Scope - $Scope for ServicePrincipal - $AppId"
}