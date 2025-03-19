using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpPanel : MonoBehaviour
{
    PopulateMenu populateMenu;
    [SerializeField] public GameObject powerUpObject;
    [SerializeField] public PowerUpBase powerUp;
    [SerializeField] public GameObject titleText;
    [SerializeField] public GameObject descriptionText;
    [SerializeField] public GameObject levelText;
    [SerializeField] public GameObject powerImage;

    private void Awake()
    {
        populateMenu = gameObject.transform.parent.parent.gameObject.GetComponent<PopulateMenu>();
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("Button component is not assigned.");
        }
    }

    private void OnButtonClicked()
    {
        Debug.Log("Button clicked. Power-up: " + powerUp.name);
        // Upgrade the power-up
        if (powerUp == null)
        {
            Debug.LogError("PowerUp is not assigned.");
            return;
        }

        if (populateMenu == null)
        {
            Debug.LogError("PopulateMenu component not found in parent.");
            return;
        }

        if (powerUp.level == -1)
        {
            powerUp.EnablePowerUp();
        }
        else
        {
            // Power-up exists, upgrade it
            if (powerUpObject != null)
            {
                // Apply changes to the existing power-up
                powerUp.LevelUp();
            }
        }

        populateMenu.CloseMenu();
    }

    public void UpdatePanel()
    {
        powerUp = powerUpObject.GetComponent<PowerUpBase>();

        // // Set the title, description, level, and image
        titleText.GetComponent<TMPro.TextMeshProUGUI>().text = powerUp.name;
        powerImage.GetComponent<UnityEngine.UI.Image>().sprite = powerUp.icon;

        if (powerUp.level == -1)
        {
            descriptionText.GetComponent<TMPro.TextMeshProUGUI>().text = powerUp.description;
            levelText.GetComponent<TMPro.TextMeshProUGUI>().text = "New!";
            levelText.GetComponent<TMPro.TextMeshProUGUI>().color = Color.yellow;
                   }
        else
        {
            descriptionText.GetComponent<TMPro.TextMeshProUGUI>().text = powerUp.upgrades[powerUp.level].description;
            levelText.GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + (powerUp.level + 1);
        }


    }

    public float GetFontSize()
    {
        TextMeshProUGUI text = descriptionText.GetComponent<TMPro.TextMeshProUGUI>();
        text.enableAutoSizing = true;
        text.ForceMeshUpdate(true);
        return text.fontSize;
    }

    public void SetFontSize(float fontSize)
    {
        TextMeshProUGUI text = descriptionText.GetComponent<TMPro.TextMeshProUGUI>();
        text.enableAutoSizing = false;
        text.fontSize = fontSize;
    }

}
