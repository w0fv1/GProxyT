# Contributing to GProxyT

Thank you for helping improve GProxyT. Bug reports, focused feature proposals, documentation fixes, translations, and code contributions are welcome.

## Before you start

- Search existing issues before opening a new one.
- Use a security advisory instead of a public issue for vulnerabilities, as described in [SECURITY.md](SECURITY.md).
- Keep changes focused on GProxyT's role as a process-level proxy launcher for the Microsoft Store desktop app.

## Development setup

You need Windows 10 version 2004 or later, Windows 11, PowerShell 7, Git, Pester 5.7.1, and the .NET SDK selected by [`global.json`](global.json).

Clone the repository and run:

```powershell
./test.ps1
./build.ps1
```

`test.ps1` runs the .NET and PowerShell test suites. `build.ps1` runs those tests and then creates the self-contained Windows x64 executable at `dist/gproxyt.exe`.

## Pull requests

- Add or update tests before changing behavior.
- Keep the project buildable after each independent change.
- Update both [`README.md`](README.md) and [`README.zh-CN.md`](README.zh-CN.md) when user-facing documentation changes.
- Update every localization resource when adding or changing interface text.
- Do not commit credentials, private release infrastructure, build output, or local configuration.
- Explain the user-visible result and the validation performed in the pull request.

## Versions and releases

The version has one source of truth: `<Version>` in [`src/Gproxyt/Gproxyt.csproj`](src/Gproxyt/Gproxyt.csproj). Versions use `x.y.z-label`, such as `1.7.4-stable` or `1.8.0-beta.1`.

Maintainers publish a release by pushing a `v` or `V` tag whose value after the prefix exactly matches the project version. The tag must point to a commit on `main`.

## License

By contributing, you agree that your contribution is licensed under the [MIT License](LICENSE).
