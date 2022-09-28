##Parameters
param
(
    [Parameter(Mandatory = $true)]
    [string]$FilePath
)

#Modules
Write-Output 'Installing powershell module'
Install-Module -Name PSScriptAnalyzer -Force

$ScriptAnalyzerRules = Get-ScriptAnalyzerRule -Severity Error,Warning
$ScriptAnalyzerResult = Invoke-ScriptAnalyzer -Path $FilePath -IncludeRule $ScriptAnalyzerRules -ExcludeRule @('PSAvoidUsingPlainTextForPassword', 'PSAvoidUsingConvertToSecureStringWithPlainText', 'PSAvoidUsingUsernameAndPasswordParams')
If ( $ScriptAnalyzerResult ) {
    $ScriptAnalyzerResultString = $ScriptAnalyzerResult | Out-String
    Write-Error $ScriptAnalyzerResultString
}