using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Kart Ayarlarý")]
    public GameObject cardPrefab;
    public Transform cardContainer;
    public Sprite backSprite;
    public List<Sprite> frontSprites; // 5 farklý ön yüz sprite'ý
    public GameObject continueButton; // Continue butonu, baþlangýçta kapalý olmalý

    private Card firstCard, secondCard;
    private bool canClick = true;
    private int matchCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        CreateCards();

        if (continueButton != null)
            continueButton.SetActive(false);
    }

    void CreateCards()
    {
        List<Sprite> cardImages = new List<Sprite>();

        // Her görselden 2 tane olacak þekilde listeye ekle
        foreach (var sprite in frontSprites)
        {
            cardImages.Add(sprite);
            cardImages.Add(sprite);
        }

        // Karýþtýr
        for (int i = 0; i < cardImages.Count; i++)
        {
            Sprite temp = cardImages[i];
            int rand = Random.Range(i, cardImages.Count);
            cardImages[i] = cardImages[rand];
            cardImages[rand] = temp;
        }

        // Kartlarý Instantiate et
        foreach (var sprite in cardImages)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            Card card = cardObj.GetComponent<Card>();
            card.SetImages(sprite, backSprite);
        }
    }

    public void CardRevealed(Card card)
    {
        if (!canClick)
            return;

        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null && card != firstCard)
        {
            secondCard = card;
            canClick = false;  // Kartlar açýkken yeni kart açýlmasýn
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1f);

        if (firstCard.frontImage == secondCard.frontImage)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();
            matchCount++;

            if (matchCount >= frontSprites.Count)
            {
                if (continueButton != null)
                    continueButton.SetActive(true);
            }
        }
        else
        {
            firstCard.ShowBack();
            secondCard.ShowBack();
        }

        firstCard = null;
        secondCard = null;
        canClick = true;
    }

    public bool CanFlipCards()
    {
        return canClick;
    }
}
