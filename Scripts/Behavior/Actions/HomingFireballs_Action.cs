using UnityEngine;

public class HomingFireballs_Action : Behavior_Node
{
    // Declarations
    GameObject owner;
    IsOnCooldown_Condition cooldown;
    float timeInAttack;
    // TODO ANIMATION
    public HomingFireballs_Action(Behavior_Condition[] conditions, IsOnCooldown_Condition cooldown, GameObject owner ) : base(conditions)
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
            //TODO Animation pour l'attaque de tete de meurt 
            owner.GetComponent<Animator>().SetTrigger("bullet");
            // Link le take damage a la fin de l'animation
        }
        else if (timeInAttack < 3.34f && !interupted)
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
