using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public int score = 0;
    public int highScore = 0;

    public static Score instance;

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
        highScore = PlayerPrefs.GetInt("highScore");
        highScoreText.text = "HighScore : " + highScore.ToString();
    }

    private void OnDestroy()
    {
        SaveScore();
    }

    public void IncreaseScore()
    {
        score += 1;
        scoreText.text = "Score : " + score.ToString();
    }

    public void SaveScore()
    {
        if(score > highScore) PlayerPrefs.SetInt("highScore", score);
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("highScore", 0);
    }

    
}
