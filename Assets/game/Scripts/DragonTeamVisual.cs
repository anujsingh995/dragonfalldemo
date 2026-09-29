using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gives PlayerDragon / EnemyDragon a simple team tint without changing the FBX asset.
/// Materials are copied once per dragon, then reused so repeated hit flashes do not create new materials.
/// </summary>
public class DragonTeamVisual : MonoBehaviour
{
    public Color teamColor = Color.white;

    readonly List<Material> tintedMaterials = new List<Material>();
    bool cached;

    public void ApplyColor()
    {
        CacheMaterials();
        SetColor(teamColor);
    }

    public void RestoreColor()
    {
        if (!cached)
        {
            ApplyColor();
            return;
        }

        SetColor(teamColor);
    }

    void CacheMaterials()
    {
        if (cached) return;

        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renderers)
        {
            if (r is ParticleSystemRenderer || r is TrailRenderer || r is LineRenderer)
                continue;

            Material[] mats = r.materials; // create per-dragon material instances once
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i] != null && (mats[i].HasProperty("_BaseColor") || mats[i].HasProperty("_Color")))
                    tintedMaterials.Add(mats[i]);
            }
        }

        cached = true;
    }

    void SetColor(Color color)
    {
        for (int i = 0; i < tintedMaterials.Count; i++)
        {
            Material m = tintedMaterials[i];
            if (m == null) continue;
            if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", color);
            else if (m.HasProperty("_Color")) m.SetColor("_Color", color);
        }
    }

    void Awake()
    {
        ApplyColor();
    }
}
