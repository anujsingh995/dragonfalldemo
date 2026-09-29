DRAGONFALL ARENA - ONE CLICK SETUP
=================================

1. Copy this entire Assets folder into your Unity project.
2. Wait for Unity to finish importing the FBX and compiling the scripts.
3. In Unity, click:
      Tools -> Dragonfall -> Build Starter Scene
4. Open Assets/Game/Scenes/Arena.unity if it is not already open.
5. Press Play.

CONTROLS
--------
W A S D = Move
1 = Fire
2 = Tail Attack
3 = Fly Attack

WHAT IS ALREADY INCLUDED
------------------------
- The dragon FBX
- Existing gameplay scripts
- Automatic Animator setup
- Run animation
- Tail Attack animation
- Simple Idle pose made from the first Run pose
- Player + Enemy
- Basic AI
- Health bars
- Ability buttons/cooldown overlays
- Camera that keeps both dragons in view
- Winner panel + restart hook
- Basic arena, walls and obstacles
- Simple placeholder particles

IMPORTANT
---------
You currently have real Run + Tail Attack animation clips. Fire and Fly still use the
existing gameplay code without custom skeletal attack animation. This setup intentionally
does not consume or require more AI animation credits.

If Unity reports a compile error after importing, send the Console screenshot before
changing anything.
