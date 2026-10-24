using UnityEngine;

public class TTL : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float tempsDeVie = 5;
    float tempsInitial;

    void Start()
    {
        tempsInitial = Time.time;
    }

    // Simple système supprimant un objet après un certain temps
    void Update()
    {
        if (Time.time > tempsInitial + tempsDeVie)
        {
            Destroy(gameObject);
        }
    }
}
