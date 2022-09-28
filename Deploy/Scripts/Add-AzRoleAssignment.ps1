param(
	[Parameter(Mandatory=$false)]
    [string]$RoleDefinitionName,
	[Parameter(Mandatory=$false)]
    [string]$ObjectId,
	[Parameter(Mandatory=$false)]
    [string]$ResourceName,
	[Parameter(Mandatory=$false)]
    [string]$ResourceType=$null,
	[Parameter(Mandatory=$false)]
    [string]$ResourceGroupName
 )

 if($ResourceType)
 {

 $existing = Get-AzRoleAssignment -ObjectId $ObjectId -RoleDefinitionName $RoleDefinitionName -ResourceName $ResourceName -ResourceType $ResourceType -ResourceGroupName $ResourceGroupName
 #if ($existing) {
 #   Remove-AzRoleAssignment -ObjectId $ObjectId -RoleDefinitionName $RoleDefinitionName -ResourceName $ResourceName -ResourceType $ResourceType -ResourceGroupName $ResourceGroupName
 #   Start-Sleep -s 50
 #   $existing = Get-AzRoleAssignment -ObjectId $ObjectId -RoleDefinitionName $RoleDefinitionName -ResourceName $ResourceName -ResourceType $ResourceType -ResourceGroupName $ResourceGroupName
 #}

 if(!$existing) {
    New-AzRoleAssignment -ObjectId $ObjectId -RoleDefinitionName $RoleDefinitionName -ResourceName $ResourceName -ResourceType $ResourceType -ResourceGroupName $ResourceGroupName
	Start-Sleep -s 50
    $existing = Get-AzRoleAssignment -ObjectId $ObjectId -RoleDefinitionName $RoleDefinitionName -ResourceName $ResourceName -ResourceType $ResourceType -ResourceGroupName $ResourceGroupName
 }

if(!$existing){
	Write-Error "Role Assignment Failed for access level: $RoleDefinitionName on Resource - $ResourceName for ServicePrincipal - $ObjectId"
}

}else{
       $roleAssignment = Get-AzRoleAssignment -ObjectId $ObjectId -RoleDefinitionName $RoleDefinitionName -ResourceGroupName $ResourceGroupName
       if($null -eq $roleAssignment)
       {
         New-AzRoleAssignment -ObjectId $ObjectId -RoleDefinitionName $RoleDefinitionName -ResourceGroupName $ResourceGroupName
       }
}