using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GifUIPlayer : MonoBehaviour
{
    public Image imageComponent;
    public Sprite[] frames;
    public float frameRate = 24f;

    IEnumerator Start()
    {
        int frameCount = frames.Length;
        int current = 0;
        float delay = 1f / frameRate;

        while (true)
        {
            imageComponent.sprite = frames[current];
            current = (current + 1) % frameCount;
            yield return new WaitForSeconds(delay);
        }
    }
}