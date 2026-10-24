using UnityEngine;

public class Heal_Action : Behavior_Node
{
    // Declarations
    float timeInAttack;
    GameObject owner;
    IsOnCooldown_Condition cooldown;
    // TODO ANIMATION

    public Heal_Action(Behavior_Condition[] conditions, IsOnCooldown_Condition cooldown, GameObject owner) : base(conditions)
    {
        this.owner = owner;
        this.cooldown = cooldown;
    }
    public override void Tick(float deltaTime)
    {
        if (interupted)
            return;

        timeInAttack += deltaTime;

        if (timeInAttack < 0.2f && !interupted)
        {
            //TODO Animation pour le HEAL 
            owner.GetComponent<Animator>().SetTrigger("heal");

            // Link le take damage a la fin de l'animation
        }
        // FIX LE TEMPS POUR L'ANIMATION
        else if (timeInAttack < 2.28f && !interupted)
        {
            FinishAction(true);
            cooldown.ActiverCooldown();
            interupted = true;
        }
    }
    override public void ExecuteAction(Behavior_Composite parentComposite)
    {
        timeInAttack = 0;
        base.ExecuteAction(parentComposite);
    }
    public override void Interupt()
    {
        base.Interupt();
    }
}
