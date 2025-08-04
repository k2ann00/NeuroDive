using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PhotoStackViewer : MonoBehaviour
{
    [Header("Destroy Edilecek DontDestroy Elemanları")]
    public GameObject persistentCanvas;
    public GameObject countdownManager;
    
    [Header("UI Referansları")]
    public RectTransform photoContainer;
    public Image photoTemplate;
    public Button nextButton;
    public GameObject winPanel;
    
    [Header("Next Button Sprite Değişimi")]
    public Sprite normalNextSprite;
    public Sprite finalNextSprite;

    [Header("Text Panel Sistemi")]
    public RectTransform textContainer; // Vertical Layout Group içeren container
    public GameObject textPrefab;       // İçinde TitleText ve BodyText olan prefab
    public float textTypingSpeed = 0.05f;

    [Header("Veriler")]
    public List<Sprite> photoSprites;
    public List<string> photoTitles;
    [TextArea] public List<string> photoBodies;

    [Header("Yığın Efekti")]
    public float stackOffsetY = -15f;
    public float stackScaleFactor = 0.92f;
    public Vector2 stackRandomPosXRange = new Vector2(-15f, 15f);
    public Vector2 stackRandomPosYRange = new Vector2(-15f, 15f);
    public Vector2 stackRandomRotZRange = new Vector2(-15f, 15f);

    [Header("Animasyon Ayarları")]
    public float photoInDuration = 0.8f;
    public float photoInFromX = 800f;

    private int currentIndex = -1;
    private Image activePhoto;
    private List<StackedPhoto> photoStack = new List<StackedPhoto>();

    [Header("Win Sonrası Yüklenecek Sahne")]
    public string sceneName;
    
    class StackedPhoto
    {
        public Image image;
        public Vector3 position;
        public float rotationZ;
        public float scale;
    }

    void Start()
    {
        nextButton.onClick.AddListener(ShowNextPhoto);
        photoTemplate.gameObject.SetActive(false);
        ShowNextPhoto();
    }

    void ShowNextPhoto()
    {
        currentIndex++;
        
        if (currentIndex >= photoSprites.Count)
        {
            return; // Safety check: don't proceed if out of bounds
        }
        
        // Mevcut renk ayarlarını koru
        ColorBlock originalColors = nextButton.colors;
        
        if (currentIndex == photoSprites.Count - 1)
        {
            // Buton resmini ve işlevini değiştir
            nextButton.image.sprite = finalNextSprite;
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() => LoadNextLevel());
        }
        else
        {
            // İlk yüklemede veya ortadaki fotolarda normal sprite kullan
            nextButton.image.sprite = normalNextSprite;
        }
        
        // Renk bloklarını eski haline getir (highlight dahil)
        nextButton.colors = originalColors;


        // Aktif fotoğrafı yığına gönder
        if (activePhoto != null)
        {
            int stackIndex = photoStack.Count;

            float randomX = Random.Range(stackRandomPosXRange.x, stackRandomPosXRange.y);
            float randomY = Random.Range(stackRandomPosYRange.x, stackRandomPosYRange.y);
            float randomZ = Random.Range(stackRandomRotZRange.x, stackRandomRotZRange.y);
            float scale = Mathf.Pow(stackScaleFactor, stackIndex + 1);

            Vector3 targetPos = new Vector3(randomX, (stackIndex * stackOffsetY) + randomY, 0);

            var stackInfo = new StackedPhoto
            {
                image = activePhoto,
                position = targetPos,
                rotationZ = randomZ,
                scale = scale
            };
            photoStack.Add(stackInfo);

            activePhoto.transform.SetAsLastSibling();
            activePhoto.rectTransform.DOLocalMove(stackInfo.position, 0.4f);
            activePhoto.rectTransform.DOLocalRotate(new Vector3(0, 0, stackInfo.rotationZ), 0.4f);
            activePhoto.rectTransform.DOScale(stackInfo.scale, 0.4f);
        }

        // Yeni fotoğrafı oluştur ve sahneye getir
        Image newPhoto = Instantiate(photoTemplate, photoContainer);
        newPhoto.sprite = photoSprites[currentIndex];
        newPhoto.gameObject.SetActive(true);

        RectTransform rt = newPhoto.rectTransform;
        rt.localScale = Vector3.one * 1.3f;
        rt.localRotation = Quaternion.identity;
        rt.localPosition = new Vector3(photoInFromX, 0, 0);

        newPhoto.transform.SetAsLastSibling(); // en öne al

        Sequence photoSeq = DOTween.Sequence();
        photoSeq.Append(rt.DOLocalMove(Vector3.zero, photoInDuration).SetEase(Ease.OutCubic));
        photoSeq.Join(rt.DOScale(1f, photoInDuration).SetEase(Ease.OutBack));

        activePhoto = newPhoto;

        // Yeni açıklama panelini oluştur
        GameObject newTextPanel = Instantiate(textPrefab, textContainer);
        TextMeshProUGUI titleTMP = newTextPanel.transform.Find("TitleText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI bodyTMP = newTextPanel.transform.Find("BodyText").GetComponent<TextMeshProUGUI>();

        titleTMP.text = photoTitles[currentIndex];
        titleTMP.alpha = 0f;
        titleTMP.DOFade(1f, 0.5f); // başlık fade-in

        bodyTMP.text = "";
        StartCoroutine(TypeBodyText(photoBodies[currentIndex], bodyTMP));
    }

    System.Collections.IEnumerator TypeBodyText(string text, TextMeshProUGUI target)
    {
        foreach (char c in text)
        {
            target.text += c;
            yield return new WaitForSeconds(textTypingSpeed);
        }
    }


    public void LoadNextLevel()
    {
        winPanel.SetActive(false);
        Time.timeScale = 1f;
        if (persistentCanvas != null)
            Destroy(persistentCanvas);
        if (countdownManager != null)
            Destroy(countdownManager);
        SceneManager.LoadScene(sceneName);
    }
    
}
