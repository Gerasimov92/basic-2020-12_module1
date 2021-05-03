using System;
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
        BeginPunch,
        Punch,
        BeginDying,
        Dead,
    }

    public enum Weapon
    {
        Pistol,
        Bat,
        Fist,
    }

    public Weapon weapon;
    public Character target;
    public TargetIndicator targetIndicator;
    public float runSpeed;
    public float distanceFromEnemy;

    private Animator animator;
    private State state;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Health health;
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Punch = Animator.StringToHash("Punch");
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int MeleeAttack = Animator.StringToHash("MeleeAttack");
    private PlaySound soundPlayer;
    private DamageEffect damageEffect;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        state = State.Idle;
        health = GetComponent<Health>();
        targetIndicator = GetComponentInChildren<TargetIndicator>(true);
        soundPlayer = GetComponent<PlaySound>();
        damageEffect = GetComponent<DamageEffect>();
        var t = transform;
        originalPosition = t.position;
        originalRotation = t.rotation;
    }

    public bool IsIdle()
    {
        return state == State.Idle;
    }

    public bool IsDead()
    {
        return state == State.BeginDying || state == State.Dead;
    }

    public void SetState(State newState)
    {
        if (IsDead())
            return;

        state = newState;
    }

    public void DoDamage()
    {
        if (IsDead())
            return;

        if (damageEffect)
            damageEffect.ShowDamageEffect();

        if (soundPlayer)
            soundPlayer.Play("TakeDamage");

        health.ApplyDamage(1.0f); // FIXME: захардкожено
        if (health.current <= 0.0f)
            state = State.BeginDying;
    }

    public void AttackEnemy(Character enemy)
    {
        if (IsDead())
            return;

        target = enemy;
        if (target.IsDead())
            return;

        switch (weapon) {
            case Weapon.Bat:
                state = State.RunningToEnemy;
                break;
            case Weapon.Pistol:
                state = State.BeginShoot;
                break;
            case Weapon.Fist:
                state = State.RunningToEnemy;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    bool RunTowards(Vector3 targetPosition, float distanceFromTarget)
    {
        var currentTransform = transform;

        Vector3 distance = targetPosition - currentTransform.position;
        if (distance.magnitude < 0.00001f) {
            transform.position = targetPosition;
            soundPlayer.Stop();
            return true;
        }

        Vector3 direction = distance.normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        targetPosition -= direction * distanceFromTarget;
        distance = (targetPosition - currentTransform.position);

        Vector3 step = direction * runSpeed;
        if (step.magnitude < distance.magnitude) {
            transform.position += step;
            soundPlayer.Play("Steps", true);
            return false;
        }

        transform.position = targetPosition;
        soundPlayer.Stop();
        return true;
    }

    void FixedUpdate()
    {
        switch (state) {
            case State.Idle:
                transform.rotation = originalRotation;
                animator.SetFloat(Speed, 0.0f);
                break;

            case State.RunningToEnemy:
                animator.SetFloat(Speed, runSpeed);
                if (RunTowards(target.transform.position, distanceFromEnemy)){
                    switch (weapon) {
                        case Weapon.Bat:
                            state = State.BeginAttack;
                            break;

                        case Weapon.Fist:
                            state = State.BeginPunch;
                            break;
                    }
                }
                break;

            case State.BeginAttack:
                animator.SetTrigger(MeleeAttack);
                if (soundPlayer)
                    soundPlayer.Play("BatHit");
                state = State.Attack;
                break;

            case State.Attack:
                break;

            case State.BeginShoot:
                animator.SetTrigger(Shoot);
                state = State.Shoot;
                break;

            case State.Shoot:
                if (soundPlayer)
                    soundPlayer.Play("ShootHit");
                break;

            case State.BeginPunch:
                animator.SetTrigger(Punch);
                if (soundPlayer)
                    soundPlayer.Play("HandHit");
                state = State.Punch;
                break;

            case State.Punch:
                break;

            case State.RunningFromEnemy:
                animator.SetFloat(Speed, runSpeed);
                if (RunTowards(originalPosition, 0.0f))
                    state = State.Idle;
                break;

            case State.BeginDying:
                animator.SetTrigger(Death);
                if (soundPlayer)
                    soundPlayer.Play("Death");
                state = State.Dead;
                break;

            case State.Dead:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
