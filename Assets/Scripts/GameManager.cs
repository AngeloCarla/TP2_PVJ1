using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Start, Playing, Paused, Victory, Defeat }

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
        Time.timeScale = 0f;
    }

    void Start()
    {
        health = FindAnyObjectByType<HealthSystem>();
        Debug.Log($"<color=white>Presiona <color=purple>ENTER <color=white>para empezar</color></color></color>");
        Debug.Log(
            $"<color=purple>WASD/Flechas <color=white>para caminar, </color></color>" +
            $"<color=purple>ESPACIO <color=white>para saltar y </color></color>" +
            $"<color=purple>E <color=white>para interactuar</color></color>");
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

    public void ShowStatus()
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
        Debug.Log("<color=red>¡MISIÓN FALLIDA! DERROTA.</color>");
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log($"<color=#FFD700>Puntos: </color><color=white>{score}</color>");
    }
}
