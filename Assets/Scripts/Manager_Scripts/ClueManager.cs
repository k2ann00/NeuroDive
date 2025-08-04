using UnityEngine;

public class ClueManager : MonoBehaviour
{
    private bool playerInRange = false;
    private GameObject pressEText;

    private void Start()
    {
        // Use the newer recommended method: use GameObject.Find and cache the reference
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform pressETransform = canvas.transform.Find("Press E (TMP)");
            if (pressETransform != null)
            {
                pressEText = pressETransform.gameObject;
                pressEText.SetActive(false); // Hide at start
            }
            else
            {
                Debug.LogWarning("'Press E (TMP)' not found under Canvas.");
            }
        }
        else
        {
            Debug.LogWarning("'Canvas' not found in the scene.");
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            GameObject countdownManagerObj = GameObject.Find("Countdown Manager");
            if (countdownManagerObj != null)
            {
                CountdownManager countdownManager = countdownManagerObj.GetComponent<CountdownManager>();
                if (countdownManager != null)
                {
                    countdownManager.LevelCompleted();
                }
                else
                {
                    Debug.LogWarning("CountdownManager script not found on 'Countdown Manager'.");
                }
            }
            else
            {
                Debug.LogWarning("'Countdown Manager' not found in the scene.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (pressEText != null)
            {
                pressEText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (pressEText != null)
            {
                pressEText.SetActive(false);
            }
        }
    }
}
