<#
    .SYNOPSIS
        Remove the APIM Auth server.
    .DESCRIPTION
        This script calls a Az command to delete the APIM auth server.
#>
param(
[Parameter(Mandatory=$true)]
[String]$resourceGroup,
[Parameter(Mandatory=$true)]
[String]$apimOauthServerNames,
[Parameter(Mandatory=$true)]
[String]$apimName
)

Write-Output "Getting Context"
$azcontext = New-AzApiManagementContext -ResourceGroupName $resourceGroup -ServiceName $apimName

$apimOauthServerNameList = $apimOauthServerNames.split(",").Trim()

foreach($apimOauthServerName in $apimOauthServerNameList){
    Write-Output "Getting Apim Oauth Server"
    $authServer = Get-AzApiManagementAuthorizationServer -Context $azcontext -ServerId $apimOauthServerName -ErrorAction SilentlyContinue

    if($authServer){
        Write-Output "Removing Authorization Server : $apimOauthServerName"
        Remove-AzApiManagementAuthorizationServer -Context $azcontext -ServerId $apimOauthServerName
        Write-Output "Removed Authorization Server : $apimOauthServerName"
    }
}