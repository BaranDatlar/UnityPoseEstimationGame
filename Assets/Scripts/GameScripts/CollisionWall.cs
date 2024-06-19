using UnityEngine;
using UnityEngine.Events;

public class CollisionWall : MonoBehaviour
{
    public UnityEvent onPlayerCollision;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onPlayerCollision.Invoke();
            Debug.Log("DEĞDİ");
        }
    }
}
