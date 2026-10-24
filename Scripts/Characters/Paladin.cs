using UnityEngine;

public class Paladin : Character
{
    [Header("Paladin Attack")]
    [SerializeField] private float meleeDamage = 25f;
    [SerializeField] private float meleeRadius = 2f;
    [SerializeField] private Transform hitPoint;

    [Header("Paladin Shield")]
    [SerializeField] private float moveMaxToShield = 0.1f;
    [SerializeField] private GameObject blockIndicator;
    [SerializeField] private AudioClip blockSound;



    public bool IsBlocking =>
        !IsDowned && !IsDead && !isAttacking &&
        Mathf.Abs(forwardInput) < moveMaxToShield &&
        Mathf.Abs(rotationInput) < moveMaxToShield;
    // ABS est 0 ou 1 pour block

    protected override void Update()
    {
        base.Update();
        if (blockIndicator != null && blockIndicator.activeSelf != IsBlocking)
            blockIndicator.SetActive(IsBlocking);
    }

    protected override void DoAttackPressed()
    {
        if (!CanAttack) return;

        isAttacking = true;

        if (animator != null) animator.SetTrigger("attack");

        audioSource.PlayOneShot(attackSound);

        DoMeleeHit();
        StartAttackCooldown();
    }

    private void DoMeleeHit()
    {
        // https://docs.unity3d.com/ScriptReference/Physics.OverlapSphere.html

        Collider[] hits = Physics.OverlapSphere(hitPoint.position, meleeRadius);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            Combattant other = hit.GetComponent<Combattant>();
            if (other != null && other.Team == this.Team) continue;

            IDamageable target = hit.GetComponent<IDamageable>();
            if (target != null)
            {
                audioSource.PlayOneShot(attackHitSound);
                target.TakeDamage(meleeDamage, Element.None, this);
            }
                
        }
    }

    protected override float IncomingDamage(float amount, Element element)
    {
        if (IsBlocking)
        {
            audioSource.PlayOneShot(blockSound);
            return 0f; // bloque tout
            
        }
            

        return base.IncomingDamage(amount, element);
    }
}
