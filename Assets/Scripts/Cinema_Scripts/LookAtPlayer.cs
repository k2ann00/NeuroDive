using System;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform playerTransform;



    void Update()
    {
        if (playerTransform != null)
        {
            LockOnPlayer();
        }
    }


            
    private void LockOnPlayer() => gameObject.transform.LookAt(playerTransform);
}
