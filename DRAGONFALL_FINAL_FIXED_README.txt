DRAGONFALL ARENA - FINAL FIXED PATCH

Target: Unity 6.6 / 6000.6.x

This patch fixes the problems seen in the current project:
- Fixes Health.cs coroutine compile error (yield break).
- Fixes DragonfallSetup.cs missing List<> namespace.
- Avoids deprecated FindFirstObjectByType warning in the setup/input helper.
- Uses the NEW Input System for WASD and keys 1/2/3.
- Fixes the old StandaloneInputModule at runtime.
- Removes the Animator Speed parameter warning by making animation playback safe when the parameter is missing.
- Tail Attack can play from either the generated controller or the older controller already in the project.
- Player is BLUE.
- Enemy is RED.
- Enemy movement speed is 2.5; player speed stays 6.
- Floating PLAYER/ENEMY pointers are added automatically.
- Arena Rock_/Box_/Obstacle_ objects are removed and replaced with the uploaded tree.
- Camera is forced to a readable 55 degree top-down angle and both dragons are kept in view.
- The uploaded good tree model is included as ArenaTree.fbx and as a Resources asset.

INSTALL (BEGINNER):
1. Close Unity.
2. Extract this ZIP.
3. Copy the included Assets folder into your Unity project folder.
4. When Windows asks, choose Merge/Replace.
5. Open Unity again.
6. Wait for Unity to finish importing and compiling.
7. Open your existing DragonfallArena scene.
8. Press Play.

CONTROLS:
W A S D = Move
1 = Fire
2 = Tail
3 = Fly

IMPORTANT:
The current Unity AI 'NoSubscription' messages can be ignored; the game does not need Unity AI.
The yellow FindFirstObjectByType warnings should be gone after replacing the old scripts with this patch.
The script patch is designed to work with the scene you already created.
