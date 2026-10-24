using UnityEngine;
using System.Collections;

public class DoorConsole : MonoBehaviour
{
    [Header("Sistema de la puerta")]
    [SerializeField] private GameObject door; // puerta
    [SerializeField] private float time; // Tiempo
    [SerializeField] private bool mision;
    private bool doorOpened;

    public void DoorActive()
    {
        if (doorOpened) return;
        if (mision)
        {
            if (GameManager.Instance == null || !GameManager.Instance.HasRequiredData()) return;
        }

        StartCoroutine(OpenedDoor());
    }

    private IEnumerator OpenedDoor()
    {
        doorOpened = true;
        if (mision) Debug.Log($"<color=#7844FF>La puerta de de la BASE SECRETA se ha abierto (se cierra en {time}s)</color>");
        door.SetActive(false);
        yield return new WaitForSeconds(time);
        if (mision) Debug.Log("<color=#7844FF>La puerta de de la BASE SECRETA se ha cerrado</color>");
        door.SetActive(true);
    }
}
