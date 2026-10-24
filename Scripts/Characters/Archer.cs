using Unity.VisualScripting;
using UnityEngine;

public class Archer : Character
{
    [Header("Archer Shooting")]
    [SerializeField] private PlayerBullet bulletPrefab;
    [SerializeField] private float minDamage = 40f;
    [SerializeField] private float maxDamage = 70f;

    private float chargeSpeedModifier = 0.5f;

    private bool isCharging = false;
    private float chargeTime = 0f;

    // Press
    protected override void DoAttackPressed()
    {
        if (!CanAttack) return;

        isCharging = true;
        animator.SetBool("isCharging", true);
        chargeTime = Time.time;

        speedModifier = chargeSpeedModifier;
    }

    // Release
    protected override void DoAttackReleased()
    {
        if (!isCharging) return;
        isCharging = false;

        speedModifier = 1f;

        float held = Time.time - chargeTime;

        float dmg;
        if (held <= 1f)
            dmg = minDamage;

        else if (held <= 2f)
            dmg = minDamage + 10f;

        else
            dmg = maxDamage;

        animator.SetBool("isCharging", false);
        if (animator != null) animator.SetTrigger("attack");

        Vector3 spawnPos = transform.position + Vector3.up * 1.2f;
        PlayerBullet arrow = Instantiate(bulletPrefab, spawnPos, transform.rotation);
        arrow.Setup(dmg, Element.None, this);
        audioSource.PlayOneShot(attackSound);

        StartAttackCooldown();
    }

    protected override void Die()
    {
        base.Die();
        isCharging = false;
        speedModifier = 1f;
    }
}
