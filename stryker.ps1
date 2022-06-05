# On linux, install powershell: dotnet tool install -g PowerShell
# and run the script with: pwsh ./stryker.ps1

# Install Stryker.NET globally: dotnet tool install -g dotnet-stryker
dotnet-stryker -p TrendFox.Blazor.Regions.csproj -tp $(Join-Path . test TrendFox.Blazor.Regions.Tests TrendFox.Blazor.Regions.Tests.csproj) -o
