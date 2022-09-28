param
(
    [Parameter(Mandatory = $true)]
    [string]$ModulePath,

    [Parameter(Mandatory = $true)]
    [String]$SqlServerConnectionString,

    [Parameter(Mandatory = $true)]
    [String]$DatabaseName,

    [Parameter(Mandatory = $true)]
    [String]$CloneDatabaseName
)

Import-Module -Name "$ModulePath\GetValue"
Import-Module -Name "$ModulePath\EditData"
$customConnectionString = Edit-Data -InputString $SqlServerConnectionString -Source $DatabaseName -Destination $CloneDatabaseName;
Write-Output "##vso[task.setvariable variable=testbedconnectionstring;]$customConnectionString"