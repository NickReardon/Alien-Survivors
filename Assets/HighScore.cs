using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    public TextMeshProUGUI highScoreText; // Reference to the TextMeshProUGUI component

    private const string HighScoreKey = "HighScore";

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the high score from PlayerPrefs
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        // Display the high score
        highScoreText.text = "High Score: " + highScore;
    }
}
