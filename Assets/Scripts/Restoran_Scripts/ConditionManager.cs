using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    [Header("Atanacak Objeler")]
    public GameObject obj1;
    public GameObject obj2; 

    [Header("Kontrol Edilecek Tag")]
    public string conditionTag;

    private Collider obj2Collider;

    private bool isInside = false;

    void Start()
    {
        if (obj2 == null)
        {
            Debug.LogError("Obj2 atanmadı!");
            enabled = false;
            return;
        }

        obj2Collider = obj2.GetComponent<Collider>();
        if (obj2Collider == null)
        {
            Debug.LogError("Obj2 üzerinde Collider yok!");
            enabled = false;
            return;
        }

        if (obj1 == null)
        {
            Debug.LogWarning("Obj1 atanmadı, obj1 aktiflik değişikliği olmayacak.");
        }
    }

    void Update()
    {
        // Scene'deki conditionTag tag'ine sahip objeleri bul
        GameObject[] conditionObjects = GameObject.FindGameObjectsWithTag(conditionTag);

        bool anyInside = false;

        foreach (var condObj in conditionObjects)
        {
            if (obj2Collider.bounds.Contains(condObj.transform.position))
            {
                anyInside = true;
                break; // bir tane bulmak yeterli
            }
        }

        if (anyInside != isInside)
        {
            isInside = anyInside;
            if (obj1 != null)
                obj1.SetActive(!isInside);
        }
    }
}
