DRAGONFALL ARENA - FIX PACK

1. CURRENT COMPILER ERROR
Unity reports:
Assembly with name 'Unity.2D.Welcome' already exists
Assets/game/Welcome/Unity.2D.Welcome.asmdef
Assets/Welcome/Unity.2D.Welcome.asmdef

The project has TWO asmdef files with the same assembly name.

FIX:
Keep:
    Assets/Welcome/Unity.2D.Welcome.asmdef

Delete:
    Assets/game/Welcome/Unity.2D.Welcome.asmdef
and its matching .meta file.

You can run FIX_DUPLICATE_WELCOME_ASMDEF.ps1 from the Unity project ROOT,
or delete the duplicate manually in File Explorer.

2. SCRIPT REPLACEMENTS
The .cs files in this folder are drop-in replacements for the uploaded versions.
Replace the matching scripts in your project:
HealthBar.cs
DamagePopup.cs
CameraRig.cs
RuntimeInputFix.cs
PlayerController.cs
WinnerButtonBinder.cs

3. CAMERA
CameraRig uses a fixed local position by default:
Position: (0, 32, -32)
Rotation: (45, 0, 0)
This prevents the previous ground-zoom problem.

4. AFTER CLEANUP
Close Unity.
Delete the project's Library folder ONLY if Unity still reports stale assembly/import errors.
Reopen the project and wait for the full import to finish.
Then open the DragonfallArena scene and press Play.

5. IMPORTANT
Do not keep multiple copies of the same script class in Assets.
There must be only one:
CameraRig
PlayerController
HealthBar
DamagePopup
RuntimeInputFix
WinnerButtonBinder
DragonCombat
Health

The scripts can reference other project classes such as DragonMotor,
DragonTeamVisual, DragonPointer and GameManager; those must exist once in
the project.
