using UnityEngine;
public enum GameState { Start, Playing, Paused, Victory, Defeat }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Estados del juego")]
    [SerializeField] private GameState currentState = GameState.Start;
    public GameState CurrentState => currentState;

    [Header("Vida y puntos")]
    private HealthSystem health; // Vida del jugador
    [SerializeField] private int score; // Puntos

    [Header("Mision")]
    [SerializeField] private int collectedData; // Objetos recolectados
    [SerializeField] private int requiredData; // Total de objetos recolectados
    private bool openedDoor;

    public int GetCollectedData()
    {
        return collectedData;
    }
    public int GetRequiredData()
    {
        return requiredData;
    }
    public bool HasRequiredData()
    {
        return collectedData >= requiredData; // Datos necesarios
    }
    private void Awake()
    {
        // Guardamos una referencia al GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Time.timeScale = 0f;
    }

    void Start()
    {
        health = FindAnyObjectByType<HealthSystem>();
        Debug.Log($"<color=white>Presiona <color=purple>ENTER <color=white>para empezar</color></color></color>");
        Debug.Log(
            $"<color=purple>WASD/Flechas <color=white>para caminar, </color></color>" +
            $"<color=purple>ESPACIO <color=white>para saltar </color></color>" +
            $"<color=purple>E <color=white>para interactuar y </color></color>" +
            $"<color=purple>Q <color=white>Dispara</color></color>");
        ShowStatus();
    }

    void Update()
    {
        if (collectedData >= requiredData && !openedDoor)
        {
            OpenDoor();
            openedDoor = true;
        }

        switch (currentState)
        {
            case GameState.Start:
                if (Input.GetKeyDown(KeyCode.Return)) ChangeState(GameState.Playing);
                break;
            case GameState.Playing:
                if (Input.GetKeyDown(KeyCode.Escape)) ChangeState(GameState.Paused);
                break;
            case GameState.Paused:
                if (Input.GetKeyDown(KeyCode.Escape)) ChangeState(GameState.Playing);
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        if (currentState == GameState.Start || currentState == GameState.Paused || currentState == GameState.Victory || currentState == GameState.Defeat)
        {
            Time.timeScale = 0f;
            ShowStatus();
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void ShowStatus() // Da la info por consola
    {
        if (health != null)
        {
            Debug.Log(
                $"<color=#FF3434> Vida: <color=white>{health.CurrentHealth}% </color></color>" +
                $"<color=cyan> Fragmentos: <color=white>{collectedData} / {requiredData} </color></color>" +
                $"<color=yellow> Puntos: <color=white>{score}</color></color>\n" +
                $"<color=purple>----- ESTADO DE LA MISION -----</color>"
                );
        }
    }

    public void CollectData(int points)
    {
        if (collectedData >= requiredData) // Si llega a los datos requeridos
        {
            return;
        }

        collectedData++; // Suma datos
        score += points; // Suma puntos

        // Debug
        Debug.Log($"<color=cyan>Datos: </color><color=white>{collectedData} / {requiredData}</color>");

        Debug.Log($"<color=#FFD700>Puntos: </color><color=white>{score}</color>");
    }

    public void OpenDoor()
    {
        DoorConsole door = FindFirstObjectByType<DoorConsole>();
        if (door == null) return;
        door.DoorActive();
    }
    public void Win()
    {
        ChangeState(GameState.Victory);
        Debug.Log("<color=#00FF66>¡MISIÓN COMPLETADA! ¡VICTORIA!</color>");
    }

    public void Lose()
    {
        ChangeState(GameState.Defeat);
        Debug.Log("<color=red>¡MISIÓN FALLIDA! DERROTA</color>");
    }
    //notificaciones
    public void NotifyPlayerDead()
    {
        if (currentState != GameState.Playing) return;

        ChangeState(GameState.Defeat);
    }

    public void NotifyEnemyDefeat()
    {
        if (currentState != GameState.Playing) return;
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log($"<color=#FFD700>Puntos: </color><color=white>{score}</color>");
    }
}
