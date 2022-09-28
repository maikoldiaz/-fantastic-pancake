
$ipAddress = Invoke-RestMethod http://ipinfo.io/json | Select-Object -exp ip

##Set Output Variable.
$key="agentIp"
Write-Output "##vso[task.setvariable variable=$key;]$ipAddress"