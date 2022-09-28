<#
 .Synopsis
  Checks if provided string is in the array/list. Returns boolean.

 .Parameter Arr
  The array or list.

 .Parameter Str
  The string to search in the array or list.

 .Example
   Find-IfStringInArray -arr $arr -str $str
#>

function Find-IfStringInArray($arr, $str) {
	if ($str -in $arr) {
		return $true;
	}
	return $false;
}

Export-ModuleMember -Function Find-IfStringInArray