param(
    [Parameter(Mandatory=$true)]
    [string]$gxcliPath

)

$config = $gxcliPath + "\gxcli.config"

if (-not (Test-Path $config)){
    Write-Host "Config file was not found under $gxcliPath"
    return
}

$json = Get-Content $config | ConvertFrom-Json

$verbs = @() 
$def = "default {"
$json.Providers | Get-Member -MemberType NoteProperty | ForEach-Object {
    
    $key = $_.Name
    $verb = [PSCustomObject]@{Name = $key; Parameters = $json.Providers."$key".Parameters}

    $line = "'" + $verb.Name + "' {"
    $def +=  $verb.Name + ","
    #Write-Host $line

    $verb.Parameters | ForEach-Object {
        $p = $_.Name
        $param = [PSCustomObject]@{Name = $p }
        $line += "`"" + $param.Name + "`"" + ","
        #Write-Host $param.Name
    }

    $line = $line.Substring(0, $line.Length -1)
    $line += ";break }"

    Write-Host $line
    $verbs.Add($line)
}

$def = $def.Substring(0,$def.Length - 1)
