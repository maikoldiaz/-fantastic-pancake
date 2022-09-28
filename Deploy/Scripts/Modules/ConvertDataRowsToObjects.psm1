<#
.Synopsis
   Converts datarows returned by Invoke-SqlCmd to Objects[]

.EXAMPLE
   Convert-DataRowsToObjects $queryResults
#>

Function Convert-DataRowsToObjects
{
    [CmdletBinding()]
    [OutputType([object])]
    Param (
        [Parameter(
            Mandatory = $true,
            ValueFromPipeline = $true
        )]
        [PSObject[]]$DataTable
    )
    $Objects = New-Object System.Collections.Generic.List[System.Object];
    ForEach ($Row in $DataTable)
    {
        $Properties = [ordered]@{}
        For($i = 0;$i -le $Row.ItemArray.Count - 1;$i++)
        {
            $Properties.Add($Row.Table.Columns[$i].ColumnName, $Row.ItemArray[$i])
        }
        $object = New-Object -TypeName PSObject -Property $Properties;
        $Objects.Add($object);
    }
    return ,$Objects.ToArray();
}
Export-ModuleMember -Function Convert-DataRowsToObjects