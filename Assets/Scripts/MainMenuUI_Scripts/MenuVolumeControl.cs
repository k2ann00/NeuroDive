using UnityEngine;
using UnityEngine.UI;

public class MenuVolumeControl : MonoBehaviour
{
    [Header("Ses Kaynağı (Audio Source)")]
    public AudioSource targetAudioSource;

    [Header("UI Slider")]
    public Slider volumeSlider;

    private void Start()
    {
        if (targetAudioSource != null && volumeSlider != null)
        {
            // Başlangıçta slider'ı ses seviyesine eşitle
            volumeSlider.value = targetAudioSource.volume;

            // Slider değiştikçe sesi güncelle
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
        }
    }

    private void UpdateVolume(float newVolume)
    {
        if (targetAudioSource != null)
        {
            targetAudioSource.volume = newVolume;
        }
    }
}