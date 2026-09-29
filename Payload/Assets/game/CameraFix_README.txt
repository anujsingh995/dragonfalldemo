DRAGONFALL ARENA - CAMERA FIX

This patch fixes the camera being too close to the ground.
The camera now uses a higher 2.5D angle, a wider framing distance, and LookRotation aimed at the dragons instead of relying on a fixed Euler angle.

In Unity:
1. Close Play Mode.
2. Replace your project's Assets folder with this ZIP's Assets folder (merge/replace).
3. Open DragonfallArena.unity.
4. Press Play.

Expected camera:
- both dragons visible
- arena visible around them
- high angled top-down view
- camera smoothly follows and zooms out as they separate
