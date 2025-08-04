using UnityEngine;
using TMPro;

public class EnemyFailTrigger : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panelToActivate;
    public TextMeshProUGUI tmpTextToActivate;

    [Header("Trigger Settings")]
    public string enemyTag = "Enemy"; // Tag of the object that triggers the panel

    private bool isPanelActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isPanelActive && other.CompareTag(enemyTag))
        {
            ActivateUIPanel();
        }
    }

    void ActivateUIPanel()
    {
        if (panelToActivate != null)
            panelToActivate.SetActive(true);

        if (tmpTextToActivate != null)
            tmpTextToActivate.gameObject.SetActive(true);

        Time.timeScale = 0f; // Freeze the game except for UI
        isPanelActive = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (panelToActivate != null)
            panelToActivate.SetActive(false);
        if (tmpTextToActivate != null)
            tmpTextToActivate.gameObject.SetActive(false);

        isPanelActive = false;
    }
}
