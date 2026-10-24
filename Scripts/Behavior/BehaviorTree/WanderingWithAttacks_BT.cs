using UnityEngine;
using UnityEngine.AI;
public class WanderingWithAttacks_BT : Behavior_Tree
{
    // Declaration
    [SerializeField] float visionAngle = 140;
    [SerializeField] int waitTime = 1;
    [SerializeField] float wanderRange = 50f;
    [SerializeField] float maxHp = 100;
    [SerializeField] float healWaitTime = 0.5f;
    [SerializeField] float pourcentageToTriggerHeal = 0.2f;
    NavMeshAgent agent;
    GameObject[] players;
    IDamageable damageable;
    // Prefab
    [SerializeField] GameObject firePrefab;
    [SerializeField] GameObject skullPrefab;
    
    // Portée pour les attaques
    [SerializeField] float meleeRange = 3.5f;
    [SerializeField] float coneFireballsRange = 10.0f;
    [SerializeField] float homingFireballsRange = 20.0f;
    [SerializeField] float healRange = 50.0f;
    [SerializeField] float lightBeamRange = 20.0f;
    [SerializeField] float rockSummonsRange = 20.0f;
    // Temps de recharge pour les attaques
    [SerializeField] float meleeCooldown = 3.5f;
    [SerializeField] float coneFireballsCooldown = 7.0f;
    [SerializeField] float homingFireballsCooldown = 10.0f;
    [SerializeField] float lightBeamCooldown = 5.5f;
    [SerializeField] float rockSummonsCooldown = 5.5f;
    [SerializeField] float healCooldown = 1f;
    [SerializeField] float wanderCooldown = 3f;
    [SerializeField] float wanderCooldownb = 3f;

    // Conditions de temps de recharge pour les attaques. Pour que je puisse les envoyer aux actions.
    //IsOnCooldown_Condition meleeCD;
    IsOnCooldown_Condition coneFireballsCD;
    IsOnCooldown_Condition homingFireballsCD;
    IsOnCooldown_Condition lightBeamCD;
    IsOnCooldown_Condition rockSummonsCD;
    IsOnCooldown_Condition healCD;
    IsOnCooldown_Condition wanderCD;
    IsOnCooldown_Condition wanderCDb;
    // Tree Actions
    Wander_Action wander_Action;
    Wander_Action wanderRage_Action;
    Wait_Action wait_Action;
    Dead_Action dead_Action;

    // Attendre pour chaque attaques
    //Wait_Action meleeWait;
    Wait_Action coneWait;
    Wait_Action homingWait;
    Wait_Action lightBeamWait;
    Wait_Action rockSummonsWait;
    Wait_Action healWait;
    Wait_Action RageWanderWait;
    // rotation vers l'objet pour chaque attaques
    RotateToFaceObject_Action meleeRotate;
    RotateToFaceObject_Action coneRotate;
    RotateToFaceObject_Action homingRotate;
    RotateToFaceObject_Action wanderRotate;
    RotateToFaceObject_Action wanderRotateb;
    RotateToFaceObject_Action beamRotate;
    RotateToFaceObject_Action rockRotate;
    //==================== Attques Actions ====================\\
    // Phase noramale
    HomingFireballs_Action homingFireballs_Action;
    ConeFireballs_Action coneFireballs_Action;
    MeleeSlap_Action meleeSlap_Action;
    //phase rage
    Heal_Action heal_Action;
    LightBeam_Action lightBeam_Action;
    RockSummons_Action rockSummons_Action;
    //==================== Tree Composites ====================\\
    // Selectors
    Behavior_Composite selector_1; // Plus de 50% 
    Behavior_Composite selector_2; // Moins de 50%

    // Sequencers
    //Behavior_Composite sequencer_1; // Melee
    Behavior_Composite sequencer_2; // ConeFireballs
    Behavior_Composite sequencer_3; // HomingFireballs
    Behavior_Composite sequencer_4; // Wander
    Behavior_Composite sequencer_4b;// Wander
    Behavior_Composite sequencer_5; // Heal
    Behavior_Composite sequencer_6; // LightBeam
    Behavior_Composite sequencer_7; // RockSummons

    public override void InitializeTree() 
    {
        // Association des variables à leur valeur
        agent = gameObject.GetComponent<NavMeshAgent>();
        damageable = gameObject.GetComponent<IDamageable>();
        // Association des conditions temps de recharge pour les attaques.
        //meleeCD = new IsOnCooldown_Condition(meleeCooldown, true);
        coneFireballsCD = new IsOnCooldown_Condition(coneFireballsCooldown, true);
        homingFireballsCD = new IsOnCooldown_Condition(homingFireballsCooldown, true);
        lightBeamCD = new IsOnCooldown_Condition(lightBeamCooldown,true);
        rockSummonsCD = new IsOnCooldown_Condition(rockSummonsCooldown, true);
        healCD = new IsOnCooldown_Condition(healCooldown,true);
        wanderCD = new IsOnCooldown_Condition(wanderCooldown, true);
        wanderCDb = new IsOnCooldown_Condition(wanderCooldownb, true);
        // Declaration de dead_Action
        dead_Action = new Dead_Action(new Behavior_Condition[] { new OverUnderHp_Condition(damageable, maxHp, 0.001f, true) },gameObject);
       
        //==================== Actions Set #1 ====================\\
        // Actions pour MeleeSlap
        //meleeRotate = new RotateToFaceObject_Action(null, 180, agent, this);
        //meleeSlap_Action = new MeleeSlap_Action(new Behavior_Condition[] { new IsInRange_Condition(gameObject, this, meleeRange, false),
        //    new HasVision_Condition(gameObject, this, visionAngle, false),meleeCD},meleeCD, gameObject);
        //meleeWait = new Wait_Action(null, waitTime);

        //==================== Actions Set #2 ====================\\
        // Actions pour ConeFireBalls
        coneRotate = new RotateToFaceObject_Action(null, 180, agent, this);
        coneFireballs_Action = new ConeFireballs_Action(new Behavior_Condition[] { new IsInRange_Condition(gameObject,this,coneFireballsRange, false),
            new HasVision_Condition(gameObject,this,visionAngle,false),coneFireballsCD}, coneFireballsCD,gameObject);
        coneWait = new Wait_Action(null, waitTime);

        //==================== Actions Set #3 ====================\\
        // Actions pour HomingFireBalls
        homingRotate = new RotateToFaceObject_Action(null, 180, agent, this);
        homingFireballs_Action = new HomingFireballs_Action(new Behavior_Condition[] {new IsInRange_Condition(gameObject,this,homingFireballsRange,false), homingFireballsCD }, homingFireballsCD,gameObject);
        homingWait = new Wait_Action(null, waitTime);

        //==================== Actions Set #4 ====================\\
        // Actions pour Wander
        wanderRotate = new RotateToFaceObject_Action(null, 180, agent, this);
        wander_Action = new Wander_Action(new Behavior_Condition[] {wanderCD }, wanderCD, this,agent, wanderRange);
        wait_Action = new Wait_Action(null, waitTime);
        //==================== Actions Set #4b ====================\\
        // Actions pour Wander
        wanderRotateb = new RotateToFaceObject_Action(null, 180, agent, this);
        wanderRage_Action = new Wander_Action(new Behavior_Condition[] { wanderCDb }, wanderCDb, this,agent, wanderRange);
        RageWanderWait = new Wait_Action(null, waitTime);
        //==================== Actions Set #5 ====================\\
        // Actions pour Heal
        heal_Action = new Heal_Action(new Behavior_Condition[] { new HasVision_Condition(gameObject, this, visionAngle, true),
            new OverUnderHp_Condition(damageable,maxHp,pourcentageToTriggerHeal, false),
            new IsInRange_Condition(gameObject,this,healRange,true), healCD},healCD,gameObject);
        healWait = new Wait_Action(null, healWaitTime);
        //==================== Actions Set #6 ====================\\
        // Actions pour LightBeam
        beamRotate = new RotateToFaceObject_Action(null, 180, agent, this);
        lightBeam_Action = new LightBeam_Action(new Behavior_Condition[] { new HasVision_Condition(gameObject,this,visionAngle,false),
            new IsInRange_Condition(gameObject,this,lightBeamRange,false),lightBeamCD}, lightBeamCD, gameObject);
        lightBeamWait = new Wait_Action(null,waitTime + 6f);
        //==================== Actions Set #7 ====================\\
        // Actions pour RockSummons
        rockRotate = new RotateToFaceObject_Action(null, 180, agent, this);
        rockSummons_Action = new RockSummons_Action(new Behavior_Condition[] { new IsInRange_Condition(gameObject, this, rockSummonsRange, false), rockSummonsCD }, rockSummonsCD, gameObject);
        rockSummonsWait = new Wait_Action(null, waitTime);
        
        //==================== Composite level 2 ====================\\
        // (Action Set #1)
        //sequencer_1 = new Behavior_Composite(Behavior_Composite.CompositeType.Sequence, new Behavior_Node[] { meleeRotate, meleeSlap_Action, meleeWait }, this, null);
        // (Action Set #2)
        sequencer_2 = new Behavior_Composite(Behavior_Composite.CompositeType.Sequence, new Behavior_Node[] { coneRotate, coneFireballs_Action, coneWait }, this, null);
        // (Action Set #3)
        sequencer_3 = new Behavior_Composite(Behavior_Composite.CompositeType.Sequence, new Behavior_Node[] { homingRotate,homingFireballs_Action, homingWait }, this, null);
        // (Action Set #4)
        sequencer_4 = new Behavior_Composite(Behavior_Composite.CompositeType.Sequence, new Behavior_Node[] { wanderRotate,wander_Action, wait_Action }, this, null);
        // (Action Set #4b)
        sequencer_4b = new Behavior_Composite(Behavior_Composite.CompositeType.Sequence, new Behavior_Node[] { wanderRotateb,wanderRage_Action, RageWanderWait }, this, null);
        // (Action Set #5)
        sequencer_5 = new Behavior_Composite(Behavior_Composite.CompositeType.Sequence, new Behavior_Node[] { heal_Action, healWait }, this, null);
        // (Action Set #6)
        sequencer_6 = new Behavior_Composite(Behavior_Composite.CompositeType.Sequence, new Behavior_Node[] { beamRotate,lightBeam_Action, lightBeamWait }, this, null);
        // (Action Set #7)
        sequencer_7 = new Behavior_Composite(Behavior_Composite.CompositeType.Sequence, new Behavior_Node[] { rockRotate,rockSummons_Action, rockSummonsWait }, this, null);

        //==================== Composite level 1 ====================\\
        // Composite level 1 (Action Sets #1 to #4) - With Interupt
        selector_1 = new Behavior_Composite(Behavior_Composite.CompositeType.Selector, new Behavior_Node[] { sequencer_2, sequencer_3, sequencer_4 }, this,
            new Behavior_Condition[] {new OverUnderHp_Condition(damageable,maxHp,0.5f,false)}, true, true);
        // Composite level 1 (Action Sets #5 to #7) - With Interupt
        selector_2 = new Behavior_Composite(Behavior_Composite.CompositeType.Selector, new Behavior_Node[] { sequencer_5, sequencer_7, sequencer_6, sequencer_4b }, this,
            new Behavior_Condition[] { new OverUnderHp_Condition(damageable, maxHp, 0.5f, true) }, true, true);
        // Root Composite
        root = new Behavior_Composite(Behavior_Composite.CompositeType.Selector, new Behavior_Node[] { dead_Action, selector_1, selector_2 }, this, null);
        
    }
    public  GameObject GetClosestPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player"); // refresh à chaque call
        if (players == null || players.Length == 0) return null;

        GameObject player = players[0];
        for(int i = 0; i < players.Length; i++)
            if(players[i] != null)
            {
                if( Vector3.Distance(players[i].transform.position, agent.transform.position) < Vector3.Distance(player.transform.position, agent.transform.position))
                    player = players[i];
            }
        return player;
    }
    public Vector3[] GetAllPlayersPosition()
    {
        players = GameObject.FindGameObjectsWithTag("Player"); // refresh à chaque call
        if (players == null || players.Length == 0) return null;

        Vector3[] playerPositions = new Vector3[players.Length];
        for (int i = 0; i < players.Length; i++)
            if (players[i] != null)
            {
                playerPositions[i] = players[i].transform.position;
            }
        return playerPositions;
    }
}
