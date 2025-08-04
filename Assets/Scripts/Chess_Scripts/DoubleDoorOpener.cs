using UnityEngine;
using System.Collections;

public class DoubleDoorOpener : MonoBehaviour
{
    [Header("Alevler")]
    public GameObject fire;

    [Header("Kapý Kanatlarý")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Açýlma Ayarlarý")]
    public float openAngle = 90f;         // Her kapý ne kadar dönecek
    public float openDuration = 1.5f;     // Açýlma süresi (saniye)

    private Quaternion leftStartRot;
    private Quaternion rightStartRot;
    private bool isOpening = false;

    private void Awake()
    {
        // Baþlangýç rotasyonlarýný kaydet
        if (leftDoor != null) leftStartRot = leftDoor.rotation;
        if (rightDoor != null) rightStartRot = rightDoor.rotation;
    }

    /// <summary>
    /// Kapýyý açmak için bu fonksiyonu dýþarýdan çaðýr.
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
