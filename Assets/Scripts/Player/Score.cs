using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI currentScoreText;
    public int highScore;
    public int currentScore;

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        currentScore = 0;
        UpdateText(highScoreText, $"High score: {highScore}");
        UpdateText(currentScoreText, $"Current score: {currentScore}");
    }

    public void EventPerformed(UserEvent userEvent) {
        int scoreReceived = 0;
        switch(userEvent)
        {
            case UserEvent.killedEnemy:
                scoreReceived = 5;
                break;
            case UserEvent.perfectShot:
                scoreReceived = 3;
                break;
            case UserEvent.normalShot:
                scoreReceived = 1;
                break;
        }
        currentScore += scoreReceived;
        UpdateText(currentScoreText, $"Current score: {currentScore}");
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", currentScore);
            UpdateText(highScoreText, $"High score: {highScore}");
        }
    }

    private void UpdateText(TextMeshProUGUI textMeshProUGUI, string text)
    {
        textMeshProUGUI.text = text;
    }

    // Event-urile care pot fi performate de user pt a primi scor.
    public enum UserEvent
    {
        normalShot,
        perfectShot,
        killedEnemy
    }
}
