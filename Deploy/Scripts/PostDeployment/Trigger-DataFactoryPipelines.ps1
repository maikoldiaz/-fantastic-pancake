param(
 [Parameter(Mandatory=$true)]
 [string]
 $resourceGroup,

 [Parameter(Mandatory=$true)]
 [string]
 $dataFactoryName
 )

 $dataFactory = Get-AzDataFactoryV2 -ResourceGroupName $resourceGroup -Name $dataFactoryName
 $truePipelines = "ADF_OperativeMovementsHistory", "ADF_OwnershipPercentageValuesHistory"
 $checkLoopTime = 30
    If($dataFactory) {
               $pipelines = Get-AzDataFactoryV2Pipeline -ResourceGroupName $resourceGroup -DataFactoryName $dataFactoryName | Where-Object {$truePipelines -contains $_.Name}
               Write-Output "Running the pipeline with parameters $($env:loadType), $($env:startDate), $($env:endDate)"
               $parameters = @{
                "LoadType" = "$($env:loadType)"
                "StartDate" = "$($env:startDate)"
                "EndDate" = "$($env:endDate)"
               }
               foreach ($pipeline in $pipelines)
               {
                    Write-Output "triggering the pipeline $($pipeline.Name)"
                    if ($pipeline.Name -eq "ADF_OperativeMovementsHistory" -or $pipeline.Name -eq "ADF_OperativeMovementswithOwnerShip")
                    {
                        $runId = Invoke-AzDataFactoryV2Pipeline -ResourceGroupName $resourceGroup -DataFactoryName $dataFactoryName -PipelineName $pipeline.Name -Parameter $parameters
                    }
                    else{
                        $runId = Invoke-AzDataFactoryV2Pipeline -ResourceGroupName $resourceGroup -DataFactoryName $dataFactoryName -PipelineName $pipeline.Name
                    }

                    $runInfo = Get-AzDataFactoryV2PipelineRun -ResourceGroupName $resourceGroup -DataFactoryName $dataFactoryName -PipelineRunId $runId

                    Write-Output "`nPipeline triggered!"
                    Write-Output "RunID: $($runInfo.RunId)"
                    Write-Output "Started: $($runInfo.RunStart)`n"

                    $sw =  [system.diagnostics.stopwatch]::StartNew()
                    While ("InProgress", "Queued" -contains (Get-AzDataFactoryV2PipelineRun -ResourceGroupName $resourceGroup -DataFactoryName $dataFactoryName -PipelineRunId $runId | Select-Object -ExpandProperty "Status"))
                        {
                            $runInfo = Get-AzDataFactoryV2PipelineRun -ResourceGroupName $resourceGroup -DataFactoryName $dataFactoryName -PipelineRunId $runId
                            Write-Output "`rLast status: $($runInfo.Status) | Last updated: $($runInfo.LastUpdated) | Running time: $($sw.Elapsed.ToString('dd\.hh\:mm\:ss'))" #-NoNewline
                            Start-Sleep $checkLoopTime
                        }
                    $sw.Stop()

                    $runInfo = Get-AzDataFactoryV2PipelineRun -ResourceGroupName $resourceGroup -DataFactoryName $dataFactoryName -PipelineRunId $runId

                    Write-Output "`nFinished running in $($sw.Elapsed.ToString('dd\.hh\:mm\:ss'))!"
                    Write-Output "Status:"
                    Write-Output $runInfo.Status

                    if ($runInfo.Status -ne "Succeeded"){
                        throw "There was an error with running pipeline: $($runInfo.Name). Returned message was:`n$($runInfo.Message)"
                    }
               }
    }
