function Resolve-GproxytDotnet {
    return (Get-Command dotnet -CommandType Application -ErrorAction Stop | Select-Object -First 1).Source
}

Export-ModuleMember -Function Resolve-GproxytDotnet
