using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PopulateMenu : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] private bool debug = true;

    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("UI Elements")]
    [SerializeField] private Transform buttonParent; // Parent transform for the buttons
    [SerializeField] private GameObject buttonPrefab; // Prefab for the buttons

    [Header("Power-Up Settings")]
    [SerializeField] public GameObject parent_PowerUpsList; // List of possible powers
    [SerializeField] public List<GameObject> possiblePowers; // List of possible powers
    [SerializeField] public List<GameObject> currentPowerUps; // List of power-ups

    [Header("Button Settings")]
    [SerializeField] private int maxButtons = 5; // Maximum number of buttons to display
    private int numberOfButtons = 0; // Number of buttons to create

    [SerializeField]
    private float buttonSpacing = 10f; // Spacing between buttons
    private int buttonPadding = 10; // Padding for buttons

    private void Awake()
    {
        if (gameManager == null)
        {
            Debug.Log("Game Manager is not assigned.");
            gameManager = FindObjectOfType<GameManager>();

            if(gameManager == null)
            {
                Debug.LogError("Game Manager not found.");
            }

        }

        gameManager.onLevelUpEvent.AddListener(OpenMenu);
        gameManager.onGameStartEvent.AddListener(NewPowerMenu);

        GetPossiblePowerUps();
        
        gameObject.SetActive(false);
    }

    public void OpenMenu()
    {
        if (debug)
        {
            Debug.Log("Opening power-up menu.");
        }

        gameObject.SetActive(true);

        Time.timeScale = 0; // Pause the game

        Populate();
    }

    public void NewPowerMenu()
    {
        if (debug)
        {
            Debug.Log("Opening new power-up menu.");
        }

        gameObject.SetActive(true);

        Time.timeScale = 0; // Pause the game

        PopulateNewPower();
    }

    public void CloseMenu()
    {
        if (debug)
        {
            Debug.Log("Closing menu.");
        }

        Time.timeScale = 1; // Resume the game

        gameObject.SetActive(false);
    }



    private void GetPossiblePowerUps()
    {

        if (debug)
        {
            Debug.Log("Getting possible power-ups.");
        }
        
        possiblePowers.Clear();
        foreach (Transform child in parent_PowerUpsList.transform)
        {
            possiblePowers.Add(child.gameObject);

            if (debug)
            {
                Debug.Log("Power-up added: " + child.gameObject.name);
            }
        }
    }

    private void UpdateCurrentPowerUps()
    {
        if (debug)
        {
            Debug.Log("Updating current power-ups.");
        }

        currentPowerUps.Clear();

        foreach (Transform child in parent_PowerUpsList.transform)
        {
            if (child.gameObject.activeSelf)
            {
                currentPowerUps.Add(child.gameObject);
                if (debug)
                {
                    Debug.Log("Power-up added: " + child.gameObject.name);
                }
            }
        }
    }

    [ContextMenu("Populate")]
    public void Populate()
    {
        numberOfButtons = 0;

        if (debug)
        {
            Debug.Log("Starting Populate method.");
        }

        // Clear existing buttons
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        if (debug)
        {
            Debug.Log("Cleared existing buttons.");
        }

        UpdateCurrentPowerUps();

        // Create a temporary list to track available power-ups
        // Shuffle the list using OrderBy

        List<GameObject> availablePowerUps = new(possiblePowers.OrderBy(x => UnityEngine.Random.value).ToList());

        // Instantiate new buttons
        int buttonsToCreate = Mathf.Min(maxButtons, availablePowerUps.Count);
        for (int i = 0; i < buttonsToCreate; i++)
        {

            GameObject selectedPowerUp = SelectRandomPowerUp(availablePowerUps);

            if (debug)
            {
                Debug.Log("Selected Power-up: " + (selectedPowerUp != null ? selectedPowerUp.name : "None"));
            }

            if (selectedPowerUp != null)
            {
                GameObject newButton = Instantiate(buttonPrefab, buttonParent);
                LevelUpPanel levelUpPanel = newButton.GetComponent<LevelUpPanel>();
                if (levelUpPanel != null)
                {
                    levelUpPanel.powerUpObject = selectedPowerUp;
                    levelUpPanel.UpdatePanel();
                    numberOfButtons++;
                }
            }
        }

        if (numberOfButtons == 0)
        {
            if (debug)
            {
                Debug.Log("No power-ups available.");
                
            }

            // Close the menu if no power-ups are available
            CloseMenu();
        }
        else
        {
            if (debug)
            {
                Debug.Log("Created " + numberOfButtons + " buttons.");
            }

            // Evenly space the buttons
            EvenlySpacePanels();
            AdjustFontSizes();

            if (debug)
            {
                Debug.Log("Finished Populate method.");
            }
        }

    }

    public void PopulateNewPower ()
    {
        
        numberOfButtons = 0;

        if (debug)
        {
            Debug.Log("Starting PopulateNewPower method.");
        }

        // Clear existing buttons
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        if (debug)
        {
            Debug.Log("Cleared existing buttons.");
        }

        UpdateCurrentPowerUps();

        if (debug)
        {
            Debug.Log("Current Power-ups: " + currentPowerUps.Count);
        }

        // Create a temporary list to track available power-ups
        List<GameObject> availablePowerUps = new List<GameObject>(possiblePowers);
        availablePowerUps.RemoveAll(x => currentPowerUps.Contains(x));

        if(debug)
        {
            Debug.Log("Available Power-ups: " + availablePowerUps.Count);
        }

        // Shuffle the list using OrderBy
        availablePowerUps = availablePowerUps.OrderBy(x => UnityEngine.Random.value).ToList();

        // Instantiate new buttons
        int buttonsToCreate = Mathf.Min(maxButtons, availablePowerUps.Count);
        for (int i = 0; i < buttonsToCreate; i++)
        {

            GameObject selectedPowerUp = availablePowerUps[i];

            if (debug)
            {
                Debug.Log("Selected Power-up: " + (selectedPowerUp != null ? selectedPowerUp.name : "None"));
            }

            if (selectedPowerUp != null)
            {
                GameObject newButton = Instantiate(buttonPrefab, buttonParent);
                LevelUpPanel levelUpPanel = newButton.GetComponent<LevelUpPanel>();
                if (levelUpPanel != null)
                {
                    levelUpPanel.powerUpObject = selectedPowerUp;
                    levelUpPanel.UpdatePanel();
                    numberOfButtons++;
                }
            }
        }

        if (numberOfButtons == 0)
        {
            if (debug)
            {
                Debug.Log("No power-ups available.");
                
            }

            // Close the menu if no power-ups are available
            CloseMenu();
        }
        else
        {
            if (debug)
            {
                Debug.Log("Created " + numberOfButtons + " buttons.");
            }

            // Evenly space the buttons
            EvenlySpacePanels();
            AdjustFontSizes();

            if (debug)
            {
                Debug.Log("Finished Populate method.");
            }
        }

    }
    private GameObject SelectRandomPowerUp(List<GameObject> availablePowerUps)
    {
        GameObject selectedPowerUp = null;

        PowerUpBase powerUpBase = null;

        while (selectedPowerUp == null && availablePowerUps.Count > 0)
        {

            selectedPowerUp = availablePowerUps[0];
            availablePowerUps.RemoveAt(0);
            powerUpBase = selectedPowerUp.GetComponent<PowerUpBase>();

            if (powerUpBase.level == powerUpBase.maxLevel)
            {
                selectedPowerUp = null;
            }

        }
        return selectedPowerUp;
    }

    protected void EvenlySpacePanels()
    {
        GridLayoutGroup gridLayoutGroup = buttonParent.GetComponent<GridLayoutGroup>();

        if (gridLayoutGroup == null)
        {
            Debug.LogError("GridLayoutGroup component not found on buttonParent.");
            return;
        }

        if (numberOfButtons == 0) return;

        RectTransform parentRectTransform = buttonParent.GetComponent<RectTransform>();
        float parentWidth = parentRectTransform.rect.width;
        float parentHeight = parentRectTransform.rect.height;

        // Determine layout direction based on aspect ratio
        bool isVertical = parentHeight > parentWidth;

        gridLayoutGroup.spacing = new Vector2(buttonSpacing, 0);
        gridLayoutGroup.padding = new RectOffset(buttonPadding, buttonPadding, buttonPadding, buttonPadding);

        if (isVertical)
        {
            // Layout panels vertically
            gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Vertical;
            float totalSpacing = buttonSpacing * (numberOfButtons + 1);

            float buttonHeight = (parentHeight - totalSpacing) / numberOfButtons;
            buttonHeight = Mathf.Min(buttonHeight, parentHeight * 0.5f); // Ensure max size is 50% of parent height

            float buttonWidth = parentWidth - 2 * buttonPadding;

            gridLayoutGroup.cellSize = new Vector2(buttonWidth, buttonHeight);

        }
        else
        {
            // Layout panels horizontally
            gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
            float totalSpacing = buttonSpacing * (numberOfButtons + 1);

            float buttonWidth = (parentWidth - totalSpacing) / numberOfButtons;
            buttonWidth = Mathf.Min(buttonWidth, parentWidth * 0.5f); // Ensure max size is 50% of parent width

            float buttonHeight = parentHeight - 2 * buttonPadding;

            gridLayoutGroup.cellSize = new Vector2(buttonWidth, buttonHeight);

        }

        

        if (debug)
        {
            Debug.Log("Evenly spaced panels with " + (isVertical ? "vertical" : "horizontal") + " layout.");
        }
    }

    private void AdjustFontSizes()
    {
        LevelUpPanel[] panels = buttonParent.GetComponentsInChildren<LevelUpPanel>();
        if (panels.Length == 0) return;

        float minFontSize = float.MaxValue;

        // Find the smallest font size
        foreach (var panel in panels)
        {
            float fontSize = panel.GetFontSize();

            if (fontSize < minFontSize)
            {
                minFontSize = fontSize;
            }
        }

        // Set all font sizes to the smallest font size
        foreach (var panel in panels)
        {
            panel.SetFontSize(minFontSize);
        }
    }
}