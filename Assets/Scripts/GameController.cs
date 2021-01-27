using System;
using System.Collections;
using System.Linq;
using UnityEngine;

internal sealed class GameController : MonoBehaviour
{
    public UiController uiController;
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
        uiController.ShowGameResult(true);
    }

    void PlayerLost()
    {
        Debug.Log("Player lost.");
        uiController.ShowGameResult(false);
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

    public void PlayerAttack()
    {
        waitingForInput = false;
    }

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
                if (player.IsDead())
                    continue;
                
                currentTarget = FirstAliveCharacter(enemyCharacter);
                if (currentTarget == null)
                    break;

                currentTarget.targetIndicator.gameObject.SetActive(true);
                uiController.controlMenu.Show(true);

                waitingForInput = true;
                while (waitingForInput)
                    yield return null;
                
                uiController.controlMenu.Hide(true);
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
        uiController.SetControlCallbacks(NextTarget, PlayerAttack);
        StartCoroutine(GameLoop());
    }
}
