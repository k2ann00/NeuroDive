using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Coroutine kullanmak için bu kütüphaneyi eklemelisin

public class SceneReloadManager : MonoBehaviour
{

    public float delayBeforeReload = 0.5f;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player ile temas oldu. Sahne yeniden yükleme gecikmeli olarak başlatılıyor...");
            
            StartCoroutine(ReloadSceneAfterDelay(delayBeforeReload));

        }
    }
    
    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Gecikme süresi doldu, sahne yeniden yükleniyor.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReloadScene()
    {
        Debug.Log("Sahne yeniden yükleniyor.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadFailedScene()
    {
        Debug.Log("Fail sahnesi yükleniyor.");
        
    }
}