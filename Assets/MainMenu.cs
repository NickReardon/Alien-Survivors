using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu Elements")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    [Header("Exit Confirmation Elements")]
    [SerializeField] private GameObject exitConfirmationWindow;
    [SerializeField] private Button confirmExitButton;
    [SerializeField] private Button cancelExitButton;

    [Header("Scene Loader")]
    [SerializeField] private SceneLoader sceneLoader;

    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        creditsButton.onClick.AddListener(OnCreditsButtonClicked);
        
        if(exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        confirmExitButton.onClick.AddListener(OnConfirmExitButtonClicked);
        cancelExitButton.onClick.AddListener(OnCancelExitButtonClicked);

        exitConfirmationWindow.SetActive(false); // Ensure the exit confirmation window is initially hidden

        Time.timeScale = 1; // Ensure the game is not paused
    }

    private void OnStartButtonClicked()
    {
       sceneLoader.InitiateSceneLoad();
    }

    private void OnSettingsButtonClicked()
    {
        MenuManager.Instance.ShowSettingsMenu();
    }

    private void OnCreditsButtonClicked()
    {
        MenuManager.Instance.ShowCreditsMenu();
    }

    private void OnExitButtonClicked()
    {
        exitConfirmationWindow.SetActive(true); // Show the exit confirmation window
    }

    private void OnConfirmExitButtonClicked()
    {
        Application.Quit(); // Quit the application
    }

    private void OnCancelExitButtonClicked()
    {
        exitConfirmationWindow.SetActive(false); // Hide the exit confirmation window
    }
}