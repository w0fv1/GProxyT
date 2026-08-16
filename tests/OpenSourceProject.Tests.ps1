BeforeAll {
    $projectRoot = Split-Path -Parent $PSScriptRoot
}

Describe "open source repository contract" {
    It "keeps the Nfirco publisher local-only" {
        $ignore = Get-Content -Raw -Path (Join-Path $projectRoot ".gitignore")
        $workspace = Get-Content -Raw -Path (Join-Path $projectRoot "GproxytWorkspace.psm1")

        Test-Path -LiteralPath (Join-Path $projectRoot "release.ps1") | Should -Be $false
        $ignore | Should -Match "(?m)^/release_nfirco\.ps1$"
        $workspace | Should -Not -Match "Nfirco"
        Test-Path -LiteralPath (Join-Path $projectRoot "ReleaseMetadata.psm1") | Should -Be $false
    }

    It "provides parallel English and Chinese documentation" {
        $english = Get-Content -Raw -Path (Join-Path $projectRoot "README.md")
        $chinese = Get-Content -Raw -Path (Join-Path $projectRoot "README.zh-CN.md")

        $english | Should -Match "README\.zh-CN\.md"
        $chinese | Should -Match "README\.md"
        $english | Should -Match "GitHub Releases"
        $chinese | Should -Match "GitHub Releases"
    }

    It "publishes version-matched Windows executables from release tags" {
        $workflow = Get-Content -Raw -Path (Join-Path $projectRoot ".github\workflows\release.yml")

        $workflow | Should -Match "(?m)^\s+tags:"
        $workflow | Should -Match "'v\*'"
        $workflow | Should -Match "'V\*'"
        $workflow | Should -Match "GITHUB_REF_NAME"
        $workflow | Should -Match "Gproxyt\.csproj"
        $workflow | Should -Match "build\.ps1"
        $workflow | Should -Match "gh release create"
        $workflow | Should -Match "SHA256SUMS\.txt"
        $workflow | Should -Match "SHA512SUMS\.txt"
    }

    It "builds the portable Windows x64 contract" {
        $buildScript = Get-Content -Raw -Path (Join-Path $projectRoot "build.ps1")

        $buildScript | Should -Match ([regex]::Escape("-r win-x64"))
        $buildScript | Should -Match ([regex]::Escape("--self-contained true"))
        $buildScript | Should -Match ([regex]::Escape("-p:PublishSingleFile=true"))
        $buildScript | Should -Match ([regex]::Escape("-p:IncludeAllContentForSelfExtract=true"))
    }

    It "builds every main branch and pull request change" {
        $workflow = Get-Content -Raw -Path (Join-Path $projectRoot ".github\workflows\build.yml")

        $workflow | Should -Match "(?m)^\s+pull_request:"
        $workflow | Should -Match "(?m)^\s+push:"
        $workflow | Should -Match "build\.ps1"
        $workflow | Should -Match "contents: read"
    }

    It "ships standard community health files" {
        foreach ($path in @(
            "LICENSE",
            "CONTRIBUTING.md",
            "SECURITY.md",
            "CODE_OF_CONDUCT.md",
            ".github\CODEOWNERS",
            ".github\ISSUE_TEMPLATE\bug_report.yml",
            ".github\ISSUE_TEMPLATE\feature_request.yml",
            ".github\pull_request_template.md",
            ".github\dependabot.yml"
        )) {
            Test-Path -LiteralPath (Join-Path $projectRoot $path) | Should -Be $true
        }
    }
}
