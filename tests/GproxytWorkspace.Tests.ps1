BeforeAll {
    $projectRoot = Split-Path -Parent $PSScriptRoot
    $modulePath = Join-Path $projectRoot "GproxytWorkspace.psm1"
    Import-Module $modulePath -Force
}

Describe "GProxyT workspace" {
    It "resolves the installed dotnet executable" {
        $dotnet = Resolve-GproxytDotnet

        Test-Path -LiteralPath $dotnet -PathType Leaf | Should -Be $true
        [IO.Path]::GetFileName($dotnet) | Should -Be "dotnet.exe"
    }
}
