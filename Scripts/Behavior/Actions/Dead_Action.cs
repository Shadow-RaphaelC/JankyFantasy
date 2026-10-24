using UnityEngine;

public class Dead_Action : Behavior_Node
{
    GameObject owner;
    public Dead_Action(Behavior_Condition[] conditions, GameObject owner) : base(conditions)
    {
        this.owner = owner;
    }
    public override void Tick(float deltaTime)
    {
        //TODO Animation pour la mort du boss 
        owner.GetComponent<Animator>().SetTrigger("death");

    }
    override public void ExecuteAction(Behavior_Composite parentComposite)
    {
        base.ExecuteAction(parentComposite);
    }
}
