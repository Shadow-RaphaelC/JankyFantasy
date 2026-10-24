using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private AudioClip son;

    private float damage;
    private Element element;
    private Combattant tireur;

    public void Setup(float damage, Element element, Combattant tireur)
    {
        this.damage = damage;
        this.element = element;
        this.tireur = tireur;

        Destroy(gameObject, lifeTime); //détruit la balle après un certain temps
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (other.GetComponent<Combattant>() == tireur)
            return; // damage pas le tireur

        // Inspiré des combos élémentaires de jeux comme "Genshin Impact" et "Magicka"  
        if (element == Element.Fire || element == Element.Ice || element == Element.Lightning)
        {
            ElementMark mark = other.GetComponent<ElementMark>();
            if (mark == null)
                mark = other.gameObject.AddComponent<ElementMark>(); // ajoute la marque d'element

            if (mark.HasMark && mark.MarkedElement != element)
            {
                float multiplier = ElementalCombo.GetMultiplier(mark.MarkedElement, element);
                damage *= multiplier;
                mark.ClearMark(); // clear la marque après le combo
            }
            else
            {
                mark.SetMark(element); // set la marque d'element
            }
        }
        target.TakeDamage(damage, element, tireur, son);
        Destroy(gameObject);
    }
}
