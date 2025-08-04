using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlideDoorHandler : MonoBehaviour
{
    public Transform movingObject;    // Duvar + tablo objesi
    public float targetZ = 5f;        // Girince ulaþacaðý Z deðeri
    public float moveDuration = 1f;   // Hareketin süresi (saniye)

    private float originalZ;
    private Coroutine moveCoroutine;

    void Start()
    {
        if (movingObject != null)
            originalZ = movingObject.position.z;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GiseciNPC.Instance != null && GiseciNPC.Instance.dialogueStarted)
            {
                // Önceki coroutine’i durdur (üst üste binmesin)
                if (moveCoroutine != null)
                    StopCoroutine(moveCoroutine);

                moveCoroutine = StartCoroutine(MoveZValue(targetZ));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GiseciNPC.Instance != null && GiseciNPC.Instance.dialogueStarted)
            { 
                if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

                moveCoroutine = StartCoroutine(MoveZValue(originalZ));
            }
        }
    }

    IEnumerator MoveZValue(float newZ)
    {
        float elapsed = 0f;
        Vector3 startPos = movingObject.position;
        Vector3 endPos = new Vector3(startPos.x, startPos.y, newZ);

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            movingObject.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Tam olarak hedef pozisyona otur
        movingObject.position = endPos;
    }
}
