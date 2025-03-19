using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplaySettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown displayModeDropdown; // Reference to the display mode dropdown
    [SerializeField] private TMP_Dropdown resolutionDropdown; // Reference to the resolution dropdown
    [SerializeField] private Button applyButton; // Reference to the apply button
    [SerializeField] private GameObject confirmationWindow; // Reference to the confirmation window
    [SerializeField] private TMP_Text countdownText; // Reference to the countdown text
    [SerializeField] private Button confirmButton; // Reference to the confirm button
    [SerializeField] private Button cancelButton; // Reference to the cancel button

    private Resolution[] availableResolutions;
    private DisplaySettingsState newState;
    private DisplaySettingsState previousState;
    private Coroutine countdownCoroutine;

    private struct DisplaySettingsState
    {
        public int width;
        public int height;
        public FullScreenMode mode;
    }

    private void Start()
    {
        PopulateDisplayModeDropdown();
        PopulateResolutionDropdown();

        LoadSettings();

        if (displayModeDropdown != null)
        {
            // Add listener to handle display mode dropdown value changes
            displayModeDropdown.onValueChanged.AddListener(delegate
            {
                newState.mode = GetFullScreenMode(displayModeDropdown.value);
            });
        }

        if (resolutionDropdown != null)
        {
            // Add listener to handle resolution dropdown value changes
            resolutionDropdown.onValueChanged.AddListener(delegate
            {
                SetNewResolution(resolutionDropdown.value);
            });
        }

        if (applyButton != null)
        {
            // Add listener to handle apply button click
            applyButton.onClick.AddListener(ApplySettings);
        }

        if (confirmButton != null)
        {
            // Add listener to handle confirm button click
            confirmButton.onClick.AddListener(ConfirmSettings);
        }

        if (cancelButton != null)
        {
            // Add listener to handle cancel button click
            cancelButton.onClick.AddListener(CancelSettings);
        }

        // Set the dropdowns to display the current settings
        SetCurrentDisplayMode();
        SetCurrentResolution();
    }

    private void PopulateDisplayModeDropdown()
    {
        if (displayModeDropdown != null)
        {
            // Clear existing options
            displayModeDropdown.ClearOptions();

            // Create a list of display mode options
            List<string> options = new List<string> { "Exclusive Fullscreen", "Window", "Borderless Window" };

            // Add options to the dropdown
            displayModeDropdown.AddOptions(options);
        }
        else
        {
            Debug.LogError("Display mode dropdown not assigned.");
        }
    }

    private void PopulateResolutionDropdown()
    {
        if (resolutionDropdown != null)
        {
            // Clear existing options
            resolutionDropdown.ClearOptions();

            // Get available resolutions
            availableResolutions = Screen.resolutions;

            // Use a HashSet to filter out duplicate resolutions
            HashSet<string> uniqueResolutions = new HashSet<string>();
            List<string> options = new List<string>();

            foreach (Resolution resolution in availableResolutions)
            {
                string resolutionString = resolution.width + " x " + resolution.height;
                if (uniqueResolutions.Add(resolutionString))
                {
                    options.Add(resolutionString);
                }
            }

            // Add options to the dropdown
            resolutionDropdown.AddOptions(options);
        }
        else
        {
            Debug.LogError("Resolution dropdown not assigned.");
        }
    }

    private void SetCurrentDisplayMode()
    {
        if (displayModeDropdown != null)
        {
            switch (Screen.fullScreenMode)
            {
                case FullScreenMode.ExclusiveFullScreen:
                    displayModeDropdown.value = 0;
                    break;
                case FullScreenMode.Windowed:
                    displayModeDropdown.value = 1;
                    break;
                case FullScreenMode.FullScreenWindow:
                    displayModeDropdown.value = 2;
                    break;
                default:
                    Debug.LogError("Unknown display mode.");
                    break;
            }
        }
    }

    private void SetCurrentResolution()
    {
        if (resolutionDropdown != null)
        {
            for (int i = 0; i < availableResolutions.Length; i++)
            {
                if (availableResolutions[i].width == Screen.width &&
                    availableResolutions[i].height == Screen.height)
                {
                    resolutionDropdown.value = i;
                    break;
                }
            }
        }
    }

    private void ApplySettings()
    {
        // Save current settings
        previousState = new DisplaySettingsState
        {
            width = Screen.width,
            height = Screen.height,
            mode = Screen.fullScreenMode
        };

        // Apply new settings
        Screen.SetResolution(newState.width, newState.height, newState.mode);

        // Show confirmation window
        confirmationWindow.SetActive(true);

        // Start countdown
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(CountdownCoroutine(10));
    }

    public void ConfirmSettings()
    {
        // Stop countdown
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        // Save new settings to PlayerPrefs
        SaveSettings();

        // Hide confirmation window
        confirmationWindow.SetActive(false);
    }

    public void CancelSettings()
    {
        // Stop countdown
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        // Revert to previous settings
        Screen.SetResolution(previousState.width, previousState.height, previousState.mode);

        // Hide confirmation window
        confirmationWindow.SetActive(false);
    }

    private IEnumerator CountdownCoroutine(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        // Time's up, revert settings
        CancelSettings();
    }

    private void SetNewResolution(int index)
    {
        if (resolutionDropdown != null && index >= 0 && index < resolutionDropdown.options.Count)
        {
            string[] dimensions = resolutionDropdown.options[index].text.Split('x');
            if (dimensions.Length == 2 && int.TryParse(dimensions[0].Trim(), out int width) && int.TryParse(dimensions[1].Trim(), out int height))
            {
                newState.width = width;
                newState.height = height;
            }
            else
            {
                Debug.LogError("Invalid resolution format.");
            }
        }
        else
        {
            Debug.LogError("Invalid resolution selected.");
        }
    }

    private FullScreenMode GetFullScreenMode(int index)
    {
        switch (index)
        {
            case 0:
                return FullScreenMode.ExclusiveFullScreen;
            case 1:
                return FullScreenMode.Windowed;
            case 2:
                return FullScreenMode.FullScreenWindow;
            default:
                Debug.LogError("Invalid display mode selected.");
                return Screen.fullScreenMode;
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionWidth", newState.width);
        PlayerPrefs.SetInt("ResolutionHeight", newState.height);
        PlayerPrefs.SetInt("DisplayMode", (int)newState.mode);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("ResolutionWidth") && PlayerPrefs.HasKey("ResolutionHeight") && PlayerPrefs.HasKey("DisplayMode"))
        {
            int width = PlayerPrefs.GetInt("ResolutionWidth");
            int height = PlayerPrefs.GetInt("ResolutionHeight");
            FullScreenMode mode = (FullScreenMode)PlayerPrefs.GetInt("DisplayMode");

            Screen.SetResolution(width, height, mode);

            newState.width = width;
            newState.height = height;
            newState.mode = mode;
        }
    }
}
