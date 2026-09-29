using UnityEngine;

/// <summary>
/// Simple enemy brain: Idle -> Chase -> Attack.
/// Picks an ability based on distance and uses the same cooldowns as the player,
/// because it calls the same DragonCombat methods.
/// </summary>
[RequireComponent(typeof(DragonMotor), typeof(DragonCombat))]
public class EnemyAI : MonoBehaviour
{
    enum State { Idle, Chase, Attack }

    [Header("Behaviour")]
    public float startDelay = 1.5f;     // wait before the fight starts
    public float stopDistance = 3f;     // stop walking when this close
    public float tailRange = 4f;        // use tail when closer than this
    public float fireMinRange = 5f;     // use fire between these distances
    public float fireMaxRange = 9f;     // (keep <= DragonCombat.fireRange)
    public float flyMinRange = 8f;      // use fly attack when farther than this

    [SerializeField] State state = State.Idle; // visible in the Inspector for debugging

    DragonMotor motor;
    DragonCombat combat;
    float startTime;

    void Awake()
    {
        motor = GetComponent<DragonMotor>();
        combat = GetComponent<DragonCombat>();
    }

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        Health targetHealth = combat.target;
        if (combat.MyHealth.IsDead || targetHealth == null || targetHealth.IsDead)
        {
            motor.Stop();
            return;
        }

        Vector3 toTarget = targetHealth.transform.position - transform.position;
        toTarget.y = 0f;
        float distance = toTarget.magnitude;

        switch (state)
        {
            case State.Idle:
                motor.Stop();
                if (Time.time - startTime >= startDelay) state = State.Chase;
                break;

            case State.Chase:
                if (TryAttack(distance))
                {
                    state = State.Attack;
                    break;
                }

                if (distance > stopDistance)
                {
                    motor.Move(toTarget);
                }
                else
                {
                    motor.Stop();
                    motor.FaceInstant(toTarget);
                }
                break;

            case State.Attack:
                // Wait until the ability finishes, then go back to chasing
                if (!combat.IsBusy) state = State.Chase;
                break;
        }
    }

    /// <summary>Choose an ability based on distance. Returns true if one started.</summary>
    bool TryAttack(float distance)
    {
        if (distance <= tailRange && combat.TryTail()) return true;
        if (distance >= fireMinRange && distance <= fireMaxRange && combat.TryFire()) return true;
        if (distance >= flyMinRange && combat.TryFly()) return true;
        return false;
    }
}
