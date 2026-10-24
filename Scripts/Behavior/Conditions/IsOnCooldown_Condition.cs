using UnityEngine;

public class IsOnCooldown_Condition : Behavior_Condition
{
    float cooldown;
    float time = 0;

    public IsOnCooldown_Condition(float cooldown,bool reverseCondition)
    {
        this.cooldown = cooldown;
        this.reverseCondition = reverseCondition;
    }
    public override bool Evaluate()
    {
        return CheckForReverseCondition(Time.time < time);
    }
    public void ActiverCooldown()
    {
        time = Time.time + cooldown;
    }
}
