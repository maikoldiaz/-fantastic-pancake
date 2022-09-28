Describe 'Add Retention Policy' {
	Context 'File' {        
        It "adds the correct retention policy for logging for blob" {
            $storageAccount = Get-AzStorageAccount -ResourceGroupName $env:resourceGroupName -Name $env:storageAccountName -ErrorAction SilentlyContinue

            $storageContext = $storageAccount.Context
            $loggingProperty = Get-AzStorageServiceLoggingProperty -ServiceType Blob -Context $storageContext
            $loggingProperty.RetentionDays | Should -Be 365
        }

        It "adds the correct retention policy for logging for queue" {
            $storageAccount = Get-AzStorageAccount -ResourceGroupName $env:resourceGroupName -Name $env:storageAccountName -ErrorAction SilentlyContinue

            $storageContext = $storageAccount.Context
            $loggingProperty = Get-AzStorageServiceLoggingProperty -ServiceType Queue -Context $storageContext
            $loggingProperty.RetentionDays | Should -Be 365
        }

        It "adds the correct retention policy for logging for table" {
            $storageAccount = Get-AzStorageAccount -ResourceGroupName $env:resourceGroupName -Name $env:storageAccountName -ErrorAction SilentlyContinue

            $storageContext = $storageAccount.Context
            $loggingProperty = Get-AzStorageServiceLoggingProperty -ServiceType Table -Context $storageContext
            $loggingProperty.RetentionDays | Should -Be 365
        }

        It "adds the correct retention policy for metrics for blob" {
            $storageAccount = Get-AzStorageAccount -ResourceGroupName $env:resourceGroupName -Name $env:storageAccountName -ErrorAction SilentlyContinue

            $storageContext = $storageAccount.Context
            $loggingProperty = Get-AzStorageServiceMetricsProperty -MetricsType 'Hour' -ServiceType Blob -Context $storageContext
            $loggingProperty.RetentionDays | Should -Be 365
        }

        It "adds the correct retention policy for metrics for file" {
            $storageAccount = Get-AzStorageAccount -ResourceGroupName $env:resourceGroupName -Name $env:storageAccountName -ErrorAction SilentlyContinue

            $storageContext = $storageAccount.Context
            $loggingProperty = Get-AzStorageServiceMetricsProperty -MetricsType 'Hour' -ServiceType File -Context $storageContext
            $loggingProperty.RetentionDays | Should -Be 365
        }

        It "adds the correct retention policy for metrics for table" {
            $storageAccount = Get-AzStorageAccount -ResourceGroupName $env:resourceGroupName -Name $env:storageAccountName -ErrorAction SilentlyContinue

            $storageContext = $storageAccount.Context
            $loggingProperty = Get-AzStorageServiceMetricsProperty -MetricsType 'Hour' -ServiceType Table -Context $storageContext
            $loggingProperty.RetentionDays | Should -Be 365
        }

        It "adds the correct retention policy for metrics for queue" {
            $storageAccount = Get-AzStorageAccount -ResourceGroupName $env:resourceGroupName -Name $env:storageAccountName -ErrorAction SilentlyContinue

            $storageContext = $storageAccount.Context
            $loggingProperty = Get-AzStorageServiceMetricsProperty -MetricsType 'Hour' -ServiceType Queue -Context $storageContext
            $loggingProperty.RetentionDays | Should -Be 365
        }

        It "does not add incorrect retention policy for metrics for queue" {
            $storageAccount = Get-AzStorageAccount -ResourceGroupName $env:resourceGroupName -Name $env:storageAccountName -ErrorAction SilentlyContinue

            $storageContext = $storageAccount.Context
            $loggingProperty = Get-AzStorageServiceMetricsProperty -MetricsType 'Hour' -ServiceType Queue -Context $storageContext
            $loggingProperty.RetentionDays | Should -Not -Be 7
        }
  }
}