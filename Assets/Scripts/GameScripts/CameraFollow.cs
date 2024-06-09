using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; 
    public Vector3 offset;
    public Vector3 initialPos;
    public Vector3 targetOffset;

    public static CameraFollow instance { get; private set; }
    private bool _isCameraZoomIn;

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    public void CameraZoomIn()
    {
        _isCameraZoomIn = true;
        var targetPos = new Vector3(transform.position.x, offset.y - 10, transform.position.z);
        Vector3.MoveTowards(transform.position, targetPos, 3f * Time.deltaTime);
    }

    public void CameraZoomOut()
    {
        _isCameraZoomIn = false;
        Vector3.MoveTowards(transform.position, initialPos, 3f * Time.deltaTime);
    }

    void Start()
    {
        initialPos = transform.position;
        offset = transform.position - player.position;
        targetOffset = new Vector3(offset.x, offset.y, offset.z + 0.1f);
    }

    void LateUpdate()
    {
        if (_isCameraZoomIn) transform.position = player.position + targetOffset;
        else transform.position = player.position + offset;
        transform.LookAt(player);
    }
}
