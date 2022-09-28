param (
	[Parameter(Mandatory=$true)]
    [string]$basePath,
	[Parameter(Mandatory=$true)]
    [string]$templateRootPath,
	[Parameter(Mandatory=$true)]
    [string]$scriptRootPath
)
Install-Module -Name Pester -PassThru -Force -SkipPublisherCheck
Update-Module Pester -Force
Import-Module Pester -MinimumVersion 5.0.4
Set-Location $basePath
$configuration = [PesterConfiguration]@{}
$configuration.Run.Path = $basePath
$configuration.Run.PassThru = $true
$configuration.Should.ErrorAction = 'Stop'
$configuration.CodeCoverage.Enabled = $true
$configuration.TestResult.Enabled = $true
$configuration.TestResult.OutputFormat = 'NUnit2.5'
$configuration.TestResult.OutputPath = "$($basePath)\testResult.xml"
Write-Output "Invoking pester tests..."
Invoke-Pester -Configuration $configuration