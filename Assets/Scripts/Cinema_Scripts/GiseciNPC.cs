using UnityEngine;
using TMPro;

public class GiseciNPC : MonoBehaviour
{
    public static GiseciNPC Instance;
    [SerializeField] private GameObject interactText;
    [SerializeField] private GameObject DialogueManager;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject DialoguePanel;
    [SerializeField] private TextMeshProUGUI npcDialogueTMP;  // NPC için farklı TMP
    [SerializeField] private string[] npcDialogueLines;

    private bool playerInRange = false;
    public bool dialogueStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DialoguePanel.SetActive(true);
            interactText.SetActive(false);
            dialogueManager.StartDialogue(npcDialogueLines, npcDialogueTMP);
            dialogueStarted = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            DialogueManager.SetActive(true);
            interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactText.SetActive(false);
            DialoguePanel.SetActive(false);
        }
    }
}
