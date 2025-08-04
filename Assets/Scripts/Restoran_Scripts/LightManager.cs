using UnityEngine;
using System.Collections.Generic;

public class LightManager : MonoBehaviour
{
    [Header("Spotlight Objeleri")]
    public List<GameObject> spotlights;

    [Header("MaterialChanger Referansı")]
    public MaterialChanger materialChanger;
    
    [Header("Enemy NPC")]
    public GameObject enemyNPC;

    [Header("Test Modu")]
    public bool testMode = false;

    private bool lastToggleState = false;

    void Start()
    {
        lastToggleState = testMode;
    }

    void Update()
    {
        if (testMode != lastToggleState)
        {
            if (testMode)
            {
                DisableSpotlightsAndChangeMaterials();
            }
            else
            {
                EnableSpotlightsAndRestoreMaterials();
            }
            lastToggleState = testMode;
        }
    }

    public void DisableSpotlightsAndChangeMaterials()
    {
        foreach (GameObject spotlight in spotlights)
        {
            if (spotlight != null)
                spotlight.SetActive(!spotlight.activeSelf);
        }

        if (materialChanger != null)
        {
            materialChanger.ChangeMaterials();
        }
        else
        {
            Debug.LogWarning("MaterialChanger referansı atanmadı!");
        }
        
        if (enemyNPC != null)
        {
            var detectionLight = enemyNPC.GetComponent<EnemyDetectionLight>();
            if (detectionLight != null)
            {
                detectionLight.enabled = true;
            }
            else
            {
                Debug.LogWarning("EnemyDetectionLight componenti enemyNPC üzerinde bulunamadı!");
            }
        }
        else
        {
            Debug.LogWarning("Enemy NPC referansı atanmadı!");
        }
        
    }

    public void EnableSpotlightsAndRestoreMaterials()
    {
        foreach (GameObject spotlight in spotlights)
        {
            if (spotlight != null)
                spotlight.SetActive(!spotlight.activeSelf); 
        }

        if (materialChanger != null)
        {
            materialChanger.RestoreOriginalMaterials();
        }
        else
        {
            Debug.LogWarning("MaterialChanger referansı atanmadı!");
        }
    }

}