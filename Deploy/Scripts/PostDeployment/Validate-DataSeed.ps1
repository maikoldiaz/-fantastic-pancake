param(
 [Parameter(Mandatory=$true)]
 [string]
 $connectionString
 )

 Write-Output "operativeMovementsCount: $env:operativeMovementsCount"
 Write-Output "ownershipPercentageValuesCount: $env:ownershipPercentageValuesCount"

 function Get-DataSeedCount([string]$connectionString, [string]$query, [int]$count) {
    $result = Invoke-Sqlcmd -ConnectionString $connectionString -Query $query -Verbose;
    Write-Output "Executing query $query"
    Write-Output "Count : $($result.Item(0))"
    if ($result.Item(0) -ne $count)
    {
        Write-Error "$query returned count as $($result.Item(0)) while the expected count was $count."
    }
}

 $query = "select count(*) from [Analytics].[OperativeMovements] where [SourceSystem] = 'CSV' and [OperationalDate] >= '$($env:gapStartDate)';"
 Get-DataSeedCount -connectionString $connectionString -query $query -count $env:operativeMovementsCount

 $query = "select count(*) from [Analytics].[OwnershipPercentageValues] where [SourceSystem] = 'CSV' and [OperationalDate] >= '$($env:gapStartDate)';"
 Get-DataSeedCount -connectionString $connectionString -query $query -count $env:ownershipPercentageValuesCount

