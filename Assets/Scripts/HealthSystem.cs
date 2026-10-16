using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float maxHealth = 100f; // Maximo de vida
    private float currentHealth; // Vida
    public float CurrentHealth => currentHealth;
    private bool isDead; // Estado de muerte
    public bool IsDead => isDead;

    void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    public void TakeDamage(float amount)
    {
        if (isDead || amount <= 0f) return;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"<color=cyan>Vida: {currentHealth}</color>");

        if (currentHealth <= 0f) Dead();
    }

    private void Dead()
    {
        isDead = true;
        Debug.Log($"<color=red>Moriste por pendejo</color>");
    }
}
