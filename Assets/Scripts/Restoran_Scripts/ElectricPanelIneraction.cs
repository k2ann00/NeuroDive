using UnityEngine;

public class ElectricPanelIneraction : MonoBehaviour
{
    [Header("LightManager scriptini buraya atayın")]
    public LightManager lightManager;

    [Header("E tuşu engelleme alanı (Chief için)")]
    public Collider checkCollider;

    public GameObject pressEText;

    private bool playerIsInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pressEText.SetActive(true);
            playerIsInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pressEText.SetActive(false);
            playerIsInside = false;
        }
    }

    private void Update()
    {
        if (playerIsInside && Input.GetKeyDown(KeyCode.E))
        {
            if (!IsChiefInsideCheckCollider())
            {
                if (lightManager != null)
                {
                    lightManager.DisableSpotlightsAndChangeMaterials();
                }
                else
                {
                    Debug.LogWarning("LightManager atanmadı!");
                }
            }
            else
            {
                Debug.Log("Chief içeride, E tuşu işlemi engellendi.");
            }
        }
    }

    private bool IsChiefInsideCheckCollider()
    {
        if (checkCollider == null)
        {
            Debug.LogWarning("Check Collider atanmadı!");
            return false;
        }

        Bounds bounds = checkCollider.bounds;
        Collider[] overlapping = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity);

        foreach (Collider col in overlapping)
        {
            if (col.CompareTag("Chief"))
            {
                return true;
            }
        }

        return false;
    }
}