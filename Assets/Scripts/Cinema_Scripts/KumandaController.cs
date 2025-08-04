using UnityEngine;

public class KumandaController : MonoBehaviour
{
    public bool IsControlTaken = false;
    public bool IsLightsDown = false;

    public GameObject Light_1;
    public GameObject Light_2;
    public GameObject PlayerLight;

    public GameObject enemy;
    public EnemyDetectionLight2 enemyDetection;
    void Start()
    {
        enemyDetection = enemy.GetComponent<EnemyDetectionLight2>();
        enemyDetection.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsControlTaken && Input.GetKeyDown(KeyCode.E))
        {
            if (!IsLightsDown)
            {
                Light_1.SetActive(false);
                Light_2.SetActive(false);
                PlayerLight.SetActive(true);
                enemyDetection.enabled = true;
                IsLightsDown = true;
            }
            else if (IsLightsDown)
            {
                Light_1.SetActive(true);
                Light_2.SetActive(true);
                PlayerLight.SetActive(false);
                enemyDetection.enabled = false;
                IsLightsDown = false;
            }
        }
    }
}
