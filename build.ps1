param($publish = $false, $config = "Release")

dotnet build -c $config

dotnet test --no-build -c $config "$PSScriptRoot\GitDiffReader.Tests\GitDiffReader.Tests"

if (Test-Path "$PSScriptRoot\output") {
   Remove-Item -Force -Recurse "$PSScriptRoot\output"
}