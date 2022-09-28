param(
    #VSTS data
	[Parameter(Mandatory=$true)]
    [string]$resourceGroupName,
	[Parameter(Mandatory=$true)]
    [string]$namespaceName,
	[Parameter(Mandatory=$true)]
    [string]$sessionQueues,
    [Parameter(Mandatory=$true)]
    [string]$normalQueues
 )

 function CreateQueues($queues, $isSessionEnabled, $resourceGroup, $namespace)
 {
    $queueArray = $queues -split ","
    foreach ($queue in $queueArray)
    {
        try
        {
             # Check if queue already exists
            $currentQ = Get-AzServiceBusQueue -ResourceGroup $resourceGroup -Namespace $namespace -Name "$queue" -ErrorAction SilentlyContinue
            if($currentQ) {
                Write-Output "The queue $queue already exists."
            } else {
                Write-Output "The $queue queue does not exist, creating the queue."

                New-AzServiceBusQueue -ResourceGroup $resourceGroup -Namespace $namespace -Name "$queue" -RequiresSession $isSessionEnabled
                Write-Output "The $queue queue in Resource Group $resourceGroup has been successfully created."
            }

            $currentQueue = Get-AzServiceBusQueue -ResourceGroupName $resourceGroupName -Namespace $namespaceName -Name $queue

            if($currentQueue){
                Write-Output "Setting the TTL and Expiration time of Queues."

                $currentQueue.DefaultMessageTimeToLive = "P14D"
                $currentQueue.LockDuration = "PT5M"
                Set-AzServiceBusQueue -ResourceGroupName $resourceGroupName -Namespace $namespaceName -Name $queue -QueueObj $currentQueue
            }
        }
        catch
        {
            Write-Output $_.Exception.Message
            Write-Output "The $queueName queue creation failed."
        }
    }
}

CreateQueues -queues $sessionQueues -isSessionEnabled $True -resourceGroup $resourceGroupName -namespace $namespaceName
CreateQueues -queues $normalQueues -isSessionEnabled $False -resourceGroup $resourceGroupName -namespace $namespaceName