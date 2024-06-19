using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 20f;
    public float maxMovementRange = 30f;
    public float lateralSpeed = 5f;

    private Vector3 targetPosition;
    private Rigidbody rb;

    public float bendThreshold = 20f; // Eğilme eşik değeri
    public float bendAmount = -5f; // Eğilme miktarı (y ekseninde)

    private float currentX;

    private Queue<float> positionHistory = new Queue<float>();
    private int windowSize = 10; // Zaman penceresindeki kare (frame) sayısı
    private float threshold = 0.1f; // Yön değişikliği eşiği

    public Animator animator;

    private Material playerMaterial;
    private Color originalColor;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private bool isImmortal = false;
    public float immortalityDuration = 8f; // Ölümsüzlük süresi
    public float blinkInterval = 0.1f; // Yanıp sönme aralığı

    private bool goStraight;
    private bool goRight;
    private bool goLeft;
    private bool isCrouch;
    private bool isCrashRock;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        skinnedMeshRenderer = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            playerMaterial = skinnedMeshRenderer.material;
            originalColor = playerMaterial.color;
            SetMaterialTransparent(playerMaterial);
        }
    }

    private void SetMaterialTransparent(Material material)
    {
        material.SetFloat("_Mode", 2);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
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

        // X eksenindeki hedef pozisyona doğru yumuşak geçiş
        Vector3 currentPosition = rb.position;
        currentPosition.x = Mathf.Lerp(currentPosition.x, targetPosition.x, Time.deltaTime * lateralSpeed);
        rb.MovePosition(currentPosition + Vector3.forward * speed * Time.deltaTime);

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
                    goRight = true;
                    goLeft = false;
                    goStraight = false;
                }
                else
                {
                    goRight = false;
                    goLeft = true;
                    goStraight = false;
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
            }
        }

        animator.SetFloat("Speed", speed);
        animator.SetBool("GoRight", goRight);
        animator.SetBool("GoLeft", goLeft);
        animator.SetBool("GoStraight", goStraight);
    }

    public void MoveCharacter(Vector3 newCoordinate)
    {
        float xMovement = Mathf.Clamp(newCoordinate.x, -maxMovementRange, maxMovementRange);
        targetPosition = new Vector3(xMovement, rb.position.y, rb.position.z);
    }

    public void BendCharacter(Vector3 newCoordinate)
    {
        float bendingYCoordinate = CameraCalibration.instance.averageYCoordinate - bendThreshold;
        Vector3 currentPosition = rb.position;

        if (newCoordinate.y < bendingYCoordinate)
        {
            isCrouch = true;
            currentPosition.y = bendAmount;
            animator.SetBool("IsCrouching", true);
        }
        else
        {
            isCrouch = false;
            currentPosition.y = 0;
            animator.SetBool("IsCrouching", false);
        }
        rb.MovePosition(currentPosition);
    }

    private IEnumerator ProtectCharacterAfterCrash()
    {
        isImmortal = true;
        float elapsedTime = 0f;

        rb.isKinematic = false;
        isCrashRock = false;
        speed = 20f;
        animator.SetBool("IsCrash", false);

        Debug.Log("Protect girdi");

        while (elapsedTime < immortalityDuration)
        {
            float lerp = Mathf.PingPong(Time.time, blinkInterval) / blinkInterval;
            Color color = originalColor;
            color.a = Mathf.Lerp(0.2f, 1f, lerp); // Alpha değeri 0.2 ile 1 arasında değişsin
            playerMaterial.color = color;

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        playerMaterial.color = originalColor; // Orijinal rengi geri getir
        isImmortal = false;
        Debug.Log("Protect çıktı");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("rock"))
        {
            if (!isImmortal)
            {
                rb.isKinematic = true;
                animator.SetBool("IsCrash", true);
                speed = 0f;
            }

            isCrashRock = true;           
            animator.SetFloat("Speed", speed);
            Debug.Log("Engele çarptı ve durdu!");
            Timer.instance.SetTimer(3f, () =>
            {
                StartCoroutine(ProtectCharacterAfterCrash());
            });
        }
    }
}
