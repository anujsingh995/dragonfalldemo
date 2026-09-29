using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds hit points. Handles damage, hit flash, damage number popup, hit sound and knockback.
/// </summary>
public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public DamagePopup popupPrefab;
    public AudioClip hitSound;

    public event Action<Health> OnDied;

    public float Current { get; private set; }
    public bool IsDead => Current <= 0f;

    DragonMotor motor;
    DragonTeamVisual teamVisual;
    readonly List<Renderer> renderers = new List<Renderer>();
    readonly List<Color> originalColors = new List<Color>();
    readonly List<bool> useBaseColor = new List<bool>();

    void Awake()
    {
        Current = maxHealth;
        motor = GetComponent<DragonMotor>();
        teamVisual = GetComponent<DragonTeamVisual>();

        // Cache the body renderers so we can flash them red when hit.
        foreach (var r in GetComponentsInChildren<Renderer>())
        {
            if (r is ParticleSystemRenderer || r is TrailRenderer || r is LineRenderer) continue;
            if (r.material.HasProperty("_BaseColor"))
            {
                renderers.Add(r);
                originalColors.Add(r.material.GetColor("_BaseColor"));
                useBaseColor.Add(true);
            }
            else if (r.material.HasProperty("_Color"))
            {
                renderers.Add(r);
                originalColors.Add(r.material.color);
                useBaseColor.Add(false);
            }
        }
    }

    public void TakeDamage(float amount, Vector3 hitFrom, float knockback = 0f)
    {
        if (IsDead) return;

        Current = Mathf.Max(0f, Current - amount);

        if (popupPrefab != null)
        {
            var popup = Instantiate(popupPrefab, transform.position + Vector3.up * 4f, Quaternion.identity);
            popup.Setup(amount);
        }

        if (hitSound != null) AudioSource.PlayClipAtPoint(hitSound, transform.position);
        if (motor != null && knockback > 0f) motor.Knockback(hitFrom, knockback);

        StartCoroutine(Flash());

        if (IsDead) OnDied?.Invoke(this);
    }

    IEnumerator Flash()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            if (useBaseColor[i]) renderers[i].material.SetColor("_BaseColor", Color.red);
            else renderers[i].material.color = Color.red;
        }
        yield return new WaitForSeconds(0.1f);
        if (teamVisual != null)
        {
            teamVisual.RestoreColor();
            yield break;
        }

        for (int i = 0; i < renderers.Count; i++)
        {
            if (useBaseColor[i]) renderers[i].material.SetColor("_BaseColor", originalColors[i]);
            else renderers[i].material.color = originalColors[i];
        }
    }
}
