using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private GameObject vfx;

    [SerializeField] private float maxHealth = 100f; // Maximo de vida
    private float damage; // Daño
    private float currentHealth; // Vida
    public float CurrentHealth => currentHealth;
    private bool isDead; // Estado de muerte
    public bool IsDead => isDead;

    private float blinkSpeed = 0.1f; // Velocidad del parpadeo
    private float blinkDuration = 0.5f; // Duracion del parpadeo

    private Renderer rend;
    private bool isFlashing;

    void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
        rend = GetComponent<Renderer>();
    }

    public void TakeDamage(float amount)
    {
        damage = amount;
        if (isDead || amount <= 0f) return;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (gameObject.CompareTag("Player"))
        {
            Debug.Log($"<color=#FF3434>-{damage} de vida</color>");
            Debug.Log($"<color=#FF3434>Tienes {currentHealth}% de vida</color>");
        }
        if (gameObject.CompareTag("Guard"))
        {
            Debug.Log($"<color=#7E00EC>-{damage} de vida al enemigo</color>");
        }

        if (currentHealth <= 0f)
        {
            Dead();
        }
        else
        {
            FlashDamage(); // Efecto de daño
        }
    }

    private void FlashDamage()
    {
        if (!isFlashing)
        {
            isFlashing = true;
            InvokeRepeating("ToggleVisibility", 0f, blinkSpeed);
        }

        // Reiniciamos el efecto
        CancelInvoke("StopFlashing");
        Invoke("StopFlashing", blinkDuration);
    }

    private void ToggleVisibility()
    {
        rend.enabled = !rend.enabled;
    }

    private void StopFlashing()
    {
        CancelInvoke("ToggleVisibility");

        rend.enabled = true;
        isFlashing = false;
    }

    public void Dead()
    {
        if (isDead) return;
        isDead = true;

        vfx.SetActive(true);
        StartCoroutine(DestroyTransition());
    }
    IEnumerator DestroyTransition()
    {
        rend.enabled = false;
        yield return new WaitForSeconds(2f);

        if (gameObject.CompareTag("Player"))
        {

            if (GameManager.Instance != null)
            {
                GameManager.Instance.NotifyPlayerDead();
            }
        }
        else if (gameObject.CompareTag("Guard"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.NotifyEnemyDefeat();
            }
            Destroy(this.gameObject);
        }
    }
}
