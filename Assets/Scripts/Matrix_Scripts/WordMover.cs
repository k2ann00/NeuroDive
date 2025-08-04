using UnityEngine;

public class WordMover : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}