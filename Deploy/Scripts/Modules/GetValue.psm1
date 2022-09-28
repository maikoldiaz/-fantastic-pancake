<#
 .Synopsis
  Extracts specified key's value from the string

 .Parameter InputString
  The string

 .Parameter Key
  The key whose value needs to be extracted from a string

 .Example
   Get-Value -InputString $sqlconnectionstring -Key "User ID"
#>

function Get-Value([string]$InputString, [string]$Key) {
    $returnValue = $null
    $splitValue = $InputString.Split(';')
    $arrayValue = $splitValue | ForEach-Object { if ($_.ToString().StartsWith($Key)) { return $_ } }

    if ($null -ne $arrayValue) {
        $index = $arrayValue.IndexOf('=') + 1
        if ($index -gt -1) {
            $returnValue = $arrayValue.Substring($index, $arrayValue.Length - $index)
        }
    }
    return $returnValue
}

Export-ModuleMember -Function Get-Value