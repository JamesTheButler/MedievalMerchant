param(
    [Parameter(Mandatory=$false, Position=0)]
    [string]$Path
)

if (-not $Path) {
    # Count all CSVs in the script's directory
    $dir = Split-Path -Parent $MyInvocation.MyCommand.Path
    $files = Get-ChildItem -Path $dir -Filter "*.csv"
    $totalKeys = 0
    $totalWords = 0

    foreach ($f in $files) {
        $lines = Get-Content $f.FullName | Select-Object -Skip 1 | Where-Object { $_.Trim() -ne "" }
        $keys = $lines.Count
        $words = 0
        foreach ($line in $lines) {
            # Parse CSV: split on first two commas to get the English value (3rd column onward)
            if ($line -match '^[^,]*,[^,]*,(.+)$') {
                $value = $Matches[1].Trim('"')
                $words += ($value -split '\s+' | Where-Object { $_ -ne "" }).Count
            }
        }
        Write-Host ("{0,-25} {1,4} keys, {2,5} words" -f $f.Name, $keys, $words)
        $totalKeys += $keys
        $totalWords += $words
    }
    Write-Host ("=" * 42)
    Write-Host ("{0,-25} {1,4} keys, {2,5} words" -f "TOTAL", $totalKeys, $totalWords)
} else {
    # Count a single file
    $lines = Get-Content $Path | Select-Object -Skip 1 | Where-Object { $_.Trim() -ne "" }
    $keys = $lines.Count
    $words = 0
    foreach ($line in $lines) {
        if ($line -match '^[^,]*,[^,]*,(.+)$') {
            $value = $Matches[1].Trim('"')
            $words += ($value -split '\s+' | Where-Object { $_ -ne "" }).Count
        }
    }
    Write-Host "$keys keys, $words words"
}
