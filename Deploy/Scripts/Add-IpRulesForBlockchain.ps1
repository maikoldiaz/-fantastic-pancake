param(
    [Parameter(Mandatory = $true)]
    [string]
    $blockchainMemberName,

    [Parameter(Mandatory = $true)]
    [string]
    $resourceGroupName,

    [Parameter(Mandatory = $true)]
    [string]
    $aksNodeResourceGroup,

    [Parameter(Mandatory = $true)]
    [string]
    $aksLoadBalancerName,

    [Parameter(Mandatory = $true)]
    [string]
    $appServiceName,

    [Parameter(Mandatory = $false)]
    [string]
    $customIp,

    [Parameter(Mandatory = $false)]
    [string]
    $customName,

    [Parameter(Mandatory = $false)]
    [string]
    $openAll,

    [Parameter(Mandatory = $false)]
    [string]
    $skipLoadBalancer
)

Write-Output "Get App Ips"


$ipFirewallJson = ''
$ruleNameId = 0


if ($openAll -eq $true) {
    $ruleName = "rule"
    $ip = "0.0.0.0"
    $endip = "255.255.255.255"
    $ipFirewallJson = $ipFirewallJson + '{""ruleName"":""' + $ruleName + '"", ""startIpAddress"":""' + $ip + '"", ""endIpAddress"":""' + $endip + '""}'
    $firewalldata = "[" + $ipFirewallJson + "]"
}
else {

    $ips = az webapp show --resource-group $resourceGroupName --name $appServiceName --query possibleOutboundIpAddresses --output tsv

    $ips = $ips.Split(",")

    if($skipLoadBalancer -eq "false"){
    $aksIps = az network lb show -g $aksNodeResourceGroup -n $aksLoadBalancerName --query "frontendIpConfigurations[].publicIpAddress.ipAddress" --expand "frontendIpConfigurations/publicIpAddress" -otsv

    $ips = $ips + $aksIps
        }
    foreach ($ip in $ips) {
        $ruleName = "IPRule$ruleNameId"
        $ipFirewallJson = $ipFirewallJson + '{""ruleName"":""' + $ruleName + '"", ""startIpAddress"":""' + $ip + '"", ""endIpAddress"":""' + $ip + '""},'
        $ruleNameId++
    }

    if ($customIp) {
        $ruleName = $customName
        $ipFirewallJson = $ipFirewallJson + '{""ruleName"":""' + $ruleName + '"", ""startIpAddress"":""' + $customIp + '"", ""endIpAddress"":""' + $customIp + '""},'
    }

    $firewalldata = "[" + $ipFirewallJson.TrimEnd(",") + "]"
}
az resource update --resource-group $resourceGroupName --name $blockchainMemberName --resource-type "Microsoft.Blockchain/blockchainMembers" --set properties.firewallRules=$firewalldata --remove properties.consortiumManagementAccountAddress
