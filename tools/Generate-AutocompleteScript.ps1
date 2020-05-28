$config = "gxcli.config"

if (-not (Test-Path $config)){
    Write-Error Config file was not found
    return
}

$json = Get-Content $config | ConvertFrom-Json

$verbs = @() 
$def = "`t`tdefault {"
$json.Providers | Get-Member -MemberType NoteProperty | ForEach-Object {
    
    $key = $_.Name
    $verb = [PSCustomObject]@{Name = $key; Parameters = $json.Providers."$key".Parameters}

    $line = "`t`t'" + $verb.Name + "' {"
    $def +=  "`"" + $verb.Name + "`","

    $verb.Parameters | ForEach-Object {
        $p = $_.Name
        $param = [PSCustomObject]@{Name = $p }
        $line += "`"" + $param.Name + "`"" + ","
    }

    $line = $line.Substring(0, $line.Length -1)
    $line += ";break }`r`n"

    $verbs += $line
}

$def = $def.Substring(0,$def.Length - 1)
$def += "}"

$template = Get-Content gxcli-autocomplete.template -Raw

$template = $template.Replace("{{VERBS}}",$verbs)
$template = $template.Replace("{{DEFAULT}}",$def)

$outputPath = "gxcli-autocomplete.ps1"

Set-Content $outputPath $template

Invoke-Expression -Command $outputPath