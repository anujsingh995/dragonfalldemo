using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player controls using Unity's NEW Input System.
/// WASD = move, 1 = Fire, 2 = Tail, 3 = Fly.
/// </summary>
[RequireComponent(typeof(DragonMotor), typeof(DragonCombat))]
public class PlayerController : MonoBehaviour
{
    DragonMotor motor;
    DragonCombat combat;

    void Awake()
    {
        motor = GetComponent<DragonMotor>();
        combat = GetComponent<DragonCombat>();
    }

    void Update()
    {
        if (Time.timeScale == 0f || combat == null || combat.MyHealth == null || combat.MyHealth.IsDead)
        {
            motor?.Stop();
            return;
        }

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            motor.Stop();
            return;
        }

        float x = 0f;
        float z = 0f;

        if (keyboard.aKey.isPressed) x -= 1f;
        if (keyboard.dKey.isPressed) x += 1f;
        if (keyboard.sKey.isPressed) z -= 1f;
        if (keyboard.wKey.isPressed) z += 1f;

        motor.Move(new Vector3(x, 0f, z));

        if (keyboard.digit1Key.wasPressedThisFrame)
            combat.TryFire();

        if (keyboard.digit2Key.wasPressedThisFrame)
            combat.TryTail();

        if (keyboard.digit3Key.wasPressedThisFrame)
            combat.TryFly();
    }
}
