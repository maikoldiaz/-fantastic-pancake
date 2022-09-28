param(
    [Parameter(Mandatory = $true)]
    [string]$WorkspaceId,
    [Parameter(Mandatory = $true)]
    [string]$TenantId,
    [Parameter(Mandatory = $true)]
    [string]$PowerBIAppId,
    [Parameter(Mandatory = $true)]
    [string]$PowerBIAppSecret,
    [Parameter(Mandatory = $true)]
    [string]$reportsToDelete
)


function Get-AllReports($baseUrl, $token) {
    # Create a httpclient with headers
    $httpclient = [System.Net.Http.HttpClient]::new();
    $httpclient.DefaultRequestHeaders.Add("Accept", "application/json");
    # Set the token in client header
    $httpclient.DefaultRequestHeaders.Add("Authorization", $token);

    $datasourceJson = $httpclient.GetAsync($baseUrl).Result.Content.ReadAsStringAsync().Result | ConvertFrom-Json
    return $datasourceJson.value;
}

function Clear-Report($allReports, $token, $reportsToDelete) {

    foreach ( $report in $allReports) {
        if ($reportsToDelete.contains($report.name)) {

            $baseUrl = "https://api.powerbi.com/v1.0/myorg/groups/" + $WorkspaceId + "/reports/" + $report.Id;
            # Declare headers with token
            $headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
            $headers.Add("Accept", 'application/json')
            $headers.Add("Content-Type", 'application/json')
            $headers.add("Authorization", $token.ToString())
            Invoke-RestMethod -Method Delete -Uri $baseUrl -Headers $headers -Debug -Verbose
            Write-Output "Deleted"
        }
    }
}

try {
    # Variables
    $restUrlWorkspaces = "https://api.powerbi.com/v1.0/myorg/groups/" + $WorkspaceId + "/reports";

    # Connect to Power BI service Account.
    $credentials = New-Object System.Management.Automation.PSCredential ($PowerBIAppId, (convertto-securestring $PowerBIAppSecret -asplaintext -force))

    Connect-PowerBIServiceAccount -ServicePrincipal -Credential $credentials -Tenant $TenantId

    # Get the token
    $token = (MicrosoftPowerBIMgmt.Profile\Get-PowerBIAccessToken -AsString)

    # Get All Reports
    $reports = Get-AllReports -baseUrl $restUrlWorkspaces -token $token

    # Delete Datasets
    Clear-Report -allReports $reports -token $token -reportsToDelete $reportsToDelete

    Write-Output "Reports Deleted"
}
catch {
    Write-Error -Message "Error Message: " -Exception $_.Exception
    throw $_.Exception
}