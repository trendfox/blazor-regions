# This script requires Daniel Palme's report generator.
# See: https://github.com/danielpalme/ReportGenerator
# Install: dotnet tool install -g dotnet-reportgenerator-globaltool

Write-Host "Fetching previous test result..."
$resultFolders = @(Get-ChildItem -Recurse -Directory TestResults | ForEach-Object { $_.FullName })

Write-Host "Deleting previous test results..."
$resultFolders | ForEach-Object { Remove-Item -Recurse $_ }

Write-Host "Deleting previous report..."
if ((Test-Path .\TestCoverageReport) -eq $true) {
    Remove-Item -Recurse .\TestCoverageReport
}

Write-Host "Run tests and coverage..."
dotnet test --collect "XPlat Code Coverage"

$resultFolders = @(Get-ChildItem -Recurse -Directory TestResults | ForEach-Object { $_.FullName })
$reportFiles = @(Get-ChildItem -Recurse -Directory TestResults | ForEach-Object { Get-ChildItem -Recurse -Path $_.FullName cov*.xml } | ForEach-Object { $_.FullName })

# Requires global dotnet tool dotnet-reportgenerator-globaltool
# Scans for all cov*.xml files
Write-Host "Generating report..."
reportgenerator "-reports:$($reportFiles -join ";")" -targetdir:TestCoverageReport -reporttypes:Html

Start-Process .\TestCoverageReport\index.html
