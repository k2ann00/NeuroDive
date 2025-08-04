using UnityEngine;
using UnityEngine.UI;

public class PosterInspect : MonoBehaviour
{
    private bool playerInRange = false;
    private bool isPanelOpened = false;
    [SerializeField] private GameObject pressE;
    [SerializeField] private SpriteRenderer posterRenderer;
    [SerializeField] private Sprite poster;
    [SerializeField] private Image posterImage;
    [SerializeField] private GameObject posterPanel;

    private void Start()
    {
        posterRenderer = GetComponentInChildren<SpriteRenderer>();
        poster = posterRenderer.sprite;
    }
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isPanelOpened) // Panel kapalýysa aç
            {
                posterPanel.SetActive(true);
                posterImage.sprite = poster;
                isPanelOpened = true;
            }
            else // Panel açýksa kapat
            {
                posterPanel.SetActive(false);
                isPanelOpened = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            pressE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pressE.SetActive(false);
        }
    }
}
