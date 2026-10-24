using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class homingSkull : MonoBehaviour
{
    GameObject[] players;
    [SerializeField] private float speed = 12f;
    [SerializeField] float damage = 30f;
    [SerializeField] float timeBeforeMove = 1.5f;
    bool canMove = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(DelayBeforeMove());
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //regarde la target et la follow
            transform.LookAt(GetClosestPlayer().transform);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        

    }

    //damage
    public void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, Element.None, null);
            Destroy(gameObject);
        }
    }

    //petit delai avent que la bullet bouge
    private IEnumerator DelayBeforeMove()
    {
        yield return new WaitForSeconds(timeBeforeMove);
        canMove = true;
    }

    //retourne le player le plus proche 
    private GameObject GetClosestPlayer()
    {
        GameObject player = players[0];
        for (int i = 0; i < players.Length; i++)
            if (players[i] != null)
            {
                if (Vector3.Distance(players[i].transform.position, transform.position) < Vector3.Distance(player.transform.position, transform.position))
                    player = players[i];
            }
        return player;
    }
}
