param(
 [Parameter(Mandatory=$true)]
 [string]
 $publisher,

 [Parameter(Mandatory=$true)]
 [string]
 $product,

 [Parameter(Mandatory=$true)]
 [string]
 $name
 )

Get-AzMarketplaceTerms -Publisher $publisher -Product $product -Name $name |  Set-AzMarketplaceTerms -ErrorAction Continue -Accept