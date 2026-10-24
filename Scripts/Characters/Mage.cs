using UnityEngine;

public class Mage : Character
{
    [Header("Mage - Elements")]
    [SerializeField] private Element[] selectElements = { Element.Fire, Element.Ice, Element.Lightning };
    private int elementIndex = 0;

    [Header("Mage Shooting")]
    [SerializeField] private float bulletDamage = 20f;

    [Header("Mage Color Balls")] // pause
    [SerializeField] private PlayerBullet fireBulletPrefab;
    [SerializeField] private PlayerBullet iceBulletPrefab;
    [SerializeField] private PlayerBullet lightningBulletPrefab;

    public Element CurrentElement => selectElements[elementIndex];

    public void CycleElement()
    {
        elementIndex = (elementIndex + 1) % selectElements.Length;
    }

    private PlayerBullet GetBulletPrefab()
    {
        switch (CurrentElement)
        {
            case Element.Ice: return iceBulletPrefab;
            case Element.Lightning: return lightningBulletPrefab;
            default: return fireBulletPrefab;
        }
    }

    protected override void DoAttackPressed()
    {
        if (!CanAttack) return;

        if (animator != null) animator.SetTrigger("attack");

        Vector3 spawnPos = transform.position + Vector3.up * 1.2f;

        PlayerBullet prefab = GetBulletPrefab();
        PlayerBullet bullet = Instantiate(prefab, spawnPos, transform.rotation);

        bullet.Setup(bulletDamage, CurrentElement, this);
        audioSource.PlayOneShot(attackSound);

        StartAttackCooldown();
    }
}