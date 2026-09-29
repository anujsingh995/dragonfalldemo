using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public bool useFixedLocalPosition = true;

    public Vector3 fixedLocalPosition = new Vector3(0f, 34f, -34f);

    public Vector3 fixedLocalEulerAngles = new Vector3(45f, 0f, 0f);

    public Transform dragonA;
    public Transform dragonB;

    public float smoothing = 5f;
    public float followDistance = 32f;
    public float heightRatio = 1f;
    public float lookHeight = 0.7f;

    public float fieldOfView = 22f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        ApplyCamera();
    }

    private void LateUpdate()
    {
        ApplyCamera();
    }

    private void ApplyCamera()
    {
        if (cam == null)
            cam = GetComponent<Camera>();

        transform.localPosition = fixedLocalPosition;
        transform.localRotation = Quaternion.Euler(fixedLocalEulerAngles);

        if (cam != null)
        {
            cam.orthographic = false;
            cam.fieldOfView = fieldOfView;
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 200f;
        }
    }
}