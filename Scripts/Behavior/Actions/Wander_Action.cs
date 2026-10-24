using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Wander_Action : Behavior_Node
{
    NavMeshAgent agent;
    IsOnCooldown_Condition cooldown;
    float roamingRange;
    WanderingWithAttacks_BT wanderingWithAttacks;
    public Wander_Action(Behavior_Condition[] conditions, IsOnCooldown_Condition cooldown, WanderingWithAttacks_BT wanderingWithAttacks ,NavMeshAgent agent, float roamingRange) : base( conditions)
    {
        this.agent = agent;
        this.cooldown = cooldown;
        this.wanderingWithAttacks = wanderingWithAttacks;
        this.roamingRange = roamingRange;
    }

    public override void Tick(float deltaTime)
    {
        if ((agent.gameObject.transform.position - agent.destination).magnitude <= .75f && !interupted)
        {
            
            FinishAction(true);
            cooldown.ActiverCooldown();
            return;
        }
    }

    override public void ExecuteAction(Behavior_Composite parentComposite)
    {
        base.ExecuteAction(parentComposite);
        GameObject closest = wanderingWithAttacks.GetClosestPlayer();
        Vector3 direction;

        int attempts = 0;
        do
        {
            direction = new Vector3(Random.Range(-roamingRange, roamingRange), 0, Random.Range(-roamingRange, roamingRange));
            attempts++;
        }
        while (Physics.CapsuleCast(agent.transform.position + new Vector3(0, agent.height / 2, 0), agent.transform.position - new Vector3(0, agent.height / 2, 0), 2, direction) && attempts < 20);
         
        agent.destination = agent.transform.position + direction;
        agent.transform.LookAt(closest.transform.position);
            


    }


    public override void Interupt()
    {
        agent.destination = agent.transform.position;
        base.Interupt();
    }



}
