using UnityEngine;

[ExecuteAlways]
public class IsometricCameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;

    [Header("Camera Position Offset")]
    public float height = 10f;
    public float distance = 10f;

    [Range(0f, 90f)]
    public float verticalAngle = 45f;

    [Header("Orbit Rotation Offsets")]
    [Tooltip("Kameranın target etrafında kendi etrafında dönüşü")]
    public Vector3 orbitRotationEuler = new Vector3(0f, 45f, 0f); // Y ekseni 45°, klasik izometrik görünüm

    [Header("Follow Smoothing")]
    [Range(0f, 1f)]
    public float followSmoothness = 0.1f;

    private Vector3 currentVelocity;

    void LateUpdate()
    {
        if (target == null) return;

        // Kamera rotasyonunu hesapla: Kullanıcı tanımlı üç eksenli dönüş
        Quaternion orbitRotation = Quaternion.Euler(orbitRotationEuler);

        // Kameranın offseti (dikey yükseklik + mesafe + açı)
        Vector3 offset = Quaternion.Euler(verticalAngle, 0f, 0f) * new Vector3(0f, 0f, -distance);
        offset += Vector3.up * height;

        // Offset'i yörünge rotasyonuna göre döndür
        Vector3 finalOffset = orbitRotation * offset;

        // Kamera hedef pozisyon
        Vector3 targetPosition = target.position + finalOffset;

        // Kamera pozisyonunu smooth geçişle ayarla
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, followSmoothness);

        transform.position = smoothedPosition;

        // Her zaman hedefe bak
        transform.LookAt(target.position);
    }

    void OnValidate()
    {
        if (distance < 0f) distance = 0f;
        if (height < 0f) height = 0f;
    }
}