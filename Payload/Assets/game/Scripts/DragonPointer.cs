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
        EnsureText();
        ApplyTextStyle();
        startTime = Time.time;
    }

    void EnsureText()
    {
        text = GetComponent<TextMeshPro>();
        if (text == null)
            text = gameObject.AddComponent<TextMeshPro>();
    }

    void ApplyTextStyle()
    {
        if (text == null) return;
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 2.0f;
        text.fontStyle = FontStyles.Bold;
        text.color = color;
        text.text = "▼";
        text.raycastTarget = false;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
            return;
        }

        if (!gameObject.activeSelf) gameObject.SetActive(true);

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

        // Add TextMeshPro explicitly because editor-time AddComponent does not
        // reliably invoke Awake before the setup code continues.
        TextMeshPro tmp = go.AddComponent<TextMeshPro>();
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 2.0f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = color;
        tmp.text = "▼\n" + label;
        tmp.raycastTarget = false;

        DragonPointer pointer = go.AddComponent<DragonPointer>();
        pointer.target = target;
        pointer.color = color;
        pointer.height = 4.6f;
        pointer.text = tmp;
        pointer.startTime = Time.time;
        pointer.ApplyTextStyle();
        return pointer;
    }
}
