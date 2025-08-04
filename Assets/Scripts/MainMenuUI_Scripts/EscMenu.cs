using UnityEngine;

public class EscMenu : MonoBehaviour
{
    [Header("Açılıp kapanacak UI Canvas objesi")]
    public GameObject targetCanvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCanvas();
        }
    }

    // ESC tuşu ve butonla çağrılabilir
    public void ToggleCanvas()
    {
        if (targetCanvas != null)
        {
            bool isActive = targetCanvas.activeSelf;
            targetCanvas.SetActive(!isActive);
        }
        else
        {
            Debug.LogWarning("Target Canvas atanmadı!");
        }
    }

    // "Canvas" altındaki "Canvas Button Manager" objesinden LoadMainMenu çağırır
    public void CallLoadMainMenu()
    {
        GameObject rootCanvas = GameObject.Find("Canvas");

        if (rootCanvas == null)
        {
            Debug.LogWarning("Canvas objesi sahnede bulunamadı.");
            return;
        }

        Transform buttonManagerTransform = rootCanvas.transform.Find("Canvas Button Manager");

        if (buttonManagerTransform == null)
        {
            Debug.LogWarning("\"Canvas Button Manager\" alt obje bulunamadı.");
            return;
        }

        // Script adını doğru yazmalısın
        var buttonManagerScript = buttonManagerTransform.GetComponent<CanvasButtonManager>();

        if (buttonManagerScript != null)
        {
            buttonManagerScript.LoadMainMenu();
        }
        else
        {
            Debug.LogWarning("\"Canvas Button Manager\" objesinde CanvasButtonManager scripti bulunamadı.");
        }
    }
}