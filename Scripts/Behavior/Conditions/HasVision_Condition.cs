using UnityEngine;

public class HasVision_Condition : Behavior_Condition
{
    GameObject self;
    GameObject target;
    float maxAngle;
    WanderingWithAttacks_BT wanderingWithAttacks;
    public HasVision_Condition(GameObject self, WanderingWithAttacks_BT wanderingWithAttacks, float maxAngle, bool reverseCondition)
    {
        this.self = self;
        this.wanderingWithAttacks = wanderingWithAttacks;
        this.maxAngle = maxAngle;
        this.reverseCondition = reverseCondition;
    }
    public override bool Evaluate()
    {
        target = wanderingWithAttacks.GetClosestPlayer();
        Vector3 directionToTarget = target.transform.position - self.transform.position;

        directionToTarget.Normalize();

        // Validation de l'angle de vue
        float angleToTarget = Vector3.Angle(self.transform.forward, directionToTarget);
        if (angleToTarget > maxAngle / 2f)
            return CheckForReverseCondition(false);

        // Raycast pour savoir si l'AI peux voir la Target
        if (Physics.Raycast(self.transform.position, directionToTarget, out RaycastHit hit, float.MaxValue))
        {
            // Si frappe un objet qui n'est pas la target
            if (hit.collider.gameObject != target)
                return CheckForReverseCondition(false);
        }

        return CheckForReverseCondition(true);
    }

}
