param(
    [Parameter(Mandatory=$true)]
    [string]$aasADAppClientId,
    [Parameter(Mandatory=$true)]
    [string]$aasADAppClientSecret,
    [Parameter(Mandatory=$true)]
    [string]$TenantId,
    [Parameter(Mandatory=$true)]
    [string]$analysisServicesName,
    [Parameter(Mandatory=$true)]
    [string]$modelName,
    [Parameter(Mandatory=$true)]
    [string]$rolloutenvironment
	)

#******************************************************************************
# Script body
# Execution begins here
#******************************************************************************

function deleteModel($modelName, $serverName) {
    Write-Output "Deleting $modelName";
    $output = Invoke-ASCmd -Query "{`"delete`": {`"object`": { `"database`": `"$modelName`" } } }" -Server $serverName
    # Print and throw error depending on output
    if($output.Contains("Warning"))
    {
        Write-Output "Model delete completed with warning..." $output
    }
    elseif($output.Contains("exception"))
    {
        Write-Error -Message ("Model delete has failed...." + $output)
        throw $output
    }
    else
    {
        Write-Output "Model delete is complete....." $output
    }
}

try
{

    # Using AAD credentials
    $password = ConvertTo-SecureString -String $aasADAppClientSecret -AsPlainText -Force #pragma: allowlist secret
    $credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $aasADAppClientId, $password

    # Login to Analysis Service using Credentials defined previously.
    Add-AzureAnalysisServicesAccount -Credential $Credential -ServicePrincipal -TenantId $TenantId -RolloutEnvironment $rolloutenvironment
    Write-Output "Adding AzureAnalysisServicesAccount completed."

    deleteModel -modelName $modelName -serverName "asazure://$rolloutenvironment/$analysisServicesName"

}
catch{
    Write-Error -Message "Error Message: " -Exception $_.Exception
	throw $_.Exception
}



