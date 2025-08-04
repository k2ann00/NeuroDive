using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterviewAreaController : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEntry
    {
        public string questionText;
        public bool isChoiceQuestion; // false = düz yazý, true = seçenekli soru
        public string choiceA;
        public string choiceB;
        public int correctAnswer; // 0 = A, 1 = B
    }

    public DialogueEntry[] dialogueEntries;

    public GameObject interactionPrompt;      // "E’ye bas" UI
    public GameObject narrativePanel;         // Düz diyalog paneli
    public GameObject questionPanel;          // Seçenekli panel
    public GameObject kumandaPanel;
    public KumandaController kumandaController;

    public TextMeshProUGUI narrativeTMP;
    public TextMeshProUGUI questionTMP;
    public TextMeshProUGUI choiceAText;
    public TextMeshProUGUI choiceBText;

    public Button choiceAButton;
    public Button choiceBButton;

    public float textSpeed = 0.05f;
    public float lineDelay = 1f;

    private int currentIndex = 0;
    private bool playerInRange = false;
    private bool dialogueStarted = false;

    void Start()
    {
        interactionPrompt.SetActive(false);
        narrativePanel.SetActive(false);
        questionPanel.SetActive(false);

        choiceAButton.onClick.AddListener(() => AnswerSelected(0));
        choiceBButton.onClick.AddListener(() => AnswerSelected(1));
    }

    void Update()
    {
        if (playerInRange && !dialogueStarted && Input.GetKeyDown(KeyCode.E))
        {
            interactionPrompt.SetActive(false);
            dialogueStarted = true;
            ShowNextEntry();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!dialogueStarted)
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionPrompt.SetActive(false);
            questionPanel.SetActive(false);
            narrativePanel.SetActive(false);
        }
    }

    private void ShowNextEntry()
    {
        if (currentIndex >= dialogueEntries.Length)
        {
            EndDialogue(true);
            return;
        }

        var entry = dialogueEntries[currentIndex];

        if (entry.isChoiceQuestion)
        {
            ShowChoice(entry);
        }
        else
        {
            StartCoroutine(TypeNarrative(entry.questionText));
        }
    }

    private IEnumerator TypeNarrative(string line)
    {
        narrativePanel.SetActive(true);
        narrativeTMP.text = "";

        foreach (char c in line)
        {
            narrativeTMP.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(lineDelay);

        narrativePanel.SetActive(false);
        currentIndex++;
        ShowNextEntry();
    }

    private void ShowChoice(DialogueEntry entry)
    {
        questionPanel.SetActive(true);
        questionTMP.text = entry.questionText;
        choiceAText.text = entry.choiceA;
        choiceBText.text = entry.choiceB;
    }

    private void AnswerSelected(int selected)
    {
        var entry = dialogueEntries[currentIndex];

        if (selected == entry.correctAnswer)
        {
            currentIndex++;
            questionPanel.SetActive(false);
            ShowNextEntry();
        }
        else
        {
            EndDialogue(false);
        }
    }

    private void EndDialogue(bool success)
    {
        questionPanel.SetActive(false);
        narrativePanel.SetActive(false);
        interactionPrompt.SetActive(false);

        if (success)
        {
            Debug.Log("Tebrikler, iþe alýndýnýz! Kumandayý aldýnýz.");
            kumandaPanel.SetActive(true);
            kumandaController.IsControlTaken = true;
        }
        else
        {
            Debug.Log("Maalesef yanlýþ cevap, mülakat baþarýsýz.");
            // Oyuncuyu geri gönder vs...
        }
    }
}
