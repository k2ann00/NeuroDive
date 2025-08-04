using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public float textSpeed = 0.05f;
    public float lineDelay = 1f;

    private Coroutine currentCoroutine;

    // Açılışta kendi TMP ile kullanmak için
    public TextMeshProUGUI defaultDialogueText;
    public string[] defaultLines;

    void Start()
    {
        if (defaultLines != null && defaultLines.Length > 0)
            StartDialogue(defaultLines, defaultDialogueText);
    }

    public void StartDialogue(string[] lines, TextMeshProUGUI targetTMP)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(RunDialogue(lines, targetTMP));
    }

    IEnumerator RunDialogue(string[] lines, TextMeshProUGUI dialogueText)
    {
        dialogueText.gameObject.SetActive(true);

        for (int i = 0; i < lines.Length; i++)
        {
            yield return StartCoroutine(TypeLine(lines[i], dialogueText));
            yield return new WaitForSeconds(lineDelay);
        }

        dialogueText.text = "";
        dialogueText.gameObject.SetActive(false);  // ✅ sadece TMP kapanır, Manager değil
    }

    IEnumerator TypeLine(string line, TextMeshProUGUI dialogueText)
    {
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
