#region Script settings
Write-Warning "Setting variables"
# Command line for building projects, requires Visual Studio Build Tools
$command="dotnet publish -c Release -f net5.0-windows -r win10-x86 --nologo --self-contained false -p:PublishSingleFile=true -o "
# 7-Zip command line
$key="enc765_-_-_inst05"
#endregion

#region Cleanup deployment directory
Write-Warning "Cleaning up deployment directory"
Remove-Item .\Deployment_Package\Executables -Recurse
Remove-Item .\Deployment_Package\Install_Package -Recurse
#endregion

#region Build projects seperately to generate single executables
Write-Warning "Building <PasswordManager.AutofillHelper>"
Set-Location PasswordManager.AutofillHelper
Invoke-Expression $command"..\Deployment_Package\Executables\"

Write-Warning "Building <PasswordManager.Core>"
Set-Location ..; Set-Location PasswordManager.Core          
Invoke-Expression $command"..\Deployment_Package\Executables\"

Write-Warning "Building <PasswordManager.Installer>"
Set-Location ..; Set-Location PasswordManager.Installer     
Invoke-Expression $command"..\Deployment_Package\Install_Package\"

Write-Warning "Building <PasswordManager.Uninstaller>"
Set-Location ..; Set-Location PasswordManager.Uninstaller
Invoke-Expression $command"..\Deployment_Package\Install_Package\"
Set-Location ..;
#endregion

#region Cleanup *.pdb files
Write-Warning "Cleaning up *.pdb files"
Remove-Item .\Deployment_Package\Executables\*.pdb -Recurse
Remove-Item .\Deployment_Package\Install_Package\*.pdb -Recurse
#endregion

#region Pack application files and prepare Installer
Write-Warning "Packing application files and setting up deployment package"
Set-Location .\Deployment_Package;
New-Item -ItemType Directory Install_Package\Binaries\7z
New-Item -ItemType Directory Install_Package\Binaries\Install
Copy-Item Binaries\7z\* Install_Package\Binaries\7z

.\Binaries\7z\7za.exe a -t7z -m0=lzma2 -mx=9 -mfb=64 -md=32m -ms=on -mhe=on -p"$key" .\Install_Package\Binaries\Install\package.7z .\Executables\*
Set-Location ..;
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
Write-Warning "Deployment completed. You can close this window."
Read-Host
#endregion