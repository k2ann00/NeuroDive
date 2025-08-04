using UnityEngine;
using System.Collections;

public class UIElementSlideInOut : MonoBehaviour
{
    public RectTransform uiElement;

    public float startX = -500f;
    public float targetX = 0f;
    public float delayBeforeIn = 1f;
    public float inDuration = 0.5f;
    public float visibleDuration = 2f;
    public float outDuration = 0.5f;

    void Start()
    {
        Vector2 pos = uiElement.anchoredPosition;
        pos.x = startX;
        uiElement.anchoredPosition = pos;

        StartCoroutine(AnimateUI());
    }

    IEnumerator AnimateUI()
    {
        yield return new WaitForSeconds(delayBeforeIn);
        yield return MoveUI(startX, targetX, inDuration);
        yield return new WaitForSeconds(visibleDuration);
        yield return MoveUI(targetX, startX, outDuration);
    }

    IEnumerator MoveUI(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            Vector2 pos = uiElement.anchoredPosition;
            pos.x = Mathf.Lerp(from, to, t);
            uiElement.anchoredPosition = pos;
            yield return null;
        }
        Vector2 finalPos = uiElement.anchoredPosition;
        finalPos.x = to;
        uiElement.anchoredPosition = finalPos;
    }
}