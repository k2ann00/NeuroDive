using UnityEngine;
using TMPro;

public class VoiceCallManager : MonoBehaviour
{
    [Header("Manually Assigned Objects")]
    public GameObject dialogueManager;
    public GameObject dialogueArea;
    public GameObject voiceCallPanel;
    public GameObject skipPanel;
    public GameObject holdPanel;

    [Header("Countdown Control")]
    public TextMeshProUGUI countdownText;

    [Header("Delays (seconds)")]
    public float callStartDelay = 5f;
    public float autoEndDelay = 10f; // <== EKLENDİ: Otomatik çağrı sonlandırma süresi

    private float spaceHoldTime = 0f;
    private bool callStarted = false;
    private bool countdownActive = false;
    private bool interactionCompleted = false;

    void Start()
    {
        Invoke(nameof(StartVoiceCall), callStartDelay);
        Invoke(nameof(AutoEndVoiceCall), callStartDelay + autoEndDelay);


        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!callStarted || interactionCompleted)
            return;

        if (Input.GetKey(KeyCode.Space))
        {
            spaceHoldTime += Time.deltaTime;

            if (!countdownActive)
            {
                countdownActive = true;

                skipPanel?.SetActive(false);
                holdPanel?.SetActive(true);

                if (countdownText != null)
                    countdownText.gameObject.SetActive(true);
            }

            if (countdownText != null)
            {
                int secondsLeft = Mathf.CeilToInt(3f - spaceHoldTime);
                countdownText.text = secondsLeft.ToString();
            }

            if (spaceHoldTime >= 3f)
            {
                EndVoiceCall();
                ResetCountdown();
                interactionCompleted = true;
            }
        }
        else
        {
            if (countdownActive)
            {
                ResetCountdown();
                skipPanel?.SetActive(true);
                holdPanel?.SetActive(false);
            }
        }
    }

    public void StartVoiceCall()
    {
        dialogueManager?.SetActive(true);
        dialogueArea?.SetActive(true);
        voiceCallPanel?.SetActive(true);
        skipPanel?.SetActive(true);

        callStarted = true;

        // EKLENDİ: Belirli bir süre sonra otomatik olarak kapat
        Invoke(nameof(AutoEndVoiceCall), autoEndDelay);
    }

    public void EndVoiceCall()
    {
        dialogueArea?.SetActive(false);
        voiceCallPanel?.SetActive(false);
        skipPanel?.SetActive(false);
        holdPanel?.SetActive(false);

        callStarted = false;

        // EKLENDİ: Eğer kullanıcı erken sonlandırdıysa, Invoke'u iptal et
        CancelInvoke(nameof(AutoEndVoiceCall));
    }

    private void AutoEndVoiceCall()
    {
        if (callStarted && !interactionCompleted)
        {
            EndVoiceCall();
        }
    }

    private void ResetCountdown()
    {
        spaceHoldTime = 0f;
        countdownActive = false;

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
            countdownText.text = "";
        }
    }
}
