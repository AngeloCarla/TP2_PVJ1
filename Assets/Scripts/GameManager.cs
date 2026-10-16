using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Vida y puntos")]
    [SerializeField] private int playerHealth; // Vida del jugador
    [SerializeField] private int score; // Puntos

    [Header("Mision")]
    [SerializeField] private int collectedData; // Objetos recolectados
    [SerializeField] private int requiredData = 5; // Total de objetos recolectados

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Test
        {
            Debug.Log($"<color=#FFD700>Puntos: </color><color=white>{score}</color>");
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

    public bool HasRequiredData()
    {
        return collectedData >= requiredData; // Datos necesarios
    }

    public int GetCollectedData()
    {
        return collectedData;
    }

    public int GetRequiredData()
    {
        return requiredData;
    }
}
