param(
 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroup,

 [Parameter(Mandatory=$true)]
 [string]
 $storageAccountName,

 [Parameter(Mandatory=$true)]
 [string]
 $downloadPath
 )

 $storageAccount = Get-AzStorageAccount -ResourceGroupName $resourceGroup -Name $storageAccountName
 $storageContext = $storageAccount.Context
 $containersAndFilesHash = @{ operativemovementshistory = "MOV_OP_CONSOLIDADOS_TRUE.csv"; workfiles = "OwnershipPercentageValues.csv";}
 foreach ($key in $containersAndFilesHash.GetEnumerator())
 {
         Write-Output "Validating the presence of blobs in $($key.Name) now..."
         $storageContainer = Get-AzStorageContainer -Context $storageContext -ErrorAction Stop | where-object {$_.Name -eq $key.Name}
         If($storageContainer)
             {
                 $fileNameArray = $key.Value.Split(",").Trim();
                 foreach($fileName in $fileNameArray)
                 {
                    $blob = Get-AzStorageBlob -Context $storageContext -Container $key.Name -ErrorAction Stop | where-object {$_.Name -eq $fileName}
                    If($blob)
                        {
                            Get-AzStorageBlobContent -Context $storageContext -Container $key.Name -Blob $blob.Name -Destination $downloadPath
                            $count = Get-Content "$downloadPath\$fileName" | Measure-Object -Line
                            if ($fileName -eq "MOV_OP_CONSOLIDADOS_TRUE.csv")
                            {
                                $operativeMovementsCount = $count.Lines - 1
                                Write-Output "##vso[task.setvariable variable=operativeMovementsCount]$operativeMovementsCount"
                            }
                            if ($fileName -eq "OwnerShipPercentageValues.csv")
                            {
                                $ownershipPercentageValuesCount = $count.Lines - 1
                                Write-Output "##vso[task.setvariable variable=ownershipPercentageValuesCount]$ownershipPercentageValuesCount"
                            }
                            Write-Output "$fileName blob found."
                        }
                        Else
                            {
                                Write-Error "$fileName blob not found."
                            }
                 }
             }
         Else
             {
                Write-Error "$($key.Name) storage container not found."
             }
 }