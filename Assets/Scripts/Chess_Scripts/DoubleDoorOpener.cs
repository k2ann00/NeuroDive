using UnityEngine;
using System.Collections;

public class DoubleDoorOpener : MonoBehaviour
{
    [Header("Alevler")]
    public GameObject fire;

    [Header("Kap� Kanatlar�")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("A��lma Ayarlar�")]
    public float openAngle = 90f;         // Her kap� ne kadar d�necek
    public float openDuration = 1.5f;     // A��lma s�resi (saniye)

    private Quaternion leftStartRot;
    private Quaternion rightStartRot;
    private bool isOpening = false;

    private void Awake()
    {
        // Ba�lang�� rotasyonlar�n� kaydet
        if (leftDoor != null) leftStartRot = leftDoor.rotation;
        if (rightDoor != null) rightStartRot = rightDoor.rotation;
    }

    /// <summary>
    /// Kap�y� a�mak i�in bu fonksiyonu d��ar�dan �a��r.
    /// </summary>
    public void OpenDoor()
    {
        if (!isOpening && leftDoor != null && rightDoor != null)
        {
            fire.SetActive(false);
            StartCoroutine(OpenDoorCoroutine());
        }
    }

    private IEnumerator OpenDoorCoroutine()
    {
        isOpening = true;

        Quaternion leftTargetRot = leftStartRot * Quaternion.Euler(0, -openAngle, 0);
        Quaternion rightTargetRot = rightStartRot * Quaternion.Euler(0, openAngle, 0);

        float elapsed = 0f;
        while (elapsed < openDuration)
        {
            float t = elapsed / openDuration;

            leftDoor.rotation = Quaternion.Slerp(leftStartRot, leftTargetRot, t);
            rightDoor.rotation = Quaternion.Slerp(rightStartRot, rightTargetRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Tam hedef rotasyona ayarla
        leftDoor.rotation = leftTargetRot;
        rightDoor.rotation = rightTargetRot;

        isOpening = false;
    }
}
