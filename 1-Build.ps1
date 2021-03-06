#region Initialize script
$ErrorActionPreference = 'SilentlyContinue'
Write-Warning "Initializing script."
# Purge solution directory
.\0-Purge.ps1
# Command line for building projects, requires Visual Studio Build Tools
$compileExe = "dotnet publish -c Release -f net5.0-windows -r win10-x86 --nologo --self-contained false -p:PublishSingleFile=true -o "
$compileLib = "dotnet publish -c Release -f net5.0-windows -r win10-x86 --nologo --self-contained false -o "
# Get project list (can get from solution)
$projects = Get-ChildItem -Directory -Name | Select-String eKasa
#endregion

#region Build projects seperately to generate single executables
foreach ($project in $projects) {
    if ($project -clike "eKasa.Library") {
        Write-Host -Fore Blue -Back Black "Building X <$project>"
        Set-Location $project
        Invoke-Expression $compileLib"..\Deployment_Package\Executables\"
        Set-Location ..;
    }
    else {
        Write-Host -Fore Blue -Back Black "Building <$project>"
        Set-Location $project
        Invoke-Expression $compileExe"..\Deployment_Package\Executables\"
        Set-Location ..;
    }
}
#endregion

#region Run post compilation clean up script
.\2-Clean.ps1
#endregion

#region Wait for user
$ErrorActionPreference = 'Continue'
Write-Warning "Build completed. Type <deploy> to start deployment script."
$deploy = Read-Host
if ($deploy -eq "deploy") {
    .\3-Deploy.ps1
}
#endregion