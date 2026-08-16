function Read-GproxytProjectVersion {
    param([Parameter(Mandatory)] [string] $ProjectPath)

    [xml] $project = Get-Content -Raw -Path $ProjectPath
    $version = [string]$project.Project.PropertyGroup.Version | Select-Object -First 1
    if ($version -notmatch '^\d+\.\d+\.\d+-[0-9A-Za-z]+(?:[.-][0-9A-Za-z]+)*$') {
        throw "Gproxyt.csproj must declare a semantic Version"
    }
    return $version
}

Export-ModuleMember -Function Read-GproxytProjectVersion
