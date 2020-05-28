
param(
    [string]$gxpath,
    [string]$conf
)

if ($gxpath) {
    $destination = $gxpath
}
else {
    if ($env:GXPath) {
        $destination = $env:GXPath
    }
    else {
        $destination = '.\bin\' 
    }
}

if (-not $destination) {
    Write-Error "Destination is empty, either set an environment variable called GXPath or pass the destination as a parameter"
    return
}

if (-not (Test-Path $destination)) {
    New-Item $destination -ItemType Directory
}

if (-not $conf) {
    $conf = "Debug"
}

Write-Host Copying to $destination from $conf

$gxclimodules = $destination + "\gxclimodules\"

Write-Host Copying Powershell support
Copy-Item -Path .\tools\*.* $destination

Write-Host Copying gxcli
Copy-Item -Path .\src\gxcli\bin\$conf\gx.* $destination

Write-Host Copying common
Copy-Item -Path .\src\common\bin\$conf\gxcli.common.* $destination

$required = "Newtonsoft.Json.dll"

foreach ($r in $required) {
    Write-Host Copying $r
    Copy-Item -Path .\src\gxcli\bin\$conf\$r $destination
}

if (-not (Test-Path $gxclimodules)) {
    Write-Host "Creating $gxclimodules folder"
    New-Item $gxclimodules -ItemType Directory
}

$modules = Get-ChildItem .\src\modules -Directory
foreach ($m in $modules) {
    Write-Host Copying $m module
    Copy-Item -Path .\src\modules\$m\bin\$conf\$m.* $gxclimodules
}
