<#
 .Synopsis
  Get users of database

 .Parameter ConnectionString
  The sql connection string

 .Example
   Get-DatabaseUsers -ConnectionString $sqlconnectionstring
#>

function Get-DatabaseUsers($ConnectionString) {
    $users = Invoke-Sqlcmd -ConnectionString $ConnectionString -Query "select name as username
    from sys.database_principals
    where type not in ('A', 'G', 'R', 'X')
          and sid is not null
          and name NOT IN ('guest', 'dbo','truedbuser', CURRENT_USER)
    order by username;";
    return $users;
}

Export-ModuleMember -Function Get-DatabaseUsers