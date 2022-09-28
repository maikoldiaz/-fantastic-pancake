param(
 [Parameter(Mandatory=$true)]
 [string]
 $ModulePath,

 [Parameter(Mandatory=$true)]
 [string]
 $SqlServerConnectionString,

 [Parameter(Mandatory=$false)]
 [string]
 $TicketIds,
 
 [Parameter(Mandatory=$true)]
 [string]
 $Recalculate

)

Import-Module -Name "$ModulePath\PrintSqlQueryOutput"

$schema = "[Admin]";

$attributeTable = "$schema.[AttributeDetailsWithoutOwner]";
$backupMovementTable = "$schema.[BackupMovementDetailsWithoutOwner]";
$inventoryTable = "$schema.[InventoryDetailsWithoutOwner]";
$kpiTable = "$schema.[KPIDataByCategoryElementNode]";
$kpiPreviousTable = "$schema.[KPIPreviousDateDataByCategoryElementNode]";
$movementTable = "$schema.[MovementDetailsWithoutOwner]";
$movementsByProductTable = "$schema.[MovementsByProductWithoutOwner]";
$qualityTable = "$schema.[QualityDetailsWithoutOwner]";
$balanceControlTable = "$schema.[BalanceControl]";

$attributeStoredProc = "$schema.[usp_SaveAttributeDetails]";
$backupMovementStoredProc = "$schema.[usp_SaveBackupMovementDetails]";
$inventoryStoredProc = "$schema.[usp_SaveInventoryDetails]";
$kpiStoredProc = "$schema.[usp_SaveKPIDataByCategoryElementNode]";
$movementStoredProc = "$schema.[usp_SaveMovementDetails]";
$movementsByProductStoredProc = "$schema.[usp_SaveMovementsByProduct]";
$qualityStoredProc = "$schema.[usp_SaveQualityDetails]";
$balanceControlStoredProc = "$schema.[usp_SaveBalanceControl]";

function recalculateForSpecificTicket($TicketId) {
    if($TicketId -eq -1) {
        Write-Output 'Calculating data for all tickets...';
    } else {
        Write-Output 'Calculating data for TicketId = '$TicketId;
    }
    Write-Output 'Calculating Attribute Details... (1/8)'
    $inputs = @{ "@TicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $attributeStoredProc @TicketId" -Inputs $inputs;
    Write-Output 'Calculating Backup Movement Details... (2/8)'
    $inputs = @{ "@TicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $backupMovementStoredProc @TicketId" -Inputs $inputs;
    Write-Output 'Calculating Inventory Details... (3/8)'
    $inputs = @{ "@TicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $inventoryStoredProc @TicketId" -Inputs $inputs;
    Write-Output 'Calculating KPI Data... (4/8)'
    $inputs = @{ "@TicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $kpiStoredProc @TicketId" -Inputs $inputs;
    Write-Output 'Calculating Movement Details... (5/8)'
    $inputs = @{ "@TicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $movementStoredProc @TicketId" -Inputs $inputs;
    Write-Output 'Calculating Movements by Product... (6/8)'
    $inputs = @{ "@TicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $movementsByProductStoredProc @TicketId" -Inputs $inputs;
    Write-Output 'Calculating Quality Details... (7/8)'
    $inputs = @{ "@TicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $qualityStoredProc @TicketId" -Inputs $inputs;
    Write-Output 'Calculating Balance Control... (8/8)'
    $inputs = @{ "@TicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $balanceControlStoredProc @TicketId" -Inputs $inputs;
    Write-Output 'Done.'
}

try {
    if($ResetAll -eq "true") {
        $ticketId = -1;
        Write-Output 'Deleting all existing data...';
        Write-Output 'Deleting Attribute Details... (1/9)'
        PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "TRUNCATE TABLE $attributeTable";
        Write-Output 'Deleting Backup Movement Details... (2/9)'
        PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "TRUNCATE TABLE $backupMovementTable";
        Write-Output 'Deleting Inventory Details... (3/9)'
        PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "TRUNCATE TABLE $inventoryTable";
        Write-Output 'Deleting KPI Data (1)... (4/9)'
        PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "TRUNCATE TABLE $kpiTable";
        Write-Output 'Deleting KPI Data (2)... (5/9)'
        PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "TRUNCATE TABLE $kpiPreviousTable";
        Write-Output 'Deleting Movement Details... (6/9)'
        PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "TRUNCATE TABLE $movementTable";
        Write-Output 'Deleting Movements by Product... (7/9)'
        PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "TRUNCATE TABLE $movementsByProductTable";
        Write-Output 'Deleting Quality Details... (8/9)'
        PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "TRUNCATE TABLE $qualityTable";
        Write-Output 'Deleting Balance Control... (9/9)'
        PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "TRUNCATE TABLE $balanceControlTable";
        Write-Output 'Data deleted.';
        if( $Recalculate -eq "true"){
            recalculateForSpecificTicket -TicketId $ticketId;
        }
    } elseif($TicketIds) {
        $ticketIdsArray = $TicketIds.Split(',')
        foreach ($TicketId in $ticketIdsArray) {
            $TicketId = $TicketId.trim();
            if($TicketId -lt 1) {
                throw 'TicketId should be greater than or equal to 1.'
            } else {
                Write-Output 'Deleting data only for TicketId = ' $TicketId;
                Write-Output 'Deleting Attribute Details... (1/9)'
                $inputs = @{ "@TicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $attributeTable WHERE TicketId=@TicketId" -Inputs $inputs;
                Write-Output 'Deleting Backup Movement Details... (2/9)'
                $inputs = @{ "@TicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $backupMovementTable WHERE TicketId=@TicketId" -Inputs $inputs;
                Write-Output 'Deleting Inventory Details... (3/9)'
                $inputs = @{ "@TicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $inventoryTable WHERE TicketId=@TicketId" -Inputs $inputs;
                Write-Output 'Deleting KPI Data (1)... (4/9)'
                $inputs = @{ "@TicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $kpiTable WHERE TicketId=@TicketId" -Inputs $inputs;
                Write-Output 'Deleting KPI Data (2)... (5/9)'
                $inputs = @{ "@TicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $kpiPreviousTable WHERE TicketId=@TicketId" -Inputs $inputs;
                Write-Output 'Deleting Movement Details... (6/9)'
                $inputs = @{ "@TicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $movementTable WHERE TicketId=@TicketId" -Inputs $inputs;
                Write-Output 'Deleting Movements by Product... (7/9)'
                $inputs = @{ "@TicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $movementsByProductTable WHERE TicketId=@TicketId" -Inputs $inputs;
                Write-Output 'Deleting Quality Details... (8/9)'
                $inputs = @{ "@TicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $qualityTable WHERE TicketId=@TicketId" -Inputs $inputs;
                Write-Output 'Deleting Balance Control... (9/9)'
                $inputs = @{ "@TicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $balanceControlTable WHERE TicketId=@TicketId" -Inputs $inputs;
                Write-Output 'Data deleted for TicketId = ' $TicketId;
                if( $Recalculate -eq "true"){
                    recalculateForSpecificTicket -TicketId $TicketId;
                }
            }
        }
    } else {
        throw 'No ticketids provided.'
    }
}
catch {
    Write-Output $_.Exception.Message
    throw;
}