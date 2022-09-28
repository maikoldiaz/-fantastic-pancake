param(
    [Parameter(Mandatory=$true)]
    [string]$server,
    [Parameter(Mandatory=$true)]
    [string]$coveragePath,
	[Parameter(Mandatory=$true)]
    [string]$threshold,
    [string]$listOfNamespacesSkipped
)



function ClientCodeCoverage($coveragePath,$threshold)
{
    [xml]$cn = Get-Content "$coveragePath"

    $currentcount = ([int]$cn.coverage.project.metrics.coveredstatements/[int]$cn.coverage.project.metrics.statements)*100

    if($currentcount -ge $threshold)
    {

    	Write-Output "Threshold limit of $threshold Passed !! Current Value : $currentcount" -BackgroundColor Green
    }
    else{
    	Write-Output "Threshold limit of $threshold Failed !! Current Value : $currentcount" -BackgroundColor Red
      exit 1
    }
}

function ServerCodeCoverage($coveragePath,$threshold,$listOfNamespacesSkipped)
{
    $listOfNamespacesSkippedArray = $listOfNamespacesSkipped.split(",")
    $xMLDocument=New-Object System.XML.XMLDocument
    $xMLDocument.Load($coveragePath)
    $xmlData = $xMLDocument.SelectNodes("coverage/packages/package")

    ###Initial Values
    $count=0
    $lineRate =0

    foreach($a in $xmlData){

    if($a.name -in $listOfNamespacesSkippedArray)
    {
         Write-Output $a.name $a.'line-rate' '----------Not considered'
    }else{
         Write-Output $a.name + $a.'line-rate'
        $lineRate = $lineRate + ([decimal]$a.'line-rate'*100)
         $count++
    }
    }

    $currentCoverage = $lineRate/$count

    if($currentCoverage -lt $threshold){
        Write-Output "Error"
        Write-Output "Threshold set is $threshold."
        Write-Output "Current value is $currentCoverage."
        exit 1
    }else{
        Write-Output "Code Coverage threshold Successfully Passed."
        Write-Output "Threshold set is $threshold."
        Write-Output "Current value is $currentCoverage."
    }
}

if($server -eq "true")
{
    ServerCodeCoverage -CoveragePath $coveragePath -Threshold $threshold -listOfNamespacesSkipped $listOfNamespacesSkipped
}else{
    ClientCodeCoverage -CoveragePath $coveragePath -Threshold $threshold
}