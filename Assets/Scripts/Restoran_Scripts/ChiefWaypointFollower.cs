using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChiefWaypointFollower : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 3.0f;
    public float rotationSpeed = 10f;
    public float stopDistance = 0.1f;

    private int currentWaypointIndex = 0;
    private Animator animator;

    private bool shouldMove = false; // 🔧 Başlangıçta hareket etmesin

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!shouldMove) return; // Dışarıdan tetiklenmediyse çık

        if (waypoints.Length == 0 || currentWaypointIndex >= waypoints.Length)
        {
            animator.SetFloat("speed", 0f);
            shouldMove = false; // Takip bittiğinde durdur
            return;
        }

        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            // Yöne doğru döndürme
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Hareket
            Vector3 movement = direction.normalized * speed * Time.deltaTime;
            transform.position += movement;

            // Animator Speed ayarı
            animator.SetFloat("speed", movement.magnitude / Time.deltaTime);
        }
        else
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                animator.SetFloat("speed", 0f);
                shouldMove = false;
            }
        }
    }

    // 🎮 Hareketi başlatmak için dışarıdan çağırılacak fonksiyon
    public void StartMoving()
    {
        currentWaypointIndex = 0; // (Opsiyonel) baştan başlasın
        shouldMove = true;
    }
}
