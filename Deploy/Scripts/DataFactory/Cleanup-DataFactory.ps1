param
(
    [parameter(Mandatory = $false)] [String] $armTemplate,
    [parameter(Mandatory = $false)] [String] $ResourceGroupName,
    [parameter(Mandatory = $false)] [String] $DataFactoryName,
    [parameter(Mandatory = $false)] [Bool] $predeployment=$true,
    [parameter(Mandatory = $false)] [String] $objectsToBeDeleted
)

function triggerSortUtil {
    param([Microsoft.Azure.Commands.DataFactoryV2.Models.PSTrigger]$trigger,
    [Hashtable] $triggerNameResourceDict,
    [Hashtable] $visited,
    [System.Collections.Stack] $sortedList)
    if ($visited[$trigger.Name] -eq $true) {
        return;
    }
    $visited[$trigger.Name] = $true;
    $trigger.Properties.DependsOn | Where-Object {$_ -and $_.ReferenceTrigger} | ForEach-Object{
        triggerSortUtil -trigger $triggerNameResourceDict[$_.ReferenceTrigger.ReferenceName] -triggerNameResourceDict $triggerNameResourceDict -visited $visited -sortedList $sortedList
    }
    $sortedList.Push($trigger)
}

function Get-SortedTriggers {
    param(
        [string] $DataFactoryName,
        [string] $ResourceGroupName
    )
    $triggers = Get-AzDataFactoryV2Trigger -ResourceGroupName $ResourceGroupName -DataFactoryName $DataFactoryName
    $triggerDict = @{}
    $visited = @{}
    $stack = new-object System.Collections.Stack
    $triggers | ForEach-Object{ $triggerDict[$_.Name] = $_ }
    $triggers | ForEach-Object{ triggerSortUtil -trigger $_ -triggerNameResourceDict $triggerDict -visited $visited -sortedList $stack }
    $sortedList = new-object Collections.Generic.List[Microsoft.Azure.Commands.DataFactoryV2.Models.PSTrigger]

    while ($stack.Count -gt 0) {
        $sortedList.Add($stack.Pop())
    }
    $sortedList
}

function deleteTrigger {
    param(
        [string] $Name
    )
    Write-Output "Deleting trigger "  $Name
    $trig = Get-AzDataFactoryV2Trigger -name $Name -ResourceGroupName $ResourceGroupName -DataFactoryName $DataFactoryName -ErrorAction SilentlyContinue
    if ($trig.RuntimeState -eq "Started") {
        Stop-AzDataFactoryV2Trigger -ResourceGroupName $ResourceGroupName -DataFactoryName $DataFactoryName -Name $Name -ErrorAction SilentlyContinue -Force
    }
    Remove-AzDataFactoryV2Trigger -Name $Name -ResourceGroupName $ResourceGroupName -DataFactoryName $DataFactoryName -ErrorAction SilentlyContinue -Force
}

function deletePipeline {
    param(
        [string] $Name
    )
    Write-Output "Deleting pipelines"  $Name
    Remove-AzDataFactoryV2Pipeline -Name $Name -ResourceGroupName $ResourceGroupName -DataFactoryName $DataFactoryName -ErrorAction SilentlyContinue -Force
}

function deleteDataset {
    param(
        [string] $Name
    )
    Write-Output "Deleting dataset " $Name
    Remove-AzDataFactoryV2Dataset -Name $Name -ResourceGroupName $ResourceGroupName -DataFactoryName $DataFactoryName -ErrorAction SilentlyContinue -Force
}

function deleteLinkedService {
    param(
        [string] $Name
    )
    Write-Output "Deleting Linked Service " $Name
    Remove-AzDataFactoryV2LinkedService -Name $Name -ResourceGroupName $ResourceGroupName -DataFactoryName $DataFactoryName -ErrorAction SilentlyContinue -Force
}

function deleteIntegrationRuntime {
    param(
        [string] $Name
    )
    Write-Output "Deleting integration runtime " $Name
    Remove-AzDataFactoryV2IntegrationRuntime -Name $Name -ResourceGroupName $ResourceGroupName -DataFactoryName $DataFactoryName -ErrorAction SilentlyContinue -Force
}

$templateJson = Get-Content $armTemplate | ConvertFrom-Json
$resources = $templateJson.resources

#Triggers
Write-Output "Getting triggers"
$triggersADF = Get-SortedTriggers -DataFactoryName $DataFactoryName -ResourceGroupName $ResourceGroupName
$triggersTemplate = $resources | Where-Object { $_.type -eq "Microsoft.DataFactory/factories/triggers" }
$triggerNames = $triggersTemplate | ForEach-Object {$_.Substring(37, $_.Length-40)}
$activeTriggerNames = $triggersTemplate | Where-Object { $_.properties.runtimeState -eq "Started" -and ($_.properties.pipelines.Count -gt 0 -or $_.properties.pipeline.pipelineReference -ne $null)} | ForEach-Object {$_.Substring(37, $_.Length-40)}
$triggerstostop = $triggerNames | Where-Object { ($triggersADF | -SelectObject name).name -contains $_ }

if ($predeployment -eq $true) {
    #Stop all triggers
    Write-Output "Stopping deployed triggers"
    $triggerstostop | ForEach-Object {
        Write-Output "Disabling trigger " $_
        Stop-AzDataFactoryV2Trigger -ResourceGroupName $ResourceGroupName -DataFactoryName $DataFactoryName -Name $_ -Force
    }
}
else {

    # objectsToBeDeleted will be as T-triggerName1,T-triggerName2,P-pipelineName,D-datasetName ...
    if($objectsToBeDeleted) {
        $objectsArray = $objectsToBeDeleted.Split(',')
        foreach ($object in $objectsArray) {
            $object = $object.Split('-')
            $objectType = $object[0]
            $objectName = $object[1]

            switch($objectType) {
                "T" {deleteTrigger -Name $objectName}
                "P" {deletePipeline -Name $objectName}
                "D" {deleteDataset -Name $objectName}
                "L" {deleteLinkedService -Name $objectName}
                "I" {deleteIntegrationRuntime -Name $objectName}
            }
        }
    }

    #Start active triggers - after cleanup
    Write-Output "Starting active triggers"
    $activeTriggerNames | ForEach-Object {
        Write-Output "Enabling trigger " $_
        Start-AzDataFactoryV2Trigger -ResourceGroupName $ResourceGroupName -DataFactoryName $DataFactoryName -Name $_ -Force
    }
}