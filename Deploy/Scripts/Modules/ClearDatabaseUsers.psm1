<#
 .Synopsis
  Drops all the provided list of users except truedbuser

 .Parameter Users
  The users that can be obtained using GetDatabaseUsers function module

 .Parameter ConnectionString
  The sql connection string

 .Example
   Clear-DatabaseUsers -ConnectionString $connectionString -Users $users
#>
function Clear-DatabaseUsers($ConnectionString, $Users) {
    if ($null -eq $Users) {
        Write-Output "Users not found to handle recreation.";
    }
    else {
        foreach ($row in $Users) {
            if ($row.username -ne "truedbuser") {
                Write-Output "User (" $row.username ") found, hence dropping.";
                $dropuserquery = "DROP USER [" + $row.username + "];";
                Invoke-Sqlcmd -ConnectionString $ConnectionString -Query $dropuserquery -AbortOnError
                Write-Output "User (" $row.username ") dropped."
            }
        }
    }
}

Export-ModuleMember -Function Clear-DatabaseUsers