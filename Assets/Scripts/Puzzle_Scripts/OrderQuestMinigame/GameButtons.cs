using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameButtons
{
    public Button button;            // Buton bileþeni
    public Image iconImage;          // Ýkon görseli
    public string iconName;          // Örnek: "eye", "spiral"
    public GameObject glowEffect;    // Parlayan çerçeve objesi (baþlangýçta kapalý olmalý)

    public void TurnOnGlow()
    {
        if (glowEffect != null)
            glowEffect.SetActive(true);
    }

    public void TurnOffGlow()
    {
        if (glowEffect != null)
            glowEffect.SetActive(false);
    }
}

