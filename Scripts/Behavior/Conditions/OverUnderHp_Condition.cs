using UnityEngine;

public class OverUnderHp_Condition : Behavior_Condition
{
    IDamageable damageable;
    float maxHp;
    // Précondition: l'utiliser comme un pourcentage. Exemples: 0.15f, 0.5f, etc.
    float limite;
    public OverUnderHp_Condition(IDamageable damageable,float maxHp,float limite, bool reverseCondition)
    {
        this.damageable = damageable;
        this.maxHp = maxHp;
        this.limite = limite;
        this.reverseCondition = reverseCondition;
    }
    public override bool Evaluate()
    {
        // J'utilise une multiplication au lieu d'une division pour qu'on le fasse comme un pourcentage, car diviser par 6.67f pour faire 15% n'est pas optimal.
        if (damageable.CurrentHP < maxHp * limite)
            return CheckForReverseCondition(false);
        return CheckForReverseCondition(true);
    }

}
