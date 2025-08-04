using UnityEngine;
using System.Collections.Generic;

public class MaterialChanger : MonoBehaviour
{
    [Header("De�i�tirilecek Objeler")]
    public List<GameObject> objectsToChange;

    [Header("Yeni Materyal")]
    public Material newMaterial;

    [Header("Test Ayarlar�")]
    public bool testMode = false;  // Inspector'dan kontrol edilen toggle

    private bool lastToggleState = false;  // Toggle��n �nceki durumu

    // Objelerin orijinal materyallerini saklamak i�in
    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();

    void Start()
    {
        // Objelerin orijinal materyallerini sakla
        foreach (GameObject obj in objectsToChange)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null)
            {
                originalMaterials[obj] = rend.sharedMaterials;
            }
        }

        // �lk toggle durumunu al
        lastToggleState = testMode;
    }

    void Update()
    {
        // Toggle de�i�tiyse kontrol et
        if (testMode != lastToggleState)
        {
            if (testMode)
            {
                ChangeMaterials();
            }
            else
            {
                RestoreOriginalMaterials();
            }

            lastToggleState = testMode;
        }
    }

    // Materyalleri yeni materyalle de�i�tirir
    public void ChangeMaterials()
    {
        foreach (GameObject obj in objectsToChange)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null && newMaterial != null)
            {
                Material[] newMats = new Material[rend.sharedMaterials.Length];
                for (int i = 0; i < newMats.Length; i++)
                {
                    newMats[i] = newMaterial;
                }
                rend.materials = newMats;
            }
        }
    }

    // Objelerin orijinal materyallerini geri y�kler
    public void RestoreOriginalMaterials()
    {
        foreach (GameObject obj in objectsToChange)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null && originalMaterials.ContainsKey(obj))
            {
                rend.materials = originalMaterials[obj];
            }
        }
    }
}
