param(
    [Parameter(Mandatory=$true)]
    [string]$WebAppName,
        [Parameter(Mandatory=$true)]
    [string]$ResourceGroupName,
        [Parameter(Mandatory=$true)]
    [string]$DnsName,
        [Parameter(Mandatory=$true)]
    [string]$CertificateFilePath,
        [Parameter(Mandatory=$true)]
    [string]$CertificatePassword
)
try {
    Write-Output "Trying to upload certificate and create ssl binding"
    New-AzWebAppSSLBinding -WebAppName $WebAppName -ResourceGroupName $ResourceGroupName -Name $DnsName -CertificatePassword $CertificatePassword -CertificateFilePath $CertificateFilePath -SslState SniEnabled
}
catch {
    Write-Output "Certificate already uploaded, creating ssl binding"
    $password = ConvertTo-SecureString $CertificatePassword -AsPlainText -Force
    $Tumbprint = (Get-PfxData -Password $password -FilePath $CertificateFilePath).EndEntityCertificates.Thumbprint
    New-AzWebAppSSLBinding -WebAppName $WebAppName -ResourceGroupName $ResourceGroupName -Name $DnsName  -Thumbprint $Tumbprint
}