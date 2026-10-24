using UnityEngine;
using UnityEngine.UI;


public class Boss : MonoBehaviour, IDamageable
{
    [SerializeField] float maxHp = 100;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] Slider hpBar;
    public float currentHp;
    AudioSource audioSource;
    public float CurrentHP => currentHp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
        audioSource = GetComponent<AudioSource>();
        hpBar.maxValue = maxHp;
        hpBar.value = currentHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float amount, Element element, Combattant enemy, AudioClip audio = null)
    {
        if(audio != null)
            audioSource.PlayOneShot(audio);
        currentHp -= amount;
        if (hitEffect != null)
            hitEffect.Play();

        if (currentHp < 0) currentHp = 0;
        hpBar.value = currentHp;

    }
}
