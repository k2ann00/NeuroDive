using UnityEngine;
using System.Collections;

public class CarAlarmController : MonoBehaviour
{
    [Header("Işık Ayarları")]
    public Light[] frontLights;
    public Light[] rearLights;

    [Header("Alarm Ayarları")]
    public AudioSource alarmAudioSource;
    public float blinkInterval = 0.5f;

    [Header("Ses Azaltma Ayarları")]
    public float fadeStartTime = 5f;
    public float fadeDuration = 3f;

    private Coroutine alarmCoroutine;
    private Coroutine fadeCoroutine;

    public void TriggerAlarm()
    {
        if (alarmCoroutine == null)
        {
            alarmCoroutine = StartCoroutine(AlarmRoutine());

            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeAudioRoutine());

            // Debug.Log("<color=lime>[Alarm]</color> <color=white>Alarm tetiklendi. Ses çalıyor ve ışıklar yanıp sönmeye başladı.</color>");
        }
        else
        {
            // Debug.Log("<color=yellow>[Alarm]</color> <color=white>Alarm zaten aktif durumda.</color>");
        }
    }

    public void StopAlarm()
    {
        if (alarmCoroutine != null)
        {
            StopCoroutine(alarmCoroutine);
            alarmCoroutine = null;
            // Debug.Log("<color=red>[Alarm]</color> <color=white>Alarm durduruldu.</color>");
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        SetLightsState(false);

        if (alarmAudioSource.isPlaying)
        {
            alarmAudioSource.Stop();
            alarmAudioSource.volume = 1f;
            // Debug.Log("<color=red>[Alarm]</color> <color=white>Alarm sesi durduruldu ve ses seviyesi sıfırlandı.</color>");
        }
    }

    private IEnumerator AlarmRoutine()
    {
        if (!alarmAudioSource.isPlaying)
        {
            alarmAudioSource.volume = 1f;
            alarmAudioSource.Play();
            // Debug.Log("<color=cyan>[Alarm]</color> <color=white>Alarm sesi çalmaya başladı.</color>");
        }

        while (true)
        {
            SetLightsState(true);
            yield return new WaitForSeconds(blinkInterval);

            SetLightsState(false);
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator FadeAudioRoutine()
    {
        // Debug.Log($"<color=orange>[Alarm]</color> <color=white>{fadeStartTime} saniye sonra ses azalmaya başlayacak.</color>");
        yield return new WaitForSeconds(fadeStartTime);

        float startVolume = alarmAudioSource.volume;
        float timer = 0f;

        // Debug.Log($"<color=magenta>[Alarm]</color> <color=white>Ses kısılmaya başlıyor... ({fadeDuration} saniye sürecek)</color>");

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            alarmAudioSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        alarmAudioSource.volume = 0f;
        alarmAudioSource.Stop();

        // Debug.Log("<color=magenta>[Alarm]</color> <color=white>Ses tamamen kapandı.</color>");
    }

    private void SetLightsState(bool state)
    {
        foreach (Light light in frontLights)
        {
            if (light != null)
                light.gameObject.SetActive(state);
        }

        foreach (Light light in rearLights)
        {
            if (light != null)
                light.gameObject.SetActive(state);
        }

        // Debug.Log($"<color=blue>[Işık]</color> <color=white>Işıklar {(state ? "açıldı" : "kapandı")}.</color>");
    }
}
