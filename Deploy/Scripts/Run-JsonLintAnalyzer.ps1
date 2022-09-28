##Parameters
param
(
    [Parameter(Mandatory = $true)]
    [string]$infraTemplatePath
)

#Modules
Write-Output "Installing Json lint analyzer"
npm install jsonlint-mod -g
Write-Output "Installed Json lint analyzer"

Write-Output "Beginning Json lint script execution"
$templateFiles = Get-ChildItem $infraTemplatePath | Where-Object {$_.Name -ne "PowerAutomate"}

    foreach($template in $templateFiles)
    {
        $jsonFiles = Get-ChildItem "$infraTemplatePath\$template"
        foreach ($file in $jsonFiles)
            {
                if($file.Name -match "deploy")
                    {
                        Write-Output "Validating file: $($template)\$($file.Name)"
                        jsonlint $infraTemplatePath\$template\$file
                    }
            }
    }

Write-Output "Json lint script execution ran successfully"
