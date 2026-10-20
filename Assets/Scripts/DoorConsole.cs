using UnityEngine;
using System.Collections;

public class DoorConsole : MonoBehaviour
{
    [Header("Sistema de la puerta")]
    [SerializeField] private GameManager gameManager; // GameManager
    [SerializeField] private GameObject door; // puerta
    [SerializeField] private float time = 10f; // Tiempo
    private bool doorOpened; // Puerta abierta

    public void DoorActive() 
    {
        if (gameManager == null || door == null) return;
        if (doorOpened) return;

        if (gameManager.HasRequiredData()) // Si tiene los objetos requeridos
        {
            StartCoroutine(OpenedDoor());
        }
    }

    private IEnumerator OpenedDoor()
    {
        Debug.Log($"<color=#7844FF>La puerta de de la BASE SECRETA se ha abierto (se cierra en {time})</color>");
        door.SetActive(false);
        yield return new WaitForSeconds(time);
        Debug.Log("<color=#7844FF>La puerta de de la BASE SECRETA se ha cerrado</color>");
        door.SetActive(true);
    }
}
