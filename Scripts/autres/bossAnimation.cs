using System.Collections;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    // Declaration des portées
    [SerializeField] float meleeRange = 3f;
    [SerializeField] float meleeDamage = 10f;

    [SerializeField] GameObject prefabFireball;
    [SerializeField] GameObject prefabRockSummon;
    [SerializeField] GameObject prefabHomingSkull;
    [SerializeField] GameObject prefabLightBeam;
    [SerializeField] float lightBeamDuration = 3f;
    [SerializeField] float lightBeamRotationDelay = 0.15f;
    [SerializeField] int nbFireballs = 5;
    [SerializeField] float coneAngle = 80f;
    [SerializeField] int rockBurst = 3;
    [SerializeField] int homingBurst = 3;
    [SerializeField] float timeBetweenRockSummons = 0.2f;
    [SerializeField] float homingSpawnOffset = 1.5f;
    [SerializeField] int healAmount = 200;
    GameObject target;
    WanderingWithAttacks_BT wanderingWithAttacks;
    Boss boss;
    GameObject spawnBullet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wanderingWithAttacks = gameObject.GetComponent<WanderingWithAttacks_BT>();
        boss = gameObject.GetComponent<Boss>();
        spawnBullet = GameObject.FindGameObjectWithTag("SpawnBullet");
    }
    //public void MeleeSlap()
    //{
    //    // Fonctions pour détecter tous les colliders dans le range
    //    // Physics.OverlapSphere
    //    // Unity offical documentation
    //    // https://docs.unity3d.com/ScriptReference/Physics.OverlapSphere.html
    //    Collider[] hits = Physics.OverlapSphere(transform.position, meleeRange);
    //    foreach (Collider hit in hits)
    //    {
    //        IDamageable damageable = hit.GetComponent<IDamageable>();
    //        if(damageable != null)
    //        {
    //            damageable.TakeDamage(meleeDamage, Element.None, null);
    //        }
    //    }

    //}
    public void ConeFireballs()
    {
        target = wanderingWithAttacks.GetClosestPlayer();
        Vector3 directionTarget = (target.transform.position - transform.position).normalized;
        float angleEntreBullet = coneAngle / nbFireballs;

        for (int i = 0; i < nbFireballs; i++)
        {
            Instantiate(prefabFireball, spawnBullet.transform.position,Quaternion.LookRotation(directionTarget) * Quaternion.Euler(0,- ((nbFireballs - 1) / 2f * angleEntreBullet) + (i * angleEntreBullet),0)); 
        }
    }
    public void RockSummons()
    {
        StartCoroutine(RockSummonsCoroutine());
    }
    public void HomingSkulls()
    {
        Vector3[] playerPositions = wanderingWithAttacks.GetAllPlayersPosition();
        for (int j = 0; j < playerPositions.Length; j++)
        {
            for (int i = 1; i < homingBurst; i++)
                Instantiate(prefabHomingSkull, playerPositions[j] + Vector3.right * homingSpawnOffset * i, Quaternion.identity);
        }
    }
    public void LightBeam()
    {
        StartCoroutine(LightBeamCoroutine());
    }
    public void Heal()
    {
        float currentHp = boss.CurrentHP;
        currentHp = currentHp + healAmount;
        boss.currentHp = currentHp;
    }

    IEnumerator RockSummonsCoroutine()
    {
        for (int i = 0; i < rockBurst; i++)
        {
            Vector3[] playerPositions = wanderingWithAttacks.GetAllPlayersPosition();
            for (int j = 0; j < playerPositions.Length; j++)
            {
                Instantiate(prefabRockSummon, playerPositions[j] , Quaternion.identity);
            }
            yield return new WaitForSeconds(timeBetweenRockSummons);
        }
    }
    IEnumerator LightBeamCoroutine()
    {
        //Le dernier transform agit pour que le beam devient un enfant du Boss
        GameObject beam = Instantiate(prefabLightBeam, spawnBullet.transform.position, transform.rotation, transform);
        float time = 0f;
        while (time < lightBeamDuration)
        {
            target = wanderingWithAttacks.GetClosestPlayer();
            Vector3 positionRetarde = target.transform.position;
            yield return new WaitForSeconds(lightBeamRotationDelay);
            transform.LookAt(positionRetarde);
            time += lightBeamRotationDelay;
        }
        Destroy(beam);
    }
}
