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

Write-Host $verbs
Write-Host $def

$templatePath = $gxcliPath + "\gxcli-autocomplete.template"
$template = Get-Content $templatePath -Raw

$template = $template.Replace("{{VERBS}}",$verbs)
$template = $template.Replace("{{DEFAULT}}",$def)

$outputPath = $gxcliPath + "\gxcli-autocomplete.ps1"

Set-Content $outputPath $template

Invoke-Expression -Command $outputPath