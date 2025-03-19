using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    #region Fields

    [Header("Debug Settings")]
    [SerializeField] private bool debug = true;

    [Header("Event Settings")]
    [SerializeField] public UnityEvent onGameStartEvent;
    [SerializeField] public UnityEvent onGameEndEvent;
    [SerializeField] public UnityEvent onLevelUpEvent;
    [SerializeField] public UnityEvent testModeEvent;

    [Header("Game Settings")]
    ///////        [SerializeField] private bool limitBreak = false; // Flag to indicate if the player is in limit break mode
    [SerializeField] private bool testMode = false; // Flag to indicate if the game is in test mode


    [Header("References")]
    [SerializeField] private Player player; // Reference to the Player script
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider xpBar;
    [SerializeField] private PopulateMenu powerMenu;
    [SerializeField] private EnemyManager enemyManager;

    [SerializeField] private GameObject gameOverUI; // Reference to the Game Over UI panel
    [SerializeField] private TextMeshProUGUI finalScoreText; // Reference to the final score text
    [SerializeField] private TextMeshProUGUI finalLevelText; // Reference to the final level text
    [SerializeField] private TextMeshProUGUI highScoreText; // Reference to the high score text
    [SerializeField] private TextMeshProUGUI scoreUI; // Reference to the score UI
    [SerializeField] private Slider levelUI; // Reference to the level UI

    [SerializeField] private GameObject startWindow;



    [Header("Score Settings")]
    [SerializeField] private int scorePerKill = 10; // Points awarded per enemy kill
    [SerializeField] private int levelUpThreshold = 100; // Initial points required to level up
    [SerializeField] private float levelUpFactor = 1.5f; // Factor by which the level up threshold increases

    [Header("Enemy Settings")]
    // [SerializeField] private float spawnRateIncreaseFactor = 1f; // Factor to increase spawn rate
    // [SerializeField] private float speedIncreaseFactor = 1.1f; // Factor to increase enemy speed

    private const string HighScoreKey = "HighScore";


    private int currentScore = 0;
    private int currentLevel = 1;
    private int currentLevelProgress = 0;



    #endregion

    #region Initialization Methods

    void Awake()
    {
        if (scoreText == null)
        {
            Debug.LogError("Score Text Object is not assigned.");
        }

        if (xpBar == null)
        {
            Debug.LogError("XP Bar Object is not assigned.");
        }

        if (powerMenu != null)
        {
            if (powerMenu != null)
            {
                powerMenu = FindObjectOfType<PopulateMenu>().GetComponent<PopulateMenu>();
            }
            else
            {
                Debug.LogError("Power Menu Object is not assigned.");
            }
        }

        if (enemyManager != null)
        {
            if (enemyManager != null)
            {
                enemyManager = FindObjectOfType<EnemyManager>().GetComponent<EnemyManager>();
            }
            else
            {
                Debug.LogError("Enemy Manager Object is not assigned.");
            }
        }

        if (player != null)
        {
            player = FindObjectOfType<Player>().GetComponent<Player>();
            if (player != null)
            {
            }
            else
            {
                Debug.LogError("Player Object is not assigned.");
            }
        }

        if (onGameStartEvent == null)
        {
            onGameStartEvent = new UnityEvent();
        }

        if (onGameEndEvent == null)
        {
            onGameEndEvent = new UnityEvent();
        }

        if (onLevelUpEvent == null)
        {
            onLevelUpEvent = new UnityEvent();
        }

        player.OnPlayerDeath.AddListener(EndGame);

        if(startWindow != null)
        {
            startWindow.SetActive(true);
        }

    }
    void Start()
    {
        Time.timeScale = 1; // Ensure the game is not paused
        UpdateScoreText();
        UpdateLevelProgressBar();

        onGameStartEvent.Invoke();

        if (testMode)
        {
            testModeEvent.Invoke();

            if (debug)
            {
                Debug.Log("Test Mode enabled. Message Invoked.");
            }
        }
    }

    #endregion // Initialization Methods

    #region Score Methods

    public void OnGainXP(int xp)
    {
        currentScore += xp;
        currentLevelProgress += xp;
        UpdateScoreText();
        UpdateLevelProgressBar();
        CheckForLevelUp();
    }
    public void OnEnemyKilled()
    {
        currentScore += scorePerKill;
        currentLevelProgress += scorePerKill;
        UpdateScoreText();
        UpdateLevelProgressBar();
        CheckForLevelUp();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + currentScore + "\nLevel: " + currentLevel;
    }

    private void UpdateLevelProgressBar()
    {
        if (xpBar != null)
        {
            xpBar.maxValue = levelUpThreshold;
            xpBar.value = currentLevelProgress;
        }
    }

    private void CheckForLevelUp()
    {
        if (currentLevelProgress >= levelUpThreshold)
        {
            LevelUp();
        }
    }

    [ContextMenu("Level Up")]
    private void LevelUp()
    {
        currentLevel++;
        currentLevelProgress = 0; // Reset progress for the new level
        levelUpThreshold = Mathf.CeilToInt(levelUpThreshold * levelUpFactor); // Increase the threshold

        onLevelUpEvent.Invoke();

        Debug.Log("Level Up! Current Level: " + currentLevel);
        UpdateLevelProgressBar(); // Update the progress bar after level up
    }


    #region Game End Methods

    private void EndGame()
    {
        Time.timeScale = 0; // Pause the game

        gameOverUI.SetActive(true); // Show the Game Over UI

        // Display the final score and level
        finalScoreText.text = "Final Score: " + currentScore;
        finalLevelText.text = "Level Reached: " + currentLevel;

        // Check and update high score
        UpdateHighScore();
    }

    private void UpdateHighScore()
    {
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, currentScore);
            PlayerPrefs.Save();
            highScore = currentScore;
        }
        highScoreText.text = "High Score: " + highScore;
    }

    #endregion // Game End Methods

    #region UI Methods

    public void RestartGame()
    {
        Time.timeScale = 1; // Resume the game
        // Add logic to restart the game
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1; // Resume the game
        // Add logic to return to the main menu
    }

    #endregion // UI Methods

    #endregion // Score Methods
}