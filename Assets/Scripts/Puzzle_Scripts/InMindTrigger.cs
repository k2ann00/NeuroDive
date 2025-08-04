using UnityEngine;
using UnityEngine.SceneManagement;

public class InMindTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "SahneAdi";
    [SerializeField] private bool loadAdditively = false;  // Bu true ise sahne üzerine yüklenir

    private bool playerInTrigger = false;

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (loadAdditively)
                SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
            else
                SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
}