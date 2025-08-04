using UnityEngine;

public class InGameCinematicCameraController : MonoBehaviour
{
    public IsometricCameraFollow cameraFollow;
    public float adjustmentSpeed = 1f;
    public float adjustmentStep = 0.1f;

    [Header("Mouse Ayarları")]
    public float mouseSensitivity = 3f;
    public float scrollSensitivity = 5f;
    private Vector3 lastMousePosition;
    private bool isRightMouseDown;

    void Update()
    {
        if (cameraFollow == null) return;

        // === Klavye ile hız ayarı ===
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            adjustmentSpeed += adjustmentStep;
            Debug.Log("Adjustment Speed Increased: " + adjustmentSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            adjustmentSpeed = Mathf.Max(0.1f, adjustmentSpeed - adjustmentStep);
            Debug.Log("Adjustment Speed Decreased: " + adjustmentSpeed);
        }

        // === Klavye ile yükseklik kontrolü ===
        if (Input.GetKey(KeyCode.Keypad4))
            cameraFollow.height -= adjustmentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Keypad7))
            cameraFollow.height += adjustmentSpeed * Time.deltaTime;

        // === Klavye ile mesafe kontrolü ===
        if (Input.GetKey(KeyCode.KeypadPeriod))
            cameraFollow.distance -= adjustmentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Keypad0))
            cameraFollow.distance += adjustmentSpeed * Time.deltaTime;

        // === Klavye ile yatay / dikey açı kontrolü ===
        if (Input.GetKey(KeyCode.Keypad5))
            cameraFollow.orbitRotationEuler.x -= adjustmentSpeed * Time.deltaTime * 30f;
        if (Input.GetKey(KeyCode.Keypad2))
            cameraFollow.orbitRotationEuler.x += adjustmentSpeed * Time.deltaTime * 30f;
        if (Input.GetKey(KeyCode.Keypad1))
            cameraFollow.orbitRotationEuler.y -= adjustmentSpeed * Time.deltaTime * 30f;
        if (Input.GetKey(KeyCode.Keypad3))
            cameraFollow.orbitRotationEuler.y += adjustmentSpeed * Time.deltaTime * 30f;

        // === Mouse Drag ile orbit rotation kontrolü ===
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
            isRightMouseDown = true;
        }

        if (Input.GetMouseButton(1) && isRightMouseDown)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            cameraFollow.orbitRotationEuler.y += delta.x * mouseSensitivity * Time.deltaTime;
            cameraFollow.orbitRotationEuler.x -= delta.y * mouseSensitivity * Time.deltaTime;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRightMouseDown = false;
        }

        // === Mouse Scroll ile zoom (distance) kontrolü ===
        float scrollDelta = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scrollDelta) > 0.01f)
        {
            cameraFollow.distance -= scrollDelta * scrollSensitivity;
        }

        // Clamp’ler
        cameraFollow.height = Mathf.Max(0f, cameraFollow.height);
        cameraFollow.distance = Mathf.Max(0.1f, cameraFollow.distance); // 0 olmasın
    }
}
