using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Drives a UI Slider (and optional label) from a Health component.</summary>
public class HealthBar : MonoBehaviour
{
    public Health health;
    public Slider slider;
    public TMP_Text label; // optional, e.g. "75 / 100"

    void Update()
    {
        if (health == null) return;

        slider.value = health.Current / health.maxHealth;
        if (label != null)
            label.text = Mathf.CeilToInt(health.Current) + " / " + Mathf.CeilToInt(health.maxHealth);
    }
}
