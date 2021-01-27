using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{
    private bool controlMenuState;
    private TextMeshProUGUI gameResultView;

    public UiElement controlMenu;
    public CanvasGroup pauseButton;
    public CanvasGroup pauseMenu;
    public CanvasGroup endGameMenu;
    public Button switchButton;
    public Button attackButton;

    public void PauseGame()
    {
        controlMenuState = controlMenu.IsVisible();
        controlMenu.Hide();
        Utility.SetCanvasGroupEnabled(pauseButton, false);
        Utility.SetCanvasGroupEnabled(pauseMenu, true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
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
    }

    public void SetControlCallbacks(UnityAction switchAction, UnityAction attackAction)
    {
        switchButton.onClick.AddListener(switchAction);
        attackButton.onClick.AddListener(attackAction);
    }

    void Start()
    {
        gameResultView = endGameMenu.GetComponentInChildren<TextMeshProUGUI>();
        Utility.SetCanvasGroupEnabled(endGameMenu, false);
        Utility.SetCanvasGroupEnabled(pauseMenu, false);
        controlMenu.Hide();
    }
}
