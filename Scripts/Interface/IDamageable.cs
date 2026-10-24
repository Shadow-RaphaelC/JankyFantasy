using UnityEngine;

public interface IDamageable
{
    float CurrentHP { get; }
    void TakeDamage(float amount, Element element, Combattant Enemy, AudioClip audio = null);
}
