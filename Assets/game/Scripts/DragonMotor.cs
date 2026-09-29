using UnityEngine;

/// <summary>
/// Moves and turns a dragon using its Rigidbody. Used by both the player and the AI.
/// Also handles knockback.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class DragonMotor : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float turnSpeed = 720f;

    /// <summary>Set to false while casting an ability.</summary>
    public bool CanMove { get; set; } = true;

    /// <summary>0 = standing still, 1 = moving. Drives the run animation.</summary>
    public float NormalizedSpeed { get; private set; }

    Rigidbody rb;
    float knockbackUntil;

    bool InKnockback => Time.time < knockbackUntil;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Move(Vector3 direction)
    {
        direction.y = 0f;

        if (!CanMove || direction.sqrMagnitude < 0.01f)
        {
            Stop();
            return;
        }

        direction.Normalize();

        if (!InKnockback && !rb.isKinematic)
        {
            rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);
            NormalizedSpeed = 1f;
        }

        Face(direction);
    }

    public void Stop()
    {
        NormalizedSpeed = 0f;
        if (InKnockback || rb.isKinematic) return;
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
    }

    /// <summary>Turn smoothly towards a direction.</summary>
    public void Face(Vector3 direction)
    {
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.001f) return;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
    }

    /// <summary>Snap to face a direction (used when casting).</summary>
    public void FaceInstant(Vector3 direction)
    {
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.001f) return;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    /// <summary>Push this dragon away from a world position.</summary>
    public void Knockback(Vector3 fromPosition, float force)
    {
        if (rb.isKinematic || force <= 0f) return; // flying dragons ignore knockback

        Vector3 dir = transform.position - fromPosition;
        dir.y = 0f;
        dir.Normalize();

        rb.linearVelocity = dir * force;
        knockbackUntil = Time.time + 0.25f;
    }

    void FixedUpdate()
    {
        // Slow the knockback down smoothly
        if (InKnockback)
        {
            Vector3 v = rb.linearVelocity;
            v.x *= 0.92f;
            v.z *= 0.92f;
            rb.linearVelocity = v;
        }
    }
}
