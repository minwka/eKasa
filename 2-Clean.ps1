#region Initialize script
$ErrorActionPreference = 'SilentlyContinue'
Write-Warning "Cleaning solution..."
# Get project list (can get from solution)
$projects = Get-ChildItem -Directory -Name | Select-String eKasa
#endregion

#region Clear deployment directory
Write-Warning "Cleaning up *.pdb files"
Remove-Item .\Deployment_Package\Executables\*.pdb -Recurse
#endregion

#region Clear post compilation artifacts
Write-Warning "Cleaning up post-compilation artifacts"
foreach ($project in $projects) {
    Write-Host -Fore Blue -Back Black "Cleaning up <$project>"
    Set-Location $project
    Remove-Item bin -Recurse
    Remove-Item obj -Recurse
    Set-Location ..;
}
#endregion

#region Wait for user
$ErrorActionPreference = 'Continue'
Write-Warning "Clean up completed. Moving on in 2 seconds."
Start-Sleep 2
#endregion