using UnityEngine;

public abstract class Character : Combattant
{
    [Header("DBNO (Down But Not Out)")]
    [SerializeField] protected float reviveHPPercent = 0.3f;

    [Header("Attack Rate")]
    [SerializeField] protected float attackCooldown = 0.8f;
    protected float attackTimer = 0f;

    public bool CanAttack => attackTimer <= 0f && !IsDowned && !IsDead;


    protected bool isAttacking = false;

    public bool IsDowned { get; protected set; }

    protected override void Update()
    {
        base.Update();
        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;
    }

    public override void SetMovementInput(float forward, float rotation)
    {
        if (IsDowned || IsDead)
        {
            base.SetMovementInput(0f, 0f);
            return;
        }
        base.SetMovementInput(forward, rotation);
    }

    protected void StartAttackCooldown()
    {
        attackTimer = attackCooldown;
    }

    // Attaque de base
    public virtual void OnAttackPressed()
    {
        if (IsDowned || IsDead) return;
        DoAttackPressed();
    }

    public virtual void OnAttackReleased()
    {
        if (IsDowned || IsDead) return;
        DoAttackReleased();
    }

    protected virtual void DoAttackPressed() { }
    protected virtual void DoAttackReleased() { }

    public void OnAttackFinished() => isAttacking = false;

    protected override void Die()
    {
        IsDowned = true;
        forwardInput = 0f;
        rotationInput = 0f;
        isAttacking = false;

        if (animator != null)
            animator.SetTrigger("death");
    }

    public virtual void ReviveDowned()
    {
        IsDowned = false;
        CurrentHP = maxHP * reviveHPPercent;

        if (animator != null)
            animator.SetTrigger("revive");
    }
}
