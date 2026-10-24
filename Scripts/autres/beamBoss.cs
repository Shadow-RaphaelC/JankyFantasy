using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class beamBoss : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float damage = 15;
    [SerializeField] float timeBetweenTick = 0.8f;
    bool canTakeDamage = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (canTakeDamage)
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(damage, Element.None, null);
                canTakeDamage = false;
                StartCoroutine(damageCooldown());
            }
               
            
        }
        
    }

    //permet de wait pour pas se fair oneShot par le beam
    private IEnumerator damageCooldown()
    {
        yield return new WaitForSeconds(timeBetweenTick);
        canTakeDamage = true;
    }



}
