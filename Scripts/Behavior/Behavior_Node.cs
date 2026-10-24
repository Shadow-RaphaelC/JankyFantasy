using System.Linq;
using UnityEngine;
using static Behavior_Composite;

public class Behavior_Node
// Classe de base qui sera le parent de tout node (Action, Composite).
// Elle est responsable des éléments communs aux nodes, tels que la gestion des conditions, l’exécution d’un node et d’autres comportements de base.
{
    public Behavior_Composite parentComposite;
    public Behavior_Condition[] conditions;
    protected bool interupted = false;

    public Behavior_Node(Behavior_Condition[] conditions) // Le seul élément commun à tous les nodes est la possibilité d’avoir des conditions d’exécution.
    {
        this.conditions = conditions;
    }

    public bool EvaluateConditions() // EvaluateConditions n’est pas Virtual, car il sera toujours identique peu importe le type de node.
                                     // Son rôle est de parcourir la liste des conditions et de retourner True si elles sont respectées.
    {
        if (conditions == null)
            return true;

        foreach (Behavior_Condition condition in conditions)
            if (!condition.Evaluate())
                return false;

        return true;
    }

    virtual public void ExecuteAction(Behavior_Composite parentComposite) // ExecuteAction devra être Override dans les enfants.
                                                                          // La base est de réinitialiser l’interruption, de s’associer à son parent composite (si ce n’est pas la racine)
                                                                          // et de définir ce node comme actif.
    {
        interupted = false;
        this.parentComposite = parentComposite;
        SetNodeAsActive();
    }

    virtual public void FinishAction(bool result) // FinishAction doit être appelée uniquement si le node n’a pas été interrompu.
    {
        if (!interupted)
            parentComposite.FinishAction(result);
    }

    virtual public void Interupt() // Marque le node comme interrompu et appelle FinishAction.
    {
        interupted = true;
        FinishAction(false);
    }

    virtual public void Tick(float deltaTime) // Tick sera Override dans chaque Action et appelé par le BT, comme un Update() dans un MonoBehaviour.
    {
    }

    void SetNodeAsActive() // Met à jour le BT avec le node actif uniquement si la fonction est appelée par une Action (pas par un Composite).
    {
        if (parentComposite == null || this is Behavior_Composite)
            return;

        parentComposite.behaviorTree.activeNode = this;
    }

    public bool isActiveInHierarchy() // Retourne vrai si le node OU un enfant du node est présentement le "activeNode" du BT
    {
        if (parentComposite == null) // Si le parent composite est null, la node n'est pas encore utiliser dans le BT
            return false;


        Behavior_Node currentActiveNode = parentComposite.behaviorTree.activeNode; // On va chercher le Active Node

        if (currentActiveNode == this) // Si ce node est le active Node, nous retournons vrai
            return true;

        if(this is Behavior_Composite) // Si le node est un composite, nous souhaitons chercher parmis les enfants pour le node actif
        {
            bool result = false;
            foreach (Behavior_Node childNode in ((Behavior_Composite)this).nodes) // Typecast notre node en tant que Composite pour atteindre ces enfants
            {
                if (childNode.isActiveInHierarchy()) // Si le node enfant est actif dans la hierarchy, nous retournons vrai (Ici c'est une fonction récursive)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        return false; // Si rien n'a retourné vrai parmi les nodes et leurs enfants, le node n'est pas actif dans la hierarchy.
    }
}