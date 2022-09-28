param(
    [Parameter(Mandatory=$true)]
    [string]
    $permissions,

    [Parameter(Mandatory=$true)]
    [string]
    $appId
)

Write-Output "Verifying the app permissons"
$scopeObject = ConvertFrom-Json $permissions
$errorHash = @{}

# Getting the app permissions for the UI App
$permissionList = az ad app permission list --id $appId | ConvertFrom-Json

# Iterating over the expected scopes defined in the configuration variable group
$scopeObject.scopes | ForEach-Object{
    Write-Output "Scope to be validated : $_"
    $scopeToBeValidated = $_
    $permissionList | ForEach-Object {
        $resourceId = $_.resourceAppId

        # Creating the list of scope permissions, obtained from the portal, for UI app
        $_.resourceAccess | ForEach-Object{
        $idList = $idList + "," + $_.id
        }

        $idList = $idList.TrimStart(",")
        $scope = az ad sp show --id $resourceId --only-show-errors | ConvertFrom-Json
        if ($scope.appDisplayName -eq $scopeToBeValidated.name)
        {
            Write-Output "Validating the scope : $($scopeToBeValidated.name)"
            $validScope = $scope.oauth2Permissions | Where-Object { $idList.Split(",") -contains $_.id }
            if(!$validScope -or $validScope.count -eq 0)
            {
                $validScope = $scope.appRoles | Where-Object { $idList.Split(",") -contains $_.id }
            }

            if(!$validScope -or $validScope.count -eq 0)
            {
                $errorHash.Add($scope.appDisplayName, "Could not validate the scope $($scopeToBeValidated.name)")
            }

            $validScope | ForEach-Object {
                if ($scopeToBeValidated.permissions.Split(",").Trim() -contains $_.value)
                    {
                        Write-Output "Validated $($scopeToBeValidated.name) for scope $($_.value)"
                    }
                    else{
                        $errorHash.Add($($_.value), "Could not validate the scope $($scopeToBeValidated.name). Please add the scope $($_.value) under $($scopeToBeValidated.name) for this app. Help link : https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis#add-permissions-to-access-web-apis")
                    }
            }
        }
        $idList = ""
}
}

if ($errorHash.count -gt 0)
{
    Write-Error ($errorHash | Out-String)
}