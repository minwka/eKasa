#region Pack application files and prepare Installer
$ErrorActionPreference = 'SilentlyContinue'
if ((Test-Path Deployment_Package\Executables) -eq $false) {
    Write-Host -Fore Blue "App files don't exist. Build solution first."
    Start-Sleep 3
    return
}
Write-Warning "Packing application files and setting up deployment package"
#endregion

#region Move in deployment directory and create folders
Set-Location .\Deployment_Package;
New-Item -ItemType Directory Install_Package\Binaries\7z
New-Item -ItemType Directory Install_Package\Binaries\Install
New-Item -ItemType Directory Install_Package\Binaries\Install\Temp
#endregion

#region Copy binaries and app files
Copy-Item Binaries\7z\* Install_Package\Binaries\7z
Copy-Item Executables\eKasa.exe Install_Package\Binaries\Install\Temp\eKasa.exe
Copy-Item Executables\AutofillHelper.exe Install_Package\Binaries\Install\Temp\AutofillHelper.exe
#endregion

#region Copy Installer files
Copy-Item Executables\Installer.exe Install_Package\Installer.exe
Copy-Item Executables\Uninstall.exe Install_Package\Uninstall.exe
#endregion

#region Pack app files
.\Binaries\7z\7za.exe a -t7z -m0=lzma2 -mx=9 -mfb=64 -md=32m -ms=on -mhe=on .\Install_Package\Binaries\Install\package.7z .\Install_Package\Binaries\Install\Temp\*
#endregion

# Clean directories
Remove-Item Install_Package\Binaries\Install\Temp -Recurse
#endregion

#region Wait for user
$ErrorActionPreference = 'Continue'
Write-Warning "Deployment completed. Press enter to quit."
Read-Host
#endregion