
Describe 'Alert ARM Template' {
    BeforeAll {
        $path = "$templateRootPath\Alert\deploy.json"
        $template = Get-Content $path -Raw -ErrorAction SilentlyContinue | ConvertFrom-Json -ErrorAction SilentlyContinue
    }
	Context 'File' {
		It 'Exists' {
                Test-Path $path -Include '*.json' | Should -Be $true
		}

		It 'Is a valid JSON file' {
			$template | Should -Not -Be $null
	  }
  }
    Context 'Content' {
        It 'Create PID deployment' {
                $deployments = 'Microsoft.Resources/deployments'
                $types = $template.Resources.type
                $resources = $template.Resources
                $types[0] | Should -Be $deployments
                $resources[0].name | Should -Be "[parameters('acrId')]"
                $resources[0].apiVersion | Should -Be "2016-06-01"
        }  
    }
}