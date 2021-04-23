#region Initialize script
# Find project files in the directory
Write-Host -Fore Yellow "Searching for projects..."
$projects = Get-ChildItem -Filter *.csproj -File -Name -Recurse

Write-Host -Fore Green "Found projects:"
foreach ($project in $projects) {
    Write-Host -Fore Blue "`t"$project
}
#endregion

#region Ask for a new version number
Write-Host -Fore Yellow "`nEnter new version number (X.X.X.X): " -NoNewline
[string]$newVersion = Read-Host
#endregion

#region Update version info of the project files and overwrite
foreach ($project in $projects) {
    Write-Host -Fore Blue "Versioning <$project>:"

    $content = Get-Content $project -Encoding Default
    $content | Set-Content -Encoding Unicode $project
    [xml]$xml = $content

    $oldVersion = $xml.Project.PropertyGroup.Version
    $xml.Project.PropertyGroup[0].Version = $newVersion
    $xml.Save($project)

    Write-Host -Fore Red $oldVersion -NoNewline
    Write-Host -Fore Blue "--> " -NoNewline
    Write-Host -Fore Green $newVersion
}
#endregion

#region Wait for user
Write-Host -Fore Yellow "`nDone versioning. Press ENTER to leave."
Read-Host
#endregion