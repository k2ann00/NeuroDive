using UnityEngine;
using UnityEngine.SceneManagement;


public class afterpuzzlesceneloader : MonoBehaviour
{
  public void LoadCinemaInMind()
  {
    SceneManager.UnloadSceneAsync("MemoryPuzzle"); 
  }
}
