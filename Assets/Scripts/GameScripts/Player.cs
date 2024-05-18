using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f; // Player'ın hareket hızı

    void Update()
    {
        // Player'ı Z ekseninde sürekli ileri hareket ettir
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
