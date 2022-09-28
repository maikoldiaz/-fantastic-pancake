<#
    .NOTES
    The script updates the paramaters for api and zip the file contents.
    The NodeApprovalFlowTemplate.json template file is edited with Api Uri, Client Id and Client Secret and NodeApprovalFlow.json is created and zipped.
    Power Automate solution is created in destination environment using the zip file.

    Zip File Contents:
    1. [Content_Types].xml
    2. customizations.xml - (contains the path of json file NodeApprovalFlow.json)
    3. NodeApprovalFlowTemplate.json
    4. solution.xml

    .PREREQUISITES
    1. A Power Automate environment must be created with Common Data Service.
    2. Azure AD app must be created with Dynamics CRM permission for getting the access token.
#>

param
(
    [Parameter(Mandatory = $false)]
    [String]$tenantId,

    [Parameter(Mandatory = $false)]
    [String]$clientId,

    [Parameter(Mandatory = $false)]
    [String]$clientSecret,

    [Parameter(Mandatory = $false)]
    [String]$zipFilePath,

    [Parameter(Mandatory = $false)]
    [String]$env,

    [Parameter(Mandatory = $false)]
    [String]$vaultName,

    [Parameter(Mandatory = $false)]
    [String]$solSecretName,

    [Parameter(Mandatory = $false)]
    [String]$flowResourceUrl
)

function Get-SHAHash($dataForEncryption) {

    $hasher = [System.Security.Cryptography.HashAlgorithm]::Create('sha256')
    $hash = $hasher.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($dataForEncryption))

    $hashString = [System.BitConverter]::ToString($hash)
    $signature = $hashString.Replace('-', '')
    return $signature
}

function Compare-Signature($expectedSignature, $filePath) {

    $Content = Get-Content $filePath

    $hashValue = Get-SHAHash -dataForEncryption $Content

    $status = ($hashValue -eq $expectedSignature)

    return $status
}

function Push-KeyVaultHash($secretName, $hash, $solutionFile, $vaultName) {

    if ($solutionFile) {
        Set-AzKeyVaultSecret -VaultName $vaultName -Name $secretName -SecretValue (ConvertTo-SecureString -String $hash -AsPlainText -Force)
    }
    else {

        $files = Get-ChildItem $zipFileEnvPath -Filter '*.json' -File

        foreach ($file in $files) {
            $fileName = $file.Name.TrimEnd(".json")
            #flow json
            $hash = Get-Hash -filePath "$zipFileEnvPath\$fileName.json"
            Set-AzKeyVaultSecret -VaultName $vaultName -Name $fileName -SecretValue (ConvertTo-SecureString -String $hash -AsPlainText -Force)
        }
    }
}

function Get-Hash($filePath) {
    $Content = Get-Content $filePath

    $hashdata = Get-SHAHash -dataForEncryption $Content

    return $hashdata
}

function Get-AccessToken($tenantId, $clientId, $clientSecret) {

    Write-Output "Login to AzureAD with same application as endpoint"
    $adTokenUrl = "https://login.microsoftonline.com/$($tenantId)/oauth2/token"

    $body = @{
        grant_type    = "client_credentials"
        client_id     = $clientId
        client_secret = $clientSecret
        resource      = $script:resourceUrl
    }

    Write-Output "Generating Authentication token..."
    $response = Invoke-RestMethod -Method 'Post' -Uri $adTokenUrl -ContentType "application/x-www-form-urlencoded" -Body $body

    $script:accessToken = $response.access_token
}

function Import-PowerAutomateSolution() {
    $headers = New-Object 'System.Collections.Generic.Dictionary[String,String]'
    $headers.Add("Accept", "application/json")
    $headers.Add("Content-Type", "application/json")
    $headers.Add("Authorization", "Bearer " + $script:accessToken)

    $newGuid = [guid]::NewGuid()
    $guidString = $newGuid.Guid.ToString()
    $base64string = [Convert]::ToBase64String([IO.File]::ReadAllBytes("$($zipFileEnvPath)NodeApproval.zip"))
    $requrl = $script:resourceUrl + "/api/data/v9.1/ImportSolution"

    $body = @"
    {
        "OverwriteUnmanagedCustomizations": "true",
        "PublishWorkflows": "true",
        "ImportJobId": "$($guidString)",
        "CustomizationFile": "$($base64string)"
    }
"@

    Invoke-RestMethod -Uri $requrl -Headers $headers -Method POST -ContentType "application/json" -Body $body

    $requrl = $script:resourceUrl + "/api/data/v9.1/importjobs($($newGuid))"
    Invoke-RestMethod -Uri $requrl -Method GET -Headers $headers
    Write-Output 'Import complete.'
}

function Get-Json($prefixPath) {

    $files = Get-ChildItem $zipFileEnvPath -Filter '*.json' -File

    foreach ($file in $files) {
        $filePath = $prefixPath + "/" + $file.Name

        Get-Content $filePath -Raw | ConvertFrom-Json
    }
}

function Get-SolutionSignature($vaultName) {

    #solution xml
    $existingSignature = Get-AzKeyVaultSecret -VaultName $vaultName -Name $solSecretName -ErrorAction Continue

    $ssPtr = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($existingSignature.SecretValue)

    try {
        $secretValueText = [System.Runtime.InteropServices.Marshal]::PtrToStringBSTR($ssPtr)
    } finally {
        [System.Runtime.InteropServices.Marshal]::ZeroFreeBSTR($ssPtr)
    }

    if ($existingSignature) {
        $signatureStatus = Compare-Signature -expectedSignature $secretValueText -filePath "$zipFileEnvPath\solution.xml"
    }
    else {
        $signatureStatus = $false
    }

    if ($signatureStatus -eq $true) {
        return $false
    }
    else {
        return $true
    }
}

function Get-FlowSignature($vaultName) {

    $files = Get-ChildItem $zipFileEnvPath -Filter '*.json' -File

    foreach ($file in $files) {
        $fileName = $file.Name.TrimEnd(".json")
        #flow json
        $existingSignature = Get-AzKeyVaultSecret -VaultName $vaultName -Name $fileName -ErrorAction Continue

        $ssPtr = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($existingSignature.SecretValue)

        try {
            $secretValueText = [System.Runtime.InteropServices.Marshal]::PtrToStringBSTR($ssPtr)
        } finally {
           [System.Runtime.InteropServices.Marshal]::ZeroFreeBSTR($ssPtr)
        }

        if ($existingSignature) {
            $signatureStatus = Compare-Signature -expectedSignature $secretValueText -filePath "$zipFileEnvPath\$fileName.json"
        }
        else {
            $signatureStatus = $false
        }

        if ($signatureStatus -eq $false) {
            return $true
        }
    }
    return $false
}

try {
    $zipFileEnvPath = $zipFilePath + $env + "/"

    $folderContent = Get-ChildItem -Path $zipFileEnvPath

    if (!$folderContent) {
        throw "FolderEmpty"
    }

    Get-Json -prefixPath $zipFileEnvPath

    $script:accessToken = ''
    $script:resourceUrl = ''

    $shouldDeploySolution = Get-SolutionSignature -vaultName $vaultName
    $shouldDeployFlow = Get-FlowSignature -vaultName $vaultName

    $shouldDeploy = $shouldDeploySolution -or $shouldDeployFlow

    if ($shouldDeploy) {
        $script:resourceUrl = $flowResourceUrl
        Get-AccessToken -tenantId $tenantId -clientId $clientId -clientSecret $clientSecret

        $env:solutionHash = Get-Hash -filePath "$zipFileEnvPath\solution.xml"

        Compress-Archive -Path "$($zipFileEnvPath)*" -DestinationPath "$($zipFileEnvPath)NodeApproval.zip" -CompressionLevel Fastest -Update
        Import-PowerAutomateSolution

        Push-KeyVaultHash -solutionFile $false -vaultName $vaultName
        Push-KeyVaultHash -secretName "$solSecretName" -hash $env:solutionHash -solutionFile $true -vaultName $vaultName

    }
    else {
        Write-Output "Flow is already Up to date."
    }
}
catch {
    Write-Output "Error: $_"

    if ($_.Exception.ToString().contains("FolderEmpty")) {
        exit
    }

}
