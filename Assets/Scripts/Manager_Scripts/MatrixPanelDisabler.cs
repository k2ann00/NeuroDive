using UnityEngine;
using System.Collections;

public class MatrixPanelDisabler : MonoBehaviour
{
    [Header("Delay before disabling (seconds)")]
    public float delay = 5f;

    [Header("Name of the Canvas")]
    public string canvasName = "Canvas";

    [Header("Name of the child object to disable")]
    public string panelName = "Matrix Panel";

    void Start()
    {
        StartCoroutine(DisableMatrixPanelAfterDelay());
    }

    IEnumerator DisableMatrixPanelAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        GameObject canvasObj = FindCanvasInDontDestroyOnLoad(canvasName);
        if (canvasObj == null)
        {
            // Debug.LogWarning($"Canvas named '{canvasName}' was not found in DontDestroyOnLoad objects.");
            yield break;
        }

        Transform matrixPanel = canvasObj.transform.Find(panelName);
        if (matrixPanel != null)
        {
            matrixPanel.gameObject.SetActive(false);
            // Debug.Log($"'{panelName}' has been deactivated.");
        }
        else
        {
            // Debug.LogWarning($"'{panelName}' was not found under the Canvas.");
        }
    }

    GameObject FindCanvasInDontDestroyOnLoad(string canvasName)
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == canvasName && obj.GetComponent<Canvas>() != null)
            {
                return obj;
            }
        }
        return null;
    }
}