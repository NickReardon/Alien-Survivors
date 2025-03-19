using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreenUI; // Reference to the parent GameObject of the pause screen UI elements
    [SerializeField] private Button returnToGameButton; // Reference to the Return to Game button
    [SerializeField] private Button exitToMainMenuButton; // Reference to the Exit to Main Menu button
    [SerializeField] private LoadScene loadScene; // Reference to the LoadScene component

    // Start is called before the first frame update
    void Start()
    {
        if (pauseScreenUI == null)
        {
            Debug.LogError("Pause screen UI GameObject not assigned.");
        }

        if (returnToGameButton == null)
        {
            Debug.LogError("Return to Game button not assigned.");
        }
        else
        {
            returnToGameButton.onClick.AddListener(ReturnToGame);
        }

        if (exitToMainMenuButton == null)
        {
            Debug.LogError("Exit to Main Menu button not assigned.");
        }
        else
        {
            exitToMainMenuButton.onClick.AddListener(ExitToMainMenu);
        }

        if (loadScene == null)
        {
            Debug.LogError("LoadScene component not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for pause input (e.g., Escape key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        // Toggle the pause state
        bool isPaused = Time.timeScale == 0;
        Time.timeScale = isPaused ? 1 : 0;

        // Show or hide the pause screen UI
        if (pauseScreenUI != null)
        {
            pauseScreenUI.SetActive(!isPaused);
        }
    }

    private void ReturnToGame()
    {
        // Resume the game
        Time.timeScale = 1;
        if (pauseScreenUI != null)
        {
            pauseScreenUI.SetActive(false);
        }
    }

    private void ExitToMainMenu()
    {
        // Resume the game before exiting to the main menu
        Time.timeScale = 1;

        // Use the LoadScene component to load the main menu scene
        if (loadScene != null)
        {
            loadScene.InitiateSceneLoad();
        }
    }
}

