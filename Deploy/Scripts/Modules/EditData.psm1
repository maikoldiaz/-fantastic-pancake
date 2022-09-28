<#
 .Synopsis
  Replace characters within a string

 .Parameter InputString
  The input string

  .Parameter Source
  The characters to replace

  .Parameter Destination
  The characters to replace with

 .Example
   Edit-Data -InputString "Hello World" -Source "World" -Destination "Carlos"
#>

function Edit-Data($InputString, $Source, $Destination) {
    $inputstring = $inputstring -replace $source , $destination
    return $inputstring
}

Export-ModuleMember -Function Edit-Data