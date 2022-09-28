<#
 .Synopsis
  Runs the query, prints the query output and sets flag if records found in global var recordsFound

 .Parameter ConnectionString
  The sql connection string

 .Parameter Procedure
  The sql stored procedure

 .Parameter Inputs
  The inputs hashtable (optional)

 .Example
   PrintQueryOutput -ConnectionString $sqlConnectionString -Procedure $procedure -Inputs $inputs
#>
function PrintSqlProcedureOutput($ConnectionString, $procedure, $inputs) {
    $Connection = New-Object System.Data.SQLClient.SQLConnection
    $Connection.ConnectionString = $ConnectionString;
    $Connection.Open()
    $Command = New-Object System.Data.SqlClient.SQLCommand
    if ($Inputs) {
        foreach ($key in $inputs.Keys) {
            $Command.Parameters.Add("$key", $inputs["$key"].Type)
            $Command.Parameters["$key"].Value = $inputs["$key"].Value
        }
    }
    $Command.CommandText = $procedure
    $Command.CommandTimeout = 0;
    $Command.Connection = $Connection
    $Reader = $Command.ExecuteReader()
    do {
        $Reader.Read();
        $values = "|";
        for ($i = 0; $i -lt $Reader.FieldCount; $i++) {
            $values = $values + $Reader.GetName($i) + " = " +  $Reader.GetValue($i) + "|";
        }
        Write-Output $values;
    }
    while ($Reader.Read())
    $Connection.Close();
}

Export-ModuleMember -Function PrintSqlProcedureOutput