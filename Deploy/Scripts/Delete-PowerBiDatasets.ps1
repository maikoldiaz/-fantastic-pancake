##Parameters
param(
	[Parameter(Mandatory=$true)]
	[string]$ModulePath,
	[Parameter(Mandatory=$true)]
	[string]$WorkspaceId,
	[Parameter(Mandatory=$true)]
	[string]$TenantId,
	[Parameter(Mandatory=$true)]
	[string]$PowerBIAppId,
	[Parameter(Mandatory=$true)]
	[string]$PowerBIAppSecret,
    [Parameter(Mandatory=$true)]
	[string]$PBIXPath
)

Import-Module -Name "$ModulePath\FindIfStringInArray"

# Get all datasets
function Get-AllDataset($baseUrl, $token) {
    # Create a httpclient with headers
	$httpclient = [System.Net.Http.HttpClient]::new();
	$httpclient.DefaultRequestHeaders.Add("Accept", "application/json");
    # Set the token in client header
	$httpclient.DefaultRequestHeaders.Add("Authorization", $token);

    $datasourceJson = $httpclient.GetAsync($baseUrl).Result.Content.ReadAsStringAsync().Result | ConvertFrom-Json
    return $datasourceJson.value;
}

# Gives datasets for which the current reports have been previously uploaded
function Get-DatasetsToDelete($reportNames, $datasets) {
    # Create a httpclient with headers
	$datasetsToBeDeleted = New-Object System.Collections.ArrayList
    foreach( $dataset in $datasets) {
        $result = Find-IfStringInArray -arr $reportNames -str $dataset.name
        if($result) {
            $null = $datasetsToBeDeleted.Add($dataset);
        }
    }
    return $datasetsToBeDeleted
}

# Delete all datasets (removes reports)
function Clear-AllDataset($baseUrl, $datasets, $token) {
    # Declare headers with token
	$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
	$headers.Add("Accept", 'application/json')
	$headers.Add("Content-Type", 'application/json')
	$headers.add("Authorization", $token.ToString())

    foreach( $dataset in $datasets) {
        Write-Output "Dataset ID: " $dataset.id;
        $uripath = $baseUrl + "/" + $dataset.id;
        Invoke-RestMethod -Method Delete -Uri $uripath -Headers $headers -Debug -Verbose
        Write-Output "Deleted"
    }
}

# Extract pbix file names
function Get-PBIXFileName($pbixFiles) {
    $names = New-Object System.Collections.ArrayList
    foreach($pbixFile in $pbixFiles) {
        $fileName = $pbixFile.BaseName
        # PowerBi PBIX names are like 10.10.05CartadeControl05 and it's corresponding dataset name are like 10-10-05CARTADECONTROL05.
        # Hence formatting the report name to match the dataset name.
        $fileName = $fileName.ToUpper().Replace('.','-');
        $null =$names.Add($fileName);
    }
    return $names;
}

try
{
	# Variables
	$restUrlWorkspaces = "https://api.powerbi.com/v1.0/myorg/groups/" + $WorkspaceId + "/datasets";

	# Connect to Power BI service Account.
	$credentials = New-Object System.Management.Automation.PSCredential ($PowerBIAppId, (convertto-securestring $PowerBIAppSecret -asplaintext -force))

	Connect-PowerBIServiceAccount -ServicePrincipal -Credential $credentials -Tenant $TenantId

	# Get the token
	$token = (MicrosoftPowerBIMgmt.Profile\Get-PowerBIAccessToken -AsString)

    # Get All Datasets
    $datasets = Get-AllDataset -baseUrl $restUrlWorkspaces  -token $token

    # Get all to be uploaded pbix files
    $pbixFiles = Get-ChildItem -Path $PBIXPath -File

    # Extract names
    $reportNames = Get-PBIXFileName -pbixFiles $pbixFiles

    # Datasets for which the current reports have been previously uploaded
    $datasetsToBeDeleted = Get-DatasetsToDelete -reportNames $reportNames -datasets $datasets

    # Delete Datasets
    Clear-AllDataset -baseUrl $restUrlWorkspaces -datasets $datasetsToBeDeleted -token $token

    Write-Output "Deleted datasets"
}
catch
{
	Write-Error -Message "Error Message: " -Exception $_.Exception
	throw $_.Exception
}