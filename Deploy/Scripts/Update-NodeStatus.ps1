param
(
    [Parameter(Mandatory = $true)]
    [String]$SqlServerConnectionString,

    [Parameter(Mandatory = $true)]
    [String]$ticketId,

    [Parameter(Mandatory = $true)]
    [String]$statusId,
    
    [Parameter(Mandatory = $true)]
    [String]$nodeName
)

# Query to update the node status by id
function Set-Node-Status($nodeId)
{
    $query = "UPDATE Admin.OwnershipNode SET OwnershipStatusId =  " + $statusId + " WHERE TicketId = " + $ticketId + " AND NodeId = " + $nodeId;
    
    Invoke-SqlCmd -Query $query -ConnectionString $SqlServerConnectionString -StatisticsVariable stats -AbortOnError

    if($stats.IduRows -eq 0)
    {
        throw 'OwnershipNode not found'
    }
}

function Get-NodeId
{
    $nodeQuery = "SELECT TOP 1 NodeId FROM Admin.Node WHERE Name = '$nodeName'";

    $nodeDS = Invoke-SqlCmd -Query $nodeQuery -ConnectionString $SqlServerConnectionString -StatisticsVariable stats -AbortOnError -As DataSet

    if($nodeDS.Tables.Count -eq 0)
    {
        throw 'Node not found'
    }

    $nodeId = $nodeDS.Tables[0].Rows[0].NodeId

    return $nodeId;
}

function Install-Sql-Module()
{
    if (Get-Module -ListAvailable -Name SqlServer) {
        Write-Host "SQL Already Installed"
    } 
    else {
        Install-Module -Name SqlServer -Force
    }
}

Install-Sql-Module;
$nodeId = Get-NodeId;
Set-Node-Status($nodeId);