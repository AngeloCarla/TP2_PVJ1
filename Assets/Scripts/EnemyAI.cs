using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum AIState { Patrol, Chase, Attack }

    [Header("Estados / Test")]
    [SerializeField] private AIState currentState = AIState.Patrol; // Inicia en Patrullaje
    public AIState CurrentState => currentState;

    [Header("Radio de deteccion")]
    [SerializeField] private float detectionRadius; // Distancia a la que detecta
    public float DetectionRadius => detectionRadius;
    [SerializeField] private float attackRadius; // Distancia a la que comienza le ataque

    [Header("Patrullaje")]
    [SerializeField] private Transform[] patrolPoints; // Lista de puntos de patrullaje
    [SerializeField] private float waitTimeAtPoint = 2f; // Tiempo de espera
    private NavMeshAgent agent; // Agentes
    private int currentPatrolIndex; // Punto actual
    private bool isWaitingAtPoint; // Esta esperando

    [Header("Ataque")]
    [SerializeField] private float attackCooldown = 1.5f; // Tiempo de espera de ataque
    [SerializeField] private float damage; // Daño
    private Transform playerTransform; // Jugador
    private HealthSystem healthSystemPlayer; // Vida
    private bool isAttacking = false; // Esta atacando

    [Header("Aturdimiento")]
    [SerializeField] private Color stunnedColor = Color.black; // Color aturdido
    private Renderer enemyRenderer;
    private Color originalColor;
    private bool isStunned; // Aturdido
    public bool IsStunned => isStunned;
    private bool hasAwardedStunPoints; // Ya se dio puntos
    public bool HasAwardedStunPoints => hasAwardedStunPoints;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color;
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); // Jugador
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            healthSystemPlayer = playerObj.GetComponent<HealthSystem>();
        }

        if (patrolPoints.Length > 0) // Comprueba si hay puntos de patrullaje asignados
        {
            GoToNextPatrolPoint();
        }
    }

    private void Update()
    {
        // si esta aturdido, el jugador es null o el jugador esta muerto
        if (isStunned || playerTransform == null || (healthSystemPlayer != null && healthSystemPlayer.IsDead))
        {
            agent.isStopped = true; // Se detiene
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position); // Calcula la distancia del jugador con el enemigo

        switch (currentState)
        {
            case AIState.Patrol: // Patrullaje
                UpdatePatrolState(distanceToPlayer);
                break;

            case AIState.Chase: // Persecucion
                UpdateChaseState(distanceToPlayer);
                break;

            case AIState.Attack: // Ataque
                UpdateAttackState(distanceToPlayer);
                break;
        }
    }

    private void UpdatePatrolState(float distanceToPlayer)
    {
        agent.isStopped = false;

        if (distanceToPlayer <= detectionRadius) // Si el jugador entra en el radio de detección
        {
            currentState = AIState.Chase; // Pasa a persecucion
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaitingAtPoint)
            {
                StartCoroutine(WaitAndMoveToNextPoint()); // Si no esta esperando, espera y pasa al otro punto
            }
        }
    }

    private void UpdateChaseState(float distanceToPlayer)
    {
        agent.isStopped = false;
        agent.SetDestination(playerTransform.position); // Sigue al jugador

        if (distanceToPlayer <= attackRadius) // si pasa al radio de ataque
        {
            currentState = AIState.Attack; // Pasa a atacar
            return;
        }

        // Si el jugador se escapa del radio de deteccion, vuelve a patrullar
        if (distanceToPlayer > detectionRadius * 1.2f)
        {
            currentState = AIState.Patrol;
            GoToNextPatrolPoint();
        }
    }

    private void UpdateAttackState(float distanceToPlayer)
    {
        // Hace que el enemigo mire hacia el jugador
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));

        if (distanceToPlayer > attackRadius && !isAttacking) // Si no ataca y el jugador pasa la zona de ataque
        {
            currentState = AIState.Chase; // Vuelve a persecucion
            return;
        }
        if (!isAttacking)
        {
            StartCoroutine(AttackCooldown()); // Espera unos segundos
        }
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        agent.isStopped = true;
        ExecuteAttack(); // Ataca
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        agent.isStopped = false;
    }

    private void ExecuteAttack()
    {
        if (healthSystemPlayer != null)
        {
            healthSystemPlayer.TakeDamage(damage); // Hace daño
        }
    }

    private IEnumerator WaitAndMoveToNextPoint() // Espera y mueve al siguiente punto
    {
        isWaitingAtPoint = true; // Esperando
        yield return new WaitForSeconds(waitTimeAtPoint);

        if (patrolPoints.Length > 0)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; //2%3
            GoToNextPatrolPoint();
        }

        isWaitingAtPoint = false;
    }

    private void GoToNextPatrolPoint() // Avanza al siguiente punto de patrullaje
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position); // Destino, el siguiente punto de la lista
    }

    private void OnDrawGizmosSelected()
    {
        // Radio de deteccion (Amarillo)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Radio de ataque (Rojo)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void Stun(float duration) // Aturdimiento
    {
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        // Aturdidto
        isStunned = true;
        agent.isStopped = true;
        ChangeEnemyColor(true);
        Debug.Log($"<color=#FF0082>Aturdido por {duration} segundos</color>");

        yield return new WaitForSeconds(duration);

        // Se recupera
        isStunned = false;
        agent.isStopped = false;
        ChangeEnemyColor(false);
        Debug.Log("<color=#FF0082>Recuperado del aturdimiento</color>");
    }

    private void ChangeEnemyColor(bool stunned)
    {
        enemyRenderer.material.color = stunned ? stunnedColor : originalColor;
    }
    public void MarkStunPointsAwarded()
    {
        hasAwardedStunPoints = true;
    }

}
