<#
 .Synopsis
  Drops the database

 .Parameter DatabaseName
  The name of the database to be dropped

 .Parameter ConnectionString
  The sql connection string

 .Example
   Remove-Database $DatabaseName $ConnectionString
#>
# Drop Database
function Remove-Database($DatabaseName, $ConnectionString) {
    $query = "Use [master]
              DROP DATABASE $DatabaseName";
    Invoke-Sqlcmd -ConnectionString $ConnectionString -Query $query;
}

Export-ModuleMember -Function Remove-Database