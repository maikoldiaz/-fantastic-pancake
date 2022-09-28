param(
	[string] [Parameter(Mandatory=$true)]
	$CertificateFilePath,

    [string] [Parameter()]
	$VariableName = "Base64EncodedValue"
)

$pfx_cert = get-content $CertificateFilePath -Encoding Byte

$base64 = [System.Convert]::ToBase64String($pfx_cert)

Write-Output "##vso[task.setvariable variable=$VariableName;]$base64"