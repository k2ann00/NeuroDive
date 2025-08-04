using UnityEngine;
using System.Collections;

public class EnemySitTrigger : MonoBehaviour
{
    [Tooltip("Enemy oturduktan sonra bekleyeceği süre (saniye cinsinden)")]
    public float sitDuration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Animator enemyAnimator = other.GetComponent<Animator>();

            if (enemyAnimator != null)
            {
                StartCoroutine(HandleSitting(enemyAnimator));
            }
            else
            {
                Debug.LogWarning("Enemy objesinde Animator bileşeni bulunamadı.");
            }
        }
    }

    private IEnumerator HandleSitting(Animator animator)
    {
        animator.SetBool("sitdownStarted", true);
        yield return null; // bir frame bekle
        animator.SetBool("sitdownStarted", false);

        animator.SetBool("isSitting", true);
        // Debug.LogWarning("<color=green><b>(ENEMY: isSitting = True)</b></color>");
        
        yield return new WaitForSeconds(sitDuration);
        // Debug.LogWarning("<color=blue><b>(ENEMY: sitDuration ="+sitDuration+")</b></color>");

        animator.SetBool("isSitting", false);
        // Debug.LogWarning("<color=red><b>(ENEMY: isSitting = False)</b></color>");
        
        
    }
}