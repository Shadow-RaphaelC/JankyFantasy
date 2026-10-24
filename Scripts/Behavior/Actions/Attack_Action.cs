using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attack_Action : Behavior_Node
{
    GameObject attackObject;
    GameObject owner;

    float timeInAttack;
    public List<Material> materials = new List<Material>();

    public Attack_Action(Behavior_Condition[] conditions, GameObject attackObject, GameObject owner) : base(conditions)
    {
        this.attackObject = attackObject;
        this.owner = owner;

        foreach (MeshRenderer m in owner.GetComponentsInChildren<MeshRenderer>())
            materials.Add(m.material);
    }

    public override void Tick(float deltaTime)
    {
        if (interupted)
            return;

        timeInAttack += deltaTime;

        if (timeInAttack < 0.2f && !interupted)
        {
            foreach (Material m in materials)
                m.color = Color.red;
        }
        else if (timeInAttack < 0.3f && !interupted)
        {
            foreach (Material m in materials)
                m.color = Color.white;

            if (attackObject != null)
                GameObject.Instantiate(attackObject, owner.transform.position, owner.transform.rotation);
            FinishAction(true);
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
        foreach (Material m in materials)
            m.color = Color.white;
        base.Interupt();
    }
}
