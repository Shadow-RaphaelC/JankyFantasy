using UnityEngine;

public class TestDummy : Combattant
{
    protected override void Die()
    {
        base.Die();
        Debug.Log($"{name} est mort !");
    }

    public override void TakeDamage(float amount, Element element, Combattant Enemy, AudioClip audio = null)
    {
        base.TakeDamage(amount, element, Enemy, audio);
        Debug.Log($"{name} a pris {amount} dégâts ({element}). PV restants: {CurrentHP}");
    }
}