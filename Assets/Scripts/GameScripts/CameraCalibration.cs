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
    public int countDownValue = 10;
    public float averageYCoordinate;
    public bool calibrationCompleted;

    public static CameraCalibration instance { get; private set; }
    public UnityEvent onCalibrationFinished;

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

    IEnumerator StartCountdown()
    {
        countDownText.gameObject.SetActive(true);
        List<float> YCoordinates = new();
        float totalYCoordinates = 0;

        while (countDownValue > 0)
        {
            if (countDownValue <= 5)
            {
                float currentYCoordinate = BodyPartAngles.ReturnMidPointOfShoulders().y;
                YCoordinates.Add(currentYCoordinate);
            }

            countDownText.text = countDownValue.ToString();
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
