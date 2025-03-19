using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Settings Elements")]
    [SerializeField] private DisplaySettings displaySettings;
    [SerializeField] private List<VolumeSlider> volumeSliders; // List of volume sliders
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private GameObject confirmationWindow;
    [SerializeField] private GameObject cancelWindow;
    [SerializeField] private Button confirmButton_CancelWindow;
    [SerializeField] private Button cancelButton_CancelWindow;

    private void Start()
    {
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);

        confirmButton_CancelWindow.onClick.AddListener(OnConfirmButton_CancelWindowClicked);
        cancelButton_CancelWindow.onClick.AddListener(OnCancelButton_CancelWindowClicked);

        foreach (var volumeSlider in volumeSliders)
        {
            volumeSlider.Initialize(); // Initialize each volume slider
        }
    }

    private void OnConfirmButtonClicked()
    {
        foreach (var volumeSlider in volumeSliders)
        {
            volumeSlider.SaveSettings(); // Save settings for each volume slider
        }
        if( displaySettings != null)
            displaySettings.SaveSettings();
        MenuManager.Instance.ShowMainMenu();

    }

    private void OnCancelButtonClicked()
    {
        cancelWindow.SetActive(true);
    }

    private void OnConfirmButton_CancelWindowClicked()
    {
        if( displaySettings != null)
            displaySettings.CancelSettings();
            
        foreach (var volumeSlider in volumeSliders)
        {
            volumeSlider.LoadSettings(); // Load settings for each volume slider
        }
        cancelWindow.SetActive(false);
        MenuManager.Instance.ShowMainMenu();
    }

    private void OnCancelButton_CancelWindowClicked()
    {
        cancelWindow.SetActive(false);
    }
}