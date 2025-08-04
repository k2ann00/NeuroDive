using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public Button continueButton;
    public float typingSpeed = 0.05f;
    [TextArea(3, 10)]
    public string[] messages;
    public GameObject controlsPanel;

    private int messageIndex = 0;
    private string fullText = "";
    private CanvasGroup introTextGroup;

    // Controls panel içindeki yazılar ve buton
    private CanvasGroup controlsPanelGroup;
    private TextMeshProUGUI[] controlsTexts;
    private CanvasGroup[] controlsTextsGroups;
    private CanvasGroup continueButtonGroup;

    void Start()
    {
        continueButton.gameObject.SetActive(false);
        controlsPanel.SetActive(false); // Başta gizli

        introTextGroup = introText.GetComponent<CanvasGroup>();
        if (introTextGroup == null)
        {
            introTextGroup = introText.gameObject.AddComponent<CanvasGroup>();
            introTextGroup.alpha = 1f;
        }

        // Controls panel için CanvasGroup
        controlsPanelGroup = controlsPanel.GetComponent<CanvasGroup>();
        if (controlsPanelGroup == null)
            controlsPanelGroup = controlsPanel.AddComponent<CanvasGroup>();

        // Controls paneldeki TextMeshProUGUI'leri al
        controlsTexts = controlsPanel.GetComponentsInChildren<TextMeshProUGUI>(true);

        // Her yazı için CanvasGroup yoksa ekle
        controlsTextsGroups = new CanvasGroup[controlsTexts.Length];
        for (int i = 0; i < controlsTexts.Length; i++)
        {
            CanvasGroup cg = controlsTexts[i].GetComponent<CanvasGroup>();
            if (cg == null)
                cg = controlsTexts[i].gameObject.AddComponent<CanvasGroup>();

            cg.alpha = 0f;
            controlsTextsGroups[i] = cg;
        }

        // Continue Button için CanvasGroup
        continueButtonGroup = continueButton.GetComponent<CanvasGroup>();
        if (continueButtonGroup == null)
        {
            continueButtonGroup = continueButton.gameObject.AddComponent<CanvasGroup>();
        }
        continueButtonGroup.alpha = 0f;
        continueButton.gameObject.SetActive(false);

        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        string currentLine = "";
        foreach (char letter in messages[messageIndex])
        {
            currentLine += letter;
            introText.text = fullText + currentLine;
            yield return new WaitForSeconds(typingSpeed);
        }

        fullText += currentLine + "\n\n";
        messageIndex++;

        yield return new WaitForSeconds(1f);

        if (messageIndex < messages.Length)
        {
            StartCoroutine(TypeSentence());
        }
        else
        {
            StartCoroutine(ShowControlsPanel());
        }
    }

    IEnumerator ShowControlsPanel()
    {
        yield return StartCoroutine(FadeOutText());     
        controlsPanel.SetActive(true);

       
        yield return StartCoroutine(FadeCanvasGroup(controlsPanelGroup, 0f, 1f, 0.5f));

       
        for (int i = 0; i < controlsTextsGroups.Length; i++)
        {
            yield return StartCoroutine(FadeCanvasGroup(controlsTextsGroups[i], 0f, 1f, 0.3f));
            
            yield return new WaitForSeconds(0.1f);
        }

      
        continueButton.gameObject.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(continueButtonGroup, 0f, 1f, 0.5f));
    }

    IEnumerator FadeOutText()
    {
        float duration = 1f;
        float time = 0f;
        float startAlpha = introTextGroup.alpha;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.SmoothStep(startAlpha, 0f, time / duration);
            introTextGroup.alpha = alpha;
            yield return null;
        }

        introTextGroup.alpha = 0f;
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.SmoothStep(start, end, time / duration);
            yield return null;
        }

        cg.alpha = end;
    }

    public void OnContinueClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}







