param(
 [Parameter(Mandatory=$true)]
 [string]
 $ModulePath,

 [Parameter(Mandatory=$true)]
 [string]
 $SqlServerConnectionString,

 [Parameter(Mandatory=$false)]
 [string]
 $TicketIds
)

Import-Module -Name "$ModulePath\PrintSqlQueryOutput"

$schema = "[Admin]";

$attributeTable = "$schema.[AttributeDetailsWithOwner]";
$backupMovementTable = "$schema.[BackupMovementDetailsWithOwner]";
$inventoryTable = "$schema.[InventoryDetailsWithOwner]";
$kpiTable = "$schema.[KPIDataByCategoryElementNodeWithOwnership]";
$kpiPreviousTable = "$schema.[KPIPreviousDateDataByCategoryElementNodeWithOwner]";
$movementTable = "$schema.[MovementDetailsWithOwner]";
$movementsByProductTable = "$schema.[MovementsByProductWithOwner]";
$qualityTable = "$schema.[QualityDetailsWithOwner]";
$movementDetailsWithOwnerOtherSegmentTable = "$schema.[movementDetailsWithOwnerOtherSegment]";

$attributeStoredProc = "$schema.[usp_SaveAttributeDetailsWithOwner]";
$backupMovementStoredProc = "$schema.[usp_SaveBackupMovementDetailsWithOwner]";
$inventoryStoredProc = "$schema.[usp_SaveInventoryDetailsWithOwner]";
$kpiStoredProc = "$schema.[usp_SaveKPIDataByCategoryElementNodeWithOwnership]";
$movementStoredProc = "$schema.[usp_SaveMovementDetailsWithOwner]";
$movementsByProductStoredProc = "$schema.[usp_SaveMovementsByProductWithOwner]";
$qualityStoredProc = "$schema.[usp_SaveQualityDetailsWithOwner]";
$movementDetailsWithOwnerOtherSegmentStoredProc = "$schema.[usp_SaveMovementDetailsWithOwnerOtherSegment]";

function recalculateForSpecificTicket($TicketId) {
    Write-Output 'Calculating Attribute Details... (1/8)'
    $inputs = @{ "@OwnershipTicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $attributeStoredProc @OwnershipTicketId" -Inputs $inputs;
    Write-Output 'Calculating Backup Movement Details... (2/8)'
    $inputs = @{ "@OwnershipTicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $backupMovementStoredProc @OwnershipTicketId" -Inputs $inputs;
    Write-Output 'Calculating Inventory Details... (3/8)'
    $inputs = @{ "@OwnershipTicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $inventoryStoredProc @OwnershipTicketId" -Inputs $inputs;
    Write-Output 'Calculating KPI Data... (4/8)'
    $inputs = @{ "@OwnershipTicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $kpiStoredProc @OwnershipTicketId" -Inputs $inputs;
    Write-Output 'Calculating Movement Details... (5/8)'
    $inputs = @{ "@OwnershipTicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $movementStoredProc @OwnershipTicketId" -Inputs $inputs;
    Write-Output 'Calculating Movements by Product... (6/8)'
    $inputs = @{ "@OwnershipTicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $movementsByProductStoredProc @OwnershipTicketId" -Inputs $inputs;
    Write-Output 'Calculating Quality Details... (7/8)'
    $inputs = @{ "@OwnershipTicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $qualityStoredProc @OwnershipTicketId" -Inputs $inputs;
    Write-Output 'Calculating Movement Details With Owner Other Segment... (8/8)'
    $inputs = @{ "@OwnershipTicketId" = $TicketId}
    PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "EXEC $movementDetailsWithOwnerOtherSegmentStoredProc @OwnershipTicketId" -Inputs $inputs;
    Write-Output 'Done.'
}

try {
    if($TicketIds) {
        $ticketIdsArray = $TicketIds.Split(',')
        foreach ($TicketId in $ticketIdsArray) {
            $TicketId = $TicketId.trim();
            if($TicketId -lt 1) {
                throw 'TicketId should be greater than or equal to 1.'
            } else {      
                Write-Output 'Deleting data only for OwnershipTicketId = ' $TicketId;
                Write-Output 'Deleting Attribute Details... (1/9)'
                $inputs = @{ "@OwnershipTicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $attributeTable WHERE OwnershipTicketId=@OwnershipTicketId" -Inputs $inputs;
                Write-Output 'Deleting Backup Movement Details... (2/9)'
                $inputs = @{ "@OwnershipTicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $backupMovementTable WHERE OwnershipTicketId=@OwnershipTicketId" -Inputs $inputs;
                Write-Output 'Deleting Inventory Details... (3/9)'
                $inputs = @{ "@OwnershipTicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $inventoryTable WHERE OwnershipTicketId=@OwnershipTicketId" -Inputs $inputs;
                Write-Output 'Deleting KPI Data (1)... (4/9)'
                $inputs = @{ "@OwnershipTicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $kpiTable WHERE OwnershipTicketId=@OwnershipTicketId" -Inputs $inputs;
                Write-Output 'Deleting KPI Data (2)... (5/9)'
                $inputs = @{ "@OwnershipTicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $kpiPreviousTable WHERE OwnershipTicketId=@OwnershipTicketId" -Inputs $inputs;
                Write-Output 'Deleting Movement Details... (6/9)'
                $inputs = @{ "@OwnershipTicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $movementTable WHERE OwnershipTicketId=@OwnershipTicketId" -Inputs $inputs;
                Write-Output 'Deleting Movements by Product... (7/9)'
                $inputs = @{ "@OwnershipTicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $movementsByProductTable WHERE OwnershipTicketId=@OwnershipTicketId" -Inputs $inputs;
                Write-Output 'Deleting Quality Details... (8/9)'
                $inputs = @{ "@OwnershipTicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $qualityTable WHERE OwnershipTicketId=@OwnershipTicketId" -Inputs $inputs;
                Write-Output 'Deleting Movement Details With Owner Other Segment... (9/9)'
                $inputs = @{ "@OwnershipTicketId" = $TicketId}
                PrintSqlQueryOutput -ConnectionString $SqlServerConnectionString -Query "DELETE FROM $movementDetailsWithOwnerOtherSegmentTable WHERE OwnershipTicketId=@OwnershipTicketId" -Inputs $inputs;
                Write-Output 'Data deleted for OwnershipTicketId = ' $TicketId;
                recalculateForSpecificTicket -TicketId $TicketId;
            }
        }
    } else {
        throw 'No ticketids provided.';
    }
}
catch {
    Write-Output $_.Exception.Message
    throw;
}