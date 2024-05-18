using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraCalibration : MonoBehaviour
{
    public Button startCalibration;
    public TextMeshProUGUI countDownText;
    public int countDownValue = 10;
    public bool calibrationCompleted;

    public static CameraCalibration instance;
    public UnityEvent onCalibrationFinished;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        startCalibration.onClick.AddListener(() =>
        {
            StartCoroutine(StartCountdown());
        });
    }

    
    IEnumerator StartCountdown()
    {
        while (countDownValue > 0)
        {
            countDownText.text = countDownValue.ToString();
            yield return new WaitForSeconds(1);
            countDownValue--;
        }
        countDownText.text = "Start!";
        calibrationCompleted = true;
        onCalibrationFinished?.Invoke();

        yield return new WaitForSeconds(2);
        countDownText.gameObject.SetActive(false);
    }
}
