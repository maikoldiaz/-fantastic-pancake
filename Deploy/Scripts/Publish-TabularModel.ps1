param(
    [Parameter(Mandatory=$true)]
	[string]$dependenciesPath,
    [Parameter(Mandatory=$true)]
	[string]$modelPath,
    [Parameter(Mandatory=$true)]
	[string]$sqlConnString,
    [Parameter(Mandatory=$true)]
    [string]$aasADAppClientId,
    [Parameter(Mandatory=$true)]
    [string]$aasADAppClientSecret,
    [Parameter(Mandatory=$true)]
    [string]$TenantId,
    [Parameter(Mandatory=$true)]
    [string]$analysisServicesName,
    [Parameter(Mandatory=$true)]
    [string]$analysisServerModelName,
    [Parameter(Mandatory=$true)]
    [string]$rolloutenvironment,
	[Parameter(Mandatory=$true)]
    [string]$environment,
    [Parameter(Mandatory=$false)]
    [string]$modelNameSuffix
	)

#******************************************************************************
# Script body
# Execution begins here
#******************************************************************************


try
{

    # Using AAD credentials
    $password = ConvertTo-SecureString -String $aasADAppClientSecret -AsPlainText -Force #pragma: allowlist secret
    $credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $aasADAppClientId, $password

    Write-Output "Changing Execution path to..."$modelPath

    # Changing work directory to tabular model location
    Set-Location -Path $modelPath

    Write-Output "Generating XMLA File...."

    # Location of exe to generate XMLA from model.
    $AnalysisServiceDeploymentExe = "$dependenciesPath\Microsoft.AnalysisServices.Deployment.exe"

    # XMLA is used for publishing tabular model to a target analysis server. Itcontains information which the
    # model project has like Datasource (sql connection string), model objects, target model name.

    # Generate XMLA from model (using .asdatabase file)
    & $AnalysisServiceDeploymentExe "$modelPath\bin\Model.asdatabase"  /s:"$modelPath\bin\ScriptLog.txt" /o:"$modelPath\bin\CustomModel.xmla" /d

    Write-Output "Customising XMLA as per Environment that is deployed to...."

    Write-Output "Reading XMLA...."
    # Read XMLA for writing connection string, model name, refresh type

    $stringContent = Get-Content -Raw "$modelPath\bin\CustomModel.xmla"
    $stringContent = "{`"sequence`": {`"operations`": [" + $stringContent
    $stringContent = $stringContent -replace "{`r`n  `"refresh`"",",{`r`n  `"refresh`""
    $stringContent = $stringContent + "]}}";
    $tempContent = $stringContent | ConvertFrom-Json

    $connString = $sqlConnString

    # Model name
    $modelName = $analysisServerModelName
    if($modelNameSuffix){
        $modelName = $modelName + $modelNameSuffix
    }

    Write-Output "Modifying XMLA properties...."
	if(([bool]($tempContent.PSobject.Properties.name -match "sequence")))
	{
        # Update Model name
        $tempContent.sequence.operations[0].createOrReplace.object.database = $modelName
        $tempContent.sequence.operations[0].createOrReplace.database.name = $modelName
        $tempContent.sequence.operations[1].refresh.objects[0].database = $modelName

        # Refresh type = full
        $tempContent.sequence.operations[1].refresh.type = "full"
        # Update SQL Connection String
        $tempContent.update | ForEach-Object { for($i=0;$i -lt $tempContent.sequence.operations[0].createOrReplace.database.model.dataSources.Count;$i++)
                                    {
                                        #Update connection string
                                        $tempContent.sequence.operations[0].createOrReplace.database.model.dataSources[$i].connectionString = "$connString"
                                    }
                                }

        $tempContent.update | ForEach-Object { for ($i=0; $i -lt $tempContent.sequence.operations[0].createOrReplace.database.model.roles.Count; $i++)
                                    {
                                        for ($j=0; $j -lt $tempContent.sequence.operations[0].createOrReplace.database.model.roles[$j].members.Count; $j++)
                                        {
                                            $tempContent.sequence.operations[0].createOrReplace.database.model.roles[$i].members[$j] = $tempContent.sequence.operations[0].createOrReplace.database.model.roles[$i].members[$j] | Select-Object * -ExcludeProperty memberid
                                        }
                                    }
                                }
	}
	else
	{
        # Update Model name
        $tempContent.createOrReplace.object.database = $modelName
        $tempContent.createOrReplace.database.name = $modelName
        $tempContent.refresh.objects[0].database = $modelName

        # Refresh type = full
        $tempContent.refresh.type = "full"

        # Update SQL Connection String
        $tempContent.update | ForEach-Object { for($i=0;$i -lt $tempContent.createOrReplace.database.model.dataSources.Count;$i++)
                                    {
                                        # Update connection string
                                        $tempContent.createOrReplace.database.model.dataSources[$i].connectionString = "$connString"
                                    }
                                }

        $tempContent.update | ForEach-Object { for ($i=0; $i -lt $tempContent.createOrReplace.database.model.roles.Count; $i++)
                                    {
                                        for ($j=0; $j -lt $tempContent.createOrReplace.database.model.roles[$j].members.Count; $j++)
                                        {
                                            $tempContent.createOrReplace.database.model.roles[$i].members[$j] = $tempContent.createOrReplace.database.model.roles[$i].members[$j] | Select-Object * -ExcludeProperty memberid
                                        }
                                    }
                                }
    }

    Write-Output "Writing CustomizedModel.xmla ....."

    # Overwrite XMLA after adding connection string
    (($tempContent | ConvertTo-Json -Depth 100) -replace "dev", "$environment") | Set-Content  $modelPath\bin\CustomisedModel.xmla -Encoding utf8

    Write-Output "XMLA Generation is complete....."

    Write-Output "Starting Model Deployment....."

    # Login to Analysis Service using Credentials defined previously.
    Add-AzureAnalysisServicesAccount -Credential $Credential -ServicePrincipal -TenantId $TenantId -RolloutEnvironment $rolloutenvironment
    Write-Output "Adding AzureAnalysisServicesAccount completed."

    # Publish Tabular Model and receive Output
	$output = Invoke-ASCmd -InputFile "$modelPath\bin\CustomisedModel.xmla" -Server "asazure://$rolloutenvironment/$analysisServicesName"

    # Print and throw error depending on output
	if($output.Contains("Warning"))
    {
        Write-Output "Model Deployment completed with warning..." $output
    }
    elseif($output.Contains("exception"))
    {
        Write-Error -Message ("Model Deployment has failed...." + $output)
        throw $output
    }
    else
    {
        Write-Output "Model Deployment is complete....." $output
    }
}
catch{
    Write-Error -Message "Error Message: " -Exception $_.Exception
	throw $_.Exception
}



