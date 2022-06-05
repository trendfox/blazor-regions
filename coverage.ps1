# On linux, install powershell: dotnet tool install -g PowerShell
# and run the script with: pwsh ./coverage.ps1

# This script requires Daniel Palme's report generator.
# See: https://github.com/danielpalme/ReportGenerator
# Install: dotnet tool install -g dotnet-reportgenerator-globaltool

Write-Host "Fetching previous test result..."
$resultFolders = @(Get-ChildItem -Recurse -Directory TestResults | ForEach-Object { $_.FullName | Resolve-Path -Relative })

Write-Host "Deleting previous test results..."
$resultFolders | ForEach-Object {
    Write-Host " -> Delete $_"
    Remove-Item -Recurse $_
}

$reportDirectory = Join-Path . TestCoverageReport

Write-Host "Deleting previous report..."
if ((Test-Path $reportDirectory) -eq $true) {
    Write-Host " -> Delete $reportDirectory"
    Remove-Item -Recurse $reportDirectory
}

Write-Host "Run tests and coverage..."
dotnet test --collect "XPlat Code Coverage"

$resultFolders = @(Get-ChildItem -Recurse -Directory TestResults | ForEach-Object { $_.FullName })
$reportFiles = @(Get-ChildItem -Recurse -Directory TestResults | ForEach-Object { Get-ChildItem -Recurse -Path $_.FullName cov*.xml } | ForEach-Object { $_.FullName })

# Requires global dotnet tool dotnet-reportgenerator-globaltool
# Scans for all cov*.xml files
Write-Host "Generating report..."
reportgenerator "-reports:$($reportFiles -join ";")" -targetdir:TestCoverageReport -reporttypes:Html

Invoke-Item $(Join-Path $reportDirectory index.html)
