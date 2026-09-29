using TMPro;
using UnityEngine;

/// <summary>
/// Floating damage number. Put on a prefab that has a TextMeshPro (3D) component.
/// </summary>
public class DamagePopup : MonoBehaviour
{
    public float lifetime = 0.8f;
    public float riseSpeed = 3f;

    public void Setup(float damage)
    {
        GetComponent<TextMeshPro>().text = Mathf.RoundToInt(damage).ToString();
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
        // Always face the camera
        transform.rotation = Camera.main.transform.rotation;
    }
}
