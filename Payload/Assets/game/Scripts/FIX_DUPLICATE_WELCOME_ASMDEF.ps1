# Run this PowerShell script from the ROOT of your Unity project.
# It removes the duplicate Welcome assembly shown by Unity:
# Assets/game/Welcome/Unity.2D.Welcome.asmdef
#
# It also removes ONLY that assembly's .meta file if it exists.
# It does not touch Assets/Welcome.

$duplicate = Join-Path $PWD "Assets\game\Welcome\Unity.2D.Welcome.asmdef"
$meta = "$duplicate.meta"

if (Test-Path $duplicate) {
    Remove-Item $duplicate -Force
    Write-Host "Removed duplicate: $duplicate"
} else {
    Write-Host "Duplicate asmdef was not found at: $duplicate"
}

if (Test-Path $meta) {
    Remove-Item $meta -Force
    Write-Host "Removed matching meta: $meta"
}

Write-Host ""
Write-Host "Now return to Unity and let it reimport the Assets folder."
