using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum State
    {
        Idle,
        RunningToEnemy,
        RunningFromEnemy,
        BeginAttack,
        Attack,
        BeginShoot,
        Shoot,
        Dead
    }

    public enum Weapon
    {
        Pistol,
        Bat,
        Fist
    }

    Animator animator;
    State state;
    private Character _enemy;

    public Weapon weapon;
    public Character target;
    public float runSpeed;
    public float distanceFromEnemy;
    Vector3 originalPosition;
    Quaternion originalRotation;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        state = State.Idle;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void SetState(State newState)
    {
        state = newState;
    }
    
    public void PerformAttack()
    {
        target.Attacked();
    }
    
    void Attacked()
    {
        SetState(Character.State.Dead);
    }

    [ContextMenu("Attack")]
    void AttackEnemy()
    {
        if (state == State.Dead)
            return;

        if (target.IsDead())
            return;
        
        switch (weapon) {
            case Weapon.Bat:
            case Weapon.Fist:
                SetState(State.RunningToEnemy);
                break;
            
            case Weapon.Pistol:
                SetState(State.BeginShoot);
                break;
        }
    }

    bool RunTowards(Vector3 targetPosition, float distanceFromTarget)
    {
        Vector3 distance = targetPosition - transform.position;
        if (distance.magnitude < 0.00001f) {
            transform.position = targetPosition;
            return true;
        }

        Vector3 direction = distance.normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        targetPosition -= direction * distanceFromTarget;
        distance = (targetPosition - transform.position);

        Vector3 step = direction * runSpeed;
        if (step.magnitude < distance.magnitude) {
            transform.position += step;
            return false;
        }

        transform.position = targetPosition;
        return true;
    }

    void FixedUpdate()
    {
        switch (state) {
            case State.Idle:
                transform.rotation = originalRotation;
                animator.SetFloat("Speed", 0.0f);
                break;

            case State.RunningToEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(target.transform.position, distanceFromEnemy))
                    SetState(State.BeginAttack);
                break;

            case State.RunningFromEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(originalPosition, 0.0f))
                    SetState(State.Idle);
                break;

            case State.BeginAttack:
                if(weapon == Weapon.Bat)
                    animator.SetTrigger("MeleeAttack");
                else if(weapon == Weapon.Fist)
                    animator.SetTrigger("FistAttack");
                SetState(State.Attack);
                break;

            case State.Attack:
                break;

            case State.BeginShoot:
                animator.SetTrigger("Shoot");
                SetState(State.Shoot);
                break;

            case State.Shoot:
                break;
            
            case State.Dead:
                animator.SetTrigger("Dead");
                break;
        }
    }

    bool IsDead()
    {
        return state == State.Dead;
    }
}
