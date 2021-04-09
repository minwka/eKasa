#region Cleanup deployment directory
Write-Warning "Cleaning up deployment directory"
Remove-Item .\Deployment_Package\Executables -Recurse
Remove-Item .\Deployment_Package\Install_Package -Recurse
#endregion

#region Post compilation cleanup
Write-Warning "Cleaning post-compilation artifacts"
Set-Location PasswordManager.AutofillHelper
Remove-Item bin -Recurse
Remove-Item obj -Recurse

Set-Location ..; Set-Location PasswordManager.Core          
Remove-Item bin -Recurse
Remove-Item obj -Recurse

Set-Location ..; Set-Location PasswordManager.Installer     
Remove-Item bin -Recurse
Remove-Item obj -Recurse

Set-Location ..; Set-Location PasswordManager.Uninstaller
Remove-Item bin -Recurse
Remove-Item obj -Recurse
Set-Location ..;
#endregion

#region Wait for user
Write-Warning "Cleanup completed. You can close this window."
Read-Host
#endregion