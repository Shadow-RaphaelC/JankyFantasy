using UnityEngine;
using UnityEngine.AI;

public class RotateToFaceObject_Action : Behavior_Node
{
    float degreePerSecond;
    NavMeshAgent agent;
    GameObject target;
    WanderingWithAttacks_BT wanderingWithAttacks;
    public RotateToFaceObject_Action(Behavior_Condition[] conditions, float degreePerSecond, NavMeshAgent agent,WanderingWithAttacks_BT wanderingWithAttacks) : base(conditions)
    {
        this.degreePerSecond = degreePerSecond;
        this.agent = agent;
        this.wanderingWithAttacks = wanderingWithAttacks;
    }

    public override void Tick(float deltaTime)
    {
        if (interupted)
            return;
        if (Mathf.Abs((agent.transform.rotation.eulerAngles.y - Quaternion.LookRotation(target.transform.position - agent.transform.position, Vector3.up).eulerAngles.y)) <= 5f && !interupted)
        {
            FinishAction(true);
            interupted = true;
        }
        else
        {
            agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, Quaternion.LookRotation(target.transform.position - agent.transform.position, Vector3.up), degreePerSecond * deltaTime);
        }
    }
    override public void ExecuteAction(Behavior_Composite parentComposite)
    {
        target = wanderingWithAttacks.GetClosestPlayer();
        base.ExecuteAction(parentComposite);
    }
}
