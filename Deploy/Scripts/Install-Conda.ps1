param(
	[Parameter(Mandatory=$True)]
	[string]
	$storageAccountName,

	[Parameter(Mandatory=$True)]
	[string]
	$keyVaultName,

	[Parameter(Mandatory=$True)]
	[string]
	$user
)


#*********************************************************************************************************************************
#Script body                                                                                                                    **
#Execution begins here                                                                                                           **
#*********************************************************************************************************************************

$ownershipFile = 'https://' + $storageAccountName +'.blob.core.windows.net/scripts/Ownership-Prediction.yml'
$pipReqFile = 'https://' + $storageAccountName +'.blob.core.windows.net/scripts/pip-requirements.txt'
$analytics = 'https://' + $storageAccountName +'.blob.core.windows.net/scripts/Analitica Propiedad Ecopetrol.zip'
$preprod = 'https://' + $storageAccountName +'.blob.core.windows.net/scripts/Preprocesamiento de datos Knime.zip'
$secrets = 'https://' + $storageAccountName +'.blob.core.windows.net/scripts/Secrets.py'
$trainOwnModel = 'https://' + $storageAccountName +'.blob.core.windows.net/scripts/train_ownership_models.py'
$trainAzureSql = 'https://' + $storageAccountName +'.blob.core.windows.net/scripts/training_azure_sql.py'
$updateXbgoostModel = 'https://' + $storageAccountName +'.blob.core.windows.net/scripts/update_xgboost_models.py'

$keyVaultValue = 'https://' + $keyVaultName + '.vault.azure.net/'
$unzipdest = 'C:\Users\' + $user + '\Documents'

#create environment variable KEY_VAULT to use with python scripts
setx /M KEY_VAULT $keyVaultValue

#download files required to update conda and to run python scripts
azcopy copy $analytics 'C:\Analitica Propiedad Ecopetrol.zip'

azcopy copy $preprod 'C:\Preprocesamiento de datos Knime.zip'

azcopy copy $ownershipFile 'C:\Ownership-Prediction.yml'

azcopy copy $pipReqFile 'C:\pip-requirements.txt'

#Install module of 7 zip to unziped files

Install-Module -Name 7Zip4Powershell -Force

Import-Module -Name 7Zip4Powershell


#unzipped files

Expand-7Zip -ArchiveFileName "C:\Analitica Propiedad Ecopetrol.zip" -TargetPath $unzipdest

Expand-7Zip -ArchiveFileName "C:\Preprocesamiento de datos Knime.zip" -TargetPath $unzipdest

#download python script from container

azcopy copy $secrets "C:\Users\$user\Documents\Analitica Propiedad Ecopetrol\Secrets.py"

azcopy copy $trainOwnModel "C:\Users\$user\Documents\Analitica Propiedad Ecopetrol\train_ownership_models.py"

azcopy copy $trainAzureSql "C:\Users\$user\Documents\Analitica Propiedad Ecopetrol\training_azure_sql.py"

azcopy copy $updateXbgoostModel "C:\Users\$user\Documents\Analitica Propiedad Ecopetrol\update_xgboost_models.py"
#commands to download and install Knime

choco install knime.install --yes

#commands to download and install conda repo

conda env create --name Ownership-Prediction -f C:\Ownership-Prediction.yml

C:\Miniconda\envs\Ownership-Prediction\python.exe -m pip install -r C:\pip-requirements.txt
