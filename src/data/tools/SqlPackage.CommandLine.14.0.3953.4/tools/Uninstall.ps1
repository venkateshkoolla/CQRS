param($installPath, $toolsPath, $package, $project)

# copy dlls and config from $toolsPath to tools folder at solution level

# get the destination path, this is one up from packages and then down into tools

if ($toolsPath) {
    #using value from nuget
}
else {
    $toolsPath = "c:\users\david.desloovere\documents\visual studio 2013\Projects\WebApplication7\packages\SqlPackage.CommandLine.11.0.1\tools"
}

#Write-Host "install.ps1 here"
#Write-Host "$toolsPath"

$destinationPath = $toolsPath.SubString(0, $toolsPath.LastIndexOf("\packages\"));
$destinationPath = $destinationPath + "\tools\SqlPackage.CommandLine\";
#Write-Host "$destinationPath"

Remove-Item $destinationPath -Force -Recurse

Write-Host "* SqlPackage files remove at $destinationPath"