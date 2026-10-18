using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    [Header("Laser")]
    [SerializeField] private float damage = 10f; // Daño
    [SerializeField] private float blinkTimeInit;
    [SerializeField] private float blinkTime;

    private Collider col;
    private Renderer rer;
    private bool isActive = true;

    void Start()
    {
        col = GetComponent<Collider>();
        rer = GetComponent<Renderer>();
        InvokeRepeating("ToggleLaser", blinkTimeInit, blinkTime);
    }

    private void ToggleLaser()
    {
        isActive = !isActive;
        col.enabled = isActive;
        rer.enabled = isActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (!other.gameObject.CompareTag("Player")) return;

        HealthSystem health = other.gameObject.GetComponent<HealthSystem>();

        if (health == null) return;

        health.TakeDamage(damage); // Daña al jugador
    }
}
