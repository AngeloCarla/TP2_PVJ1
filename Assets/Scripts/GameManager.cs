using UnityEngine;
using UnityEngine.Rendering;
public enum GameState { Start, Playing, Paused, Victory, Defeat }

public class GameManager : MonoBehaviour
{
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

        Time.timeScale = 1f;
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
                if (Input.GetKeyDown(KeyCode.P)) Debug.Log($"<color=#FFD700>Puntos: {score}</color>");
                break;
            case GameState.Paused:
                if (Input.GetKeyDown(KeyCode.Escape)) ChangeState(GameState.Playing);
                break;
            case GameState.Victory:
                // Gano
                break;
            case GameState.Defeat:
                // Pedio
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
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (currentState == GameState.Paused)
        {
            if (health != null)
                Debug.Log("<color=purple>¡Juego pausado!</color>");
                Debug.Log("" +
                    $"<color=#00FF5C>| Vida: {health.CurrentHealth} |</color>" + "" +
                    $"<color=#007566>| Datos: {collectedData} / {requiredData} |</color>" + "" +
                    $"<color=#FFE86F>| Puntos: {score} |</color>" +
                    "");
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
        Debug.Log($"<color=#FFD700>Datos: </color><color=white>{collectedData} / {requiredData}</color>");

        Debug.Log($"<color=#FFD700>Puntos: </color><color=white>{score}</color>");
    }
}
