# Run once after cloning: .\setup-hooks.ps1

Write-Host "Setting up git hooks..." -ForegroundColor Cyan

# Point git to our committed hooks folder
git config core.hooksPath .githooks

# Verify
$hooksPath = git config core.hooksPath
Write-Host "Git hooks path set to: $hooksPath" -ForegroundColor Green
Write-Host "Pre-push hook is now active." -ForegroundColor Green