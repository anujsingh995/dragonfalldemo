using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies a per-dragon team tint using MaterialPropertyBlock.
/// This avoids creating material instances while editing the scene.
/// </summary>
public class DragonTeamVisual : MonoBehaviour
{
    public Color teamColor = Color.white;

    readonly List<Renderer> tintedRenderers = new List<Renderer>();
    readonly Dictionary<Renderer, string> colorProperties = new Dictionary<Renderer, string>();
    bool cached;

    public void ApplyColor()
    {
        CacheMaterials();
        SetColor(teamColor);
    }

    public void RestoreColor()
    {
        ApplyColor();
    }

    void CacheMaterials()
    {
        if (cached) return;

        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer r = renderers[i];
            if (r == null || r is ParticleSystemRenderer || r is TrailRenderer || r is LineRenderer)
                continue;

            Material[] mats = r.sharedMaterials;
            string property = null;
            for (int j = 0; j < mats.Length; j++)
            {
                Material m = mats[j];
                if (m == null) continue;
                if (m.HasProperty("_BaseColor")) { property = "_BaseColor"; break; }
                if (m.HasProperty("_Color")) { property = "_Color"; break; }
            }

            if (property != null)
            {
                tintedRenderers.Add(r);
                colorProperties[r] = property;
            }
        }

        cached = true;
    }

    void SetColor(Color color)
    {
        for (int i = 0; i < tintedRenderers.Count; i++)
        {
            Renderer r = tintedRenderers[i];
            if (r == null) continue;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            r.GetPropertyBlock(block);
            if (colorProperties.TryGetValue(r, out string property))
                block.SetColor(property, color);
            r.SetPropertyBlock(block);
        }
    }

    void Awake()
    {
        ApplyColor();
    }
}
