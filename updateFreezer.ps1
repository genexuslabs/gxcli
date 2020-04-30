
param(
    [string]$gxpath
    )

if ($gxpath)
{
    $source = $gxpath
}
else {
    if ($env:GXPath)
    {
        $source = $env:GXPath
    }
}

if (-not $source)
{
    Write-Error "Destination is empty, either set an environment variable called GXPath or pass the destination as a parameter"
    return
}

if (-not (Test-Path $source))
{
    Write-Error "Invalid path $source"
    return
}

if (-not (Test-Path $source\genexus.exe))
{
    Write-Error "$source does not look like a valid GeneXus installation"
    return
}

Copy-Item $source\Genexus.MsBuild.Tasks.dll .\freezer
