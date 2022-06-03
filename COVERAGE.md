# Coverage script
## Prerequisites
The script requires [Daniel Palme's report generator](https://github.com/danielpalme/ReportGenerator).

Install the tool with the following command:
```
dotnet tool install -g dotnet-reportgenerator-globaltool
```
## Script artifacts & source control
The script generates two types of folders:
- TestResults
- TestCoverageReport

Ignore these artifacts in your source control.
### Git Ignore example
```
# coverage.ps1 generated artifacts
TestResults/
TestCoverageReport/
```
## Usage
Run the script in a PowerShell console:
```
.\coverage.ps1
```
The script will delete existing test results and
coverage reports, generate new ones, and open the
new report in the browser.