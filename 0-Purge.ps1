#region Initialize script
$ErrorActionPreference = 'SilentlyContinue'
Write-Warning "Purging solution..."
# Get project list (can get from solution)
$projects = Get-ChildItem -Directory -Name | Select-String eKasa
#endregion

#region Purge deployment directory (Solution-specific)
Write-Warning "Purging deployment directory"
Remove-Item .\Deployment_Package\Executables -Recurse
Remove-Item .\Deployment_Package\Install_Package -Recurse
#endregion

#region Purge compilation artifacts
Write-Warning "Purging compilation artifacts"
foreach ($project in $projects) {
    Write-Host -Fore Blue -Back Black "Purging <$project>"
    Set-Location $project
    Remove-Item bin -Recurse
    Remove-Item obj -Recurse
    Set-Location ..;
}
#endregion

#region Wait for user
$ErrorActionPreference = 'Continue'
Write-Warning "Purge completed. Moving on in 2 seconds."
Start-Sleep 2
#endregion