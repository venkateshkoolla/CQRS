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

New-Item -ItemType Directory -Force -Path $destinationPath
Copy-Item "$toolsPath\*" -Destination $destinationPath -Filter *.dll
Copy-Item "$toolsPath\*" -Destination $destinationPath -Filter *.exe
Copy-Item "$toolsPath\*" -Destination $destinationPath -Filter *.config


Write-Host "* SqlPackage files copied to $destinationPath"