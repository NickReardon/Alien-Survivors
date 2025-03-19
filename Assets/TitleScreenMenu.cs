using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

enum MenuState
{
    MAIN_MENU,
    SETTINGS_MENU,
    SETTINGS_CONFIRMATION_WINDOW,
    SETTINGS_CANCEL_WINDOW,
    CREDITS_MENU,
    EXIT_CONFIRMATION_WINDOW
};



class TitleScreenMenu : MonoBehaviour
{
    [Header("Main Menu Elements")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Button startButton_MainMenu;
    [SerializeField] private Button settingsButton_MainMenu;
    [SerializeField] private Button creditsButton_MainMenu;
    [SerializeField] private Button exitButton_MainMenu;



    [Header("Settings Elements")]
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private Button confirmbutton_Settings;
    [SerializeField] private Button cancelbutton_Settings;
    [SerializeField] private GameObject confirmationWindow_Settings;
    [SerializeField] private Button confirmButton_ConfirmationWindow_Settings;
    [SerializeField] private Button cancelButton_ConfirmationWindow_Settings;
    [SerializeField] private GameObject cancelWindow_Settings;
    [SerializeField] private Button confirmButton_CancelWindow_Settings;
    [SerializeField] private Button cancelButton_CancelWindow_Settings;




    [Header("Credits Elements")]
    [SerializeField] private GameObject creditsWindow;


    [Header("Exit Confirmation Elements")]
    [SerializeField] private GameObject exitConfirmationWindow_MainMenu;
    [SerializeField] private Button confirmButton_ExitConfirmationWindow_MainMenu;
    [SerializeField] private Button cancelButton_ExitConfirmationWindow_MainMenu;

    //private CreditsWindow creditsWindow;


    // [Header("Main Menu Elements")]

    private MenuState currentMenuState = MenuState.MAIN_MENU;
  
    void Start()
    {
        
        if (settingsButton_MainMenu != null)
            settingsButton_MainMenu.onClick.AddListener(On_SettingsButton_MainMenu_Clicked);
        if (creditsButton_MainMenu != null)
            creditsButton_MainMenu.onClick.AddListener(On_CreditsButton_MainMenu_Clicked);
        if (exitButton_MainMenu != null)
            exitButton_MainMenu.onClick.AddListener(On_ExitButton_MainMenu_Clicked);

        if (confirmbutton_Settings != null)
            confirmbutton_Settings.onClick.AddListener(On_ConfirmButton_Settings_Clicked);
        if (cancelbutton_Settings != null)
            cancelbutton_Settings.onClick.AddListener(On_CancelButton_Settings_Clicked);
        if (confirmButton_ConfirmationWindow_Settings != null)
            confirmButton_ConfirmationWindow_Settings.onClick.AddListener(On_ConfirmButton_ConfirmationWindow_Settings_Clicked);
        if (cancelButton_ConfirmationWindow_Settings != null)
            cancelButton_ConfirmationWindow_Settings.onClick.AddListener(On_CancelButton_ConfirmationWindow_Settings_Clicked);
        if (confirmButton_CancelWindow_Settings != null)
            confirmButton_CancelWindow_Settings.onClick.AddListener(On_ConfirmButton_CancelWindow_Settings_Clicked);
        if (cancelButton_CancelWindow_Settings != null)
            cancelButton_CancelWindow_Settings.onClick.AddListener(On_CancelButton_CancelWindow_Settings_Clicked);


       if (confirmButton_ExitConfirmationWindow_MainMenu != null)
            confirmButton_ExitConfirmationWindow_MainMenu.onClick.AddListener(On_ConfirmButton_ExitConfirmationWindow_MainMenu_Clicked);
        if (cancelButton_ExitConfirmationWindow_MainMenu != null)
            cancelButton_ExitConfirmationWindow_MainMenu.onClick.AddListener(On_CancelButton_ExitConfirmationWindow_MainMenu_Clicked);


        SetStartState();
    }

    private void SetStartState()
    {
        if (mainMenu != null) mainMenu.SetActive(true);
        if (settingsWindow != null) settingsWindow.SetActive(false);
        if (creditsWindow != null) creditsWindow.SetActive(false);
        if (exitConfirmationWindow_MainMenu != null) exitConfirmationWindow_MainMenu.SetActive(false);
        if (confirmationWindow_Settings != null) confirmationWindow_Settings.SetActive(false);
        if (cancelWindow_Settings != null) cancelWindow_Settings.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapeKeyPress();
        }
    }

    private void HandleEscapeKeyPress()
    {
        switch (currentMenuState)
        {
            case MenuState.MAIN_MENU:
                On_ExitButton_MainMenu_Clicked();
                break;
            case MenuState.SETTINGS_MENU:
                On_CancelButton_Settings_Clicked();
                break;
            case MenuState.SETTINGS_CONFIRMATION_WINDOW:
                On_CancelButton_ConfirmationWindow_Settings_Clicked();
                break;
            case MenuState.SETTINGS_CANCEL_WINDOW:
                On_CancelButton_CancelWindow_Settings_Clicked();
                break;
            case MenuState.CREDITS_MENU:
                creditsWindow.SetActive(false);
                currentMenuState = MenuState.MAIN_MENU;
                break;
            case MenuState.EXIT_CONFIRMATION_WINDOW:
                On_CancelButton_ExitConfirmationWindow_MainMenu_Clicked();
                break;
        }
    }


    private void On_CancelButton_ExitConfirmationWindow_MainMenu_Clicked()
    {
        QuitGame();
    }

    private void QuitGame()
    {
        throw new NotImplementedException();
    }

    private void On_ConfirmButton_ExitConfirmationWindow_MainMenu_Clicked()
    {
        exitConfirmationWindow_MainMenu.SetActive(false);
        currentMenuState = MenuState.MAIN_MENU;
    }

    private void On_CancelButton_CancelWindow_Settings_Clicked()
    {
        cancelWindow_Settings.SetActive(false);

        currentMenuState = MenuState.SETTINGS_MENU;
    }

    private void On_ConfirmButton_CancelWindow_Settings_Clicked()
    {
        cancelWindow_Settings.SetActive(false);

        revertSettings();

        settingsWindow.SetActive(false);

        currentMenuState = MenuState.MAIN_MENU;
    }

    private void On_CancelButton_ConfirmationWindow_Settings_Clicked()
    {
        confirmationWindow_Settings.SetActive(false);

        revertSettings();

        currentMenuState = MenuState.SETTINGS_MENU;
    }

    private void revertSettings()
    {
        throw new NotImplementedException();
    }

    private void On_ConfirmButton_ConfirmationWindow_Settings_Clicked()
    {
        ApplySettings();

        confirmationWindow_Settings.SetActive(false);

        settingsWindow.SetActive(false);

        currentMenuState = MenuState.MAIN_MENU;
    }

    private void ApplySettings()
    {
        throw new NotImplementedException();
    }

    private void On_CancelButton_Settings_Clicked()
    {
        cancelWindow_Settings.SetActive(true);
        currentMenuState = MenuState.SETTINGS_CANCEL_WINDOW;
    }

    private void On_ConfirmButton_Settings_Clicked()
    {
        confirmationWindow_Settings.SetActive(true);
        currentMenuState = MenuState.SETTINGS_CONFIRMATION_WINDOW;

    }

    private void On_ExitButton_MainMenu_Clicked()
    {
        exitConfirmationWindow_MainMenu.SetActive(true);

        currentMenuState = MenuState.EXIT_CONFIRMATION_WINDOW;
    }

    private void On_CreditsButton_MainMenu_Clicked()
    {
        creditsWindow.SetActive(true);

        currentMenuState = MenuState.CREDITS_MENU;

    }

    private void On_SettingsButton_MainMenu_Clicked()
    {
        settingsWindow.SetActive(true);

        currentMenuState = MenuState.SETTINGS_MENU;
    }

    


}