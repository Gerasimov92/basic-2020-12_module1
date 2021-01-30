using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class UiController : MonoBehaviour
{
    private bool controlMenuState;
    private TextMeshProUGUI gameResultView;
    private PlaySound soundPlayer;

    public UiElement controlMenu;
    public CanvasGroup pauseButton;
    public CanvasGroup pauseMenu;
    public CanvasGroup endGameMenu;
    public Button switchButton;
    public Button attackButton;

    public void PauseGame()
    {
        foreach (var source in FindObjectsOfType<AudioSource>())
        {
            source.Pause();
        }
        controlMenuState = controlMenu.IsVisible();
        controlMenu.Hide();
        Utility.SetCanvasGroupEnabled(pauseButton, false);
        Utility.SetCanvasGroupEnabled(pauseMenu, true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        foreach (var source in FindObjectsOfType<AudioSource>())
        {
            source.UnPause();
        }
        Utility.SetCanvasGroupEnabled(pauseMenu, false);
        Utility.SetCanvasGroupEnabled(pauseButton, true);
        if(controlMenuState)
            controlMenu.Show();
        Time.timeScale = 1;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowGameResult(bool won)
    {
        if(gameResultView)
            gameResultView.text = won ? "You won!" : "You lose =(";
        
        controlMenu.Hide();
        Utility.SetCanvasGroupEnabled(pauseButton, false);
        Utility.SetCanvasGroupEnabled(pauseMenu, false);
        Utility.SetCanvasGroupEnabled(endGameMenu, true);

        if (soundPlayer)
            soundPlayer.Play(won ? "Win" : "Lose");
    }

    public void SetControlCallbacks(UnityAction switchAction, UnityAction attackAction)
    {
        switchButton.onClick.AddListener(switchAction);
        attackButton.onClick.AddListener(attackAction);
    }

    public void PlayClickSound()
    {
        if(soundPlayer)
            soundPlayer.Play("Click");
    }

    void Start()
    {
        soundPlayer = GetComponent<PlaySound>();

        foreach (var button in GetComponentsInChildren<Button>())
        {
            button.onClick.AddListener(PlayClickSound);
        }

        gameResultView = endGameMenu.GetComponentInChildren<TextMeshProUGUI>();
        Utility.SetCanvasGroupEnabled(endGameMenu, false);
        Utility.SetCanvasGroupEnabled(pauseMenu, false);
        controlMenu.Hide();
    }
}
