using UnityEngine;

public enum Team { Players, Enemies }
public enum Element { None, Fire, Ice, Lightning }

[RequireComponent(typeof(Rigidbody))]
public abstract class Combattant : MonoBehaviour, IDamageable
{
    [Header("Base Stats")]
    [SerializeField] protected float maxHP = 100f;
    [SerializeField] protected Team team;

    [Header("Movement (tank-style)")]
    [SerializeField] protected float moveSpeed = 12f;
    [SerializeField] protected float rotationSpeed = 260f;

    [Header("FX")]
    [SerializeField] protected ParticleSystem bloodSplash;

    [Header("Sounds")]
    [SerializeField] protected AudioClip attackSound;
    [SerializeField] protected AudioClip attackHitSound;
    [SerializeField] protected AudioClip playerHitSound;

    protected Rigidbody rb;
    protected AudioSource audioSource;

    protected Animator animator;
    protected float forwardInput;
    protected float rotationInput;

    public float CurrentHP { get; protected set; }
    public float MaxHP => maxHP;
    public Team Team => team;
    public bool IsDead { get; protected set; }

    protected float speedModifier = 1f;

    protected virtual void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        CurrentHP = maxHP;
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }
       

    // Deplacement
    public virtual void SetMovementInput(float forward, float rotation)
    {
        if (IsDead)
        {
            forwardInput = 0f;
            rotationInput = 0f;
            return;
        }


        // Avance et recule (W/S)
        forwardInput = forward;

        // Rotation en tank (A/D)
        rotationInput = rotation;
    }

    protected virtual void Update()
    {
        HandleMovement();
    }

    protected virtual void HandleMovement()
    {
        if (IsDead) return;

        // Rotation tank (pivote sur place).
        float rotation = rotationInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);

        // Avance/recule dans la direction regardée.
        Vector3 step = transform.forward * forwardInput * moveSpeed * speedModifier * Time.deltaTime;
        transform.position += step;

        if (animator != null)
        {
            animator.SetFloat("MoveX", rotationInput);
            animator.SetFloat("MoveY", forwardInput);
        }
    }

    // Degats
    public virtual void TakeDamage(float amount, Element element, Combattant Enemy, AudioClip audio = null)
    {
        if (IsDead) return;

        float finalDamage = IncomingDamage(amount, element);
        CurrentHP -= finalDamage;

        if (bloodSplash != null && finalDamage > 0)
        {
            bloodSplash.Play();
            audioSource.PlayOneShot(playerHitSound);
        }

        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            Die();
        }
    }

    protected virtual float IncomingDamage(float amount, Element element)
    {
        // Par defaut, les elements n'ont pas d'effet general
        return amount;
    }

    protected virtual void Die()
    {
        IsDead = true;
        forwardInput = 0f;
        rotationInput = 0f;
    }
}
