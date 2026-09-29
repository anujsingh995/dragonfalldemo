using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One ability icon. Shows a radial cooldown overlay and the seconds remaining.
/// abilityIndex: 0 = Fire, 1 = Tail, 2 = Fly.
/// </summary>
public class AbilityIcon : MonoBehaviour
{
    public DragonCombat combat;
    public int abilityIndex;
    public Image cooldownOverlay;   // Image Type = Filled, Radial 360
    public TMP_Text timerText;      // optional

    void Update()
    {
        if (combat == null || cooldownOverlay == null) return;
        Ability a = combat.GetAbility(abilityIndex);
        float remaining = a.Remaining;

        cooldownOverlay.fillAmount = a.cooldown > 0f ? remaining / a.cooldown : 0f;
        if (timerText != null)
            timerText.text = remaining > 0.05f ? remaining.ToString("0.0") : "";
    }
}
