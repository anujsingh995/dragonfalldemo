# Dragonfall Arena - Input Fix

This patch is for Unity 6.6 projects using the NEW Input System.

Changes:
- PlayerController now reads WASD + 1/2/3 with UnityEngine.InputSystem.Keyboard.
- DragonfallSetup creates InputSystemUIInputModule instead of StandaloneInputModule.
- RuntimeInputFix automatically disables an old StandaloneInputModule and adds InputSystemUIInputModule when a scene loads.
- Editor menu added: Tools > Dragonfall Arena > Fix Input System In Current Scene.

Recommended steps:
1. Close Unity.
2. Merge this Assets folder into the project.
3. Open Unity and wait for import/compile.
4. Open DragonfallArena.unity.
5. Click Play.
6. Click the Game view, then test W/A/S/D and 1/2/3.
