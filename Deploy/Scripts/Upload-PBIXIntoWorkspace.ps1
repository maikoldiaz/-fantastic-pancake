##Parameters
param(
	[Parameter(Mandatory=$true)]
	[string]$ModulePath,
    [Parameter(Mandatory=$true)]
	[string]$PBIXPath,
	[Parameter(Mandatory=$true)]
	[string]$WorkspaceId,
	[Parameter(Mandatory=$true)]
	[string]$AnalysisServerDataSource,
    [Parameter(Mandatory=$true)]
	[string]$AnalysisServerDBNameDataSource,
    [Parameter(Mandatory=$true)]
	[string]$AnalysisServerAuditDBNameDataSource,
	[Parameter(Mandatory=$true)]
	[string]$TenantId,
	[Parameter(Mandatory=$true)]
	[string]$PowerBIAppId,
	[Parameter(Mandatory=$true)]
	[string]$PowerBIAppSecret,
	[Parameter(Mandatory=$true)]
	[string]$PowerBIObjectId,
    [Parameter(Mandatory = $true)]
    [String]$SqlServerConnectionString,
	[Parameter(Mandatory = $true)]
	[String]$SqlPowerBiReports,
	[Parameter(Mandatory = $true)]
	[String]$AuditModelPowerBiReports
)

Import-Module -Name "$ModulePath\GetValue"
Import-Module -Name "$ModulePath\FindIfStringInArray"

# Generates an array by splitting string by comma
function Get-ArrayByStringSplit([string]$reportNamesString) {
	$splitValue = $reportNamesString.Split(',');
	return $splitValue;
}

# This function uploads a PBIX to the workspace
function Publish-PowerBIReport([string]$pbixPath, [string]$fileName, [string]$powerBiServiceWorkspaceUrl, [System.Net.Http.HttpClient]$httpClient) {
	# Full Path of Report
	$pbixPathReport = $pbixPath+$fileName+".pbix"
	$multipartContent = [System.Net.Http.MultipartFormDataContent]::new([Guid]::NewGuid().toString())
	$fileStream = [System.IO.File]::Open($pbixPathReport, [System.IO.FileMode]::Open)

	# Form header
	$fileHeader = [System.Net.Http.Headers.ContentDispositionHeaderValue]::new("form-data")
	$fileHeader.Name = "file"
	$fileHeader.FileName = $fileName
	$fileContent = [System.Net.Http.StreamContent]::new($fileStream)
	$fileContent.Headers.ContentDisposition = $fileHeader
	$fileContent.Headers.ContentType = [System.Net.Http.Headers.MediaTypeHeaderValue]::Parse("application/octet-stream")
	$multipartContent.Add($fileContent)

	# Url for uploading report
	$displayName = $fileName.Replace(".","-").ToUpper()
	$fileUploadURI = $powerBiServiceWorkspaceUrl +"imports?datasetDisplayName=" + $displayName+ "&nameConflict=CreateOrOverwrite";

	# Rest Call to create the report.
	$response = $httpclient.PostAsync($fileUploadURI, $multipartContent).Result

	$fileStream.Close()

	# Check for Request completion by polling the request ID
	if ($response.StatusCode.toString() -eq ("Accepted"))
	{
		$request = $response.Content.ReadAsStringAsync().Result | ConvertFrom-Json;

		do
		{
			$pollResponse = $httpclient.GetAsync($powerBiServiceWorkspaceUrl + "imports/" + $request.id).Result.Content.ReadAsStringAsync().Result | ConvertFrom-Json;
		}
		while ($pollResponse.importState -ne "Succeeded");
	}
	return $pollResponse
}

# This function updates datasource depending on the type of the report (name of the report)
function Edit-Datasource{
	param (
        $parameters
	)
    # Extract values
    $accessToken = $parameters.accessToken
	$workspaceId = $parameters.workspaceId
	$httpClient = $parameters.httpClient
	$fileName = $parameters.fileName
	$powerBiServiceRootUrl = $parameters.powerBiServiceRootUrl
	$powerBiServiceWorkspaceUrl = $parameters.powerBiServiceWorkspaceUrl
	$datasetId = $parameters.datasetId
	$reportId = $parameters.reportId
	$datasource = $parameters.datasource
	$analysisServerDataSource = $parameters.analysisServerDataSource
	$analysisServerDBNameDataSource = $parameters.analysisServerDBNameDataSource
	$analysisServerAuditDBNameDataSource = $parameters.analysisServerAuditDBNameDataSource
	$connectionString = $parameters.connectionString
	$sqlBasedReportNames = $parameters.sqlBasedReportNames
	$auditModelBasedReportNames = $parameters.auditModelBasedReportNames

	$json = $null
	if(Find-IfStringInArray -arr $sqlBasedReportNames -str $fileName) {
		$json = Edit-DatasourceForSqlBasedReport -httpClient $httpClient -accessToken $accessToken -datasource $datasource -datasetId $datasetId -reportId $reportId -fileName $fileName -powerBiServiceRootUrl $powerBiServiceRootUrl -powerBiServiceWorkspaceUrl $powerBiServiceWorkspaceUrl -workspaceId $workspaceId -connectionString $connectionString
	} elseif(Find-IfStringInArray -arr $auditModelBasedReportNames -str $fileName) {
		$json = Edit-DatasourceForAasBasedReport -httpClient $httpClient -isAudit $true -datasource $datasource -powerBiServiceWorkspaceUrl $powerBiServiceWorkspaceUrl -fileName $fileName -datasetId $datasetId -reportId $reportId -workspaceId $workspaceId -analysisServerDataSource $analysisServerDataSource -analysisServerDBNameDataSource $analysisServerDBNameDataSource -analysisServerAuditDBNameDataSource $analysisServerAuditDBNameDataSource
	} else {
		$json = Edit-DatasourceForAasBasedReport -httpClient $httpClient -isAudit $false -datasource $datasource -powerBiServiceWorkspaceUrl $powerBiServiceWorkspaceUrl -fileName $fileName -datasetId $datasetId -reportId $reportId -workspaceId $workspaceId -analysisServerDataSource $analysisServerDataSource -analysisServerDBNameDataSource $analysisServerDBNameDataSource -analysisServerAuditDBNameDataSource $analysisServerAuditDBNameDataSource
	}
	return $json
}

# This function is to update datasource for SQL based report
function Edit-DatasourceForSqlBasedReport($httpClient, $accessToken, $datasource, $datasetId, $reportId, $fileName, $powerBiServiceRootUrl, $powerBiServiceWorkspaceUrl, $connectionString, $workspaceId) {
	# Get credentials from SQL Connection string
    $dBUsername = Get-Value -InputString $connectionString -Key "User ID"
    $dBPassword = Get-Value -InputString $connectionString -Key "Password" #pragma: allowlist secret
    $dbServer = Get-Value -InputString $connectionString -Key "Server"
	$dbName = Get-Value -InputString $connectionString -Key "Initial Catalog"

	# Update datasource for SQL based PowerBI report
	# Request body for replacing an existing datasource in the PBIX file with the one we need
	$UpdateBodyText = '{
		"updateDetails": [
		{
			"datasourceSelector": '+($datasource|ConvertTo-Json) +',
			"connectionDetails": {
			"server": "' + $dbServer+ '",
				"database": "' + $dbName + '"
			}
		}]
		}';
	# SQL Username and password body
	$credentialsBody = '{"credentialType":"Basic","basicCredentials":{"username":"'+$dbUsername +'","password":"'+ $dbPassword +'"}}' #pragma: allowlist secret
	$UpdateBody = [System.Net.Http.StringContent]::new($UpdateBodyText);
	$UpdateBody.Headers.ContentType='application/json';

	# API call to set dataset's datasource
	$null = $httpclient.PostAsync($powerBiServiceWorkspaceUrl + "datasets/" + $datasetId + "/Default.UpdateDatasources",$UpdateBody).Result.StatusCode
	$gatewayDataSources = $httpclient.GetAsync($powerBiServiceWorkspaceUrl + "datasets/" + $datasetId + "/Default.GetBoundGatewayDataSources").Result.Content.ReadAsStringAsync().Result|ConvertFrom-Json;
	$datasourceId = $gatewayDataSources.value[0].id
	$gatewayId = $gatewayDataSources.value[0].gatewayId
	$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
	$headers.Add("Accept", 'application/json')
	$headers.Add("Content-Type", 'application/json')
	$headers.add("Authorization", $accessToken.ToString())
	$uripath = $powerBiServiceRootUrl + "gateways/" + $gatewayId + "/datasources/"+ $datasourceId +"/"

	# API call to set SQL credentials for the report
	$null = Invoke-RestMethod -Method Patch -Uri $uripath -Headers $headers -Body $credentialsBody -ContentType 'application/json' -Debug -Verbose

	$json = '\"'+$fileName+'\":{\"ReportId\":\"'+$reportId+'\",\"GroupId\":\"'+$workspaceId+'\",\"DataSetId\":\"'+$datasetId+'\",\"IsAnalysis\":'+0+'},'
	return $json
}

# This function is to update datasource for AAS based report
function Edit-DatasourceForAasBasedReport($httpClient, $isAudit, $datasource, $powerBiServiceWorkspaceUrl, $fileName, $datasetId, $reportId, $workspaceId, $analysisServerDataSource, $analysisServerDBNameDataSource, $analysisServerAuditDBNameDataSource) {
	# Model name
	$modelName = If ($isAudit) {$analysisServerAuditDBNameDataSource} Else {$analysisServerDBNameDataSource}

	# Update datasource for Analysis Service based PowerBI report
	$UpdateBodyText = '{
		"updateDetails": [
		{
			"datasourceSelector": '+($datasource | ConvertTo-Json) +',
			"connectionDetails": {
			"server": "' + $analysisServerDataSource + '",
			"database": "' + $modelName + '"
			}
		}
		]
	}';

	$UpdateBody = [System.Net.Http.StringContent]::new($UpdateBodyText);
	$UpdateBody.Headers.ContentType='application/json';

	# API Call to update the datasource
	$null = $httpclient.PostAsync($powerBiServiceWorkspaceUrl + "datasets/" + $datasetId + "/Default.UpdateDatasources",$UpdateBody).Result.StatusCode

	$json = '\"'+$fileName+'\":{\"ReportId\":\"'+$reportId+'\",\"GroupId\":\"'+$workspaceId+'\",\"DataSetId\":\"'+$datasetId+'\",\"IsAnalysis\":'+1+'},'
	return $json
}

function Export-ReportsAndModifyDatasource
{
	param (
		$parameters
	)

    # Extract values
    $accessToken = $parameters.accessToken
	$pbixPath = $parameters.pbixPath
	$workspaceId = $parameters.workspaceId
	$analysisServerDataSource = $parameters.analysisServerDataSource
	$analysisServerDBNameDataSource = $parameters.analysisServerDBNameDataSource
	$analysisServerAuditDBNameDataSource = $parameters.analysisServerAuditDBNameDataSource
	$tenantId = $parameters.tenantId
	$powerBiAppId = $parameters.powerBiAppId
	$powerBiObjectId = $parameters.powerBiObjectId
	$connectionString = $parameters.connectionString
	$sqlPowerBiReports = $parameters.sqlPowerBiReports
	$auditModelPowerBiReports = $parameters.auditModelPowerBiReports

	# Set base url and basic request headers
	$powerBiServiceRootUrl = "https://api.powerbi.com/v1.0/myorg/";
	$powerBiServiceWorkspaceUrl = $powerBiServiceRootUrl +"groups/"+ $workspaceId + "/";

	# Define HttpClient using token
	$httpclient = [System.Net.Http.HttpClient]::new();
	$httpclient.DefaultRequestHeaders.Add("Accept", "application/json");
	$httpclient.DefaultRequestHeaders.Add("Authorization", $accessToken);

	# Get all pbix files
	$pbixfiles = Get-ChildItem -Path $pbixPath -File

	$reportjson=$null

	# Get SQL based PowerBI Report names in a Array by splitting report names string by a comma
	$sqlBasedReportNames = Get-ArrayByStringSplit -reportNamesString $sqlPowerBiReports

	# Get Audit Model based PowerBI Report names in a Array by splitting report names string by a comma
	$auditModelBasedReportNames = Get-ArrayByStringSplit -reportNamesString $auditModelPowerBiReports

	foreach($pbix in $pbixfiles)
	{
		#Execution Starts here
		$fileName = $pbix.BaseName

		# Upload report
		$response = Publish-PowerBIReport -pbixPath $pbixPath -fileName $fileName -powerBiServiceWorkspaceUrl $powerBiServiceWorkspaceUrl -httpClient $httpClient

		# Dataset Id and Report Id
		$powerBiDataSetId = $response.datasets[0].id
		$powerBiReportId  = $response.reports[0].id

		# Get current datasource of the uploaded report
		$datasourceJson = $httpclient.GetAsync($powerBiServiceWorkspaceUrl + "datasets/" + $powerBiDataSetId + "/datasources").Result.Content.ReadAsStringAsync().Result | ConvertFrom-Json
		$datasource = $datasourceJson.value[0]

		$parameters = @{
			accessToken = $token
			workspaceId = $workspaceId
			httpClient = $httpClient
			fileName = $fileName
			powerBiServiceRootUrl = $powerBiServiceRootUrl
			powerBiServiceWorkspaceUrl = $powerBiServiceWorkspaceUrl
			datasetId = $powerBiDataSetId
			reportId = $powerBiReportId
			datasource = $datasource
			analysisServerDataSource = $analysisServerDataSource
			analysisServerDBNameDataSource = $analysisServerDBNameDataSource
			analysisServerAuditDBNameDataSource = $analysisServerAuditDBNameDataSource
			connectionString = $connectionString
			sqlBasedReportNames = $sqlBasedReportNames
			auditModelBasedReportNames = $auditModelBasedReportNames
		}

		# Update datasource for the uploaded report
		$output = Edit-Datasource $parameters

		$reportjson = $reportjson + $output;
	}

	##Creating a Json for Table Configuration.
	$reports= $reportjson.TrimEnd(",")

	$data = '{\"ReportSettings.TenantId\":\"'+$tenantId+'\",\"ReportSettings.ClientId\":\"'+$powerBiAppId+'\",\"ReportSettings.PrincipalId\":\"'+$powerBiObjectId+'\",\"ReportSettings.Reports\":{'+$reports+'}}'

	Write-Output $data
	##Set Output Variable.
	$key="reportconfig"
	$value = $data
	Write-Output "##vso[task.setvariable variable=$key;]$value"
}

#******************************************************************************
# Execution begins here
#******************************************************************************

try
{

	##Variables
	$DeploymentPrincipalAppId = $PowerBIAppId
	$DeploymentPrincipalAppSecret = $PowerBIAppSecret

	##Connect to Power BI service Account.
	$credentials = New-Object System.Management.Automation.PSCredential ($DeploymentPrincipalAppId, (convertto-securestring $DeploymentPrincipalAppSecret -asplaintext -force))

	Connect-PowerBIServiceAccount -ServicePrincipal -Credential $credentials -Tenant $TenantId

	# Get the token
	$token = (MicrosoftPowerBIMgmt.Profile\Get-PowerBIAccessToken -AsString)

	$parameters = @{
		accessToken = $token
		pbixPath = $PBIXPath
		workspaceId = $WorkspaceId
		analysisServerDataSource = $AnalysisServerDataSource
		analysisServerDBNameDataSource = $AnalysisServerDBNameDataSource
		analysisServerAuditDBNameDataSource = $AnalysisServerAuditDBNameDataSource
		tenantId = $TenantId
		powerBiAppId = $DeploymentPrincipalAppId
		powerBiObjectId = $PowerBIObjectId
		connectionString = $SqlServerConnectionString
		sqlPowerBiReports = $SqlPowerBiReports
		auditModelPowerBiReports = $AuditModelPowerBiReports
	}
	Export-ReportsAndModifyDatasource $parameters
}
catch
{
	Write-Error -Message "Error Message: " -Exception $_.Exception
	throw $_.Exception
}