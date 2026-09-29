# Dragonfall Arena - current progress package

This ZIP keeps the user's current Assets and adds a one-click setup tool.

## What is already in this package
- PlayerDragon.fbx
- Run animation
- Tail Attack animation
- Existing combat / movement / AI / health / UI scripts
- `DragonfallSetup.cs` editor tool
- `WinnerButtonBinder.cs`
- Safer cooldown icon and URP hit-flash handling

## Finish the scene
1. Open your Unity project.
2. Put this `Assets` folder into the Unity project root and allow overwrite/merge.
3. Let Unity finish compiling.
4. In Unity, click:
   `Tools -> Dragonfall Arena -> BUILD COMPLETE ARENA`
5. Open `Assets/game/Scenes/DragonfallArena.unity` if it is not already open.
6. Press Play.

## Controls
WASD = Move
1 = Fire
2 = Tail
3 = Fly

## Important
You currently only have Run and Tail Attack animations. The setup therefore:
- Uses the Run clip as the base state and freezes it at Speed 0, so it works as a temporary idle pose.
- Uses the Tail Attack clip for the Tail ability.
- Leaves Fire and Fly without custom animation triggers for now; their gameplay code still produces fire particles / flight movement.
