using UnityEngine;

public class DoorConsole : MonoBehaviour
{
    [SerializeField] private GameManager gameManager; // GameManager
    [SerializeField] private GameObject door; // Puerta
    private bool doorActivated = false;

    public void DoorActivate() 
    {
        if (doorActivated) return; // Evita que se active sin cumplir la mision
        if (gameManager == null || door == null) return;

        if (gameManager.HasRequiredData()) // Si tiene los objetos requeridos
        {
            doorActivated = true; // Activa la puerta
            door.SetActive(false); // Desaparece la puerta

            Debug.Log("<color=#00FF66>¡Acceso AUTORIZADO!</color>");
        }
        else
        {
            Debug.Log("<color=#FF3434>¡Acceso DENEGADO!</color>\n" + $"<color=#FF3434>Falta: </color><color=white>{gameManager.GetCollectedData()} / {gameManager.GetRequiredData()}</color>");
        }
    }
}
