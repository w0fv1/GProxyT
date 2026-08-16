BeforeAll {
    $projectRoot = Split-Path -Parent $PSScriptRoot
    $modulePath = Join-Path $projectRoot "ProjectVersion.psm1"
    Import-Module $modulePath -Force
}

Describe "GProxyT project version" {
    It "reads the version from the project" {
        $projectPath = Join-Path $TestDrive "Gproxyt.csproj"
        Set-Content -Path $projectPath -Value '<Project><PropertyGroup><Version>1.0.1-stable</Version></PropertyGroup></Project>'

        Read-GproxytProjectVersion -ProjectPath $projectPath | Should -Be "1.0.1-stable"
    }

    It "rejects versions without a release label" {
        $projectPath = Join-Path $TestDrive "Gproxyt.csproj"
        Set-Content -Path $projectPath -Value '<Project><PropertyGroup><Version>1.0.1</Version></PropertyGroup></Project>'

        { Read-GproxytProjectVersion -ProjectPath $projectPath } | Should -Throw "*semantic Version*"
    }
}
