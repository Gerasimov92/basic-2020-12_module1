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

    (Character, bool allDead) FirstAliveCharacter(Character[] characters)
    {
        var ch = characters.FirstOrDefault(character => !character.IsDead());
        return (ch, object.ReferenceEquals(ch, null));
    }

    void PlayerWon()
    {
        uiController.ShowGameResult(true);
    }

    void PlayerLost()
    {
        uiController.ShowGameResult(false);
    }

    bool CheckEndGame()
    {
        var (_, allDead) = FirstAliveCharacter(playerCharacter);
        if (allDead) {
            PlayerLost();
            return true;
        }

        (_, allDead) = FirstAliveCharacter(enemyCharacter);
        if (allDead) {
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

                bool allDead;
                (currentTarget, allDead) = FirstAliveCharacter(enemyCharacter);
                if (allDead)
                    break;

                currentTarget.targetIndicator.gameObject.SetActive(true);
                uiController.controlMenu.Show(true);

                waitingForInput = true;
                while (waitingForInput)
                    yield return null;
                
                uiController.controlMenu.Hide(true);
                currentTarget.targetIndicator.gameObject.SetActive(false);

                player.AttackEnemy(currentTarget);

                while (!player.IsIdle())
                    yield return null;

                break;
            }

            foreach (var enemy in enemyCharacter)
            {
                if (enemy.IsDead())
                    continue;
                
                var (target, allDead) = FirstAliveCharacter(playerCharacter);
                if (allDead)
                    break;

                enemy.AttackEnemy(target);

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
