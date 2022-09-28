param
(
    [Parameter(Mandatory=$true)]
    [string]
    $ModulePath,

    [Parameter(Mandatory = $true)]
    [String]$SqlServerConnectionString,

    [Parameter(Mandatory = $true)]
    [String]$SegmentName,

    [Parameter(Mandatory = $true)]
    [String]$StartDate,
    
    [Parameter(Mandatory = $true)]
    [String]$EndDate
)

Import-Module -Name "$ModulePath\PrintSqlProcedureOutput"
$schema = "[Admin]";

$deleteConsolidatedMovementsStoredProc = "$schema.[usp_DeleteConsolidatedInformation]";

# Query to update the node status by id
function DeleteConsolidatedInformation {
    Write-Output "Deleting data only for SegmentName: $SegmentName";
    $inputs = @{
        "@SegmentName" = @{"Value" = $SegmentName; "Type" = [System.Data.SqlDbType]::NVarChar };
        "@StartDate" = @{"Value" = $StartDate; "Type" = [System.Data.SqlDbType]::Date };
        "@EndDate" = @{"Value" = $EndDate; "Type" = [System.Data.SqlDbType]::Date }
    }
    
    $query = "EXEC $deleteConsolidatedMovementsStoredProc @SegmentName, @StartDate, @EndDate";
    PrintSqlProcedureOutput -ConnectionString $SqlServerConnectionString -Inputs $inputs -Procedure $query;
}

try {
    DeleteConsolidatedInformation;
}
catch {
    Write-Output $_.Exception.Message
    throw;
}