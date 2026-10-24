using System.Threading.Tasks;
using UnityEngine;

public class Behavior_Interupt
{
    // Behavior_Interupt est une classe qui se charge de relancer la recherche dans le BT lorsqu’une condition change d’état.
    // Elle peut contenir plusieurs conditions, et si l’une d’elles change, elle relancera le BT.
    // Par exemple : relancer l’arbre si l’IA a perdu de vue le joueur.

    Behavior_Composite attachedComposite;
    Behavior_Tree behavior_Tree;
    Behavior_Condition[] behavior_Conditions;
    bool interuptSelf = false;

    bool interuptIsActive = true;

    public Behavior_Interupt(Behavior_Composite attachedComposite, bool interuptSelf)
    {
        this.attachedComposite = attachedComposite;
        this.behavior_Tree = attachedComposite.behaviorTree;
        this.behavior_Conditions = attachedComposite.conditions;
        this.interuptSelf = interuptSelf;

        CheckInterupt(); // Lance la fonction asynchrone qui restera active tant que l’IA sera en activité.
    }

    async void CheckInterupt() // Fonction qui, toutes les 0,1 secondes, vérifie si un interupt est nécessaire.
    {
        while(interuptIsActive)
        {
            await Task.Delay(100);

            if (attachedComposite.isActiveInHierarchy()) // Si le node est présentement actif, nous souhaitons vérifier que ces conditions sont toujours vrai. Sinon, interupt (Seulement si "interuptSelf" est actif!)
            {
                if (interuptSelf)
                {
                    bool result = true;
                    for (int i = 0; i < behavior_Conditions.Length; i++) // Regarde que tous les conditions sont vrai
                    {
                        if (!behavior_Conditions[i].Evaluate())
                        {
                            result = false;
                            break;
                        }
                    }

                    if(!result)
                    {
                        Debug.Log("Interupt Self");
                        behavior_Tree.Interupt(); // Arrête l’action et relance le BT.
                    }
                }
            }
            else // Si n'est pas actif dans la hierarchy ET que la/les conditons sont vrai ET que la priorité du node actif est plus basse : INTERUPT
            {
                bool result = true;
                for (int i = 0; i < behavior_Conditions.Length; i++) // Regarde que tous les conditions sont vrai
                {
                    if (!behavior_Conditions[i].Evaluate())
                    {
                        result = false;
                        break;
                    }
                }

                if (result && attachedComposite.priorityIndex < behavior_Tree.activeNode.parentComposite.priorityIndex) // Si résultat est vrai ET que la priorité est plus élevé (chiffre plus bas) que le node actif
                {
                    Debug.Log("Interupt");
                    behavior_Tree.Interupt(); // Arrête l’action et relance le BT.
                }

            }
        }
    }

    public void StopInterupt()
    {
        interuptIsActive = false;
    }

}
