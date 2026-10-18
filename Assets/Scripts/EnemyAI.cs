using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Test
    [SerializeField] private float damage = 20f;
    private void OnCollisionEnter(Collision collision)
    {
        HealthSystem health = collision.gameObject.GetComponent<HealthSystem>();

        if (health == null) return;

        health.TakeDamage(damage); // Daña al jugador
    }
}
