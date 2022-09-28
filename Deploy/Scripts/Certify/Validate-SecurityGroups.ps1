function Get-Members([string]$groupId, [string]$groupName) {
    $memberOfPowerBIGroup = Get-AzADGroupMember -objectid $groupId | Where-Object { $_.objectType -eq "ServicePrincipal" }
    Write-Output "Got the member of the security group. $($memberOfPowerBIGroup.DisplayName)"
    if ($memberOfPowerBIGroup -and $memberOfPowerBIGroup.Id -eq $env:servicePrincipalId)
        {
            Write-Output "The service principal is correctly configured for $groupName"
        }
    else{
        $exceptionObject = [System.Exception]@{Source="Validate-SecurityGroups.ps1";HelpLink="https://docs.microsoft.com/en-us/azure/active-directory/fundamentals/active-directory-groups-members-azure-portal#to-add-group-members"}
        Write-Error -Exception $exceptionObject -Message "The service principal is not correctly configured for $groupName. Please ensure that the service principal is added as a member in the security group for Power BI."
    }
}
function Get-SecurityGroup([string]$groupId, [string]$groupName)
{
    $existing = Get-AzADGroup -ObjectId $groupId
    if (!$existing)
    {
        Write-Error "The group with object Id $groupId and name $groupName doesn't exist. please check if the group is correctly configured in the variable group."
    }

    if ($existing -and $existing.ObjectType -ne "Group")
    {
        Write-Error "The group with object Id $groupId and name $groupName isn't a security Group."
    }

    if ($existing -and ($groupName -eq "powerBIGroupId"))
    {
        Write-Output "Validating the powerBI security group member"
        Get-Members -groupId $groupId -groupName $groupName
    }

    Write-Output "The group with object Id $groupId and name $groupName exists."
}

Write-Output "Validating the security group roleGroupIdAdministrator"
Get-SecurityGroup -groupId $env:roleGroupIdAdministrator -groupName "roleGroupIdAdministrator"

Write-Output "Validating the security group roleGroupIdChain"
Get-SecurityGroup -groupId $env:roleGroupIdChain -groupName "roleGroupIdChain"

Write-Output "Validating the security group roleGroupIdApprover"
Get-SecurityGroup -groupId $env:roleGroupIdApprover -groupName "roleGroupIdApprover"

Write-Output "Validating the security group roleGroupIdProfessionalSegmentBalances"
Get-SecurityGroup -groupId $env:roleGroupIdProfessionalSegmentBalances -groupName "roleGroupIdProfessionalSegmentBalances"

Write-Output "Validating the security group roleGroupIdProgrammer"
Get-SecurityGroup -groupId $env:roleGroupIdProgrammer -groupName "roleGroupIdProgrammer"

Write-Output "Validating the security group roleGroupIdQuery"
Get-SecurityGroup -groupId $env:roleGroupIdQuery -groupName "roleGroupIdQuery"

Write-Output "Validating the security group powerBIGroupId"
Get-SecurityGroup -groupId $env:powerBIGroupId -groupName "powerBIGroupId"

Write-Output "Validating the security group sqlMsiGroupId"
Get-SecurityGroup -groupId $env:sqlMsiGroupId -groupName "sqlMsiGroupId"

Write-Output "Verified the security groups."




