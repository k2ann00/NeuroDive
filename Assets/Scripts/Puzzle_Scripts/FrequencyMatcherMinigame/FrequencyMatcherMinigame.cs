using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FrequencyManager : MonoBehaviour
{
    [Header("Images")]
    public RawImage[] targetFrequencyImages;   // Alttaki sabit frekanslar
    public RawImage[] currentFrequencyImages;  // �stte de�i�en frekanslar

    [Header("Frequencies")]
    public Texture[] frequencyTextures;        // T�m frekans resimleri (3 farkl� olmal�)

    [Header("Feedback")]
    public RawImage[] feedbackImages;         // Feedback i�in RawImage'ler
    public Texture tickTexture;               // Inspector'a s�r�klenecek Texture
    public Texture crossTexture;              // Inspector'a s�r�klenecek Texture

    [Header("UI")]
    public Button completeButton;

    private int[] targetIndexes;
    private int[] currentIndexes;
    private bool[] isMatched;

    void Start()
    {
        int count = targetFrequencyImages.Length;

        targetIndexes = new int[count];
        currentIndexes = new int[count];
        isMatched = new bool[count];

        completeButton.gameObject.SetActive(false);

        // Sabit ve benzersiz hedef frekanslar (0, 1, 2)
        for (int i = 0; i < count; i++)
        {
            targetIndexes[i] = i % frequencyTextures.Length;
            targetFrequencyImages[i].texture = frequencyTextures[targetIndexes[i]];

            // Ba�lang��ta farkl� frekans ata
            do
            {
                currentIndexes[i] = Random.Range(0, frequencyTextures.Length);
            } while (currentIndexes[i] == targetIndexes[i]);

            currentFrequencyImages[i].texture = frequencyTextures[currentIndexes[i]];
            feedbackImages[i].enabled = false;
            isMatched[i] = false;
        }
    }

    public void OnRightButton(int index)
    {
        if (isMatched[index]) return;

        currentIndexes[index] = (currentIndexes[index] + 1) % frequencyTextures.Length;
        currentFrequencyImages[index].texture = frequencyTextures[currentIndexes[index]];
        feedbackImages[index].enabled = false;
    }

    public void OnLeftButton(int index)
    {
        if (isMatched[index]) return;

        currentIndexes[index]--;
        if (currentIndexes[index] < 0)
            currentIndexes[index] = frequencyTextures.Length - 1;

        currentFrequencyImages[index].texture = frequencyTextures[currentIndexes[index]];
        feedbackImages[index].enabled = false;
    }

    public void OnConfirmButton(int index)
    {
        if (isMatched[index]) return;

        if (currentIndexes[index] == targetIndexes[index])
        {
            feedbackImages[index].texture = tickTexture;
            feedbackImages[index].enabled = true;
            isMatched[index] = true;
            CheckAllMatched();
        }
        else
        {
            feedbackImages[index].texture = crossTexture;
            feedbackImages[index].enabled = true;
            StartCoroutine(ResetFeedback(index));
        }
    }

    IEnumerator ResetFeedback(int index)
    {
        yield return new WaitForSeconds(1f);
        if (!isMatched[index])
        {
            feedbackImages[index].enabled = false;
        }
    }

    void CheckAllMatched()
    {
        foreach (bool matched in isMatched)
        {
            if (!matched) return;
        }
        completeButton.gameObject.SetActive(true);
    }

    public void OnCompleteButton()
    {
        SceneManager.UnloadSceneAsync("FrequencyMatcherMinigame");
    }
}