using System.Linq;
using System.Collections;
using UnityEngine;

abstract public class Behavior_Tree : MonoBehaviour
{
    public bool stopBehavior = false;
    private int priorityIndex = 0;
    // Classe abstraite qui est la base de tout Behavior Tree. Elle implémente les fonctions de base présentes dans tout BT.

    protected Behavior_Composite root; // La racine d'un Behavior Tree sera toujours un Composite. Souvent un Selector, mais pas uniquement. Cette variable devra être créée dans chaque enfant de BT.
    public Behavior_Node activeNode; // L’activeNode représente l’action que le BT est actuellement en train d’exécuter. Il sert à lancer les Tick sur les Actions.
    abstract public void InitializeTree(); // InitializeTree est la fonction qui permet de créer le BT. Elle contient la logique de l’arbre entier, incluant les Interrupts.

    private void Update()
    {
        if (activeNode != null)
            activeNode.Tick(Time.deltaTime); // Réalise le Tick de l’action en cours.
    }
    public void RunTree()
    {
        if(!stopBehavior)
            root.ExecuteAction(null); // RunTree doit toujours simplement lancer l’exécution de sa racine. La racine se chargera de parcourir les nodes enfants.
    }

    private IEnumerator Start()
    {
        yield return null; // Attend que tous les Start() soient terminés
        InitializeTree();
        BuildCompositePriorityList(root);
        RunTree();
    }

    public void Interupt() // Arrête le node actuellement actif et relance le tree depuis le départ.
    {
        if(!stopBehavior)
        {
            activeNode.Interupt();
            RunTree();
        }
        
    }

    private void BuildCompositePriorityList(Behavior_Composite currentComposite) // Fonction récursive pour indexé tous les composites
    {
        currentComposite.priorityIndex = priorityIndex++; //Assigne l'index au composite et incrémente l'index

        foreach (Behavior_Composite composite in currentComposite.nodes.OfType<Behavior_Composite>()) // Parcour les enfants du node qui viens de recevoir son index et cherche les Composites enfants
            BuildCompositePriorityList(composite); // Pour chaque composite, refait la même chose.
    }
}
