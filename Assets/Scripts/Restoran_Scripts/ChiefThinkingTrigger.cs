using UnityEngine;

public class ChiefThinkingTrigger : MonoBehaviour
{
    [Header("NPC'nin Animator bileşeni (elle atanır)")]
    public Animator npcAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Chief")) return;

        if (npcAnimator != null)
        {
            npcAnimator.SetBool("isThinking", true);
            // Debug.Log("isThinking parametresi true yapıldı.");
        }
        else
        {
            // Debug.LogWarning("npcAnimator atanmadı!");
        }
    }

    // (İsteğe bağlı) Çıkınca kapatmak istersen:
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Chief")) return;

        if (npcAnimator != null)
        {
            npcAnimator.SetBool("isThinking", false);
            // Debug.Log("isThinking parametresi false yapıldı.");
        }
    }
}