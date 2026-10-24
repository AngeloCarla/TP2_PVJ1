using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Daño")]
    [SerializeField] private float damage;

    [Header("Movimiento")]
    [SerializeField] private float speed; // Velocidad
    private Vector3 dir = Vector3.forward; // Se mueve en Z, hacia adelante

    void Update()
    {
        // --- MOVIMIENTO ---
        Vector3 z = dir.normalized * speed * Time.deltaTime;
        transform.Translate(z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // --- DAÑO ---
        if (collision.gameObject.CompareTag("Guard"))
        {
            // Buscamos al enemigo en el objeto impactado o en sus padres
            HealthSystem health =
                collision.collider.GetComponentInParent<HealthSystem>();

            EnemyAI enemy =
                collision.collider.GetComponentInParent<EnemyAI>();

            // Si encontramos un enemigo con sistema de vida, hacemos daño
            if (enemy != null && health != null)
            {
                health.TakeDamage(damage);
            }
        }

        // La bala desaparece al impactar con cualquier objeto
        Destroy(gameObject);
    }
}

