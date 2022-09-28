param(
#[Parameter(Mandatory=$True)]
[string]
$resourceGroupName,

#[Parameter(Mandatory=$True)]
[string]
$storageAccountName,

#[Parameter(Mandatory=$True)]
[string]
$file,

#[Parameter(Mandatory=$True)]
[string]
$fileOwership,

#[Parameter(Mandatory=$True)]
[string]
$filePipReq,

#[Parameter(Mandatory=$True)]
[string]
$analytics,

#[Parameter(Mandatory=$True)]
[string]
$preprod,

#[Parameter(Mandatory=$True)]
[string]
$secrets,

#[Parameter(Mandatory=$True)]
[string]
$trainOwnModel,

#[Parameter(Mandatory=$True)]
[string]
$trainAzureSql,

#[Parameter(Mandatory=$True)]
[string]
$updateXbgoostModel
)


#*********************************************************************************************************************************
#Script body                                                                                                                    **
#Execution beins here                                                                                                           **
#*********************************************************************************************************************************

$storage=Get-AzStorageAccount -ResourceGroupName $resourceGroupName -Name $storageAccountName

$container=Get-AzStorageContainer -Name "scripts" -Context $storage.Context -ErrorAction SilentlyContinue


if(!$container) {
  Write-Output "Container scripts does not exists";
  Write-Output "Creating container scripts.";
  New-AzStorageContainer -Name "scripts" -Permission Container -Context $storage.Context
}
else {
   Write-Output "Usando el container scripts"
}



Set-AzStorageBlobContent -File $file -Container "scripts" -Blob "Install-Conda.ps1" -Context $storage.Context -Force

Set-AzStorageBlobContent -File $fileOwership -Container "scripts" -Blob "Ownership-Prediction.yml" -Context $storage.Context -Force

Set-AzStorageBlobContent -File $filePipReq -Container "scripts" -Blob "pip-requirements.txt" -Context $storage.Context -Force

Set-AzStorageBlobContent -File $analytics -Container "scripts" -Blob "Analitica Propiedad Ecopetrol.zip" -Context $storage.Context -Force

Set-AzStorageBlobContent -File $preprod -Container "scripts" -Blob "Preprocesamiento de datos Knime.zip" -Context $storage.Context -Force

Set-AzStorageBlobContent -File $secrets -Container "scripts" -Blob "Secrets.py" -Context $storage.Context -Force

Set-AzStorageBlobContent -File $trainOwnModel -Container "scripts" -Blob "train_ownership_models.py" -Context $storage.Context -Force

Set-AzStorageBlobContent -File $trainAzureSql -Container "scripts" -Blob "training_azure_sql.py" -Context $storage.Context -Force

Set-AzStorageBlobContent -File $updateXbgoostModel -Container "scripts" -Blob "update_xgboost_models.py" -Context $storage.Context -Force

