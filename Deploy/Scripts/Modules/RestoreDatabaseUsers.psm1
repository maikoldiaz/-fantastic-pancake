<#
 .Synopsis
  Restores specified list of users with their roles.

  .Parameter Users
  The users of a database, can be obtained using GetDatabaseUsers function module

  .Parameter Roles
  The roles of tje database users, can be obtained using GetDatabaseUserRole function module

 .Parameter ConnectionString
  The sql connection string

 .Example
   Restore-DatabaseUsers -Users $users -Roles $roles -ConnectionString $sqlconnectionstring
#>
function Restore-DatabaseUsers($Users, $Roles, $ConnectionString) {
    if ($null -ne $Users) {
        Write-Output "NOTE: LOGIN and USERNAME are expected to be same.";
        foreach ($row in $Users) {
            $createuserquery = "CREATE USER [" + $row.username + "] FROM  LOGIN [" + $row.username + "];";
            Invoke-Sqlcmd -ConnectionString $ConnectionString -Query $createuserquery -AbortOnError
            Write-Output "User (" $row.username ") created.";
            if ($null -ne $Roles) {
                foreach ($rolerow in $Roles) {
                    if ($rolerow.username -eq $row.username) {
                        $alterrolequery = "ALTER ROLE " + $rolerow.role + " ADD MEMBER [" + $rolerow.username + "];";
                        Invoke-Sqlcmd -ConnectionString $ConnectionString -Query $alterrolequery -AbortOnError
                        Write-Output "Role (" $rolerow.role ") set for " $row.username;
                    }
                }
            }
        }
    }
}

Export-ModuleMember -Function Restore-DatabaseUsers