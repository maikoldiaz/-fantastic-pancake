Describe 'Send Grid' {
	Context 'Terms' {
        
        It "should have all the terms accepted" {
            $terms = Get-AzMarketplaceTerms -Publisher "Sendgrid" -Product "sendgrid_azure" -Name "free"

            $terms.Accepted | Should -Be True
        }
  }
}