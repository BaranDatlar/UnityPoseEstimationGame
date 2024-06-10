using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float speed = 20f;
    public float maxMovementRange = 30f;
    public float lateralSpeed = 5f;

    private Vector3 targetPosition;

    public float bendThreshold = 20f; // Eğilme eşik değeri
    public float bendAmount = -5f; // Eğilme miktarı (y ekseninde)

    private float currentX;

    private Queue<float> positionHistory = new Queue<float>();
    private int windowSize = 10; // Zaman penceresindeki kare (frame) sayısı
    private float threshold = 0.1f; // Yön değişikliği eşiği

    public Animator animator;

    private bool goStraight;
    private bool goRight;
    private bool goLeft;
    private bool isCrouch;

    public delegate void OnPLayerCrouch(float positionY);
    public static event OnPLayerCrouch OnPLayerCrouchEvent;
    public static void InvokeOnPLayerCrouch(float positionY) { OnPLayerCrouchEvent?.Invoke(positionY); }

    private void Start()
    {
        OnPLayerCrouchEvent += ResetCharacterBend;
    }

    void Update()
    {
        if (!CameraCalibration.instance.calibrationCompleted) return;

        currentX = transform.position.x;

        positionHistory.Enqueue(currentX);
        if (positionHistory.Count > windowSize)
        {
            positionHistory.Dequeue();
        }


        MoveCharacter(BodyPartAngles.ReturnMidPointOfShoulders());
        BendCharacter(BodyPartAngles.ReturnMidPointOfShoulders());

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // X eksenindeki hedef pozisyona doğru yumuşak geçiş
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Lerp(currentPosition.x, targetPosition.x, Time.deltaTime * lateralSpeed);
        transform.position = currentPosition;

        if (positionHistory.Count == windowSize)
        {
            float sum = 0.0f;
            float[] positions = positionHistory.ToArray();

            for (int i = 1; i < positions.Length; i++)
            {
                sum += positions[i] - positions[i - 1];
            }

            float averageChange = sum / (positions.Length - 1);

            if (Mathf.Abs(averageChange) > threshold)
            {
                if (averageChange > 0)
                {
                    // Sağa dönüyor
                    goRight = true;
                    goLeft = false;
                    goStraight= false;
                    //Debug.Log("Obje sağa dönüyor");
                }
                else
                {
                    // Sola dönüyor
                    goRight = false;
                    goLeft = true;
                    goStraight = false;
                    //Debug.Log("Obje sola dönüyor");
                }
            }
            else
            {
                if (isCrouch)
                {
                    goRight = false;
                    goLeft = false;
                    goStraight = false;
                }
                else
                {
                    goRight = false;
                    goLeft = false;
                    goStraight = true;
                }
                
                //Debug.Log("Obje düz gidiyor");
            }
        }


        //animator.SetFloat("Direction", targetPosition.x / maxMovementRange);
        animator.SetFloat("Speed", speed);
        animator.SetBool("GoRight", goRight);
        animator.SetBool("GoLeft", goLeft);
        animator.SetBool("GoStraight", goStraight);

        //Debug.Log("DİRECTİON " + targetPosition.x / maxMovementRange);
    }

    public void MoveCharacter(Vector3 newCoordinate)
    {
        // X eksenindeki koordinata göre hedef pozisyonu hesapla
        float xMovement = Mathf.Clamp(newCoordinate.x, -maxMovementRange, maxMovementRange);

        // Yeni hedef pozisyonu ayarla
        targetPosition = new Vector3(xMovement, transform.position.y, transform.position.z);
    }

    public void BendCharacter(Vector3 newCoordinate)
    {
        //Debug.Log("AVERAGE Y COORDİNATE " + CameraCalibration.instance.averageYCoordinate);
        float bendingYCoordinate = CameraCalibration.instance.averageYCoordinate - bendThreshold;
        Vector3 currentPosition = transform.position;

        if (newCoordinate.y < bendingYCoordinate)
        {
            // Eğilme pozisyonuna ayarla (y ekseninde eğilme)
            isCrouch = true;
            //CameraFollow.instance.CameraZoomIn();
            currentPosition.y = bendAmount;
            animator.SetBool("IsCrouching", true);
            InvokeOnPLayerCrouch(currentPosition.y);
        }
        else
        {
            // Dik pozisyona ayarla
            isCrouch = false;
            //CameraFollow.instance.CameraZoomOut();
            currentPosition.y = 0;
            animator.SetBool("IsCrouching", false);
        }
        transform.position = currentPosition;
    }
     
    private void ResetCharacterBend(float currentPosition)
    {
        Timer.instance.SetTimer(5f, () =>
        {
            //animator.SetBool("GoStraight", true);
            //CameraFollow.instance.CameraZoomOut();
            //currentPosition = 0;
            Debug.Log("5 saniye geçti");
        });
    }
}
