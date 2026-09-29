using TMPro;
using UnityEngine;

/// <summary>Floating pointer above a dragon that always faces the camera.</summary>
public class DragonPointer : MonoBehaviour
{
    public Transform target;
    public Color color = Color.white;
    public float height = 4.6f;
    public float bobHeight = 0.12f;
    public float bobSpeed = 3f;

    TextMeshPro text;
    float startTime;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        if (text == null) text = gameObject.AddComponent<TextMeshPro>();
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 2.0f;
        text.fontStyle = FontStyles.Bold;
        text.color = color;
        text.text = "▼";
        text.raycastTarget = false;
        startTime = Time.time;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        float bob = Mathf.Sin((Time.time - startTime) * bobSpeed) * bobHeight;
        transform.position = target.position + Vector3.up * (height + bob);

        Camera cam = Camera.main;
        if (cam != null)
            transform.rotation = Quaternion.LookRotation(-cam.transform.forward, Vector3.up);
    }

    public static DragonPointer Create(string objectName, Transform target, string label, Color color, Transform parent)
    {
        GameObject go = new GameObject(objectName);
        if (parent != null) go.transform.SetParent(parent, false);

        DragonPointer pointer = go.AddComponent<DragonPointer>();
        pointer.target = target;
        pointer.color = color;
        pointer.height = 4.6f;

        if (pointer.text == null) pointer.text = go.GetComponent<TextMeshPro>();
        pointer.text.text = "▼\n" + label;
        pointer.text.color = color;
        return pointer;
    }
}
