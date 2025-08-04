using UnityEngine;
using System.Collections;

public class NpcSitTrigger : MonoBehaviour
{
    [Tooltip("Npc oturduktan sonra bekleyeceği süre (saniye cinsinden)")]
    public float sitDuration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Npc"))
        {
            Animator enemyAnimator = other.GetComponent<Animator>();

            if (enemyAnimator != null)
            {
                StartCoroutine(HandleSitting(enemyAnimator));
            }
            else
            {
                Debug.LogWarning("Npc objesinde Animator bileşeni bulunamadı.");
            }
        }
    }

    private IEnumerator HandleSitting(Animator animator)
    {
        animator.SetBool("sitdownStarted", true);
        yield return null; // bir frame bekle
        animator.SetBool("sitdownStarted", false);

        animator.SetBool("isSitting", true);
        // Debug.LogWarning("<color=green><b>(NPC: isSitting = True)</b></color>");

        yield return new WaitForSeconds(sitDuration);
        // Debug.LogWarning("<color=blue><b>(NPC: sitDuration ="+sitDuration+")</b></color>");

        animator.SetBool("isSitting", false);
        // Debug.LogWarning("<color=red><b>(NPC: isSitting = False)</b></color>");
    }
    
    
}