<#
 .Synopsis
  Runs the query, prints the query output and sets flag if records found in global var recordsFound

 .Parameter ConnectionString
  The sql connection string

 .Parameter Query
  The sql query

 .Parameter Inputs
  The inputs hashtable (optional)

 .Example
   PrintQueryOutput -ConnectionString $sqlConnectionString -Query $sqlQuery -Inputs $Inputs
#>

function PrintSqlQueryOutput ($ConnectionString, $Query, $Inputs) {
    $Connection = New-Object System.Data.SQLClient.SQLConnection
    $Connection.ConnectionString = $ConnectionString;
    $Connection.Open()
    $Command = New-Object System.Data.SQLClient.SQLCommand
    if($Inputs) {
        foreach($key in $Inputs.keys) {
            $_ = $Command.Parameters.Add("$key", $Inputs[$key]);
        }
    }
    $Command.CommandTimeout = 0;
    $Command.Connection = $Connection
    $Command.CommandText = $Query
    $Reader = $Command.ExecuteReader()
    $columnNames = "|";
    for($i=0;$i -lt $Reader.FieldCount;$i++)
    {
        $columnNames = $columnNames + $Reader.GetName($i) + "|";
    }
    if($Reader.HasRows) {
        $global:recordsFound = $true;
        Write-Output $columnNames;
    }
    while ($Reader.Read()) {
        $values = "|";
        for ($i = 0; $i -lt $Reader.FieldCount-5; $i++) {
            $values = $values + $Reader.GetValue($i)+ "|";
        }
        Write-Output $values;
    }
    $Connection.Close();
}

Export-ModuleMember -Function PrintSqlQueryOutput