using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    enum Screen
    {
        Main,
        NewGame,
        Settings
    }

    public CanvasGroup mainScreen;
    public CanvasGroup newGameScreen;
    public CanvasGroup settingsScreen;
    private PlaySound soundPlayer;

    void SetCurrentScreen(Screen screen)
    {
        Utility.SetCanvasGroupEnabled(mainScreen, screen == Screen.Main);
        Utility.SetCanvasGroupEnabled(newGameScreen, screen == Screen.NewGame);
        Utility.SetCanvasGroupEnabled(settingsScreen, screen == Screen.Settings);
    }
    
    void Start()
    {
        soundPlayer = GetComponent<PlaySound>();

        foreach (var button in GetComponentsInChildren<Button>())
        {
            button.onClick.AddListener(PlayClickSound);
        }

        SetCurrentScreen(Screen.Main);
    }

    public void StartNewGame()
    {
        var levelName = EventSystem.current.currentSelectedGameObject.name;
        
        switch (levelName)
        {
        case "Level1Button":
            SceneManager.LoadScene("Level1Scene");
            break;
        
        case "Level2Button":
            SceneManager.LoadScene("Level2Scene");
            break;
        }
    }

    public void OpenNewGameMenu()
    {
        SetCurrentScreen(Screen.NewGame);
    }

    public void OpenSettings()
    {
        SetCurrentScreen(Screen.Settings);
    }

    public void CloseSettings()
    {
        SetCurrentScreen(Screen.Main);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayClickSound()
    {
        if(soundPlayer)
            soundPlayer.Play("Click");
    }
}
