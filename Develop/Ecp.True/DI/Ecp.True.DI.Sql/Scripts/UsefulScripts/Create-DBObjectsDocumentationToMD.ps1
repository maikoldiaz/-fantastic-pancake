param
(
 [Parameter(Mandatory=$true)]
 [string]
 $rootPath,

 [Parameter(Mandatory=$true)]
 [string]
 $outputPath
)

$pathArray = New-Object -TypeName "System.Collections.ArrayList"
$firstTag = "<Description>:"
$secondTag = "</Description>"

# Regex pattern for string comparision
$pattern = "$firstTag(.*?)$secondTag"



# File paths for Objects
$adminTables = "$($rootPath)\Ecp.True.DI.Sql\Admin\Tables\"
$adminViews = "$($rootPath)\Ecp.True.DI.Sql\Admin\Views\"
$adminSPs ="$($rootPath)\Ecp.True.DI.Sql\Admin\Stored Procedures\"
$adminUDFs = "$($rootPath)\Ecp.True.DI.Sql\Admin\Functions\"
$adminUDTs = "$($rootPath)\Ecp.True.DI.Sql\Admin\User Defined Types\"
$adminTriggers = "$($rootPath)\Ecp.True.DI.Sql\Admin\Triggers\"
$analyticsTables = "$($rootPath)\Ecp.True.DI.Sql\Analytics\Tables\"
$analyticsSPs = "$($rootPath)\Ecp.True.DI.Sql\Analytics\Stored Procedures\"
$auditTables = "$($rootPath)\Ecp.True.DI.Sql\Audit\Tables\"
$auditSPs = "$($rootPath)\Ecp.True.DI.Sql\Audit\Stored Procedures\"
$offchainTables = "$($rootPath)\Ecp.True.DI.Sql\Offchain\Tables\"
$offchainTriggers = "$($rootPath)\Ecp.True.DI.Sql\Offchain\Triggers\"



# Adding File Paths to Path Array
$pathArray.Add($adminTables)
$pathArray.Add($adminViews)
$pathArray.Add($adminSPs)
$pathArray.Add($adminUDFs)
$pathArray.Add($adminUDTs)
$pathArray.Add($adminTriggers)
$pathArray.Add($analyticsTables)
$pathArray.Add($analyticsSPs)
$pathArray.Add($auditTables)
$pathArray.Add($auditSPs)
$pathArray.Add($offchainTables)
$pathArray.Add($offchainTriggers)


foreach($p in $pathArray)
{
    
$objName = Split-Path $p -Leaf

$allFiles = Get-ChildItem -Recurse $p -Filter *.sql 

$schemaPath = [System.IO.Path]::GetDirectoryName([System.IO.Path]::GetDirectoryName($p))

$schemaName = Split-Path $schemaPath -Leaf

"#$($schemaName) $($objName)`n`n" | Out-File "$($outputPath)\$($schemaName)_$($objName).md" -Encoding utf8

Write-Host $($schemaName)_$($objName)

$num = 0

foreach($f in $allFiles)
{

$num += 1

$content = get-content $f.FullName

# Matching operation
$result = [regex]::Match($content,$pattern).Groups[1].Value


# Removing .sql from the end of File Names
$fileName = $f.Name -replace '.sql',''


"# $num. $($fileName)`n","##Description: `n","###$($result.Trim())`n`n" | Out-File -Append "$($outputPath)\$($schemaName)_$($objName).md" -Encoding utf8

}

}