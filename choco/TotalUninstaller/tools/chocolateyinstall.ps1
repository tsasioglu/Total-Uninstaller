$ErrorActionPreference = 'Stop';

$packageName = 'Total-Uninstaller'
$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$url = 'https://github.com/tsasioglu/Total-Uninstaller/blob/master/TotalUninstaller/bin/TotalUninstaller.exe?raw=true'

$exePath = Join-Path $toolsDir 'TotalUninstaller.exe'
Get-ChocolateyWebFile $packageName $exePath $url
