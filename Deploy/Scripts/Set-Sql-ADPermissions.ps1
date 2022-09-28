param(
    [Parameter(Mandatory = $true)]
    [string]$applicationId,
    [Parameter(Mandatory = $true)]
    [string]$servicePrincipalPassword,
    [Parameter(Mandatory = $true)]
    [string]$sqlServerName,
    [Parameter(Mandatory = $true)]
    [string]$databaseName,
    [Parameter(Mandatory = $true)]
    [string]$objgroupId,
    [Parameter(Mandatory = $true)]
    [string]$sqlDevsGroupName,
    [Parameter(Mandatory = $true)]
    [string]$tenantId
)

Function Get-AADToken {
    [CmdletBinding()]
    [OutputType([string])]
    PARAM (
        [String]$TenantID,
        [string]$ServicePrincipalId,
        [string]$ServicePrincipalPwd
    )
    Try {

        $resourceAppIdURI = 'https://database.windows.net/'

        $tokenResponse = Invoke-RestMethod -Method Post -UseBasicParsing `
            -Uri "https://login.windows.net/$($TenantID)/oauth2/token" `
            -Body @{
            resource      = $resourceAppIdURI
            client_id     = $ServicePrincipalId
            grant_type    = 'client_credentials'
            client_secret = $ServicePrincipalPwd
        } -ContentType 'application/x-www-form-urlencoded'

        if ($tokenResponse) {
            Write-Output "Access token type is $($tokenResponse.token_type), expires $($tokenResponse.expires_on)"
            return $tokenResponse.access_token
        }
    }
    Catch {
        Throw $_
        $ErrorMessage = 'Failed to acquire Azure AD token.'
        Write-Error -Message $ErrorMessage
    }
}

$password = $servicePrincipalPassword;
$token = Get-AADToken -TenantID $tenantId -ServicePrincipalId $applicationId -ServicePrincipalPwd $password -OutVariable SPNToken

Write-Verbose "Create SQL connectionstring"
$conn = New-Object System.Data.SqlClient.SQLConnection
$conn.ConnectionString = "Data Source=$sqlServerName.database.windows.net;Initial Catalog=$databaseName;Connect Timeout=30"
$conn.AccessToken = $token

function ConvertTo-Sid {
    param (
        [string]$appId
    )
    [guid]$guid = [System.Guid]::Parse($appId)
    foreach ($byte in $guid.ToByteArray()) {
        $byteGuid += [System.String]::Format("{0:X2}", $byte)
    }
    return "0x" + $byteGuid
}

$SID = ConvertTo-Sid -appId $objgroupId

Write-Output "Connect to database and execute SQL script"
$conn.Open()
$query = "CREATE USER [$($sqlDevsGroupName)] WITH SID = $($SID), TYPE = X;
ALTER ROLE db_datareader ADD MEMBER [$($sqlDevsGroupName)];"
$command = New-Object -TypeName System.Data.SqlClient.SqlCommand($query, $conn)
$command.ExecuteNonQuery()
$conn.Close()