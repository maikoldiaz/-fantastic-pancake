#Parameters
param
(
	[Parameter(Mandatory = $true)]
	[string]$groupObjectId,

	[Parameter(Mandatory = $true)]
	[string]$memberObjectId
)

$existing = Get-AzADGroup -ObjectId $groupObjectId | Get-AzADGroupMember

function Add-GroupMember {
	param (
		[Parameter(Mandatory = $true)]
		[string]$groupObjectId,
		[Parameter(Mandatory = $true)]
		[string]$memberObjectId
	)
	try {
		Get-AzADGroup -ObjectId $groupObjectId | Add-AzADGroupMember -MemberObjectId $memberObjectId
	}
	catch {
		Write-Warning "Error adding member $($memberObjectId) member might already exist. Continuing without adding the user."
	}	
}

if ($existing) {
	Write-Output "Group $($groupObjectId) already exists"
	if (!$existing.id.contains($memberObjectId)) {
		Write-Output "The $memberObjectId object id is already part of security group $groupObjectId"
	}
	else {
		Write-Output "Adding member $($memberObjectId)"
		Add-GroupMember -GroupObjectId $groupObjectId -MemberObjectId $memberObjectId
	}
}
else {
	Write-Output "Adding member $($memberObjectId)"
	Add-GroupMember -GroupObjectId $groupObjectId -MemberObjectId $memberObjectId
}