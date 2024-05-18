using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionWall : MonoBehaviour
{
    public UnityEvent onPlayerCollision;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player ile çarpışma olduğunda event'i tetikle
            Debug.Log("ÇARPIŞMA");
            onPlayerCollision.Invoke();
        }
    }
}
