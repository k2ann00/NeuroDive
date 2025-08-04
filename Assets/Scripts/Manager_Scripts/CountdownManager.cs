using UnityEngine;
using TMPro;
using System.Collections;
using System;
using Unity.VisualScripting;

public class CountdownManager : MonoBehaviour
{
    [Header("Timer Settings")]
    public float initialDelay;         // İlk bekleme süresi
    public float countdownDuration;    // Geri sayım süresi
    public float panelDelay;           // LevelCompleted sonrası panelin açılma gecikmesi

    [Header("UI References")]
    public TextMeshProUGUI countdownText;   // Sayaç metni
    public GameObject panelToActivate;      // win panel
    public Canvas uiCanvas;                 // UI Canvas
    public GameObject countdownPanel;       // Sayaç paneli

    [Header("Countdown Manager Object")]
    public GameObject persistentObject;     // dont destroy edilecek Countdown manager objesi

    [Header("Timeout Settings")]
    public GameObject timeoutPanel;         // Süre bittiğinde ama level tamamlanmadıysa açılacak panel
    public TextMeshProUGUI timeoutText;     // Süre bittiğinde gösterilecek TMP metin

    private float countdownTimeLeft;
    private bool levelCompleted = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (uiCanvas != null)
            DontDestroyOnLoad(uiCanvas.gameObject);
        
        if (persistentObject != null)
            DontDestroyOnLoad(persistentObject);

        // if (countdownText != null)
        //     DontDestroyOnLoad(countdownText.gameObject);

        // if (panelToActivate != null)
        //     DontDestroyOnLoad(panelToActivate.gameObject);
        
        // if (timeoutPanel != null)
        //     DontDestroyOnLoad(timeoutPanel);

        // if (timeoutText != null)
        //     DontDestroyOnLoad(timeoutText.gameObject);
    }

    private void Start()
    {
        countdownTimeLeft = countdownDuration;

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (countdownPanel != null)
            countdownPanel.SetActive(false);

        if (timeoutText != null)
            timeoutText.gameObject.SetActive(false);

        if (timeoutPanel != null)
            timeoutPanel.SetActive(false);

        StartCoroutine(StartCountdownWithDelay());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            TimeScaler();
    }

    private void TimeScaler()
    {
        if (Time.timeScale == 1f) { Time.timeScale = 2f; }
        else if (Time.timeScale == 2f) { Time.timeScale = 1f; }

        Debug.Log("Time Scale : " +  Time.timeScale);
    }

    IEnumerator StartCountdownWithDelay()
    {
        yield return new WaitForSeconds(initialDelay);

        if (levelCompleted) yield break;

        if (countdownText != null)
            countdownText.gameObject.SetActive(true);

        if (countdownPanel != null)
            countdownPanel.SetActive(true);

        while (countdownTimeLeft > 0f && !levelCompleted)
        {
            countdownTimeLeft -= Time.deltaTime;
            UpdateCountdownDisplay(countdownTimeLeft);
            yield return null;
        }

        if (!levelCompleted)
        {
            if (countdownText != null)
                countdownText.gameObject.SetActive(false);

            if (countdownPanel != null)
                countdownPanel.SetActive(false);

            // GECİKME KALDIRILDI: yield return new WaitForSeconds(panelDelay); satırı silindi

            if (timeoutPanel != null)
                timeoutPanel.SetActive(true);

            if (timeoutText != null)
                timeoutText.gameObject.SetActive(true);

            Time.timeScale = 0f; // Oyunu duraklat
        }
    }


    private void UpdateCountdownDisplay(float time)
    {
        if (countdownText != null)
        {
            time = Mathf.Max(time, 0f);
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            countdownText.text = string.Format("{0}:{1:00}", minutes, seconds);
        }
    }

    public void LevelCompleted()
    {
        if (levelCompleted) return;
        levelCompleted = true;

        StopAllCoroutines();

        if (countdownPanel != null)
            countdownPanel.SetActive(false);

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        StartCoroutine(ShowPanelAfterDelay());
    }

    IEnumerator ShowPanelAfterDelay()
    {
        yield return new WaitForSeconds(panelDelay);

        if (panelToActivate != null)
            panelToActivate.SetActive(true);
    }
}
