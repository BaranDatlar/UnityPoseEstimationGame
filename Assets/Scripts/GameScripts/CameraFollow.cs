using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Vector3 _currentOffset;

    public static CameraFollow instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        _currentOffset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        transform.position = player.position + _currentOffset;
        transform.LookAt(player);
    }

}
