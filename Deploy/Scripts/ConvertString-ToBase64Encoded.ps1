param(
	[string] [Parameter(Mandatory=$true)]
	$StringValue,

    [string] [Parameter(Mandatory=$true)]
	$VariableName
)

$byteString = [System.Text.Encoding]::UTF8.GetBytes($StringValue)

$base64 = [System.Convert]::ToBase64String($byteString)

Write-Output "##vso[task.setvariable variable=$VariableName;]$base64"