Describe 'Key Vault' {
	Context 'Key' {
        It "should have the correct number of cryptographic permissions" {
            $keyVaultKeyDetails = Get-AzKeyVaultKey -Name dataprotection -VaultName $env:keyVault
            $keyVaultKeyDetails.key.keyOps | Should -HaveCount 5
        }

        It "should not have verify cryptographic permissions" {
            $keyVaultKeyDetails = Get-AzKeyVaultKey -Name dataprotection -VaultName $env:keyVault
            $keyVaultKeyDetails.key.keyOps | Should -Not -Contain verify
        }

        It "should have all the required cryptographic permission" {
            $requiredPermissions = @('decrypt', 'encrypt', 'sign', 'unwrapKey', 'wrapKey')
            $keyVaultKeyDetails = Get-AzKeyVaultKey -Name dataprotection -VaultName $env:keyVault
            $keyVaultKeyDetails.key.keyOps | Should -Be $requiredPermissions
        }
  }
}