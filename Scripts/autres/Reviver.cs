using UnityEngine;

[RequireComponent(typeof(Character))]
public class Reviver : MonoBehaviour
{
    [Header("Reanimation")]
    [SerializeField] private float reviveTime = 3f;
    [SerializeField] private float reviveRadius = 1.3f;

    private Character self;
    private Character currentTarget;
    private float progress;

    [SerializeField] protected ParticleSystem HealSpark;

    public float ReviveProgress => currentTarget != null ? progress / reviveTime : 0f;

    private void Awake()
    {
        HealSpark.Stop();
        self = GetComponent<Character>();
    }

    private void Update()
    {
        if (self.IsDowned || self.IsDead)
        {
            ResetRevive();
            return;
        }

        Character downed = FindDownedAlly();

        if (downed == null)
        {
            ResetRevive();
            return;
        }

        if (downed != currentTarget)
        {
            currentTarget = downed;
            progress = 0f;

            //fait jouer le fx
            if (HealSpark != null && !HealSpark.isPlaying)
                HealSpark.Play();
        }

        progress += Time.deltaTime;
        //pregresse le heal
        if (progress >= reviveTime)
        {
            currentTarget.ReviveDowned();
            ResetRevive();
        }
    }

    private Character FindDownedAlly()
    {
        // https://docs.unity3d.com/ScriptReference/Physics.OverlapSphere.html
        Collider[] hits = Physics.OverlapSphere(transform.position, reviveRadius);
        foreach (Collider hit in hits)
        {
            //retourne un ally downed
            Character character = hit.GetComponent<Character>();
            if (character != null && character != self && character.IsDowned)
                return character;
        }
        return null;
    }

    //reset le heal
    private void ResetRevive()
    {
        //stop le fx
        if (HealSpark != null && HealSpark.isPlaying)
            HealSpark.Stop();

        currentTarget = null;
        progress = 0f;
    }
}
