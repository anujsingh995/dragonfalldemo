using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Automatically applies the final look to the current scene when Play starts:
/// blue player, red enemy, slower enemy, floating pointers, trees instead of rock/box
/// obstacles, and a safe top-down camera setup.
/// </summary>
public static class DragonfallRuntimePresentation
{
    const string TreeResourceName = "DragonfallTree";
    static readonly Color PlayerBlue = new Color(0.15f, 0.55f, 1f, 1f);
    static readonly Color EnemyRed = new Color(1f, 0.18f, 0.18f, 1f);

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Apply()
    {
        GameObject player = GameObject.Find("PlayerDragon");
        GameObject enemy = GameObject.Find("EnemyDragon");
        if (player == null || enemy == null) return;

        ApplyDragon(player, PlayerBlue, 6f);
        ApplyDragon(enemy, EnemyRed, 2f);
        ApplyDemoPower(player);
        ApplyDemoPower(enemy);
        ReplaceRockBoxesWithTrees();
        CreatePointers(player.transform, enemy.transform);
        // Camera is controlled manually from the Main Camera Inspector.
        FixCamera(player.transform, enemy.transform);
    }

    static void ApplyDragon(GameObject dragon, Color color, float speed)
    {
        DragonTeamVisual visual = dragon.GetComponent<DragonTeamVisual>();
        if (visual == null) visual = dragon.AddComponent<DragonTeamVisual>();
        visual.teamColor = color;
        visual.ApplyColor();

        DragonMotor motor = dragon.GetComponent<DragonMotor>();
        if (motor != null) motor.moveSpeed = speed;
    }

    static void ReplaceRockBoxesWithTrees()
    {
        GameObject arena = GameObject.Find("ARENA");
        if (arena == null) return;

        GameObject treePrefab = Resources.Load<GameObject>(TreeResourceName);
        if (treePrefab == null) return;

        List<TransformData> replacements = new List<TransformData>();
        for (int i = arena.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = arena.transform.GetChild(i);
            if (!IsArenaBoxObstacle(child.name)) continue;
            replacements.Add(new TransformData(child.position, child.rotation, child.localScale));
            Object.Destroy(child.gameObject);
        }

        if (replacements.Count == 0) return;

        for (int i = 0; i < replacements.Count; i++)
        {
            TransformData data = replacements[i];
            GameObject tree = Object.Instantiate(treePrefab, data.position, data.rotation, arena.transform);
            tree.name = "ArenaTree_" + (i + 1);
            FitTreeToArena(tree, 3.4f);
        }
    }

    static bool IsArenaBoxObstacle(string name)
    {
        return name.StartsWith("Rock_") ||
               name.StartsWith("Box_") ||
               name.StartsWith("Obstacle_") ||
               name == "Cube";
    }

    static void FitTreeToArena(GameObject tree, float targetHeight)
    {
        Renderer[] renderers = tree.GetComponentsInChildren<Renderer>(true);
        if (renderers.Length == 0) return;

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);

        float factor = targetHeight / Mathf.Max(0.001f, bounds.size.y);
        tree.transform.localScale *= factor;

        bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
        tree.transform.position += Vector3.up * (-bounds.min.y + 0.02f);

        if (tree.GetComponent<Collider>() == null)
            tree.AddComponent<BoxCollider>();
    }

    static void CreatePointers(Transform player, Transform enemy)
    {
        GameObject root = GameObject.Find("DragonPointers");
        if (root == null) root = new GameObject("DragonPointers");

        RemovePointer(root.transform, "PlayerPointer");
        RemovePointer(root.transform, "EnemyPointer");

        DragonPointer.Create("PlayerPointer", player, "PLAYER", PlayerBlue, root.transform);
        DragonPointer.Create("EnemyPointer", enemy, "ENEMY", EnemyRed, root.transform);
    }

    static void ApplyDemoPower(GameObject player)
    {
        DragonCombat combat = player.GetComponent<DragonCombat>();
        if (combat == null) return;
        combat.fire.damage = 24f;
        combat.tail.damage = 12f;
        combat.fly.damage = 30f;
    }

    static void FixCamera(Transform player, Transform enemy)
    {
        Camera cam = Camera.main;
        if (cam == null) return;
        CameraRig rig = cam.GetComponent<CameraRig>();
        if (rig == null) rig = cam.gameObject.AddComponent<CameraRig>();
        rig.useFixedLocalPosition = true;
        rig.fixedLocalPosition = new Vector3(0f, 34f, -34f);
        rig.fixedLocalEulerAngles = new Vector3(45f, 0f, 0f);

        rig.fieldOfView = 22f;

        cam.transform.localPosition = new Vector3(0f, 34f, -34f);
        cam.transform.localRotation = Quaternion.Euler(45f, 0f, 0f);
        cam.fieldOfView = 22f;
    }

    static void RemovePointer(Transform parent, string name)
    {
        Transform old = parent.Find(name);
        if (old != null) Object.Destroy(old.gameObject);
    }

    readonly struct TransformData
    {
        public readonly Vector3 position;
        public readonly Quaternion rotation;
        public readonly Vector3 scale;
        public TransformData(Vector3 p, Quaternion r, Vector3 s) { position = p; rotation = r; scale = s; }
    }
}
