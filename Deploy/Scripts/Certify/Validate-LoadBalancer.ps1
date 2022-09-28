param(
    [Parameter(Mandatory=$true)]
    [string]
    $resourceGroupName
)

Write-Output "Starting certification..."

$loadBalancers = az network lb list -g $resourceGroupName --query "[?name=='kubernetes']" | ConvertFrom-Json

if (!$loadBalancers -or $loadBalancers.count -eq 0)
    {
        $exceptionObject = [System.Exception]@{Source="Validate-LoadBalancer.ps1";HelpLink="https://docs.microsoft.com/en-us/azure/aks/egress"}
        Write-Error -Exception $exceptionObject -Message "Validation failed for the load balancer! Load balancer named kubernetes does not exist."
    }
    else {
        Write-Output "Validated Load balancer successfully!"
    }