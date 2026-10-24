using UnityEngine;

public class bossBullet : MonoBehaviour
{
   
    [SerializeField] private float speed = 10f;
    [SerializeField] float damage = 30f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position += transform.forward * speed * Time.deltaTime;

    }


    public void OnTriggerEnter(Collider other)
    {
        IDamageable objet = other.GetComponent<IDamageable>();
        if (objet != null)
        {
            objet.TakeDamage(damage, Element.None, null);
            Destroy(gameObject);
        }
    }

   
}
