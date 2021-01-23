using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    public Animation shootAnimation;
    Character character;

    void Start()
    {
        character = GetComponentInParent<Character>();
    }

    void ShootEnd()
    {
        character.SetState(Character.State.Idle);
    }

    void AttackEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
    }

    void PunchEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
    }

    void DoDamage()
    {
        if (shootAnimation) shootAnimation.Play();
        
        Character targetCharacter = character.target.GetComponent<Character>();
        targetCharacter.DoDamage();
    }
}
