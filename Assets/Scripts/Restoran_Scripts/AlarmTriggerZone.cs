using UnityEngine;

public class AlarmTriggerZone : MonoBehaviour
{
    [Tooltip("Alarmın bağlı olduğu nesne (CarAlarmController component içermeli)")]
    public CarAlarmController alarmController;

    public GameObject pressEText;

    private bool playerInside = false;

    public ChiefWaypointFollower chiefWaypointFollower;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            // Debug.Log("E tuşuna basıldı. Alarm tetikleniyor...");

            if (alarmController != null)
            {
                alarmController.TriggerAlarm();
                // Debug.Log("Alarm başarıyla tetiklendi.");
            }
            else
            {
                // Debug.LogWarning("AlarmController atanmadı! Alarm tetiklenemedi.");
            }

            // Debug.LogWarning("Şef tetiklendi");
            ChiefAlerted();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pressEText.SetActive(true);
            playerInside = true;
            // Debug.Log("Oyuncu alarm alanına girdi.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pressEText.SetActive(false);
            playerInside = false;
            // Debug.Log("Oyuncu alarm alanından çıktı.");
        }
    }
    
    private void ChiefAlerted()
    {
        // Debug.Log("Şef yola çıktı");
        chiefWaypointFollower.StartMoving();
    }
}