<#
 .Synopsis
  Get roles of database users

 .Parameter ConnectionString
  The sql connection string

 .Example
   Get-DatabaseUserRole -ConnectionString $sqlconnectionstring
#>

function Get-DatabaseUserRole([string]$ConnectionString) {
    $rolesquery = "SELECT DP1.name AS [role],
    DP2.name as username
    FROM sys.database_role_members AS DRM
    RIGHT OUTER JOIN sys.database_principals AS DP1
    ON DRM.role_principal_id = DP1.principal_id
    RIGHT OUTER JOIN sys.database_principals AS DP2
    ON DRM.member_principal_id = DP2.principal_id
    WHERE DP1.type = 'R' AND DP2.name <> 'dbo'";
    $roles = Invoke-Sqlcmd -ConnectionString $ConnectionString -Query $rolesquery -AbortOnError
    return $roles;
}

Export-ModuleMember -Function  Get-DatabaseUserRole