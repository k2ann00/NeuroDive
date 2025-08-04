using UnityEngine;

public class HoverAndRotateUpdated : MonoBehaviour
{
    [Header("Hover Ayarlarý")]
    public float hoverAmplitude = 0.5f;       // Yükseklik
    public float hoverFrequency = 1f;         // Hýz

    [Header("Dönme Ayarlarý")]
    public bool rotationEnabled = true;       // Dönüþ açýk mý?
    public float rotationSpeed = 50f;         // Dönüþ hýzý (derece/saniye)
    public float axisChangeSpeed = 0.5f;      // Eksen deðiþtirme hýzý

    [Header("Rastgele Baþlangýç Yüksekliði")]
    public bool randomizeStartHeight = false; // Rastgele yükseklik kullanýlsýn mý?
    public float minStartY = 0f;              // Minimum baþlangýç yüksekliði
    public float maxStartY = 2f;              // Maksimum baþlangýç yüksekliði

    private Vector3 initialPosition;
    private Vector3 currentRotationAxis;

    void Start()
    {
        // Rastgele yükseklik atanacaksa
        float startY = transform.position.y;
        if (randomizeStartHeight)
        {
            startY = Random.Range(minStartY, maxStartY);
        }

        initialPosition = new Vector3(transform.position.x, startY, transform.position.z);
        transform.position = initialPosition;

        currentRotationAxis = Random.onUnitSphere;  // Rastgele bir eksenle baþla
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
