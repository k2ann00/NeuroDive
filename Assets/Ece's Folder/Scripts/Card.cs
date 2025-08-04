using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Sprite frontImage;  // Kart�n �n y�z�
    public Sprite backImage;   // Kart�n arka y�z�

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
        // Kart a��k m� ya da e�le�mi� mi kontrol�
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







