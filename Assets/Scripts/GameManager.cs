using UnityEngine;
using UnityEngine.Rendering;

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
    [SerializeField] private int requiredData = 5; // Total de objetos recolectados

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

    void Start()
    {
        health = FindAnyObjectByType<HealthSystem>();

        //Time.timeScale = 1f;
        //Debug.Log($"<color=purple>El cubo rojo parte 2</color>");
    }

    void Update()
    {
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
            case GameState.Victory:
                // Gano
                break;
            case GameState.Defeat:
                health.Dead();
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
                $"<color=#FF3434> Vida: <color=white>{health.CurrentHealth} </color></color>" +
                $"<color=cyan> Datos: <color=white>{collectedData} / {requiredData} </color></color>" +
                $"<color=yellow> Puntos: <color=white>{score}</color></color>\n" +
                $"<color=purple>----- ESTADO DE LA MISION -----</color>"
                );
        }
    }

    public void CollectData(int points)
    {
        if (collectedData >= requiredData) // Si llega a los datos requeridos
        {
            Debug.Log("Ya recogiste todos los datos");
            return;
        }

        collectedData++; // Suma datos
        score += points; // Suma puntos

        // Debug
        Debug.Log($"<color=cyan>Datos: </color><color=white>{collectedData} / {requiredData}</color>");

        Debug.Log($"<color=#FFD700>Puntos: </color><color=white>{score}</color>");
    }
}
