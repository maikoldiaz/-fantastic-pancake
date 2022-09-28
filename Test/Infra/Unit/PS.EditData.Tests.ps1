
Describe 'Edit Data' {
    BeforeAll{
        Import-Module -Name "$scriptRootPath\Modules\EditData"
    }

	Context 'Swaps' {
		It 'Source with Destination' {
                $replacedString = Edit-Data -InputString "Hello World" -Source "World" -Destination "Universe"
                $replacedString | Should -Be 'Hello Universe'
		}
  }
}