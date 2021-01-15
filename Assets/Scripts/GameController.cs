using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

internal sealed class GameController : MonoBehaviour
{
    public Button attackButton;
    public UiElement buttonPanel;
    public CanvasGroup pauseButton;
    public CanvasGroup pauseMenu;
    public CanvasGroup endGameMenu;
    public TextMeshProUGUI gameResultView;

    public Character[] playerCharacter;
    public Character[] enemyCharacter;
    Character currentTarget;
    bool waitingForInput;

    private bool buttonPanelState;

    Character FirstAliveCharacter(Character[] characters)
    {
        return characters.FirstOrDefault(character => !character.IsDead());
    }

    void PlayerWon()
    {
        Debug.Log("Player won.");
        ShowGameResult(true);
    }

    void PlayerLost()
    {
        Debug.Log("Player lost.");
        ShowGameResult(false);
    }

    bool CheckEndGame()
    {
        if (FirstAliveCharacter(playerCharacter) == null) {
            PlayerLost();
            return true;
        }

        if (FirstAliveCharacter(enemyCharacter) == null) {
            PlayerWon();
            return true;
        }

        return false;
    }

    //[ContextMenu("Player Attack")]
    public void PlayerAttack()
    {
        waitingForInput = false;
    }

    //[ContextMenu("Next Target")]
    public void NextTarget()
    {
        int index = Array.IndexOf(enemyCharacter, currentTarget);
        for (int i = 1; i < enemyCharacter.Length; i++) {
            int next = (index + i) % enemyCharacter.Length;
            if (!enemyCharacter[next].IsDead()) {
                currentTarget.targetIndicator.gameObject.SetActive(false);
                currentTarget = enemyCharacter[next];
                currentTarget.targetIndicator.gameObject.SetActive(true);
                return;
            }
        }
    }

    public void PauseGame()
    {
        buttonPanelState = buttonPanel.IsVisible();
        buttonPanel.Hide();
        Utility.SetCanvasGroupEnabled(pauseButton, false);
        Utility.SetCanvasGroupEnabled(pauseMenu, true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Utility.SetCanvasGroupEnabled(pauseMenu, false);
        Utility.SetCanvasGroupEnabled(pauseButton, true);
        if(buttonPanelState)
            buttonPanel.Show();
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

    IEnumerator GameLoop()
    {
        yield return null;
        while (!CheckEndGame()) {
            foreach (var player in playerCharacter)
            {
                if (player.IsDead())
                    continue;
                
                currentTarget = FirstAliveCharacter(enemyCharacter);
                if (currentTarget == null)
                    break;

                currentTarget.targetIndicator.gameObject.SetActive(true);
                buttonPanel.Show(true);

                waitingForInput = true;
                while (waitingForInput)
                    yield return null;
                
                buttonPanel.Hide(true);
                currentTarget.targetIndicator.gameObject.SetActive(false);

                player.target = currentTarget.transform;
                player.AttackEnemy();

                while (!player.IsIdle())
                    yield return null;

                break;
            }

            foreach (var enemy in enemyCharacter)
            {
                if (enemy.IsDead())
                    continue;
                
                Character target = FirstAliveCharacter(playerCharacter);
                if (target == null)
                    break;

                enemy.target = target.transform;
                enemy.AttackEnemy();

                while (!enemy.IsIdle())
                    yield return null;

                break;
            }
        }
    }
    
    void Start()
    {
        attackButton.onClick.AddListener(PlayerAttack);
        Utility.SetCanvasGroupEnabled(endGameMenu, false);
        Utility.SetCanvasGroupEnabled(pauseMenu, false);
        buttonPanel.Hide();
        StartCoroutine(GameLoop());
    }

    void ShowGameResult(bool won)
    {
        gameResultView.text = won ? "You won!" : "You lose =(";
        buttonPanel.Hide();
        Utility.SetCanvasGroupEnabled(pauseButton, false);
        Utility.SetCanvasGroupEnabled(pauseMenu, false);
        Utility.SetCanvasGroupEnabled(endGameMenu, true);
    }
}