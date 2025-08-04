using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloatingChessPieces : MonoBehaviour
{
    [Header("Y�kseltilecek Objeler")]
    public List<Transform> piecesToLift = new List<Transform>();

    [Header("Y�kselme Ayarlar�")]
    public float liftDuration = 0.25f;
    public float minLiftHeight = 1.2f;
    public float maxLiftHeight = 2.2f;
    public float hoverAmplitude = 0.15f;
    public float hoverSpeed = 2f;

    [Header("Rotasyon Ayarlar�")]
    public float maxTiltX = 20f; // X ekseni: �ne arkaya yatma
    public float maxTiltY = 360f; // Y ekseni: tam d�n��
    public float maxTiltZ = 20f; // Z ekseni: sa�a sola yatma
    public float rotationLerpSpeed = 1.5f;

    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, float> individualHeights = new Dictionary<Transform, float>();
    private Dictionary<Transform, Quaternion> targetRotations = new Dictionary<Transform, Quaternion>();
    private bool isFloating = false;
    private float floatStartTime;

    /// <summary>
    /// Ta�lar� yukar� kald�r�r ve hover & d�nme efektlerini ba�lat�r.
    /// </summary>
    public void BeginFloating()
    {
        if (isFloating) return;

        originalPositions.Clear();
        individualHeights.Clear();
        targetRotations.Clear();

        foreach (Transform piece in piecesToLift)
        {
            originalPositions[piece] = piece.position;

            // Rastgele y�kseklik
            float height = Random.Range(minLiftHeight, maxLiftHeight);
            individualHeights[piece] = height;

            // Rastgele ama kontroll� rotasyon
            Vector3 randomEuler = new Vector3(
                Random.Range(-maxTiltX, maxTiltX),
                Random.Range(0f, maxTiltY),
                Random.Range(-maxTiltZ, maxTiltZ)
            );
            targetRotations[piece] = Quaternion.Euler(randomEuler);

            // Y�kselt
            StartCoroutine(LiftPiece(piece, height));
        }

        isFloating = true;
        floatStartTime = Time.time;
    }

    private IEnumerator LiftPiece(Transform piece, float targetHeight)
    {
        Vector3 startPos = piece.position;
        Vector3 targetPos = startPos + Vector3.up * targetHeight;

        float elapsed = 0f;
        while (elapsed < liftDuration)
        {
            piece.position = Vector3.Lerp(startPos, targetPos, elapsed / liftDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        piece.position = targetPos;
    }

    private void Update()
    {
        if (!isFloating) return;

        float hoverOffset = Mathf.Sin((Time.time - floatStartTime) * hoverSpeed) * hoverAmplitude;

        foreach (Transform piece in piecesToLift)
        {
            if (!originalPositions.ContainsKey(piece)) continue;

            // Hover pozisyonu
            Vector3 basePos = originalPositions[piece] + Vector3.up * individualHeights[piece];
            piece.position = basePos + Vector3.up * hoverOffset;

            // D�n��
            if (targetRotations.ContainsKey(piece))
            {
                piece.rotation = Quaternion.Slerp(piece.rotation, targetRotations[piece], Time.deltaTime * rotationLerpSpeed);
            }
        }
    }

    /// <summary>
    /// Hover efektini durdurur. (Opsiyonel)
    /// </summary>
    public void StopFloating()
    {
        isFloating = false;
    }
}
