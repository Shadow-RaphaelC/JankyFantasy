using UnityEngine;

public class rockSummon : MonoBehaviour
{

    [SerializeField] float riseSpeed = 8f;
    [SerializeField] float riseDistance = 1.6f;
    [SerializeField] float timeBeforeRise = 3f;
    [SerializeField] float damage = 30f;
    [SerializeField] ParticleSystem dust;
    [SerializeField] AudioClip son;
    AudioSource audioSource;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isRising = false;
    private float timer = 0f;
    private bool alreadyHit = false;
    
    

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.up * riseDistance;
        audioSource = GetComponent<AudioSource>();
       
    }

    void Update()
    {
        timer += Time.deltaTime;

        
        if (timer >= timeBeforeRise && !isRising)
        {
            audioSource.PlayOneShot(son);
            isRising = true;
            //dust fx
            if (dust != null)
                dust.Play();
        }

        //fait monter la roche 
        if (isRising)
            transform.position = Vector3.MoveTowards(transform.position,targetPos,riseSpeed * Time.deltaTime);
        
    }

    public void OnTriggerEnter(Collider other)
    {
        //peux pas hit plusieurs fois
        if (!alreadyHit)
        {
            alreadyHit = true;
            IDamageable objet = other.gameObject.GetComponent<IDamageable>();
            if(objet!=null)
                objet.TakeDamage(damage, Element.None, null);
        }
    }
}