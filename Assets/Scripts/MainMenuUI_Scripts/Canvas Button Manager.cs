using UnityEngine;
using UnityEngine.SceneManagement;
public class CanvasButtonManager : MonoBehaviour
{
    public GameObject persistentCanvas;
    public GameObject countdownManager;
    
    
    public void LoadMainMenu()
    {
        if (persistentCanvas != null)
            Destroy(persistentCanvas);

        if (countdownManager != null)
            Destroy(countdownManager);
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void LoadRestoran()
    {
        if (persistentCanvas != null)
            Destroy(persistentCanvas);

        if (countdownManager != null)
            Destroy(countdownManager);
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("Restoran");
    }
    
    public void LoadCinema()
    {
        if (countdownManager != null)
            Destroy(countdownManager);
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("Cinema");
        
        if (persistentCanvas != null)
            Destroy(persistentCanvas);
    }
    
    
}
