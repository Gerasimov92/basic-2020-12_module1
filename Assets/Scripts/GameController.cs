using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

internal sealed class GameController : MonoBehaviour
{
    public Button attackButton;
    public CanvasGroup buttonPanel;
    
    public Character[] playerCharacter;
    public Character[] enemyCharacter;
    Character currentTarget;
    bool waitingForInput;

    Character FirstAliveCharacter(Character[] characters)
    {
        return characters.FirstOrDefault(character => !character.IsDead());
    }

    void PlayerWon()
    {
        Debug.Log("Player won.");
    }

    void PlayerLost()
    {
        Debug.Log("Player lost.");
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

    IEnumerator GameLoop()
    {
        yield return null;
        while (!CheckEndGame()) {
            foreach (var player in playerCharacter)
            {
                currentTarget = FirstAliveCharacter(enemyCharacter);
                if (currentTarget == null)
                    break;

                currentTarget.targetIndicator.gameObject.SetActive(true);
                Utility.SetCanvasGroupEnabled(buttonPanel, true);

                waitingForInput = true;
                while (waitingForInput)
                    yield return null;

                Utility.SetCanvasGroupEnabled(buttonPanel, false);
                currentTarget.targetIndicator.gameObject.SetActive(false);

                player.target = currentTarget.transform;
                player.AttackEnemy();

                while (!player.IsIdle())
                    yield return null;

                break;
            }

            foreach (var enemy in enemyCharacter)
            {
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
        Utility.SetCanvasGroupEnabled(buttonPanel, false);
        StartCoroutine(GameLoop());
    }
}