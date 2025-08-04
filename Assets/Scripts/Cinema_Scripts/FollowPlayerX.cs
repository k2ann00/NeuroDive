using UnityEngine;

public class FollowPlayerX : MonoBehaviour
{
    public Transform player;      // Player’ýn Transform referansý
    private float offsetZ = -5.8f;    // Oyuncudan ne kadar yanda dursun

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = player.position.x;
        pos.z = offsetZ;
        transform.position = pos;
    }
}
