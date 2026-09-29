@echo off
setlocal
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0FIX_DRAGONFALL_DUPLICATES.ps1"
endlocal
