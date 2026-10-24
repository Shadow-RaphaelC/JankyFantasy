using UnityEngine;

public class MeleeSlap_Action : Behavior_Node
{
    // Declarations
    GameObject owner;
    IsOnCooldown_Condition cooldown;
    float timeInAttack;
    // TODO ANIMATION

    public MeleeSlap_Action(Behavior_Condition[] conditions, IsOnCooldown_Condition cooldown,GameObject owner) : base(conditions)
    {
        this.cooldown = cooldown;
        this.owner = owner;
    }
    public override void Tick(float deltaTime)
    {
        if (interupted)
            return;

        timeInAttack += deltaTime;

        if (timeInAttack < 0.2f && !interupted)
        {
            //TODO Animation pour l'attaque slap 
            owner.GetComponent<Animator>().SetTrigger("slap");

            // Link le take damage a la fin de l'animation
        }
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
