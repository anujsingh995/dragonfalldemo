using UnityEngine;

/// <summary>Fixed 2.5D battle camera. Uses local position so the framing stays stable.</summary>
public class CameraRig : MonoBehaviour
{
    [Header("Fixed camera framing")]
    public bool useFixedLocalPosition = true;
    public Vector3 fixedLocalPosition = new Vector3(0f, 20f, -20f);
    public Vector3 fixedLocalEulerAngles = new Vector3(50f, 0f, 0f);

    [Header("Optional follow framing")]
    public Transform dragonA;
    public Transform dragonB;
    public float smoothing = 5f;
    public float followDistance = 20f;
    public float heightRatio = 1f;
    public float lookHeight = 0.7f;
    public float fieldOfView = 22f;

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        ApplyCameraSettings();
        ApplyFixedPose();
    }

    void OnEnable() => ApplyFixedPose();

    void LateUpdate()
    {
        ApplyCameraSettings();
        if (useFixedLocalPosition)
        {
            ApplyFixedPose();
            return;
        }

        if (dragonA == null || dragonB == null) return;

        Vector3 middle = (dragonA.position + dragonB.position) * 0.5f;
        middle.y = 0f;
        float horizontal = followDistance / Mathf.Sqrt(1f + heightRatio * heightRatio);
        float height = horizontal * heightRatio;
        Vector3 desired = middle + new Vector3(0f, height, -horizontal);
        transform.position = Vector3.Lerp(transform.position, desired, smoothing * Time.deltaTime);
        Quaternion look = Quaternion.LookRotation(middle + Vector3.up * lookHeight - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, smoothing * Time.deltaTime);
    }

    void ApplyCameraSettings()
    {
        if (cam == null) return;
        cam.orthographic = false;
        cam.fieldOfView = fieldOfView;
        cam.nearClipPlane = 0.1f;
        cam.farClipPlane = 200f;
    }

    public void ApplyFixedPose()
    {
        transform.localPosition = fixedLocalPosition;
        transform.localRotation = Quaternion.Euler(fixedLocalEulerAngles);
        if (cam != null) cam.fieldOfView = fieldOfView;
    }
}
