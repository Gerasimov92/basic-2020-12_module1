using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    Character character;

    void Start()
    {
        character = GetComponentInParent<Character>();
    }

    void ShootEnd()
    {
        character.PerformAttack();
        character.SetState(Character.State.Idle);
    }

    void AttackEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
    }

    void AttackEvent()
    {
        character.PerformAttack();
    }
}
