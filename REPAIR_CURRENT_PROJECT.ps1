param(
    [Parameter(Mandatory=$true)]
    [string]$ProjectPath
)

$ErrorActionPreference = 'Stop'
$ProjectPath = (Resolve-Path $ProjectPath).Path
$AssetsPath = Join-Path $ProjectPath 'Assets'
if (-not (Test-Path $AssetsPath)) { throw "Not a Unity project: Assets folder not found at $AssetsPath" }

Write-Host "Dragonfall Arena repair" -ForegroundColor Cyan
Write-Host "Project: $ProjectPath"
Write-Host "CLOSE UNITY before continuing." -ForegroundColor Yellow
Read-Host "Press ENTER after Unity is fully closed"

$stamp = Get-Date -Format 'yyyyMMdd_HHmmss'
$backup = Join-Path (Split-Path $ProjectPath -Parent) "DragonfallRepairBackup_$stamp"
New-Item -ItemType Directory -Path $backup -Force | Out-Null

function Move-ToBackup([string]$relativePath) {
    $src = Join-Path $ProjectPath $relativePath
    if (Test-Path $src) {
        $dest = Join-Path $backup ($relativePath -replace '[\\/:*?"<>|]', '_')
        Write-Host "Backing up $relativePath" -ForegroundColor DarkYellow
        Move-Item -LiteralPath $src -Destination $dest -Force
    }
}

# These are the duplicate trees visible in the GUID/class errors.
Move-ToBackup 'Assets/Assets'
Move-ToBackup 'Assets/game'

# Restore exactly one clean game tree.
$payload = Join-Path $PSScriptRoot 'Payload/Assets/game'
if (-not (Test-Path $payload)) { throw "Repair payload missing: $payload" }
New-Item -ItemType Directory -Path (Join-Path $AssetsPath 'game') -Force | Out-Null
Copy-Item -Path (Join-Path $payload '*') -Destination (Join-Path $AssetsPath 'game') -Recurse -Force

# The old Library contains the GUID/import database for the corrupted tree.
foreach ($folder in @('Library','Temp','obj')) {
    $p = Join-Path $ProjectPath $folder
    if (Test-Path $p) {
        Write-Host "Deleting $folder so Unity reimports cleanly..." -ForegroundColor DarkYellow
        Remove-Item -LiteralPath $p -Recurse -Force
    }
}

$readme = Join-Path $ProjectPath 'Dragonfall_REPAIR_RESULT.txt'
@"
Dragonfall Arena repair completed.

Open the Unity project normally, then open:
Assets/game/Scenes/DragonfallArena.unity

Expected setup:
- One clean Assets/game tree
- Fixed CameraRig local position (0,20,-20)
- Fixed camera FOV 22
- Blue Player / red Enemy
- Player and Enemy pointers
- Tree obstacles
- No nested Assets/Assets copy

A backup of the old conflicting Assets folders is here:
$backup
"@ | Set-Content -Path $readme -Encoding UTF8

Write-Host "DONE." -ForegroundColor Green
Write-Host "Open the project and open Assets/game/Scenes/DragonfallArena.unity" -ForegroundColor Green
Write-Host "Backup: $backup"
