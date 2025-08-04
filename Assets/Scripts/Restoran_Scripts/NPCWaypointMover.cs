using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Waypoint
{
    public Transform point;
    public float waitTime = 0f;
    public Transform lookTarget; // 🔁 Waypoint'e ulaşıldığında bakılacak hedef (isteğe bağlı)
}

public class NPCWaypointMover : MonoBehaviour
{
    public List<Waypoint> waypoints = new List<Waypoint>();
    public float moveSpeed = 2f;
    public float stoppingDistance = 0.1f;

    private int currentWaypointIndex = 0;

    private Animator animator;
    private Vector3 lastPosition;

    void Start()
    {
        animator = GetComponentInChildren<Animator>(); // Animator alt objede olabilir
        lastPosition = transform.position;

        if (waypoints.Count > 0)
            StartCoroutine(MoveToNextWaypoint());
    }

    void Update()
    {
        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;

        if (animator != null)
            animator.SetFloat("Speed", speed); // Animator parametresine hız aktarımı

        lastPosition = transform.position;
    }

    IEnumerator MoveToNextWaypoint()
    {
        while (true)
        {
            if (waypoints.Count == 0)
                yield break;

            Waypoint targetWaypoint = waypoints[currentWaypointIndex];

            // Y ekseni sabitlenmiş pozisyon (havalanma engellenir)
            Vector3 targetPos = new Vector3(
                targetWaypoint.point.position.x,
                transform.position.y,
                targetWaypoint.point.position.z
            );

            while (Vector3.Distance(transform.position, targetPos) > stoppingDistance)
            {
                Vector3 dir = (targetPos - transform.position).normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 5f * Time.deltaTime);
                yield return null;
            }

            // Waypoint'e varınca belirli bir yöne döndür
            if (targetWaypoint.lookTarget != null)
            {
                Vector3 lookDirection = targetWaypoint.lookTarget.position - transform.position;
                lookDirection.y = 0f; // Dikey yönü yok say
                if (lookDirection != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = lookRotation;
                }
            }

            // ⏳ Bekleme süresi varsa bekle
            if (targetWaypoint.waitTime > 0f)
                yield return new WaitForSeconds(targetWaypoint.waitTime);

            // ➕ Sonraki waypoint'e geç
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }
}
