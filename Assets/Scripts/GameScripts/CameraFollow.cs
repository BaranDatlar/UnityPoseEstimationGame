using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float zoomInDistance = 2.0f; // Kameranın yaklaşacağı mesafe
    public float zoomSpeed = 5.0f; // Yaklaşma ve uzaklaşma hızı

    public static CameraFollow instance { get; private set; }
    private Vector3 _originalOffset;
    private Vector3 _currentOffset;
    private bool _isZoomingIn = false;
    private bool _isZoomingOut = false;

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
        _originalOffset = offset = transform.position - player.position;
        _currentOffset = offset;
    }

    private void LateUpdate()
    {
        if (_isZoomingIn)
        {
            _currentOffset = Vector3.Lerp(_currentOffset, _originalOffset - _originalOffset.normalized * zoomInDistance, Time.deltaTime * zoomSpeed);
            if (Vector3.Distance(_currentOffset, _originalOffset - _originalOffset.normalized * zoomInDistance) < 0.1f)
            {
                _currentOffset = _originalOffset - _originalOffset.normalized * zoomInDistance;
                _isZoomingIn = false;
            }
        }
        else if (_isZoomingOut)
        {
            _currentOffset = Vector3.Lerp(_currentOffset, _originalOffset, Time.deltaTime * zoomSpeed);
            if (Vector3.Distance(_currentOffset, _originalOffset) < 0.1f)
            {
                _currentOffset = _originalOffset;
                _isZoomingOut = false;
            }
        }

        transform.position = player.position + _currentOffset;
        transform.LookAt(player);
    }

    public void CameraZoomIn()
    {
        _isZoomingIn = true;
        _isZoomingOut = false;
    }

    public void CameraZoomOut()
    {
        _isZoomingOut = true;
        _isZoomingIn = false;
    }
}
