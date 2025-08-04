using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Sprite frontImage;  // Kartýn ön yüzü
    public Sprite backImage;   // Kartýn arka yüzü

    private Image image;
    private bool isFlipped = false;
    private bool isMatched = false;

    void Start()
    {
        image = GetComponent<Image>();
        ShowBack();
    }

    public void SetImages(Sprite front, Sprite back)
    {
        frontImage = front;
        backImage = back;

        if (image == null)
            image = GetComponent<Image>();

        ShowBack();
    }

    public void Flip()
    {
        // Kart açýk mý ya da eþleþmiþ mi kontrolü
        if (isFlipped || isMatched || !GameManager.Instance.CanFlipCards())
            return;

        isFlipped = true;
        image.sprite = frontImage;

        GameManager.Instance.CardRevealed(this);
    }

    public void ShowBack()
    {
        isFlipped = false;
        image.sprite = backImage;
    }

    public void SetMatched()
    {
        isMatched = true;
    }

    public bool IsFlipped()
    {
        return isFlipped;
    }

    public bool IsMatched()
    {
        return isMatched;
    }
}







