using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraCalibration : MonoBehaviour
{
    public Button startCalibration;
    public Button skipCalibration;
    public TextMeshProUGUI countDownText;
    public int countDownValue = 5;
    public float averageYCoordinate;
    public bool calibrationCompleted;

    public static CameraCalibration instance { get; private set; }
    public UnityEvent onCalibrationFinished;
    public Canvas calibrationCanvas;

    private Vector3 startScale = new Vector3(1, 1, 1);

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    void Start()
    {
        startCalibration.onClick.AddListener(() =>
        {
            StartCoroutine(StartCountdown());
        });
        skipCalibration.onClick.AddListener(SkipCalibration);
    }

    IEnumerator ScaleOverTime(float time, TextMeshProUGUI countDownText, Canvas calibrationCanvas)
    {
        float currentTime = 0.0f;
        Color originalCanvasColor = calibrationCanvas.GetComponent<Image>().color;
        while (currentTime <= time)
        {
            countDownText.transform.localScale = Vector3.Lerp(startScale, startScale * 5f, currentTime / time);
            originalCanvasColor.a = Mathf.Lerp(0, 0.5f, currentTime / time);
            calibrationCanvas.GetComponent<Image>().color = new Color(originalCanvasColor.r, originalCanvasColor.g, originalCanvasColor.b, originalCanvasColor.a);
            currentTime += Time.deltaTime;
            yield return null; // Bir sonraki frame'i bekle
        }
        countDownText.transform.localScale = startScale;
        originalCanvasColor.a = 0;
        calibrationCanvas.GetComponent<Image>().color = originalCanvasColor;

    }

    IEnumerator StartCountdown()
    {
        countDownText.gameObject.SetActive(true);
        List<float> YCoordinates = new();
        float totalYCoordinates = 0;

        while (countDownValue > 0)
        {
            if (countDownValue <= 3)
            {
                float currentYCoordinate = BodyPartAngles.ReturnMidPointOfShoulders().y;
                YCoordinates.Add(currentYCoordinate);
            }

            countDownText.text = countDownValue.ToString();
            StartCoroutine(ScaleOverTime(1, countDownText,calibrationCanvas));
            yield return new WaitForSeconds(1);
            countDownValue--;
        }

        countDownText.text = "Start!";

        foreach (var val in YCoordinates)
        {
            totalYCoordinates += val;
        }

        averageYCoordinate = totalYCoordinates / YCoordinates.Count;
        //Debug.Log("Y EKSENİ KALİBRASYON KOORDİNATI " + averageYCoordinate);

        calibrationCompleted = true;
        onCalibrationFinished?.Invoke();

        
        yield return new WaitForSeconds(2);
        countDownText.gameObject.SetActive(false);
    }

    void SkipCalibration()
    {
        calibrationCompleted = true;
        onCalibrationFinished?.Invoke();
        StopAllCoroutines();
    }
}
