using UnityEngine;
using UnityEngine.UI;

public class ImageSpriteSwitcher : MonoBehaviour
{
    [Header("Atanacak Sprite'lar (2 adet)")]
    public Sprite sprite1;
    public Sprite sprite2;

    [Header("Değişim Süresi (saniye)")]
    public float switchInterval = 1f;

    private Image imageComponent;
    private float timer;
    private bool usingFirstSprite = true;

    void Start()
    {
        imageComponent = GetComponent<Image>();

        if (imageComponent == null)
        {
            Debug.LogError("Bu scriptin bulunduğu objede Image component yok.");
            enabled = false;
            return;
        }

        if (sprite1 == null || sprite2 == null)
        {
            Debug.LogError("Lütfen iki sprite'ı da Inspector üzerinden atayın.");
            enabled = false;
            return;
        }

        imageComponent.sprite = sprite1;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            ToggleSprite();
            timer = 0f;
        }
    }

    void ToggleSprite()
    {
        if (usingFirstSprite)
        {
            imageComponent.sprite = sprite2;
        }
        else
        {
            imageComponent.sprite = sprite1;
        }

        usingFirstSprite = !usingFirstSprite;
    }
}