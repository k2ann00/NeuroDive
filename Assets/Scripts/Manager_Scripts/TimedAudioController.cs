using UnityEngine;
using System.Collections;

public class TimedAudioController : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public float delayBeforeStart = 2f;
    public float playDuration = 5f;

    [Header("UI Dependency")]
    public GameObject uiObjectToWatch;

    private Coroutine playCoroutine;
    private bool uiWasEverActive = false;

    void Start()
    {
        if (audioSource == null)
        {
            // Debug.LogError("AudioSource is not assigned.");
            return;
        }

        playCoroutine = StartCoroutine(PlayAudioAfterDelay());
    }

    IEnumerator PlayAudioAfterDelay()
    {
        // Debug.Log("Waiting before starting audio...");
        yield return new WaitForSeconds(delayBeforeStart);

        // Debug.Log("Playing audio.");
        audioSource.Play();

        float elapsedTime = 0f;

        while (elapsedTime < playDuration)
        {
            // UI o an aktifse, işaretle
            if (uiObjectToWatch != null && uiObjectToWatch.activeInHierarchy)
            {
                uiWasEverActive = true;
            }

            // Eğer UI daha önce aktif olduysa ve şimdi inaktifse, sesi durdur
            if (uiWasEverActive && uiObjectToWatch != null && !uiObjectToWatch.activeInHierarchy)
            {
                // Debug.Log("UI was active before and now became inactive. Stopping audio.");
                audioSource.Stop();
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Debug.Log("Play duration ended. Stopping audio.");
        audioSource.Stop();
    }
}