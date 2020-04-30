
param(
    [string]$gxpath,
    [string]$conf
    )

if ($gxpath)
{
    $destination = $gxpath
}
else {
    if ($env:GXPath)
    {
        $destination = $env:GXPath
    }
}

if (-not $destination)
{
    Write-Error "Destination is empty, either set an environment variable called GXPath or pass the destination as a parameter"
    return
}

if (-not (Test-Path $destination))
{
    Write-Error "Invalid path $destination"
    return
}

if (-not (Test-Path $destination\genexus.exe))
{
    Write-Error "$destination does not look like a valid GeneXus installation"
    return
}

if (-not $conf)
{
    $conf = "Debug"
}

$gxclimodules = $destination+ "\gxclimodules\"

Write-Host Copying gxcli
Copy-Item -Path .\src\gxcli\bin\$conf\gx.* $destination

$required = "Microsoft.Build.Framework.dll", "Microsoft.Build.dll", "System.Threading.Tasks.Dataflow.dll", "System.Collections.Immutable.dll"

foreach($r in $required)
{
    Write-Host Copying $r
    Copy-Item -Path .\src\gxcli\bin\$conf\$r $destination
}

Write-Host Copying common
Copy-Item -Path .\src\common\bin\$conf\gxcli.common.* $destination

if (-not (Test-Path $gxclimodules))
{
    Write-Host "Creating $gxclimodules folder"
    New-Item $gxclimodules -ItemType Directory
}

$modules = Get-ChildItem .\src\modules -Directory

foreach($m in $modules)
{
    Write-Host Copying $m module
    Copy-Item -Path .\src\modules\$m\bin\$conf\$m.* $gxclimodules
}
