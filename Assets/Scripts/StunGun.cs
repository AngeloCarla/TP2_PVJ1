using UnityEngine;

public class StunGun : MonoBehaviour
{
    [Header("Aturdidor")]
    [SerializeField] private float stunDuration = 5f; // Duracion del aturdidor

    [Header("Puntos por aturdir")]
    [SerializeField] private int stealthPoints = 30; // Puntos fuera de la deteccion
    [SerializeField] private int detectedPoints = 20; // Puntos dentro de la deteccion
    [SerializeField] private int attackPoints = 5; // Puntos dentro del ataque

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void TryStun(EnemyAI enemy)
    {
        if (enemy == null || enemy.IsStunned) return;

        if (enemy.HasAwardedStunPoints) // No se otorgan puntos mas de una vez
        {
            enemy.Stun(stunDuration);
            Debug.Log("<color=red>Sin puntos extra</color>");
            return;
        }

        float distance = Vector3.Distance(transform.position, enemy.transform.position); // Distancia del jugador al enemigo

        int points;

        if (enemy.CurrentState == EnemyAI.AIState.Attack) // Al entrar en ataque
        {
            points = attackPoints;
        }
        else if (distance > enemy.DetectionRadius) // Al estar lejos del enemigo
        {
            points = stealthPoints;
        }
        else // Al ser detectado
        {
            points = detectedPoints;
        }

        enemy.Stun(stunDuration); // Aturde al enemigo

        enemy.MarkStunPointsAwarded(); // Ya se dieron los puntos

        if (gameManager != null)
        {
            gameManager.AddScore(points); // Suma puntos
        }

        Debug.Log($"<color=#FFD700>¡Guardia aturdido! +{points} puntos</color>");
    }
}
