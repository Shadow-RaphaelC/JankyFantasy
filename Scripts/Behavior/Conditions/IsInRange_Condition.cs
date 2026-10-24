using UnityEngine;

public class IsInRange_Condition : Behavior_Condition
{
    GameObject self;
    GameObject target;
    WanderingWithAttacks_BT wanderingWithAttacks;
    float maxRange;
    public IsInRange_Condition(GameObject self, WanderingWithAttacks_BT wanderingWithAttacks, float maxRange, bool reverseCondition)
    {
        this.self = self;
        this.maxRange = maxRange;
        this.reverseCondition = reverseCondition;
        this.wanderingWithAttacks = wanderingWithAttacks;
    }
    public override bool Evaluate()
    {
        if (self != null)
        {
            target = wanderingWithAttacks.GetClosestPlayer();
            return CheckForReverseCondition(Vector3.Distance(self.transform.position, target.transform.position) <= maxRange);
        }
        return false;
    }
}
