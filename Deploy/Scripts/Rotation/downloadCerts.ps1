param(
 [Parameter(Mandatory=$true)]
 [string]
 $keyVaultName,

 [Parameter(Mandatory=$true)]
 [string]
 $certName,

 [Parameter(Mandatory=$true)]
 [string]
 $password,

 [Parameter(Mandatory=$true)]
 [string]
 $pfxFilePath,

 [Parameter(Mandatory=$true)]
 [string]
 $certRename
)

$kvCert = Get-AzKeyVaultSecret -VaultName $keyVaultName -Name $certName  

$ssPtr = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($kvCert.SecretValue)

    try {
        $secretValueText = [System.Runtime.InteropServices.Marshal]::PtrToStringBSTR($ssPtr)
    } finally {
        [System.Runtime.InteropServices.Marshal]::ZeroFreeBSTR($ssPtr)
    }

$kvCertBytes = [System.Convert]::FromBase64String($secretValueText)  
$certCollection = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2Collection  
$certCollection.Import($kvCertBytes, $null, [System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::Exportable)
$protectedCertificateBytes = $certCollection.Export([System.Security.Cryptography.X509Certificates.X509ContentType]::Pkcs12, $password)  
$FilePath =  $pfxFilePath + "$certRename.pfx" 
[System.IO.File]::WriteAllBytes($FilePath , $protectedCertificateBytes)