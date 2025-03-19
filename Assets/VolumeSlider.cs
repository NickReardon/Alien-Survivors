using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] protected Slider volumeSlider; // Reference to the Slider component
    [SerializeField] protected TextMeshProUGUI volumeText; // Reference to the TextMeshProUGUI component
    [SerializeField] private AudioMixer audioMixer; // Reference to the AudioMixer
    [SerializeField] private string volumeParameter = "Master"; // Name of the volume parameter in the AudioMixer
    [SerializeField, Range(0, 100)] private float normalVolumePercentage = 85f; // Percentage for normal volume

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        // Find the Slider and TextMeshProUGUI components in the children
        if (volumeSlider != null)
        {
            // Initialize the slider value and text
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
            LoadSettings();
        }
        else
        {
            Debug.LogError("Slider component not found in children.");
        }

        if (volumeText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found in children.");
        }

        UpdateVolume(volumeSlider.value);
    }

    private void UpdateVolume(float value)
    {
        if (volumeText != null)
        {
            // Update the text to display the current slider value as a whole number from 0 to 100
            volumeText.text = Mathf.RoundToInt(value * 100).ToString();
        }

        // Convert the slider value to a logarithmic scale for the AudioMixer
        float volume;
        float normalVolumeValue = normalVolumePercentage / 100f;

        if (value <= 0)
        {
            volume = -80f; // Minimum volume
        }
        else if (value <= normalVolumeValue)
        {
            // Map 0 to normalVolumePercentage to -80dB to 0dB
            volume = Mathf.Log10(value / normalVolumeValue) * 20;
        }
        else
        {
            // Map normalVolumePercentage to 1 to 0dB to +20dB
            volume = (value - normalVolumeValue) / (1f - normalVolumeValue) * 20;
        }

        audioMixer.SetFloat(volumeParameter, volume);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(volumeParameter, volumeSlider.value);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey(volumeParameter))
        {
            float savedVolume = PlayerPrefs.GetFloat(volumeParameter);
            volumeSlider.value = savedVolume;
            UpdateVolume(savedVolume);
        }
    }
}