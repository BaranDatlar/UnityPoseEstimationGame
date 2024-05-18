using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float maxMovementRange = 30f;
    public float lateralSpeed = 5f;

    private Vector3 targetPosition;

    void Update()
    {
        if (!CameraCalibration.instance.calibrationCompleted) return;

        MoveCharacter(BodyPartAngles.ReturnMidPointOfShoulders());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // X eksenindeki hedef pozisyona doğru yumuşak geçiş
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Lerp(currentPosition.x, targetPosition.x, Time.deltaTime * lateralSpeed);
        transform.position = currentPosition;
    }

    public void MoveCharacter(Vector3 newCoordinate)
    {
        // X eksenindeki koordinata göre hedef pozisyonu hesapla
        float xMovement = Mathf.Clamp(newCoordinate.x, -maxMovementRange, maxMovementRange);

        // Yeni hedef pozisyonu ayarla
        targetPosition = new Vector3(xMovement, transform.position.y, transform.position.z);
    }
}
