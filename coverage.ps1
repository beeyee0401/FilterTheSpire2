# coverage.ps1

$root = $PSScriptRoot
$resultsDirectory = Join-Path $root "TestResults"
$coverageFile = Join-Path $resultsDirectory "coverage.cobertura.xml"
$reportDirectory = Join-Path $resultsDirectory "CoverageReport"
$reportIndex = Join-Path $reportDirectory "index.html"

dotnet test `
    --coverage `
    --results-directory "$resultsDirectory" `
    --coverage-output "coverage.cobertura.xml" `
    --coverage-output-format cobertura

if ($LASTEXITCODE -ne 0) {
    Write-Error "Tests or coverage collection failed."
    exit $LASTEXITCODE
}

reportgenerator `
    "-reports:$coverageFile" `
    "-targetdir:$reportDirectory" `
    "-reporttypes:Html"

if ($LASTEXITCODE -ne 0) {
    Write-Error "Coverage report generation failed."
    exit $LASTEXITCODE
}

Start-Process $reportIndex