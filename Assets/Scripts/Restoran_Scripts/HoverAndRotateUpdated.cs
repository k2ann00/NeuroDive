using UnityEngine;

public class HoverAndRotateUpdated : MonoBehaviour
{
    [Header("Hover Ayarlar�")]
    public float hoverAmplitude = 0.5f;       // Y�kseklik
    public float hoverFrequency = 1f;         // H�z

    [Header("D�nme Ayarlar�")]
    public bool rotationEnabled = true;       // D�n�� a��k m�?
    public float rotationSpeed = 50f;         // D�n�� h�z� (derece/saniye)
    public float axisChangeSpeed = 0.5f;      // Eksen de�i�tirme h�z�

    [Header("Rastgele Ba�lang�� Y�ksekli�i")]
    public bool randomizeStartHeight = false; // Rastgele y�kseklik kullan�ls�n m�?
    public float minStartY = 0f;              // Minimum ba�lang�� y�ksekli�i
    public float maxStartY = 2f;              // Maksimum ba�lang�� y�ksekli�i

    private Vector3 initialPosition;
    private Vector3 currentRotationAxis;

    void Start()
    {
        // Rastgele y�kseklik atanacaksa
        float startY = transform.position.y;
        if (randomizeStartHeight)
        {
            startY = Random.Range(minStartY, maxStartY);
        }

        initialPosition = new Vector3(transform.position.x, startY, transform.position.z);
        transform.position = initialPosition;

        currentRotationAxis = Random.onUnitSphere;  // Rastgele bir eksenle ba�la
    }

    void Update()
    {
        HoverEffect();
        if (rotationEnabled)
        {
            RotateEffect();
        }
    }

    void HoverEffect()
    {
        float newY = initialPosition.y + Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        Vector3 position = transform.position;
        position.y = newY;
        transform.position = position;
    }

    void RotateEffect()
    {
        Vector3 targetAxis = Random.onUnitSphere;
        currentRotationAxis = Vector3.Slerp(currentRotationAxis, targetAxis, Time.deltaTime * axisChangeSpeed);
        transform.Rotate(currentRotationAxis, rotationSpeed * Time.deltaTime, Space.World);
    }
}
