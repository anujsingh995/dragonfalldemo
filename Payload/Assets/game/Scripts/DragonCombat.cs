using System.Collections;
using UnityEngine;

/// <summary>Data for one ability: damage, cooldown, animation trigger and sound.</summary>
[System.Serializable]
public class Ability
{
    public string name = "Ability";
    [Tooltip("Total damage dealt by the ability")]
    public float damage = 10f;
    public float cooldown = 3f;
    [Tooltip("Animator trigger to play. Optional when using direct state playback.")]
    public string animTrigger;
    public AudioClip sound;

    [HideInInspector] public float readyTime;

    public bool IsReady => Time.time >= readyTime;
    public float Remaining => Mathf.Max(0f, readyTime - Time.time);
}

/// <summary>
/// The three abilities (Fire, Tail, Fly). Used by BOTH the player and the enemy.
/// This version also works with the user's existing Animator Controller even when
/// Speed/Tail parameters or transitions have not been created yet.
/// </summary>
[RequireComponent(typeof(DragonMotor), typeof(Health))]
public class DragonCombat : MonoBehaviour
{
    [Header("References")]
    public Health target;
    public Animator animator;
    public Transform model;
    public ParticleSystem fireFx;
    public ParticleSystem tailFx;
    public ParticleSystem landFx;

    [Header("Abilities")]
    public Ability fire = new Ability { name = "Fire", damage = 24f, cooldown = 5f };
    public Ability tail = new Ability { name = "Tail", damage = 12f, cooldown = 2f, animTrigger = "Tail" };
    public Ability fly  = new Ability { name = "Fly",  damage = 30f, cooldown = 9f };

    [Header("Fire settings")]
    public float fireRange = 9f;
    public float fireHalfAngle = 30f;
    public float fireWindup = 0.3f;
    public float fireDuration = 1.5f;

    [Header("Tail settings")]
    public float tailRange = 4f;
    public float tailHitDelay = 0.4f;
    public float tailDuration = 0.9f;
    public float tailKnockback = 12f;

    [Header("Fly settings")]
    public float flyHeight = 6f;
    public float takeoffTime = 0.6f;
    public float hoverTime = 0.5f;
    public float diveTime = 0.35f;
    public float landRadius = 5f;
    public float landStopDistance = 2.5f;
    public float landKnockback = 18f;

    public bool IsBusy { get; private set; }
    public Health MyHealth { get; private set; }

    DragonMotor motor;
    Rigidbody rb;
    bool isFlying;
    string runStateName;
    string tailStateName;

    void Awake()
    {
        motor = GetComponent<DragonMotor>();
        rb = GetComponent<Rigidbody>();
        MyHealth = GetComponent<Health>();
        CacheAnimationStates();
    }

    void Start()
    {
        CacheAnimationStates();
        PlayRunState();
    }

    void Update()
    {
        if (animator == null || animator.runtimeAnimatorController == null || motor == null)
            return;

        // We don't require a Speed parameter. The Run clip is used as a simple
        // idle pose when stationary; it plays normally while moving/attacking.
        animator.speed = (IsBusy || isFlying) ? (isFlying ? 1.5f : 1f) :
                         (motor.NormalizedSpeed > 0.01f ? 1f : 0f);
    }

    public Ability GetAbility(int index) => index == 0 ? fire : index == 1 ? tail : fly;

    bool CanUse(Ability a) =>
        !IsBusy && a.IsReady && !MyHealth.IsDead && target != null && !target.IsDead;

    public bool TryFire() { if (!CanUse(fire)) return false; StartCoroutine(FireRoutine()); return true; }
    public bool TryTail() { if (!CanUse(tail)) return false; StartCoroutine(TailRoutine()); return true; }
    public bool TryFly()  { if (!CanUse(fly))  return false; StartCoroutine(FlyRoutine());  return true; }

    IEnumerator FireRoutine()
    {
        Begin(fire);
        yield return new WaitForSeconds(fireWindup);
        if (fireFx != null) fireFx.Play();

        const float tickTime = 0.25f;
        int ticks = Mathf.Max(1, Mathf.RoundToInt(fireDuration / tickTime));
        float damagePerTick = fire.damage / ticks;

        for (int i = 0; i < ticks; i++)
        {
            if (target != null && !target.IsDead && InCone(fireRange, fireHalfAngle))
                target.TakeDamage(damagePerTick, transform.position, 1f);
            yield return new WaitForSeconds(fireDuration / ticks);
        }

        if (fireFx != null) fireFx.Stop();
        yield return new WaitForSeconds(0.3f);
        End();
    }

    IEnumerator TailRoutine()
    {
        Begin(tail);
        yield return new WaitForSeconds(tailHitDelay);

        if (tailFx != null) tailFx.Play();
        if (target != null && !target.IsDead && FlatToTarget().magnitude <= tailRange)
            target.TakeDamage(tail.damage, transform.position, tailKnockback);

        yield return new WaitForSeconds(Mathf.Max(0f, tailDuration - tailHitDelay));
        End();
    }

    IEnumerator FlyRoutine()
    {
        Begin(fly);
        isFlying = true;
        rb.isKinematic = true;

        Vector3 groundLocal = model != null ? model.localPosition : Vector3.zero;
        Vector3 airLocal = groundLocal + Vector3.up * flyHeight;

        for (float t = 0f; t < 1f; t += Time.deltaTime / takeoffTime)
        {
            if (model != null) model.localPosition = Vector3.Lerp(groundLocal, airLocal, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        if (model != null) model.localPosition = airLocal;

        for (float t = 0f; t < hoverTime; t += Time.deltaTime)
        {
            if (target != null) motor.FaceInstant(FlatToTarget());
            yield return null;
        }

        Vector3 from = transform.position;
        Vector3 dir = target != null ? FlatToTarget().normalized : transform.forward;
        Vector3 to = target != null ? target.transform.position - dir * landStopDistance : from + dir * 3f;
        to.y = from.y;

        for (float t = 0f; t < 1f; t += Time.deltaTime / diveTime)
        {
            float e = t * t;
            transform.position = Vector3.Lerp(from, to, e);
            if (model != null) model.localPosition = Vector3.Lerp(airLocal, groundLocal, e);
            yield return null;
        }

        transform.position = to;
        if (model != null) model.localPosition = groundLocal;

        rb.isKinematic = false;
        isFlying = false;
        if (landFx != null) landFx.Play();
        if (target != null && !target.IsDead && FlatToTarget().magnitude <= landRadius)
            target.TakeDamage(fly.damage, transform.position, landKnockback);

        yield return new WaitForSeconds(0.4f);
        End();
    }

    void Begin(Ability a)
    {
        IsBusy = true;
        a.readyTime = Time.time + a.cooldown;

        motor.Stop();
        motor.CanMove = false;
        if (target != null) motor.FaceInstant(FlatToTarget());

        if (a.sound != null) AudioSource.PlayClipAtPoint(a.sound, transform.position);

        // Tail works with either the generated controller (Tail parameter) or
        // the older controller that only contains the imported animation state.
        if (a == tail)
            PlayTailState();
        else if (animator != null && !string.IsNullOrEmpty(a.animTrigger) && HasTrigger(a.animTrigger))
            animator.SetTrigger(a.animTrigger);
    }

    void End()
    {
        IsBusy = false;
        motor.CanMove = true;
        PlayRunState();
    }

    void CacheAnimationStates()
    {
        runStateName = null;
        tailStateName = null;
        if (animator == null || animator.runtimeAnimatorController == null) return;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        AnimationClip runClip = null;
        AnimationClip tailClip = null;

        for (int i = 0; i < clips.Length; i++)
        {
            AnimationClip clip = clips[i];
            if (clip == null || clip.name.StartsWith("__preview__")) continue;
            string n = clip.name.ToLowerInvariant();
            if (runClip == null && n.Contains("run")) runClip = clip;
            else if (tailClip == null && (n.Contains("tail") || n.Contains("stylized_heroic_quadruped_dragon_performs_a_powerf"))) tailClip = clip;
        }

        if (runClip != null)
        {
            string[] candidates = { "Base Layer.Run", "Run", "Base Layer." + runClip.name, runClip.name };
            runStateName = FirstExistingState(candidates);
        }

        if (tailClip != null)
        {
            string[] candidates = { "Base Layer.Tail Attack", "Tail Attack", "Base Layer.Tail", "Tail", "Base Layer." + tailClip.name, tailClip.name };
            tailStateName = FirstExistingState(candidates);
        }
    }

    string FirstExistingState(string[] candidates)
    {
        for (int i = 0; i < candidates.Length; i++)
        {
            if (animator.HasState(0, Animator.StringToHash(candidates[i])))
                return candidates[i];
        }
        return null;
    }

    bool HasTrigger(string name)
    {
        if (animator == null) return false;
        AnimatorControllerParameter[] parameters = animator.parameters;
        for (int i = 0; i < parameters.Length; i++)
            if (parameters[i].name == name && parameters[i].type == AnimatorControllerParameterType.Trigger)
                return true;
        return false;
    }

    void PlayTailState()
    {
        if (animator == null) return;
        if (tailStateName == null) CacheAnimationStates();
        if (!string.IsNullOrEmpty(tailStateName)) animator.Play(tailStateName, 0, 0f);
        else if (HasTrigger("Tail")) animator.SetTrigger("Tail");
    }

    void PlayRunState()
    {
        if (animator == null) return;
        if (runStateName == null) CacheAnimationStates();
        if (!string.IsNullOrEmpty(runStateName)) animator.Play(runStateName, 0, 0f);
        animator.speed = 0f;
    }

    Vector3 FlatToTarget()
    {
        if (target == null) return transform.forward;
        Vector3 d = target.transform.position - transform.position;
        d.y = 0f;
        return d;
    }

    bool InCone(float range, float halfAngle)
    {
        Vector3 d = FlatToTarget();
        return d.magnitude <= range && Vector3.Angle(transform.forward, d) <= halfAngle;
    }
}
