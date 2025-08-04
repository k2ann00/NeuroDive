using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameButtons
{
    public Button button;            // Buton bile�eni
    public Image iconImage;          // �kon g�rseli
    public string iconName;          // �rnek: "eye", "spiral"
    public GameObject glowEffect;    // Parlayan �er�eve objesi (ba�lang��ta kapal� olmal�)

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

